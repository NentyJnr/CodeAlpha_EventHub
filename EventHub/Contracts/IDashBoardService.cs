using EventHub.Dtos;
using EventHub.Responses;

namespace EventHub.Contracts
{
    public interface IDashboardService
    {
        Task<ApiResponse<DashboardResponse>> GetDashboardKPIsAsync();
    }
}
