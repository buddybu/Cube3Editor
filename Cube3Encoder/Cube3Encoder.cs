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

        public static BlowfishEngine engine { get; private set; }

        public static void Main(string[] args)
        {
            string cube3File = null;
            string bfbFile = null;

            if (args.Length > 0)
            {
                if (args[0].StartsWith("-") || args[0].StartsWith("/"))
                {
                    System.Console.WriteLine("usage: cube3Encoder <bfbFile> [<Cube3File>]");
                    System.Console.WriteLine("");
                    System.Console.WriteLine("cube3Encoder reads bfbFile and encodes into Cube3File.   Will generate");
                    System.Console.WriteLine("into specified Cube3File or a file with the same name as the BFB File,");
                    System.Console.WriteLine("but with the extension Cube3");
                    System.Console.WriteLine("");
                    Environment.Exit(0);
                }

                bfbFile = args[0];
                if (args.Length > 1)
                {
                   cube3File = args[1];
                }

            }

            if (bfbFile == null)
            {
                System.Console.WriteLine("usage: cube3Encoder <bfbFile> [<Cube3File>]");
                System.Console.WriteLine("");
                System.Console.WriteLine("cube3Encoder reads bfbFile and encodes into Cube3File.   Will generate");
                System.Console.WriteLine("into specified Cube3File or a file with the same name as the BFB File,");
                System.Console.WriteLine("but with the extension Cube3");
                System.Console.WriteLine("");
                Environment.Exit(0);
            }

            if (cube3File == null)
            {
                cube3File = Path.GetFileNameWithoutExtension(bfbFile) + ".Cube3";
            }

            engine = new BlowfishEngine();
            engine.UseLittleEndian = true;
            generateCube3FromBFBFile(bfbFile, cube3File);

        }

        private static void generateCube3FromBFBFile(String FileName, String cube3FileName)
        {
            Encoding encoding = Encoding.ASCII;

            PaddedBufferedBlockCipher cipher;

            if (FileName != null || FileName.Length > 0)
            {
                try
                {
                    using (var inFile = File.OpenRead(FileName))
                    using (var streamReader = new StreamReader(inFile))
                    {
                        var inputBFBFile = streamReader.ReadToEnd();

                        try
                        {
                            ZeroBytePadding padding = new ZeroBytePadding();
                            cipher = new PaddedBufferedBlockCipher(engine, padding);

                            // create the key parameter 
                            Byte[] keyBytes = encoding.GetBytes(key);
                            KeyParameter param = new KeyParameter(encoding.GetBytes(key));

                            // initalize the cipher
                            cipher.Init(true, new KeyParameter(keyBytes));

                            Byte[] newDataModel = encoding.GetBytes(inputBFBFile);

                            Byte[] encodedBytes = new Byte[cipher.GetOutputSize(newDataModel.Length)];

                            int encodedLength = cipher.ProcessBytes(newDataModel, 0, newDataModel.Length, encodedBytes, 0);
                            cipher.DoFinal(encodedBytes, encodedLength);

                            FileBackup.MakeBackup(cube3FileName, 5);

                            using (var outFile = File.OpenWrite(cube3FileName))
                            using (var binaryWriter = new BinaryWriter(outFile))
                            {
                                binaryWriter.Write(encodedBytes);
                                outFile.Close();
                            }

                        }
                        catch (IOException)
                        {
                            Console.WriteLine($"Unable to process BFB File [{FileName}] or unable to open output file [{cube3FileName}]");
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
    }


}
