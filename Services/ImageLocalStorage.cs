using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentGenAPI.Services
{
    public class ImageLocalStorage : IImageLocalStorage
    {
        private readonly IWebHostEnvironment env;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ImageLocalStorage(IWebHostEnvironment env,
            IHttpContextAccessor httpContextAccessor)
        {
            this.env = env;
            this.httpContextAccessor = httpContextAccessor;
        }

        public Task DeleteFile(string path, string container)
        {
            if (path != null)
            {
                var fileName = Path.GetFileName(path);
                string fileDirectory = Path.Combine(env.WebRootPath, container, fileName);

                if (File.Exists(fileDirectory))
                {
                    File.Delete(fileDirectory);
                }
            }

            return Task.FromResult(0);

        }
        public async Task<string> UpdateFile(byte[] content, string extention, string container, string path, 
            string contentType)
        {
            await DeleteFile(path, container);
            return await SaveFile(content, extention, container, contentType);
        }

        public async Task<string> SaveFile(byte[] content, string extention, string container, 
            string contentType)
        {
            var fileName = $"{Guid.NewGuid()}{extention}";
            string folder = Path.Combine(env.WebRootPath, container);
        
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string path = Path.Combine(folder, fileName);
            await File.WriteAllBytesAsync(path, content);

            var urlActual = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}";
            //var urltoBD = Path.Combine(urlActual, container, fileName).Replace("\\", "/");
            var urltoBD = Path.Combine("", container, fileName).Replace("\\", "/");
            return urltoBD;
        }
    }

    public interface IImageLocalStorage
    {
        Task<string> UpdateFile(byte[] content, string extention, string container, string path, 
            string contentType);
        Task DeleteFile(string path, string container);
        Task<string> SaveFile(byte[] content, string extention, string container, string contentType);
    }

}
