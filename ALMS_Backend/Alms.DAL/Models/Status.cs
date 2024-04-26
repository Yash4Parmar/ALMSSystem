using Alms.DAL.TempModels;
using System;
using System.Collections.Generic;

namespace Alms.DAL.Models;

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

    public virtual ICollection<Leave> Leaves { get; set; } = new List<Leave>();

    public virtual ICollection<PunchTime> PunchTimes { get; set; } = new List<PunchTime>();
}
