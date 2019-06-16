using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using BitForByteSupport;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;


namespace Cube3ScriptGenerator
{
    class Program
    {
        private static string key = "221BBakerMycroft";

        public static BlowfishEngine engine { get; private set; }

        public static void Main(string[] args)
        {
            string cube3File = null;
            string scriptFile = null;

            if (args.Length > 0)
            {
                cube3File = args[0];
                if (args.Length > 1)
                {
                    scriptFile = args[1];
                }

            }

            if (cube3File == null)
            {
                System.Console.WriteLine("usage: c3sg <cube3File> [<scriptFile>]");
                System.Console.WriteLine("");
                System.Console.WriteLine("c3sg reads cube3File and generates a script file.  Will generate file");
                System.Console.WriteLine("in specified scriptFile or in a file with the same name as the Cube3 File,");
                System.Console.WriteLine("but with the extension SCR");
                System.Console.WriteLine("");
                Environment.Exit(0);
            }

            if (scriptFile == null)
            {
                scriptFile = Path.GetFileNameWithoutExtension(cube3File) + ".SCR";
            }

            engine = new BlowfishEngine(true);
            LoadFile(cube3File);

        }
        private static void LoadFile(String FileName)
        {
            Byte[] dataModel;
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
                        var inputCube3File = binaryReader.ReadBytes((int)binaryReader.BaseStream.Length);
                        int modelDataOffset = 0;
                        if (ValidCube3Header(inputCube3File))
                        {
                            modelDataOffset = Array.IndexOf(inputCube3File, (Byte)0xC8);
                            if (modelDataOffset < 0)
                            {
                                inFile.Close();
                                throw new SecurityException("Invalid CUBE3 File!");
                            }

                            if (modelDataOffset < 14)
                            {
                                modelDataOffset = Array.IndexOf(inputCube3File, (byte)0xC8, modelDataOffset + 1);
                                if (modelDataOffset < 0)
                                {
                                    inFile.Close();
                                    throw new SecurityException("Invalid CUBE3 File!");
                                }
                            }

                        }



                        if (modelDataOffset > 0)
                        {
                            int cube3Size = inputCube3File.Length - modelDataOffset;
                            dataModel = new byte[cube3Size];
                            Array.Copy(inputCube3File, modelDataOffset, dataModel, 0, cube3Size);
                        }
                        else
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

                            bfbObject = new BitFromByte(encoding, decodedBytes);

                            string[] seperator = new string[] { "\r\n" };
                            string[] decodedModelArray = decodedModel.Split(seperator, StringSplitOptions.RemoveEmptyEntries);

                            List<String> bfbStringList = bfbObject.BfbLines;

                            string firmwareStr = bfbObject.GetText(BFBConstants.FIRMWARE);
                            string minFirmwareStr = bfbObject.GetText(BFBConstants.MINFIRMWARE);
                            string printerModelStr = bfbObject.GetText(BFBConstants.PRINTERMODEL);
                            string materialE1Str = bfbObject.GetText(BFBConstants.MATERIALCODEE1);
                            string materialE2Str = bfbObject.GetText(BFBConstants.MATERIALCODEE2);
                            string materialE3Str = bfbObject.GetText(BFBConstants.MATERIALCODEE3);


                            List<int> uniqueLeftTemps = bfbObject.getUniqueTemperatures(BFBConstants.LEFT_TEMP);
                            List<int> uniqueRightTemps = bfbObject.getUniqueTemperatures(BFBConstants.RIGHT_TEMP);
                            List<int> uniqueMidTemps = bfbObject.getUniqueTemperatures(BFBConstants.MID_TEMP);

                            List<int> uniqueRetractStarts = bfbObject.GetUniqueRetractions(BFBConstants.RETRACT_START);
                            List<int> UniqueRetractStops = bfbObject.GetUniqueRetractions(BFBConstants.RETRACT_STOP);

                            

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
        private static bool ValidCube3Header(byte[] inputCube3File)
        {
            bool valid = false;

            if (inputCube3File[14] == 'i' && inputCube3File[15] == 'n' &&
                inputCube3File[16] == 'd' && inputCube3File[17] == 'e' &&
                inputCube3File[18] == 'x')
            {
                valid = true;
            }
            return valid;
        }

    }


}
