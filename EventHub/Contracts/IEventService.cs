using EventHub.Dtos.Events;
using EventHub.Dtos.Registrations;
using EventHub.Entities;
using EventHub.Responses;
using Microsoft.AspNetCore.Identity.Data;

namespace EventHub.Contracts
{
    public interface IEventService
    {
        Task<ApiResponse<CreateEventResponse>> CreateAsync(CreateEventRequest request);
        Task<ApiResponse<bool>> UpdateAsync(UpdateEventRequest request);
        Task<ApiResponse<bool>> ActivateAsync(Guid id);
        Task<ApiResponse<bool>> DeactivateAsync(Guid id);
        Task<ApiResponse<List<EventResponse>>> GetAllRecord();
        Task<ApiResponse<EventResponse>> GetRecordById(Guid id);
    }
}
