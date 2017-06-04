﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace WebScraper
{
    /// <summary>
    /// Web page scraper that downloads its content and saves it locally.
    /// </summary>
    public class Scraper
    {
        private HtmlParser _htmlParser = new HtmlParser();
        private CssParser _cssParser = new CssParser();
        private string _baseUrl;

        /// <summary>
        /// Begins the download of a website.
        /// </summary>
        /// <param name="url">The URL to the website.</param>
        /// <param name="page">The page to start the download from.</param>
        /// <returns></returns>
        public async Task<bool> Run(string url, string page)
        {
            _baseUrl = url;
            //await DownLoadPageAsync(new List<string> { page }, new List<string>());
            await DownloadContentFromCssAsync(@"tretton37.com\assets\css\");

            return false;
        }

        public async Task DownloadContentFromCssAsync(string path)
        {
            foreach (var filepath in DirectoryHandler.GetCssFilepaths(path))
            {
                var css = await DirectoryHandler.ReadFileToString(filepath);
                foreach (var url in _cssParser.GetContentUrls(css))
                {
                    await DownloadFileAsync(VerifyDownloadUrl(url));
                }
            }
        }

        /// <summary>
        /// Asynchronously downloads a webpage with its content, recursively calling its subpages and downloads them.
        /// </summary>
        /// <param name="pagesToFetch"></param>
        /// <param name="fetchedPages"></param>
        /// <returns></returns>
        public async Task DownLoadPageAsync(IEnumerable<string> pagesToFetch, IEnumerable<string> fetchedPages)
        {
            var pageToGet = pagesToFetch?.FirstOrDefault();
            if (pageToGet == null)
                return;

            var result = await GetPageAsync(_baseUrl + pageToGet);
            var aHrefs = _htmlParser.ExtractAllTagAttribute(result, "a", "href").Where(x => x.StartsWith("/") && !x.Contains("#") && !x.Contains("www.") && x.Length > 1).Distinct();
            var linkHrefs = _htmlParser.ExtractAllTagAttribute(result, "link", "href");
            var scriptSrcs = _htmlParser.ExtractAllTagAttribute(result, "script", "src");
            var imgSrcs = _htmlParser.ExtractAllTagAttribute(result, "img", "src");

            foreach (var link in imgSrcs.Union(scriptSrcs.Union(linkHrefs)))
            {
                if (link.Contains(','))
                {
                    var actualLinks = link.Split(',');
                    foreach (var actualLink in actualLinks)
                    {
                        var downloadUrl = VerifyDownloadUrl(actualLink);
                        await DownloadFileAsync(downloadUrl);
                    }
                }
                else
                {
                    var downloadUrl = VerifyDownloadUrl(link);
                    await DownloadFileAsync(downloadUrl);
                }
            }
            await DownloadFileAsync(VerifyDownloadUrl(_baseUrl + pageToGet) + ".html");
            
            fetchedPages = Enumerable.Union(fetchedPages, new List<string> { pageToGet });
            pagesToFetch = Enumerable.Except(pagesToFetch.Union(aHrefs), fetchedPages);

            await DownLoadPageAsync(pagesToFetch, fetchedPages);
        }

        /// <summary>
        /// Asynchronously gets the GET request of an URL as a string.
        /// </summary>
        /// <param name="url">The URL to get the content of.</param>
        /// <returns></returns>
        public async Task<string> GetPageAsync(string url)
        {
            using (var client = new HttpClient())
                return await client.GetStringAsync(url);
        }

        /// <summary>
        /// Asynchronously tries to download a file and save locally. If file already exists in designated target, no download will be initiated.
        /// </summary>
        /// <param name="url">The URL of the file to download.</param>
        /// <returns></returns>
        public async Task DownloadFileAsync(string url)
        {
            var filepath = DirectoryHandler.ExtractPathFromUrl(url);
            if (File.Exists(filepath))
                return;

            Console.WriteLine("Downloading... " + url);
            using (var client = new HttpClient())
            using (var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
            using (var content = await response.Content.ReadAsStreamAsync())
            {
                await DirectoryHandler.SaveFileAsync(content, DirectoryHandler.ExtractPathFromUrl(url));   
            }
        }

        /// <summary>
        /// Take a URL which will be used for downloading a file and verifies that it is in a proper format.
        /// Removes any input and relative paths and make sure it stats with an http or https request.
        /// </summary>
        /// <param name="url">The URL to be verified.</param>
        /// <returns></returns>
        public string VerifyDownloadUrl(string url)
        {
            //Clear input from URL
            url = url.Contains('?') ? url.Substring(0, url.IndexOf('?')) : url;
            
            //Not interested in relative paths
            url = url.Contains("../") ? url.Replace("../", string.Empty) : url;

            //Make sure the URL either starts of with http or https
            url = url.StartsWith("http://") || url.StartsWith("https://") ? url : _baseUrl + url;
            
            return url;
        }
    }
}
