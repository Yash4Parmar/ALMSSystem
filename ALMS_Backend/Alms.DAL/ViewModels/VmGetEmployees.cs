using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Alms.DAL.ViewModels
{
    public class VmSPGetEmployees
    {
        public long Id { get; set; }
        //public string? Id { get; set; }
        public int? EmployeeId { get; set; }
        public string? Name { get; set; }
        public DateTime? DateOfJoin { get; set; }
        public string? Role { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }

    }
    public class VmGetEmployees
    {
        public int Count { get; set; }

        public List<VmSPGetEmployees>? Employees { get; set; }

    }

    public class VmEmployee
    {
        public int? Id { get; set; }
    }
}
