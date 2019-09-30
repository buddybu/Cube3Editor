using BitForByteSupport;
using System;

namespace Cube3Editor
{
    public partial class MainEditor
    {
        internal void PopulatePressures()
        {
            SourceGrid.Cells.Editors.TextBox pressureEditor = new SourceGrid.Cells.Editors.TextBox(typeof(Double));
            pressureEditor.EditableMode = SourceGrid.EditableMode.None;


            int gridRow = 1;
            PressureChangedEvent valueChangedController = new PressureChangedEvent(this);

            foreach (String keyLine in bfbObject.PressureLineList.Keys)
            {
                int index = bfbObject.PressureLineList[keyLine][0];
                Double pressure = bfbObject.PressureDictionary[index];

                gridPressure.Rows.Insert(gridRow);
                gridPressure[gridRow, 0] = new SourceGrid.Cells.CheckBox("", false);
                gridPressure[gridRow, 1] = new SourceGrid.Cells.Cell(pressure, typeof(double));
                gridPressure[gridRow, 1].AddController(valueChangedController);
                gridPressure[gridRow, 2] = new SourceGrid.Cells.Cell(0, typeof(double));
                gridPressure[gridRow, 3] = new SourceGrid.Cells.Cell(index, pressureEditor);

                gridRow++;
            }
        }

        internal void UpdatePressures(bool updateGrid = false)
        {
            if (gridPressure.Rows.Count > 1)
            {
                for (int i = 1; i < gridPressure.Rows.Count; i++)
                {
                    Double pressure = (Double)gridPressure[i, 1].Value;
                    int index = (int)gridPressure[i, 3].Value;

                    string pressureCmd = BFBConstants.EXTRUDER_PRESSURE + " S" + pressure;

                    bfbObject.updatePressureLines(index, pressureCmd);

                }
            }

            if (updateGrid)
            {
                if (gridPressure.Rows.Count > 1)
                    gridPressure.Rows.RemoveRange(1, gridPressure.Rows.Count - 1);
                bfbObject.RebuildPressures();
                PopulatePressures();
            }
        }

        internal void CalculatePressures(Double pressureChangeValue, Boolean updatePressures)
        {
            if (gridPressure.Rows.Count > 1)
            {
                if (pressureChangeValue > 0.0 || pressureChangeValue < 0.0)
                {
                    for (int i = 1; i < gridPressure.Rows.Count; i++)
                    {
                        if ((bool)(gridPressure[i, 0].Value) == true)
                        {
                            Double currentPressure = (Double)gridPressure[i, 1].Value;

                            Double pressureModifier = Convert.ToDouble(String.Format("{0:F2}", (pressureChangeValue / 100.0) * currentPressure));
                            Double newPressure = Convert.ToDouble(String.Format("{0:F2}", currentPressure + pressureModifier));

                            // don't allow negative pressures
                            if (newPressure < 0.0)
                            {
                                newPressure = 0.0;
                            }

                            gridPressure[i, 2].Value = newPressure;
                            if (updatePressures)
                            {
                                gridPressure[i, 1].Value = newPressure;
                            }

                        }
                    }

                    if (updatePressures)
                    {
                        UpdatePressures(true);
                    }
                }
            }
        }

    }
}