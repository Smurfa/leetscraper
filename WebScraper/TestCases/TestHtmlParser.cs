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

        [TestMethod]
        public void ExtractAllTagAttribute_InputOk()
        {
            Assert.IsTrue(_parser.ExtractAllTagAttribute(_htmlOk, "a", "href").Count() == 29);
        }

        [TestMethod]
        public void ExtractAllTagAttribute_MixUpTags()
        {
            Assert.IsTrue(_parser.ExtractAllTagAttribute(_htmlOk, "href", "a").Count() == 0);
        }

        [TestMethod]
        public void ExtractAllTagAttribute_EmptyHtml()
        {
            Assert.IsTrue(_parser.ExtractAllTagAttribute(string.Empty, "a", "href").Count() == 0);
        }

        [TestMethod]
        public void ExtractAllTagAttribute_NullHtml()
        {
            Assert.IsTrue(_parser.ExtractAllTagAttribute(null, "a", "href").Count() == 0);
        }
    }
}
