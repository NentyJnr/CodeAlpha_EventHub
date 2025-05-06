namespace EventHub.Dtos
{
    public class DashboardResponse
    {
        public int TotalEvents { get; set; }
        public int TotalUpcomingEvents { get; set; }
        public int TotalOngoingEvents { get; set; }
        public int TotalPastEvents { get; set; }
    }
}
