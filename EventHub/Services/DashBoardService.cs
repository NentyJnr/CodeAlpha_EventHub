using EventHub.Context;
using EventHub.Contracts;
using EventHub.Dtos;
using EventHub.Responses;
using EventHub.Services;
using Microsoft.EntityFrameworkCore;
using System;

namespace EventHub.Services
{
    public class DashBoardService : IDashboardService
    {
        private readonly ApplicationDbContext _context;

        public DashBoardService(ApplicationDbContext dbContext)
        {
            _context = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<ApiResponse<DashboardResponse>> GetDashboardKPIsAsync()
        {
            var response = new ApiResponse<DashboardResponse>();
            try
            {
                var totalEvents = await _context.Events.CountAsync();
                var totalUpcomingEvents = await _context.Events.CountAsync(e => e.StartDate > DateTime.Now);
                var totalOngoingEvents = await _context.Events.CountAsync(e => e.StartDate.Date <= DateTime.Now.Date && e.EndDate.Date >= DateTime.Now.Date);
                var totalPastEvents = await _context.Events.CountAsync(e => e.EndDate.Date < DateTime.Now.Date);

                var data = new DashboardResponse
                {
                    TotalEvents = totalEvents,
                    TotalUpcomingEvents = totalUpcomingEvents,
                    TotalOngoingEvents = totalOngoingEvents,
                    TotalPastEvents = totalPastEvents
                };
                response.Status = true;
                response.Data = data;
                response.Message = "Data Populated Successfully";
            }
            catch
            {
                response.Status = false;
                response.Message = "Unsuccessful";
                response.Errors = new List<string> { "An unexpected error occurred. Please try again." };
            }
            return response;
        }
    }
}

