using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alms.DAL.ViewModels
{
    public class VmSPGetProjectsCommon
    {
        public long Id { get; set; }
        public int? ProjectId { get; set; }
        public string? Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Manager { get; set; }

    }

    public class VmSPGetProjects : VmSPGetProjectsCommon
    {
        public string? Employees { get; set; }
    }

    public class VmProjects : VmSPGetProjectsCommon
    {
        public string[]? Employees { get; set; }
    }

    public class VmGetProjects
    {
        public int Count { get; set; }

        public List<VmProjects>? Projects { get; set; }

    }
}