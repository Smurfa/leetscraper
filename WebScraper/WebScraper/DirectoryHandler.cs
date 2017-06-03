using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WebScraper
{
    public static class DirectoryHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filepath"></param>
        public static async Task<DirectoryInfo> CreateDirectoryFromFilepath(string filepath)
        {
            var splitPath = filepath.Split('/');
            return Directory.CreateDirectory(Path.Combine(splitPath.Take(splitPath.Length - 1).ToArray()));
        }

        
        public static async Task<bool> SaveFile(Stream stream, string filepath, int retry)
        {
            try
            {
                if (retry < 0)
                    return false;
                
                using (var fileStream = File.Open(filepath, FileMode.Create))
                {
                    await stream.CopyToAsync(fileStream);
                }
            }
            catch (DirectoryNotFoundException)
            {
                if ((await CreateDirectoryFromFilepath(filepath)).Exists)
                    await SaveFile(stream, filepath, --retry);
            }
            return true;
        }
        
        //public static async Task<bool> SaveFile(string path)
        //{
        //    var temp = await CreateDirectoryFromFilepath(path);
        //    return temp.Exists;
        //}

        //public static async Task<bool> Exists(string name)
        //{
        //    //var folderName = ExtractFoldername(url);
        //    return Directory.Exists(name);
        //}

        //public async Task<string> ExtractRootFoldername(string name)
        //{
        //   return url.Split(new[] { '/' }).Where(x => !string.IsNullOrEmpty(x)).Last();
        //}
    }
}
