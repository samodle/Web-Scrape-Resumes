using MongoDB.Bson;
using MongoDB.Driver;
using Oden;
using Oden.Enums;
using Oden.Talent;
using System;
using System.Collections.Generic;
using System.Text;

namespace ResumeScrape
{
    static partial class Program
    {
        public static void printTimeStatus(TimeSpan ts, string messageA = "Time Elapsed: ", string messageB = "")
        {
            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);

            Console.WriteLine($"{messageA} {elapsedTime} {messageB}");
        }

        private static void ExportResumes(List<Talent> exportList)
        {
            MongoClient dbClient = new MongoClient(Oden.Mongo.Connection.LOCAL);
            IMongoDatabase database = dbClient.GetDatabase(Oden.Mongo.DB.TALENT);
            var resume_collection = database.GetCollection<BsonDocument>(Oden.Mongo.Collection.RESUME);

            var EmpInfoArray = new List<BsonDocument>();
            foreach (Talent t in exportList)
                EmpInfoArray.Add(t.ToBsonDocument());
            resume_collection.InsertMany(EmpInfoArray);

        }

        private static EducationExperience getEducationExperience(string title, string company, string dateLoc, string desc)
        {
            var x = GetCityStateStartEndParams(dateLoc);
            EducationLevel e;

            //education level
            if (title.Contains("High School") || company.Contains("High School") || title.Contains("GED") || company.Contains("GED"))
                e = EducationLevel.HighSchool;
            else if (title.Contains("Bachelor") || company.Contains("Bachelor"))
                e = EducationLevel.BachelorsDegree;
            else if (title.Contains("Associate") || company.Contains("Associate"))
                e = EducationLevel.AssociateDegree;
            else if (title.Contains("License") || company.Contains("License"))
                e = EducationLevel.Certification;
            else
                e = EducationLevel.Unknown;

            return new EducationExperience
            {
                City = x.Item1,
                State = x.Item2,
                Title = title.Trim(),
                Organization = company.Trim(),
                Description = desc.Trim(),
                Level = e
            };
        }

        private static WorkExperience getWorkExperience(string title, string company, string dateLoc, string desc)
        {
            var x = GetCityStateStartEndParams(dateLoc);

            return new WorkExperience
            {
                City = x.Item1,
                State = x.Item2,
                Title = title.Trim(),
                Organization = company.Trim(),
                Description = desc.Trim()
            };
        }

        private static Tuple<string, string, DateTime, DateTime> GetCityStateStartEndParams(string dateLoc)
        {
            DateTime StartDate = DateTime.MinValue, EndDate = DateTime.MinValue;
            string City = "", State = "";

            if (dateLoc.Contains("|"))  //date and location
            {
                var strings = dateLoc.Split('|');

                if (strings[0].Contains("-"))
                {
                    var dateStrings = strings[0].Split('-');
                    StartDate = dateStrings[0].Trim().GetDate();
                    EndDate = dateStrings[1].Trim().GetDate();
                }
                else //no start date, assume end date is 'present'
                {
                    EndDate = strings[0].Trim().GetDate();
                }

                if (strings[1].Contains(","))
                {
                    var locStrings = strings[1].Split(',');
                    State = locStrings[1].Trim();
                    City = locStrings[0].Trim();
                }
                else
                {
                    if (strings[1].Trim().Length == 2)
                    {
                        City = "";
                        State = strings[1];
                    }
                    else
                    {
                        City = strings[1];
                        State = "";
                    }
                }
            }
            else if (dateLoc.Contains("-")) //only date
            {
                var dateStrings = dateLoc.Split('-');
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
                        if (dateLoc.Contains(","))
                        {
                            var locStrings = dateLoc.Split(',');
                            State = locStrings[1].Trim();
                            City = locStrings[0].Trim();
                        }
                        else
                        {
                            if (dateLoc.Trim().Length == 2)
                            {
                                City = "";
                                State = dateLoc;
                            }
                            else
                            {
                                City = dateLoc;
                                State = "";
                            }
                        }
                    }
                }
                else
                {
                    City = "";
                    State = "";
                }
            }

            return new Tuple<string, string, DateTime, DateTime>(City, State, StartDate, EndDate);
        }

        private static Talent getTalent(string name, string location, string url)
        {
            string firstName, lastName, city, state;
            int zip;

            //location
            if (location == "" || !location.Contains(","))
            {
                city = "";
                state = "";
                zip = -1;
            }
            else
            {
                var strings = location.Split(',');
                city = strings[0].Trim();

                var stateString = strings[1].Trim().Split(' ');
                state = stateString[0].Trim();

                if (stateString.Length > 1 && stateString[1].Length > 1)
                    zip = Convert.ToInt32(stateString[1].Trim());
                else
                    zip = -1;
            }

            //name
            var firstSpaceIndex = name.IndexOf(" ");
            firstName = name.Substring(0, firstSpaceIndex);
            lastName = name.Substring(firstSpaceIndex + 1);

            return new Talent
            {
                FirstName = firstName,
                LastName = lastName,
                Url = url,
                City = city,
                State = state,
                ZipCode = zip
            };
        }

        private static string GetJobCaseSearchURL(string firstName, string lastName, int pageNum, string location = "")
        {
            return "https://www.jobcase.com/profiles/search?distance=&employer=&first_name=" + firstName + "&last_name=" + lastName + "&location=&page=" + pageNum.ToString() + "&position=&school=&utf8=%E2%9C%93";
        }
    }
}
