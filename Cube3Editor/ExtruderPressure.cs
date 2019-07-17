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
                gridPressure[gridRow, 0] = new SourceGrid.Cells.Cell(pressure, typeof(double));
                gridPressure[gridRow, 0].AddController(valueChangedController);
                gridPressure[gridRow, 1] = new SourceGrid.Cells.Cell(index, pressureEditor);

                gridRow++;
            }
        }

        internal void UpdatePressures()
        {
            if (gridPressure.Rows.Count > 1)
            {
                for (int i = 1; i < gridPressure.Rows.Count; i++)
                {
                    Double pressure = (Double)gridPressure[i, 0].Value;
                    int index = (int)gridPressure[i, 1].Value;

                    string pressureCmd = BFBConstants.EXTRUDER_PRESSURE + " S" + pressure;

                    bfbObject.updatePressureLines(index, pressureCmd);

                }
            }
        }


    }
}