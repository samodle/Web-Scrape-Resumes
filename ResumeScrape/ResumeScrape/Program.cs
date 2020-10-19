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
            Console.WriteLine("Hello World!");

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

            foreach(var title in titleStrings)
                Console.WriteLine(title);

            //*[@id="tsf"]/div[2]/div[1]/div[1]/div/div[2]/input
        }
    }
}
