using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebScraper;

namespace TestCases
{
    [TestClass]
    public class TestHtmlParser
    {
        private HtmlParser _parser = new HtmlParser();
        private static string _htmlOk;

        [ClassInitialize()]
        public static void TestCssParserInitialize(TestContext context)
        {
            _htmlOk = File.ReadAllText(Directory.GetCurrentDirectory() + "\\..\\..\\Resources\\index.html");
        }

        /// <summary>
        /// Test to extract all of the value for a certain tag and attribute in an HTML source with correct syntax.
        /// </summary>
        [TestMethod]
        public void ExtractAllTagAttribute_InputOk()
        {
            Assert.IsTrue(_parser.ExtractAllTagAttribute(_htmlOk, "a", "href").Count() == 29);
        }

        /// <summary>
        /// Test to extract all of the value for a certain tag and attribute in an HTML source with correct syntax.
        /// Input tag and attribute has been mixed. Expects 0 results to be returned.
        /// </summary>
        [TestMethod]
        public void ExtractAllTagAttribute_MixUpTags()
        {
            Assert.IsTrue(_parser.ExtractAllTagAttribute(_htmlOk, "href", "a").Count() == 0);
        }

        /// <summary>
        /// Test to extract all of the value for a certain tag and attribute in an empty HTML source.
        /// Expects 0 results to be returned.
        /// </summary>
        [TestMethod]
        public void ExtractAllTagAttribute_EmptyHtml()
        {
            Assert.IsTrue(_parser.ExtractAllTagAttribute(string.Empty, "a", "href").Count() == 0);
        }

        /// <summary>
        /// Test to extract all of the value for a certain tag and attribute.
        /// HTML source is null. Expects 0 results to be returned.
        /// </summary>
        [TestMethod]
        public void ExtractAllTagAttribute_NullHtml()
        {
            Assert.IsTrue(_parser.ExtractAllTagAttribute(null, "a", "href").Count() == 0);
        }
    }
}
