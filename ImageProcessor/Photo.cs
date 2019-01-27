using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ImageProcessor
{
    public class Photo
    {
        private FileInfo FileInfo;
        private string Name;

        public Photo (FileInfo fileInfo)
        {
            this.FileInfo = fileInfo;
            this.Name = fileInfo.Name;
        }
        public FileInfo GetFileInfo
        {
            get
            {
                return FileInfo;
            }
        }

        public string GetName
        {
            get
            {
                return Name;
            }
        }
    }
}
