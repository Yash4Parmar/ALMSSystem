using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alms.DAL.ViewModels
{
    public class VmUpdateEmployee
    {

        public int? UpdateUID { get; set; }
        public int? EmployeeId { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string? DOB { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }

        public string? DateOfJoin { get; set; }

        public string? PhoneNumber { get; set; }

        public int? ManagerId { get; set; }

        public string? ProjectIds { get; set; }

        public string? EmployeeIds { get; set; }

        public string? RoleName { get; set; }
    }

    public class VmUpdateEmployeeInput
    {
        public int? UpdateUID { get; set; }
        public int? EmployeeId { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string? DOB { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }

        public string? DateOfJoin { get; set; }

        public string? PhoneNumber { get; set; }

        public int? ManagerId { get; set; }

        public int[]? ProjectIds { get; set; }

        public int[]? EmployeeIds { get; set; }

        public string? RoleName { get; set; }

    }


}
