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
        public IEnumerable<string> ExtractAllAttributesFromTag(string tag, string attribute, string html)
        {
            var document = new HtmlDocument();
            document.LoadHtml(html);

            var nodes = document.DocumentNode.SelectNodes("//" + tag + "[@" + attribute +  "]");
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    yield return node.GetAttributeValue(attribute, null);
                }
            }
        }

        public IEnumerable<string> ExtractLinkHrefTags(string html)
        {
            var document = new HtmlDocument();
            document.LoadHtml(html);

            var nodes = document.DocumentNode.SelectNodes("//link[@href]");
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    yield return node.GetAttributeValue("href", null);
                }
            }
        }

        public IEnumerable<string> ExtractScriptSrcTags(string html)
        {
            var document = new HtmlDocument();
            document.LoadHtml(html);

            var nodes = document.DocumentNode.SelectNodes("//script[@src]");
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    yield return node.GetAttributeValue("src", null);
                }
            }
        }
    }
}
