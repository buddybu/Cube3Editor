using BitForByteSupport;
using DevAge.Drawing;
using FileHelper;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using SourceGrid.Cells.Views;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Cube3Editor
{
    public partial class MainEditor : Form
    {

        bool modified = false;

        string fileName;
        Stream inFile;
        Stream outFile;

        BitFromByte bfbObject;
        CubeScript cubeScript;
        private PreferencesDialog preferencesDialog;
        BlowfishEngine engine;
        byte[] inputCube3File;
        CubeExtractor extractor;

        private string key = "221BBakerMycroft";

        static readonly Encoding encoding = Encoding.ASCII;
        private String decodedModel;
        private bool copyInputFile;
        private Byte[] dataModel;
        private RectangleBorder border;
        private SourceGrid.Cells.Views.Cell tempView;


        private Preferences prefs = new Preferences();

        public RectangleBorder Border
        {
            get => border;
            set => border = value;
        }

        public Cell TempView
        {
            get => tempView;
            set => tempView = value;
        }
        public PreferencesDialog PreferencesDialog { get => preferencesDialog; set => preferencesDialog = value; }
        internal Preferences Prefs { get => prefs; set => prefs = value; }

        public MainEditor(string cubeFile)
        {
            InitializeComponent();

            openFileDialog.Filter = "CUBE Files |*.cube3;*.cubepro|All Files (*.*)|*.*";
            openFileDialog.Title = "Please Select a CUBE3 File to edit.";

            saveFileDialog.Filter = "CUBE Files |*.cube3;*.cubepro|All Files (*.*)|*.*";
            saveFileDialog.Title = "Save CUBE File";

            saveScriptDialog.Filter = "CUBE Script Files |*.cubescr;*.scr|All Files (*.*)|*.*";
            saveScriptDialog.Title = "Save Cube Script";


            PreferencesDialog = new PreferencesDialog(Prefs);

            engine = new BlowfishEngine();
            engine.UseLittleEndian = true;


            if (cubeFile != null)
            {
                fileName = cubeFile;
                PrepareForm();
                LoadFile(fileName);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

            try
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    ClearUI();

                    char[] seperator = { ' ', '-', ' ' };
                    string title = Text.Split(seperator)[0];
                    Text = title;

                    LoadFile(openFileDialog.FileName);
                    fileName = openFileDialog.FileName;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to open file [" + openFileDialog.FileName + "]", "Invalid CUBE File!");
            }
            finally
            {
                if (inFile != null)
                    inFile.Close();
            }

        }

        internal void CommandLineSave()
        {
            SaveFile();
        }

        private void LoadFile(String filename)
        {
            PaddedBufferedBlockCipher cipher;
            if (filename != null || filename.Length > 0)
            {
                ClearUI();
                try
                {

                    using (inFile = File.OpenRead(filename))
                    using (var binaryReader = new BinaryReader(inFile))
                    {
                        inputCube3File = binaryReader.ReadBytes((int)binaryReader.BaseStream.Length);

                        copyInputFile = true;
                        extractor = null;

                        if (!RawCubeFile())
                        {
                            extractor = new CubeExtractor(inputCube3File)
                            {
                                ModelFilenameSize = 0x104,
                                MaxFilenameLengthPlusSize = 0x108
                            };
                            

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
                            //cipher = new BufferedBlockCipher(engine);

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

                            Text = Text + " - " + filename;

                            decodedModel = encoding.GetString(decodedBytes);

                            ProcessAndLoadBFB(decodedBytes);

                            // clear modified flag
                            modified = false;

                        }
                        catch (IOException)
                        {
                            MessageBox.Show($"Unable to process CUBE3 File [{openFileDialog.FileName}]\n\n" +
                                $"Was this really a CUBE3 file?");
                        }
                    }
                }
                catch (SecurityException ex)
                {
                    MessageBox.Show($"Security error\n\nError message: {ex.Message}\n\n" +
                        $"Details:\n\n{ex.StackTrace}");
                }
            }
            else
            {
                throw new SecurityException("No File specified!");
            }
        }


        private void ProcessAndLoadBFB(Byte[] bfbBytes)
        {
            bfbObject = new BitFromByte(encoding, bfbBytes);

            string[] seperator = { "\r\n" };
            string[] decodedModelArray = decodedModel.Split(seperator, StringSplitOptions.RemoveEmptyEntries);

            List<String> bfbStringList = decodedModelArray.ToList<string>();

            ClearUI();

            // populate all fields
            cbFirmware.Text = bfbObject.GetText(BFBConstants.FIRMWARE);

            cbFirmware.Enabled = true;
            cbMinFirmware.Enabled = true;
            cbPrinterModel.Enabled = true;

            cbRightColor.Enabled = true;
            cbLeftColor.Enabled = true;
            cbCubeProColor.Enabled = true;

            cbMinFirmware.Text = bfbObject.GetText(BFBConstants.MINFIRMWARE);
            cbPrinterModel.Text = bfbObject.GetText(BFBConstants.PRINTERMODEL);
            cbLeftMaterial.Text = bfbObject.GetMaterialType(BFBConstants.MATERIALCODEE1);

            cbLeftColor.Text = bfbObject.GetMaterialColor(BFBConstants.MATERIALCODEE1);
            if (cbLeftMaterial.Text.Equals(BFBConstants.EMPTY))
            {
                cbLeftColor.Enabled = false;
                cbLeftSidewalk.Enabled = false;
                cbLeftSupport.Enabled = false;
            }
            else
            {
                if (bfbObject.GetSUPPORTS().Equals(bfbObject.GetMaterialColor(BFBConstants.MATERIALCODEE1)))
                {
                    cbLeftSupport.Checked = true;
                }

                if (bfbObject.GetSIDEWALKS().Equals(bfbObject.GetMaterialColor(BFBConstants.MATERIALCODEE1)))
                {
                    cbLeftSidewalk.Checked = true;
                }
            }

            cbRightMaterial.Text = bfbObject.GetMaterialType(BFBConstants.MATERIALCODEE2);
            cbRightColor.Text = bfbObject.GetMaterialColor(BFBConstants.MATERIALCODEE2);
            if (cbRightMaterial.Text.Equals(BFBConstants.EMPTY))
            {
                cbRightColor.Enabled = false;
                cbRightSidewalk.Enabled = false;
                cbRightSupport.Enabled = false;
            }
            else
            {
                if (bfbObject.GetSUPPORTS().Equals(bfbObject.GetMaterialColor(BFBConstants.MATERIALCODEE2)))
                {
                    cbRightSupport.Checked = true;
                }

                if (bfbObject.GetSIDEWALKS().Equals(bfbObject.GetMaterialColor(BFBConstants.MATERIALCODEE2)))
                {
                    cbRightSidewalk.Checked = true;
                }
            }

            cbCubeProMaterial.Text = bfbObject.GetMaterialType(BFBConstants.MATERIALCODEE3);
            cbCubeProColor.Text = bfbObject.GetMaterialColor(BFBConstants.MATERIALCODEE3);
            if (cbCubeProMaterial.Text.Equals(BFBConstants.EMPTY))
            {
                cbCubeProColor.Enabled = false;
                cbCubeProSidewalk.Enabled = false;
                cbCubeProSupport.Enabled = false;
            }
            else
            {
                if (bfbObject.GetSUPPORTS().Equals(bfbObject.GetMaterialColor(BFBConstants.MATERIALCODEE3)))
                {
                    cbCubeProSupport.Checked = true;
                }

                if (bfbObject.GetSIDEWALKS().Equals(bfbObject.GetMaterialColor(BFBConstants.MATERIALCODEE3)))
                {
                    cbCubeProSidewalk.Checked = true;
                }
            }

            PopulateTemperatures(BFBConstants.LEFT_TEMP, gridLeftTemps);
            PopulateTemperatures(BFBConstants.RIGHT_TEMP, gridRightTemps);
            PopulateRetractionStarts();
            PopulateRetractionStops();
            PopulatePressures();

            // enable material fields
            cbFirmware.Enabled = true;
            cbMinFirmware.Enabled = true;
            cbPrinterModel.Enabled = true;
            cbRightMaterial.Enabled = true;
            cbLeftMaterial.Enabled = true;
            cbCubeProMaterial.Enabled = true;

        }
        private bool RawCubeFile()
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

        private void ClearUI()
        {
            cbFirmware.Text = "";
            cbMinFirmware.Text = "";
            cbPrinterModel.Text = "";

            cbLeftColor.Text = "";
            cbLeftMaterial.Text = "";
            cbRightColor.Text = "";
            cbRightMaterial.Text = "";
            cbCubeProColor.Text = "";
            cbCubeProMaterial.Text = "";

            if (gridLeftTemps.Rows.Count > 1)
                gridLeftTemps.Rows.RemoveRange(1, gridLeftTemps.Rows.Count - 1);
            if (gridRightTemps.Rows.Count > 1)
                gridRightTemps.Rows.RemoveRange(1, gridRightTemps.Rows.Count - 1);
            if (gridRetractionStart.Rows.Count > 1)
                gridRetractionStart.Rows.RemoveRange(1, gridRetractionStart.Rows.Count - 1);
            if (gridRetractionStop.Rows.Count > 1)
                gridRetractionStop.Rows.RemoveRange(1, gridRetractionStop.Rows.Count - 1);
            if (gridPressure.Rows.Count > 1)
                gridPressure.Rows.RemoveRange(1, gridPressure.Rows.Count - 1);

            cubeScript = new CubeScript();
        }

        private void ResetUIToDefault()
        {
            // don't do this if a file is loaded
            if (fileName == null)
            {
                cbFirmware.Enabled = false;
                cbMinFirmware.Enabled = false;
                cbPrinterModel.Enabled = false;

                cbRightMaterial.Enabled = false;
                cbLeftMaterial.Enabled = false;
                cbCubeProMaterial.Enabled = false;

                cbRightColor.Enabled = false;
                cbLeftColor.Enabled = false;
                cbCubeProColor.Enabled = false;

                cbRightSupport.Enabled = false;
                cbRightSidewalk.Enabled = false;
                cbLeftSupport.Enabled = false;
                cbLeftSidewalk.Enabled = false;
                cbCubeProSupport.Enabled = false;
                cbCubeProSidewalk.Enabled = false;
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Boolean doClose = false;
            if (modified)
            {
                DialogResult result = MessageBox.Show("There have been changes, are you sure you want to close?", "Confirmation",
                    MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    doClose = true;
                } 
                // display dialog box that something has changed.  Are you sure you want to close the file?
                // if yes, then close the file.
                // If no, then call the save file routine
                // reset all controls
            }
            else
            {
                doClose = true;
            }

            if (doClose)
            {
                ClearUI();
                cubeScript = null;
                bfbObject = null;
                fileName = null;
                Text = "Cube3Editor";
                ResetUIToDefault();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (bfbObject != null)
            {
                SaveFile();
            }
            else
            {
                MessageBox.Show("Please load a Cube model.", "No Model Present", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    SaveFile(saveFileDialog.FileName, false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to save file [" + saveFileDialog.FileName + "]", ex.Message);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (modified)
            {
                DialogResult result = MessageBox.Show("There have been changes, are you sure you want to exit?", "Confirmation",
                    MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    Application.Exit();
                }
            }
        }

        private Byte[] EncodeBFB()
        {
            Byte[] newDataModel = bfbObject.GetBytesFromBFB();
            PaddedBufferedBlockCipher cipher;
            ZeroBytePadding padding = new ZeroBytePadding();
            cipher = new PaddedBufferedBlockCipher(engine, padding);

            // create the key parameter 
            Byte[] keyBytes = encoding.GetBytes(key);
            KeyParameter param = new KeyParameter(encoding.GetBytes(key));

            // initalize the cipher
            cipher.Init(true, new KeyParameter(keyBytes));

            Byte[] encodedBytes = new Byte[cipher.GetOutputSize(newDataModel.Length)];

            int encodedLength = cipher.ProcessBytes(newDataModel, 0, newDataModel.Length, encodedBytes, 0);
            cipher.DoFinal(encodedBytes, encodedLength);

            return encodedBytes;

        }

        private Byte[] ProcessXML(Byte[] xmlBytes, Byte[] encodedBFB)
        {
            Byte[] newXMLBytes = null;

            string xmlString = encoding.GetString(xmlBytes);
            var xDoc = XDocument.Parse(xmlString);

            var build = xDoc.Root.Element("build");

            build.SetElementValue("type", "cube");

            CRC32 crc = new CRC32();
            build.SetElementValue("build_crc32", crc.ComputeChecksum(encodedBFB).ToString());

            build.Element("materials").Element("extruder1").SetElementValue("code", bfbObject.GetMATERIALCODE(BFBConstants.MATERIALCODEE1).ToString());
            build.Element("materials").Element("extruder1").SetElementValue("recycled","0");
            build.Element("materials").Element("extruder2").SetElementValue("code", bfbObject.GetMATERIALCODE(BFBConstants.MATERIALCODEE2).ToString());
            build.Element("materials").Element("extruder2").SetElementValue("recycled", "0");
            build.Element("materials").Element("extruder3").SetElementValue("code", bfbObject.GetMATERIALCODE(BFBConstants.MATERIALCODEE3).ToString());
            build.Element("materials").Element("extruder3").SetElementValue("recycled", "0");

            build.SetElementValue("supports", bfbObject.GetSUPPORTS());
            build.SetElementValue("sidewalk", bfbObject.GetSIDEWALKS());

            StringBuilder sb = new StringBuilder();
            TextWriter tr = new StringWriter(sb);
            xDoc.Save(tr);
            String newXml = sb.ToString();
            newXMLBytes = encoding.GetBytes(sb.ToString());
            return newXMLBytes;
        }

        private void SaveFile(Boolean addMod = true)
        {
            SaveFile(fileName, addMod);
        }

        private void SaveFile(String filename, Boolean addMod = true)
        {
            inFile.Close();
            inFile.Dispose();

            Byte[] encodedBFB = EncodeBFB();

            var newPath = Path.GetDirectoryName(filename);
            var newFileName = Path.GetFileNameWithoutExtension(filename);
            var newExtension = Path.GetExtension(filename);

            if (Prefs.PreserveOriginalCube3 && addMod)
            {
                if (!newFileName.EndsWith("_MOD", StringComparison.CurrentCultureIgnoreCase))
                {
                    newFileName = newPath + "\\" + newFileName + "_MOD" + newExtension;
                }
                else
                {
                    newFileName = filename;
                }
            }
            else
            {
                newFileName = filename;
            }

            if (Prefs.CreateBackupFiles)
            {
                FileBackup.MakeBackup(newFileName, Prefs.MaxNumberBackupFiles);
            }

            try
            {
                File.Delete(newFileName);

                using (outFile = File.OpenWrite(newFileName))
                using (var binaryWriter = new BinaryWriter(outFile))
                {
                    if (extractor == null || Prefs.MinimizeCube3Size)
                    {
                        binaryWriter.Write(encodedBFB);

                        // close the original file
                        // rename original file to .cube3.1 or .2 or .3 (etc)
                        // encode the BFB using blowfish encryption
                        // save the encrypted data to .cube3 
                    }
                    else
                    {
                        extractor.ModelFileSize = 10;

                        Byte[] updatedXMLBytes = ProcessXML(extractor.ModelFiles[extractor.GetXMLFilename()], encodedBFB);

                        binaryWriter.Write(extractor.ModelFileCount);
                        binaryWriter.Write(extractor.ModelFileSize);
                        binaryWriter.Write(extractor.MaxFilenameLengthPlusSize);
                        foreach (String fname in extractor.ModelFileNames)
                        {
                            int lengthWritten = extractor.MaxFilenameLengthPlusSize;

                            Byte[] fnameData = new byte[extractor.ModelFilenameSize];
                            Byte[] fnameBytes = Encoding.ASCII.GetBytes(fname);
                            int fnameLength = extractor.ModelFilenameSize <= fnameBytes.Length ? extractor.ModelFilenameSize : fnameBytes.Length;
                            Array.Copy(fnameBytes, fnameData, fnameLength);


                            if (fname.ToUpper().EndsWith("XML", StringComparison.CurrentCulture))
                            {
                                binaryWriter.Write(updatedXMLBytes.Length);
                                binaryWriter.Write(fnameData, 0, extractor.ModelFilenameSize);
                                binaryWriter.Write(updatedXMLBytes);
                                lengthWritten += updatedXMLBytes.Length;
                            }
                            else if (fname.ToUpper().EndsWith("CUBE3", StringComparison.CurrentCulture))
                            {
                                binaryWriter.Write(encodedBFB.Length);
                                binaryWriter.Write(fnameData, 0, extractor.ModelFilenameSize);
                                binaryWriter.Write(encodedBFB);
                                lengthWritten += encodedBFB.Length;
                            }
                            else
                            {
                                binaryWriter.Write(extractor.ModelFiles[fname].Length);
                                binaryWriter.Write(fnameData, 0, extractor.ModelFilenameSize);
                                binaryWriter.Write(extractor.ModelFiles[fname]);
                                lengthWritten += extractor.ModelFiles[fname].Length;

                            }
                            extractor.ModelFileSize += lengthWritten;
                            lengthWritten = 0;
                        }


                        binaryWriter.Seek(4, SeekOrigin.Begin);
                        binaryWriter.Write(extractor.ModelFileSize);


                    }
                    outFile.Close();

                    fileName = newFileName;

                    char[] seperator = { ' ', '-', ' ' };
                    Text = Text.Split(seperator, StringSplitOptions.RemoveEmptyEntries)[0] + " - " + newFileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to save file [{ex.Message}].  Check permissions and/or attributes");
                if (Prefs.CreateBackupFiles)
                {
                    try
                    {
                        FileBackup.DeleteLastBackup(newFileName);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Unable to remove backup file.  Check permissions and/or attributes");
                    }
                }
            }

        }


        private void AbouitToolStripMenuItem_Click(object sender, EventArgs e)
        {
                AboutWindow aboutWindow = new AboutWindow();
                aboutWindow.Show();
        }

        private int GetModelDataOffset()
        {
            for (int i = 0; i < inputCube3File.Length; i++)
            {
                if (inputCube3File[i] == 0xc8)
                {
                    return i;
                }
            }

            return -1;
        }

        private void CbLeftMaterial_SelectedIndexChanged(object sender, EventArgs e)
        {
            modified = true;
            if (cbLeftMaterial.Text.Equals(BFBConstants.INFRINSE) || cbLeftMaterial.Text.Equals(BFBConstants.CP_INFRINSE))
            {
                cbLeftColor.Text = BFBConstants.INFINITY_RINSE;
                cbLeftColor.Enabled = false;
                cbLeftSupport.Enabled = true;
                cbLeftSidewalk.Enabled = true;
            } 
            else if (cbLeftMaterial.Text.Equals(BFBConstants.EMPTY))
            {
                cbLeftColor.Text = "";
                cbLeftColor.Enabled = false;
                cbLeftSupport.Enabled = false;
                cbLeftSidewalk.Enabled = false;
            }
            else
            {
                if (cbLeftColor.Text.Equals(BFBConstants.INFINITY_RINSE))
                {
                    cbLeftColor.Text = "";
                }
                SetColors(cbLeftMaterial.Text, cbLeftColor);
                cbLeftColor.Enabled = true;
                cbLeftSupport.Enabled = true;
                cbLeftSidewalk.Enabled = true;
            }
            bfbObject.SetMATERIALCODE(BFBConstants.MATERIALCODEE1, cbLeftMaterial.Text, cbLeftColor.Text);
            if (cbLeftSupport.Checked)
            {
                bfbObject.SetSUPPORTS(BFBConstants.getCUBE3Code(cbLeftMaterial.Text, cbLeftColor.Text).ToString());
            }
            if (cbLeftSidewalk.Checked)
            {
                bfbObject.SetSIDEWALKS(BFBConstants.getCUBE3Code(cbLeftMaterial.Text, cbLeftColor.Text).ToString());
            }
        }


        private void CbRightMaterial_SelectedIndexChanged(object sender, EventArgs e)
        {
            modified = true;
            if (cbRightMaterial.Text.Equals(BFBConstants.INFRINSE) || cbRightMaterial.Text.Equals(BFBConstants.CP_INFRINSE))
            {
                cbRightColor.Text = BFBConstants.INFINITY_RINSE;
                cbRightColor.Enabled = false;
                cbRightSupport.Enabled = true;
                cbRightSidewalk.Enabled = true;
            }
            else if (cbRightMaterial.Text.Equals(BFBConstants.EMPTY))
            {
                cbRightColor.Text = "";
                cbRightColor.Enabled = false;
                cbRightSupport.Enabled = false;
                cbRightSidewalk.Enabled = false;
            }
            else
            {
                if (cbRightColor.Text.Equals(BFBConstants.INFINITY_RINSE))
                {
                    cbRightColor.Text = "";
                }
                SetColors(cbRightMaterial.Text, cbRightColor);
                cbRightColor.Enabled = true;
                cbRightSupport.Enabled = true;
                cbRightSidewalk.Enabled = true;
            }
            bfbObject.SetMATERIALCODE(BFBConstants.MATERIALCODEE2, cbRightMaterial.Text, cbRightColor.Text);
            if (cbRightSupport.Checked)
            {
                bfbObject.SetSUPPORTS(BFBConstants.getCUBE3Code(cbRightMaterial.Text, cbRightColor.Text).ToString());
            }
            if (cbRightSidewalk.Checked)
            {
                bfbObject.SetSIDEWALKS(BFBConstants.getCUBE3Code(cbRightMaterial.Text, cbRightColor.Text).ToString());
            }
        }

        private void CbCubeProMaterial_SelectedIndexChanged(object sender, EventArgs e)
        {
            modified = true;
            if (cbCubeProMaterial.Text.Equals(BFBConstants.INFRINSE) || cbCubeProMaterial.Text.Equals(BFBConstants.CP_INFRINSE))
            {
                cbCubeProColor.Text = BFBConstants.INFINITY_RINSE;
                cbCubeProColor.Enabled = false;
                cbCubeProSupport.Enabled = true;
                cbCubeProSidewalk.Enabled = true;
            }
            else if (cbCubeProMaterial.Text.Equals(BFBConstants.EMPTY))
            {
                cbCubeProColor.Text = "";
                cbCubeProColor.Enabled = false;
                cbCubeProSupport.Enabled = false;
                cbCubeProSidewalk.Enabled = false;
            }
            else
            {
                if (cbCubeProColor.Text.Equals(BFBConstants.INFINITY_RINSE))
                {
                    cbCubeProColor.Text = "";
                }
                SetColors(cbCubeProMaterial.Text, cbCubeProColor);
                cbCubeProColor.Enabled = true;
                cbCubeProSupport.Enabled = true;
                cbCubeProSidewalk.Enabled = true;
            }
            bfbObject.SetMATERIALCODE(BFBConstants.MATERIALCODEE3, cbCubeProMaterial.Text, cbCubeProColor.Text);
            if (cbCubeProSupport.Checked)
            {
                bfbObject.SetSUPPORTS(BFBConstants.getCUBE3Code(cbCubeProMaterial.Text, cbCubeProColor.Text).ToString());
            }
            if (cbCubeProSidewalk.Checked)
            {
                bfbObject.SetSIDEWALKS(BFBConstants.getCUBE3Code(cbCubeProMaterial.Text, cbCubeProColor.Text).ToString());
            }
        }
        private void SetColors(string text, System.Windows.Forms.ComboBox cbColor)
        {
            if (text.Equals(BFBConstants.ABS))
            {
                cbColor.Items.Clear();
                cbColor.Text = "";
                foreach (String color in BFBConstants.cube3ABSColors)
                    cbColor.Items.Add(color);
            }
            if (text.Equals(BFBConstants.CP_ABS))
            {
                cbColor.Items.Clear();
                cbColor.Text = "";
                foreach (String color in BFBConstants.cubeProABSColors)
                    cbColor.Items.Add(color);
            }
            else if (text.Equals(BFBConstants.PLA))
            {
                cbColor.Items.Clear();
                cbColor.Text = "";
                foreach (String color in BFBConstants.cube3PLAColors)
                    cbColor.Items.Add(color);
            }
            else if (text.Equals(BFBConstants.CP_PLA))
            {
                cbColor.Items.Clear();
                cbColor.Text = "";
                foreach (String color in BFBConstants.cubeProPLAColors)
                    cbColor.Items.Add(color);
            }
            else if (text.Equals(BFBConstants.EKO))
            {
                cbColor.Items.Clear();
                cbColor.Text = "";
                foreach (String color in BFBConstants.ekoCycleColors)
                    cbColor.Items.Add(color);
            }


        }


        private void CbLeftColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            modified = true;
            bfbObject.SetMATERIALCODE(BFBConstants.MATERIALCODEE1, cbLeftMaterial.Text, cbLeftColor.Text);
        }

        private void CbRightColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            modified = true;
            bfbObject.SetMATERIALCODE(BFBConstants.MATERIALCODEE2, cbRightMaterial.Text, cbRightColor.Text);
        }

        private void CbCubeProColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            bfbObject.SetMATERIALCODE(BFBConstants.MATERIALCODEE3, cbCubeProMaterial.Text, cbCubeProColor.Text);
            modified = true;
        }


        private void AddOrRemoveNatural(System.Windows.Forms.ComboBox comboBox, System.Windows.Forms.ComboBox cbColor)
        {
            bool doAddNatural = true;
            if (String.Compare(comboBox.SelectedItem.ToString(), BFBConstants.PLA) == 0)
            {
                if (!cbColor.Items.Contains(BFBConstants.NATURAL))
                {
                    doAddNatural = true;
                }
            }
            else if (String.Compare(comboBox.SelectedItem.ToString(), BFBConstants.ABS) == 0)
            {
                if (cbColor.Items.Contains(BFBConstants.NATURAL))
                {
                    doAddNatural = false;
                }
            }

            if (!doAddNatural)
                cbColor.Items.Remove(BFBConstants.NATURAL);
            else
                cbColor.Items.Add(BFBConstants.NATURAL);

        }

        bool prepared = false;

        private void PrepareForm()
        {
            if (prepared)
                return;

            prepared = true;

            Border = new RectangleBorder(
                new BorderLine(Color.DarkGreen),
                new BorderLine(Color.DarkGreen));

            DevAge.Drawing.VisualElements.ColumnHeader flatHeader = new DevAge.Drawing.VisualElements.ColumnHeader();
            flatHeader.Border = Border;
            flatHeader.BackgroundColorStyle = DevAge.Drawing.BackgroundColorStyle.Solid;
            flatHeader.BackColor = Color.ForestGreen;
            SourceGrid.Cells.Views.ColumnHeader headerView = new SourceGrid.Cells.Views.ColumnHeader();


            headerView.Font = new Font(gridLeftTemps.Font, FontStyle.Bold | FontStyle.Underline);
            headerView.Background = flatHeader;
            headerView.ForeColor = Color.White;

            TempView = new SourceGrid.Cells.Views.Cell();
            TempView.Background = new DevAge.Drawing.VisualElements.BackgroundLinearGradient(Color.ForestGreen, Color.White, 45);
            TempView.Border = Border;

            gridLeftTemps.ColumnsCount = 5;
            gridLeftTemps.FixedRows = 1;
            gridLeftTemps.Rows.Insert(0);

            gridLeftTemps[0, 0] = new SourceGrid.Cells.ColumnHeader("Temp");
            gridLeftTemps[0, 0].View = headerView;
            gridLeftTemps[0, 1] = new SourceGrid.Cells.ColumnHeader("Count");
            gridLeftTemps[0, 1].View = headerView;
            gridLeftTemps[0, 2] = new SourceGrid.Cells.ColumnHeader("Modifier");
            gridLeftTemps[0, 2].View = headerView;
            gridLeftTemps[0, 3] = new SourceGrid.Cells.ColumnHeader("Modifier Type");
            gridLeftTemps[0, 3].View = headerView;
            gridLeftTemps[0, 4] = new SourceGrid.Cells.ColumnHeader("Adj Temp");
            gridLeftTemps[0, 4].View = headerView;
            gridLeftTemps.AutoSize = true;
            gridLeftTemps.AutoSizeCells();
            gridLeftTemps.Columns.StretchToFit();
            gridLeftTemps.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            gridRightTemps.ColumnsCount = 5;
            gridRightTemps.FixedRows = 1;
            gridRightTemps.Rows.Insert(0);

            gridRightTemps[0, 0] = new SourceGrid.Cells.ColumnHeader("Temp");
            gridRightTemps[0, 0].View = headerView;
            gridRightTemps[0, 1] = new SourceGrid.Cells.ColumnHeader("Count");
            gridRightTemps[0, 1].View = headerView;
            gridRightTemps[0, 2] = new SourceGrid.Cells.ColumnHeader("Modifier");
            gridRightTemps[0, 2].View = headerView;
            gridRightTemps[0, 3] = new SourceGrid.Cells.ColumnHeader("Modifier Type");
            gridRightTemps[0, 3].View = headerView;
            gridRightTemps[0, 4] = new SourceGrid.Cells.ColumnHeader("AdjTemp");
            gridRightTemps[0, 4].View = headerView;
            gridRightTemps.AutoSize = true;
            gridRightTemps.AutoSizeCells();
            gridRightTemps.Columns.StretchToFit();
            gridRightTemps.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            gridRetractionStart.ColumnsCount = 5;
            gridRetractionStart.FixedRows = 1;
            gridRetractionStart.Rows.Insert(0);

            gridRetractionStart[0, 0] = new SourceGrid.Cells.ColumnHeader("P Value");
            gridRetractionStart[0, 0].View = headerView;
            gridRetractionStart[0, 1] = new SourceGrid.Cells.ColumnHeader("S Value");
            gridRetractionStart[0, 1].View = headerView;
            gridRetractionStart[0, 2] = new SourceGrid.Cells.ColumnHeader("G Value");
            gridRetractionStart[0, 2].View = headerView;
            gridRetractionStart[0, 3] = new SourceGrid.Cells.ColumnHeader("F Value");
            gridRetractionStart[0, 3].View = headerView;
            gridRetractionStart[0, 4] = new SourceGrid.Cells.ColumnHeader("1st Index");
            gridRetractionStart[0, 4].View = headerView;
            gridRetractionStart.AutoSize = true;
            gridRetractionStart.AutoSizeCells();
            gridRetractionStart.Columns.StretchToFit();
            gridRetractionStart.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            gridRetractionStop.ColumnsCount = 3;
            gridRetractionStop.FixedRows = 1;
            gridRetractionStop.Rows.Insert(0);

            gridRetractionStop[0, 0] = new SourceGrid.Cells.ColumnHeader("P Value");
            gridRetractionStop[0, 0].View = headerView;
            gridRetractionStop[0, 1] = new SourceGrid.Cells.ColumnHeader("S Value");
            gridRetractionStop[0, 1].View = headerView;
            gridRetractionStop[0, 2] = new SourceGrid.Cells.ColumnHeader("1st Index");
            gridRetractionStop[0, 2].View = headerView;
            gridRetractionStop.AutoSize = true;
            gridRetractionStop.AutoSizeCells();
            gridRetractionStop.Columns.StretchToFit();
            gridRetractionStop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            gridPressure.ColumnsCount = 4;
            gridPressure.FixedRows = 1;
            gridPressure.Rows.Insert(0);

            gridPressure[0, 0] = new SourceGrid.Cells.ColumnHeader("Selected");
            gridPressure[0, 0].View = headerView;
            gridPressure[0, 1] = new SourceGrid.Cells.ColumnHeader("Extruder Pressure");
            gridPressure[0, 1].View = headerView;
            gridPressure[0, 2] = new SourceGrid.Cells.ColumnHeader("Calculated Pressure");
            gridPressure[0, 2].View = headerView;
            gridPressure[0, 3] = new SourceGrid.Cells.ColumnHeader("1st Index");
            gridPressure[0, 3].View = headerView;
            gridPressure.AutoSize = true;
            gridPressure.AutoSizeCells();
            gridPressure.Columns.StretchToFit();
            gridPressure.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            cbFirmware.Enabled = false;
            cbMinFirmware.Enabled = false;
            cbPrinterModel.Enabled = false;

        }

        private void MainEditor_Load(object sender, EventArgs e)
        {
            PrepareForm();
        }

        private void BtnLeftCalculate_Click(object sender, EventArgs e)
        {
            modified = true;
            CalculateTemperatures(gridLeftTemps);
        }

        private void BtnRightCalculate_Click(object sender, EventArgs e)
        {
            modified = true;
            CalculateTemperatures(gridRightTemps);
        }

        private void BtnLeftTempUpdate_Click(object sender, EventArgs e)
        {
            modified = true;
            UpdateTemperaturesInBFB(BFBConstants.LEFT_TEMP, gridLeftTemps);
        }

        private void BtnRightTempUpdate_Click(object sender, EventArgs e)
        {
            modified = true;
            UpdateTemperaturesInBFB(BFBConstants.RIGHT_TEMP, gridRightTemps);
        }

        private void cbFirware_TextChanged(object sender, EventArgs e)
        {
            if (cbFirmware.Text.Length > 0)
            {
                modified = true;
                bfbObject.SetFIRMWARE(cbFirmware.Text);
            }
        }

        private void CbMinFirmware_TextChanged(object sender, EventArgs e)
        {
            if (cbMinFirmware.Text.Length > 0)
            {
                modified = true;
                bfbObject.SetMINFIRMWARE(cbMinFirmware.Text);
            }
        }

        private void CbPrinterModel_TextChanged(object sender, EventArgs e)
        {
            if (cbPrinterModel.Text.Length > 0)
            {
                modified = true;
                bfbObject.SetPRINTERMODEL(cbPrinterModel.Text);
            }
        }

        private void BtnViewRaw_Click(object sender, EventArgs e)
        {
            if (bfbObject != null)
            {
                FrmRawView rawViewForm = new FrmRawView(bfbObject.BfbLines);
                // display bfb data in a text box.  no editing allowed.
                rawViewForm.ShowDialog();

                if (rawViewForm.ApplyChanges)
                {
                    string rawBFBText = rawViewForm.rtbRawView.Text;

                    ProcessAndLoadBFB(encoding.GetBytes(rawBFBText));
                }
            }
            else
            {
                MessageBox.Show("Please open a Cube model.", "No Model Present", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        bool validData;

        protected bool GetFilename(out string filename, DragEventArgs e)
        {
            bool ret = false;
            filename = String.Empty;

            if ((e.AllowedEffect & DragDropEffects.Copy) == DragDropEffects.Copy)
            {
                string[] fileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);

                if (fileList != null)
                {
                    if ((fileList.Length == 1) && (fileList[0] is String))
                    {
                        filename = fileList[0];
                        string ext = Path.GetExtension(filename).ToLower();
                        if ((ext == ".cube3") || (ext == ".cubepro"))
                        {
                            ret = true;
                        }
                    }
                }
            }
            return ret;
        }
        private void MainEditor_DragEnter(object sender, DragEventArgs e)
        {
            string filename;
            validData = GetFilename(out filename, e);
            if (validData)
            {
                try
                {
                    LoadFile(filename);
                    e.Effect = DragDropEffects.Copy;
                }
                catch
                {
                    validData = false;
                    e.Effect = DragDropEffects.None;
                }
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void MainEditor_DragDrop(object sender, DragEventArgs e)
        {
        }


        internal bool CommandLineLoad(String newFWStr, String newMinFWStr, String newPrinterModel, String newMCE1, String newMCE2, String newMCE3,
            List<TemperatureModifier> tempMods, List<RetractModifier> retractStartMods, List<RetractModifier> retractStopMods,
            List<PressureModifier> pressureMods)
        {
            bool success = true;

            if (newFWStr != null && newFWStr.Length > 0)
            {
                cbFirmware.Text = newFWStr;
            }

            if (newMinFWStr != null && newMinFWStr.Length > 0)
            {
                cbMinFirmware.Text = newMinFWStr;
            }

            if (newPrinterModel != null && newPrinterModel.Length > 0)
            {
                cbPrinterModel.Text = newPrinterModel;
            }

            int.TryParse(newMCE1, out int cubeCode);
            bfbObject.SetMATERIALCODE(BFBConstants.MATERIALCODEE1, cubeCode);
            int.TryParse(newMCE2, out cubeCode);
            bfbObject.SetMATERIALCODE(BFBConstants.MATERIALCODEE2, cubeCode);
            int.TryParse(newMCE3, out cubeCode);
            bfbObject.SetMATERIALCODE(BFBConstants.MATERIALCODEE3, cubeCode);

            Dictionary<int, int> leftTempDict = new Dictionary<int, int>();
            Dictionary<int, int> rightTempDict = new Dictionary<int, int>();
            //Dictionary<int, int> midTempDict = new Dictionary<int, int>();

            if (tempMods.Count > 0)
            {
                // process temperatures
                for (int i = 1; i < gridLeftTemps.Rows.Count; i++)
                {
                    leftTempDict.Add((int)gridLeftTemps[i, 0].Value, i);
                }
                for (int i = 1; i < gridRightTemps.Rows.Count; i++)
                {
                    rightTempDict.Add((int)gridRightTemps[i, 0].Value, i);
                }
                //for (int i = 1; i < gridMidTemps.Rows.Count; i++)
                //{
                //    midTempDict.Add((int)gridMidTemps[i, 0].Value, i);
                //}

                foreach (TemperatureModifier tempMod in tempMods)
                {
                    int i;
                    if (tempMod.extruder.Equals(BFBConstants.LEFT_TEMP))
                    {
                        if (leftTempDict.Keys.Contains(tempMod.oldValue))
                        {
                            i = leftTempDict[tempMod.oldValue];

                            // set row to replace and replace value to newvalue
                            gridLeftTemps[i, 2].Value = tempMod.newValue;
                            gridLeftTemps[i, 3].Value = "Replace";
                        }
                    }
                    else if (tempMod.extruder.Equals(BFBConstants.RIGHT_TEMP))
                    {
                        if (rightTempDict.Keys.Contains(tempMod.oldValue))
                        {
                            i = rightTempDict[tempMod.oldValue];

                            // set row to replace and replace value to newvalue
                            gridRightTemps[i, 2].Value = tempMod.newValue;
                            gridRightTemps[i, 3].Value = "Replace";
                        }
                    }
                    else if (tempMod.extruder.Equals(BFBConstants.MID_TEMP))
                    {
                        /* TODO: Update when I have MIDTEMP working....
                        if (midTempDict.Keys.Contains(tempMod.oldValue))
                        {
                            i = midTempDict[tempMod.oldValue];

                            // set row to replace and replace value to newvalue
                            gridMidTemps[i, 2].Value = tempMod.newValue;
                            gridMidTemps[i, 3].Value = "Replace";
                        }
                        */
                    }
                }
                CalculateTemperatures(gridLeftTemps);
                UpdateTemperaturesInBFB(BFBConstants.LEFT_TEMP, gridLeftTemps);
                CalculateTemperatures(gridRightTemps);
                UpdateTemperaturesInBFB(BFBConstants.RIGHT_TEMP, gridRightTemps);
                //CalculateTemperatures(gridMidTemps);
                //UpdateTemperatures(gridMidTemps);
            }

            if (retractStartMods.Count > 0)
            {
                // process retract start
                Dictionary<int, int> retractStartDict = new Dictionary<int, int>();
                for (int i = 1; i < gridRetractionStart.Rows.Count; i++)
                {
                    if (gridRetractionStart[i, 0].Value.Equals(gridRetractionStart[i, 1].Value))
                    {
                        retractStartDict.Add((int)gridRetractionStart[i, 0].Value, i);
                    }
                }

                foreach (RetractModifier retractMod in retractStartMods)
                {
                    if (retractStartDict.Keys.Contains(retractMod.oldRetractValue))
                    {
                        int i = retractStartDict[retractMod.oldRetractValue];
                        gridRetractionStart[i, 0].Value = retractMod.newPValue;
                        gridRetractionStart[i, 1].Value = retractMod.newSValue;
                        if (retractMod.newGValue >= 0)
                        {
                            gridRetractionStart[i, 2].Value = retractMod.newGValue;
                        }
                        if (retractMod.newFValue >= 0)
                        {
                            gridRetractionStart[i, 3].Value = retractMod.newFValue;
                        }
                    }
                }

                UpdateStartRetractions(gridRetractionStart);
            }

            if (retractStopMods.Count > 0)
            {
                // process retract stop
                Dictionary<int, int> retractStopDict = new Dictionary<int, int>();
                for (int i = 1; i < gridRetractionStop.Rows.Count; i++)
                {
                    retractStopDict.Add((int)gridRetractionStop[i, 1].Value, i);
                }

                foreach (RetractModifier retractMod in retractStopMods)
                {
                    if (retractStopDict.Keys.Contains(retractMod.oldRetractValue))
                    {
                        int i = retractStopDict[retractMod.oldRetractValue];
                        gridRetractionStop[i, 1].Value = retractMod.newSValue;
                    }
                }
                UpdateStopRetractions(gridRetractionStop);
            }

            if (pressureMods.Count > 0)
            {
                // process pressures
                Dictionary<double, int> pressureDict = new Dictionary<double, int>();
                for (int i = 1; i < gridPressure.Rows.Count; i++)
                {
                    pressureDict.Add((double)gridPressure[i, 1].Value, i);
                }

                foreach (PressureModifier pressureMod in pressureMods)
                {
                    if (pressureDict.Keys.Contains(pressureMod.oldPressureValue))
                    {
                        int i = pressureDict[pressureMod.oldPressureValue];

                        // set row to replace and replace value to newvalue
                        gridPressure[i, 1].Value = (double)pressureMod.newPressureValue;
                    }
                }
                UpdatePressures();
            }

            return success;
        }

        private void GenerateScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (bfbObject == null)
            {
                MessageBox.Show("Please load a script or generate a script from a Cube model.", "No Model Present", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                cubeScript = new CubeScript(bfbObject);
                StreamWriter scriptFile = null;
                try
                {
                    saveScriptDialog.FileName = Path.GetFileNameWithoutExtension(fileName) + ".CUBESCR";
                    if (saveScriptDialog.ShowDialog() == DialogResult.OK)
                    {
                        scriptFile = new System.IO.StreamWriter(saveScriptDialog.FileName, false);

                        foreach (String line in cubeScript.CubeScriptLines)
                        {
                            scriptFile.WriteLine(line);
                        }

                        scriptFile.Close();
                        scriptFile = null;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to save script [" + saveScriptDialog.FileName + "]", ex.Message);
                }
                finally
                {
                    if (scriptFile != null)
                        scriptFile.Close();
                }

            }

        }

        private void LoadScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (bfbObject == null)
            {
                MessageBox.Show("Please load a Cube model.", "No Model Present", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                try
                {
                    if (fdLoadScript.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            cubeScript = new CubeScript(fdLoadScript.FileName);
                            CommandLineLoad(cubeScript.FirmwareStr, cubeScript.MinFirmwareStr, cubeScript.PrinterModelStr,
                               cubeScript.MaterialE1Str, cubeScript.MaterialE2Str, cubeScript.MaterialE3Str,
                               cubeScript.TemperatureModifers, cubeScript.RetractStartModifers, cubeScript.RetractStopModifers,
                               cubeScript.PressureModifiers);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Unable to open {fdLoadScript.FileName}.  Reason: [{ex.InnerException.Message}]");
                        }
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Unable to open file [" + openFileDialog.FileName + "]", "Invalid CUBESCRIPT File!");
                }
                finally
                {
                }
            }
        }

        private void BtnViewScript_Click(object sender, EventArgs e)
        {
            if (cubeScript == null)
            {
                MessageBox.Show("Please load a script or generate a script from a Cube model.", "No Model Present", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                ScriptViewForm scriptViewForm = new ScriptViewForm(cubeScript.CubeScriptLines);
                // display bfb data in a text box.  no editing allowed.
                scriptViewForm.ShowDialog();

                if (scriptViewForm.applyScript)
                {
                    string scriptText = scriptViewForm.rtbScript.Text;
                    string[] separator = { "\r\n", "\n", "\r" };
                    string[] scriptLineArray = scriptText.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                    cubeScript = new CubeScript(scriptLineArray);
                    CommandLineLoad(cubeScript.FirmwareStr, cubeScript.MinFirmwareStr, cubeScript.PrinterModelStr,
                       cubeScript.MaterialE1Str, cubeScript.MaterialE2Str, cubeScript.MaterialE3Str,
                       cubeScript.TemperatureModifers, cubeScript.RetractStartModifers, cubeScript.RetractStopModifers,
                       cubeScript.PressureModifiers);
                }
            }

        }


        private void PreferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            preferencesDialog.ShowDialog();
            prefs.Load();
        }

        private void CbLeftSidewalk_CheckedChanged(object sender, EventArgs e)
        {
            if (cbLeftSidewalk.Checked == true)
            {
                cbCubeProSidewalk.Checked = false;
                cbRightSidewalk.Checked = false;
            }
        }

        private void CbMidSidewalk_CheckedChanged(object sender, EventArgs e)
        {
            if (cbCubeProSidewalk.Checked == true)
            {
                cbLeftSidewalk.Checked = false;
                cbRightSidewalk.Checked = false;
            }
        }

        private void CbRightSidewalk_CheckedChanged(object sender, EventArgs e)
        {
            if (cbRightSidewalk.Checked)
            {
                cbLeftSidewalk.Checked = false;
                cbCubeProSidewalk.Checked = false;
            }
        }

        private void CbLeftSupport_CheckedChanged(object sender, EventArgs e)
        {
            if (cbLeftSupport.Checked)
            {
                cbCubeProSupport.Checked = false;
                cbRightSupport.Checked = false;
            }
        }

        private void CbMidSupport_CheckedChanged(object sender, EventArgs e)
        {
            if (cbCubeProSupport.Checked)
            {
                cbLeftSupport.Checked = false;
                cbRightSupport.Checked = false;
            }
        }

        private void CbRightSupport_CheckedChanged(object sender, EventArgs e)
        {
            if (cbRightSupport.Checked)
            {
                cbLeftSupport.Checked = false;
                cbCubeProSupport.Checked = false;
            }
        }

        private void CbPrinterModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbPrinterModel.Text.Length > 0)
            {
                if (cbPrinterModel.Text.ToUpper().Equals(BFBConstants.CUBE3_MODEL))
                {
                    cbFirmware.Text = BFBConstants.CUBE3_VERSION;
                    cbMinFirmware.Text = BFBConstants.CUBE3_VERSION;
                }

                else if (cbPrinterModel.Text.ToUpper().Equals(BFBConstants.EKO_MODEL))
                {
                    cbFirmware.Text = BFBConstants.EKO_VERSION;
                    cbMinFirmware.Text = BFBConstants.EKO_VERSION;
                }

                else if (cbPrinterModel.Text.ToUpper().Equals(BFBConstants.CUBEPRO_MODEL))
                {
                    cbFirmware.Text = BFBConstants.CUBEPRO_VERSION;
                    cbMinFirmware.Text = BFBConstants.CUBEPRO_VERSION;
                }
                else
                {
                    cbFirmware.Text = "";
                    cbMinFirmware.Text = "";
                }
                modified = true;
                bfbObject.SetPRINTERMODEL(cbPrinterModel.Text);
                bfbObject.SetFIRMWARE(cbFirmware.Text);
                bfbObject.SetMINFIRMWARE(cbMinFirmware.Text);
            }
        }

        private void CbFirmware_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbFirware_TextChanged(sender, e);
        }

        private void CbMinFirmware_SelectedIndexChanged(object sender, EventArgs e)
        {
            CbMinFirmware_TextChanged(sender, e);
        }

        private void GridPressureSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 1; i < gridPressure.Rows.Count; i++)
            {
                if ((bool)(gridPressure[i, 0].Value) == false)
                {
                    gridPressure[i, 0].Value = true;
                }
            }

        }

        private void GridPressureClearSelection_Click(object sender, EventArgs e)
        {
            for (int i = 1; i < gridPressure.Rows.Count; i++)
            {
                if ((bool)(gridPressure[i, 0].Value) == true) {
                    gridPressure[i, 0].Value = false;
                }
            }
        }

        private void BtnApplyChange_Click(object sender, EventArgs e)
        {
            CalculatePressures((double)nudPressureMod.Value, false);
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            CalculatePressures((double)nudPressureMod.Value, true);
        }

        private void XMLViewerShow(object sender, EventArgs e)
        {

            if (extractor != null)
            {
                Byte[] xmlBytes = extractor.ModelFiles[extractor.GetXMLFilename()];

                string xmlString = encoding.GetString(xmlBytes);
                var xDoc = XDocument.Parse(xmlString);

                XmlViewForm xmlViewForm = new XmlViewForm(xDoc);

                xmlViewForm.ShowDialog();

                //if (rawViewForm.ApplyChanges)
                //{
                //    string rawBFBText = rawViewForm.rtbRawView.Text;

                //    ProcessAndLoadBFB(encoding.GetBytes(rawBFBText));
                //}
            }
            else
            {
                MessageBox.Show("Please open a Cube model.", "No Model Present", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }
    }
}
