using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebScraper
{
    public class Scraper
    {
        private DirectoryHandler _dirHandler = new DirectoryHandler();
        private HttpClient _httpClient = new HttpClient();
        private HtmlParser _parser = new HtmlParser();

        public async Task Run(string url)
        {
            //_dirHandler.CreateDirectory(url);
            var result = await GetPage(url);

            var tags = _parser.ExtractAHrefTags(result).Where(x => x.StartsWith("/")).Distinct();

            Console.WriteLine(result);
            Console.ReadLine();
        }
        
        public static async Task<string> GetPage(string url)
        {
            var client = new HttpClient();
            return await client.GetStringAsync(url);
        }
    }
}
