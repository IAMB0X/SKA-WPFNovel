using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SKA_Novel.Classes.Technical
{
    internal class FileModule
    {
        public bool fileExists;
        public string checkedFile;
        public static readonly List<string> musicExt = new List<string>() { ".wav", ".mp3", ".ogg", ""};
        
        public void FileCheck(string fileName, string directory)
        {
            List<string> ext = musicExt;
            int errors = 0;
            foreach (var myext in ext)
            {
                FileInfo file = new FileInfo(directory + fileName + myext);
                if (file.Exists)
                {
                    checkedFile = Convert.ToString(file.FullName);
                    break;
                }
                else
                    errors++;
            }
            if (errors == ext.Count)
            {
                fileExists = false;
                return;
            }
            else
            {
                fileExists = true;
                errors = 0;
            }
        }
    }
}
