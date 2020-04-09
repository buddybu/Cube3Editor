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


namespace Cube3ScriptGenerator
{
    class c3sg
    {
        private static string key = "221BBakerMycroft";

        private static Byte[] inputCubeFile;
        private static CubeExtractor extractor;
        private static Byte[] dataModel;

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

            if (cube3File == null ||
                cube3File.ToLower().Equals("-help") ||
                cube3File.ToLower().Equals("/help") ||
                cube3File.ToLower().Equals("-?") ||
                cube3File.ToLower().Equals("/?"))
            {
                System.Console.WriteLine("usage: c3sg <cube3File> [<scriptFile>]");
                System.Console.WriteLine("");
                System.Console.WriteLine("c3sg reads cube3File and generates a script file.  Will generate file");
                System.Console.WriteLine("in specified scriptFile or in a file with the same name as the Cube3 File,");
                System.Console.WriteLine("but with the extension CUBESCR");
                System.Console.WriteLine("");
                Environment.Exit(0);
            }

            if (scriptFile == null)
            {
                scriptFile = Path.GetFileNameWithoutExtension(cube3File) + ".CUBESCR";
            }

            engine = new BlowfishEngine();
            engine.UseLittleEndian = true;
            generateScriptFromCube3File(cube3File, scriptFile);

        }
        private static void generateScriptFromCube3File(String FileName, String scriptFile)
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
                        inputCubeFile = binaryReader.ReadBytes((int)binaryReader.BaseStream.Length);
                        bool copyInputFile = true;
                        if (!RawCubeFile())
                        {
                            extractor = new CubeExtractor(inputCubeFile);

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
                            dataModel = new byte[inputCubeFile.Length];
                            Array.Copy(inputCubeFile, dataModel, inputCubeFile.Length);
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

                            CubeScript cscript = new CubeScript(bfbObject);

                            System.IO.StreamWriter file = new System.IO.StreamWriter(scriptFile, false);

                            foreach(String line in cscript.CubeScriptLines)
                            {
                                file.WriteLine(line);
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

        private static bool RawCubeFile()
        {
            bool raw = true;

            Int32 length = BitConverter.ToInt32(inputCubeFile, 4);

            if (inputCubeFile.Length == length)
            {
                raw = false;
            }
            return raw;
        }

    }


}
