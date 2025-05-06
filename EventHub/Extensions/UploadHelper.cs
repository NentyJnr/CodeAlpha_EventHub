using EventHub.Contracts;

namespace EventHub.Extension
{
    public class UploadHelper : IUploadHelper
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;
        private readonly string _uploadUrl;

        public UploadHelper(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
            _uploadUrl = _configuration["ApplicationSettings:UploadUrl"];
        }
        public async Task<string> UploadImage(IFormFile imageFile, bool isForRegistration = false)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                string webRootPath = _webHostEnvironment.ContentRootPath;
                var extension = "." + imageFile.FileName.Split('.')[imageFile.FileName.Split('.').Length - 1];
                string imageFileName = $"{Guid.NewGuid()}{extension}";
                string imageDirectory = Path.Combine(webRootPath + "\\Uploads\\PassportUpload");
                string imageFilePath = Path.Combine(imageDirectory, imageFileName);
                var imageUrl = $"{_uploadUrl}PassportUpload/{imageFileName}";

                if (!Directory.Exists(imageDirectory))
                {
                    Directory.CreateDirectory(imageDirectory);
                }

                using (var stream = new FileStream(imageFilePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                return imageUrl;
            }
            else
            {
                return isForRegistration
                    ? $"{_uploadUrl}PassportUpload/default-image.png" 
                    : $"{_uploadUrl}PassportUpload/default-image.png"; 
            }
        }

        public async Task<string> SaveImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "EventUploads");

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var filePath = Path.Combine(uploadPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return $"{_uploadUrl}{fileName}";
        }

        public async Task<string> SaveFile(IFormFile file)
        {
            try
            {
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                var filename = DateTime.Now.Ticks.ToString() + extension;

                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files");

                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                var exactpath = Path.Combine(filepath, filename);
                using (var stream = new FileStream(exactpath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return filename;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> SaveFile(string filepath, IFormFile formFile, string directoryPath)
        {
            try
            {
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                if (File.Exists(filepath))
                {
                    File.Delete(filepath);
                }

                using (Stream stream = File.Create(filepath))
                {
                    await formFile.CopyToAsync(stream);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}




