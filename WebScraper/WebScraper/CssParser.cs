using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alba.CsCss.Style;

namespace WebScraper
{
    /// <summary>
    /// Contains methods for parsing and extracting information from CSS.
    /// </summary>
    public class CssParser
    {
        /// <summary>
        /// Gets the set of URLs from a specified CSS source.
        /// </summary>
        /// <param name="css">The CSS source to parse.</param>
        /// <returns></returns>
        public IEnumerable<string> GetContentUrls(string css)
        {
            return new CssLoader().GetUris(css ?? string.Empty);
        }
    }
}
