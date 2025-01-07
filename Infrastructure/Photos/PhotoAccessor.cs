using Application.Interfaces;
using Application.Photos;
using Imagekit.Sdk;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Infrastructure.Photos
{
    public class PhotoAccessor : IPhotoAccessor
    {
        private readonly ImagekitClient imagekitClient;

        public PhotoAccessor(IOptions<CloudSettings> config)
        {
            this.imagekitClient = new ImagekitClient(
                config.Value.PublicKey,
                config.Value.PrivateKey,
                config.Value.Url
            );
        }

        public async Task<PhotoUploadResult> AddPhoto(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    throw new ArgumentException("File is empty or null", nameof(file));
                }

                // First, load the image for processing
                using var initialStream = new MemoryStream();
                await file.CopyToAsync(initialStream);
                initialStream.Position = 0;

                using var image = await Image.LoadAsync(initialStream);

                // Calculate dimensions for square crop
                int size = Math.Min(image.Width, image.Height);
                int x = (image.Width - size) / 2;
                int y = (image.Height - size) / 2;

                // Crop to square
                image.Mutate(ctx => ctx.Crop(new Rectangle(x, y, size, size)));

                // Save processed image to new memory stream
                using var outputStream = new MemoryStream();
                await image.SaveAsJpegAsync(outputStream);
                var fileBytes = outputStream.ToArray();

                // Create upload options
                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var options = new FileCreateRequest
                {
                    file = fileBytes,
                    fileName = $"{fileName}-{DateTime.UtcNow:yyyyMMddHHmmss}",
                    useUniqueFileName = true,
                    tags = ["user-upload"],
                    responseFields = ["url", "fileId"],
                };

                // Upload to ImageKit
                var result = await imagekitClient.UploadAsync(options);
                return new PhotoUploadResult { PublicId = result.fileId, Url = result.url };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> DeletePhoto(string publicId)
        {
            try
            {
                var result = await imagekitClient.DeleteFileAsync(publicId);
                return result.Raw;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
