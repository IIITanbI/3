namespace QA.TestLibs.FileManager
{
    using QA.TestLibs.Commands;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [CommandManager(null, Description = "Manager for file")]
    public class FileManager : CommandManagerBase
    {
        public FileManager()
            :base(null)
        { }

        [Command("Copy directory", Description = "Copy directory include subdirectories")]
        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs, ILogger log)
        {
            try
            {
                log?.DEBUG($"Copy directory include subdirectories");
                DirectoryInfo dir = new DirectoryInfo(sourceDirName);
                DirectoryInfo[] dirs = dir.GetDirectories();
                FileInfo[] files = dir.GetFiles();

                foreach (FileInfo file in files)
                {
                    string temppath = Path.Combine(destDirName, file.Name);
                    file.CopyTo(temppath, false);
                }

                if (copySubDirs)
                {
                    foreach (DirectoryInfo subdir in dirs)
                    {
                        string temppath = Path.Combine(destDirName, subdir.Name);
                        DirectoryCopy(subdir.FullName, temppath, copySubDirs, log);
                    }
                }
                log?.DEBUG($"Copying directory include subdirectories completed");
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during copying directory include subdirectories");
                throw new CommandAbortException($"Error occurred during copying directory include subdirectories", ex);
            }
        }

    }
}
