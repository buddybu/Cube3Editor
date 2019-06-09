namespace Cube3Editor
{
    partial class FrmRawView
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
            this.tbRawView = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // tbRawView
            // 
            this.tbRawView.Location = new System.Drawing.Point(12, 12);
            this.tbRawView.Multiline = true;
            this.tbRawView.Name = "tbRawView";
            this.tbRawView.ReadOnly = true;
            this.tbRawView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbRawView.Size = new System.Drawing.Size(776, 426);
            this.tbRawView.TabIndex = 0;
            // 
            // FrmRawView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tbRawView);
            this.Name = "FrmRawView";
            this.Text = "BFB Raw View";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbRawView;
    }
}