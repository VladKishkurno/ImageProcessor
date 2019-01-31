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
    public class Photo : IWebGPSDecoder
    {
        private FileInfo FileInfo;
        private string Name;
        private Lazy<Image> _image;

        public Photo (FileInfo fileInfo)
        {
            this.FileInfo = fileInfo;
            this.Name = fileInfo.Name;
            _image = new Lazy<Image>(() =>
            {
                return Image.FromFile(this.FileInfo.FullName);
            });
        }
        public Image MyImage
        {
            get
            {
                return _image.Value;
            }
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


        public string WebRequestToService(string GPS)
        {
            WebRequest request = WebRequest.Create($"https://geocode-maps.yandex.ru/1.x/?geocode={GPS}");
            WebResponse response = request.GetResponse();
            using (Stream stream = response.GetResponseStream())
            {
                XDocument doc = XDocument.Load(stream);
                response.Close();

                foreach (XElement el in doc.Descendants())
                {
                    if (el.Name.LocalName == "text")
                    {
                        return el.Value;
                    }
                }
            }
            return "";
        }
    }
}
