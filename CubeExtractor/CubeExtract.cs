using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using FileHelper;

namespace CubeExtractorProgram
{
    class CubeExtractorProgram
    {
        private static string cube3File = null;

        public static void Main(string[] args)
        {
            int cube3FileIndex = 0;


            if (args.Length > 0)
            {
                cube3File = args[cube3FileIndex];
            }

            if (cube3File == null ||
                cube3File.ToLower().Equals("-help") ||
                cube3File.ToLower().Equals("/help") ||
                cube3File.ToLower().Equals("-?") ||
                cube3File.ToLower().Equals("/?"))
            {
                displayHelp();
                Environment.Exit(0);
            }

            //engine = new BlowfishEngine(true);
            DecodeCubeFile();

        }

        private static void displayHelp()
        {
            System.Console.WriteLine("usage: CubeExtract <cubeFile>");
            System.Console.WriteLine("");
            System.Console.WriteLine("CubeExtract reads cubeFile and extracts the contents.");
            System.Console.WriteLine("");
        }


        private static void DecodeCubeFile()
        {
            Encoding encoding = Encoding.ASCII;


            if (cube3File != null || cube3File.Length > 0)
            {
                try
                {
                    using (var inFile = File.OpenRead(cube3File))
                    using (var binaryReader = new BinaryReader(inFile))
                    {
                        var inputCubeFile = binaryReader.ReadBytes((int)binaryReader.BaseStream.Length);

                        CubeExtractor ce = new CubeExtractor(inputCubeFile);

                        foreach(String filename in ce.ModelFiles.Keys)
                        {

                            String directory = Path.GetDirectoryName(filename);
                           
                            if (directory.Length > 0)
                            {
                                Directory.CreateDirectory(directory);
                            }

                            FileBackup.MakeBackup(filename, 5);

                            using (var outFile = File.OpenWrite(filename))
                            using (var binaryWriter = new BinaryWriter(outFile))
                            {
                                binaryWriter.Write(ce.ModelFiles[filename]);
                                outFile.Close();
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine($"Exception thrown {ex.Message}");
                }

            }

        }

    }
}

