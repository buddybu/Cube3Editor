using BitForByteSupport;
using DevAge.Drawing;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using SourceGrid;
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

        public MainEditor(string cubeFile)
        {
            InitializeComponent();

            openFileDialog.Filter = "CUBE Files |*.cube3;*.cubepro|All Files (*.*)|*.*";
            openFileDialog.Title = "Please Select a CUBE3 File to edit.";

            engine = new BlowfishEngine(true);

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

        internal void CommandLineSave()
        {
            SaveFile();
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

                            PopulateTemperatures(BFBConstants.LEFT_TEMP, gridLeftTemps, border);
                            PopulateTemperatures(BFBConstants.RIGHT_TEMP, gridRightTemps, border);
                            PopulateRetractionStarts(border);
                            PopulateRetractionStops(border);

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

        internal void PopulateRetractionStarts(RectangleBorder border)
        {

            //List<string> retractionStartLines = bfbObject.generateRetractionStartList();

            SourceGrid.Cells.Editors.TextBox retractCountEditor = new SourceGrid.Cells.Editors.TextBox(typeof(int));
            retractCountEditor.EditableMode = SourceGrid.EditableMode.None;


            int gridRow = 1;
            RetractionStartChangedEvent valueChangedController = new RetractionStartChangedEvent(this);


            foreach (string key in bfbObject.RetractionStartLineList.Keys)
            {
                int index = bfbObject.RetractionStartLineList[key][0];
                Retraction retraction = bfbObject.RetractionStartDictionary[index];

                gridRetractionStart.Rows.Insert(gridRow);
                gridRetractionStart[gridRow, 0] = new SourceGrid.Cells.Cell(retraction.P, typeof(int));
                gridRetractionStart[gridRow, 0].AddController(valueChangedController);
                gridRetractionStart[gridRow, 1] = new SourceGrid.Cells.Cell(retraction.S, typeof(int));
                gridRetractionStart[gridRow, 1].AddController(valueChangedController);
                if (retraction.G != -1)
                {
                    gridRetractionStart[gridRow, 2] = new SourceGrid.Cells.Cell(retraction.G, typeof(int));
                    gridRetractionStart[gridRow, 2].AddController(valueChangedController);
                }
                else
                {
                    gridRetractionStart[gridRow, 2] = new SourceGrid.Cells.Cell(" ", retractCountEditor);
                }
                if (retraction.F != -1)
                {
                    gridRetractionStart[gridRow, 3] = new SourceGrid.Cells.Cell(retraction.F, typeof(int));
                    gridRetractionStart[gridRow, 3].AddController(valueChangedController);
                }
                else
                {
                    gridRetractionStart[gridRow, 3] = new SourceGrid.Cells.Cell(" ", retractCountEditor);
                }
                gridRetractionStart[gridRow, 4] = new SourceGrid.Cells.Cell(index, retractCountEditor);

                //gridLeftTemps[gridRow, 1] = new SourceGrid.Cells.CellControl(); 
                gridRow++;
            }
        }

        internal void PopulateRetractionStops(RectangleBorder border)
        {

            //List<string> retractionStartLines = bfbObject.generateRetractionStartList();

            SourceGrid.Cells.Editors.TextBox retractCountEditor = new SourceGrid.Cells.Editors.TextBox(typeof(int));
            retractCountEditor.EditableMode = SourceGrid.EditableMode.None;


            int gridRow = 1;
            RetractionStopChangedEvent valueChangedController = new RetractionStopChangedEvent(this);


            foreach (string key in bfbObject.RetractionStopLineList.Keys)
            {
                int index = bfbObject.RetractionStopLineList[key][0];
                Retraction retraction = bfbObject.RetractionStopDictionary[index];

                gridRetractionStop.Rows.Insert(gridRow);
                gridRetractionStop[gridRow, 0] = new SourceGrid.Cells.Cell(retraction.P, typeof(int));
                gridRetractionStop[gridRow, 0].AddController(valueChangedController);
                gridRetractionStop[gridRow, 1] = new SourceGrid.Cells.Cell(retraction.S, typeof(int));
                gridRetractionStop[gridRow, 1].AddController(valueChangedController);
                gridRetractionStop[gridRow, 2] = new SourceGrid.Cells.Cell(index, retractCountEditor);

                gridRow++;
            }
        }

        internal void UpdateStartRetractions(Grid gridStartRetracts)
        {
            if (gridStartRetracts.Rows.Count > 1)
            {
                for (int i = 1; i < gridStartRetracts.Rows.Count; i++)
                {
                    int p = (int)gridStartRetracts[i, 0].Value;
                    int s = (int)gridStartRetracts[i, 1].Value;
                    int g = (int)gridStartRetracts[i, 2].Value;
                    int f = (int)gridStartRetracts[i, 3].Value;
                    int index = (int)gridStartRetracts[i, 4].Value;

                    string retractCmd = BFBConstants.RETRACT_START + " P" + p + " S" + s;
                    if (g != -1)
                        retractCmd += " G" + g;

                    if (f != -1)
                        retractCmd += " F" + f;

                    bfbObject.updateRetractionStartLines(index, retractCmd);

                }
            }
        }

        internal void UpdateStopRetractions(Grid gridStopRetracts)
        {
            if (gridStopRetracts.Rows.Count > 1)
            {
                for (int i = 1; i < gridStopRetracts.Rows.Count; i++)
                {
                    int p = (int)gridStopRetracts[i, 0].Value;
                    int s = (int)gridStopRetracts[i, 1].Value;
                    int index = (int)gridStopRetracts[i, 2].Value;

                    string retractCmd = BFBConstants.RETRACT_STOP + " P" + p + " S" + s;

                    bfbObject.updateRetractionStopLines(index, retractCmd);

                }
            }
        }


        internal void PopulateTemperatures(string tempCmdStr, Grid gridTemps, RectangleBorder border)
        {
            Dictionary<int, int> tempDict = new Dictionary<int, int>();

            List<string> tempLines = bfbObject.getTemperatures(tempCmdStr);

            for (int i = 0; i < tempLines.Count; i++)
            {
                int temperature = bfbObject.GetTemperatureFromString(tempLines[i]);
                if (tempDict.ContainsKey(temperature))
                {
                    tempDict[temperature]++;
                }
                else
                {
                    tempDict.Add(temperature, 1);
                }
            }

            SourceGrid.Cells.Editors.ComboBox tempModEditor;
            String[] tempModType = new String[] { "Percentage", "Additive", "Replace" };
            tempModEditor = new SourceGrid.Cells.Editors.ComboBox(typeof(String));
            tempModEditor.StandardValues = tempModType;
            tempModEditor.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.SingleClick | SourceGrid.EditableMode.AnyKey;

            SourceGrid.Cells.Editors.TextBox tempCountEditor = new SourceGrid.Cells.Editors.TextBox(typeof(int));
            tempCountEditor.EditableMode = SourceGrid.EditableMode.None;

            int gridRow = 1;
            TemperatureChangedEvent valueChangedController = new TemperatureChangedEvent(this);

            foreach (int temp in tempDict.Keys)
            {
                gridTemps.Rows.Insert(gridRow);
                gridTemps[gridRow, 0] = new SourceGrid.Cells.Cell(temp, tempCountEditor);
                gridTemps[gridRow, 1] = new SourceGrid.Cells.Cell(tempDict[temp], tempCountEditor);
                gridTemps[gridRow, 2] = new SourceGrid.Cells.Cell(0, typeof(int));
                gridTemps[gridRow, 2].AddController(valueChangedController);
                gridTemps[gridRow, 3] = new SourceGrid.Cells.Cell("Percentage", tempModEditor);
                gridTemps[gridRow, 3].View = SourceGrid.Cells.Views.ComboBox.Default;
                gridTemps[gridRow, 3].AddController(valueChangedController);
                gridTemps[gridRow, 4] = new SourceGrid.Cells.Cell(0, tempCountEditor);

                //gridLeftTemps[gridRow, 1] = new SourceGrid.Cells.CellControl(); 
                gridRow++;
            }
        }

        internal void UpdateTemperatures(Grid gridTemps)
        {
            for (int i = 1; i < gridTemps.Rows.Count; i++)
            {
                if ((int)gridTemps[i, 2].Value != 0)
                {
                    gridTemps[i, 0].Value = gridTemps[i, 4].Value;
                    gridTemps[i, 4].Value = 0;
                }
            }
        }

        internal void UpdateTemperatures(string tempCmd, Grid gridTemps)
        {

            int oldTemp;
            int newTemp;

            for (int i = 1; i < gridTemps.Rows.Count; i++)
            {
                if ((int)gridTemps[i, 2].Value != 0)
                {

                    oldTemp = (int)gridTemps[i, 0].Value;
                    newTemp = (int)gridTemps[i, 4].Value;

                    if (oldTemp != newTemp)
                    {
                        bfbObject.UpdateTemperatureLines(tempCmd, oldTemp, newTemp);
                    }
                }
            }

            UpdateTemperatures(gridTemps);

        }


        internal void CalculateTemperatures(Grid gridTemps)
        {
            if (gridTemps.Rows.Count > 1)
            {
                for (int i = 1; i < gridTemps.Rows.Count; i++)
                {
                    int currentTemp = (int)gridTemps[i, 0].Value;
                    int newTemp = 0;

                    if (gridTemps[i, 3].Value.Equals("Percentage"))
                    {
                        double percentage = (int)(gridTemps[i, 2].Value);
                        if (currentTemp > 0 && percentage != 0)
                        {
                            newTemp = Convert.ToInt32((percentage / 100) * currentTemp + currentTemp);
                        }
                    }
                    else if (gridTemps[i, 3].Value.Equals("Additive"))
                    {
                        int additive = (int)(gridTemps[i, 2].Value);
                        newTemp = currentTemp + additive;
                    }
                    else if (gridTemps[i, 3].Value.Equals("Replace"))
                    {
                        int replace = (int)(gridTemps[i, 2].Value);
                        newTemp = replace;
                    }
                    else
                    {
                        newTemp = currentTemp;
                    }

                    if (newTemp < 0)
                        newTemp = 0;

                    if (newTemp > 265)
                        newTemp = 265;

                    gridTemps[i, 4].Value = newTemp;
                }
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
            if (gridRetractionStart.Rows.Count > 1)
                gridRetractionStart.Rows.RemoveRange(1, gridRetractionStart.Rows.Count - 1);

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
                SaveFile();
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

        private void SaveFile()
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


        private void AbouitToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void CbLeftMaterial_SelectedIndexChanged(object sender, EventArgs e)
        {
            modified = true;
            AddOrRemoveNatural(cbLeftMaterial, cbLeftColor);
            bfbObject.SetMATERIALCODE(BFBConstants.MATERIALCODEE1, cbLeftMaterial.Text, cbLeftColor.Text);
        }


        private void CbRightMaterial_SelectedIndexChanged(object sender, EventArgs e)
        {
            modified = true;
            AddOrRemoveNatural(cbRightMaterial, cbRightColor);
            bfbObject.SetMATERIALCODE(BFBConstants.MATERIALCODEE2, cbRightMaterial.Text, cbRightColor.Text);
        }

        private void CbCubeProMaterial_SelectedIndexChanged(object sender, EventArgs e)
        {
            modified = true;
            AddOrRemoveNatural(cbCubeProMaterial, cbCubeProColor);
            bfbObject.SetMATERIALCODE(BFBConstants.MATERIALCODEE3, cbCubeProMaterial.Text, cbCubeProColor.Text);
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
            UpdateTemperatures(BFBConstants.LEFT_TEMP, gridLeftTemps);
        }

        private void BtnRightTempUpdate_Click(object sender, EventArgs e)
        {
            modified = true;
            UpdateTemperatures(BFBConstants.RIGHT_TEMP, gridRightTemps);
        }

        private void TbFirware_TextChanged(object sender, EventArgs e)
        {
            if (tbFirmware.Text.Length > 0)
            {
                modified = true;
                bfbObject.SetFIRMWARE(tbFirmware.Text);
            }
        }

        private void TbMinFirmware_TextChanged(object sender, EventArgs e)
        {
            if (tbMinFirmware.Text.Length > 0)
            {
                modified = true;
                bfbObject.SetMINFIRMWARE(tbMinFirmware.Text);
            }
        }

        private void TbPrinterModel_TextChanged(object sender, EventArgs e)
        {
            if (tbPrinterModel.Text.Length > 0)
            {
                modified = true;
                bfbObject.SetPRINTERMODEL(tbPrinterModel.Text);
            }
        }

        private void BtnViewRaw_Click(object sender, EventArgs e)
        {
            FrmRawView rawViewForm = new FrmRawView(bfbObject.BfbLines);
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


        internal bool CommandLineLoad(String newFWStr, String newMinFWStr, String newPrinterModel, String newMCE1, String newMCE2, String newMCE3,
            List<TemperatureModifier> tempMods, List<RetractModifier> retractStartMods, List<RetractModifier> retractStopMods)
        {
            bool success = true;


            tbFirmware.Text = newFWStr;
            tbMinFirmware.Text = newMinFWStr;
            tbPrinterModel.Text = newPrinterModel;

            int.TryParse(newMCE1, out int cubeCode);
            bfbObject.SetMATERIALCODE(BFBConstants.MATERIALCODEE1, cubeCode);
            int.TryParse(newMCE2, out cubeCode);
            bfbObject.SetMATERIALCODE(BFBConstants.MATERIALCODEE2, cubeCode);
            int.TryParse(newMCE3, out cubeCode);
            bfbObject.SetMATERIALCODE(BFBConstants.MATERIALCODEE3, cubeCode);

            // process temperatures
            foreach (TemperatureModifier tempMod in tempMods)
            {
                if (tempMod.extruder.Equals(BFBConstants.LEFT_TEMP))
                {
                    for (int i = 1; i < gridLeftTemps.Rows.Count; i++)
                    {
                        if ((int)gridLeftTemps[i, 0].Value == tempMod.oldValue)
                        {
                            // set row to replace and replace value to newvalue
                            gridLeftTemps[i, 2].Value = tempMod.newValue;
                            gridLeftTemps[i, 3].Value = "Replace";
                        }
                    }
                }
                else if (tempMod.extruder.Equals(BFBConstants.RIGHT_TEMP))
                {
                    for (int i = 1; i < gridRightTemps.Rows.Count; i++)
                    {
                        if ((int)gridRightTemps[i, 0].Value == tempMod.oldValue)
                        {
                            // set row to replace and replace value to newvalue
                            gridRightTemps[i, 2].Value = tempMod.newValue;
                            gridRightTemps[i, 3].Value = "Replace";
                        }
                    }
                }
                else if (tempMod.extruder.Equals(BFBConstants.MID_TEMP))
                {
                    /* TODO: Update when I have MIDTEMP working....
                    for (int i = 1; i < gridMidTemps.Rows.Count; i++)
                    {
                        if ((int)gridMidTemps[i, 0].Value == tempMod.oldValue)
                        {
                            // set row to replace and replace value to newvalue
                            gridMidTemps[i, 2].Value = tempMod.newValue;
                            gridMidTemps[i, 3].Value = "Replace";
                        }
                    }
                    */
                }
            }
            CalculateTemperatures(gridLeftTemps);
            UpdateTemperatures(BFBConstants.LEFT_TEMP, gridLeftTemps);
            CalculateTemperatures(gridRightTemps);
            UpdateTemperatures(BFBConstants.RIGHT_TEMP, gridRightTemps);
            //CalculateTemperatures(gridMidTemps);
            //UpdateTemperatures(gridMidTemps);

            // process retract start
            foreach (RetractModifier retractMod in retractStartMods)
            {
                for (int i = 1; i < gridRetractionStart.Rows.Count; i++)
                {
                    if ((int)gridRetractionStart[i, 0].Value == retractMod.oldRetractValue &&
                        (int)gridRetractionStart[i, 1].Value == retractMod.oldRetractValue)
                    {
                        // set row to replace and replace value to newvalue
                        gridRetractionStart[i, 0].Value = retractMod.newRetractValue;
                        gridRetractionStart[i, 1].Value = retractMod.newRetractValue;
                    }
                }
            }
            UpdateStartRetractions(gridRetractionStart);

            // process retract stop
            foreach (RetractModifier retractMod in retractStopMods)
            {
                for (int i = 1; i < gridRetractionStop.Rows.Count; i++)
                {
                    if ((int)gridRetractionStop[i, 1].Value == retractMod.oldRetractValue)
                    {
                        // set row to replace and replace value to newvalue
                        gridRetractionStop[i, 1].Value = retractMod.newRetractValue;
                    }
                }
            }
            UpdateStopRetractions(gridRetractionStop);

            return success;
        }
    }
}
