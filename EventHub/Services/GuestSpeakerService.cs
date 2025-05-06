using EventHub.Context;
using EventHub.Contracts;
using EventHub.Dtos.GuestSpeakers;
using EventHub.Entities;
using EventHub.Extension;
using EventHub.Responses;
using Microsoft.EntityFrameworkCore;
using System;

namespace EventHub.Services
{
    public class GuestSpeakerService : IGuestSpeakerService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUploadHelper _uploadHelper;
        public GuestSpeakerService(ApplicationDbContext context, IUploadHelper uploadHelper)
        {
            _context = context;
            _uploadHelper = uploadHelper;
        }

        public async Task<ApiResponse<GuestSpeakerResponse>> CreateAsync(GuestSpeakerRequest request)
        {
            var response = new ApiResponse<GuestSpeakerResponse>();
            try
            {
                var speaker = await _context.GuestSpeakers
                    .FirstOrDefaultAsync(e => e.FirstName == request.LastName && e.EventId == request.EventId);

                if (speaker != null)
                {
                    response.Status = false;
                    response.Message = "Speaker already exist";
                    return response;
                }

                string? ImagePath = null;
                if (request.Image != null)
                {
                    ImagePath = await _uploadHelper.UploadImage(request.Image, false);
                }

                speaker = new GuestSpeaker
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    EventId = request.EventId,
                    Topic = request.Topic,
                    Biography = request.Biography,
                    ImageUrl = ImagePath,
                };

                speaker.IsActive = true;
                speaker.DateCreated = DateTime.Now;
                speaker.IsDeactivated = false;

                string image = await _uploadHelper.UploadImage(request.Image);
                speaker.ImageUrl = image;

                _context.GuestSpeakers.Add(speaker);
                await _context.SaveChangesAsync();

                response.Status = true;
                response.Data = new GuestSpeakerResponse
                {
                    FirstName = speaker.FirstName,
                    LastName = speaker.LastName,
                    EventId = speaker.EventId,
                    Topic = speaker.Topic,
                    Biography = speaker.Biography,
                    Image = speaker.ImageUrl,
                };
                response.Message = "Guest Speaker created successfully.";

            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = "Unsuccessful";
                response.Errors = new List<string> { "An error occurred while creating Guestspeaker." };
            }
            return response;
        }

        public async Task<ApiResponse<GuestSpeakerResponse>> GetRecordByIdAsync(Guid id)
        {
            var response = new ApiResponse<GuestSpeakerResponse>();
            try
            {
                var record = await _context.GuestSpeakers
                    .Where(e => e.Id == id)
                    .Select(e => new GuestSpeakerResponse
                    {
                        Id = e.Id,
                        FirstName = e.FirstName,
                        LastName = e.LastName,
                        Biography = e.Biography,
                        Topic = e.Topic,
                        Image = e.ImageUrl,
                        EventId = e.EventId,
                        DateCreated = e.DateCreated,
                        IsActive = e.IsActive,
                        IsDeactivated = e.IsDeactivated,
                        DateModified = e.DateModified
                    })
                    .FirstOrDefaultAsync();

                if (record == null)
                {
                    response.Status = false;
                    response.Message = "Unsuccessful";
                    response.Errors = new List<string> { "An Event Exist." };
                    return response;
                }

                response.Data = record;
                response.Status = true;
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = "Unsuccessful";
                response.Errors = new List<string> { "An error occurred while Getting record." };
            }
            return response;
        }

        public async Task<ApiResponse<List<GuestSpeakerResponse>>> GetAllRecordAsync()
        {
            var response = new ApiResponse<List<GuestSpeakerResponse>>();
            try
            {
                var data = await _context.GuestSpeakers.ToListAsync();
                var records = data.Select(e => new GuestSpeakerResponse
                {
                    Id = e.Id,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Biography = e.Biography,
                    Topic = e.Topic,
                    Image = e.ImageUrl,
                    EventId = e.EventId,
                    DateCreated = e.DateCreated,
                    IsActive = e.IsActive,
                    IsDeactivated = e.IsDeactivated,
                    DateModified = e.DateModified
                }).ToList();
                
                response.Data = records;
                response.Status = true;
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = "Unsuccessful";
                response.Errors = new List<string> { "An error occurred while Getting Guestspeaker." };
                response.Data = new List<GuestSpeakerResponse>();
            }
            return response;
        }

        public async Task<ApiResponse<UpdateGuestSpeakerResponse>> UpdateAsync(Guid Id, UpdateGuestSpeakerRequest request)
        {
            var response = new ApiResponse<UpdateGuestSpeakerResponse>();
            try
            {
                var exSpeaker = await _context.GuestSpeakers.FirstOrDefaultAsync();
                if (exSpeaker == null)
                {
                    response.Status = false;
                    response.Message = "Guest Speaker not found.";
                    return response;
                }

                exSpeaker.FirstName = request.FirstName;
                exSpeaker.LastName = request.LastName;
                exSpeaker.Topic = request.Topic;
                exSpeaker.Biography = request.Biography;

                string image = await _uploadHelper.UploadImage(request.Image);
                exSpeaker.ImageUrl = image;
                exSpeaker.DateModified = DateTime.Now;

                _context.GuestSpeakers.Update(exSpeaker);
                await _context.SaveChangesAsync();

                response.Status = true;
                response.Data = new UpdateGuestSpeakerResponse
                {
                    FirstName = exSpeaker.FirstName,
                    LastName = exSpeaker.LastName,
                    Topic = exSpeaker.Topic,
                    Biography = exSpeaker.Biography,
                    Image = exSpeaker.ImageUrl,
                    EventId = exSpeaker.EventId,
                    DateModified = DateTime.UtcNow
                }; response.Message = "Guest Speaker Updated successfully.";

            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = "Unsuccessful";
                response.Errors = new List<string> { "An error occurred while Updating Guestspeaker." };
            }
            return response;
        }

        public async Task<ApiResponse<bool>> DeleteAsync(Guid Id)
        {
            var response = new ApiResponse<bool>();
            try
            {
                var existingSpeaker = await _context.GuestSpeakers.FindAsync(Id);

                if (existingSpeaker == null)
                {
                    response.Status = false;
                    response.Message = "Guest Speaker not found.";
                    return response;
                }
                existingSpeaker.IsActive = false;

                _context.GuestSpeakers.Update(existingSpeaker);
                await _context.SaveChangesAsync();

                response.Status = true;
                response.Data = true;
                response.Message = "Guest Speaker Deleted successfully.";
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = "Unsuccessful";
                response.Errors = new List<string> { "An error occurred while Updating Guestspeaker." };
            }
            return response;
        }

        public async Task<ApiResponse<bool>> Activate(Guid id)
        {
            var response = new ApiResponse<bool>();
            try
            {      
                var existingSpeaker = await _context.GuestSpeakers.FindAsync(id);

                if (existingSpeaker == null)
                {
                    response.Status = false;
                    response.Message = "Unsuccessful";
                    response.Errors = new List<string> { "Speaker not found." };
                    return response;
                }
                existingSpeaker.IsActive = true;
                existingSpeaker.IsDeactivated = false;
                await _context.SaveChangesAsync();

                response.Status = true;
                response.Data = true;
                response.Message = "Speaker Activated successfully.";
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = "Unsuccessful";
                response.Errors = new List<string> { "An error occurred while Activating Guestspeaker." };
            }
            return response;
        }
    }
}


