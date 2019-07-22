using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cube3Editor
{
    public partial class ScriptViewForm : Form
    {
        public bool applyScript { get; private set; }

        public ScriptViewForm(List<String> scriptLines)
        {
            InitializeComponent();

            applyScript = false;
            btnApply.Enabled = false;

            string seperator = "\r\n";
            rtbScript.Text = string.Join(seperator, scriptLines);

            Font defaultFont = new Font(FontFamily.GenericMonospace, 14.0F, FontStyle.Regular, GraphicsUnit.Pixel);
            rtbScript.Font = defaultFont;

        }

        private void BtnSaveScript_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "Cube Script Files|*.cubescr|All Files (*.*)|*.*";
            //saveFileDialog1.ShowDialog(); //Opens the Show File Dialog  
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK) //Check if it's all ok  
            {
                string name = Path.GetFileNameWithoutExtension(saveFileDialog1.FileName) + ".cubescr"; //Just to make sure the extension is .txt  

                char[] seperator = { '\r', '\n' };
                string[] tbLines = rtbScript.Text.Split(seperator, StringSplitOptions.RemoveEmptyEntries);
                try
                {
                    using (var outFile = File.OpenWrite(name))
                    using (var bfbWriter = new StreamWriter(outFile))
                    {
                        foreach (string line in tbLines)
                        {
                            bfbWriter.WriteLine(line);
                        }
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show($"Unable to write to file '{name}'");
                }
            }

        }

        private void BtnFont_Click(object sender, EventArgs e)
        {
            //fontDialog1.ShowDialog(); //Shows the font dialog               //fontDialog1.ShowDialog(); //Shows the font dialog   
            if (fontDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                rtbScript.Font = fontDialog1.Font; //Sets the font to the one selected in the dialog  
            }

        }

        private void CbApplyScript_CheckedChanged(object sender, EventArgs e)
        {
            applyScript = true;
            btnApply.Enabled = false;
        }

        private void RtbScript_TextChanged(object sender, EventArgs e)
        {
            btnApply.Enabled = true;
        }
    }
}
