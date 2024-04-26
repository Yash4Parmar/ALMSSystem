using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alms.DAL.ViewModels
{
    public class VmGetProjectDetailsCommon
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Manager { get; set; }
        public int? ManagerId { get; set; }

    }

    public class VmSPGetProjectDetails : VmGetProjectDetailsCommon
    {
        public string? Employee { get; set; }
        public int? EmployeeId { get; set; }
    }

    public class ProjectEmployeeObj
    {
        public int? EmployeeId { get; set; }
        public string? Employee { get; set; }
    }

    public class VmGetProjectDetails : VmGetProjectDetailsCommon
    {
        public List<ProjectEmployeeObj> Employees { get; set; } = new List<ProjectEmployeeObj>();
    }
}