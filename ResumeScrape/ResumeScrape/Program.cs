using ShackletonJobData.Talent;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ResumeScrape
{
    static partial class Program
    {
        private enum ProgramMode
        {
            ScrapeNewData,
            RemoveDuplicates
        }
 

        static void Main(string[] args)
        {
            ProgramMode activeMode = ProgramMode.ScrapeNewData;

            switch (activeMode)
            {
                case ProgramMode.ScrapeNewData:
                    RunScrapeMain();
                    break;
                case ProgramMode.RemoveDuplicates:
                    RemoveAllDuplicates();
                    break;
            }
        }


    
    }
}
