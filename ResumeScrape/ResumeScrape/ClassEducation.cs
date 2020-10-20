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
        Training = 5 // classes not resulting in certification
        
    }

    class EducationExperience: Experience
    {
        public EducationLevel Level { get; set; }

        public override string ToString()
        {
            return $"{Level}: {Title}, {Organization}, {StartDate}-{EndDate}";
        }
    }


}
