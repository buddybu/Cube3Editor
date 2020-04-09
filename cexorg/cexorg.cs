using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using FileHelper;

namespace cexorg
{

    class cexorg
    {
        [DllImport("3DSystemsCubeBuildIt.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int EncryptBuffer(IntPtr buffer, int length, string password);

        [DllImport("3DSystemsCubeBuildIt.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int DecryptBuffer(IntPtr buffer, int length, string password);


        private static Blowfish bf;
        private static List<String> modelFileNames = new List<string>();
        private static Dictionary<String, Byte[]> modelFiles = new Dictionary<string, byte[]>();

        private static string key = "221BBakerMycroft";
        private static string key2 = "eXbfXoDj^e\\>?/12";

        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Expecting a filename...");
                Environment.Exit(-2);
            }

            Encoding encoding = Encoding.ASCII;


            try
            {
                using (var inFile = File.OpenRead(args[0]))
                using (var binaryReader = new BinaryReader(inFile))
                {
                    Byte[] inputCubeFile = binaryReader.ReadBytes((int)binaryReader.BaseStream.Length);

                    GCHandle gcHandle = GCHandle.Alloc((object)inputCubeFile, GCHandleType.Pinned);
                    DecryptBuffer(gcHandle.AddrOfPinnedObject(), inputCubeFile.Length, key);
                    gcHandle.Free();

                    var outFile = File.OpenWrite(args[0] + ".decoded");
                    var binaryWriter = new BinaryWriter(outFile);
                    binaryWriter.Write(inputCubeFile);
                }


                //using (var inFile = File.OpenRead(args[0]))
                //using (var binaryReader = new BinaryReader(inFile))
                //{
                //    Byte[] inputCubeFile = binaryReader.ReadBytes((int)binaryReader.BaseStream.Length);

                //    bf.Decipher(inputCubeFile, inputCubeFile.Length);
                    
                    

                //    int fileSize = 0;
                //    int fileIndex = 0;

                //    while (fileIndex < inputCubeFile.Length)
                //    {
                //        fileSize = BitConverter.ToInt32(inputCubeFile, fileIndex);
                //        string fileName = "FILE_" + fileIndex.ToString();
                //        fileIndex += sizeof(UInt32);

                //        Byte[] fileData = new Byte[fileSize];
                //        Array.Copy(inputCubeFile, fileIndex, fileData, 0, fileSize);
                //        fileIndex += fileSize;
                        
                //        modelFileNames.Add(fileName);
                //        modelFiles.Add(fileName, fileData);
                //    }

                //    foreach (String filename in modelFiles.Keys)
                //    {

                //        String directory = Path.GetDirectoryName(filename);

                //        if (directory.Length > 0)
                //        {
                //            Directory.CreateDirectory(directory);
                //        }

                //        FileBackup.MakeBackup(filename, 5);

                //        using (var outFile = File.OpenWrite(filename))
                //        using (var binaryWriter = new BinaryWriter(outFile))
                //        {
                //            binaryWriter.Write(modelFiles[filename]);
                //            outFile.Close();
                //        }
                //    }

                //}
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Exception thrown {ex.Message}");
            }
        }
    }
}