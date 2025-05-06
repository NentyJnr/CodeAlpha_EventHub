using EventHub.Dtos.Tags;
using EventHub.Entities;
using EventHub.Responses;
using Microsoft.EntityFrameworkCore.Storage;

namespace EventHub.Contracts
{
    public interface ITagService
    {
        Task<ApiResponse<bool>> UploadPassport(UploadTagPassportRequest uploadTagPassport);
        Task<ApiResponse<TagResponse>> GetTag(TagRequest request);
        Task<ApiResponse<bool>> GenerateTagInternal(
           RegistrationForm registration,
           string reference,
           IDbContextTransaction transaction,
           bool canCommit);
        string GenerateUniqueReference(Guid registrationId);
        Task<string?> GetReferenceByRegistrationId(Guid registrationId);

    }
}
