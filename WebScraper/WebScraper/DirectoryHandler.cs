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
        /// Asynchronously tries to save a stream into a file.
        /// </summary>
        /// <param name="stream">The stream to read to the file.</param>
        /// <param name="filepath">The path to save the file to.</param>
        /// <returns></returns>
        public static async Task<bool> SaveFileAsync(Stream stream, string filepath)
        {
            var success = false;
            try
            {
                new FileInfo(filepath).Directory.Create();
                using (var fileStream = File.Create(filepath, 4096, FileOptions.Asynchronous))
                {
                    await stream.CopyToAsync(fileStream);
                }
                success = true;
            }
            catch (NotSupportedException)
            {
                Console.WriteLine("Unsupported filepath... " + filepath);
            }
            return success;
        }
    }
}
