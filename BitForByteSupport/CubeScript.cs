using System;
using System.Collections.Generic;
using static BitForByteSupport.TemperatureType;

namespace BitForByteSupport
{
    public class CubeScript
    {
        private List<String> bfbStringList;
        private BitFromByte bfbObject;
        private string firmwareStr;
        private string minFirmwareStr;
        private string printerModelStr;
        private string materialE1Str;
        private string materialE2Str;
        private string materialE3Str;
        private List<int> uniqueLeftTemps;
        private List<int> uniqueRightTemps;
        private List<int> uniqueMidTemps;

        private List<int> uniqueRetractStarts;
        private List<int> uniqueRetractStops;
        private List<Double> uniqueExtruderPressure;

        private List<TemperatureModifier> temperatureModifers;
        private List<RetractModifier> retractStartModifers;
        private List<RetractModifier> retractStopModifers;
        private List<PressureModifier> pressureModifiers;

        private List<String> cubeScriptLines;

        public CubeScript()
        {
            // initialize lists
            CubeScriptLines = new List<string>();
            temperatureModifers = new List<TemperatureModifier>();
            retractStartModifers = new List<RetractModifier>();
            retractStopModifers = new List<RetractModifier>();
            pressureModifiers = new List<PressureModifier>();
        }

        public CubeScript(string cubeScriptFile) : this()
        {
            LoadAndValidateScript(cubeScriptFile);
        }

        public CubeScript(string[] newScriptLines) : this()
        {
            LoadAndValidateScript(newScriptLines);
        }

        public CubeScript(BitFromByte bfbObject) : this()
        {
            BfbObject = bfbObject;

            List<String> bfbStringList = bfbObject.BfbLines;

            FirmwareStr = bfbObject.GetText(BFBConstants.FIRMWARE);
            MinFirmwareStr = bfbObject.GetText(BFBConstants.MINFIRMWARE);
            PrinterModelStr = bfbObject.GetText(BFBConstants.PRINTERMODEL);
            MaterialE1Str = bfbObject.GetText(BFBConstants.MATERIALCODEE1);
            MaterialE2Str = bfbObject.GetText(BFBConstants.MATERIALCODEE2);
            MaterialE3Str = bfbObject.GetText(BFBConstants.MATERIALCODEE3);

            bfbObject.getTemperatures(BFBConstants.LEFT_TEMP);
            bfbObject.getTemperatures(BFBConstants.RIGHT_TEMP);
            bfbObject.getTemperatures(BFBConstants.MID_TEMP);

            UniqueLeftTemps = bfbObject.getUniqueTemperatures(BFBConstants.LEFT_TEMP);
            UniqueRightTemps = bfbObject.getUniqueTemperatures(BFBConstants.RIGHT_TEMP);
            UniqueMidTemps = bfbObject.getUniqueTemperatures(BFBConstants.MID_TEMP);

            UniqueRetractStarts = bfbObject.GetUniqueRetractions(BFBConstants.RETRACT_START);
            UniqueRetractStops = bfbObject.GetUniqueRetractions(BFBConstants.RETRACT_STOP);

            bfbObject.getExtruderPressures(BFBConstants.EXTRUDER_PRESSURE);
            UniqueExtruderPressure = bfbObject.GetUniquePressures(BFBConstants.EXTRUDER_PRESSURE);

            CubeScriptLines = new List<string>();
            GenerateScriptLines();
        }

        private void GenerateScriptLines()
        {
            PrintSetLine("FIRMWARE", FirmwareStr);
            PrintSetLine("MINFIRMWARE", MinFirmwareStr);
            PrintSetLine("MODEL", PrinterModelStr);
            PrintSetLine("E1", MaterialE1Str);
            PrintSetLine("E2", MaterialE2Str);
            PrintSetLine("E3", MaterialE3Str);


            foreach (int temp in UniqueLeftTemps)
            {
                if (temp >= 0)
                {
                    PrintModifyTempLine("LEFT", temp);
                }
            }

            foreach (int temp in UniqueRightTemps)
            {
                if (temp >= 0)
                {
                    PrintModifyTempLine("RIGHT", temp);
                }
            }

            foreach (int temp in UniqueMidTemps)
            {
                if (temp >= 0)
                {
                    PrintModifyTempLine("MID", temp);
                }
            }

            foreach (int retract in UniqueRetractStarts)
            {
                PrintModifyRetractLine("RETRACTSTART", retract);
            }

            foreach (int retract in UniqueRetractStops)
            {
                PrintModifyRetractLine("RETRACTSTOP", retract);
            }

            foreach (Double pressure in UniqueExtruderPressure)
            {
                PrintModifyPressureLine("EXTPRESSURE", pressure);
            }


        }
        private void PrintSetLine(string command, string value)
        {
            String setLine = $"SET {command.ToUpper()} {value}";

            CubeScriptLines.Add(setLine);
        }

        private void PrintModifyTempLine(string side, int temperature)
        {
            String tempLine = $"MODIFY TEMPERATURE {side} {temperature} BY REPLACE {temperature}";
            CubeScriptLines.Add(tempLine);
        }

        private void PrintModifyRetractLine(string command, int retract)
        {
            String retractLine = $"MODIFY {command} {retract} BY REPLACE {retract}";
            CubeScriptLines.Add(retractLine);
        }

        private void PrintModifyPressureLine(string command, Double pressure)
        {
            String pressureLine = $"MODIFY {command} {pressure} BY REPLACE {pressure}";
            CubeScriptLines.Add(pressureLine);
        }

        private void LoadAndValidateScript(string scriptFile)
        {
            string line;
            int count = 0;

            try
            {
                System.IO.StreamReader file = new System.IO.StreamReader(scriptFile);
                while ((line = file.ReadLine()) != null)
                {
                    count++;
                    ProcessLine(line, count);
                    CubeScriptLines.Add(line);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to open file '" + scriptFile + "'", ex);
            }
        }

        private void LoadAndValidateScript(string[] scriptFile)
        {
            string line;
            int count = 0;

            for (int i = 0; i < scriptFile.Length; i++)
            {
                count++;
                line = scriptFile[i];
                ProcessLine(line, count);
                CubeScriptLines.Add(line);
            }
        }

        private void ProcessLine(String line, int count)
        {
            if (line.Trim().Length > 0)
            {
                string[] lineArray = line.ToUpper().Split(null);

                if (lineArray.Length == 0)
                {
                    // empty line, skip it
                    return;
                }

                if (lineArray[0].Equals("SET"))
                {
                    DoSet(lineArray, count);
                }
                else if (lineArray[0].ToUpper().Equals("MODIFY"))
                {
                    DoModify(lineArray, count);
                }

                //else if (lineArray[0].ToUpper().Equals("EXECUTE"))
                //{
                //    valid = doExecute(lineArray, count);
                //}
                else
                {
                    throw new Exception("Invalid script file at line " + count);
                }

            }

        }

        // SET <Field> <value>
        private void DoSet(string[] lineArray, int lineNumber)
        {
            if (lineArray[1].Equals("") || lineArray.Length != 3)
            {
                throw new Exception("Invalid SET line at line " + lineNumber);
            }
            else if (lineArray[1].Equals("FIRMWARE"))
            {
                firmwareStr = lineArray[2];
            }
            else if (lineArray[1].Equals("MINFIRMWARE"))
            {
                minFirmwareStr = lineArray[2];
            }
            else if (lineArray[1].Equals("MODEL"))
            {
                printerModelStr = lineArray[2];
            }
            else if (lineArray[1].Equals("E1"))
            {
                materialE1Str = lineArray[2];
            }
            else if (lineArray[1].Equals("E2"))
            {
                materialE2Str = lineArray[2];
            }
            else if (lineArray[1].Equals("E3"))
            {
                materialE3Str = lineArray[2];
            }
            else
            {
                throw new Exception("Invalid SET command at line " + lineNumber);
            }

        }

        // modify temperature [<modifier>] xxxx by <mod type> <mod value>
        // modify retractstart xxxx by <mod type> <mod value>
        // modify retractstop xxxx by <mod type> <mod value>
        private void DoModify(string[] lineArray, int lineNumber)
        {
            // MODIFY TEMPERATURE [LEFT|RIGHT|MID] XXXX BY [PERCENTAGE [+|-]YYYY|ADD [+|-]YYYY|REPLACE YYYY]
            if (lineArray[1].Equals("TEMPERATURE"))
            {
                DoModifyTemperature(lineArray, lineNumber);
            }

            // MODIFY RETRACTSTART XXXX BY [PERCENTAGE [+|-]YYYY|ADD [+|-]YYYY|REPLACE YYYY]
            else if (lineArray[1].Equals("RETRACTSTART"))
            {
                DoModifyRetractStart(lineArray, lineNumber);
            }

            // MODIFY RETRACTSTOP XXXX BY [PERCENTAGE [+|-]YYYY|ADD [+|-]YYYY|REPLACE YYYY]
            else if (lineArray[1].Equals("RETRACTSTOP"))
            {
                DoModifyRetractStop(lineArray, lineNumber);
            }
            else if (lineArray[1].Equals("EXTPRESSURE"))
            {
                DoModifyPressure(lineArray, lineNumber);
            }
            else
            {
                throw new Exception("Invalid MODIFY command at line " + lineNumber);
            }

        }

        private void DoModifyTemperature(string[] lineArray, int lineNumber)
        {
            int oldTempIndex = 2;
            int byIndex = 3;
            int modTypeIndex = 4;
            int modValueIndex = 5;
            int sideIndex = -1;
            TemperatureEnum tempType = TemperatureEnum.ALL;

            // check parameter count
            if (lineArray.Length < 6 || lineArray.Length > 7)
            {
                throw new Exception("Invalid MODIFY TEMPERATURE at line " + lineNumber);
            }
            // LEFT/RIGHT/MID specified
            else if (lineArray.Length == 7)
            {
                sideIndex = 2;
                oldTempIndex = 3;
                byIndex = 4;
                modTypeIndex = 5;
                modValueIndex = 6;
            }


            if (sideIndex > 0)
            {
                bool valid = Enum.TryParse(lineArray[sideIndex], out tempType);
                if (!valid)
                {
                    throw new Exception("Invalid MODIFY TEMPERATURE side setting at line " + lineNumber);
                }
            }

            String oldTemperature = lineArray[oldTempIndex];
            String byStr = lineArray[byIndex].ToUpper();
            String modType = lineArray[modTypeIndex].ToUpper();
            String modValueStr = lineArray[modValueIndex];
            int modValue;

            TemperatureModifier tempMod = new TemperatureModifier();

            // process OldTemperature
            if (!int.TryParse(oldTemperature, out tempMod.oldValue))
            {
                throw new Exception("Nonnumeric MODIFY TEMPERATURE old value at line " + lineNumber);
            }

            if (!int.TryParse(modValueStr, out modValue))
            {
                throw new Exception("Invalid MODIFY TEMPERATURE modification value at line " + lineNumber);
            }

            // process BY string
            if (!byStr.Equals("BY"))
            {
                throw new Exception("Invalid MODIFY TEMPERATURE at line " + lineNumber);
            }

            // process modtype string
            if (modType.Equals("PERCENTAGE"))
            {
                double percentage = modValue;
                tempMod.newValue = Convert.ToInt32(tempMod.oldValue + (percentage / 100) * tempMod.oldValue);
            }
            else if (modType.Equals("ADD"))
            {
                tempMod.newValue = tempMod.oldValue + modValue;
            }
            else if (modType.Equals("REPLACE"))
            {
                tempMod.newValue = modValue;
            }
            else
            {
                throw new Exception("Invalid MODIFY TEMPERATURE modificaiton setting at line " + lineNumber);
            }

            TemperatureModifier leftMod = null;
            TemperatureModifier rightMod = null;
            TemperatureModifier midMod = null;

            switch (tempType)
            {

                case TemperatureEnum.ALL:
                    leftMod = new TemperatureModifier();
                    rightMod = new TemperatureModifier();
                    midMod = new TemperatureModifier();
                    break;
                case TemperatureEnum.LEFT:
                    leftMod = new TemperatureModifier();
                    break;
                case TemperatureEnum.RIGHT:
                    rightMod = new TemperatureModifier();
                    break;
                case TemperatureEnum.MID:
                    midMod = new TemperatureModifier();
                    break;
                default:
                    break;
            }

            if (leftMod != null)
            {
                leftMod.extruder = BFBConstants.LEFT_TEMP;
                leftMod.oldValue = tempMod.oldValue;
                leftMod.newValue = tempMod.newValue;
                TemperatureModifers.Add(leftMod);
            }

            if (rightMod != null)
            {
                rightMod.extruder = BFBConstants.RIGHT_TEMP;
                rightMod.oldValue = tempMod.oldValue;
                rightMod.newValue = tempMod.newValue;
                TemperatureModifers.Add(rightMod);
            }

            if (midMod != null)
            {
                midMod.extruder = BFBConstants.MID_TEMP;
                midMod.oldValue = tempMod.oldValue;
                midMod.newValue = tempMod.newValue;
                TemperatureModifers.Add(midMod);
            }
        }

        // MODIFY RETRACTSTART XXXX BY [PERCENTAGE [+|-]YYYY|ADD [+|-]YYYY|REPLACE YYYY]
        // MODIFY RETRACTSTART XXXX REPLACE P-VALUE S-VALUE G-VALUE F-VALUE
        private void DoModifyRetractStart(string[] lineArray, int lineNumber)
        {
            int oldRetractIndex = 2;
            int byIndex = 3;
            int modTypeIndex = 4;
            int modValueIndex = 5;

            // check parameter count and validate line
            if (lineArray.Length != 6 && lineArray.Length != 8)
            {
                throw new Exception("Invalid MODIFY RETRACTSTART at line " + lineNumber);
            }

            if (!lineArray[byIndex].ToUpper().Equals("REPLACE"))
                ValidateRetractLine(lineArray, oldRetractIndex, byIndex, modTypeIndex, modValueIndex);
            else
            {
                DoModifyRetractStartReplace(lineArray, lineNumber);
                return;
            }

            RetractModifier retractMod = new RetractModifier();

            // process OldTemperature
            int.TryParse(lineArray[oldRetractIndex], out retractMod.oldRetractValue);
            int.TryParse(lineArray[modValueIndex], out int modValue);

            string modType = lineArray[modTypeIndex].ToUpper();
            if (modType.Equals("PERCENTAGE"))
            {
                double percentage = modValue;
                retractMod.newPValue = Convert.ToInt32(retractMod.oldRetractValue + (percentage / 100) * retractMod.oldRetractValue);
                retractMod.newSValue = retractMod.newPValue;
            }
            else if (modType.Equals("ADD"))
            {
                retractMod.newPValue = retractMod.oldRetractValue + modValue;
                retractMod.newSValue = retractMod.newPValue;
            }
            else if (modType.Equals("REPLACE"))
            {
                retractMod.newPValue = modValue;
                retractMod.newSValue = retractMod.newPValue;
            }

            retractMod.newGValue = -1;
            retractMod.newFValue = -1;

            retractMod.retractCmd = BFBConstants.RETRACT_START;

            RetractStartModifers.Add(retractMod);

        }

        private void DoModifyRetractStartReplace(string[] lineArray, int lineNumber)
        {
            int oldRetractIndex = 2;
            int replaceIndex = 3;
            int pIndex = 4;
            int sIndex = 5;
            int gIndex = 6;
            int fIndex = 7;

            String oldRetract = lineArray[oldRetractIndex].ToUpper();
            String replaceStr = lineArray[replaceIndex].ToUpper();
            String pValueStr = lineArray[pIndex].ToUpper();
            String sValueStr = lineArray[sIndex].ToUpper();
            String gValueStr = lineArray[gIndex].ToUpper();
            String fValueStr = lineArray[fIndex].ToUpper();

            int pValue;
            int sValue;
            int gValue;
            int fValue;

            // process OldTemperature
            if (!int.TryParse(oldRetract, out int oldValue))
            {
                throw new Exception($"Nonnumeric MODIFY RETRACT old value at line {lineNumber}");
            }

            // process REPLACE string
            if (!replaceStr.Equals("REPLACE"))
            {
                throw new Exception($"Invalid MODIFY RETRACT at line {lineNumber}");
            }

            // P-Value
            if (!int.TryParse(pValueStr, out pValue) ||
                !int.TryParse(sValueStr, out sValue) ||
                !int.TryParse(gValueStr, out gValue) ||
                !int.TryParse(fValueStr, out fValue))
            {
                throw new Exception($"Invalid MODIFY RETRACT modification value at line {lineNumber}");
            }



            RetractModifier retractMod = new RetractModifier();

            // process OldTemperature
            retractMod.oldRetractValue = oldValue;

            retractMod.newPValue = pValue;
            retractMod.newSValue = sValue;
            retractMod.newGValue = gValue;
            retractMod.newFValue = fValue;

            retractMod.retractCmd = BFBConstants.RETRACT_START;

            RetractStartModifers.Add(retractMod);
        }

        // MODIFY RETRACTSTOP XXXX BY [PERCENTAGE [+|-]YYYY|ADD [+|-]YYYY|REPLACE YYYY]
        private void DoModifyRetractStop(string[] lineArray, int lineNumber)
        {
            int oldRetractIndex = 2;
            int byIndex = 3;
            int modTypeIndex = 4;
            int modValueIndex = 5;

            // check parameter count
            if (lineArray.Length != 6)
            {
                throw new Exception("Invalid MODIFY RETRACTSTOP at line " + lineNumber);
            }

            ValidateRetractLine(lineArray, oldRetractIndex, byIndex, modTypeIndex, modValueIndex);

            RetractModifier retractMod = new RetractModifier();

            // process OldTemperature
            int.TryParse(lineArray[oldRetractIndex], out retractMod.oldRetractValue);
            int.TryParse(lineArray[modValueIndex], out int modValue);

            string modType = lineArray[modTypeIndex].ToUpper();
            if (modType.Equals("PERCENTAGE"))
            {
                double percentage = modValue;
                retractMod.newSValue = Convert.ToInt32(retractMod.oldRetractValue + (percentage / 100) * retractMod.oldRetractValue);
            }
            else if (modType.Equals("ADD"))
            {
                retractMod.newSValue = retractMod.oldRetractValue + modValue;
            }
            else if (modType.Equals("REPLACE"))
            {
                retractMod.newSValue = modValue;
            }

            retractMod.newPValue = -1;
            retractMod.newGValue = -1;
            retractMod.newFValue = -1;

            retractMod.retractCmd = BFBConstants.RETRACT_STOP;

            RetractStopModifers.Add(retractMod);
        }

        private void DoModifyPressure(string[] lineArray, int lineNumber)
        {
            int oldPressureIndex = 2;
            int byIndex = 3;
            int modTypeIndex = 4;
            int modValueIndex = 5;

            // check parameter count
            if (lineArray.Length != 6)
            {
                throw new Exception("Invalid MODIFY EXTPRESSURE at line " + lineNumber);
            }


            String oldTPressure = lineArray[oldPressureIndex];
            String byStr = lineArray[byIndex].ToUpper();
            String modType = lineArray[modTypeIndex].ToUpper();
            String modValueStr = lineArray[modValueIndex];
            Double modValue;

            PressureModifier pressureMod = new PressureModifier();

            // process OldPressure
            if (!Double.TryParse(oldTPressure, out pressureMod.oldPressureValue))
            {
                throw new Exception("Nonnumeric MODIFY EXTPRESSURE old value at line " + lineNumber);

            }

            if (!Double.TryParse(modValueStr, out modValue))
            {
                throw new Exception("Invalid MODIFY EXTPRESSURE modification value at line " + lineNumber);
            }

            // process BY string
            if (!byStr.Equals("BY"))
            {
                throw new Exception("Invalid MODIFY EXTPRESSURE at line " + lineNumber);
            }

            // process modtype string
            if (modType.Equals("PERCENTAGE"))
            {
                double percentage = modValue;
                pressureMod.newPressureValue = pressureMod.oldPressureValue + (percentage / 100) * pressureMod.oldPressureValue;
            }
            else if (modType.Equals("ADD"))
            {
                pressureMod.newPressureValue = pressureMod.oldPressureValue + modValue;
            }
            else if (modType.Equals("REPLACE"))
            {
                pressureMod.newPressureValue = modValue;
            }
            else
            {
                throw new Exception("Invalid MODIFY EXTPRESSURE modificaiton setting at line " + lineNumber);
            }


            pressureMod.pressureCmd = BFBConstants.EXTRUDER_PRESSURE;
            PressureModifiers.Add(pressureMod);
        }


        private void ValidateRetractLine(string[] lineArray, int oldRetractIndex, int byIndex, int modTypeIndex, int modValueIndex)
        {
            String oldRetract = lineArray[oldRetractIndex];
            String byStr = lineArray[byIndex].ToUpper();
            String modType = lineArray[modTypeIndex].ToUpper();
            String modValueStr = lineArray[modValueIndex];
            int modValue;

            // process OldTemperature
            if (!int.TryParse(oldRetract, out int oldValue))
            {
                throw new Exception("Nonnumeric MODIFY RETRACT old value");
            }

            if (!int.TryParse(modValueStr, out modValue))
            {
                throw new Exception("Invalid MODIFY RETRACT modification value");
            }

            // process BY string
            if (!byStr.Equals("BY"))
            {
                throw new Exception("Invalid MODIFY RETRACT");
            }

            // process modtype string
            {
                if (!(modType.Equals("PERCENTAGE") || modType.Equals("ADD") || modType.Equals("REPLACE")))
                {
                    throw new Exception("Invalid MODIFY TEMPERATURE modificaiton setting");
                }
            }
        }


        public BitFromByte BfbObject { get => bfbObject; set => bfbObject = value; }
        public string FirmwareStr { get => firmwareStr; set => firmwareStr = value; }
        public string MinFirmwareStr { get => minFirmwareStr; set => minFirmwareStr = value; }
        public string PrinterModelStr { get => printerModelStr; set => printerModelStr = value; }
        public string MaterialE1Str { get => materialE1Str; set => materialE1Str = value; }
        public string MaterialE2Str { get => materialE2Str; set => materialE2Str = value; }
        public string MaterialE3Str { get => materialE3Str; set => materialE3Str = value; }
        public List<int> UniqueLeftTemps { get => uniqueLeftTemps; set => uniqueLeftTemps = value; }
        public List<int> UniqueRightTemps { get => uniqueRightTemps; set => uniqueRightTemps = value; }
        public List<int> UniqueMidTemps { get => uniqueMidTemps; set => uniqueMidTemps = value; }
        public List<int> UniqueRetractStarts { get => uniqueRetractStarts; set => uniqueRetractStarts = value; }
        public List<int> UniqueRetractStops { get => uniqueRetractStops; set => uniqueRetractStops = value; }
        public List<double> UniqueExtruderPressure { get => uniqueExtruderPressure; set => uniqueExtruderPressure = value; }
        public List<string> CubeScriptLines { get => cubeScriptLines; set => cubeScriptLines = value; }
        public List<TemperatureModifier> TemperatureModifers { get => temperatureModifers; set => temperatureModifers = value; }
        public List<RetractModifier> RetractStartModifers { get => retractStartModifers; set => retractStartModifers = value; }
        public List<RetractModifier> RetractStopModifers { get => retractStopModifers; set => retractStopModifers = value; }
        public List<PressureModifier> PressureModifiers { get => pressureModifiers; set => pressureModifiers = value; }
    }
}
