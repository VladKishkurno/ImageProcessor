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

            int count = 1;

            foreach (var photo in photos)
            {
                Console.WriteLine($"{count}/{photos.Count}");

                string date = photo.GetDate;

                photo.SaveAs(destDir, date);    

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

            int count = 1;

            foreach (var photo in photos)
            {
                Console.WriteLine($"{count}/{photos.Count}");

                Graphics g = Graphics.FromImage(photo.MyImage);

                string date = photo.GetDate;

                g.DrawString(date, new Font("Tahoma", 8), Brushes.DarkRed, new System.Drawing.Point((photo.MyImage.Width - 350), 10));

                photo.SaveAs(destDir);

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

            int count = 1;

            foreach (var photo in photos)
            {
                Console.WriteLine($"{count}/{photos.Count}");

                string year = photo.GetYear;

                string destFolder = $@"{destDir}\{year}";

                photo.SaveAs(destFolder);

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

            int count = 1;

            foreach (var photo in photos)
            {
                Console.WriteLine($"{count}/{photos.Count}");

                var result = photo.Location;

                if(result != "")
                { 
                    string destFolder = $@"{destDir}\{result}";

                    photo.SaveAs(destFolder);
                }

                photo.MyImage.Dispose();
                count++;

            }

        }
    }

}
   

