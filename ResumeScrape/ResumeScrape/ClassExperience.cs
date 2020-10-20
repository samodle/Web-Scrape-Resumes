using System;
using System.Collections.Generic;
using System.Text;

namespace ResumeScrape
{
    class Experience
    {
        public string Title { get; set; }
        public string Organization { get; set; } //company, educational institute, training provider, etc
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string City { get; set; }
        public string State { get; set; }


        public override string ToString()
        {
            return $"{Title}, {Organization}, {StartDate}-{EndDate}";
        }
    }
}
