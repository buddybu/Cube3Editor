using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitForByteSupport
{
    public class BitFromByte
    {
        private byte[] byteArray;
        private Encoding encoding;
        private List<string> bfbStringList;

        private Dictionary<int, Retraction> retractionStartDictionary = new Dictionary<int, Retraction>();
        private Dictionary<string, List<int>> retractionStartLineList = new Dictionary<string, List<int>>();

        private Dictionary<int, Retraction> retractionStopDictionary = new Dictionary<int, Retraction>();
        private Dictionary<string, List<int>> retractionStopLineList = new Dictionary<string, List<int>>();

        private Dictionary<int, List<int>> tempLineList = new Dictionary<int, List<int>>();

        private Dictionary<int, Double> pressureDictionary = new Dictionary<int, Double>();
        private Dictionary<string, List<int>> pressureLineList = new Dictionary<string, List<int>>();

        public List<string> BfbLines
        {
            get => bfbStringList;
        }

        public Dictionary<int, Retraction> RetractionStartDictionary
        {
            get => retractionStartDictionary;
        }

        public Dictionary<int, Retraction> RetractionStopDictionary
        {
            get => retractionStopDictionary;
        }

        public Dictionary<string, List<int>> RetractionStartLineList { get => retractionStartLineList; set => retractionStartLineList = value; }
        public Dictionary<string, List<int>> RetractionStopLineList { get => retractionStopLineList; set => retractionStopLineList = value; }
        public Dictionary<int, List<int>> TempLineList { get => tempLineList; set => tempLineList = value; }
        public Dictionary<string, List<int>> PressureLineList { get => pressureLineList; set => pressureLineList = value; }
        public Dictionary<int, double> PressureDictionary { get => pressureDictionary; set => pressureDictionary = value; }

        public BitFromByte(Encoding encoding, byte[] byteArray)
        {
            this.byteArray = byteArray;
            this.encoding = encoding;

            string[] modelLines = encoding.GetString(byteArray).
                Split(new string[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            bfbStringList = modelLines.ToList();

            generateRetractionStartList();
            generateRetractionStopList();
            getExtruderPressures(BFBConstants.EXTRUDER_PRESSURE);

            RemoveImage();
        }

        public void RemoveImage()
        {
            // find last entry of M107
            int m107Index = BfbLines.FindLastIndex(x => x.StartsWith("M107"));

            if (0 <= m107Index && m107Index < BfbLines.Count)
            {
                // only do this if the M107 is not at the end of the file....
                if (m107Index + 1 < BfbLines.Count)
                {
                    char[] imageLine = BfbLines[m107Index + 1].ToArray();

                    if (imageLine[0] != '^' && imageLine[0] != '#' && imageLine[0] != 'M' && imageLine[0] != 'G')
                    {
                        try
                        {
                            int startIndex = m107Index + 1;
                            int count = bfbStringList.Count - startIndex;
                            BfbLines.RemoveRange(startIndex, count);

                            BfbLines[m107Index] = BfbLines[m107Index].Substring(0, 4);
                        }
                        catch (Exception ex)
                        {
                            string test = ex.Message;
                        }
                    }
                }
            }
        }

        public byte[] GetBytesFromBFB()
        {
            string bfbString = string.Join("\r\n", BfbLines);

            byte[] bytes = encoding.GetBytes(bfbString);

            return bytes;
        }


        public string GetText(string control)
        {
            int index = BfbLines.FindIndex(x => x.Contains(control));
            if (index >= 0)
            {
                return BfbLines[index].Split(':')[1];
            }
            else
            {
                return "";
            }
        }

        public string GetMaterialType(string material)
        {
            int index = BfbLines.FindIndex(x => x.Contains(material));
            if (index != -1)
            {
                string materialCodeStr = BfbLines[index].Split(':')[1];
                int materialCode = Convert.ToInt32(materialCodeStr);
                return BFBConstants.GetMaterialType(materialCode);
            }
            else
            {
                return BFBConstants.EMPTY;

            }
        }

        public string GetMaterialColor(string material)
        {
            int index = BfbLines.FindIndex(x => x.Contains(material));
            if (index != -1)
            {
                string materialCodeStr = BfbLines[index].Split(':')[1];
                int materialCode = Convert.ToInt32(materialCodeStr);

                return BFBConstants.GetMaterialColor(materialCode);
            }
            else
            {
                return BFBConstants.EMPTY;
            }
        }


        public void SetFIRMWARE(string firmwareVersion)
        {
            int index = BfbLines.FindIndex(x => x.Contains(BFBConstants.FIRMWARE));
            if (0 <= index && index < BfbLines.Count)
            {
                string[] firmwareStrArr = BfbLines[index].Split(':');
                BfbLines[index] = string.Join(":", firmwareStrArr[0], firmwareVersion);
            }

            // ^Firmware: entry not present, need to add as first entry
            else
            {
                string firmwareCmd = BFBConstants.FIRMWARE + firmwareVersion;
                BfbLines.Insert(0, firmwareCmd);
            }
        }

        public void SetMINFIRMWARE(string minFirmwareVersion)
        {
            int index = BfbLines.FindIndex(x => x.Contains(BFBConstants.MINFIRMWARE));
            if (0 <= index && index < BfbLines.Count)
            {
                string[] strArr = BfbLines[index].Split(':');
                BfbLines[index] = string.Join(":", strArr[0], minFirmwareVersion);
            }

            // ^MinFirmware: entry not present, need to add as first entry
            else
            {
                string minFirmwareCmd = BFBConstants.MINFIRMWARE + minFirmwareVersion;
                BfbLines.Insert(0, minFirmwareCmd);
            }
        }

        public void SetPRINTERMODEL(string printerModel)
        {
            int index = BfbLines.FindIndex(x => x.Contains(BFBConstants.PRINTERMODEL));
            if (0 <= index && index < BfbLines.Count)
            {
                string[] strArr = BfbLines[index].Split(':');
                BfbLines[index] = string.Join(":", strArr[0], printerModel);
            }

            // ^PrinterMode: entry not present, need to add as first entry
            else
            {
                string printModelCmd = BFBConstants.PRINTERMODEL + printerModel;
                BfbLines.Insert(0, printModelCmd);
            }
        }

        public void SetMATERIALCODE(string materialCode, string type, string color)
        {
            int cubeCode = BFBConstants.getCUBE3Code(type, color);

            SetMATERIALCODE(materialCode, cubeCode);
        }

        public void SetMATERIALCODE(string materialCode, int cubeCode)
        {
            if (cubeCode != -1)
            {
                int index = BfbLines.FindIndex(x => x.Contains(materialCode));
                if (0 <= index && index <= bfbStringList.Count)
                {
                    string[] materialStrArr = BfbLines[index].Split(':');
                    BfbLines[index] = string.Join(":", materialStrArr[0], cubeCode.ToString());
                }
                else
                {
                    string materialCmd = materialCode + ":" + cubeCode.ToString();
                    BfbLines.Insert(0, materialCmd);
                }
            }
        }

        public void SetSIDEWALKS(string newSidewalk)
        {
            int index = BfbLines.FindIndex(x => x.Contains(BFBConstants.SIDEWALKS));
            if (0 <= index && index < BfbLines.Count)
            {
                string[] sidewalkStrArr = BfbLines[index].Split(':');
                BfbLines[index] = string.Join(":", sidewalkStrArr[0], newSidewalk);
            }

            // ^Sidewalks: entry not present, need to add as first entry
            else
            {
                string sidewalksCmd = BFBConstants.SIDEWALKS + newSidewalk;
                BfbLines.Insert(0, sidewalksCmd);
            }
        }

        public void SetSUPPORTS(string newSupport)
        {
            int index = BfbLines.FindIndex(x => x.Contains(BFBConstants.SUPPORTS));
            if (0 <= index && index < BfbLines.Count)
            {
                string[] supportStrArr = BfbLines[index].Split(':');
                BfbLines[index] = string.Join(":", supportStrArr[0], newSupport);
            }

            // ^Sidewalks: entry not present, need to add as first entry
            else
            {
                string supportsCmd = BFBConstants.SUPPORTS + newSupport;
                BfbLines.Insert(0, supportsCmd);
            }
        }

        public int GetMATERIALCODE(string materialCode)
        {
            int index = BfbLines.FindIndex(x => x.Contains(materialCode));
            int code = -1;

            if (0 <= index && index <= bfbStringList.Count)
            {
                string[] materialStrArr = BfbLines[index].Split(':');
                code = Convert.ToInt32(materialStrArr[1]);
            }

            return code;
        }

        public string GetSIDEWALKS()
        {
            int index = BfbLines.FindIndex(x => x.Contains(BFBConstants.SIDEWALKS));
            string sidewalk = null;

            if (0 <= index && index <= bfbStringList.Count)
            {
                string[] sidewalksStrArr = BfbLines[index].Split(':');
                sidewalk = sidewalksStrArr[1];
            }

            return sidewalk;
        }

        public string GetSUPPORTS()
        {
            int index = BfbLines.FindIndex(x => x.Contains(BFBConstants.SUPPORTS));
            string supports = null;

            if (0 <= index && index <= bfbStringList.Count)
            {
                string[] supportsStrArr = BfbLines[index].Split(':');
                supports = supportsStrArr[1];
            }

            return supports;
        }

        public void RebuildPressures()
        {
            // clear dictionary
            // clear line list
            // repopulate dictionary and linelist

            PressureDictionary.Clear();
            PressureLineList.Clear();

            getExtruderPressures(BFBConstants.EXTRUDER_PRESSURE);


        }
        public List<string> getExtruderPressures(string bfbPressureCode)
        {
            List<string> pressures = new List<string>();

            for (int i = 0; i < BfbLines.Count; i++)
            {
                if (BfbLines[i].StartsWith(bfbPressureCode))
                {

                    FixPressureLine(i);
                    Double pressure = GetPressureFromString(BfbLines[i]);
                    pressures.Add(BfbLines[i]);
                    if (!PressureDictionary.Keys.Contains(i))
                    {
                        PressureDictionary.Add(i, pressure);
                    }

                    if (!pressureLineList.Keys.Contains(BfbLines[i]))
                    {
                        pressureLineList.Add(BfbLines[i], new List<int>(i));
                    }
                    pressureLineList[BfbLines[i]].Add(i);
                }
            }
            return pressures;
        }

        public List<Double> GetUniquePressures(string pressureString)
        {

            List<int> keys;

            keys = PressureDictionary.Keys.ToList();

            List<Double> uniquePressures = new List<Double>();
            foreach (int i in keys)
            {
                if (BfbLines[i].StartsWith(pressureString))
                {
                    if (!uniquePressures.Contains(PressureDictionary[i]))
                    {
                        uniquePressures.Add(PressureDictionary[i]);
                    }
                }
            }

            return uniquePressures;
        }
        public double GetPressureFromString(string pressureStr)
        {
            string splitPressure = pressureStr.Split(' ')[1];
            string cvtPressureStr = new string(splitPressure.ToCharArray(1, splitPressure.Length - 1));
            double pressure = Convert.ToDouble(cvtPressureStr);
            return pressure;
        }

        private void FixPressureLine(int index)
        {
            string[] splitPressure = BfbLines[index].Split(' ');

            if (splitPressure.Length == 1)
            {
                splitPressure = BfbLines[index].Split('S');
                if (splitPressure.Length == 2)
                {
                    BfbLines[index] = splitPressure[0] + " S" + splitPressure[1];
                }
            }
        }


        public List<string> generateRetractionStartList()
        {
            List<string> retractions = new List<string>();

            for (int i = 0; i < BfbLines.Count; i++)
            {
                if (BfbLines[i].StartsWith(BFBConstants.RETRACT_START))
                {

                    retractions.Add(BfbLines[i]);
                    Retraction retraction = new Retraction(BfbLines[i], i);
                    RetractionStartDictionary.Add(i, retraction);

                    if (!RetractionStartLineList.Keys.Contains(bfbStringList[i]))
                    {
                        RetractionStartLineList.Add(bfbStringList[i], new List<int>());
                    }
                    RetractionStartLineList[bfbStringList[i]].Add(i);
                }
            }
            return retractions;

        }

        public List<int> GetUniqueRetractions(string retractCmd)
        {
            Boolean retractStop = false;
            bool addRetract = false;
            int retractVal = -1;

            List<int> keys;

            if (retractCmd.Equals(BFBConstants.RETRACT_STOP))
                retractStop = true;

            if (!retractStop)
            {
                keys = RetractionStartDictionary.Keys.ToList();
            }
            else
            {
                keys = RetractionStopDictionary.Keys.ToList();
            }

            List<int> uniqueRetractions = new List<int>();
            foreach (int i in keys)
            {
                addRetract = false;
                if (BfbLines[i].StartsWith(retractCmd))
                {

                    // retraction start
                    if (!retractStop)
                    {
                        if (!uniqueRetractions.Contains(RetractionStartDictionary[i].P))
                        {
                            if (RetractionStartDictionary[i].P == RetractionStartDictionary[i].S &&
                                RetractionStartDictionary[i].P > 1)
                            {
                                retractVal = retractionStartDictionary[i].P;
                                addRetract = true;
                            }
                        }
                    }

                    // retraction stop
                    else
                    {
                        if (!uniqueRetractions.Contains(RetractionStopDictionary[i].S))
                        {
                            if (RetractionStopDictionary[i].P == 0 && RetractionStopDictionary[i].S > 1)
                            {
                                retractVal = retractionStopDictionary[i].S;
                                addRetract = true;
                            }
                        }
                    }

                    if (addRetract)
                    {
                        uniqueRetractions.Add(retractVal);
                    }
                }
            }

            return uniqueRetractions;
        }

        public List<string> generateRetractionStopList()
        {
            List<string> retractionss = new List<string>();

            for (int i = 0; i < BfbLines.Count; i++)
            {
                if (BfbLines[i].StartsWith(BFBConstants.RETRACT_STOP))
                {

                    retractionss.Add(BfbLines[i]);
                    Retraction retraction = new Retraction(BfbLines[i], i);
                    RetractionStopDictionary.Add(i, retraction);

                    if (!RetractionStopLineList.Keys.Contains(bfbStringList[i]))
                    {
                        RetractionStopLineList.Add(bfbStringList[i], new List<int>());
                    }
                    RetractionStopLineList[bfbStringList[i]].Add(i);
                }
            }
            return retractionss;

        }

        public List<string> getTemperatures(string bfbTemperatureCode)
        {
            List<string> temps = new List<string>();

            for (int i = 0; i < BfbLines.Count; i++)
            {
                if (BfbLines[i].StartsWith(bfbTemperatureCode))
                {
                    temps.Add(BfbLines[i]);
                    int temperature = GetTemperatureFromString(BfbLines[i]);

                    if (!tempLineList.Keys.Contains(temperature))
                    {
                        tempLineList.Add(temperature, new List<int>(i));
                    }
                    tempLineList[temperature].Add(i);
                }
            }
            return temps;
        }

        public List<int> getUniqueTemperatures(string tempString)
        {
            List<int> uniqueTemps = new List<int>();

            foreach (int temp in tempLineList.Keys)
            {
                foreach (int line in tempLineList[temp])
                {
                    if (BfbLines[line].StartsWith(tempString))
                    {
                        if (!uniqueTemps.Contains(temp))
                        {
                            uniqueTemps.Add(temp);
                        }
                    }
                }
            }
            return uniqueTemps;
        }

        public int GetTemperatureFromString(string temperatureStr)
        {
            string splitTemp = temperatureStr.Split(' ')[1];
            string cvtTempStr = new string(splitTemp.ToCharArray(1, splitTemp.Length - 1));
            int temperature = Convert.ToInt32(cvtTempStr);
            return temperature;
        }

        public void updatePressureLines(int index, string pressureCmd)
        {
            if (PressureLineList.Keys.Contains(bfbStringList[index]))
            {
                List<int> pressureLines = PressureLineList[bfbStringList[index]];
                PressureLineList.Remove(bfbStringList[index]);

                List<int> newRetractLines = new List<int>();

                foreach (int line in pressureLines)
                {
                    bfbStringList[line] = pressureCmd;

                    if (!PressureLineList.Keys.Contains(pressureCmd))
                    {
                        PressureLineList.Add(pressureCmd, pressureLines);
                    }

                    if (PressureDictionary.Keys.Contains(line))
                    {
                        PressureDictionary[line] = GetPressureFromString(pressureCmd);
                    }
                    else
                    {
                        PressureDictionary.Add(line, GetPressureFromString(pressureCmd));
                    }
                }
            }

        }

        public void updateRetractionStartLines(int index, string retractCmd)
        {
            List<int> retractLines = RetractionStartLineList[bfbStringList[index]];
            RetractionStartLineList.Remove(bfbStringList[index]);

            Dictionary<int, string> updatedBFBLines = new Dictionary<int, string>();


            foreach (int line in retractLines)
            {
                bfbStringList[line] = retractCmd;

                if (!RetractionStartLineList.Keys.Contains(retractCmd))
                {
                    RetractionStartLineList.Add(retractCmd, retractLines);
                }

                if (RetractionStartDictionary.Keys.Contains(line))
                {
                    RetractionStartDictionary[line] = new Retraction(retractCmd, line);
                }
                else
                {
                    RetractionStartDictionary.Add(line, new Retraction(retractCmd, line));
                }
            }
        }

        public void updateRetractionStopLines(int index, string retractCmd)
        {
            List<int> retractLines = RetractionStopLineList[bfbStringList[index]];

            List<int> newRetractLines = new List<int>();

            RetractionStopLineList.Remove(bfbStringList[index]);


            foreach (int line in retractLines)
            {
                bfbStringList[line] = retractCmd;

                if (!RetractionStopLineList.Keys.Contains(retractCmd))
                {
                    RetractionStopLineList.Add(retractCmd, retractLines);
                }

                if (RetractionStopDictionary.Keys.Contains(line))
                {
                    RetractionStopDictionary[line] = new Retraction(retractCmd, line);
                }
                else
                {
                    RetractionStopDictionary.Add(line, new Retraction(retractCmd, line));
                }
            }
        }

        public void UpdateTemperatureLines(string tempCmd, int oldTemp, int newTemp)
        {
            List<int> intLines;
            List<int> newTempLineList;
            List<int> oldTempLineList;
            Dictionary<int, string> updatedBFBLines = new Dictionary<int, string>();

            intLines = new List<int>(tempLineList[oldTemp]);

            // if there is no change in temperature, move on....
            if (oldTemp == newTemp)
            {
                return;
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
                if (BfbLines[index].StartsWith(tempCmd))
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

            // update the temperature lines in the BFB array
            foreach (int index in updatedBFBLines.Keys)
            {
                bfbStringList[index] = updatedBFBLines[index];
            }


        }

        public string UpdateTemperaturesInBFB(int index, int temperature)
        {
            string[] tempStrArray = BfbLines[index].Split(' ');
            string newTemperature = "S" + temperature;
            tempStrArray[1] = newTemperature;

            return string.Join(" ", tempStrArray);
        }

    }

}
