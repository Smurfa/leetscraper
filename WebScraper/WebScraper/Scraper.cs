using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraper
{
    public class Scraper
    {
        private readonly DirectoryHandler _dirHandler = new DirectoryHandler();

        public void Run(string url)
        {
            _dirHandler.CreateDirectory(url);
        }
    }
}
