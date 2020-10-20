using System;
using System.Globalization;

namespace ResumeScrape
{
    class WorkExperience: Experience
    {
        public string Next { get; set; } = "";

        public WorkExperience(string title, string company, string dateLoc, string desc) : base(title, company, dateLoc, desc){}

        public override string ToString()
        {
            return $"{Title}, {Organization}, {State}, {StartDate.ToLongDateString()}-{EndDate.ToLongDateString()}";
        }
    }
}
