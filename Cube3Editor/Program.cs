using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Cube3Editor
{
    static partial class Program
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


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string cubeFile = null, string scriptFile = null)
        {
            // if script is empty, just run the gui
            if (scriptFile == null)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainEditor(cubeFile));
            }
            else
            {
                // load and vaidate script

                // load the file 
                MainEditor cubeEdit = new MainEditor(cubeFile);
            }
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

                    if (lineArray[0].Equals("SET"))
                    {
                        valid = doSet(lineArray, count);
                    }
                    else if (lineArray[0].ToUpper().Equals("MODIFY"))
                    {
                        valid = doModify(lineArray, count);
                    }

                    else if (lineArray[0].ToUpper().Equals("EXECUTE"))
                    {
                        valid = doExecute(lineArray, count);
                    }
                    else
                    {
                        System.Console.WriteLine("Invalid script file at line " + count);
                        valid = false;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Unable to open file '" + scriptFile +"'");
                valid = false;
            }

            return valid;
        }

        // SET <Field> <value>
        private static bool doSet(string[] lineArray, int lineNumber)
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
        private static bool doModify(string[] lineArray, int lineNumber)
        {
            bool valid = false;
            // MODIFY TEMPERATURE [LEFT|RIGHT|MID] XXXX BY [PERCENTAGE [+|-]YYYY|ADD [+|-]YYYY|REPLACE YYYY]
            if (lineArray[1].Equals("TEMPERATURE"))
            {
                valid = doModifyTemperature(lineArray, lineNumber);
            }
            else if (lineArray[1].Equals("RETRACTSTART"))
            {
            }
            else if (lineArray[1].Equals("RETRACTSTOP"))
            {
            }
            else
            {
                System.Console.WriteLine("Invalid MODIFY command at line " + lineNumber);
                valid = false;
            }

            return valid;
        }
        // execute
        private static bool doExecute(string[] lineArray, int lineNumber)
        {
            bool valid = true;
            if (lineArray.Length != 1)
            {
                System.Console.WriteLine("Invalid EXECUTE format at line " + lineNumber);
                valid = false;
            }
            else
            {
            }

            return valid;
        }


        private static bool doModifyTemperature(string[] lineArray, int lineNumber)
        {
            bool valid = true;

            // check parameter count
            if (lineArray.Length < 5 || lineArray.Length > 6)
            {
                System.Console.WriteLine("Invalid MODIFY TEMPERATURE line at line " + lineNumber);
                valid = false;
            }
            // no LEFT/RIGHT/MID specified
            else if (lineArray.Length == 5)
            {
            }
            // LEFT/RIGHT/MID specified
            else
            {
            }

            return valid;
        }

    }
}
