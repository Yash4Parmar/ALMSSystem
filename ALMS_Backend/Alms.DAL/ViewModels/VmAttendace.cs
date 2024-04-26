using Alms.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alms.DAL.ViewModels
{
    public class VmAttendaceModel
    {
        public long Id { get; set; }
        public int EmployeeId { get; set; }
        public string? Name { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan WorkingHours { get; set; }
        public int AttendanceId { get; set; }
        public int StatusId { get; set; }

    }

    public class VmAttendace
    {
        public int Count { get; set; }
        public List<VmAttendaceModel>? Attendaces { get; set; }
    }

    public class VmAttendanceInput
    {
        public string Field { get; set; } = "";
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string Sort { get; set; } = "asc";
        public string? EmployeeIds { get; set; }
        public string? ManagerIds { get; set; }
        public string? fromDate { get; set; }
        public string? toDate { get; set; }
        public string? StatusIds { get; set; }
    }

    public class VmAttendancesResponse
    {
        public long Id { get; set; }
        public int EmployeeId { get; set; }
        public string? Name { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public int AttendanceId { get; set; }
    }


    public class VmAttendaceDetails
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }

        public string? Name { get; set; }
        public DateTime Date { get; set; }

        public TimeSpan WorkingHours { get; set; }

        public List<TimeSpan>? Times { get; set; }


    }

    public class VmAttendaceDetailsResponse
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string? Name { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
    }

    public class VmAttendaceDetailsInput
    {
        public int EmployeeId { get; set; }
        public string? Date { get; set; }
    }


    public class VmAddAttendanceInput
    {
        public string? Time { get; set; }

        public int EmployeeId { get; set; }

        public string? Date { get; set; }

        public int AttendaceTypeId { get; set; } = 7;

        public int? StatusId { get; set; }
    }


    public class VmRequestAttendanceInput
    {
        public int AttendanceId { get; set; }
        public string[]? Times { get; set; }
    }

    public class VmAttendanceRequestModel
    {
        public int AttendanceId { get; set; }
        public string? Times { get; set; }
    }

    public class VmAttendanceUpdateInput
    {
        public int AttendanceId { get; set; }
        public int StatusId { get; set; }
    }

    public class VmGetRequestedAttendanceByIdInput
    {
        public int AttendanceId { get; set; }
    }

    public class VmGetRequestedAttendanceById
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }

        public string? Name { get; set; }
        public DateTime Date { get; set; }

        public TimeSpan WorkingHours { get; set; }

        public List<TimeSpan>? Times { get; set; }


    }

    public class VmGetRequestedAttendanceByIdResponse
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string? Name { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
    }



    public class VmGetAttendanceByAttenndanceIdInput
    {
        public int AttendanceId { get; set; }
    }

    public class VmGetAttendanceByAttenndanceId
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }

        public string? Name { get; set; }
        public DateTime Date { get; set; }

        public TimeSpan WorkingHours { get; set; }

        public List<TimeSpan>? Times { get; set; }


    }

    public class VmGetAttendanceByAttenndanceIdResponse
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string? Name { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
    }
}
