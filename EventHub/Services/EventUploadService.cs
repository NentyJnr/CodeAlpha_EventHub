using EventHub.Context;
using EventHub.Contracts;
using EventHub.Dtos.EventUploads;
using EventHub.Entities;
using EventHub.Extension;
using EventHub.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace EventHub.Services
{
    public class EventUploadService : IEventUploadService
    {

        private readonly ApplicationDbContext _context;
        private readonly ILogger<EventUploadService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;
        private readonly IUploadHelper _uploadHelper;
        private readonly string _uploadUrl;

        public EventUploadService(ApplicationDbContext context, ILogger<EventUploadService> logger, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment, IConfiguration configuration, IOptions<ApplicationSettings> appSettings, IUploadHelper uploadHelper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _webHostEnvironment = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _uploadUrl = appSettings.Value.UploadUrl ?? throw new ArgumentNullException(nameof(appSettings.Value.UploadUrl));
            _uploadHelper = uploadHelper ?? throw new ArgumentNullException(nameof(uploadHelper));
        }

        public async Task<ApiResponse<EventUploadResponse>> CreateUploadAsync(EventUploadRequest request)
        {
            var response = new ApiResponse<EventUploadResponse>();
            if (request.EventId == Guid.Empty)
            {
                response.Status = false;
                response.Message = "Unsuccessful";
                response.Errors = new List<string> { "An Event Is required." };
                return response;
            }
            using var trans = await _context.Database.BeginTransactionAsync();
            try
            {
                var uploadManagement = new EventUploads
                {
                    DateModified = null,
                    EventId = request.EventId,
                    IsActive = true,
                    IsDeactivated = false,
                };
                string webRootPath = _webHostEnvironment.ContentRootPath;
                if (request.UploadLogo.Length > 0)
                {
                    var extension = "." + request.UploadLogo.FileName.Split('.')[request.UploadLogo.FileName.Split('.').Length - 1];
                    string filename = DateTime.Now.Ticks.ToString() + extension;
                    string directoryPath = Path.Combine(webRootPath + "\\Uploads\\AdminImages\\Logo\\");
                    string logofilepath = Path.Combine(directoryPath, filename.Trim());
                    await SaveFile(logofilepath, request.UploadLogo, directoryPath);
                    uploadManagement.LogoUrl = $"{_uploadUrl}AdminImages/Logo/{filename}";
                }

                if (request.UploadSignature.Length > 0)
                {
                    var extension = "." + request.UploadLogo.FileName.Split('.')[request.UploadLogo.FileName.Split('.').Length - 1];
                    string filename = DateTime.Now.Ticks.ToString() + extension;
                    string directoryPath = Path.Combine(webRootPath + "\\Uploads\\AdminImages\\Signature\\");
                    string signaturefilepath = Path.Combine(directoryPath, filename.Trim());
                    await SaveFile(signaturefilepath, request.UploadSignature, directoryPath);
                    uploadManagement.SignatureUrl = $"{_uploadUrl}AdminImages/Signature/{filename}";
                }

                _context.Uploads.Add(uploadManagement);
                int save = await _context.SaveChangesAsync();

                if (save > 0)
                {
                    await trans.CommitAsync();
                    response.Status = true;
                    response.Data = new EventUploadResponse
                    {
                        LogoUrl = uploadManagement.LogoUrl,
                        SignatureUrl = uploadManagement.SignatureUrl,
                        DateCreated = uploadManagement.DateCreated,
                        EventId = uploadManagement.EventId,
                    };
                    response.Message = "Image successfully uploaded.";
                }
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                _logger.LogCritical(ex.Message, "An error ocurred");
                response.Message = "Unsuccessful";
                response.Errors = new List<string> { "An Error occurred", "Request not successfull." };
                return response;
            }
            return response;
        }

        public async Task<ApiResponse<UploadResponse>> GetRecordByIdAsync(Guid id)
        {
            var response = new ApiResponse<UploadResponse>();
            try
            {
                var record = await _context.Uploads
                   .Where(u => u.Id == id)
                   .Select(u => new UploadResponse
                   {
                       Id = u.Id,
                       DateCreated = u.DateCreated,
                       DateModified = u.DateModified,
                       SignatureUrl = u.SignatureUrl,
                       LogoUrl = u.LogoUrl,
                       EventId = u.EventId,
                       IsActive = true,
                       IsDeactivated = false
                   }).FirstOrDefaultAsync();

                if (record == null)
                {
                    response.Status = false;
                    response.Message = "Unsuccessful";
                    response.Errors = new List<string> { "No Record Found." };
                    return response;
                }
                response.Status = true;
                response.Data = record;
                response.Message = "Record Retrieved Successfully.";
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = "Unsuccessful";
                response.Errors = new List<string> { $"An Error occurred :{ex.Message}" };
            }
            return response;
        }

        public async Task<ApiResponse<List<UploadResponse>>> GetAllRecordAsync()
        {
            var response = new ApiResponse<List<UploadResponse>>();
            try
            {
                var data = await _context.Uploads.ToListAsync();
                var records = data.Select(u => new UploadResponse
                {
                    Id = u.Id,
                    DateCreated = u.DateCreated,
                    DateModified = u.DateModified,
                    SignatureUrl = u.SignatureUrl,
                    LogoUrl = u.LogoUrl,
                    EventId = u.EventId,
                }).ToList();
                response.Data = records;
                response.Status = true;
                response.Message = "Record Retrieved Successfully.";
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = "Failed to fetch Uploads.";
                response.Errors = new List<string> { $"An Error occurred :{ex.Message}" };
                response.Data = new List<UploadResponse>();
            }
            return response;
        }

        public async Task<ApiResponse<UpdateEventUploadResponse>> UpdateUploadAsync(Guid Id, UpdateEventUploadRequest request)
        {
            var response = new ApiResponse<UpdateEventUploadResponse>();
            using var trans = await _context.Database.BeginTransactionAsync();
            try
            {
                var uploadUpdate = _context.Uploads.FirstOrDefault();
                if (uploadUpdate == null)
                {
                    response.Status = false;
                    response.Data = null;
                    response.Message = "Unsuccessful";
                    response.Errors = new List<string> { "No Record Found." };
                    return response;
                }

                string webRootPath = _webHostEnvironment.ContentRootPath;
                if (request.UploadLogo.Length > 0)
                {
                    var extension = "." + request.UploadLogo.FileName.Split('.')[request.UploadLogo.FileName.Split('.').Length - 1];
                    string filename = DateTime.Now.Ticks.ToString() + extension;
                    string directoryPath = Path.Combine(webRootPath + "\\Uploads\\AdminImages\\Logo\\");
                    string logofilepath = Path.Combine(directoryPath, filename.Trim());
                    await SaveFile(logofilepath, request.UploadLogo, directoryPath);
                    uploadUpdate.LogoUrl = $"{_uploadUrl}AdminImages/Logo/{filename}";
                }

                if (request.UploadSignature.Length > 0)
                {
                    var extension = "." + request.UploadLogo.FileName.Split('.')[request.UploadLogo.FileName.Split('.').Length - 1];
                    string filename = DateTime.Now.Ticks.ToString() + extension;
                    string directoryPath = Path.Combine(webRootPath + "\\Uploads\\AdminImages\\Signature\\");
                    string signaturefilepath = Path.Combine(directoryPath, filename.Trim());
                    await SaveFile(signaturefilepath, request.UploadSignature, directoryPath);
                    uploadUpdate.SignatureUrl = $"{_uploadUrl}AdminImages/Signature/{filename}";
                }

                
                uploadUpdate.DateModified = DateTime.UtcNow;
                _context.Uploads.Update(uploadUpdate);

                await _context.SaveChangesAsync();
                response.Status = true;
                response.Data = new UpdateEventUploadResponse
                {
                    UploadLogo = uploadUpdate.LogoUrl,
                    UploadSignature = uploadUpdate.SignatureUrl,
                    EventId = uploadUpdate.EventId,
                    DateModified = uploadUpdate.DateModified,
                };
                response.Message = "Event Updated successfully.";
                return response;
               
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = "Unsuccessful";
                response.Errors = new List<string> { "An error occurred while Updating Uploads." };
            }
            return response;
        }

        public async Task<ApiResponse<bool>> DeleteUploadAsync(Guid id)
        {
            var response = new ApiResponse<bool>();
            using var trans = await _context.Database.BeginTransactionAsync();
            try
            {
                var uploadManagement = await _context.Uploads.FindAsync(id);

                if (uploadManagement == null)
                {
                    response.Status = false;
                    response.Message = "Unsuccessful";
                    response.Errors = new List<string> { "No Record Found." };
                    return response;
                }

                await DeleteFile(uploadManagement.LogoUrl);
                await DeleteFile(uploadManagement.SignatureUrl);

                _context.Uploads.Update(uploadManagement);
                int save = await _context.SaveChangesAsync();
                if (save > 0)
                {
                    await trans.CommitAsync();
                    response.Status = true;
                    response.Message = "Image successfully deleted.";
                }
                else
                {
                    await trans.RollbackAsync();
                }
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                response.Status = false;
                response.Message = "Unsuccessful";
                response.Errors = new List<string> { "An Error occurred", "Request not successfull." };
                return response;
            }
            return response;
        }


        private async Task DeleteFile(string filePath)
        {
            try
            {
                if (!string.IsNullOrEmpty(filePath))
                {
                    var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files", filePath);

                    if (File.Exists(fullPath))
                    {
                        File.Delete(fullPath);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving file to path: {FilePath}", filePath);
                throw;
            }
        }

        public string GetImageUrl(string imagePath)
        {
            var swaggerBaseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";

            var imageEndpoint = "/api/Upload/Files";

            var imageUrl = $"{swaggerBaseUrl}{imageEndpoint}/{imagePath}";

            return imageUrl;
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
                _logger.LogError(ex, "An error occurred while saving file to path: {FilePath}", file);
                throw;
            }
        }

        private async Task<bool> SaveFile(string filepath, IFormFile formFile, string directoryPath)
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
                _logger.LogError(ex, "An error occurred while saving file to path: {FilePath}", filepath);
                throw;
            }
        }

    }
}






        

        

        
   




