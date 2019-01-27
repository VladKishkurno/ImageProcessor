using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            string PathToPhoto = @"D:\Download\New wallpapers №7";
            ImageProcessor.SortOnGPS(PathToPhoto);
            Console.ReadKey();
        }
    }
}
