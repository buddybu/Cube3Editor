using DevAge.Drawing;
using SourceGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cube3Editor
{
    class BitFromByte
    {
        private Byte[] byteArray;
        private Encoding encoding;
        private List<String> bfbStringList;
        private RectangleBorder _border;

        private Dictionary<int, Retraction> retractionDictionary = new Dictionary<int, Retraction>();
        private Dictionary<string, List<int>> retractionLineList = new Dictionary<string, List<int>>();


        private Dictionary<int, List<int>> tempLineList = new Dictionary<int, List<int>>();
        private List<BFBTemps> indexTemps = new List<BFBTemps>();

        public List<string> BfbStringList
        {
            get => bfbStringList;
            set => bfbStringList = value;
        }

        public BitFromByte(Encoding encoding, Byte[] byteArray)
        {
            this.byteArray = byteArray;
            this.encoding = encoding;

            string decodedModel = encoding.GetString(byteArray);

            string[] seperator = new string[] { "\r", "\n" };
            string[] decodedModelArray = decodedModel.Split(seperator, StringSplitOptions.RemoveEmptyEntries);

            BfbStringList = decodedModelArray.ToList<string>();

            int index = BfbStringList.FindLastIndex(x => x.StartsWith("M107"));

            if (0 <= index && index <= BfbStringList.Count)
            {
                char[] bob = BfbStringList[index + 1].ToArray();

                if (bob[0] != '^' && bob[0] != '#' && bob[0] != 'M' && bob[0] != 'G')
                {
                    try
                    {
                        int startIndex = index + 1;
                        int count = bfbStringList.Count - startIndex;
                        bfbStringList.RemoveRange(startIndex, count);
                    }
                    catch (Exception ex)
                    {
                        string test = ex.Message;
                    }
                }
            }
        }

        internal Byte[] getBytesFromBFB()
        {
            String bfbString = String.Join("\r\n", BfbStringList);

            Byte[] bytes = encoding.GetBytes(bfbString);

            return bytes;
        }


        internal string GetText(string control)
        {
            int index = BfbStringList.FindIndex(x => x.Contains(control));
            if (index >= 0)
            {
                return BfbStringList[index].Split(':')[1];
            }
            else
            {
                return "";
            }
        }

        internal string GetMaterialType(string material)
        {
            int index = BfbStringList.FindIndex(x => x.Contains(material));
            string materialCodeStr = BfbStringList[index].Split(':')[1];
            int materialCode = Convert.ToInt32(materialCodeStr);

            if (materialCode != -1)
            {
                return BFBConstants.GetMaterialType(materialCode);
            }
            else
            {
                return "";

            }
        }

        internal string GetMaterialColor(string material)
        {
            int index = BfbStringList.FindIndex(x => x.Contains(material));
            string materialCodeStr = BfbStringList[index].Split(':')[1];
            int materialCode = Convert.ToInt32(materialCodeStr);

            if (materialCode != -1)
            {
                return BFBConstants.GetMaterialColor(materialCode);
            }
            else
            {
                return "";

            }
        }


        internal void SetFIRMWARE(string firmwareVersion)
        {
            int index = BfbStringList.FindIndex(x => x.Contains(BFBConstants.FIRMWARE));
            if (0 <= index && index < BfbStringList.Count)
            {
                string[] firmwareStrArr = BfbStringList[index].Split(':');
                BfbStringList[index] = String.Join(":", firmwareStrArr[0], firmwareVersion);
            }

            // ^Firmware: entry not present, need to add as first entry
            else
            {
                string firmwareCmd = BFBConstants.FIRMWARE + firmwareVersion;
                BfbStringList.Insert(0, firmwareCmd);
            }
        }

        internal void SetMINFIRMWARE(string minFirmwareVersion)
        {
            int index = BfbStringList.FindIndex(x => x.Contains(BFBConstants.MINFIRMWARE));
            if (0 <= index && index < BfbStringList.Count)
            {
                string[] strArr = BfbStringList[index].Split(':');
                BfbStringList[index] = String.Join(":", strArr[0], minFirmwareVersion);
            }

            // ^MinFirmware: entry not present, need to add as first entry
            else
            {
                string minFirmwareCmd = BFBConstants.MINFIRMWARE + minFirmwareVersion;
                BfbStringList.Insert(0, minFirmwareCmd);
            }
        }

        internal void SetPRINTERMODEL(string printerModel)
        {
            int index = BfbStringList.FindIndex(x => x.Contains(BFBConstants.PRINTERMODEL));
            if (0 <= index && index < BfbStringList.Count)
            {
                string[] strArr = BfbStringList[index].Split(':');
                BfbStringList[index] = String.Join(":", strArr[0], printerModel);
            }

            // ^PrinterMode: entry not present, need to add as first entry
            else
            {
                string printModelCmd = BFBConstants.PRINTERMODEL + printerModel;
                BfbStringList.Insert(0, printModelCmd);
            }
        }

        internal void SetMATERIALCODE(string materialCode, string type, string color)
        {
            int cubeCode = BFBConstants.getCUBE3Code(type, color);

            if (cubeCode != -1)
            {
                int index = BfbStringList.FindIndex(x => x.Contains(materialCode));
                if (0 <= index && index <= bfbStringList.Count)
                {
                    string[] materialStrArr = BfbStringList[index].Split(':');
                    BfbStringList[index] = String.Join(":", materialStrArr[0], cubeCode.ToString());
                }
                else
                {
                    string materialCmd = materialCode + ":" + cubeCode.ToString();
                    BfbStringList.Insert(0, materialCmd);
                }
            }
        }


        internal List<string> getUniqueRetractions(string bfbRetractionCode)
        {
            List<string> retractionss = new List<string>();

            for (int i = 0; i < BfbStringList.Count; i++)
            {
                if (BfbStringList[i].StartsWith(bfbRetractionCode))
                {

                    retractionss.Add(BfbStringList[i]);
                    Retraction retraction = new Retraction(BfbStringList[i], i);
                    retractionDictionary.Add(i, retraction);

                    if (!retractionLineList.Keys.Contains(bfbStringList[i]))
                    {
                        retractionLineList.Add(bfbStringList[i], new List<int>());
                    }
                    retractionLineList[bfbStringList[i]].Add(i);
                }
            }
            return retractionss;

        }

        internal List<string> getUniqueTemperatures(string bfbTemperatureCode)
        {
            List<string> temps = new List<string>();

            for (int i = 0; i < BfbStringList.Count; i++)
            {
                if (BfbStringList[i].StartsWith(bfbTemperatureCode))
                {
                    temps.Add(BfbStringList[i]);
                    int temperature = GetTemperatureFromString(BfbStringList[i]);
                    
                    if (!tempLineList.Keys.Contains(temperature))
                    {
                        tempLineList.Add(temperature, new List<int>(i));
                    }
                    tempLineList[temperature].Add(i);
                }
            }
            return temps;
        }

        internal int GetTemperatureFromString(string temperatureStr)
        {
            string splitTemp = temperatureStr.Split(' ')[1];
            string cvtTempStr = new string(splitTemp.ToCharArray(1, splitTemp.Length - 1));
            int temperature = Convert.ToInt32(cvtTempStr);
            return temperature;
        }
        internal void PopulateTemperatures(string tempCmdStr, Grid gridTemps, RectangleBorder border)
        {
            _border = border;

            Dictionary<int, int> tempDict = new Dictionary<int, int>();

            List<string> tempLines = getUniqueTemperatures(tempCmdStr);

            for (int i = 0; i < tempLines.Count; i++)
            {
                string tempStr = tempLines[i].Split(' ')[1];
                string cvtTempStr = new string(tempStr.ToCharArray(1, tempStr.Length - 1));
                int temperature = Convert.ToInt32(cvtTempStr);
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
            String[] tempModType = new String[] { "Percentage", "Hard", "Value" };
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

        internal void PopulateRetractions(string retractCmd, Grid gridRetraction, RectangleBorder border)
        {
            _border = border;

            List<string> retractionLines = getUniqueRetractions(retractCmd);

            SourceGrid.Cells.Editors.TextBox retractCountEditor = new SourceGrid.Cells.Editors.TextBox(typeof(int));
            retractCountEditor.EditableMode = SourceGrid.EditableMode.None;


            int gridRow = 1;
            RetractionChangedEvent valueChangedController = new RetractionChangedEvent(this);


            foreach (string key in retractionLineList.Keys)
            {
                int index = retractionLineList[key][0];
                Retraction retraction = retractionDictionary[index];


                gridRetraction.Rows.Insert(gridRow);
                gridRetraction[gridRow, 0] = new SourceGrid.Cells.Cell(retraction.P, typeof(int));
                gridRetraction[gridRow, 0].AddController(valueChangedController);
                gridRetraction[gridRow, 1] = new SourceGrid.Cells.Cell(retraction.S, typeof(int));
                gridRetraction[gridRow, 1].AddController(valueChangedController);
                if (retraction.G != -1)
                {
                    gridRetraction[gridRow, 2] = new SourceGrid.Cells.Cell(retraction.G, typeof(int));
                    gridRetraction[gridRow, 2].AddController(valueChangedController);
                }
                else
                {
                    gridRetraction[gridRow, 2] = new SourceGrid.Cells.Cell(" ", retractCountEditor);
                }
                if (retraction.F != -1)
                {
                    gridRetraction[gridRow, 3] = new SourceGrid.Cells.Cell(retraction.F, typeof(int));
                    gridRetraction[gridRow, 3].AddController(valueChangedController);
                }
                else
                {
                    gridRetraction[gridRow, 3] = new SourceGrid.Cells.Cell(" ", retractCountEditor);
                }
                gridRetraction[gridRow, 4] = new SourceGrid.Cells.Cell(index, retractCountEditor);

                //gridLeftTemps[gridRow, 1] = new SourceGrid.Cells.CellControl(); 
                gridRow++;
            }
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
                    else if (gridTemps[i, 3].Value.Equals("Hard"))
                    {
                        int hard = (int)(gridTemps[i, 2].Value);
                        newTemp = currentTemp + hard;
                    }
                    else if (gridTemps[i, 3].Value.Equals("Value"))
                    {
                        int value = (int)(gridTemps[i, 2].Value);
                        newTemp = value;
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

        internal void UpdateRetractions(Grid gridRetracts)
        {
            if (gridRetracts.Rows.Count > 1)
            {
                for (int i = 1; i < gridRetracts.Rows.Count; i++)
                {
                    int p = (int)gridRetracts[i, 0].Value;
                    int s = (int)gridRetracts[i, 1].Value;
                    int g = (int)gridRetracts[i, 2].Value;
                    int f = (int)gridRetracts[i, 3].Value;
                    int index = (int)gridRetracts[i, 4].Value;

                    string retractCmd = BFBConstants.RETRACT_START + " P" + p + " S" + s;
                    if (g != -1)
                        retractCmd += " G" + g;

                    if (f != -1)
                        retractCmd += " F" + f;

                    List<int> retractLines = retractionLineList[bfbStringList[index]];
                    retractionLineList.Remove(bfbStringList[index]);

                    foreach (int line in retractLines)
                    {
                        bfbStringList[line] = retractCmd;

                        if (!retractionLineList.Keys.Contains(retractCmd))
                        {
                            retractionLineList.Add(retractCmd, retractLines);
                        }

                        if (retractionDictionary.Keys.Contains(line))
                        {
                            retractionDictionary[line] = new Retraction(retractCmd, line);
                        }
                        else
                        {
                            retractionDictionary.Add(line, new Retraction(retractCmd, line));
                        }
                    }
                }
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
            List<int> intLines;
            List<int> newTempLineList;
            List<int> oldTempLineList;
            Dictionary<int, String> updatedBFBLines = new Dictionary<int, string>();

            for (int i = 1; i < gridTemps.Rows.Count; i++)
            {
                if ((int)gridTemps[i, 2].Value != 0)
                {
                    oldTemp = (int)gridTemps[i, 0].Value;
                    newTemp = (int)gridTemps[i, 4].Value;
                    intLines = new List<int>(tempLineList[oldTemp]);

                    // if there is no change in temperature, move on....
                    if (oldTemp == newTemp)
                    {
                        continue;
                    }

                    if (!tempLineList.Keys.Contains(newTemp))
                    {
                        newTempLineList = new List<int>();
                    }
                    else
                    {
                        newTempLineList = new List<int>(tempLineList[newTemp]);
                    }

                    oldTempLineList = new List<int>(tempLineList[oldTemp]);

                    // need to filter for tempCmd and only move the indices for the command to the new value
                    foreach (int index in intLines)
                    {
                        if (BfbStringList[index].StartsWith(tempCmd))
                        {
                            newTempLineList.Add(index);
                            oldTempLineList.Remove(index);
                            updatedBFBLines.Add(index, UpdateTemperaturesInBFB(index, newTemp));
                        }

                    }

                    // remove old and new temps, if present.
                    tempLineList.Remove(oldTemp);

                    // add new and old, if contains values back to line list
                    if (oldTempLineList.Count != 0)
                    {
                        tempLineList.Add(oldTemp, oldTempLineList);
                    }
                    if (!tempLineList.Keys.Contains(newTemp))
                    {
                        tempLineList.Add(newTemp, newTempLineList);
                    }
                    else
                    {
                        tempLineList.Remove(newTemp);
                        tempLineList.Add(newTemp, newTempLineList);
                    }
                }
            }

            // update the temperature lines in the BFB array
            foreach (int index in updatedBFBLines.Keys)
            {
                bfbStringList[index] = updatedBFBLines[index];
            }

            UpdateTemperatures(gridTemps);

        }

        private string UpdateTemperaturesInBFB(int index, int temperature)
        {
            string[] tempStrArray = BfbStringList[index].Split(' ');
            string newTemperature = "S" + temperature;
            tempStrArray[1] = newTemperature;

            return string.Join(" ", tempStrArray);
        }

        public class TemperatureChangedEvent : SourceGrid.Cells.Controllers.ControllerBase
        {
            private BitFromByte mBFB;
            public TemperatureChangedEvent(BitFromByte bfb)
            {
                mBFB = bfb;
            }

            public override void OnValueChanged(SourceGrid.CellContext sender, EventArgs e)
            {
                base.OnValueChanged(sender, e);
                mBFB.CalculateTemperatures((Grid)sender.Grid);
            }
        }

        public class RetractionChangedEvent : SourceGrid.Cells.Controllers.ControllerBase
        {
            private BitFromByte mBFB;
            public RetractionChangedEvent(BitFromByte bfb)
            {
                mBFB = bfb;
            }

            public override void OnValueChanged(SourceGrid.CellContext sender, EventArgs e)
            {
                base.OnValueChanged(sender, e);
                mBFB.UpdateRetractions((Grid)sender.Grid);
            }
        }

    }

}
