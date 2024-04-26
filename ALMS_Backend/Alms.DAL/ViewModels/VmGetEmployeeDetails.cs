using Alms.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alms.DAL.ViewModels
{
    public class VmGetEmployeeDetailsCommon
    {
        public int? Id { get; set; }
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? PhoneNumber { get; set; }

        public DateTime? DateOfJoin { get; set; }

        public string? Role { get; set; }
        public string? Email { get; set; }

        public string? Gender { get; set; }
        public string? Address { get; set; }
        public DateTime? DOB { get; set; }
        public int? ManagerId { get; set; }
        public string? Manager { get; set; }
    }
    public class VmSPGetEmployeeDetails : VmGetEmployeeDetailsCommon
    {
        public int? ProjectId { get; set; }
        public string? Project { get; set; }

        public int? EmployeeId { get; set; }
        public string? Employee { get; set; }

    }

    public class VmGetEmployeeDetails : VmGetEmployeeDetailsCommon
    {
        public List<ProjectObj> Projects { get; set; } = new List<ProjectObj>();
        public List<EmployeeObj> Employees { get; set; } = new List<EmployeeObj>();
    }


    public class ProjectObj
    {
        public int? ProjectId { get; set; }
        public string? Project { get; set; }

    }

    public class EmployeeObj
    {
        public int? EmployeeId { get; set; }
        public string? Employee { get; set; }
    }

}
