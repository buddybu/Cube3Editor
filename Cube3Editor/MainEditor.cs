using DevAge.Drawing;
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

namespace Cube3Editor
{
    public partial class MainEditor : Form
    {

        bool modified = false;

        string fileName;
        Stream inFile;
        Stream outFile;

        BitFromByte bfbObject;

        BlowfishEngine engine;
        byte[] inputCube3File;
        byte[] outputCube3File;

        private string key = "221BBakerMycroft";

        static readonly Encoding encoding = Encoding.ASCII;
        private String decodedModel;

        private Byte[] dataModel;
        private RectangleBorder border;
        private SourceGrid.Cells.Views.Cell tempView;

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

        public MainEditor()
        {
            InitializeComponent();

            openFileDialog.Filter = "CUBE Files |*.cube3;*.cubepro|All Files (*.*)|*.*";
            openFileDialog.Title = "Please Select a CUBE3 File to edit.";

            engine = new BlowfishEngine(true);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

            try
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    ClearUI();
                    LoadFile(openFileDialog.FileName);
                    fileName = openFileDialog.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to open file [" + openFileDialog.FileName + "]", "Invalid CUBE File!");
            }
            finally
            {
                if (inFile != null)
                    inFile.Close();
            }

        }

        private void LoadFile(String FileName)
        {
            PaddedBufferedBlockCipher cipher;
            if (FileName != null || FileName.Length > 0)
            {
                ClearUI();
                try
                {
                    using (inFile = File.OpenRead(FileName))
                    using (var binaryReader = new BinaryReader(inFile))
                    {
                        inputCube3File = binaryReader.ReadBytes((int)binaryReader.BaseStream.Length);
                        int modelDataOffset = 0;
                        if (validCube3Header(inputCube3File))
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

                            Text = Text + " - " + openFileDialog.FileName;

                            decodedModel = encoding.GetString(decodedBytes);

                            bfbObject = new BitFromByte(encoding, decodedBytes);

                            string[] seperator = new string[] { "\r\n" };
                            string[] decodedModelArray = decodedModel.Split(seperator, StringSplitOptions.RemoveEmptyEntries);

                            List<String> bfbStringList = decodedModelArray.ToList<string>();


                            // populate all fields
                            tbFirmware.Text = bfbObject.GetText(BFBConstants.FIRMWARE);

                            tbMinFirmware.Text = bfbObject.GetText(BFBConstants.MINFIRMWARE);
                            tbPrinterModel.Text = bfbObject.GetText(BFBConstants.PRINTERMODEL);
                            cbLeftMaterial.Text = bfbObject.GetMaterialType(BFBConstants.MATERIALCODEE1);
                            cbLeftColor.Text = bfbObject.GetMaterialColor(BFBConstants.MATERIALCODEE1);
                            cbRightMaterial.Text = bfbObject.GetMaterialType(BFBConstants.MATERIALCODEE2);
                            cbRightColor.Text = bfbObject.GetMaterialColor(BFBConstants.MATERIALCODEE2);
                            cbCubeProMaterial.Text = bfbObject.GetMaterialType(BFBConstants.MATERIALCODEE3);
                            cbCubeProColor.Text = bfbObject.GetMaterialColor(BFBConstants.MATERIALCODEE3);

                            bfbObject.PopulateTemperatures(BFBConstants.LEFT_TEMP, gridLeftTemps, border);
                            bfbObject.PopulateTemperatures(BFBConstants.RIGHT_TEMP, gridRightTemps, border);
                            bfbObject.PopulateRetractions(BFBConstants.RETRACT_START, gridRetraction, border);

                            // enable material fields
                            tbFirmware.Enabled = true;
                            tbMinFirmware.Enabled = true;
                            tbPrinterModel.Enabled = true;
                            cbRightMaterial.Enabled = true;
                            cbRightColor.Enabled = true;
                            cbLeftColor.Enabled = true;
                            cbLeftMaterial.Enabled = true;
                            cbCubeProColor.Enabled = true;
                            cbCubeProMaterial.Enabled = true;

                            // clear modified flag
                            modified = false;

                        }
                        catch (IOException ex)
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


        private bool validCube3Header(byte[] inputCube3File)
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

        private void ClearUI()
        {
            tbFirmware.Text = "";
            tbMinFirmware.Text = "";
            tbPrinterModel.Text = "";

            cbLeftColor.Text = "";
            cbLeftMaterial.Text = "";
            cbRightColor.Text = "";
            cbRightMaterial.Text = "";

            if (gridLeftTemps.Rows.Count > 1)
                gridLeftTemps.Rows.RemoveRange(1, gridLeftTemps.Rows.Count - 1);
            if (gridRightTemps.Rows.Count > 1)
                gridRightTemps.Rows.RemoveRange(1, gridRightTemps.Rows.Count - 1);
            if (gridRetraction.Rows.Count > 1)
                gridRetraction.Rows.RemoveRange(1, gridRetraction.Rows.Count - 1);

            char[] seperator = new char[] { ' ', '-', ' ' };
            string title = Text.Split(seperator)[0];
            Text = title;
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (modified)
            {
                // display dialog box that something has changed.  Are you sure you want to close the file?
                // if yes, then close the file.
                // If no, then call the save file routine
                // reset all controls
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (modified)
            {

                inFile.Close();

                FileHelper.MakeBackup(fileName, 5);

                Byte[] newDataModel = bfbObject.getBytesFromBFB();
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


                using (outFile = File.OpenWrite(fileName))
                using (var binaryWriter = new BinaryWriter(outFile))
                {
                    binaryWriter.Write(encodedBytes);

                    outFile.Close();
                    // close the original file
                    // rename original file to .cube3.1 or .2 or .3 (etc)
                    // encode the BFB using blowfish encryption
                    // save the encrypted data to .cube3 
                }
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // disoplay save as dialog with cube3 as the default extension and the current
            // filename as the default name.
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (modified)
            {
                // show dialog box that something has changed and ask them
                // if they really want to quit.
            }
        }

        private void abouitToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private int GetModelDataOffset(byte[] inputCube3File)
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

        private void cbLeftMaterial_SelectedIndexChanged(object sender, EventArgs e)
        {
            modified = true;
            AddOrRemoveNatural(cbLeftMaterial, cbLeftColor);
            bfbObject.SetMATERIALCODE(BFBConstants.MATERIALCODEE1, cbLeftMaterial.Text, cbLeftColor.Text);
        }


        private void cbRightMaterial_SelectedIndexChanged(object sender, EventArgs e)
        {
            modified = true;
            AddOrRemoveNatural(cbRightMaterial, cbRightColor);
            bfbObject.SetMATERIALCODE(BFBConstants.MATERIALCODEE2, cbRightMaterial.Text, cbRightColor.Text);
        }

        private void cbCubeProMaterial_SelectedIndexChanged(object sender, EventArgs e)
        {
            modified = true;
            AddOrRemoveNatural(cbCubeProMaterial, cbCubeProColor);
            bfbObject.SetMATERIALCODE(BFBConstants.MATERIALCODEE3, cbCubeProMaterial.Text, cbCubeProColor.Text);
        }

        private void cbLeftColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            modified = true;
            bfbObject.SetMATERIALCODE(BFBConstants.MATERIALCODEE1, cbLeftMaterial.Text, cbLeftColor.Text);
        }

        private void cbRightColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            modified = true;
            bfbObject.SetMATERIALCODE(BFBConstants.MATERIALCODEE2, cbRightMaterial.Text, cbRightColor.Text);
        }

        private void cbCubeProColor_SelectedIndexChanged(object sender, EventArgs e)
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



        private void MainEditor_Load(object sender, EventArgs e)
        {
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

            gridRetraction.ColumnsCount = 5;
            gridRetraction.FixedRows = 1;
            gridRetraction.Rows.Insert(0);

            gridRetraction[0, 0] = new SourceGrid.Cells.ColumnHeader("P Value");
            gridRetraction[0, 0].View = headerView;
            gridRetraction[0, 1] = new SourceGrid.Cells.ColumnHeader("S Value");
            gridRetraction[0, 1].View = headerView;
            gridRetraction[0, 2] = new SourceGrid.Cells.ColumnHeader("G Value");
            gridRetraction[0, 2].View = headerView;
            gridRetraction[0, 3] = new SourceGrid.Cells.ColumnHeader("F Value");
            gridRetraction[0, 3].View = headerView;
            gridRetraction[0, 4] = new SourceGrid.Cells.ColumnHeader("1st Index");
            gridRetraction[0, 4].View = headerView;
            gridRetraction.AutoSize = true;
            gridRetraction.AutoSizeCells();
            gridRetraction.Columns.StretchToFit();
            gridRetraction.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;


        }

        private void btnLeftCalculate_Click(object sender, EventArgs e)
        {
            modified = true;
            bfbObject.CalculateTemperatures(gridLeftTemps);
        }

        private void btnRightCalculate_Click(object sender, EventArgs e)
        {
            modified = true;
            bfbObject.CalculateTemperatures(gridRightTemps);
        }

        private void btnLeftTempUpdate_Click(object sender, EventArgs e)
        {
            modified = true;
            bfbObject.UpdateTemperatures(BFBConstants.LEFT_TEMP, gridLeftTemps);
        }

        private void btnRightTempUpdate_Click(object sender, EventArgs e)
        {
            modified = true;
            bfbObject.UpdateTemperatures(BFBConstants.RIGHT_TEMP, gridRightTemps);
        }

        private void tbFirware_TextChanged(object sender, EventArgs e)
        {
            if (tbFirmware.Text.Length > 0)
            {
                modified = true;
                bfbObject.SetFIRMWARE(tbFirmware.Text);
            }
        }

        private void tbMinFirmware_TextChanged(object sender, EventArgs e)
        {
            if (tbMinFirmware.Text.Length > 0)
            {
                modified = true;
                bfbObject.SetMINFIRMWARE(tbMinFirmware.Text);
            }
        }

        private void tbPrinterModel_TextChanged(object sender, EventArgs e)
        {
            if (tbPrinterModel.Text.Length > 0)
            {
                modified = true;
                bfbObject.SetPRINTERMODEL(tbPrinterModel.Text);
            }
        }

        private void btnViewRaw_Click(object sender, EventArgs e)
        {
            FrmRawView rawViewForm = new FrmRawView(bfbObject.BfbStringList);
            // display bfb data in a text box.  no editing allowed.
            rawViewForm.ShowDialog();
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

    }
}
