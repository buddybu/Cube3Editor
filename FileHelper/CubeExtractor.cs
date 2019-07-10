using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileHelper
{
    public class CubeExtractor
    {
        private int modelFileCount;
        private int modelFileSize;
        private List<String> modelFileNames = new List<string>();
        private Dictionary<String, Byte[]> modelFiles = new Dictionary<string, byte[]>();

        private int modelFilenameSize;
        private Int16 maxFilenameLengthPlusSize;

        public int ModelFileCount { get => modelFileCount; set => modelFileCount = value; }
        public List<string> ModelFileNames { get => modelFileNames; set => modelFileNames = value; }
        public Dictionary<string, byte[]> ModelFiles { get => modelFiles; set => modelFiles = value; }
        public int ModelFileSize { get => modelFileSize; set => modelFileSize = value; }
        public int ModelFilenameSize { get => modelFilenameSize; set => modelFilenameSize = value; }
        public Int16 MaxFilenameLengthPlusSize { get => maxFilenameLengthPlusSize; set => maxFilenameLengthPlusSize = value; }

        public CubeExtractor()
        {
            ModelFileCount = 0;
            ModelFileSize = 0;
        }

        public CubeExtractor(Byte[] cubeData)
        {
            int fileSize;
            int fileIndex = 0;
            ModelFileCount = BitConverter.ToInt32(cubeData, fileIndex);
            fileIndex += sizeof(UInt32);

            ModelFileSize = BitConverter.ToInt32(cubeData, fileIndex);
            fileIndex += sizeof(UInt32);

            // fileVersion = BitConverter.ToUInt16(cubeData, fileIndex);
            fileIndex += sizeof(UInt16);

            for (int i = 0; i < ModelFileCount; i ++)
            {
                // get file size
                fileSize = BitConverter.ToInt32(cubeData, fileIndex);
                fileIndex += sizeof(UInt32);

                // get file name
                Byte[] fileNameArray = new Byte[260];
                Array.Copy(cubeData, fileIndex, fileNameArray, 0, 260);
                int fileNameLength = Array.IndexOf(fileNameArray, (byte)0, 0);
                Array.Resize(ref fileNameArray, fileNameLength);
                fileIndex += 260;

                Byte[] fileData = new Byte[fileSize];
                Array.Copy(cubeData, fileIndex, fileData, 0, fileSize);
                fileIndex += fileSize;

                string fileName = Encoding.ASCII.GetString(fileNameArray, 0, fileNameArray.Length);
                ModelFileNames.Add(fileName);
                ModelFiles.Add(fileName, fileData);

            }
        }

        public String GetCubeFilename()
        {
            String cubeFilename = null;
            foreach(String filename in ModelFileNames)
            {
                String extension = Path.GetExtension(filename);

                if (extension.ToUpper().Equals(".CUBE3") || extension.ToUpper().Equals(".CUBEPRO"))
                {
                    cubeFilename = filename;
                    break;
                }
            }
            return cubeFilename;
        }
        public String GetXMLFilename()
        {
            String xmlFilename = null;
            foreach (String filename in ModelFileNames)
            {
                String extension = Path.GetExtension(filename);

                if (extension.ToUpper().Equals(".XML"))
                {
                    xmlFilename = filename;
                    break;
                }
            }
            return xmlFilename;
        }
    }
}
