using AdvanceChat.Client.Models;
using ChatModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AdvanceChat.Data
{
    public class ApplicationDbContext :  IdentityDbContext<AppUser>
    {
        public DbSet<Chat> chats { get; set; }
        public DbSet<AvaliableUser> avaliableUsers { get; set; }
        public DbSet<IndividualChat> individualChats { get; set; }
        IHttpContextAccessor contextAccessor { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor _contextAccessor) : base(options)
        {
            contextAccessor = _contextAccessor;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
