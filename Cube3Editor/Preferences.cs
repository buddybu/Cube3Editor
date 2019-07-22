using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cube3Editor
{
    public class Preferences
    {
        private bool preserveOriginalCube3;
        private bool createBackupFiles;
        private int maxNumberBackupFiles;
        private bool allowZeroTemperatures;
        private int maxTemperatureValue;
        private bool minimizeCube3Size;

        public bool PreserveOriginalCube3 { get => preserveOriginalCube3; set => preserveOriginalCube3 = value; }
        public bool CreateBackupFiles { get => createBackupFiles; set => createBackupFiles = value; }
        public int MaxNumberBackupFiles { get => maxNumberBackupFiles; set => maxNumberBackupFiles = value; }
        public bool AllowZeroTemperatures { get => allowZeroTemperatures; set => allowZeroTemperatures = value; }
        public int MaxTemperatureValue { get => maxTemperatureValue; set => maxTemperatureValue = value; }
        public bool MinimizeCube3Size { get => minimizeCube3Size; set => minimizeCube3Size = value; }

        public Preferences()
        {
            PreserveOriginalCube3 = Properties.Settings.Default.PreserveOriginalCube3File;
            CreateBackupFiles = Properties.Settings.Default.CreateBackupFiles;
            MaxNumberBackupFiles = Properties.Settings.Default.MaxBackupFiles;
            AllowZeroTemperatures = Properties.Settings.Default.ZeroTemperatureAllowed;
            MaxTemperatureValue = Properties.Settings.Default.MaxTemperatureAllowed;
            MinimizeCube3Size = Properties.Settings.Default.MinimizeCube3FileSize;
        }

        public void Save()
        {
            Properties.Settings.Default.PreserveOriginalCube3File = PreserveOriginalCube3;
            Properties.Settings.Default.CreateBackupFiles = CreateBackupFiles;
            Properties.Settings.Default.MaxBackupFiles = MaxNumberBackupFiles;
            Properties.Settings.Default.ZeroTemperatureAllowed = AllowZeroTemperatures;
            Properties.Settings.Default.MaxTemperatureAllowed = MaxTemperatureValue;
            Properties.Settings.Default.MinimizeCube3FileSize = MinimizeCube3Size;
            Properties.Settings.Default.Save();
        }

        public void Load()
        {
            PreserveOriginalCube3 = Properties.Settings.Default.PreserveOriginalCube3File;
            CreateBackupFiles = Properties.Settings.Default.CreateBackupFiles;
            MaxNumberBackupFiles = Properties.Settings.Default.MaxBackupFiles;
            AllowZeroTemperatures = Properties.Settings.Default.ZeroTemperatureAllowed;
            MaxTemperatureValue = Properties.Settings.Default.MaxTemperatureAllowed;
            MinimizeCube3Size = Properties.Settings.Default.MinimizeCube3FileSize;
        }

    }
}
