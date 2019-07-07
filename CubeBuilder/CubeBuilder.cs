using FileHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeBuilder
{
    class CubeBuilder
    {
        private static string cubeFolder = null;
        private static string cubeFilename = null;
        private static string[] cubeFiles;

        static void Main(string[] args)
        {

            if (args.Length > 0)
            {
                cubeFolder = args[0];
            }

            if (cubeFolder == null)
            {
                displayHelp();
                Environment.Exit(0);
            }

            if (!Directory.Exists(cubeFolder))
            {
                System.Console.WriteLine($"Specified folder, {cubeFolder}, does not exist.");
                Environment.Exit(2);
            }

            cubeFiles = Directory.GetFiles(cubeFolder);
            if (cubeFiles.Length == 0)
            {
                System.Console.WriteLine($"Specified Folder, {cubeFolder}, does not contain any files.");
                Environment.Exit(3);
            }

            cubeFilename = GetCubeFilename();
            if (cubeFilename == null)
            {
                System.Console.WriteLine($"Specified folder, {cubeFolder}, does not contain a Cube3 or CubePro file.");
                Environment.Exit(4);
            }

            BuildCubeFile();
        }

        private static string GetCubeFilename()
        {
            for (int i = 0; i < cubeFiles.Length; i++)
            {
                System.Console.WriteLine(Path.GetExtension(cubeFiles[i]));
                if (Path.GetExtension(cubeFiles[i]).ToUpper().Equals(".CUBE3") || 
                    Path.GetExtension(cubeFiles[i]).ToUpper().Equals(".CUBEPRO"))
                {
                    return Path.GetFileName(cubeFiles[i]);
                }
            }

            return null;
        }

        private static void displayHelp()
        {
            System.Console.WriteLine("usage: cbld <folder>");
            System.Console.WriteLine("");
            System.Console.WriteLine("cbld build a cubeFile from the contents of the folder.  The");
            System.Console.WriteLine("filename of the generated file will be the same as the first");
            System.Console.WriteLine("Cube file located in the folder.");
            System.Console.WriteLine("");
        }

        private static void BuildCubeFile()
        {
            Int16 modelFilenameSize = 0x108;

            CubeExtractor cbld = new CubeExtractor
            {
                ModelFileCount = cubeFiles.Length,

                ModelFileSize = 12 // fileCount, FileSize, filename Size
            };

            for (int i=0; i < cubeFiles.Length; i++)
            {
                cbld.ModelFileSize += modelFilenameSize;
                cbld.ModelFileNames.Add(cubeFiles[i]);

                using (var inFile = File.OpenRead(cubeFiles[i]))
                using (var binaryReader = new BinaryReader(inFile))
                {
                    var inputData = binaryReader.ReadBytes((int)binaryReader.BaseStream.Length);
                    cbld.ModelFiles.Add(cubeFiles[i], inputData);
                    cbld.ModelFileSize += inputData.Length;
                    inFile.Close();
                }
            }

            FileBackup.MakeBackup(cubeFilename, 5);

            using (var outFile = File.OpenWrite(cubeFilename))
            using (var binaryWriter = new BinaryWriter(outFile))
            {
                binaryWriter.Write(cbld.ModelFileCount);
                binaryWriter.Write(cbld.ModelFileSize);
                binaryWriter.Write(modelFilenameSize);
                foreach(String fname in cbld.ModelFileNames)
                {
                    Byte[] fnameData = new byte[modelFilenameSize];
                    Array.Copy(Encoding.ASCII.GetBytes(fname), fnameData, modelFilenameSize);
                    binaryWriter.Write(cbld.ModelFiles[fname].Length);
                    binaryWriter.Write(fnameData, 0, modelFilenameSize);
                }
                outFile.Close();
            }


        }
    }

}
