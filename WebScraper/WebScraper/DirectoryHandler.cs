using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraper
{
    /// <summary>
    /// Contains methods for handling directories and files.
    /// </summary>
    public static class DirectoryHandler
    {
        /// <summary>
        /// Creates all directories and subdirectories based on a specified filepath. No directories are created if they already exists.
        /// </summary>
        /// <param name="filepath">The path to create directories for.</param>
        public static DirectoryInfo CreateDirectoryFromFilepath(string filepath)
        {
            var splitPath = filepath.Split('/');
            return Directory.CreateDirectory(Path.Combine(splitPath.Take(splitPath.Length - 1).ToArray()));
        }
        
        /// <summary>
        /// Gets a local path of a file based on it URL adress. The path is relative to the application path.
        /// </summary>
        /// <param name="url">The URL adress of the file.</param>
        /// <returns></returns>
        public static string ExtractPathFromUrl(string url)
        {
            url = url.StartsWith("http://") ? url.Substring("http://".Length) : url.Substring("https://".Length);
            return url;
        }

        /// <summary>
        /// Gets the set of filepath for all CSS-files in a specified directory.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetCssFilepaths(string path)
        {
            return Directory.GetFiles(path).Where(x => x.EndsWith(".css"));
        }

        /// <summary>
        /// Asynchronously reads the content of a file to a string.
        /// </summary>
        /// <param name="filepath">The path of the file to load.</param>
        /// <returns></returns>
        public static async Task<string> ReadFileToStringAsync(string filepath)
        {
            using (var fileStream = File.Open(filepath, FileMode.Open, FileAccess.Read))
            using (var reader = new StreamReader(fileStream))
            {
                return await reader.ReadToEndAsync();
            }
        }

        /// <summary>
        /// Asynchronously tries to save a stream into a file.
        /// </summary>
        /// <param name="stream">The stream to read to the file.</param>
        /// <param name="filepath">The path to save the file to.</param>
        /// <returns></returns>
        public static async Task SaveFileAsync(Stream stream, string filepath)
        {
            try
            {
                new FileInfo(filepath).Directory.Create();
                using (var fileStream = File.Create(filepath, 4096, FileOptions.Asynchronous))
                {
                    await stream.CopyToAsync(fileStream);
                }
            }
            catch (NotSupportedException)
            {
                Console.WriteLine("Unsupported filepath... " + filepath);
            }
        }
    }
}
