using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using GPSdecoder;

namespace ImageProcessor
{
    public static class ImageExtention
    {
        public static DateTime GetImageDate(this Image image, FileInfo fileInfo)
        {
            try
            {
                PropertyItem propItem = image.GetPropertyItem(36867);
                string dateTaken = new System.Text.RegularExpressions.Regex(":").Replace(System.Text.Encoding.UTF8.GetString(propItem.Value), "-", 2);
                return DateTime.Parse(dateTaken);
            }
            catch
            {
                return fileInfo.CreationTime;
            }

        }

        public static string GetGPS(this Image image)
        {
            try
            { 
                string gpsLatitudeRef = BitConverter.ToChar(image.GetPropertyItem(1).Value, 0).ToString();
                string latitude = GPSdecoder.GPSdecoder.DecodeGPS(image.GetPropertyItem(2));
                string gpsLongitudeRef = BitConverter.ToChar(image.GetPropertyItem(3).Value, 0).ToString();
                string longitude = GPSdecoder.GPSdecoder.DecodeGPS(image.GetPropertyItem(4));
                return $"{gpsLatitudeRef} {latitude} {gpsLongitudeRef} {longitude}";
            }
            catch
            {
                return null;
            }
        }
    }
}
