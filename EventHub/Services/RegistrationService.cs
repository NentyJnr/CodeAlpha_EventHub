using EventHub.Context;
using EventHub.Contracts;
using EventHub.Dtos.Registrations;
using EventHub.Dtos.Tags;
using EventHub.Entities;
using EventHub.Responses;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.Xml;

namespace EventHub.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IUploadHelper _uploadHelper;
        private readonly ITagService _tagService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ILogger _log;
        private readonly bool _isTest;

        public RegistrationService(ApplicationDbContext context, IConfiguration configuration, IWebHostEnvironment webHostEnvironment,
            IUploadHelper uploadHelper, ITagService tagService, ILoggerFactory log)
        {
            _hostEnvironment = webHostEnvironment;
            _context = context;
            _configuration = configuration;
            _uploadHelper = uploadHelper;
            _tagService = tagService;
            _log = log.CreateLogger(nameof(RegistrationService));
        }

        public async Task<ApiResponse<RegisterResponse>> RegisterAsync(RegisterRequest request)
        {
            var response = new ApiResponse<RegisterResponse>();

            using var trans = await _context.Database.BeginTransactionAsync();
            var existingRegistration = await _context.RegistrationForms
                .FirstOrDefaultAsync(r => r.Email == request.Email && r.EventId == request.EventId);

            if (existingRegistration != null)
            {
                response.Status = false;
                response.Message = "User already registered for this event.";
                return response;
            }

            try
            {
                string? passportPath = null;

                if (request.UploadPassport != null)
                {
                    passportPath = await _uploadHelper.UploadImage(request.UploadPassport, true);
                }

                var reg =  new RegistrationForm();
                reg.Email = request.Email;
                reg.EventId = request.EventId;
                reg.FirstName = request.FirstName;
                reg.LastName = request.LastName;
                reg.PhoneNumber = request.PhoneNumber ?? string.Empty;
                reg.UploadPassport = passportPath;
                reg.DateCreated = DateTime.Now;

                if (request.UploadPassport != null)
                {
                    reg.UploadPassport = await _uploadHelper.UploadImage(request.UploadPassport, true);
                }
                else
                {
                    reg.UploadPassport = "default-passport.png";
                }
                
                var eventta = await _context.Events.FindAsync(request.EventId);
                if (eventta is null)
                {
                    await trans.RollbackAsync();
                    response.Status = false;
                    response.Message = "Event not found.";
                    return response;
                }
                reg.Events = eventta;
                await _context.RegistrationForms.AddAsync(reg);
                await _context.SaveChangesAsync();

                var reference = _tagService.GenerateUniqueReference(reg.Id);

                var tagResult = await _tagService.GenerateTagInternal(
                    reg,
                    reference,
                    trans,
                    canCommit: false 
                );
                
                if (!tagResult.Status)
                {
                    await trans.RollbackAsync();
                    response.Status = false;
                    response.Message = tagResult.Message; 
                    return response;
                }
                await trans.CommitAsync();
                response.Data = (new RegisterResponse
                {
                    FirstName = reg.FirstName,
                    LastName = reg.LastName,
                    Email = reg.Email,
                    EventId = reg.EventId,
                    Reference = reference,
                });
                response.Status = true;
                response.Message = "User registered successfully.";
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                response.Status = false;
                response.Message = "Registration failed";
                response.Errors = new List<string> { "An error occurred." };
            }
            return response;
        }

        public async Task<ApiResponse<RegisterResponse>> GetRegistrationByIdAsync(Guid registrationId)
        {
            var response = new ApiResponse<RegisterResponse>();
            var reg = await _context.RegistrationForms.FindAsync(registrationId);

            if (reg is null)
            {
                response.Status = false;
                response.Message = "Registration not found.";
                return response;
            }
            var tag = await _context.Tags.FirstOrDefaultAsync(t => t.RegistrationId == reg.Id);

            response.Status = true;
            response.Message = "Registration retrieved successfully.";
            response.Data = new RegisterResponse
            {
                FirstName = reg.FirstName,
                LastName = reg.LastName,
                Email = reg.Email,
                EventId = reg.EventId,
                Reference = tag?.Reference
            };
            return response;
        }

        public async Task<ApiResponse<List<RegisterResponse>>> GetAllRegistrationByEventAsync(Guid eventId)
        {
            var response = new ApiResponse<List<RegisterResponse>>();

            var regs = await _context.RegistrationForms
                .Where(r => r.EventId == eventId)
                .ToListAsync();

            var responseList = await Task.WhenAll(regs.Select(async r =>
            {
                var reference = await _tagService.GetReferenceByRegistrationId(r.Id);
                return new RegisterResponse
                {
                    FirstName = r.FirstName,
                    LastName = r.LastName,
                    Email = r.Email,
                    PhoneNumber = r.PhoneNumber,
                    PassportUrl = r.UploadPassport,
                    Reference = reference,
                };
            }));
            response.Status = true;
            response.Message = "Registrations retrieved.";
            response.Data = responseList.ToList();

            return response;
        }

        public async Task<ApiResponse<string>> UpdateRegistrationAsync(Guid registrationId, UpdateRegisterRequest request)
        {
            var response = new ApiResponse<string>();
            var reg = await _context.RegistrationForms.FindAsync(registrationId);

            if (reg is null)
            {
                response.Status = false;
                response.Message = "Registration not found.";
                return response;
            }

            reg.FirstName = request.FirstName;
            reg.LastName = request.LastName;
            reg.PhoneNumber = request.PhoneNumber ?? reg.PhoneNumber;

            if (request.UploadPassport != null)
            {
                reg.UploadPassport = await _uploadHelper.UploadImage(request.UploadPassport, true);
            }

            reg.DateModified = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            response.Status = true;
            response.Message = "Registration updated successfully.";
            response.Data = reg.Id.ToString();
            return response;
        }
    }
}

