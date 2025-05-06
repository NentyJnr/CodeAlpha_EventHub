using EventHub.Dtos.EventUploads;
using EventHub.Entities;
using EventHub.Responses;

namespace EventHub.Contracts
{
    public interface IEventUploadService
    {
        Task<ApiResponse<EventUploadResponse>> CreateUploadAsync(EventUploadRequest request);
        Task<ApiResponse<UpdateEventUploadResponse>> UpdateUploadAsync( Guid Id, UpdateEventUploadRequest request);
        Task<ApiResponse<bool>> DeleteUploadAsync(Guid id);
        Task<ApiResponse<List<UploadResponse>>> GetAllRecordAsync();
        Task<ApiResponse<UploadResponse>> GetRecordByIdAsync(Guid id);
    }
}
