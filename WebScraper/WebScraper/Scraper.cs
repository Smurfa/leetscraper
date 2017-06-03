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
        private HtmlParser _parser = new HtmlParser();

        public string BaseUrl { get; set; }
        private const string _rootDirectory = "tretton37.com";

        public async Task<bool> Run(string page)
        {
            //page = page.StartsWith("/") ? page.Substring(1) : page;
            //var path = Path.Combine("tretton37.com", page + ".html");
            //if (File.Exists(Path.Combine("tretton37.com", page, ".html")))
            //    return false;

            var result = await GetPage(BaseUrl + page);

            //Filter out the href tags for the subpages. Assuming that tags with anchor (#) will be covered from other hrefs and removing redirects to other sites.
            var aHrefs = _parser.ExtractAllAttributesFromTag("a", "href", result).Where(x => x.StartsWith("/") && !x.Contains("#") && !x.Contains("www.") && x.Length > 1).Distinct();
            var linkHrefs = _parser.ExtractAllAttributesFromTag("link", "href", result);
            var scriptSrcs = _parser.ExtractAllAttributesFromTag("script", "src", result);
            var imgSrcs = _parser.ExtractAllAttributesFromTag("img", "src", result);

            foreach (var link in imgSrcs.Union(scriptSrcs.Union(linkHrefs)))
            {
                var dowloadUrl = link.Contains('?') ? link.Substring(0, link.IndexOf('?')) : link;
                await DownloadFile(BaseUrl + dowloadUrl);
            }
            await DownloadFile(BaseUrl + page + ".html");

            await FetchSubpages(aHrefs, new List<string>());

            return false;
        }

        public async Task FetchSubpages(IEnumerable<string> pagesToFetch, IEnumerable<string> fetchedPages)
        {
            var pageToGet = pagesToFetch?.FirstOrDefault();
            if (pageToGet == null)
                return;

            var result = await GetPage(BaseUrl + pageToGet);

            var linkHrefs = _parser.ExtractAllAttributesFromTag("link", "href", result);
            var scriptSrcs = _parser.ExtractAllAttributesFromTag("script", "src", result);
            var imgSrcs = _parser.ExtractAllAttributesFromTag("img", "src", result);

            foreach (var link in imgSrcs.Union(scriptSrcs.Union(linkHrefs)))
            {
                if (link.Contains(','))
                {
                    var actualLinks = link.Split(',');
                    foreach (var actualLink in actualLinks)
                    {
                        var downloadUrl = await CreateDownloadUrl(actualLink);
                        await DownloadFile(downloadUrl);
                    }
                }
                else
                {
                    var downloadUrl = await CreateDownloadUrl(link);
                    await DownloadFile(downloadUrl);
                }
            }
            await DownloadFile(await CreateDownloadUrl(BaseUrl + pageToGet) + ".html");

            fetchedPages = Enumerable.Union(fetchedPages, new List<string> { pageToGet });
            pagesToFetch = pagesToFetch.Where(x => x != pageToGet).Except(fetchedPages);

            await FetchSubpages(pagesToFetch, fetchedPages);
        }

        public async Task<string> GetPage(string url)
        {
            using (var client = new HttpClient())
                return await client.GetStringAsync(url);
        }

        public async Task DownloadFile(string url)
        {
            var filepath = await ExtractPathFromUrl(url);
            if (File.Exists(filepath))
                return;

            Console.WriteLine("Downloading... " + url);
            using (var client = new HttpClient())
            using (var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
            using (var content = await response.Content.ReadAsStreamAsync())
            {
                await DirectoryHandler.SaveFile(content, await ExtractPathFromUrl(url), 1);   
            }
        }

        public static async Task<string> ExtractPathFromUrl(string url)
        {
            url = url.StartsWith(@"http://") ? url.Substring(@"http://".Length) : url.Substring(@"https://".Length);
            return url;
        }

        public async Task<string> CreateDownloadUrl(string url)
        {
            url = url.Contains('?') ? url.Substring(0, url.IndexOf('?')) : url;
            url = url.StartsWith(@"http://") || url.StartsWith(@"https://") ? url : BaseUrl + url;
            return url;
        }

        //public static async Task<Stream> DonwloadHtml(string url)
        //{
        //    using (var client = new HttpClient())
        //        return await client.GetStreamAsync(url);
        //}
    }
}
