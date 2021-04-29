using System;
using System.Collections.Generic;
using System.Text;

namespace JobDescriptionScrape
{
    static class Indeed
    {
        public static string get_job_list(string search_term, int search_page, string search_city = "", string search_state = "")
        {
            var url = get_url(search_term, search_page, search_city, search_state);
            //return get_job_posts(url, search_term);
            return url;
        }

        // Returns URL that queries Indeed for the given search term at the given location.
        //     search_page_number counts from 0! 
        public static string get_url(string search_term, int search_page_number = 0, string search_city = "", string search_state = "")
        {
            var base_url = "https://www.indeed.com/jobs?q=";
            search_term = search_term.Replace(" ", "+");
            search_city = search_city.Replace(" ", "+");
            if (search_state.Length > 0)
            {
                search_state = "%2C+" + search_state;
            }
            if (search_page_number > 0)
            {
                return base_url + search_term + "&l=" + search_city + search_state + "&start=" + (search_page_number * 10).ToString();
            }
            else
            {
                return base_url + search_term + "&l=" + search_city + search_state;
            }
        }
    }
}
