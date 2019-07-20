using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileHelper
{
    public static class FileBackup
    {
        /// <summary>
        /// Make a numbered backup copy of the specified files.  Backup files have the name filename.exe.yymmdd##, where yymmdd is the date and ## is a zero justified sequence number starting at 1
        /// </summary>
        /// <param name="fileName">Name of the file to backup.</param>
        /// <param name="maxBackups">The maximum backups to keep.</param>
        public static void MakeBackup(string fileName, int maxBackups)
        {
            // Make sure that the file exists, you don't backup a new file
            if (File.Exists(fileName))
            {
                // First backup copy of the day starts at 1
                int newSequence = 1;

                String pathDirectoryName = Path.GetDirectoryName(fileName);
                if (pathDirectoryName.Length == 0)
                {
                    pathDirectoryName = Directory.GetCurrentDirectory();
                }

                String pathFileName = Path.GetFileNameWithoutExtension(fileName);
                String pathExtension = Path.GetExtension(fileName);

                // Get the list of previous backups of the file, skipping the current file
                var backupFiles = Directory.GetFiles(pathDirectoryName, pathFileName + "*" + pathExtension)
                    .ToList()
                    .Where(d => !d.Equals(fileName))
                    .OrderBy(d => d);

                // Get the name of the last backup performed
                var lastBackupFilename = backupFiles.LastOrDefault();

                // If we have at least one previous backup copy
                if (lastBackupFilename != null)
                {
                    // Get the last sequence number back taking the last 2 characters and convert them to an int. And add 1 to that number
                    if (Int32.TryParse(Path.GetFileNameWithoutExtension(lastBackupFilename).GetLast(4), out newSequence))
                        newSequence++;

                    // If we have more backups than we need to keep
                    if (backupFiles.Count() >= maxBackups)
                    {
                        // Get a list of the oldest files to delele
                        var expiredFiles = backupFiles.Take(backupFiles.Count() - maxBackups + 1);

                        foreach (var expiredFile in expiredFiles)
                        {
                            File.Delete(expiredFile);
                        }
                    }
                }

                // Create the file name for the newest back up file.
                //var latestBackup = String.Format("{0}.{1:yyMMdd}{2:00}", fileName, DateTime.Now, newSequence);
                var latestBackup = String.Format("{0}{1}{2}_{3:0000}{4}",
                                            pathDirectoryName, Path.DirectorySeparatorChar.ToString(), pathFileName,
                                            newSequence, pathExtension);

                // Copy the current file to the new backup name and overwrite any existing copy
                File.Copy(fileName, latestBackup, true);
            }
        }

        public static void DeleteLastBackup(string fileName)
        {
            // Make sure that the file exists, you don't backup a new file
            if (File.Exists(fileName))
            {
                // First backup copy of the day starts at 1
                int newSequence = 1;

                String pathDirectoryName = Path.GetDirectoryName(fileName);
                if (pathDirectoryName.Length == 0)
                {
                    pathDirectoryName = Directory.GetCurrentDirectory();
                }

                String pathFileName = Path.GetFileNameWithoutExtension(fileName);
                String pathExtension = Path.GetExtension(fileName);

                // Get the list of previous backups of the file, skipping the current file
                var backupFiles = Directory.GetFiles(pathDirectoryName, pathFileName + "*" + pathExtension)
                    .ToList()
                    .Where(d => !d.Equals(fileName))
                    .OrderBy(d => d);

                // Get the name of the last backup performed
                var lastBackupFilename = backupFiles.LastOrDefault();

                // If we have at least one previous backup copy
                if (lastBackupFilename != null)
                {
                    // Get the last sequence number back taking the last 2 characters and convert them to an int. And add 1 to that number
                    Int32.TryParse(Path.GetFileNameWithoutExtension(lastBackupFilename).GetLast(4), out newSequence);
                }

                // Create the file name for the newest back up file.
                //var latestBackup = String.Format("{0}.{1:yyMMdd}{2:00}", fileName, DateTime.Now, newSequence);
                var latestBackup = String.Format("{0}{1}{2}_{3:0000}{4}",
                                            pathDirectoryName, Path.DirectorySeparatorChar.ToString(), pathFileName,
                                            newSequence, pathExtension);

                // Delete the latest backup copy
                File.Delete(latestBackup);
            }
        }
    }


    // String Extension that was used in the code but left out when I first published
    public static class StringExtension
    {
        public static string GetLast(this string source, int tail_length)
        {
            if (tail_length >= source.Length)
                return source;
            return source.Substring(source.Length - tail_length);
        }
    }
}
