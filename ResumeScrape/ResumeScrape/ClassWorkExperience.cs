using System;
using System.Globalization;

namespace ResumeScrape
{
    class WorkExperience: Experience
    {
        public string Next { get; set; } = "";

        public WorkExperience(string title, string company, string dateLoc, string desc)
        {
            Title = title;
            Organization = company.Trim();
            Description = desc.Trim();

            if (dateLoc.Contains("|"))  //date and location
            {
                var strings = dateLoc.Split("|");

                var dateStrings = strings[0].Split("-");
                StartDate = dateStrings[0].Trim().GetDate();
                EndDate = dateStrings[1].Trim().GetDate();

                var locStrings = strings[1].Split(",");
                State = locStrings[1].Trim();
                City = locStrings[0].Trim();
            }
            else if (dateLoc.Contains("-")) //only date
            {
                var dateStrings = dateLoc.Split("-");
                StartDate = dateStrings[0].Trim().GetDate();
                EndDate = dateStrings[1].Trim().GetDate();

                City = "";
                State = "";
            }
            else //only location
            {
                if(dateLoc.Length > 2)
                {
                    var locStrings = dateLoc.Split(",");
                    State = locStrings[1].Trim();
                    City = locStrings[0].Trim();
                }
                else
                {
                    City = "";
                    State = "";
                }

            }
        }

        public override string ToString()
        {
            return $"{Title}, {Organization}, {State}, {StartDate.ToLongDateString()}-{EndDate.ToLongDateString()}";
        }
    }
}
