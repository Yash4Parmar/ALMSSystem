using System;
using System.Collections.Generic;

namespace Alms.DAL.Models;

public partial class Attendance
{
    public int Id { get; set; }

    public int? EmployeeId { get; set; }

    public DateOnly Date { get; set; }

    public int? AttendanceTypeId { get; set; }

    public int? StatusId { get; set; }

    public bool? IsDeleted { get; set; }

    public DateOnly? InsertedDate { get; set; }

    public int? InsertedUid { get; set; }

    public DateOnly? UpdateDate { get; set; }

    public int? UpdatedUid { get; set; }

    public virtual AttendanceType? AttendanceType { get; set; }

    public virtual AspNetUser? Employee { get; set; }

    public virtual ICollection<PunchTime> PunchTimes { get; set; } = new List<PunchTime>();

    public virtual Status? Status { get; set; }
}
