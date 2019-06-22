using BitForByteSupport;
using DevAge.Drawing;
using SourceGrid;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Cube3Editor
{
    public partial class MainEditor
    {

        internal void PopulateTemperatures(string tempCmdStr, Grid gridTemps, RectangleBorder border)
        {
            Dictionary<int, int> tempDict = new Dictionary<int, int>();

            List<string> tempLines = bfbObject.getTemperatures(tempCmdStr);

            for (int i = 0; i < tempLines.Count; i++)
            {
                int temperature = bfbObject.GetTemperatureFromString(tempLines[i]);
                if (tempDict.ContainsKey(temperature))
                {
                    tempDict[temperature]++;
                }
                else
                {
                    tempDict.Add(temperature, 1);
                }
            }

            SourceGrid.Cells.Editors.ComboBox tempModEditor;
            String[] tempModType = new String[] { "Percentage", "Additive", "Replace" };
            tempModEditor = new SourceGrid.Cells.Editors.ComboBox(typeof(String));
            tempModEditor.StandardValues = tempModType;
            tempModEditor.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.SingleClick | SourceGrid.EditableMode.AnyKey;

            SourceGrid.Cells.Editors.TextBox tempCountEditor = new SourceGrid.Cells.Editors.TextBox(typeof(int));
            tempCountEditor.EditableMode = SourceGrid.EditableMode.None;

            int gridRow = 1;
            TemperatureChangedEvent valueChangedController = new TemperatureChangedEvent(this);

            foreach (int temp in tempDict.Keys)
            {
                gridTemps.Rows.Insert(gridRow);
                gridTemps[gridRow, 0] = new SourceGrid.Cells.Cell(temp, tempCountEditor);
                gridTemps[gridRow, 1] = new SourceGrid.Cells.Cell(tempDict[temp], tempCountEditor);
                gridTemps[gridRow, 2] = new SourceGrid.Cells.Cell(0, typeof(int));
                gridTemps[gridRow, 2].AddController(valueChangedController);
                gridTemps[gridRow, 3] = new SourceGrid.Cells.Cell("Percentage", tempModEditor);
                gridTemps[gridRow, 3].View = SourceGrid.Cells.Views.ComboBox.Default;
                gridTemps[gridRow, 3].AddController(valueChangedController);
                gridTemps[gridRow, 4] = new SourceGrid.Cells.Cell(0, tempCountEditor);

                //gridLeftTemps[gridRow, 1] = new SourceGrid.Cells.CellControl(); 
                gridRow++;
            }
        }

        internal void UpdateTemperatures(Grid gridTemps)
        {
            for (int i = 1; i < gridTemps.Rows.Count; i++)
            {
                if ((int)gridTemps[i, 2].Value != 0)
                {
                    gridTemps[i, 0].Value = gridTemps[i, 4].Value;
                    gridTemps[i, 4].Value = 0;
                }
            }
        }

        internal void UpdateTemperatures(string tempCmd, Grid gridTemps)
        {

            int oldTemp;
            int newTemp;

            for (int i = 1; i < gridTemps.Rows.Count; i++)
            {
                if ((int)gridTemps[i, 2].Value != 0)
                {

                    oldTemp = (int)gridTemps[i, 0].Value;
                    newTemp = (int)gridTemps[i, 4].Value;

                    if (oldTemp != newTemp)
                    {
                        bfbObject.UpdateTemperatureLines(tempCmd, oldTemp, newTemp);
                    }
                }
            }

            UpdateTemperatures(gridTemps);

        }


        internal void CalculateTemperatures(Grid gridTemps)
        {
            if (gridTemps.Rows.Count > 1)
            {
                for (int i = 1; i < gridTemps.Rows.Count; i++)
                {
                    int currentTemp = (int)gridTemps[i, 0].Value;
                    int newTemp = 0;

                    if (gridTemps[i, 3].Value.Equals("Percentage"))
                    {
                        double percentage = (int)(gridTemps[i, 2].Value);
                        if (currentTemp > 0 && percentage != 0)
                        {
                            newTemp = Convert.ToInt32((percentage / 100) * currentTemp + currentTemp);
                        }
                    }
                    else if (gridTemps[i, 3].Value.Equals("Additive"))
                    {
                        int additive = (int)(gridTemps[i, 2].Value);
                        newTemp = currentTemp + additive;
                    }
                    else if (gridTemps[i, 3].Value.Equals("Replace"))
                    {
                        int replace = (int)(gridTemps[i, 2].Value);
                        newTemp = replace;
                    }
                    else
                    {
                        newTemp = currentTemp;
                    }

                    if (newTemp < 0)
                        newTemp = 0;

                    if (newTemp > 265)
                        newTemp = 265;

                    gridTemps[i, 4].Value = newTemp;
                }
            }
        }
    }
}