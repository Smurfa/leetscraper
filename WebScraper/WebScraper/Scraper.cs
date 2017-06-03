using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace WebScraper
{
    public class Scraper
    {
        //private DirectoryHandler _dirHandler = new DirectoryHandler();
        //private HttpClient _httpClient = new HttpClient();
        private HtmlParser _parser = new HtmlParser();

        public string BaseUrl { get; set; }

        public async Task<bool> Run(string page)
        {
            //if (!CreateFolder(BaseUrl).Result)
            //    return false;


            var result = await GetPage(BaseUrl + page);

            //Filter out the href tags for the subpages. Assuming that tags with anchor (#) will be covered from other hrefs and removing redirects to other sites.
            var aHrefs = _parser.ExtractAHrefTags(result).Where(x => x.StartsWith("/") && !x.Contains("#") && !x.Contains("www.")).Distinct();
            var linkHrefs = _parser.ExtractLinkHrefTags(result);
            var scriptSrcs = _parser.ExtractScriptSrcTags(result);

            foreach (var link in scriptSrcs.Union(linkHrefs))
            {
                var dowloadUrl = link.Contains('?') ? link.Substring(0, link.IndexOf('?')) : link;
                await DownloadFile(BaseUrl + dowloadUrl);
            }

            await DownloadFile(BaseUrl + page + ".html");

            //foreach (var tag in aHrefs.AsParallel())
            //{
            //    await Run(tag);
            //}
            return false;
        }
        
        public static async Task<string> GetPage(string url)
        {
            using (var client = new HttpClient())
                return await client.GetStringAsync(url);
        }

        public static async Task DownloadFile(string url)
        {
            Console.WriteLine("Downloading... " + url);
            using (var client = new HttpClient())
            using (var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
            using (var content = await response.Content.ReadAsStreamAsync())
            {
                await DirectoryHandler.SaveFile(content, ExtractPathFromUrl(url), 1);   
            }
        }

        public static string ExtractPathFromUrl(string url)
        {
            return url.Substring(@"http://".Length);
        }

        public static async Task<Stream> DonwloadHtml(string url)
        {
            using (var client = new HttpClient())
                return await client.GetStreamAsync(url);
        }
    }
}
