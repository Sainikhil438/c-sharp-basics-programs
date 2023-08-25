using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using RepoLayer.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace RepoLayer.Services
{
    public class FileService
    {
        private IWebHostEnvironment environment;
        public FileService(IWebHostEnvironment env)
        {
            this.environment = env;
        }
        public async Task<Tuple<int, string>> SaveImage(IFormFile imageFile)
        {
            try
            {
                var connectPath = this.environment.ContentRootPath;
                var path = Path.Combine(connectPath, "Uploads"); //path is Uploads folder path
                if(!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var ext = Path.GetExtension(imageFile.FileName);
                var allowedExtensions = new string[] { ".jpg", ".png", ".jpeg" };
                if(!allowedExtensions.Contains(ext))
                {
                    string msg = string.Format("Only {0} extensions are allowed", string.Join(",", allowedExtensions));
                    return new Tuple<int, string>(0, msg);
                }
                string uniqueString = Guid.NewGuid().ToString();
                string newFileName = uniqueString + ext;
                string fullPath = Path.Combine(path,newFileName);
                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }
                return new Tuple<int, string>(1, "Image saved successfully");
            }
            catch(Exception ex)
            {
                return new Tuple<int, string>(0, "Error has occured" + ex.Message);
            }
        }
    }
}
