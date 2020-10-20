using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ResumeScrape
{
    public static class MyExtensions
    {
        public static DateTime GetDate(this String str)
        {
            if (str.Length == 4)
            {
                DateTime dt;
                if (DateTime.TryParseExact(str, "yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                    return dt;
                else
                    Console.WriteLine("DATE ERROR!!!! " + str);
                return DateTime.Now;
            }
            else
            {
                if (!str.Contains("Present"))
                    return DateTime.Parse(str);
                else
                    return DateTime.Now;
            }
        }
    }
}
