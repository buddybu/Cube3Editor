namespace Cube3Editor
{
    partial class XmlViewForm
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
            this.xmlEditor = new XmlEditor();
            this.btnXMLApply = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // xmlEditor
            // 
            this.xmlEditor.AllowXmlFormatting = true;
            this.xmlEditor.Location = new System.Drawing.Point(3, 51);
            this.xmlEditor.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.xmlEditor.Name = "xmlEditor";
            this.xmlEditor.ReadOnly = true;
            this.xmlEditor.Size = new System.Drawing.Size(796, 396);
            this.xmlEditor.TabIndex = 0;
            // 
            // btnXMLApply
            // 
            this.btnXMLApply.Location = new System.Drawing.Point(655, 10);
            this.btnXMLApply.Name = "btnXMLApply";
            this.btnXMLApply.Size = new System.Drawing.Size(133, 31);
            this.btnXMLApply.TabIndex = 1;
            this.btnXMLApply.Text = "Apply Changes";
            this.btnXMLApply.UseVisualStyleBackColor = true;
            // 
            // XmlViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnXMLApply);
            this.Controls.Add(this.xmlEditor);
            this.Name = "XmlViewForm";
            this.Text = "XML View";
            this.ResumeLayout(false);

        }

        #endregion

        public XmlEditor xmlEditor;
        private System.Windows.Forms.Button btnXMLApply;
    }
}