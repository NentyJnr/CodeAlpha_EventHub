using EventHub.Context;
using EventHub.Contracts;
using EventHub.Dtos.Events;
using EventHub.Entities;
using EventHub.Responses;
using Microsoft.EntityFrameworkCore;
using System;

namespace EventHub.Services
{
    public class EventService : IEventService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EventService> _logger;
        private readonly IUploadHelper _uploadHelper;

        public EventService(ApplicationDbContext context, ILogger<EventService> logger, IUploadHelper uploadHelper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _uploadHelper = uploadHelper;
        }

        public async Task<ApiResponse<CreateEventResponse>> CreateAsync(CreateEventRequest request)
        {
            var response = new ApiResponse<CreateEventResponse>();

            try
            {
                var exEvent = await _context.Events
                    .FirstOrDefaultAsync(e => e.Name == request.Name);

                if (exEvent != null)
                {
                    response.Status = false;
                    response.Message = "Unsuccessful";
                    response.Errors = new List<string> { "An Event Exist." };
                    return response;
                }

                string? coverImagePath = null;

                if (request.CoverImage != null)
                {
                    coverImagePath = await _uploadHelper.UploadImage(request.CoverImage, false);
                }

                var newEvent = new Events
                {
                    Name = request.Name,
                    CoverImage = coverImagePath,
                    Information = request.Information,
                    Location = request.Location,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    EventTime = request.EventTime,
                    IsActive = true,
                    IsDeactivated = false,
                    DateCreated = DateTime.UtcNow,
                };

                await _context.Events.AddAsync(newEvent);
                await _context.SaveChangesAsync();

                response.Status = true;
                response.Data = new CreateEventResponse
                {
                    Name = newEvent.Name,
                    CoverImage = newEvent.CoverImage,
                    Information = newEvent.Information,
                    Location = newEvent.Location,
                    StartDate = newEvent.StartDate,
                    EndDate = newEvent.EndDate,
                    EventTime = newEvent.EventTime,
                    IsActive = newEvent.IsActive = true,
                    IsDeactivated = newEvent.IsDeactivated = false,
                };
                response.Message = "Event created successfully.";
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = "Unsuccessful";
                response.Errors = new List<string> { "An error occurred while creating Event." };
            }
            return response;
        }

        public async Task<ApiResponse<EventResponse>> GetRecordById(Guid id)
        {
            var response = new ApiResponse<EventResponse>();

            var record = await _context.Events
                    .Where(e => e.Id == id)
                    .Select(e => new EventResponse
                    {
                        Id = e.Id,
                        Name = e.Name,
                        StartDate = e.StartDate,
                        EndDate = e.EndDate,
                        Location = e.Location,
                        DateCreated = e.DateCreated,
                        CoverImage = e.CoverImage,
                        GuestSpeakers = e.GuestSpeakers,
                        IsActive = e.IsActive,
                        IsDeactivated = e.IsDeactivated,
                        DateModified = e.DateModified,
                        Information = e.Information,
                    })
                    .FirstOrDefaultAsync();

            if (record == null)
            {
                response.Status = false;
                response.Message = "Unsuccessful";
                response.Errors = new List<string> { "An unexpected error occurred. Please try again." };
                return response;
            }
            response.Status = true;
            response.Data = record;
            response.Message = "Event created successfully.";
            return response;
        }

        public async Task<ApiResponse<List<EventResponse>>> GetAllRecord()
        {
            var response = new ApiResponse<List<EventResponse>>();

            var data = await _context.Events
                .Include(e => e.GuestSpeakers)
                .ToListAsync();
            var records = data.Select(e => new EventResponse
            {
                Id = e.Id,
                Name = e.Name,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                Location = e.Location,
                DateCreated = e.DateCreated,
                GuestSpeakers = e.GuestSpeakers,
                CoverImage = e.CoverImage,
                IsActive = e.IsActive,
                IsDeactivated = false,
                DateModified = e.DateModified,
                Information = e.Information,

            }).ToList();
            response.Data = records;
            response.Status = true;
            response.Message = "Events retrived successfully";
            return response;
        }

        public async Task<ApiResponse<bool>> UpdateAsync(UpdateEventRequest request)
        {
            var response = new ApiResponse<bool>();
            try
            {
                var exEvent = await _context.Events.FindAsync(request.Id);
                if (exEvent == null)
                {
                    response.Status = false;
                    response.Message = "Unsuccessful";
                    response.Errors = new List<string> { "Event Not Found." };
                    return response;
                }
                string coverImage = await _uploadHelper.UploadImage(request.CoverImage);
                exEvent.CoverImage = coverImage;
                exEvent.Name = request.Name;
                exEvent.Information = request.Information;
                exEvent.Location = request.Location;
                exEvent.StartDate = request.StartDate;
                exEvent.EndDate = request.EndDate;
                exEvent.EventTime = request.EventTime;
                exEvent.DateModified = DateTime.Now;

                _context.Events.Update(exEvent);
                await _context.SaveChangesAsync();

                response.Status = true;
                response.Data = true;
                response.Message = "Event Updated successfully.";
                return response;
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = "Unsuccessful";
                response.Errors = new List<string> { "An error occurred while Updating Event." };
            }
            return response;

        }

        public async Task<ApiResponse<bool>> DeactivateAsync(Guid id)
        {
            var response = new ApiResponse<bool>();
            try
            {
                var existingEvent = await _context.Events.FindAsync(id);
                if (existingEvent == null)
                {
                    response.Status = false;
                    response.Message = "Unsuccessful";
                    response.Errors = new List<string> { "An unexpected error occurred. Please try again." };
                    return response;
                }

                existingEvent.IsActive = false;
                existingEvent.IsDeactivated = true;

                _context.Events.Update(existingEvent);
                await _context.SaveChangesAsync();

                response.Status = true;
                response.Data = true;
                response.Message = "Event Deactivated successfully.";
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = "Unsuccessful";
                response.Errors = new List<string> { "An unexpected error occurred. Please try again." };
            }
            return response;
        }

        public async Task<ApiResponse<bool>> ActivateAsync(Guid id)
        {
            var response = new ApiResponse<bool>();
            var existingEvent = await _context.Events.FindAsync(id);

            if (existingEvent == null)
            {
                response.Status = false;
                response.Message = "Unsuccessful";
                response.Errors = new List<string> { "An unexpected error occurred. Please try again." };
                return response;
            }
            existingEvent.IsActive = true;
            existingEvent.IsDeactivated = false;

            await _context.SaveChangesAsync();

            response.Status = true;
            response.Data = true;
            response.Message = "Event Activated successfully.";
            return response;
        } 
    }
}




