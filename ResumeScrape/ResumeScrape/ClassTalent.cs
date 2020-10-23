using System;
using System.Collections.Generic;
using System.Text;

namespace ResumeScrape
{
    class Talent
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string url { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int ZipCode { get; set; }

        public List<WorkExperience> EmploymentHistory { get; set; } = new List<WorkExperience>();
        public List<EducationExperience> EducationHistory { get; set; } = new List<EducationExperience>();

        public Talent(string name, string location, string url)
        {
            this.url = url;

            //location
            if(location == "" || !location.Contains(","))
            {
                City = "";
                State = "";
                ZipCode = -1;
            }
            else
            {
                var strings = location.Split(",");
                City = strings[0].Trim();

                var stateString = strings[1].Trim().Split(" ");
                State = stateString[0].Trim();

                if(stateString.Length > 1 && stateString[1].Length > 1)
                    ZipCode = Convert.ToInt32(stateString[1].Trim());
                else
                    ZipCode = -1;
            }

            //name
            var firstSpaceIndex = name.IndexOf(" ");
            FirstName = name.Substring(0, firstSpaceIndex); 
            LastName = name.Substring(firstSpaceIndex + 1);
        }

        public override string ToString()
        {
            return $"{FirstName} {LastName}: {City}, {State}. Jobs: {EmploymentHistory.Count}";
        }

    }
}
