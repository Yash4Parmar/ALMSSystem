
using Microsoft.AspNetCore.Identity;

namespace Alms.DAL.Helper
{
    public class ApplicationUser : IdentityUser
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly DateOfJoin { get; set; }
    }
}
