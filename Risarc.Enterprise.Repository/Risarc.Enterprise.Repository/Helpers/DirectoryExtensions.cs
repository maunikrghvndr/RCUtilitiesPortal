using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Risarc.Enterprise.Repository.Helpers
{
    public class DirectoryExtensions
    {
        public bool exists;
        public string path;

        public static DirectoryExtensions Validate(string path)
        {
            if (path.LastIndexOf("\\") + 1 != path.Length && path.LastIndexOf("/") + 1 != path.Length)
                path += "\\";
            return new DirectoryExtensions()
            {
                path = path,
                exists = Directory.Exists(path)
            };
        }

        public static bool IsFileLocked(FileInfo file)
        {
            FileStream fileStream = (FileStream)null;
            try
            {
                fileStream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException ex)
            {
                return true;
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (fileStream != null)
                    fileStream.Close();
            }
            return false;
        }
    }
}
