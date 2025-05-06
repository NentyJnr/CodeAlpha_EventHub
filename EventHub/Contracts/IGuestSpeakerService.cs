using EventHub.Dtos.GuestSpeakers;
using EventHub.Entities;
using EventHub.Responses;

namespace EventHub.Contracts
{
    public interface IGuestSpeakerService
    {
        Task<ApiResponse<GuestSpeakerResponse>> CreateAsync(GuestSpeakerRequest request);
        Task<ApiResponse<UpdateGuestSpeakerResponse>> UpdateAsync(Guid Id, UpdateGuestSpeakerRequest request);
        Task<ApiResponse<bool>> Activate(Guid id);
        Task<ApiResponse<bool>> DeleteAsync(Guid id);
        Task<ApiResponse<List<GuestSpeakerResponse>>> GetAllRecordAsync();
        Task<ApiResponse<GuestSpeakerResponse>> GetRecordByIdAsync(Guid id);
    }
}
