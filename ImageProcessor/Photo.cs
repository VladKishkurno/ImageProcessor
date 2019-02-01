using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Linq;
using System.Drawing;
using System.Drawing.Imaging;

namespace ImageProcessor
{
    public partial class Photo
    {
        private FileInfo _fileInfo;
        private string _name;
        private Lazy<Image> _image;
        private Lazy<string> _location;

        public Photo (FileInfo fileInfo)
        {
            this._fileInfo = fileInfo;
            this._name = fileInfo.Name;

            _image = new Lazy<Image>(() =>
            {
                return Image.FromFile(this._fileInfo.FullName);
            });

            this._location = new Lazy<string>(() =>
            {
                return WebRequestToService(GetGPS());
            });
        }
        public Image MyImage
        {
            get
            {
                return _image.Value;
            }
        }

        public string Location
        {
            get
            {
                return _location.Value;
            }
        }

        public string GetDate
        {
            get
            {
                return GetImageDate().DateTimeToString();
            }
        }

        public string GetYear
        {
            get
            {
                return GetImageDate().GetYear();
            }
        }
        public FileInfo GetFileInfo
        {
            get
            {
                return _fileInfo;
            }
        }

        public string GetName
        {
            get
            {
                return _name;
            }
        }
    }
}
