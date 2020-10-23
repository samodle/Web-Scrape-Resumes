using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ResumeScrape
{
    public static class MyExtensions
    {
        /// <summary>
        /// Returns Date Representation Of String.
        /// Returns "Now" for "Present", returns MinValue if unknown
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DateTime GetDate(this String str)
        {
            if (str.Length == 4)
            {
                DateTime dt;
                if (DateTime.TryParseExact(str, "yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                    return dt;
                else
                    return DateTime.MinValue;
            }
            else
            {
                if (!str.Contains("Present"))
                {
                    try
                    {
                        return DateTime.Parse(str);
                    }
                    catch { return DateTime.MinValue; }
                }
                else
                    return DateTime.Now;
            }
        }
    }
}
