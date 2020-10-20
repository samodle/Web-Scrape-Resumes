using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;

namespace ResumeScrape
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Initiating Web Scrape");
            IWebDriver driver = new ChromeDriver();

            int numberOfSearchPages = 3;
            List<string> SearchTerms = new List<string> { "a", "b", "c"};

            foreach (var term in SearchTerms)
            {
                for(int i = 1; i <= numberOfSearchPages; i++)
                {
                    var TalentList = new List<Talent>();
                    var UrlList = new List<string>();

                    driver.Navigate().GoToUrl(GetJobCaseSearchURL("",term,i));

                    //get all URLs
                    //var profileElements = driver.FindElements(By.ClassName("list-cell__info"));
                    var profileElements = driver.FindElements(By.ClassName("jc-link"));
                    foreach (var x in profileElements)
                    {
                       if(x.GetAttribute("href") != null)
                        UrlList.Add(x.GetAttribute("href"));
                    }

                    var profileNameElements = driver.FindElements(By.XPath("/html/body/div/div[2]/div[2]/div[1]/div[1]/div[2]/a"));

                    //get page info for each URL
                    foreach(var url in UrlList)
                    {
                        driver.Navigate().GoToUrl(url);
                        string name, location;
                        var nameElem = driver.FindElement(By.XPath("/html/body/div/div/div[1]/div[2]/div/div[1]/div/div/div[2]/div[1]/div"));
                        name = nameElem.Text;
                        try
                        {
                            var locationElem = driver.FindElement(By.XPath("/html/body/div/div/div[1]/div[2]/div/div[1]/div/div/div[2]/div[2]/div/div[2]"));
                            location = locationElem.Text;
                        }
                        catch
                        {
                            location = "";
                        }

                        TalentList.Add(new Talent(name, location, url));
                    }
                }
            }
        }

        private static string GetJobCaseSearchURL(string firstName, string lastName, int pageNum, string location = "")
        {
            return "https://www.jobcase.com/profiles/search?distance=&employer=&first_name=" + firstName +"&last_name=" + lastName + "&location=&page=" + pageNum.ToString() +"&position=&school=&utf8=%E2%9C%93";
        }


        static void old()
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz";

            foreach (char c in alphabet)
            {
                //do something with letter
            }


            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://www.google.com");

            // Using Name:
            //var searchField = driver.FindElement(By.Name("q"));

            // Using CSS Selector:
            //var searchField = driver.FindElement(By.CssSelector(".gLFyf.gsfi"));

            // Using XPATH:
            var searchField = driver.FindElement(By.XPath("//*[@id=\"tsf\"]/div[2]/div[1]/div[1]/div/div[2]/input"));
            searchField.SendKeys("Webshop");
            searchField.Submit();

            // Using XPATH:
            //var titles = driver.FindElements(By.XPath("//*[@id=\"rso\"]/div/div/div/div"+
            //                                          "/div/div[1]/a[1]/h3/div"));

            //Using ClassName:
            var titles = driver.FindElements(By.ClassName("LC20lb"));
            var titleStrings = new List<string>();

            foreach (var title in titles)
                titleStrings.Add(title.Text);

            foreach (var title in titleStrings)
                Console.WriteLine(title);

            //*[@id="tsf"]/div[2]/div[1]/div[1]/div/div[2]/input
        }
    }
}
