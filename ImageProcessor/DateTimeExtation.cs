using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessor
{
    public static class DateTimeExtation
    {
        public static string DateTimeToString(this DateTime date)
        {
            return $@"{date.Day}_{date.Month}_{date.Year}_{date.Hour}_{date.Minute}_{date.Second}";
        }

        public static string GetYear(this DateTime date)
        {
            return $@"{date.Year}";
        }
    }

}
