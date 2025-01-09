
using System.Drawing;
using System.Drawing.Imaging;

namespace Foodie.Common.Services
{
    public class FileUpload
    {
        private readonly IHostEnvironment _environment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FileUpload(IHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
        {
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<string> CompressAndSaveImage(IFormFile file, string folder)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }
            try
            {
                if (file.Length <= 500000)
                {
                    // If file size is already less than or equal to 500KB, no need to compress
                    var uploadsFolder = Path.Combine(_environment.ContentRootPath + "/wwwroot", "Upload", folder);
                    Directory.CreateDirectory(uploadsFolder);
                    var uniqueFileGuid = Guid.NewGuid().ToString();
                    var fileExtension = Path.GetExtension(file.FileName); // Get the original file extension
                    var fileNameWithExtension = uniqueFileGuid + fileExtension; // Combine unique GUID and original extension
                    var filePath = Path.Combine(uploadsFolder, fileNameWithExtension);
                    var url = Path.Combine("Upload", folder, fileNameWithExtension);

                    // Save the image as is (no compression needed)
                    using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        await file.CopyToAsync(stream);
                    }

                    return url;
                }

                using (var originalImage = Image.FromStream(file.OpenReadStream()))
                {
                    // Optionally, resize the image if it's large (you can change the max dimensions)
                    const int maxWidth = 1920; // Max width (in pixels)
                    const int maxHeight = 1080; // Max height (in pixels)
                    Image resizedImage = originalImage;

                    if (originalImage.Width > maxWidth || originalImage.Height > maxHeight)
                    {
                        resizedImage = ResizeImage(originalImage, maxWidth, maxHeight);
                    }

                    // Define initial quality
                    long quality = 90; // Start at high quality (90%)
                    long targetSize = 500000; // 500KB


                    var uploadsFolder = Path.Combine(_environment.ContentRootPath + "/wwwroot", "Upload", folder);
                    Directory.CreateDirectory(uploadsFolder);
                    var uniqueFileGuid = Guid.NewGuid().ToString();
                    var fileExtension = Path.GetExtension(file.FileName); // Get the original file extension
                    var fileNameWithExtension = uniqueFileGuid + fileExtension; // Combine unique GUID and original extension
                    var filePath = Path.Combine(uploadsFolder, fileNameWithExtension);
                    var url = Path.Combine("Upload", folder, fileNameWithExtension);



                    ImageCodecInfo jpegCodec = GetEncoderInfo("image/jpeg");
                    EncoderParameters encoderParams = new EncoderParameters(1);
                    MemoryStream ms = new MemoryStream();

                    do
                    {
                        ms.SetLength(0); // Reset memory stream
                        EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
                        encoderParams.Param[0] = qualityParam;

                        // Save the image to a memory stream to check the size
                        resizedImage.Save(ms, jpegCodec, encoderParams);

                        // Adjust quality
                        quality -= 10;
                    }
                    while (ms.Length > targetSize && quality > 0);

                    // Save the compressed image to file if desired size is achieved
                    if (ms.Length <= targetSize)
                    {
                        using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                        {
                            ms.WriteTo(fs);
                        }
                        return url;
                    }
                    return "";
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        private Image ResizeImage(Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var resizedImage = new Bitmap(newWidth, newHeight);
            using (var graphics = Graphics.FromImage(resizedImage))
            {
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);
            }
            return resizedImage;
        }
        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            // Get image codecs for all image formats 
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            // Find the correct image codec 
            for (int i = 0; i < codecs.Length; i++)
                if (codecs[i].MimeType == mimeType)
                    return codecs[i];

            return null;
        }
        public async Task<string> UploadFileAsync(IFormFile file, string folder)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            try
            {
                var uploadsFolder = Path.Combine(_environment.ContentRootPath + "/wwwroot", "Upload", folder);
                Directory.CreateDirectory(uploadsFolder);
                var uniqueFileGuid = Guid.NewGuid().ToString();
                var fileExtension = Path.GetExtension(file.FileName); // Get the original file extension
                var fileNameWithExtension = uniqueFileGuid + fileExtension; // Combine unique GUID and original extension
                var filePath = Path.Combine(uploadsFolder, fileNameWithExtension);
                var url = Path.Combine("Upload", folder, fileNameWithExtension);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return url;
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately (e.g., log the error).
                return null;
            }
        }
        public async Task<bool> DeleteFileAsync(string folder, string fileGuid)
        {
            try
            {
                var uploadsFolder = Path.Combine(_environment.ContentRootPath, "wwwroot");
                var fullPath = Path.Combine(uploadsFolder, fileGuid);
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    return true;
                }
                //  File.Delete(fullPath);
                return false;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string GetLocalIPAddress()
        {
            var request = _httpContextAccessor.HttpContext.Request;
            return $"{request.Scheme}://{request.Host}";
        }
    }
}
