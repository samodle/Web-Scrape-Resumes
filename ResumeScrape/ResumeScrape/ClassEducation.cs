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
        public EducationLevel Level { get; set; }

        public EducationExperience(string title, string company, string dateLoc, string desc): base(title, company, dateLoc, desc)
        {
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

        public override string ToString()
        {
            return $"{Level}: {Title}, {Organization}, {State}, {StartDate.ToLongDateString()}-{EndDate.ToLongDateString()}";
        }
    }


}
