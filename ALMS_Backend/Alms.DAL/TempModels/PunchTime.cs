using System;
using System.Collections.Generic;

namespace Alms.DAL.TempModels;

public partial class PunchTime
{
    public int Id { get; set; }

    public int? AttendanceId { get; set; }

    public TimeOnly Time { get; set; }

    public int? StatusId { get; set; }

    public bool? IsDeleted { get; set; }

    public DateOnly? InsertedDate { get; set; }

    public int? InsertedUid { get; set; }

    public DateOnly? UpdateDate { get; set; }

    public int? UpdatedUid { get; set; }

    public virtual Attendance? Attendance { get; set; }

    public virtual Status? Status { get; set; }
}
