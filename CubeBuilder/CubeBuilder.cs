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


            if (cubeFolder == null || 
                cubeFolder.ToLower().Equals("-help") ||
                cubeFolder.ToLower().Equals("/help") ||
                cubeFolder.ToLower().Equals("-?") ||
                cubeFolder.ToLower().Equals("/?"))
            {
                displayHelp();
                Environment.Exit(0);
            }

            if (!Directory.Exists(cubeFolder))
            {
                System.Console.WriteLine($"Specified folder, {cubeFolder}, does not exist.");
                Environment.Exit(2);
            }

            GetFiles(cubeFolder);
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

        private static void GetFiles(string folder)
        {
            List<string> fileList = new List<string>();
            int baseFolderLen = folder.Length;

            string[] allFiles = Directory.GetFiles(cubeFolder, "*.*", SearchOption.AllDirectories);

            foreach (string file in allFiles)
            {
                string newFile = file;

                if (baseFolderLen > 0)
                {
                    if (file.ToLower().StartsWith(folder.ToLower()))
                    {
                        newFile = file.Substring(baseFolderLen + 1, file.Length - baseFolderLen - 1);
                    }
                }
                fileList.Add(newFile);
            }

            cubeFiles = fileList.ToArray();
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

            CubeExtractor cbld = new CubeExtractor
            {
                ModelFileCount = cubeFiles.Length,

                ModelFileSize = 10, // fileCount, FileSize, filename Size

                ModelFilenameSize = 0x104,
                MaxFilenameLengthPlusSize = 0x108
            };


            for (int i=0; i < cubeFiles.Length; i++)
            {
                cbld.ModelFileSize += cbld.MaxFilenameLengthPlusSize;
                cbld.ModelFileNames.Add(cubeFiles[i]);

                using (var inFile = File.OpenRead(cubeFolder + "\\" + cubeFiles[i]))
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
                binaryWriter.Write(cbld.MaxFilenameLengthPlusSize);
                foreach(String fname in cbld.ModelFileNames)
                {
                    Byte[] fnameData = new byte[cbld.ModelFilenameSize];
                    Byte[] fnameBytes = Encoding.ASCII.GetBytes(fname);
                    int fnameLength = cbld.ModelFilenameSize <= fnameBytes.Length ? cbld.ModelFilenameSize : fnameBytes.Length;
                    Array.Copy(fnameBytes, fnameData, fnameLength);
                    binaryWriter.Write(cbld.ModelFiles[fname].Length);
                    binaryWriter.Write(fnameData, 0, cbld.ModelFilenameSize);
                    binaryWriter.Write(cbld.ModelFiles[fname]);
                }
                outFile.Close();
            }


        }
    }

}
