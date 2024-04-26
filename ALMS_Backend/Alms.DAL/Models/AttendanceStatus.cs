using System;
using System.Collections.Generic;

namespace Alms.DAL.Models;

public partial class AttendanceStatus
{
    public int Id { get; set; }

    public string StatusName { get; set; } = null!;

    public int? IsDeleted { get; set; }

    public DateOnly? InsetedDate { get; set; }

    public int? InsertedUid { get; set; }

    public DateOnly? UpdateDate { get; set; }

    public int? UpdatedUid { get; set; }

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public virtual ICollection<PunchTime> PunchTimes { get; set; } = new List<PunchTime>();
}
