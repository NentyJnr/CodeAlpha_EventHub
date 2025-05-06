using EventHub.Dtos.Registrations;
using EventHub.Entities;
using EventHub.Responses;
using Microsoft.SqlServer.Server;

namespace EventHub.Contracts
{
    public interface IRegistrationService
    {

        Task<ApiResponse<RegisterResponse>> RegisterAsync(RegisterRequest request);
        Task<ApiResponse<RegisterResponse>> GetRegistrationByIdAsync(Guid registrationId);
        Task<ApiResponse<List<RegisterResponse>>> GetAllRegistrationByEventAsync(Guid eventId);
        Task<ApiResponse<string>> UpdateRegistrationAsync(Guid registrationId, UpdateRegisterRequest request);
    }
}
