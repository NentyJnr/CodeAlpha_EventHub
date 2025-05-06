using EventHub.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace EventHub.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<Events> Events { get; set; }
        public virtual DbSet<EventUploads> Uploads { get; set; }
        public virtual DbSet<RegistrationForm> RegistrationForms { get; set; }
        public virtual DbSet<GuestSpeaker> GuestSpeakers { get; set; }
        public virtual DbSet<Tags> Tags { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Events>()
                .HasMany(e => e.GuestSpeakers)
                .WithOne(gs => gs.Event)  
                .HasForeignKey(gs => gs.EventId)  
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
