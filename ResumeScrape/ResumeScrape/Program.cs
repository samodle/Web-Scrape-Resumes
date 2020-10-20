using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ResumeScrape
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Initiating Web Scrape");
            IWebDriver driver = new ChromeDriver();

            int numberOfSearchPages = 3;
            List<string> SearchTerms = new List<string> { "b", "c" };

            foreach (var term in SearchTerms)
            {
                for (int i = 1; i <= numberOfSearchPages; i++)
                {
                    var TalentList = new List<Talent>();
                    var UrlList = new List<string>();

                    driver.Navigate().GoToUrl(GetJobCaseSearchURL("", term, i));

                    //get all URLs
                    var profileElements = driver.FindElements(By.ClassName("jc-link"));
                    foreach (var x in profileElements)
                    {
                        if (x.GetAttribute("href") != null)
                            UrlList.Add(x.GetAttribute("href"));
                    }

                    var profileNameElements = driver.FindElements(By.XPath("/html/body/div/div[2]/div[2]/div[1]/div[1]/div[2]/a"));

                    //get page info for each URL
                    foreach (var url in UrlList)
                    {
                        driver.Navigate().GoToUrl(url);
                        string name, location;
                        var nameElem = driver.FindElement(By.XPath("/html/body/div/div/div[1]/div[2]/div/div[1]/div/div/div[2]/div[1]/div"));
                        name = nameElem.Text;
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

                        var t = new Talent(name, location, url);
                        bool goodToGo = true;
                        IWebElement workExperienceElement = null;

                        //get work and education history
                        try
                        {
                            workExperienceElement = driver.FindElement(By.XPath("/html/body/div/div/div[4]/div"));
                        }
                        catch
                        {
                            Console.WriteLine($"{name} has no work history");
                            goodToGo = false;
                        }

                        if (goodToGo)
                        {
                            driver.FindElement(By.XPath("/html/body/div/div/div[4]/div/div[3]/button")).Click();
                            var profileItems = workExperienceElement.FindElements(By.ClassName("profile-item"));

                            for (int n = 0; n < profileItems.Count; n++)
                            {
                                string company, title, dateLocation, description;
                                var item = profileItems[n];

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

                                t.EmploymentHistory.Add(new WorkExperience(title, company, dateLocation, description));
                                if (n > 0)
                                    t.EmploymentHistory[n - 1].Next = title;
                            }

                            //education history
                            TalentList.Add(t);
                            Console.WriteLine(t.ToString());
                            foreach (WorkExperience w in t.EmploymentHistory)
                                Console.WriteLine(w.ToString());
                        }

                    }
                }
            }
        }

        private static string GetJobCaseSearchURL(string firstName, string lastName, int pageNum, string location = "")
        {
            return "https://www.jobcase.com/profiles/search?distance=&employer=&first_name=" + firstName + "&last_name=" + lastName + "&location=&page=" + pageNum.ToString() + "&position=&school=&utf8=%E2%9C%93";
        }
    }
}
