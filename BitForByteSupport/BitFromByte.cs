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

        public BitFromByte(Encoding encoding, byte[] byteArray)
        {
            this.byteArray = byteArray;
            this.encoding = encoding;

            string[] modelLines = encoding.GetString(byteArray).
                Split(new string[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            bfbStringList = modelLines.ToList();

            generateRetractionStartList();
            generateRetractionStopList();

            RemoveImage();
        }

        public  void RemoveImage()
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
                            bfbStringList.RemoveRange(startIndex, count);
                        }
                        catch (Exception ex)
                        {
                            string test = ex.Message;
                        }
                    }
                }
            }
        }

        public  byte[] getBytesFromBFB()
        {
            string bfbString = string.Join("\r\n", BfbLines);

            byte[] bytes = encoding.GetBytes(bfbString);

            return bytes;
        }


        public  string GetText(string control)
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

        public  string GetMaterialType(string material)
        {
            int index = BfbLines.FindIndex(x => x.Contains(material));
            string materialCodeStr = BfbLines[index].Split(':')[1];
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

        public  string GetMaterialColor(string material)
        {
            int index = BfbLines.FindIndex(x => x.Contains(material));
            string materialCodeStr = BfbLines[index].Split(':')[1];
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


        public  void SetFIRMWARE(string firmwareVersion)
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

        public  void SetMINFIRMWARE(string minFirmwareVersion)
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

        public  void SetPRINTERMODEL(string printerModel)
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

        public  List<string> generateRetractionStartList()
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
            List<int> uniqueRetractions = new List<int>();
            foreach (int i in RetractionStartDictionary.Keys)
            {
                if (BfbLines[i].StartsWith(retractCmd))
                {
                    if (!uniqueRetractions.Contains(RetractionStartDictionary[i].P))
                    {
                        if (RetractionStartDictionary[i].P == RetractionStartDictionary[i].S && RetractionStartDictionary[i].P > 1)
                        {
                            uniqueRetractions.Add(RetractionStartDictionary[i].P);
                        }
                    }
                }
            }

            return uniqueRetractions;
        }

        public  List<string> generateRetractionStopList()
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

        public  List<string> getTemperatures(string bfbTemperatureCode)
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
            return  uniqueTemps;
        }

        public int GetTemperatureFromString(string temperatureStr)
        {
            string splitTemp = temperatureStr.Split(' ')[1];
            string cvtTempStr = new string(splitTemp.ToCharArray(1, splitTemp.Length - 1));
            int temperature = Convert.ToInt32(cvtTempStr);
            return temperature;
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
