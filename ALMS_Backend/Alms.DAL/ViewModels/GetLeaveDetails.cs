using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alms.DAL.ViewModels
{
    public class VmGetLeaveDetailsCommon
    {
        public int? LeaveId { get; set; }
       
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public int? NoOfDays { get; set; }

        public string? Reason { get; set; }

    }


    public class VmGetLeaveDetails : VmGetLeaveDetailsCommon
    {
        public int? LeaveTypeId { get; set; }
        public string? LeaveType { get; set; }

        public int? EmployeeId { get; set; }
        public string? Name { get; set; }

        public int? StatusId { get; set; }
        public string? StatusName { get; set; }
    }
}
