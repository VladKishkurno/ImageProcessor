using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Serialization;
using Yandex;
using Yandex.Geocoder;
using Yandex.Geocoder.Raw;

namespace ImageProcessor
{
    public static class ImageProcessor
    {
        public static void RenamePhoto(string PathToPhoto)
        {
            var dirInfo = new DirectoryInfo(PathToPhoto);
            List<Photo> photos = new List<Photo>();

            foreach (var file in dirInfo.GetFiles("*.jpg"))
            {
                photos.Add(new Photo(file));
            }

            DirectoryInfo parent = dirInfo.Parent;
            string destDir = $@"{parent.FullName}\{dirInfo.Name}_RenamePhoto";
            Directory.CreateDirectory(destDir);

            int count = 1;

            foreach (var photo in photos)
            {
                Console.WriteLine($"{count}/{photos.Count}");
                Image image = Image.FromFile(photo.GetFileInfo.FullName);
                
                string date = DateTimeToString(GetImageDate(photo));

                string destFolder = $@"{destDir}\{date}{photo.GetFileInfo.Extension}";
                if (File.Exists(destFolder))
                {
                    string newDestFolder = $@"{destDir}\{date}({count}){photo.GetFileInfo.Extension}";

                    image.Save($"{newDestFolder}", ImageFormat.Jpeg);
                }
                else
                {
                    image.Save(destFolder, ImageFormat.Jpeg);
                }

                image.Dispose();
                count++;
            }
        }

        public static void AddDateOnPhoto(string PathToPhoto)
        {
            var dirInfo = new DirectoryInfo(PathToPhoto);
            List<Photo> photos = new List<Photo>();

            foreach (var file in dirInfo.GetFiles("*.jpg"))
            {
                photos.Add(new Photo(file));
            }

            DirectoryInfo parent = dirInfo.Parent;
            string destDir = $@"{parent.FullName}\{dirInfo.Name}_AddDateOnPhoto";
            Directory.CreateDirectory(destDir);

            int count = 1;

            foreach (var photo in photos)
            {
                Console.WriteLine($"{count}/{photos.Count}");
                Image image = Image.FromFile(photo.GetFileInfo.FullName);
                Graphics g = Graphics.FromImage(image);

                string date = DateTimeToString(GetImageDate(photo));

                g.DrawString(date, new Font("Tahoma", 8), Brushes.White, new System.Drawing.Point(image.Width - 300, 10));

                string destFolder = $@"{destDir}\{photo.GetName}{photo.GetFileInfo.Extension}";

                image.Save(destFolder, ImageFormat.Jpeg);

                image.Dispose();
                count++;
            }
        }

        public static void SortOnYear(string PathToPhoto)
        {
            var dirInfo = new DirectoryInfo(PathToPhoto);
            List<Photo> photos = new List<Photo>();

            foreach (var file in dirInfo.GetFiles("*.jpg"))
            {
                photos.Add(new Photo(file));
            }

            DirectoryInfo parent = dirInfo.Parent;
            string destDir = $@"{parent.FullName}\{dirInfo.Name}_SortOnYear";
            Directory.CreateDirectory(destDir);

            int count = 1;

            foreach (var photo in photos)
            {
                Console.WriteLine($"{count}/{photos.Count}");
                Image image = Image.FromFile(photo.GetFileInfo.FullName);

                string year = GetYear(GetImageDate(photo));

                string destFolder = $@"{destDir}\{year}";

                if (!Directory.Exists(destFolder))
                {
                    Directory.CreateDirectory(destFolder);
                }

                image.Save($@"{destFolder}\{ photo.GetName}{ photo.GetFileInfo.Extension}", ImageFormat.Jpeg);

                image.Dispose();
                count++;
            }
        }

        public static void SortOnGPS(string PathToPhoto)
        {
            var dirInfo = new DirectoryInfo(PathToPhoto);
            List<Photo> photos = new List<Photo>();

            foreach (var file in dirInfo.GetFiles("*.jpg"))
            {
                photos.Add(new Photo(file));
            }

            DirectoryInfo parent = dirInfo.Parent;
            string destDir = $@"{parent.FullName}\{dirInfo.Name}_SortOnGPS";
            Directory.CreateDirectory(destDir);

            int count = 1;

            foreach (var photo in photos)
            {
                Console.WriteLine($"{count}/{photos.Count}");
                Image image = Image.FromFile(photo.GetFileInfo.FullName);

                string GPS = ImageProcessor.GetGPS(photo);
                string result = null;

                if (GPS != null)
                {
                    WebRequest request = WebRequest.Create($"https://geocode-maps.yandex.ru/1.x/?geocode={GPS}");
                    WebResponse response = request.GetResponse();
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string read = $@"{reader.ReadToEnd()}";
                            var adapter = new StreamWriter("D:/History.xml");

                            adapter.Write(read);
                            adapter.Dispose();

                            XmlSerializer xmlSerializer = new XmlSerializer(typeof(GeoObjectCollection));

                            using (XmlReader xmlreader = XmlReader.Create("D:/History.xml"))
                            {
                                while (xmlreader.Read())
                                {
                                    if (xmlreader.IsStartElement())
                                    {
                                        if (xmlreader.Name == "text")
                                        {
                                            xmlreader.Read();
                                            result = xmlreader.Value;
                                            break;
                                        }
                                    }
                                }
                            }
                            adapter.Dispose();
                            adapter.Close();
                        }
                    }
                    response.Close();

                    string destFolder = $@"{destDir}\{result}";

                    if (!Directory.Exists(destFolder))
                    {
                        Directory.CreateDirectory(destFolder);
                    }

                    image.Save($@"{destFolder}\{photo.GetName}{ photo.GetFileInfo.Extension}", ImageFormat.Jpeg);

                }


                image.Dispose();
                count++;
            }
        }

        public static string DateTimeToString(DateTime date)
        {
            return $@"{date.Day}_{date.Month}_{date.Year}_{date.Hour}_{date.Minute}_{date.Second}";
        }

        public static string GetYear(DateTime date)
        {
            return $@"{date.Year}";
        }

        public static DateTime GetImageDate(Photo photo)
        {
            try
            {
                Image image = Image.FromFile(photo.GetFileInfo.FullName);
                PropertyItem propItem = image.GetPropertyItem(36867);
                string dateTaken = new System.Text.RegularExpressions.Regex(":").Replace(System.Text.Encoding.UTF8.GetString(propItem.Value), "-", 2);
                return DateTime.Parse(dateTaken);
            }
            catch
            {
                return photo.GetFileInfo.CreationTime;
            }
        }

        public static string GetGPS(Photo photo)
        {
            try
            {
                Image image = Image.FromFile(photo.GetFileInfo.FullName);
                string gpsLatitudeRef = BitConverter.ToChar(image.GetPropertyItem(1).Value, 0).ToString();
                string latitude = DecodeGPS(image.GetPropertyItem(2));
                string gpsLongitudeRef = BitConverter.ToChar(image.GetPropertyItem(3).Value, 0).ToString();
                string longitude = DecodeGPS(image.GetPropertyItem(4));
                return $"{gpsLatitudeRef} {latitude} {gpsLongitudeRef} {longitude}";
            }
            catch
            {
                return null;
            }
        }

        private static string DecodeGPS(PropertyItem propertyItem)
        {
            uint dN = BitConverter.ToUInt32(propertyItem.Value, 0);
            uint dD = BitConverter.ToUInt32(propertyItem.Value, 4);
            uint mN = BitConverter.ToUInt32(propertyItem.Value, 8);
            uint mD = BitConverter.ToUInt32(propertyItem.Value, 12);
            uint sN = BitConverter.ToUInt32(propertyItem.Value, 16);
            uint sD = BitConverter.ToUInt32(propertyItem.Value, 20);

            decimal deg;
            decimal min;
            decimal sec;
            
            if (dD > 0) { deg = (decimal)dN / dD; } else { deg = dN; }
            if (mD > 0) { min = (decimal)mN / mD; } else { min = mN; }
            if (sD > 0) { sec = (decimal)sN / sD; } else { sec = sN; }

            if (sec == 0) return string.Format("{0}° {1:0.###}'", deg, min);
            else return string.Format("{0}° {1:0}' {2:0.#}\"", deg, min, sec);
        }
    }
}
