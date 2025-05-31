using Ecommorce.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommorce_.infrastructure.Repositories.Services
{
    class ImageMangmentService : IImageMangmentService
    {
        private readonly IFileProvider _fileProvider;

        public ImageMangmentService(IFileProvider fileProvider)
        {
            _fileProvider = fileProvider;
        }

        public async Task<List<string>> AddImageAsync(IFormFileCollection files, string src)
        {
            var SaveImageSrc = new List<string>();
            var imageDirectory = Path.Combine("wwwroot", "Images", src);
            if(Directory.Exists(imageDirectory)is not true)
            {
                Directory.CreateDirectory(imageDirectory);
            }
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var ImageName = file.FileName;
                    var ImageSrc = $"Images/{src}/{ImageName}";
                    var root = Path.Combine(imageDirectory, ImageName);
                    using (FileStream stream = new FileStream(root, FileMode.Create))
                    {
                    await file.CopyToAsync(stream);
                    }
                    SaveImageSrc.Add(ImageSrc);

                }
            }
            return SaveImageSrc;

        }

        public void DeleteImageAsync(string src)
        {
            var info = _fileProvider.GetFileInfo(src);
            var root = info.PhysicalPath;
            File.Delete(root);
          
        }
    }
}
