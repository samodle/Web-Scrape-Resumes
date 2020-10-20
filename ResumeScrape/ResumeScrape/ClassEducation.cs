using System;
using System.Collections.Generic;
using System.Text;

namespace ResumeScrape
{
    public enum EducationLevel
    {
        HighSchool = 0,
        AssociateDegree = 1,
        BachelorsDegree = 2,
        GraduateDegree = 3, //anything beyond bachelors
        Certification = 4, //training resulting in certification
        Training = 5, // classes not resulting in certification
        Unknown = 6
        
    }

    class EducationExperience: Experience
    {
        public EducationExperience(string title, string company, string dateLoc, string desc)
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
            else //only location
            {
                if (dateLoc.Length > 2)
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

            //education level
            if (Title.Contains("High School") || Organization.Contains("High School"))
                Level = EducationLevel.HighSchool;
            else if (Title.Contains("Bachelor") || Organization.Contains("Bachelor"))
                Level = EducationLevel.BachelorsDegree;
            else if (Title.Contains("Associate") || Organization.Contains("Associate"))
                Level = EducationLevel.AssociateDegree;
            else if (Title.Contains("License") || Organization.Contains("License"))
                Level = EducationLevel.Certification;
            else
                Level = EducationLevel.Unknown;

        }

        public EducationLevel Level { get; set; }

        public override string ToString()
        {
            return $"{Level}: {Title}, {Organization}, {State}, {StartDate.ToLongDateString()}-{EndDate.ToLongDateString()}";
        }
    }


}
