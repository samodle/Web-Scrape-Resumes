using ShackletonJobData.Talent;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;

namespace ResumeScrape
{
    static partial class Program
    {
        private enum SearchMode
        {
            alphabetical,
            firstnames,
            lastnames,
            custom
        }

        static void RunScrapeMain()
        {
            Console.WriteLine("GoinG to SleeP Now");
            System.Threading.Thread.Sleep(TimeSpan.FromHours(3));

            var ActiveSearchMode = SearchMode.firstnames;
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            IWebDriver driver = new ChromeDriver();

            int numberOfSearchPages = 1000, termCount = 1, timeoutCounter = 0;
            List<string> SearchTerms;
            const bool SlowMode = true;

            switch (ActiveSearchMode)
            {
                case SearchMode.alphabetical:
                    SearchTerms = GetSearchTerms_Alphabet(skipSearchTerms: 0);
                    break;
                case SearchMode.firstnames:
                    SearchTerms = GetSearchTerms_FirstName(skipSearchTerms: 0);
                    break;
                case SearchMode.lastnames:
                    SearchTerms = GetSearchTerms_FirstName(skipSearchTerms: 0);
                    break;
                default:
                    SearchTerms = new List<string> { "a", "b" };
                    break;
            }

            foreach (var term in SearchTerms)
            {
                Console.WriteLine("***");
                ShackletonJobData.ConsoleIO.printTimeStatus(watch.Elapsed, $"Starting {term}:", $", {termCount} of {SearchTerms.Count}");
                Console.WriteLine("***");
                termCount++;
                for (int i = 1; i <= numberOfSearchPages; i++)
                {
                    ShackletonJobData.ConsoleIO.printTimeStatus(watch.Elapsed, $"Page {i}:");
                    driver.Navigate().GoToUrl(GetJobCaseSearchURL(term, "", i));
                    var validityText = "No Results";

                    try
                    {
                        var validityCheckElement = driver.FindElement(By.XPath("/html/body/div/div[2]/div[2]/div/div"));
                        validityText = validityCheckElement.Text;
                    }

                    catch
                    {
                        ShackletonJobData.ConsoleIO.printEmphStatus("POTENTIAL 500 ERROR. Pausing 2 Minutes.");
                        ShackletonJobData.ConsoleIO.printTimeStatus(watch.Elapsed, "Timout at: ");

                        System.Threading.Thread.Sleep(TimeSpan.FromMinutes(2));

                        ShackletonJobData.ConsoleIO.printTimeStatus(watch.Elapsed, $"Page {i}:");
                        driver.Navigate().GoToUrl(GetJobCaseSearchURL(term, "", i));
                        var validityCheckElement = driver.FindElement(By.XPath("/html/body/div/div[2]/div[2]/div/div"));
                        validityText = validityCheckElement.Text;
                        timeoutCounter++;
                    }

                    if (validityText.Contains("No Results"))
                        i = numberOfSearchPages + 1;
                    else
                    {
                        var TalentList = new List<Talent>();
                        var UrlList = new List<string>();

                        //get all URLs
                        var profileElements = driver.FindElements(By.ClassName("jc-link"));
                        foreach (var x in profileElements)
                        {
                            if (x.GetAttribute("href") != null)
                                UrlList.Add(x.GetAttribute("href"));
                        }

                        //get page info for each URL
                        foreach (var url in UrlList)
                        {
                            bool goodToGo = true;
                            if (SlowMode)
                                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(2));
                            driver.Navigate().GoToUrl(url);
                            string name = "", location;
                            try
                            {
                                var nameElem = driver.FindElement(By.XPath("/html/body/div/div/div[1]/div[2]/div/div[1]/div/div/div[2]/div[1]/div"));
                                name = nameElem.Text;
                            }
                            catch
                            {
                                ShackletonJobData.ConsoleIO.printEmphStatus("POTENTIAL 500 ERROR. Pausing 2 Minutes.");
                                ShackletonJobData.ConsoleIO.printTimeStatus(watch.Elapsed, "Timout at: ");
                                goodToGo = false;
                                System.Threading.Thread.Sleep(TimeSpan.FromMinutes(2 + timeoutCounter));
                                driver.Navigate().GoToUrl(url);
                                var nameElem = driver.FindElement(By.XPath("/html/body/div/div/div[1]/div[2]/div/div[1]/div/div/div[2]/div[1]/div"));
                                name = nameElem.Text;
                                timeoutCounter++;
                            }


                            try
                            {
                                try
                                {
                                    var locationElem2 = driver.FindElement(By.XPath("/html/body/div/div/div[1]/div[2]/div[1]/div[1]/div/div/div[2]/div[2]/div[2]"));
                                    location = locationElem2.Text;
                                }
                                catch
                                {
                                    var locationElem = driver.FindElement(By.XPath("/html/body/div/div/div[1]/div[2]/div/div[1]/div/div/div[2]/div[2]/div/div[2]"));
                                    location = locationElem.Text;
                                }
                            }
                            catch
                            {
                                location = "";
                            }

                            var t = getTalent(name, location, url);
                            IWebElement workExperienceElement = null;

                            //get work history
                            try
                            {
                                workExperienceElement = driver.FindElement(By.XPath("/html/body/div/div/div[4]/div"));
                            }
                            catch
                            {
                                //  Console.WriteLine($"*** {name} has no work history");
                                goodToGo = false;
                            }

                            if (goodToGo)
                            {
                                try
                                {
                                    driver.FindElement(By.XPath("/html/body/div/div/div[4]/div/div[3]/button")).Click();
                                }
                                catch { /*Console.WriteLine("      No Education Button");*/ }

                                var profileItems = workExperienceElement.FindElements(By.ClassName("profile-item"));

                                int blankElementCounter = 0;
                                for (int n = 0; n < profileItems.Count; n++)
                                {
                                    string company = "", title = "", dateLocation, description;
                                    var item = profileItems[n];

                                    try
                                    {
                                        var companyElement = item.FindElement(By.ClassName("profile-item__primary-text"));
                                        company = companyElement.Text;

                                        var titleElement = item.FindElement(By.ClassName("profile-item__secondary-text"));
                                        title = titleElement.Text.Trim();
                                    }
                                    catch
                                    {
                                        goodToGo = false;
                                    }

                                    if (goodToGo)
                                    {
                                        if (company.Length > 1 || title.Length > 1)
                                        {
                                            try
                                            {
                                                var descElement = item.FindElement(By.ClassName("profile-item__description"));
                                                description = descElement.Text;
                                            }
                                            catch
                                            {
                                                description = "";
                                            }

                                            try
                                            {
                                                var dateElement = item.FindElement(By.ClassName("profile-item__metadata"));
                                                dateLocation = dateElement.Text;
                                            }
                                            catch
                                            {
                                                dateLocation = "";
                                            }


                                            t.EmploymentHistory.Add(getWorkExperience(title, company, dateLocation, description));
                                            if (n > 0 && t.EmploymentHistory.Count > 1)
                                                t.EmploymentHistory[t.EmploymentHistory.Count - 1].Next = t.EmploymentHistory[t.EmploymentHistory.Count - 2].Title;
                                        }
                                        else
                                            blankElementCounter++;
                                    }

                                }

                                //education history - only if has work history
                                bool areThereTraits = false;

                                string traitTitleXPath = "/html/body/div/div/div[5]/div/div[1]/div/span";

                                try
                                {
                                    var tmpElement = driver.FindElement(By.XPath(traitTitleXPath));
                                    if (tmpElement.Text.Equals("Traits"))
                                        areThereTraits = true;
                                }
                                catch
                                {
                                    goodToGo = false;
                                }

                                string parentElementXPath = areThereTraits ? "/html/body/div/div/div[6]/div" : "/html/body/div/div/div[5]/div";
                                string buttonElementXPath = areThereTraits ? "/html/body/div/div/div[6]/div/div[3]/button" : "/html/body/div/div/div[5]/div/div[3]/button";

                                try
                                {
                                    if (goodToGo)
                                        workExperienceElement = driver.FindElement(By.XPath(parentElementXPath));
                                }
                                catch
                                {
                                    // Console.WriteLine($"{name} no education history");
                                    goodToGo = false;
                                }

                                if (goodToGo)
                                {
                                    try
                                    {
                                        driver.FindElement(By.XPath(buttonElementXPath)).Click();
                                    }
                                    catch { /*Console.WriteLine("      No Education Button");*/ }

                                    profileItems = workExperienceElement.FindElements(By.ClassName("profile-item"));

                                    for (int n = 0; n < profileItems.Count; n++)
                                    {
                                        string company, title, dateLocation, description;
                                        var item = profileItems[n];

                                        try
                                        {
                                            var companyElement = item.FindElement(By.ClassName("profile-item__primary-text"));
                                            var titleElement = item.FindElement(By.ClassName("profile-item__secondary-text"));
                                            var dateElement = item.FindElement(By.ClassName("profile-item__metadata"));


                                            try
                                            {
                                                var descElement = item.FindElement(By.ClassName("profile-item__description"));
                                                description = descElement.Text;
                                            }
                                            catch
                                            {
                                                description = "";
                                            }

                                            company = companyElement.Text;
                                            title = titleElement.Text.Trim();

                                            dateLocation = dateElement.Text;

                                            t.EducationHistory.Add(getEducationExperience(title, company, dateLocation, description));
                                        }
                                        catch { }

                                    }
                                }

                                TalentList.Add(t);
                                Console.Write(t.ToString() + " | ");
                            }

                        }

                        if (TalentList.Count > 0)
                            ExportResumes(TalentList);
                    }

                }
            }
        }

    }
}
