﻿using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebScraper;

namespace TestCases
{
    [TestClass]
    public class TestDirectoryHandler
    {
        [TestCleanup()]
        public void ScraperTestCleanup()
        {
            foreach (var directory in Directory.GetDirectories(Directory.GetCurrentDirectory()))
            {
                Directory.Delete(directory, true);
            }
        }

        /// <summary>
        /// Test to get the set of filenames that are have the .css-extension.
        /// </summary>
        [TestMethod]
        public void GetCssFilepaths_PathOk()
        {
            Assert.IsTrue(DirectoryHandler.GetCssFilepaths(Directory.GetCurrentDirectory() + "\\..\\..\\Resources").Count() == 2);
        }

        /// <summary>
        /// Test to get the set of filenames that are have the .css-extension from an empty directory.
        /// </summary>
        [TestMethod]
        public void GetCssFilepaths_DirectoryEmpty()
        {
            Assert.IsTrue(DirectoryHandler.GetCssFilepaths(Directory.GetCurrentDirectory() + "\\..\\..\\Resources\\Dummy").Count() == 0);
        }

        /// <summary>
        /// Test to get the set of filenames that are have the .css-extension from a directory which does not exist.
        /// Expects a DirectoryNotFoundException.
        /// </summary>
        [TestMethod]
        public void GetCssFilepaths_PathBad()
        {
            try
            {
                DirectoryHandler.GetCssFilepaths(Directory.GetCurrentDirectory() + "\\..\\Resources");
                Assert.Fail("GetCssFilepaths_PathBad did not throw any exception");
            }
            catch (DirectoryNotFoundException) { }
        }

        /// <summary>
        /// Test to extract a filepath from an URL by removing http or https.
        /// </summary>
        [TestMethod]
        public void ExtractPathFromUrl_UrlOk()
        {
            Assert.AreEqual("something.com", DirectoryHandler.ExtractPathFromUrl("http://something.com"));
            Assert.AreEqual("something.com", DirectoryHandler.ExtractPathFromUrl("https://something.com"));
        }

        /// <summary>
        /// Test to verify that the asynchronously read file content is correct.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task ReadFileToStringAsync_PathOk()
        {
            Assert.AreEqual(await DirectoryHandler.ReadFileToStringAsync(Directory.GetCurrentDirectory() + "\\..\\..\\Resources\\TextFile1.txt"), "just Some cONTENT text123");
        }

        /// <summary>
        /// Test to check the asynchronous file read if paths are bad. Expects DirectoryNotFoundException if directory does not exist and FileNotFoundException if file does not exist.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task ReadFileToStringAsync_PathBad()
        {
            try
            {
                var read = await DirectoryHandler.ReadFileToStringAsync(Directory.GetCurrentDirectory() + "\\..\\Resources\\TextFile2.txt");
                Assert.Fail("ReadFileToString_PathBad did not throw any exception when expecting DirectoryNotFoundException");
            }
            catch (DirectoryNotFoundException) { }
            try
            {
                var read = await DirectoryHandler.ReadFileToStringAsync(Directory.GetCurrentDirectory() + "\\..\\..\\Resources\\TextFile2.txt");
                Assert.Fail("ReadFileToString_PathBad did not throw any exception when expecting FileNotFoundException");
            }
            catch (FileNotFoundException) { }
        }

        /// <summary>
        /// Test to verify that the asynchronously saved file are saved and contains correct information.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task SaveFileAsync_PathOk()
        {
            var someData = new byte[] { 0, 1, 2, 3, 4 };
            using (var stream = new MemoryStream(someData))
            {
                await DirectoryHandler.SaveFileAsync(stream, @"Test\SaveFileTest");
            }

            var readData = new byte[someData.Length];
            using (var filestream = File.OpenRead(Directory.GetCurrentDirectory() + "\\Test\\SaveFileTest"))
            {
                var x = await filestream.ReadAsync(readData, 0, someData.Length);
            }

            for (int i = 0; i < someData.Length; i++)
            {
                Assert.AreEqual(someData[i], readData[i]);
            }
        }

    }
}
