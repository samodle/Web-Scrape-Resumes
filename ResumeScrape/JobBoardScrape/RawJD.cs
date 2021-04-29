using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ShackletonJobData.Enums;
using System;
using System.Collections.Generic;

namespace JobBoardScrape
{
    public class RawJD : IEquatable<RawJD>
    {
        public string JobTitle { get; set; }
        [BsonId]
        public ObjectId ID { get; set; }
        public string url { get; set; }
        public int CompanyID { get; set; }
        public string company { get; set; }
        public string location { get; set; }
        public string? rating { get; set; }
        public string? salary { get; set; }
        public string? commitment { get; set; }
        public JobSource source { get; set; }
        public string description { get; set; }
        public string description_cleaned { get; set; }
        public DateTime post_date { get; set; }
        public List<string> search_terms { get; set; }
        public List<DateTime> dates_found { get; set; }

        public bool Equals(RawJD other)
        {
            if (other.company.Equals(this.company, StringComparison.OrdinalIgnoreCase))
            {
                if (other.location.Equals(this.location, StringComparison.OrdinalIgnoreCase))
                {
                    if (other.JobTitle.Equals(this.JobTitle, StringComparison.OrdinalIgnoreCase))
                    {
                        if (other.source.Equals(this.source))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public override string ToString()
        {
            return company + ", Title: " + JobTitle + ", Location: " + location + ", Source: " + source;
        }
    }
}
