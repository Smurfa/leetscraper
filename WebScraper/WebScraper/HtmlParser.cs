using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace WebScraper
{
    public class HtmlParser
    {
        public IEnumerable<string> ExtractAHrefTags(string html)
        {
            var document = new HtmlDocument();
            document.LoadHtml(html);

            var nodes = document.DocumentNode.SelectNodes("//a[@href]");
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    yield return node.GetAttributeValue("href", null);
                }
            }
        }
    }
}
