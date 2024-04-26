using System;
using System.Collections.Generic;

namespace Alms.DAL.Models;

public partial class AttendanceType
{
    public int Id { get; set; }

    public string AttendanceType1 { get; set; } = null!;

    public bool? IsDeleted { get; set; }

    public DateOnly? InsetedDate { get; set; }

    public int? InsertedUid { get; set; }

    public DateOnly? UpdateDate { get; set; }

    public int? UpdatedUid { get; set; }

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
}
