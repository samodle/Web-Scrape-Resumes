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
    }
}
