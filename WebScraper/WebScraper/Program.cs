using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            var url = args.Count() == 0 ? @"http://tretton37.com/" : args.First();
            var _scraper = new Scraper();
            _scraper.Run(url).Wait();
        }
    }
}
