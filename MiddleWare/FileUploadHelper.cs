using System;
using System.Drawing;
using System.IO;
using MediaToolkit.Services;
using MediaToolkit.Tasks;
using Microsoft.AspNetCore.Http;

namespace Source.Middleware
{
    public static class FileUploadHelper
    {
        public static string UploadImage(IFormFile file)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = Guid.NewGuid() + "_" + file.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            var fileStream = new FileStream(filePath, FileMode.Create);
            file.CopyTo(fileStream);
            fileStream.Close();

            return filePath;
        }

        public static string UploadThumbnailImage(IFormFile file)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = Guid.NewGuid() + "_" + file.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            var stream = file.OpenReadStream();
            var newImage = GetReducedImage(32, 32, stream);
            newImage.Save(filePath);

            return filePath;
        }

        public static string UploadFile(IFormFile file)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Files");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = Guid.NewGuid() + "_" + file.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            var fileStream = new FileStream(filePath, FileMode.Create);
            file.CopyTo(fileStream);
            fileStream.Close();

            return filePath;
        }
        
        public static Image GetReducedImage(int width, int height, Stream resourceImage)
        {
            try
            {
                var image = Image.FromStream(resourceImage);
                var thumb = image.GetThumbnailImage(width, height, () => false, IntPtr.Zero);

                return thumb;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string GetImage(string videoUrl, string fileName)
        {
            var uniqueFileName = Guid.NewGuid() + "_" + fileName.Split(".")[0] + ".png";
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var ffmpegPath = Path.Combine(uploadsFolder, "Files", "ffmpeg.exe");
            var imagePath = Path.Combine(uploadsFolder, "Images", uniqueFileName);

            var mediaToolkitService = MediaToolkitService.CreateInstance(ffmpegPath);
            var metadataTask = new FfTaskGetMetadata(videoUrl);
            var metadata = mediaToolkitService.ExecuteAsync(metadataTask).Result;

            var thumbTask = new FfTaskSaveThumbnail(videoUrl, imagePath, TimeSpan.FromSeconds(1));
            mediaToolkitService.ExecuteAsync(thumbTask);
            return imagePath;
        }
    }
}