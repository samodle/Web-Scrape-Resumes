using HtmlAgilityPack;
using ShackletonJobData;
using ShackletonJobData.Enums;
using System;
using System.Collections.Generic;

namespace JobBoardScrape
{
    class Program
    {
        public static List<JobSource> BoardsToScrape =  new List<JobSource>() {JobSource.Indeed, JobSource.USAJobs};
        public static List<string> SearchTerms = new List<string>() { "supervisor", "manager", "project", "developer", "customer support", "customer experience", "data center","help desk", "technician","slack", "server", "recruiter", "computer", "product", "assistant", "genius", "retail", "sales" };
        public static List<(string City, string Region)> SearchLocations = new List<(string City, string Region)>() {("Philadelphia", "PA"),("Knoxville", "TN") };
        public static int PagesToScrape = 1;

        public static HtmlWeb web;

        static void Main(string[] args)
        {
            Console.WriteLine("Initialize Job Scrape");

            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            web = new HtmlWeb();

            foreach(string term in SearchTerms)
            {
                ConsoleIO.printTimeStatus(watch.Elapsed, "Starting: " + term);
                foreach ((string City, string Region) loc in SearchLocations)
                {
                    ConsoleIO.printTimeStatus(watch.Elapsed, "    " + loc.City + " " + loc.Region);
                    foreach (JobSource j in BoardsToScrape)
                    {
                        var tempJobList = new List<RawJD>();

                        ConsoleIO.printTimeStatus(watch.Elapsed, "        " + Enum.GetName(typeof(JobSource), j));

                        for(int i = 0; i < PagesToScrape; i++)
                        {
                            tempJobList.AddRange(GetJobList(j, term, i, loc.City, loc.Region));
                        }
                    }
                }
            }

            ConsoleIO.printTimeStatus(watch.Elapsed, "WEB SCRAPE COMPLETE!!!");
        }

        private static List<RawJD> GetJobList(JobSource j, string SearchTerm, int SearchPage, string SearchCity = "", string SearchRegion = "")
        {
            var url = GetJobBoardURL(j, SearchTerm, SearchPage, SearchCity, SearchRegion);
            return GetJobPosts(j, url);
        }

        // Returns URL that queries Indeed for the given search term at the given location.
        //     search_page_number counts from 0! 
        public static string GetJobBoardURL(JobSource j, string SearchTerm, int SearchPageNumber = 0, string SearchCity = "", string SearchRegion = "")
        {
            switch (j)
            {
                case JobSource.Indeed:
                    var BaseURL = "https://www.indeed.com/jobs?q=";
                    SearchTerm = SearchTerm.Replace(" ", "+");
                    SearchCity = SearchCity.Replace(" ", "+");
                    if (SearchRegion.Length > 0)
                    {
                        SearchRegion = "%2C+" + SearchRegion;
                    }
                    if (SearchPageNumber > 0)
                    {
                        return BaseURL + SearchTerm + "&l=" + SearchCity + SearchRegion + "&start=" + (SearchPageNumber * 10).ToString();
                    }
                    else
                    {
                        return BaseURL + SearchTerm + "&l=" + SearchCity + SearchRegion;
                    }
                default:
                    return "error";
            }
        }

        public static List<RawJD> GetJobPosts(JobSource j, string url)
        {
            var tmpList = new List<RawJD>();
            HtmlDocument document = web.Load(url);

            return tmpList;
        }
    }
}
