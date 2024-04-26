using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Alms.DAL.ViewModels
{
    public class VmGetLeavesComman
    {
        public long Id { get; set; }
        public int? LeaveId { get; set; }
        public string? Name { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public int? NoOfDays { get; set; }
        public string? Reason { get; set; }

    }
    public class VmSPGetLeaves : VmGetLeavesComman
    {
        public int? StatusId { get; set; }
        public string? StatusName { get; set; }
        public int? LeaveTypeId { get; set; }
        public string? LeaveTypeName { get; set; }
    }

    public class VmLeaveTypes
    {
        public int? LeaveTypeId { get; set; }
        public string? LeaveTypeName { get; set; }
    }

    public class VmGetLeavesWithStatus : VmGetLeavesComman
    {
       public VmStatus? Status { get; set; }
       public VmLeaveTypes? LeaveType { get; set; }
    }

    public class VmGetLeaves
    {
        public int Count { get; set; }

        // public List<VmGetLeavesWithStatus>? Leaves { get; set; }

        public List<VmSPGetLeaves>? Leaves { get; set; }

    }
}
