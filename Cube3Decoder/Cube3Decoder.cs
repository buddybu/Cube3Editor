using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using BitForByteSupport;
using FileHelper;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;


namespace Cube3Decoder
{
    class cube3Decoder
    {
        private static string key = "221BBakerMycroft";
        private static CubeExtractor extractor;
        private static byte[] inputCube3File;
        private static Byte[] dataModel;

        public static BlowfishEngine engine { get; private set; }

        public static void Main(string[] args)
        {
            int cube3FileIndex = 0;
            int bfbFileIndex = 1;

            bool xmlMode = false;
            string cube3File = null;
            string bfbFile = null;

            if (args.Length > 0)
            {
                if (args[0].StartsWith("-") || args[0].StartsWith("/"))
                {
                    cube3FileIndex++;
                    bfbFileIndex++;

                    if (args[0].ToUpper().Equals("-XML"))
                        xmlMode = true;
                    else
                    {
                        System.Console.WriteLine($"Invalid parameter {args[0]}!");
                        System.Console.WriteLine("");
                        displayHelp();
                        Environment.Exit(-1);
                    }
                }
                cube3File = args[cube3FileIndex];
                if (args.Length > bfbFileIndex)
                {
                    bfbFile = args[bfbFileIndex];
                }

            }

            if (cube3File == null)
            {
                displayHelp();
                Environment.Exit(0);
            }

            if (bfbFile == null)
            {
                if (!xmlMode)
                {
                    bfbFile = Path.GetFileNameWithoutExtension(cube3File) + ".BFB";
                }
                else
                {
                    bfbFile = Path.GetFileNameWithoutExtension(cube3File) + ".XML";
                }
            }

            engine = new BlowfishEngine();
            engine.UseLittleEndian = true;
            generateBFBFromCube3File(cube3File, bfbFile, xmlMode);

        }

        private static void displayHelp()
        {
            System.Console.WriteLine("usage: cube3Decoder [-xml] <cube3File> [<bfbFile>]");
            System.Console.WriteLine("");
            System.Console.WriteLine("cube3decoder reads cube3File and decodes the BFBfile.  Will generate output");
            System.Console.WriteLine("into specified bfbFile or a file with the same name as the Cube3 File,");
            System.Console.WriteLine("but with the extension BFB");
            System.Console.WriteLine("");
            System.Console.WriteLine("-xml will strip any ending bytes following the last closing angle bracket.");

        }


        private static void generateBFBFromCube3File(String FileName, String bfbFileName, Boolean xmlMode)
        {
            Encoding encoding = Encoding.ASCII;
            String decodedModel;
            BitFromByte bfbObject;

            PaddedBufferedBlockCipher cipher;

            if (FileName != null || FileName.Length > 0)
            {
                try
                {
                    using (var inFile = File.OpenRead(FileName))
                    using (var binaryReader = new BinaryReader(inFile))
                    {
                        inputCube3File = binaryReader.ReadBytes((int)binaryReader.BaseStream.Length);
                        bool copyInputFile = true;
                        if (!RawCubeFile())
                        {
                            extractor = new CubeExtractor(inputCube3File);

                            String rawCube3Filename = extractor.GetCubeFilename();
                            if (rawCube3Filename != null)
                            {
                                copyInputFile = false;
                                dataModel = new byte[extractor.ModelFiles[rawCube3Filename].Length];
                                Array.Copy(extractor.ModelFiles[rawCube3Filename], dataModel, extractor.ModelFiles[rawCube3Filename].Length);
                            }
                        }

                        if (copyInputFile)
                        {
                            dataModel = new byte[inputCube3File.Length];
                            Array.Copy(inputCube3File, dataModel, inputCube3File.Length);
                        }

                        try
                        {
                            ZeroBytePadding padding = new ZeroBytePadding();
                            cipher = new PaddedBufferedBlockCipher(engine, padding);

                            // make sure buffer is a multiple of Blowfish Block size.
                            int leftover = dataModel.Length % cipher.GetBlockSize();
                            if (leftover != 0)
                            {
                                Array.Resize(ref dataModel, dataModel.Length + (cipher.GetBlockSize() - leftover));
                            }

                            // create the key parameter 
                            Byte[] keyBytes = encoding.GetBytes(key);
                            KeyParameter param = new KeyParameter(encoding.GetBytes(key));

                            // initalize the cipher
                            cipher.Init(false, new KeyParameter(keyBytes));

                            byte[] decodedBytes = new byte[cipher.GetOutputSize(dataModel.Length)];

                            int decodedLength = cipher.ProcessBytes(dataModel, 0, dataModel.Length, decodedBytes, 0);
                            if (decodedLength % 8 == 0)
                                cipher.DoFinal(decodedBytes, decodedLength);

                            decodedModel = encoding.GetString(decodedBytes);

                            if (xmlMode)
                            {
                                int endXmlIndex = decodedModel.LastIndexOf('>');

                                //if (endXmlIndex < decodedModel.Length - 1)
                                //{
                                //    decodedModel = decodedModel.Substring(0, endXmlIndex + 1);
                                //}
                            }

                            string[] seperator = new string[] { "\r\n" };
                            string[] decodedModelArray = decodedModel.Split(seperator, StringSplitOptions.RemoveEmptyEntries);

                            List<String> bfbStringList;
                            if (xmlMode)
                            {
                                bfbStringList = decodedModelArray.ToList();
                            }
                            else
                            {
                                bfbObject = new BitFromByte(encoding, decodedBytes);
                                bfbStringList = bfbObject.BfbLines;
                            }

                            System.IO.StreamWriter file = new System.IO.StreamWriter(bfbFileName, false);

                            int numBFBLines = 0;
                            foreach (string bfbLine in bfbStringList)
                            {
                                numBFBLines++;
                                if (numBFBLines < bfbStringList.Count)
                                    file.WriteLine(bfbLine);
                                else
                                    file.Write(bfbLine);
                            }

                            file.Close();

                        }
                        catch (IOException)
                        {
                            Console.WriteLine($"Unable to process CUBE3 File [{FileName}]\n\n" +
                                $"Was this really a CUBE3 file?");
                            Environment.Exit(-1);
                        }
                    }
                }
                catch (SecurityException ex)
                {
                    Console.WriteLine($"Security error\n\nError message: {ex.Message}\n\n" +
                        $"Details:\n\n{ex.StackTrace}");
                    Environment.Exit(ex.HResult);
                }
            }
            else
            {
                throw new SecurityException("No File specified!");
            }
        }

        private static  bool RawCubeFile()
        {
            bool raw = true;

            Int32 length = BitConverter.ToInt32(inputCube3File, 4);

            if (inputCube3File.Length == length)
            {
                raw = false;
            }
            //if (inputCube3File[14] == 'i' && inputCube3File[15] == 'n' &&
            //    inputCube3File[16] == 'd' && inputCube3File[17] == 'e' &&
            //    inputCube3File[18] == 'x')
            //{
            //    valid = true;
            //}
            return raw;
        }

    }


}
