using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alba.CsCss.Style;

namespace WebScraper
{
    public class CssParser
    {
        public IEnumerable<string> GetContentUrls(string css)
        {
            return new CssLoader().GetUris(css);
        }
    }
}
