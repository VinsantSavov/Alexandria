namespace Alexandria.Services.Cloudinary
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Http;

    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary cloudinary;

        public CloudinaryService(Cloudinary cloudinary)
        {
            this.cloudinary = cloudinary;
        }

        public async Task<string> UploadImageAsync(IFormFile imageFile, string fileName)
        {
            using var ms = new MemoryStream();
            await imageFile.CopyToAsync(ms);
            var array = ms.ToArray();

            using var stream = new MemoryStream(array);

            var file = new ImageUploadParams()
            {
                File = new FileDescription(fileName, stream),
            };

            var uploadResult = await this.cloudinary.UploadAsync(file);

            return uploadResult.Url.ToString();
        }
    }
}
