using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cube3Editor
{
    public partial class PreferencesDialog : Form
    {

        private Preferences prefs;

        public PreferencesDialog(Preferences newPrefs)
        {
            InitializeComponent();

            prefs = newPrefs;

            cbPreserve.Checked = prefs.PreserveOriginalCube3;
            cbCreateBackup.Checked = prefs.CreateBackupFiles;
            ndBackupCount.Value = prefs.MaxNumberBackupFiles;
            cbZeroTempAllowed.Checked = prefs.AllowZeroTemperatures;
            ndMaxTemperature.Value = prefs.MaxTemperatureValue;
            cbMinimizeSize.Checked = prefs.MinimizeCube3Size;

        }

        private void Preferences_Load(object sender, EventArgs e)
        {
            if (cbCreateBackup.Checked)
            {
                ndBackupCount.Enabled = true;
            }
            else
            {
                ndBackupCount.Enabled = false;
            }
            btnApply.Enabled = false;
        }

        private void CbPreserve_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void CbCreateBackup_CheckedChanged(object sender, EventArgs e)
        {
            if (cbCreateBackup.Checked)
            {
                ndBackupCount.Enabled = true;
            }
            else
            {
                ndBackupCount.Enabled = false;
            }
        }

        private void NdBackupCount_ValueChanged(object sender, EventArgs e)
        {

        }

        private void CbZeroTempAllowed_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void CbMinimizeSize_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void BtnApply_Click(object sender, EventArgs e)
        {
            btnApply.Enabled = false;
            prefs.PreserveOriginalCube3 = cbPreserve.Checked;
            prefs.CreateBackupFiles = cbCreateBackup.Checked;
            prefs.MaxNumberBackupFiles = (int)ndBackupCount.Value;
            prefs.AllowZeroTemperatures = cbZeroTempAllowed.Checked;
            prefs.MaxTemperatureValue = (int)ndMaxTemperature.Value;
            prefs.MinimizeCube3Size = cbMinimizeSize.Checked;
            prefs.Save();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PreferencesModified()
        {
            btnApply.Enabled = true;
        }

        private void CbMinimizeSize_CheckStateChanged(object sender, EventArgs e)
        {
            PreferencesModified();
        }

        private void CbZeroTempAllowed_CheckStateChanged(object sender, EventArgs e)
        {
            PreferencesModified();
        }

        private void CbCreateBackup_CheckStateChanged(object sender, EventArgs e)
        {
            PreferencesModified();
        }

        private void CbPreserve_CheckStateChanged(object sender, EventArgs e)
        {
            PreferencesModified();
        }
    }
}
