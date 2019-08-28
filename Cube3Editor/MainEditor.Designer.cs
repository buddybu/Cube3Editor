namespace Cube3Editor
{
    partial class MainEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainEditor));
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbPrinterModel = new System.Windows.Forms.ComboBox();
            this.cbMinFirmware = new System.Windows.Forms.ComboBox();
            this.cbFirmware = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cbLeftMaterial = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cbLeftColor = new System.Windows.Forms.ComboBox();
            this.cbRightColor = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbRightMaterial = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbLeftSupport = new System.Windows.Forms.CheckBox();
            this.cbLeftSidewalk = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbRightSupport = new System.Windows.Forms.CheckBox();
            this.cbRightSidewalk = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.cbCubeProSupport = new System.Windows.Forms.CheckBox();
            this.cbCubeProMaterial = new System.Windows.Forms.ComboBox();
            this.cbCubeProSidewalk = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cbCubeProColor = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnGenerateCube3 = new System.Windows.Forms.Button();
            this.btnViewRaw = new System.Windows.Forms.Button();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.abouitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateScriptToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.preferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabRetractrionControl = new System.Windows.Forms.TabPage();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.gridRetractionStop = new SourceGrid.Grid();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.gridRetractionStart = new SourceGrid.Grid();
            this.tabTemperatureControl = new System.Windows.Forms.TabPage();
            this.gridLeftTemps = new SourceGrid.Grid();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnLeftCalculate = new System.Windows.Forms.Button();
            this.btnLeftTempUpdate = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.gridRightTemps = new SourceGrid.Grid();
            this.btnRightTempUpdate = new System.Windows.Forms.Button();
            this.btnRightCalculate = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tbPressure = new System.Windows.Forms.TabPage();
            this.btnCalculate = new System.Windows.Forms.Button();
            this.nudPressureMod = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.btnClearSelection = new System.Windows.Forms.Button();
            this.gridPressure = new SourceGrid.Grid();
            this.btnGenScript = new System.Windows.Forms.Button();
            this.btnViewScript = new System.Windows.Forms.Button();
            this.saveScriptDialog = new System.Windows.Forms.SaveFileDialog();
            this.saveBFBFile = new System.Windows.Forms.SaveFileDialog();
            this.fdLoadScript = new System.Windows.Forms.OpenFileDialog();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.mainMenu.SuspendLayout();
            this.tabRetractrionControl.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.tabTemperatureControl.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tbPressure.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPressureMod)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "cube3";
            this.openFileDialog.Title = "Open Cube File";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbPrinterModel);
            this.panel1.Controls.Add(this.cbMinFirmware);
            this.panel1.Controls.Add(this.cbFirmware);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(12, 27);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(216, 93);
            this.panel1.TabIndex = 1;
            // 
            // cbPrinterModel
            // 
            this.cbPrinterModel.FormattingEnabled = true;
            this.cbPrinterModel.Items.AddRange(new object[] {
            "CUBE3",
            "EKOCYCLE",
            "CUBEPRO"});
            this.cbPrinterModel.Location = new System.Drawing.Point(114, 5);
            this.cbPrinterModel.Name = "cbPrinterModel";
            this.cbPrinterModel.Size = new System.Drawing.Size(94, 24);
            this.cbPrinterModel.TabIndex = 5;
            this.cbPrinterModel.SelectedIndexChanged += new System.EventHandler(this.CbPrinterModel_SelectedIndexChanged);
            this.cbPrinterModel.TextChanged += new System.EventHandler(this.CbPrinterModel_TextChanged);
            // 
            // cbMinFirmware
            // 
            this.cbMinFirmware.FormattingEnabled = true;
            this.cbMinFirmware.Items.AddRange(new object[] {
            "V1.14B",
            "V1.05",
            "V1.87"});
            this.cbMinFirmware.Location = new System.Drawing.Point(114, 62);
            this.cbMinFirmware.Name = "cbMinFirmware";
            this.cbMinFirmware.Size = new System.Drawing.Size(94, 24);
            this.cbMinFirmware.TabIndex = 4;
            this.cbMinFirmware.SelectedIndexChanged += new System.EventHandler(this.CbMinFirmware_SelectedIndexChanged);
            this.cbMinFirmware.TextChanged += new System.EventHandler(this.CbMinFirmware_TextChanged);
            // 
            // cbFirmware
            // 
            this.cbFirmware.FormattingEnabled = true;
            this.cbFirmware.Items.AddRange(new object[] {
            "V1.14B",
            "V1.05",
            "V1.87"});
            this.cbFirmware.Location = new System.Drawing.Point(114, 34);
            this.cbFirmware.Name = "cbFirmware";
            this.cbFirmware.Size = new System.Drawing.Size(94, 24);
            this.cbFirmware.TabIndex = 3;
            this.cbFirmware.SelectedIndexChanged += new System.EventHandler(this.CbFirmware_SelectedIndexChanged);
            this.cbFirmware.TextChanged += new System.EventHandler(this.cbFirware_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "MinFirmware:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "Printer Model:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Firmware:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 17);
            this.label4.TabIndex = 2;
            this.label4.Text = "Material Type";
            // 
            // cbLeftMaterial
            // 
            this.cbLeftMaterial.Enabled = false;
            this.cbLeftMaterial.FormattingEnabled = true;
            this.cbLeftMaterial.Items.AddRange(new object[] {
            "ABS",
            "PLA",
            "EKO",
            "CP_ABS",
            "CP_PLA",
            "INFRINSE",
            "CP_INFRINSE",
            "EMPTY"});
            this.cbLeftMaterial.Location = new System.Drawing.Point(142, 20);
            this.cbLeftMaterial.Name = "cbLeftMaterial";
            this.cbLeftMaterial.Size = new System.Drawing.Size(68, 24);
            this.cbLeftMaterial.TabIndex = 4;
            this.cbLeftMaterial.SelectedIndexChanged += new System.EventHandler(this.CbLeftMaterial_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 50);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(95, 17);
            this.label7.TabIndex = 7;
            this.label7.Text = "Material Color";
            // 
            // cbLeftColor
            // 
            this.cbLeftColor.Enabled = false;
            this.cbLeftColor.FormattingEnabled = true;
            this.cbLeftColor.Items.AddRange(new object[] {
            "Teal",
            "Purple",
            "Brown",
            "Silver",
            "GITDG",
            "GITDB",
            "Forest Green",
            "Navy Blue",
            "Coral",
            "Dark Grey",
            "Pale Yellow",
            "Gold",
            "Bronze",
            "Red",
            "Green",
            "Blue",
            "Yellow",
            "White",
            "Black",
            "Tan",
            "Magenta",
            "Orange",
            "Neon Green"});
            this.cbLeftColor.Location = new System.Drawing.Point(142, 47);
            this.cbLeftColor.Name = "cbLeftColor";
            this.cbLeftColor.Size = new System.Drawing.Size(68, 24);
            this.cbLeftColor.TabIndex = 8;
            this.cbLeftColor.SelectedIndexChanged += new System.EventHandler(this.CbLeftColor_SelectedIndexChanged);
            // 
            // cbRightColor
            // 
            this.cbRightColor.Enabled = false;
            this.cbRightColor.FormattingEnabled = true;
            this.cbRightColor.Items.AddRange(new object[] {
            "Teal",
            "Purple",
            "Brown",
            "Silver",
            "GITG",
            "GITB",
            "Forest Green",
            "Navy Blue",
            "Coral",
            "Dark Grey",
            "Pale Yellow",
            "Gold",
            "Bronze",
            "Red",
            "Green",
            "Blue",
            "Yellow",
            "White",
            "Black",
            "Tan",
            "Magenta",
            "Orange",
            "Neon Green"});
            this.cbRightColor.Location = new System.Drawing.Point(142, 51);
            this.cbRightColor.Name = "cbRightColor";
            this.cbRightColor.Size = new System.Drawing.Size(68, 24);
            this.cbRightColor.TabIndex = 13;
            this.cbRightColor.SelectedIndexChanged += new System.EventHandler(this.CbRightColor_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 54);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(95, 17);
            this.label5.TabIndex = 12;
            this.label5.Text = "Material Color";
            // 
            // cbRightMaterial
            // 
            this.cbRightMaterial.Enabled = false;
            this.cbRightMaterial.FormattingEnabled = true;
            this.cbRightMaterial.Items.AddRange(new object[] {
            "ABS",
            "PLA",
            "CP_ABS",
            "CP_PLA",
            "EKO",
            "INFRINSE",
            "CP_INFRINSE",
            "EMPTY"});
            this.cbRightMaterial.Location = new System.Drawing.Point(142, 20);
            this.cbRightMaterial.Name = "cbRightMaterial";
            this.cbRightMaterial.Size = new System.Drawing.Size(68, 24);
            this.cbRightMaterial.TabIndex = 10;
            this.cbRightMaterial.SelectedIndexChanged += new System.EventHandler(this.CbRightMaterial_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(15, 23);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(94, 17);
            this.label9.TabIndex = 9;
            this.label9.Text = "Material Type";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbLeftSupport);
            this.groupBox1.Controls.Add(this.cbLeftSidewalk);
            this.groupBox1.Controls.Add(this.cbLeftMaterial);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.cbLeftColor);
            this.groupBox1.Location = new System.Drawing.Point(12, 126);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(216, 103);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Left Extruder (E1)";
            // 
            // cbLeftSupport
            // 
            this.cbLeftSupport.AutoSize = true;
            this.cbLeftSupport.Enabled = false;
            this.cbLeftSupport.Location = new System.Drawing.Point(125, 76);
            this.cbLeftSupport.Name = "cbLeftSupport";
            this.cbLeftSupport.Size = new System.Drawing.Size(80, 21);
            this.cbLeftSupport.TabIndex = 17;
            this.cbLeftSupport.Text = "Support";
            this.cbLeftSupport.UseVisualStyleBackColor = true;
            this.cbLeftSupport.CheckedChanged += new System.EventHandler(this.CbLeftSupport_CheckedChanged);
            // 
            // cbLeftSidewalk
            // 
            this.cbLeftSidewalk.AutoSize = true;
            this.cbLeftSidewalk.Enabled = false;
            this.cbLeftSidewalk.Location = new System.Drawing.Point(12, 76);
            this.cbLeftSidewalk.Name = "cbLeftSidewalk";
            this.cbLeftSidewalk.Size = new System.Drawing.Size(85, 21);
            this.cbLeftSidewalk.TabIndex = 16;
            this.cbLeftSidewalk.Text = "Sidewalk";
            this.cbLeftSidewalk.UseVisualStyleBackColor = true;
            this.cbLeftSidewalk.CheckedChanged += new System.EventHandler(this.CbLeftSidewalk_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbRightSupport);
            this.groupBox2.Controls.Add(this.cbRightSidewalk);
            this.groupBox2.Controls.Add(this.cbRightMaterial);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.cbRightColor);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(12, 354);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(216, 112);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Right Extruder (E2)";
            // 
            // cbRightSupport
            // 
            this.cbRightSupport.AutoSize = true;
            this.cbRightSupport.Enabled = false;
            this.cbRightSupport.Location = new System.Drawing.Point(125, 85);
            this.cbRightSupport.Name = "cbRightSupport";
            this.cbRightSupport.Size = new System.Drawing.Size(80, 21);
            this.cbRightSupport.TabIndex = 15;
            this.cbRightSupport.Text = "Support";
            this.cbRightSupport.UseVisualStyleBackColor = true;
            this.cbRightSupport.CheckedChanged += new System.EventHandler(this.CbRightSupport_CheckedChanged);
            // 
            // cbRightSidewalk
            // 
            this.cbRightSidewalk.AutoSize = true;
            this.cbRightSidewalk.Enabled = false;
            this.cbRightSidewalk.Location = new System.Drawing.Point(12, 85);
            this.cbRightSidewalk.Name = "cbRightSidewalk";
            this.cbRightSidewalk.Size = new System.Drawing.Size(85, 21);
            this.cbRightSidewalk.TabIndex = 14;
            this.cbRightSidewalk.Text = "Sidewalk";
            this.cbRightSidewalk.UseVisualStyleBackColor = true;
            this.cbRightSidewalk.CheckedChanged += new System.EventHandler(this.CbRightSidewalk_CheckedChanged);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.cbCubeProSupport);
            this.groupBox5.Controls.Add(this.cbCubeProMaterial);
            this.groupBox5.Controls.Add(this.cbCubeProSidewalk);
            this.groupBox5.Controls.Add(this.label6);
            this.groupBox5.Controls.Add(this.cbCubeProColor);
            this.groupBox5.Controls.Add(this.label8);
            this.groupBox5.Location = new System.Drawing.Point(12, 235);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(216, 113);
            this.groupBox5.TabIndex = 28;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Mid Extruder (E3)";
            // 
            // cbCubeProSupport
            // 
            this.cbCubeProSupport.AutoSize = true;
            this.cbCubeProSupport.Enabled = false;
            this.cbCubeProSupport.Location = new System.Drawing.Point(128, 86);
            this.cbCubeProSupport.Name = "cbCubeProSupport";
            this.cbCubeProSupport.Size = new System.Drawing.Size(80, 21);
            this.cbCubeProSupport.TabIndex = 17;
            this.cbCubeProSupport.Text = "Support";
            this.cbCubeProSupport.UseVisualStyleBackColor = true;
            this.cbCubeProSupport.CheckedChanged += new System.EventHandler(this.CbMidSupport_CheckedChanged);
            // 
            // cbCubeProMaterial
            // 
            this.cbCubeProMaterial.Enabled = false;
            this.cbCubeProMaterial.FormattingEnabled = true;
            this.cbCubeProMaterial.Items.AddRange(new object[] {
            "ABS",
            "PLA",
            "CP_ABS",
            "CP_PLA",
            "EKO",
            "INFRINSE",
            "CP_INFRINSE",
            "EMPTY"});
            this.cbCubeProMaterial.Location = new System.Drawing.Point(142, 20);
            this.cbCubeProMaterial.Name = "cbCubeProMaterial";
            this.cbCubeProMaterial.Size = new System.Drawing.Size(68, 24);
            this.cbCubeProMaterial.TabIndex = 10;
            this.cbCubeProMaterial.SelectedIndexChanged += new System.EventHandler(this.CbCubeProMaterial_SelectedIndexChanged);
            // 
            // cbCubeProSidewalk
            // 
            this.cbCubeProSidewalk.AutoSize = true;
            this.cbCubeProSidewalk.Enabled = false;
            this.cbCubeProSidewalk.Location = new System.Drawing.Point(15, 86);
            this.cbCubeProSidewalk.Name = "cbCubeProSidewalk";
            this.cbCubeProSidewalk.Size = new System.Drawing.Size(85, 21);
            this.cbCubeProSidewalk.TabIndex = 16;
            this.cbCubeProSidewalk.Text = "Sidewalk";
            this.cbCubeProSidewalk.UseVisualStyleBackColor = true;
            this.cbCubeProSidewalk.CheckedChanged += new System.EventHandler(this.CbMidSidewalk_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 23);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(94, 17);
            this.label6.TabIndex = 9;
            this.label6.Text = "Material Type";
            // 
            // cbCubeProColor
            // 
            this.cbCubeProColor.Enabled = false;
            this.cbCubeProColor.FormattingEnabled = true;
            this.cbCubeProColor.Items.AddRange(new object[] {
            "Teal",
            "Purple",
            "Brown",
            "Silver",
            "GITG",
            "GITB",
            "Forest Green",
            "Navy Blue",
            "Coral",
            "Dark Grey",
            "Pale Yellow",
            "Gold",
            "Bronze",
            "Red",
            "Green",
            "Blue",
            "Yellow",
            "White",
            "Black",
            "Tan",
            "Magenta",
            "Orange",
            "Neon Green"});
            this.cbCubeProColor.Location = new System.Drawing.Point(142, 51);
            this.cbCubeProColor.Name = "cbCubeProColor";
            this.cbCubeProColor.Size = new System.Drawing.Size(68, 24);
            this.cbCubeProColor.TabIndex = 13;
            this.cbCubeProColor.SelectedIndexChanged += new System.EventHandler(this.CbCubeProColor_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(15, 54);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(95, 17);
            this.label8.TabIndex = 12;
            this.label8.Text = "Material Color";
            // 
            // btnGenerateCube3
            // 
            this.btnGenerateCube3.Location = new System.Drawing.Point(733, 415);
            this.btnGenerateCube3.Name = "btnGenerateCube3";
            this.btnGenerateCube3.Size = new System.Drawing.Size(133, 23);
            this.btnGenerateCube3.TabIndex = 29;
            this.btnGenerateCube3.Text = "Generate Cube3";
            this.btnGenerateCube3.UseVisualStyleBackColor = true;
            this.btnGenerateCube3.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // btnViewRaw
            // 
            this.btnViewRaw.Location = new System.Drawing.Point(328, 415);
            this.btnViewRaw.Name = "btnViewRaw";
            this.btnViewRaw.Size = new System.Drawing.Size(108, 23);
            this.btnViewRaw.TabIndex = 30;
            this.btnViewRaw.Text = "View Raw BFB";
            this.btnViewRaw.UseVisualStyleBackColor = true;
            this.btnViewRaw.Click += new System.EventHandler(this.BtnViewRaw_Click);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.closeToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.loadScriptToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F)));
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(46, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(225, 26);
            this.openToolStripMenuItem.Text = "Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F4)));
            this.closeToolStripMenuItem.ShowShortcutKeys = false;
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(225, 26);
            this.closeToolStripMenuItem.Text = "Close Model";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(225, 26);
            this.saveToolStripMenuItem.Text = "Save Model";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(225, 26);
            this.saveAsToolStripMenuItem.Text = "Save Model As...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // loadScriptToolStripMenuItem
            // 
            this.loadScriptToolStripMenuItem.Name = "loadScriptToolStripMenuItem";
            this.loadScriptToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.loadScriptToolStripMenuItem.Size = new System.Drawing.Size(225, 26);
            this.loadScriptToolStripMenuItem.Text = "Load Script...";
            this.loadScriptToolStripMenuItem.Click += new System.EventHandler(this.LoadScriptToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(225, 26);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.abouitToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.H)));
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(55, 24);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // abouitToolStripMenuItem
            // 
            this.abouitToolStripMenuItem.Name = "abouitToolStripMenuItem";
            this.abouitToolStripMenuItem.Size = new System.Drawing.Size(142, 26);
            this.abouitToolStripMenuItem.Text = "About...";
            this.abouitToolStripMenuItem.Click += new System.EventHandler(this.AbouitToolStripMenuItem_Click);
            // 
            // mainMenu
            // 
            this.mainMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(903, 28);
            this.mainMenu.TabIndex = 0;
            this.mainMenu.Text = "menuStrip1";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.generateScriptToolStripMenuItem1,
            this.preferencesToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(49, 24);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // generateScriptToolStripMenuItem1
            // 
            this.generateScriptToolStripMenuItem1.Name = "generateScriptToolStripMenuItem1";
            this.generateScriptToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.generateScriptToolStripMenuItem1.Size = new System.Drawing.Size(246, 26);
            this.generateScriptToolStripMenuItem1.Text = "Generate Script";
            this.generateScriptToolStripMenuItem1.Click += new System.EventHandler(this.GenerateScriptToolStripMenuItem_Click);
            // 
            // preferencesToolStripMenuItem
            // 
            this.preferencesToolStripMenuItem.Name = "preferencesToolStripMenuItem";
            this.preferencesToolStripMenuItem.Size = new System.Drawing.Size(246, 26);
            this.preferencesToolStripMenuItem.Text = "Preferences";
            this.preferencesToolStripMenuItem.Click += new System.EventHandler(this.PreferencesToolStripMenuItem_Click);
            // 
            // tabRetractrionControl
            // 
            this.tabRetractrionControl.Controls.Add(this.groupBox7);
            this.tabRetractrionControl.Controls.Add(this.groupBox6);
            this.tabRetractrionControl.Location = new System.Drawing.Point(4, 25);
            this.tabRetractrionControl.Name = "tabRetractrionControl";
            this.tabRetractrionControl.Padding = new System.Windows.Forms.Padding(3);
            this.tabRetractrionControl.Size = new System.Drawing.Size(621, 357);
            this.tabRetractrionControl.TabIndex = 1;
            this.tabRetractrionControl.Text = "Retraction Control";
            this.tabRetractrionControl.UseVisualStyleBackColor = true;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.gridRetractionStop);
            this.groupBox7.Location = new System.Drawing.Point(3, 183);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(609, 179);
            this.groupBox7.TabIndex = 2;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "M228";
            // 
            // gridRetractionStop
            // 
            this.gridRetractionStop.EnableSort = true;
            this.gridRetractionStop.Location = new System.Drawing.Point(6, 19);
            this.gridRetractionStop.Name = "gridRetractionStop";
            this.gridRetractionStop.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.gridRetractionStop.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.gridRetractionStop.Size = new System.Drawing.Size(597, 154);
            this.gridRetractionStop.TabIndex = 0;
            this.gridRetractionStop.TabStop = true;
            this.gridRetractionStop.ToolTipText = "";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.gridRetractionStart);
            this.groupBox6.Location = new System.Drawing.Point(6, 6);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(609, 171);
            this.groupBox6.TabIndex = 1;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "M227";
            // 
            // gridRetractionStart
            // 
            this.gridRetractionStart.EnableSort = true;
            this.gridRetractionStart.Location = new System.Drawing.Point(6, 19);
            this.gridRetractionStart.Name = "gridRetractionStart";
            this.gridRetractionStart.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.gridRetractionStart.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.gridRetractionStart.Size = new System.Drawing.Size(597, 154);
            this.gridRetractionStart.TabIndex = 0;
            this.gridRetractionStart.TabStop = true;
            this.gridRetractionStart.ToolTipText = "";
            // 
            // tabTemperatureControl
            // 
            this.tabTemperatureControl.Controls.Add(this.gridLeftTemps);
            this.tabTemperatureControl.Controls.Add(this.groupBox3);
            this.tabTemperatureControl.Controls.Add(this.groupBox4);
            this.tabTemperatureControl.Location = new System.Drawing.Point(4, 25);
            this.tabTemperatureControl.Name = "tabTemperatureControl";
            this.tabTemperatureControl.Padding = new System.Windows.Forms.Padding(3);
            this.tabTemperatureControl.Size = new System.Drawing.Size(621, 357);
            this.tabTemperatureControl.TabIndex = 0;
            this.tabTemperatureControl.Text = "Temperature Control";
            this.tabTemperatureControl.UseVisualStyleBackColor = true;
            // 
            // gridLeftTemps
            // 
            this.gridLeftTemps.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gridLeftTemps.EnableSort = true;
            this.gridLeftTemps.Location = new System.Drawing.Point(6, 59);
            this.gridLeftTemps.Name = "gridLeftTemps";
            this.gridLeftTemps.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.gridLeftTemps.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.gridLeftTemps.Size = new System.Drawing.Size(290, 247);
            this.gridLeftTemps.TabIndex = 28;
            this.gridLeftTemps.TabStop = true;
            this.gridLeftTemps.ToolTipText = "";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnLeftCalculate);
            this.groupBox3.Controls.Add(this.btnLeftTempUpdate);
            this.groupBox3.Location = new System.Drawing.Point(6, 25);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(290, 317);
            this.groupBox3.TabIndex = 34;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Left Cartridge Temperatures";
            // 
            // btnLeftCalculate
            // 
            this.btnLeftCalculate.Location = new System.Drawing.Point(209, 287);
            this.btnLeftCalculate.Name = "btnLeftCalculate";
            this.btnLeftCalculate.Size = new System.Drawing.Size(75, 23);
            this.btnLeftCalculate.TabIndex = 30;
            this.btnLeftCalculate.Text = "Calculate";
            this.btnLeftCalculate.UseVisualStyleBackColor = true;
            this.btnLeftCalculate.Click += new System.EventHandler(this.BtnLeftCalculate_Click);
            // 
            // btnLeftTempUpdate
            // 
            this.btnLeftTempUpdate.Location = new System.Drawing.Point(128, 287);
            this.btnLeftTempUpdate.Name = "btnLeftTempUpdate";
            this.btnLeftTempUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnLeftTempUpdate.TabIndex = 32;
            this.btnLeftTempUpdate.Text = "Update";
            this.btnLeftTempUpdate.UseVisualStyleBackColor = true;
            this.btnLeftTempUpdate.Click += new System.EventHandler(this.BtnLeftTempUpdate_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.gridRightTemps);
            this.groupBox4.Controls.Add(this.btnRightTempUpdate);
            this.groupBox4.Controls.Add(this.btnRightCalculate);
            this.groupBox4.Location = new System.Drawing.Point(305, 25);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(295, 317);
            this.groupBox4.TabIndex = 35;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Right Cartridge Temperatures";
            // 
            // gridRightTemps
            // 
            this.gridRightTemps.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gridRightTemps.EnableSort = true;
            this.gridRightTemps.Location = new System.Drawing.Point(0, 34);
            this.gridRightTemps.Name = "gridRightTemps";
            this.gridRightTemps.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.gridRightTemps.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.gridRightTemps.Size = new System.Drawing.Size(295, 247);
            this.gridRightTemps.TabIndex = 29;
            this.gridRightTemps.TabStop = true;
            this.gridRightTemps.ToolTipText = "";
            // 
            // btnRightTempUpdate
            // 
            this.btnRightTempUpdate.Location = new System.Drawing.Point(45, 287);
            this.btnRightTempUpdate.Name = "btnRightTempUpdate";
            this.btnRightTempUpdate.Size = new System.Drawing.Size(119, 23);
            this.btnRightTempUpdate.TabIndex = 33;
            this.btnRightTempUpdate.Text = "Update";
            this.btnRightTempUpdate.UseVisualStyleBackColor = true;
            this.btnRightTempUpdate.Click += new System.EventHandler(this.BtnRightTempUpdate_Click);
            // 
            // btnRightCalculate
            // 
            this.btnRightCalculate.Location = new System.Drawing.Point(170, 287);
            this.btnRightCalculate.Name = "btnRightCalculate";
            this.btnRightCalculate.Size = new System.Drawing.Size(119, 23);
            this.btnRightCalculate.TabIndex = 31;
            this.btnRightCalculate.Text = "Calculate";
            this.btnRightCalculate.UseVisualStyleBackColor = true;
            this.btnRightCalculate.Click += new System.EventHandler(this.BtnRightCalculate_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabTemperatureControl);
            this.tabControl1.Controls.Add(this.tabRetractrionControl);
            this.tabControl1.Controls.Add(this.tbPressure);
            this.tabControl1.Location = new System.Drawing.Point(262, 27);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(629, 386);
            this.tabControl1.TabIndex = 31;
            // 
            // tbPressure
            // 
            this.tbPressure.Controls.Add(this.btnUpdate);
            this.tbPressure.Controls.Add(this.btnCalculate);
            this.tbPressure.Controls.Add(this.nudPressureMod);
            this.tbPressure.Controls.Add(this.label10);
            this.tbPressure.Controls.Add(this.btnSelectAll);
            this.tbPressure.Controls.Add(this.btnClearSelection);
            this.tbPressure.Controls.Add(this.gridPressure);
            this.tbPressure.Location = new System.Drawing.Point(4, 25);
            this.tbPressure.Name = "tbPressure";
            this.tbPressure.Padding = new System.Windows.Forms.Padding(3);
            this.tbPressure.Size = new System.Drawing.Size(621, 357);
            this.tbPressure.TabIndex = 2;
            this.tbPressure.Text = "Extruder Pressure";
            this.tbPressure.UseVisualStyleBackColor = true;
            // 
            // btnCalculate
            // 
            this.btnCalculate.Location = new System.Drawing.Point(467, 316);
            this.btnCalculate.Name = "btnCalculate";
            this.btnCalculate.Size = new System.Drawing.Size(77, 26);
            this.btnCalculate.TabIndex = 6;
            this.btnCalculate.Text = "Calculate";
            this.btnCalculate.UseVisualStyleBackColor = true;
            this.btnCalculate.Click += new System.EventHandler(this.BtnApplyChange_Click);
            // 
            // nudPressureMod
            // 
            this.nudPressureMod.DecimalPlaces = 1;
            this.nudPressureMod.Location = new System.Drawing.Point(374, 319);
            this.nudPressureMod.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.nudPressureMod.Name = "nudPressureMod";
            this.nudPressureMod.Size = new System.Drawing.Size(70, 22);
            this.nudPressureMod.TabIndex = 5;
            this.nudPressureMod.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(291, 321);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(77, 17);
            this.label10.TabIndex = 4;
            this.label10.Text = "Change %:";
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Location = new System.Drawing.Point(6, 316);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(111, 26);
            this.btnSelectAll.TabIndex = 3;
            this.btnSelectAll.Text = "Select All";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.GridPressureSelectAll_Click);
            // 
            // btnClearSelection
            // 
            this.btnClearSelection.Location = new System.Drawing.Point(123, 316);
            this.btnClearSelection.Name = "btnClearSelection";
            this.btnClearSelection.Size = new System.Drawing.Size(111, 26);
            this.btnClearSelection.TabIndex = 2;
            this.btnClearSelection.Text = "Clear Selection";
            this.btnClearSelection.UseVisualStyleBackColor = true;
            this.btnClearSelection.Click += new System.EventHandler(this.GridPressureClearSelection_Click);
            // 
            // gridPressure
            // 
            this.gridPressure.EnableSort = true;
            this.gridPressure.Location = new System.Drawing.Point(3, 0);
            this.gridPressure.Name = "gridPressure";
            this.gridPressure.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.gridPressure.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.gridPressure.Size = new System.Drawing.Size(612, 310);
            this.gridPressure.TabIndex = 0;
            this.gridPressure.TabStop = true;
            this.gridPressure.ToolTipText = "";
            // 
            // btnGenScript
            // 
            this.btnGenScript.Location = new System.Drawing.Point(583, 415);
            this.btnGenScript.Name = "btnGenScript";
            this.btnGenScript.Size = new System.Drawing.Size(133, 23);
            this.btnGenScript.TabIndex = 32;
            this.btnGenScript.Text = "Generate Script";
            this.btnGenScript.UseVisualStyleBackColor = true;
            this.btnGenScript.Click += new System.EventHandler(this.GenerateScriptToolStripMenuItem_Click);
            // 
            // btnViewScript
            // 
            this.btnViewScript.Location = new System.Drawing.Point(454, 415);
            this.btnViewScript.Name = "btnViewScript";
            this.btnViewScript.Size = new System.Drawing.Size(108, 23);
            this.btnViewScript.TabIndex = 33;
            this.btnViewScript.Text = "View Script";
            this.btnViewScript.UseVisualStyleBackColor = true;
            this.btnViewScript.Click += new System.EventHandler(this.BtnViewScript_Click);
            // 
            // fdLoadScript
            // 
            this.fdLoadScript.FileName = "fdLoadScript";
            this.fdLoadScript.Filter = "\"Cube Script Files|*.cubescr|AllFiless|*.*\"";
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(550, 316);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(65, 26);
            this.btnUpdate.TabIndex = 7;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.BtnUpdate_Click);
            // 
            // MainEditor
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(903, 478);
            this.Controls.Add(this.btnViewScript);
            this.Controls.Add(this.btnGenScript);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnViewRaw);
            this.Controls.Add(this.btnGenerateCube3);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.mainMenu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mainMenu;
            this.MaximizeBox = false;
            this.Name = "MainEditor";
            this.Text = "Cube3Editor";
            this.Load += new System.EventHandler(this.MainEditor_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainEditor_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainEditor_DragEnter);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.tabRetractrionControl.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.tabTemperatureControl.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tbPressure.ResumeLayout(false);
            this.tbPressure.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPressureMod)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbLeftMaterial;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbLeftColor;
        private System.Windows.Forms.ComboBox cbRightColor;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbRightMaterial;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ComboBox cbCubeProMaterial;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cbCubeProColor;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnGenerateCube3;
        private System.Windows.Forms.Button btnViewRaw;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem abouitToolStripMenuItem;
        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.TabPage tabRetractrionControl;
        private System.Windows.Forms.GroupBox groupBox7;
        private SourceGrid.Grid gridRetractionStop;
        private System.Windows.Forms.GroupBox groupBox6;
        private SourceGrid.Grid gridRetractionStart;
        private System.Windows.Forms.TabPage tabTemperatureControl;
        private SourceGrid.Grid gridLeftTemps;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnLeftCalculate;
        private System.Windows.Forms.Button btnLeftTempUpdate;
        private System.Windows.Forms.GroupBox groupBox4;
        private SourceGrid.Grid gridRightTemps;
        private System.Windows.Forms.Button btnRightTempUpdate;
        private System.Windows.Forms.Button btnRightCalculate;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tbPressure;
        private SourceGrid.Grid gridPressure;
        private System.Windows.Forms.Button btnGenScript;
        private System.Windows.Forms.Button btnViewScript;
        private System.Windows.Forms.SaveFileDialog saveScriptDialog;
        private System.Windows.Forms.SaveFileDialog saveBFBFile;
        private System.Windows.Forms.ToolStripMenuItem loadScriptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generateScriptToolStripMenuItem1;
        private System.Windows.Forms.OpenFileDialog fdLoadScript;
        private System.Windows.Forms.ToolStripMenuItem preferencesToolStripMenuItem;
        private System.Windows.Forms.CheckBox cbLeftSupport;
        private System.Windows.Forms.CheckBox cbLeftSidewalk;
        private System.Windows.Forms.CheckBox cbRightSupport;
        private System.Windows.Forms.CheckBox cbRightSidewalk;
        private System.Windows.Forms.CheckBox cbCubeProSupport;
        private System.Windows.Forms.CheckBox cbCubeProSidewalk;
        private System.Windows.Forms.ComboBox cbPrinterModel;
        private System.Windows.Forms.ComboBox cbMinFirmware;
        private System.Windows.Forms.ComboBox cbFirmware;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Button btnClearSelection;
        private System.Windows.Forms.Button btnCalculate;
        private System.Windows.Forms.NumericUpDown nudPressureMod;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnUpdate;
    }
}

