using System;
using System.Collections.Generic;

namespace Alms.DAL.TempModels;

public partial class Status
{
    public int Id { get; set; }

    public string? StatusName { get; set; }

    public bool? IsDeleted { get; set; }

    public DateOnly? InsertedDate { get; set; }

    public int? InsertedUid { get; set; }

    public DateOnly? UpdateDate { get; set; }

    public int? UpdatedUid { get; set; }

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public virtual ICollection<Leaf> Leaves { get; set; } = new List<Leaf>();

    public virtual ICollection<PunchTime> PunchTimes { get; set; } = new List<PunchTime>();
}
