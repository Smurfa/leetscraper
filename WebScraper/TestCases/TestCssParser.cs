using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebScraper;

namespace TestCases
{
    [TestClass]
    public class TestCssParser
    {
        private static string _cssOk;
        private static string _cssBad;
        private CssParser _parser = new CssParser();

        [ClassInitialize()]
        public static void TestCssParserInitialize(TestContext context)
        {
            _cssOk = File.ReadAllText(Directory.GetCurrentDirectory() + "\\..\\..\\Resources\\main.css");
            _cssBad = File.ReadAllText(Directory.GetCurrentDirectory() + "\\..\\..\\Resources\\bad.css");
        }

        /// <summary>
        /// Test the CSS parser to see if the extracted number of URLs are as expected. CSS input is in a proper format.
        /// </summary>
        [TestMethod]
        public void GetContentUrls_CssOk()
        {
            Assert.IsTrue(_parser.GetContentUrls(_cssOk).Count() == 151);
        }

        /// <summary>
        /// Test the CSS parser to see if the extracted number of URLs are as expected. CSS input is in a bad format (syntax errors).
        /// Expects the parser to be able to extract the URLs that are in good format.
        /// </summary>
        [TestMethod]
        public void GetContentUrls_CssBad()
        {
            Assert.IsTrue(_parser.GetContentUrls(_cssBad).Count() == 149);
        }

        /// <summary>
        /// Test the CSS parser to handle empty string as input. Expects to return an empty set.
        /// </summary>
        [TestMethod]
        public void GetContentUrls_CssEmptyString()
        {
            Assert.IsTrue(_parser.GetContentUrls(string.Empty).Count() == 0);
        }

        /// <summary>
        /// Test the CSS parser to handle null as input. Expects to return an empty set.
        /// </summary>
        [TestMethod]
        public void GetContentUrls_CssNullString()
        {
            Assert.IsTrue(_parser.GetContentUrls(null).Count() == 0);
        }
    }
}
