using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace WebScraper
{
    /// <summary>
    /// Contains methods for parsing and extracting information from HTML.
    /// </summary>
    public class HtmlParser
    {
        /// <summary>
        /// Gets all the value from a specified attribute that belongs to an HTML-tag.
        /// </summary>
        /// <param name="html">The HTML code to parse.</param>
        /// <param name="tag">The tag to extract from.</param>
        /// <param name="attribute">The attribute to get the value for.</param>
        /// <returns></returns>
        public IEnumerable<string> ExtractAllTagAttribute(string html, string tag, string attribute)
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
    }
}
