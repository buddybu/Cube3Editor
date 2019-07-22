namespace Cube3Editor
{
    partial class PreferencesDialog
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
            this.cbPreserve = new System.Windows.Forms.CheckBox();
            this.cbCreateBackup = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ndBackupCount = new System.Windows.Forms.NumericUpDown();
            this.cbZeroTempAllowed = new System.Windows.Forms.CheckBox();
            this.cbMinimizeSize = new System.Windows.Forms.CheckBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.ndMaxTemperature = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ndBackupCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ndMaxTemperature)).BeginInit();
            this.SuspendLayout();
            // 
            // cbPreserve
            // 
            this.cbPreserve.AutoSize = true;
            this.cbPreserve.Checked = true;
            this.cbPreserve.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbPreserve.Location = new System.Drawing.Point(13, 26);
            this.cbPreserve.Name = "cbPreserve";
            this.cbPreserve.Size = new System.Drawing.Size(211, 21);
            this.cbPreserve.TabIndex = 0;
            this.cbPreserve.Text = "Preserve Original Cube3 File";
            this.cbPreserve.UseVisualStyleBackColor = true;
            this.cbPreserve.CheckedChanged += new System.EventHandler(this.CbPreserve_CheckedChanged);
            this.cbPreserve.CheckStateChanged += new System.EventHandler(this.CbPreserve_CheckStateChanged);
            // 
            // cbCreateBackup
            // 
            this.cbCreateBackup.AutoSize = true;
            this.cbCreateBackup.Location = new System.Drawing.Point(13, 51);
            this.cbCreateBackup.Name = "cbCreateBackup";
            this.cbCreateBackup.Size = new System.Drawing.Size(130, 21);
            this.cbCreateBackup.TabIndex = 1;
            this.cbCreateBackup.Text = "Create Backups";
            this.cbCreateBackup.UseVisualStyleBackColor = true;
            this.cbCreateBackup.CheckedChanged += new System.EventHandler(this.CbCreateBackup_CheckedChanged);
            this.cbCreateBackup.CheckStateChanged += new System.EventHandler(this.CbCreateBackup_CheckStateChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(57, 76);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(162, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Number of Backup Files:";
            this.label1.Click += new System.EventHandler(this.Label1_Click);
            // 
            // ndBackupCount
            // 
            this.ndBackupCount.Enabled = false;
            this.ndBackupCount.Location = new System.Drawing.Point(225, 74);
            this.ndBackupCount.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.ndBackupCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ndBackupCount.Name = "ndBackupCount";
            this.ndBackupCount.Size = new System.Drawing.Size(60, 22);
            this.ndBackupCount.TabIndex = 3;
            this.ndBackupCount.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.ndBackupCount.ValueChanged += new System.EventHandler(this.NdBackupCount_ValueChanged);
            // 
            // cbZeroTempAllowed
            // 
            this.cbZeroTempAllowed.AutoSize = true;
            this.cbZeroTempAllowed.Checked = true;
            this.cbZeroTempAllowed.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbZeroTempAllowed.Location = new System.Drawing.Point(12, 97);
            this.cbZeroTempAllowed.Name = "cbZeroTempAllowed";
            this.cbZeroTempAllowed.Size = new System.Drawing.Size(198, 21);
            this.cbZeroTempAllowed.TabIndex = 4;
            this.cbZeroTempAllowed.Text = "Zero Temperature Allowed";
            this.cbZeroTempAllowed.UseVisualStyleBackColor = true;
            this.cbZeroTempAllowed.CheckedChanged += new System.EventHandler(this.CbZeroTempAllowed_CheckedChanged);
            this.cbZeroTempAllowed.CheckStateChanged += new System.EventHandler(this.CbZeroTempAllowed_CheckStateChanged);
            // 
            // cbMinimizeSize
            // 
            this.cbMinimizeSize.AutoSize = true;
            this.cbMinimizeSize.Location = new System.Drawing.Point(13, 143);
            this.cbMinimizeSize.Name = "cbMinimizeSize";
            this.cbMinimizeSize.Size = new System.Drawing.Size(186, 21);
            this.cbMinimizeSize.TabIndex = 5;
            this.cbMinimizeSize.Text = "Minimize Cube3 File Size";
            this.cbMinimizeSize.UseVisualStyleBackColor = true;
            this.cbMinimizeSize.CheckedChanged += new System.EventHandler(this.CbMinimizeSize_CheckedChanged);
            this.cbMinimizeSize.CheckStateChanged += new System.EventHandler(this.CbMinimizeSize_CheckStateChanged);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(475, 190);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnApply
            // 
            this.btnApply.Enabled = false;
            this.btnApply.Location = new System.Drawing.Point(394, 190);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 7;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.BtnApply_Click);
            // 
            // ndMaxTemperature
            // 
            this.ndMaxTemperature.Location = new System.Drawing.Point(225, 120);
            this.ndMaxTemperature.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.ndMaxTemperature.Name = "ndMaxTemperature";
            this.ndMaxTemperature.Size = new System.Drawing.Size(60, 22);
            this.ndMaxTemperature.TabIndex = 9;
            this.ndMaxTemperature.Value = new decimal(new int[] {
            265,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(48, 122);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(171, 17);
            this.label2.TabIndex = 8;
            this.label2.Text = "Max Temperature Allowed";
            // 
            // PreferencesDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(562, 225);
            this.Controls.Add(this.ndMaxTemperature);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.cbMinimizeSize);
            this.Controls.Add(this.cbZeroTempAllowed);
            this.Controls.Add(this.ndBackupCount);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbCreateBackup);
            this.Controls.Add(this.cbPreserve);
            this.Name = "PreferencesDialog";
            this.Text = "Preferences";
            this.Load += new System.EventHandler(this.Preferences_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ndBackupCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ndMaxTemperature)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cbPreserve;
        private System.Windows.Forms.CheckBox cbCreateBackup;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown ndBackupCount;
        private System.Windows.Forms.CheckBox cbZeroTempAllowed;
        private System.Windows.Forms.CheckBox cbMinimizeSize;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.NumericUpDown ndMaxTemperature;
        private System.Windows.Forms.Label label2;
    }
}