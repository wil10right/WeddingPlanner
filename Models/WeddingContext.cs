using Microsoft.EntityFrameworkCore;

namespace WeddingCrasher.Models
{
    public class WeddingContext : DbContext
    {
        public WeddingContext(DbContextOptions<WeddingContext> options) : base(options){}
        //models go under here as DbSet<ModelName>
        public DbSet<User> users {get;set;}
        public DbSet<Wedding> weddings {get;set;}
        public DbSet<Rsvp> rsvps {get;set;}
    }
}