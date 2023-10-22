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
        public string CheckedFile;
        public static readonly List<string> listExt = new List<string>() { ".mp4",".mkv",".png", ".jpg", ".jpeg", ".wav", ".mp3", ".ogg", null};

        public bool CheckFile(string fileName, string directory)
        {
            foreach (var myext in listExt)
            {
                FileInfo file = new FileInfo(directory.Trim() + fileName.Trim() + myext);
                if (file.Exists)
                {
                    CheckedFile = Convert.ToString(file.FullName);
                    return true;
                }
            }
            return false;
        }
    }
}
