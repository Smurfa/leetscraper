using System;
using System.Text;
using System.IO;
using System.Linq;
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
        /// Test to download a file and verify if the file was downloaded.
        /// </summary>
        [TestMethod]
        public async Task DownLoadFileAsync_UrlOk()
        {
            await _scraper.DownloadFileAsync(@"http://www.google.com/images/branding/googlelogo/1x/googlelogo_color_150x54dp.png");
            Assert.IsTrue(File.Exists(@"www.google.com/images/branding/googlelogo/1x/googlelogo_color_150x54dp.png"));
        }

        /// <summary>
        /// Test to download a file with a URL which is missing http/https. Expects to get an InvalidOperationException.
        /// </summary>
        [TestMethod]
        public async Task DownLoadFileAsync_UrlMissingHttp()
        {
            try
            {
                await _scraper.DownloadFileAsync(@"google.com/images/branding/googlelogo/1x/googlelogo_color_150x54dp.png");
                Assert.Fail("DownLoadFileAsync_UrlBad did not throw any exception when expecting InvalidOperationException");
            }
            catch (InvalidOperationException) { }
        }

        /// <summary>
        /// Test to get the verified download URL. Input is in a good format.
        /// </summary>
        [TestMethod]
        public void VerifyDownLoadUrl_UrlOk()
        {
            Assert.AreEqual(@"http://someadress.com/sub/img/file.png", _scraper.VerifyDownloadUrl(@"http://someadress.com/sub/img/file.png"));
        }

        /// <summary>
        /// Test to get the verified download URL. Input is missing http/https.
        /// </summary>
        [TestMethod]
        public void VerifyDownLoadUrl_MissingHttp()
        {
            Assert.AreEqual(@"http://someadress.com/sub/img/file.png", _scraper.VerifyDownloadUrl(@"someadress.com/sub/img/file.png"));
        }

        /// <summary>
        /// Test to get the verified download URL. Input has a parameter in the URL.
        /// </summary>
        [TestMethod]
        public void VerifyDownLoadUrl_ClearInputFromUrl()
        {
            Assert.AreEqual(@"http://someadress.com/sub/img/file.png", _scraper.VerifyDownloadUrl(@"http://someadress.com/sub/img/file.png?parameter=somevalue"));
        }

        /// <summary>
        /// Test to get the set of unique content URLs from an HTML source.
        /// </summary>
        [TestMethod]
        public void GetContentUrls_HtmlOk()
        {
            var html = File.ReadAllText(Directory.GetCurrentDirectory() + "\\..\\..\\Resources\\index.html");
            Assert.IsTrue(_scraper.GetContentUrls(html).Count() == 18);
        }
    }
}
