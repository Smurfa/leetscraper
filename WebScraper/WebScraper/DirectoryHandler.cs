using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WebScraper
{
    public class DirectoryHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        public void CreateDirectory(string url)
        {
            var folderName = ExtractFoldername(url);
            Directory.CreateDirectory(folderName);
        }

        private string ExtractFoldername(string url)
        {
            return url.Split(new[] { '/' }).Where(x => !string.IsNullOrEmpty(x)).Last();
        }
    }
}
