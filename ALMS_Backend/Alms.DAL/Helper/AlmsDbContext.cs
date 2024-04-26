using Alms.DAL.Helper;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Alms.DAL.DbModels
{
    public partial class AlmsDbContext : IdentityDbContext<ApplicationUser>
    {
        public AlmsDbContext(DbContextOptions<AlmsDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
