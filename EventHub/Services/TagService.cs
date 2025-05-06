using EventHub.Context;
using EventHub.Contracts;
using EventHub.Dtos.Tags;
using EventHub.Entities;
using EventHub.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EventHub.Services
{
    public class TagService : ITagService
    {
        private readonly ApplicationDbContext _context;
        private readonly IServiceProvider _serviceProvider;
        private readonly IUploadHelper _uploadHelper;
        private readonly ILogger<TagService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;
        public TagService(ApplicationDbContext context, ILogger<TagService> logger, IServiceProvider serviceProvider, IUploadHelper uploadHelper,
            IHttpContextAccessor contextAccessor, IConfiguration configuration)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider)); ;
            _uploadHelper = uploadHelper ?? throw new ArgumentNullException(nameof(uploadHelper)); ;
            _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor)); ;
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration)); ;
        }

       
        public async Task<ApiResponse<TagResponse>> GetTag(TagRequest request)
        {
            var response = new ApiResponse<TagResponse>();
            try
            {
                var tag = await _context.Tags
                    .Include(t => t.Event) 
                    .FirstOrDefaultAsync(t =>
                        t.Reference == request.Reference 
                    );

                if (tag == null)
                {
                    response.Status = false;
                    response.Message = "Tag not found";
                    return response;
                }

                response.Data = new TagResponse
                {
                    Reference = tag.Reference,
                    EventName = tag.EventName,
                    FullName = tag.FullName,
                    EventStartDate = tag.EventStartDate,
                    EventEndDate = tag.EventEndDate,
                    ProfilePicture = tag.PassportUrl
                };
                response.Status = true;
                response.Message = "Tag retrieved successfully";
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = $"Error retrieving tag: {ex.Message}";
            }
            return response;
        }
        
        public async Task<ApiResponse<bool>> GenerateTagInternal(
            RegistrationForm registration,
            string reference,
            IDbContextTransaction transaction,
            bool canCommit)
        {
            try
            {
                var eventUploads = await _context.Uploads
                    .FirstOrDefaultAsync(e => e.EventId == registration.EventId);

                if (eventUploads == null)
                {
                    return new ApiResponse<bool>
                    {
                        Status = false,
                        Message = "Event branding (logo/signature) not configured"
                    };
                }
                var eventDetails = await _context.Events
                    .FirstOrDefaultAsync(e => e.Id == registration.EventId);

                if (eventDetails == null)
                {
                    return new ApiResponse<bool>
                    {
                        Status = false,
                        Message = "Associated event not found"
                    };
                }

                var newTag = new Tags
                {
                    LogoUrl = eventUploads.LogoUrl,
                    SignatureUrl = eventUploads.SignatureUrl,

                    PassportUrl = registration.UploadPassport,
                    FullName = $"{registration.FirstName} {registration.LastName}",

                    EventName = eventDetails.Name,
                    EventStartDate = eventDetails.StartDate,
                    EventEndDate = eventDetails.EndDate,

                    Reference = reference,
                    DateCreated = DateTime.UtcNow,
                    EventId = registration.EventId,
                    RegistrationId = registration.Id,
                    IsActive = true
                };

                _context.Tags.Add(newTag);
                await _context.SaveChangesAsync();

                if (canCommit)
                {
                    await transaction.CommitAsync();
                }

                return new ApiResponse<bool> { Status = true, Message = "Tag generated" };
            }
            catch (Exception ex)
            {
                if (canCommit)
                {
                    await transaction.RollbackAsync();
                }
                return new ApiResponse<bool> { Status = false, Message = ex.Message };
            }
        }

        public string GenerateUniqueReference(Guid registrationId)
        {
            return $"{registrationId}-{DateTime.UtcNow:yyyyMMddHHmmssfff}-{Guid.NewGuid().ToString("N").Substring(0, 4)}";
        }
        public async Task<ApiResponse<bool>> UploadPassport(UploadTagPassportRequest request)
        {
            var response = new ApiResponse<bool>();
            if (request.reference == null)
            {
                response.Status = false;
                response.Message = "No Reference Id";
                return response;
            }
            using var trans = await _context.Database.BeginTransactionAsync();
            if (request.file == null)
            {
                response.Status = false;
                response.Message = "No image uploaded";
                return response;
            }
            string passportFileName = await _uploadHelper.UploadImage(request.file, false);
            var tagdetails = await _context.Tags.FirstOrDefaultAsync(x => x.Reference == request.reference && x.EventId == request.EventId);
            if (tagdetails == null)
            {
                response.Status = false;
                response.Message = "No tag found";
                return response;
            }
            tagdetails.PassportUrl = passportFileName;
            _context.Tags.Update(tagdetails);
            int save = await _context.SaveChangesAsync();
            if (save > 0)
            {
                response.Status = true;
                response.Message = "Request was successful";
                return response;
            }
            else
            {
                response.Status = false;
                response.Message = "Request wasnot successful";
            }
            return response;
        }

        public async Task<string?> GetReferenceByRegistrationId(Guid registrationId)
        {
            var tag = await _context.Tags
                .Where(t => t.RegistrationId == registrationId)
                .Select(t => t.Reference)
                .FirstOrDefaultAsync();

            return tag;
        }

    }
}


    


        

        
        


        

        

        