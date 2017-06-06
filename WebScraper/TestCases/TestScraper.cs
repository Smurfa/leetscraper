using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebScraper;

namespace TestCases
{
    /// <summary>
    /// Summary description for TestScraper
    /// </summary>
    [TestClass]
    public class TestScraper
    {
        private Scraper _scraper = new Scraper();

        [TestCleanup()]
        public void ScraperTestCleanup()
        {
            foreach (var directory in Directory.GetDirectories(Directory.GetCurrentDirectory()))
            {
                Directory.Delete(directory, true);
            }
        }

        /// <summary>
        /// Tries to download a file and verify if the file was downloaded.
        /// </summary>
        [TestMethod]
        public async Task DownLoadFile_UrlOk()
        {
            await _scraper.DownloadFileAsync(@"http://www.google.com/images/branding/googlelogo/1x/googlelogo_color_150x54dp.png");
            Assert.IsTrue(File.Exists(@"www.google.com/images/branding/googlelogo/1x/googlelogo_color_150x54dp.png"));
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void VerifyDownLoadUrl_UrlOk()
        {
            Assert.AreEqual(@"http://someadress.com/sub/img/file.png", _scraper.VerifyDownloadUrl(@"http://someadress.com/sub/img/file.png"));
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void VerifyDownLoadUrl_MissingHttp()
        {
            Assert.AreEqual(@"http://someadress.com/sub/img/file.png", _scraper.VerifyDownloadUrl(@"someadress.com/sub/img/file.png"));
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void VerifyDownLoadUrl_ClearInputFromUrl()
        {
            Assert.AreEqual(@"http://someadress.com/sub/img/file.png", _scraper.VerifyDownloadUrl(@"http://someadress.com/sub/img/file.png?parameter=somevalue"));
        }
    }
}
