using System;
using System.Collections.Generic;
using System.Text;

namespace ResumeScrape
{
    public class Experience
    {
        public string Title { get; set; }
        public string Organization { get; set; } //company, educational institute, training provider, etc
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        public Experience(string title, string company, string dateLoc, string desc)
        {
            Title = title;
            Organization = company.Trim();
            Description = desc.Trim();

            if (dateLoc.Contains("|"))  //date and location
            {
                var strings = dateLoc.Split("|");

                if (strings[0].Contains("-"))
                {
                    var dateStrings = strings[0].Split("-");
                    StartDate = dateStrings[0].Trim().GetDate();
                    EndDate = dateStrings[1].Trim().GetDate();
                }
                else //no start date, assume end date is 'present'
                {
                    EndDate = strings[0].Trim().GetDate();
                }

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
            else
            {
                if (dateLoc.Length > 2)
                {
                    if (dateLoc.Trim().Length == 4)
                    {
                        EndDate = dateLoc.Trim().GetDate();
                    }
                    else
                    {
                        var locStrings = dateLoc.Split(",");
                        State = locStrings[1].Trim();
                        City = locStrings[0].Trim();
                    }
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
            return $"{Title}, {Organization}, {StartDate}-{EndDate}";
        }
    }
}
