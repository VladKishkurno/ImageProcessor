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

            while (true)
            {
                Console.WriteLine("Выберите 'Y' для сортировки фотографий по годам");
                Console.WriteLine("Выберите 'G' для сортировки фотографий по местоположению");
                Console.WriteLine("Выберите 'R' для переименования фотографий  по дате их создания");
                Console.WriteLine("Выберите 'A' для добавления даты снимка на фото");
                Console.WriteLine("Выберите 'E' для выхода из программы");

                switch (Console.ReadKey().KeyChar)
                {
                    case 'G':case 'g':
                        ImageProcessor.SortOnGPS(PathToPhoto);
                        break;

                    case 'Y':  case 'y':
                        ImageProcessor.SortOnYear(PathToPhoto);
                        break;

                    case 'R': case 'r':
                        ImageProcessor.RenamePhoto(PathToPhoto);
                        break;

                    case 'A': case 'a':
                        ImageProcessor.AddDateOnPhoto(PathToPhoto);
                        break;

                    case 'E': case 'e':
                        return;

                    default:
                        Console.WriteLine("Выберите доступную команду");
                        break;
                }
                Console.Clear();
            }
        }
    }
}
