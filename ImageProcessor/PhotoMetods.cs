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
        public void SaveAs(string pathToFolder)
        {
            Save(pathToFolder);
        }

        public void SaveAs(string pathToFolder, string name)
        {
            Save(pathToFolder, name);
        }

        private void Save(params string[] listString)
        {
            string destFolder = null;
            int i = 1;

            if(!Directory.Exists(listString[0]))
            {
                Directory.CreateDirectory(listString[0]);
            }

            if (listString.Length > 1)
            {
                destFolder = $@"{listString[0]}\{listString[1]}{_fileInfo.Extension}";
                while (File.Exists(destFolder))
                {
                    destFolder = $@"{listString[0]}\{listString[1]}({i}){_fileInfo.Extension}";
                    i++;
                }
            }
            else
            {
                destFolder = $@"{listString[0]}\{_fileInfo.Name}";
            }


            _image.Value.Save(destFolder, ImageFormat.Jpeg);
        }
        private string WebRequestToService(string GPS)
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

        private DateTime GetImageDate()
        {
            try
            {
                PropertyItem propItem = _image.Value.GetPropertyItem(36867);
                string dateTaken = new System.Text.RegularExpressions.Regex(":").Replace(System.Text.Encoding.UTF8.GetString(propItem.Value), "-", 2);
                return DateTime.Parse(dateTaken);
            }
            catch
            {
                return _fileInfo.CreationTime;
            }

        }

        private string GetGPS()
        {
            try
            {
                string gpsLatitudeRef = BitConverter.ToChar(_image.Value.GetPropertyItem(1).Value, 0).ToString();
                string latitude = GPSdecoder.GPSdecoder.DecodeGPS(_image.Value.GetPropertyItem(2));
                string gpsLongitudeRef = BitConverter.ToChar(_image.Value.GetPropertyItem(3).Value, 0).ToString();
                string longitude = GPSdecoder.GPSdecoder.DecodeGPS(_image.Value.GetPropertyItem(4));
                return $"{gpsLatitudeRef} {latitude} {gpsLongitudeRef} {longitude}";
            }
            catch
            {
                return null;
            }
        }
    }
}
