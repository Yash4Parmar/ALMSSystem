using Alms.DAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alms.DAL.ViewModels
{
    public class StoreProcedureInputModel
    {

        public string Field { get; set; } = "";
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string Sort { get; set; } = "asc";
    }

    public class GetEmployeesInputModel : StoreProcedureInputModel
    {
        public string? ProjectIds { get; set; }
        public string? EmployeeIds { get; set; }
        public string? ManagerIds { get; set; }

        public bool? ManagerData { get; set; }
    }

    public class GetLeavesInputModel : StoreProcedureInputModel
    {
        public string? LeaveTypeIds { get; set; }
        public string? EmployeeIds { get; set; }
        public string? ManagerIds { get; set; }
        public string? StatusIds { get; set; }

        public string? fromDate { get; set; }
        public string? toDate { get; set; }

        public bool? ManagerLeave { get; set; }
    }

    public class GetProjectsInputModel : StoreProcedureInputModel
    {
        public string? ProjectIds { get; set; }
        public string? EmployeeIds { get; set; }

    }

    public class GetProjectsInputModelInt : StoreProcedureInputModel
    {
        public int[]? ProjectIds { get; set; }
        public int[]? EmployeeIds { get; set; }

    }

    public class GetAttendancesInputModel : StoreProcedureInputModel
    {
        public string EmployeeIds { get; set; } = "";
        public string ManagerIds { get; set; } = "";
        public string StatusIds { get; set; } = "";
        public string fromDate { get; set; } = "";
        public string toDate { get; set; } = "";

        public bool? ManagerAttendance { get; set; } = false;


    }

}