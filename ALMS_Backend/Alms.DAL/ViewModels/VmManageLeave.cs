using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alms.DAL.ViewModels
{

    public class VmLeaveCommon
    {
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? Reason { get; set; }

        public int LeaveTypeId { get; set;}
        public int? StatusId { get; set;}

    }

    public class VmLeave : VmLeaveCommon
    {
        public int Id { get; set; }
        public int NoOfDays { get; set; }
        public int? UID { get; set; }

    }

    public class VmAddLeaveCommon : VmLeaveCommon
    {
        public int? EmployeeId { get; set; }
    }

    public class VmAddLeave : VmAddLeaveCommon
    {
        public int NoOfDays { get; set; }
        public int? UID { get; set; }

    }

    // VmUpdateLeaves modal is a responce class when leave is Update and Add
    public class VmManageLeave
    {
        public bool? IsValid { get; set; }

        public string? Message { get; set; }


    }
}
