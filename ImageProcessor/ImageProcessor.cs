using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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
                //Image image = Image.FromFile(photo.GetFileInfo.FullName);
                

                string date = photo.MyImage.GetImageDate(photo.GetFileInfo).DateTimeToString();

                string destFolder = $@"{destDir}\{date}{photo.GetFileInfo.Extension}";
                if (File.Exists(destFolder))
                {
                    string newDestFolder = $@"{destDir}\{date}({count}){photo.GetFileInfo.Extension}";

                    photo.MyImage.Save($"{newDestFolder}", ImageFormat.Jpeg);
                }
                else
                {
                    photo.MyImage.Save(destFolder, ImageFormat.Jpeg);
                }

                photo.MyImage.Dispose();
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
                //Image image = Image.FromFile(photo.GetFileInfo.FullName);
                Graphics g = Graphics.FromImage(photo.MyImage);

                string date = photo.MyImage.GetImageDate(photo.GetFileInfo).DateTimeToString();

                g.DrawString(date, new Font("Tahoma", 8), Brushes.DarkRed, new System.Drawing.Point((photo.MyImage.Width - 350), 10));

                string destFolder = $@"{destDir}\{photo.GetName}{photo.GetFileInfo.Extension}";

                photo.MyImage.Save(destFolder, ImageFormat.Jpeg);

                photo.MyImage.Dispose();
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
                //Image image = Image.FromFile(photo.GetFileInfo.FullName);

                string year = photo.MyImage.GetImageDate(photo.GetFileInfo).GetYear();

                string destFolder = $@"{destDir}\{year}";

                if (!Directory.Exists(destFolder))
                {
                    Directory.CreateDirectory(destFolder);
                }

                photo.MyImage.Save($@"{destFolder}\{ photo.GetName}{ photo.GetFileInfo.Extension}", ImageFormat.Jpeg);

                photo.MyImage.Dispose();
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
                //Image image = Image.FromFile(photo.GetFileInfo.FullName);

                string GPS = photo.MyImage.GetGPS();
                string result = null;

                if (GPS != null)
                {
                    result = photo.WebRequestToService(GPS);

                    string destFolder = $@"{destDir}\{result}";

                    if (!Directory.Exists(destFolder))
                    {
                         Directory.CreateDirectory(destFolder);
                    }

                    photo.MyImage.Save($@"{destFolder}\{photo.GetName}{ photo.GetFileInfo.Extension}", ImageFormat.Jpeg);

                }

                photo.MyImage.Dispose();
                count++;

            }

        }
    }

}
   

