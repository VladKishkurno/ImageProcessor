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
using System.Xml.Linq;

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
                
                string date = image.GetImageDate(photo.GetFileInfo).DateTimeToString();

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

                string date = image.GetImageDate(photo.GetFileInfo).DateTimeToString();

                g.DrawString(date, new Font("Tahoma", 8), Brushes.DarkRed, new System.Drawing.Point((image.Width - 350), 10));

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

                string year = image.GetImageDate(photo.GetFileInfo).GetYear();

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
            bool flag = false;
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

                string GPS = image.GetGPS();
                string result = null;

                if (GPS != null)
                {
                    WebRequest request = WebRequest.Create($"https://geocode-maps.yandex.ru/1.x/?geocode={GPS}");
                    WebResponse response = request.GetResponse();
                    using (Stream stream = response.GetResponseStream())
                    {
                        XDocument doc = XDocument.Load(stream);
                        
                        Console.WriteLine(doc.Element("ymaps"));

                        foreach (XElement el in doc.Root.Elements()) // заходим в корневой каталог
                        {
                           foreach (XElement element in el.Elements())  // ymaps
                            {
                                if (element.Name.LocalName == "featureMember")  // ищем featureMember
                                {
                                    foreach(XElement node in element.Elements()) // featureMember
                                    {
                                        if (node.Name.LocalName == "GeoObject")
                                        {
                                            foreach (XElement node2 in node.Elements()) // GeoObject
                                            {
                                                if (node2.Name.LocalName == "metaDataProperty")
                                                {
                                                    foreach (XElement node3 in node2.Elements()) // metaDataProperty
                                                    {
                                                        if(node3.Name.LocalName == "GeocoderMetaData")
                                                        foreach (XElement node4 in node3.Elements()) // GeocoderMetaData
                                                        {
                                                            if (node4.Name.LocalName == "text")
                                                            {
                                                                result = node4.Value;
                                                                goto Finish;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        Finish :

                        string destFolder = $@"{destDir}\{result}";

                        if (!Directory.Exists(destFolder))
                        {
                            Directory.CreateDirectory(destFolder);
                        }

                        image.Save($@"{destFolder}\{photo.GetName}{ photo.GetFileInfo.Extension}", ImageFormat.Jpeg);

                    }
                    response.Close();
                }
                
                    image.Dispose();
                    count++;
            }
              
            }

        }
    }
