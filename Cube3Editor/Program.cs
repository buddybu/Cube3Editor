using BitForByteSupport;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static Cube3Editor.MainEditor;
using static Cube3Editor.TemperatureType;

namespace Cube3Editor
{
    class Program
    {
        private static string firmwareStr;
        private static string minfirmwareStr;
        private static string printerModelStr;
        private static string materialCodeE1;
        private static string materialCodeE2;
        private static string materialCodeE3;

        private static List<TemperatureModifier> temperatureModifers = new List<TemperatureModifier>();
        private static List<RetractModifier> retractStartModifers = new List<RetractModifier>();
        private static List<RetractModifier> retractStopModifers = new List<RetractModifier>();
        private static List<PressureModifier> pressureModifiers = new List<PressureModifier>();

        [DllImport("kernel32.dll")]
        static extern bool AttachConsole(int dwProcessId);
        private const int ATTACH_PARENT_PROCESS = -1;

        //static void Main(string[] args)
        //{
        //    System.Console.WriteLine($"args = {args}");
        //    System.Console.Error.WriteLine($"args = {args}");
        //    Main2(args);
        //}

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {



            string cubeFile = null;
            string scriptFile = null;

            if (args.Length > 0)
            {
                cubeFile = args[0];
                if (args.Length > 1)
                {
                    scriptFile = args[1];
                }

            }

            // if script is empty, just run the gui
            if (scriptFile == null)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainEditor(cubeFile));
            }
            else
            {

                // redirect console output to parent process;
                // must be before any calls to Console.WriteLine()
                AttachConsole(ATTACH_PARENT_PROCESS);

                System.Console.WriteLine($"cubeFile = '{cubeFile}'");
                System.Console.WriteLine($"scriptFile = '{scriptFile}'");

                // load and vaidate script
                if (LoadAndValidateScript(scriptFile))
                {
                    // load the file 
                    MainEditor cubeEdit = new MainEditor(cubeFile);

                    cubeEdit.CommandLineLoad(firmwareStr, minfirmwareStr, printerModelStr, materialCodeE1, materialCodeE2, materialCodeE3,
                        temperatureModifers, retractStartModifers, retractStopModifers, pressureModifiers);

                    cubeEdit.CommandLineSave();
                }

                SendKeys.SendWait("{ENTER}");
                Environment.Exit(0);

            }
        }

        // Send the "enter" to the console to release the command prompt
        // on the parent console
        static void SendEnterKey()
        {
        }

        private static bool LoadAndValidateScript(string scriptFile)
        {
            bool valid = true;
            string line;
            int count = 0;

            try
            {
                System.IO.StreamReader file = new System.IO.StreamReader(scriptFile);
                while ((line = file.ReadLine()) != null && valid)
                {
                    count++;
                    string[] lineArray = line.ToUpper().Split(null);

                    if (lineArray.Length == 0)
                    {
                        // empty line, skip it
                        continue;
                    }

                    if (lineArray[0].Equals("SET"))
                    {
                        valid = DoSet(lineArray, count);
                    }
                    else if (lineArray[0].ToUpper().Equals("MODIFY"))
                    {
                        valid = DoModify(lineArray, count);
                    }

                    //else if (lineArray[0].ToUpper().Equals("EXECUTE"))
                    //{
                    //    valid = doExecute(lineArray, count);
                    //}
                    else
                    {
                        System.Console.WriteLine("Invalid script file at line " + count);
                        valid = false;
                    }
                }
            }
            catch (Exception)
            {
                System.Console.WriteLine("Unable to open file '" + scriptFile + "'");
                valid = false;
            }

            return valid;
        }

        // SET <Field> <value>
        private static bool DoSet(string[] lineArray, int lineNumber)
        {
            bool valid = true;
            if (lineArray[1].Equals("") || lineArray.Length != 3)
            {
                System.Console.WriteLine("Invalid SET line at line " + lineNumber);
                valid = false;
            }
            else if (lineArray[1].Equals("FIRMWARE"))
            {
                firmwareStr = lineArray[2];
            }
            else if (lineArray[1].Equals("MINFIRMWARE"))
            {
                minfirmwareStr = lineArray[2];
            }
            else if (lineArray[1].Equals("MODEL"))
            {
                printerModelStr = lineArray[2];
            }
            else if (lineArray[1].Equals("E1"))
            {
                materialCodeE1 = lineArray[2];
            }
            else if (lineArray[1].Equals("E2"))
            {
                materialCodeE2 = lineArray[2];
            }
            else if (lineArray[1].Equals("E3"))
            {
                materialCodeE3 = lineArray[2];
            }
            else
            {
                System.Console.WriteLine("Invalid SET command at line " + lineNumber);
                valid = false;
            }

            return valid;
        }

        // modify temperature [<modifier>] xxxx by <mod type> <mod value>
        // modify retractstart xxxx by <mod type> <mod value>
        // modify retractstop xxxx by <mod type> <mod value>
        private static bool DoModify(string[] lineArray, int lineNumber)
        {
            bool valid = false;
            // MODIFY TEMPERATURE [LEFT|RIGHT|MID] XXXX BY [PERCENTAGE [+|-]YYYY|ADD [+|-]YYYY|REPLACE YYYY]
            if (lineArray[1].Equals("TEMPERATURE"))
            {
                valid = DoModifyTemperature(lineArray, lineNumber);
            }

            // MODIFY RETRACTSTART XXXX BY [PERCENTAGE [+|-]YYYY|ADD [+|-]YYYY|REPLACE YYYY]
            else if (lineArray[1].Equals("RETRACTSTART"))
            {
                valid = DoModifyRetractStart(lineArray, lineNumber);
            }

            // MODIFY RETRACTSTOP XXXX BY [PERCENTAGE [+|-]YYYY|ADD [+|-]YYYY|REPLACE YYYY]
            else if (lineArray[1].Equals("RETRACTSTOP"))
            {
                valid = DoModifyRetractStop(lineArray, lineNumber);
            }
            else if (lineArray[1].Equals("EXTPRESSURE"))
            {
                valid = DoModifyPressure(lineArray, lineNumber);
            }
            else
            {
                System.Console.WriteLine("Invalid MODIFY command at line " + lineNumber);
                valid = false;
            }

            return valid;
        }

        //// execute
        //private static bool DoExecute(string[] lineArray, int lineNumber)
        //{
        //    bool valid = true;
        //    if (lineArray.Length != 1)
        //    {
        //        System.Console.WriteLine("Invalid EXECUTE format at line " + lineNumber);
        //        valid = false;
        //    }
        //    else
        //    {
        //    }

        //    return valid;
        //}


        private static bool DoModifyTemperature(string[] lineArray, int lineNumber)
        {
            bool valid = true;
            int oldTempIndex = 2;
            int byIndex = 3;
            int modTypeIndex = 4;
            int modValueIndex = 5;
            int sideIndex = -1;
            TemperatureEnum tempType = TemperatureEnum.ALL;

            // check parameter count
            if (lineArray.Length < 6 || lineArray.Length > 7)
            {
                System.Console.WriteLine("Invalid MODIFY TEMPERATURE at line " + lineNumber);
                valid = false;
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


            if (valid && sideIndex > 0)
            {
                valid = Enum.TryParse(lineArray[sideIndex], out tempType);
                if (!valid)
                {
                    System.Console.WriteLine("Invalid MODIFY TEMPERATURE side setting at line " + lineNumber);
                }
            }

            if (valid)
            {
                String oldTemperature = lineArray[oldTempIndex];
                String byStr = lineArray[byIndex].ToUpper();
                String modType = lineArray[modTypeIndex].ToUpper();
                String modValueStr = lineArray[modValueIndex];
                int modValue;

                TemperatureModifier tempMod = new TemperatureModifier();

                // process OldTemperature
                if (!int.TryParse(oldTemperature, out tempMod.oldValue))
                {
                    System.Console.WriteLine("Nonnumeric MODIFY TEMPERATURE old value at line " + lineNumber);
                    valid = false;
                }

                if (!int.TryParse(modValueStr, out modValue))
                {
                    System.Console.WriteLine("Invalid MODIFY TEMPERATURE modification value at line " + lineNumber);
                    valid = false;
                }

                // process BY string
                if (valid && !byStr.Equals("BY"))
                {
                    System.Console.WriteLine("Invalid MODIFY TEMPERATURE at line " + lineNumber);
                    valid = false;
                }

                // process modtype string
                if (valid)
                {
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
                        System.Console.WriteLine("Invalid MODIFY TEMPERATURE modificaiton setting at line " + lineNumber);
                        valid = false;
                    }
                }

                if (valid)
                {
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
                    }

                    if (leftMod != null)
                    {
                        leftMod.extruder = BFBConstants.LEFT_TEMP;
                        leftMod.oldValue = tempMod.oldValue;
                        leftMod.newValue = tempMod.newValue;
                        temperatureModifers.Add(leftMod);
                    }

                    if (rightMod != null)
                    {
                        rightMod.extruder = BFBConstants.RIGHT_TEMP;
                        rightMod.oldValue = tempMod.oldValue;
                        rightMod.newValue = tempMod.newValue;
                        temperatureModifers.Add(rightMod);
                    }

                    if (midMod != null)
                    {
                        midMod.extruder = BFBConstants.MID_TEMP;
                        midMod.oldValue = tempMod.oldValue;
                        midMod.newValue = tempMod.newValue;
                        temperatureModifers.Add(midMod);
                    }
                }
            }

            return valid;
        }

        // MODIFY RETRACTSTART XXXX BY [PERCENTAGE [+|-]YYYY|ADD [+|-]YYYY|REPLACE YYYY]
        private static bool DoModifyRetractStart(string[] lineArray, int lineNumber)
        {
            bool valid = true;
            int oldRetractIndex = 2;
            int byIndex = 3;
            int modTypeIndex = 4;
            int modValueIndex = 5;

            // check parameter count and validate line
            if (lineArray.Length != 6 || !ValidRetractLine(lineArray, oldRetractIndex, byIndex, modTypeIndex, modValueIndex))
            {
                System.Console.WriteLine("Invalid MODIFY RETRACTSTART at line " + lineNumber);
                valid = false;
            }

            if (valid)
            {
                RetractModifier retractMod = new RetractModifier();

                // process OldTemperature
                int.TryParse(lineArray[oldRetractIndex], out retractMod.oldRetractValue);
                int.TryParse(lineArray[modValueIndex], out int modValue);

                string modType = lineArray[modTypeIndex].ToUpper();
                if (modType.Equals("PERCENTAGE"))
                {
                    double percentage = modValue;
                    retractMod.newRetractValue = Convert.ToInt32(retractMod.oldRetractValue + (percentage / 100) * retractMod.oldRetractValue);
                }
                else if (modType.Equals("ADD"))
                {
                    retractMod.newRetractValue = retractMod.oldRetractValue + modValue;
                }
                else if (modType.Equals("REPLACE"))
                {
                    retractMod.newRetractValue = modValue;
                }

                retractMod.retractCmd = BFBConstants.RETRACT_START;

                retractStartModifers.Add(retractMod);
            }

            return valid;
        }

        // MODIFY RETRACTSTOP XXXX BY [PERCENTAGE [+|-]YYYY|ADD [+|-]YYYY|REPLACE YYYY]
        private static bool DoModifyRetractStop(string[] lineArray, int lineNumber)
        {
            bool valid = true;
            int oldRetractIndex = 2;
            int byIndex = 3;
            int modTypeIndex = 4;
            int modValueIndex = 5;

            // check parameter count
            if (lineArray.Length != 6)
            {
                System.Console.WriteLine("Invalid MODIFY RETRACTSTOP at line " + lineNumber);
                valid = false;
            }

            if (valid)
            {
                RetractModifier retractMod = new RetractModifier();

                // process OldTemperature
                int.TryParse(lineArray[oldRetractIndex], out retractMod.oldRetractValue);
                int.TryParse(lineArray[modValueIndex], out int modValue);

                string modType = lineArray[modTypeIndex].ToUpper();
                if (modType.Equals("PERCENTAGE"))
                {
                    double percentage = modValue;
                    retractMod.newRetractValue = Convert.ToInt32(retractMod.oldRetractValue + (percentage / 100) * retractMod.oldRetractValue);
                }
                else if (modType.Equals("ADD"))
                {
                    retractMod.newRetractValue = retractMod.oldRetractValue + modValue;
                }
                else if (modType.Equals("REPLACE"))
                {
                    retractMod.newRetractValue = modValue;
                }

                retractMod.retractCmd = BFBConstants.RETRACT_STOP;

                retractStopModifers.Add(retractMod);
            }
            return valid;
        }

        private static bool DoModifyPressure(string[] lineArray, int lineNumber)
        {
            bool valid = true;
            int oldPressureIndex = 2;
            int byIndex = 3;
            int modTypeIndex = 4;
            int modValueIndex = 5;

            // check parameter count
            if (lineArray.Length != 6)
            {
                System.Console.WriteLine("Invalid MODIFY EXTPRESSURE at line " + lineNumber);
                valid = false;
            }


            if (valid)
            {
                String oldTPressure = lineArray[oldPressureIndex];
                String byStr = lineArray[byIndex].ToUpper();
                String modType = lineArray[modTypeIndex].ToUpper();
                String modValueStr = lineArray[modValueIndex];
                Double modValue;

                PressureModifier pressureMod = new PressureModifier();

                // process OldPressure
                if (!Double.TryParse(oldTPressure, out pressureMod.oldPressureValue))
                {
                    System.Console.WriteLine("Nonnumeric MODIFY EXTPRESSURE old value at line " + lineNumber);
                    valid = false;
                }

                if (!Double.TryParse(modValueStr, out modValue))
                {
                    System.Console.WriteLine("Invalid MODIFY EXTPRESSURE modification value at line " + lineNumber);
                    valid = false;
                }

                // process BY string
                if (valid && !byStr.Equals("BY"))
                {
                    System.Console.WriteLine("Invalid MODIFY EXTPRESSURE at line " + lineNumber);
                    valid = false;
                }

                // process modtype string
                if (valid)
                {
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
                        System.Console.WriteLine("Invalid MODIFY EXTPRESSURE modificaiton setting at line " + lineNumber);
                        valid = false;
                    }
                }

                if (valid)
                {
                    pressureMod.pressureCmd = BFBConstants.EXTRUDER_PRESSURE;
                    pressureModifiers.Add(pressureMod);
                }
            }

            return valid;
        }


        private static bool ValidRetractLine(string[] lineArray, int oldRetractIndex, int byIndex, int modTypeIndex, int modValueIndex)
        {
            String oldRetract = lineArray[oldRetractIndex];
            String byStr = lineArray[byIndex].ToUpper();
            String modType = lineArray[modTypeIndex].ToUpper();
            String modValueStr = lineArray[modValueIndex];
            int modValue;
            bool valid = true;

            // process OldTemperature
            if (!int.TryParse(oldRetract, out int oldValue))
            {
                System.Console.WriteLine("Nonnumeric MODIFY RETRACT old value");
                valid = false;
            }

            if (!int.TryParse(modValueStr, out modValue))
            {
                System.Console.WriteLine("Invalid MODIFY RETRACT modification value");
                valid = false;
            }

            // process BY string
            if (valid && !byStr.Equals("BY"))
            {
                System.Console.WriteLine("Invalid MODIFY RETRACT");
                valid = false;
            }

            // process modtype string
            if (valid)
            {
                if (modType.Equals("PERCENTAGE"))
                {
                    valid = true;
                }
                else if (modType.Equals("ADD"))
                {
                    valid = true;
                }
                else if (modType.Equals("REPLACE"))
                {
                    valid = true;
                }
                else
                {
                    System.Console.WriteLine("Invalid MODIFY TEMPERATURE modificaiton setting");
                    valid = false;
                }
            }

            return valid;
        }


    }
}
