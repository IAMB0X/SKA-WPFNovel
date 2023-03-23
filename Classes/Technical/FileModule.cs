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
        public string checkedFile;
        public static readonly List<string> listExt = new List<string>() { ".mp4",".mkv",".png", ".jpg", ".jpeg", ".wav", ".mp3", ".ogg", null};

        public bool FileCheck(string fileName, string directory)
        {
            List<string> ext = listExt;
            bool isMatch = false;
            foreach (var myext in ext)
            {
                FileInfo file = new FileInfo(directory + fileName + myext);
                isMatch = file.Exists;
                if (isMatch)
                {
                    checkedFile = Convert.ToString(file.FullName);
                    return isMatch;
                }
            }

            return isMatch;
        }
    }
}
