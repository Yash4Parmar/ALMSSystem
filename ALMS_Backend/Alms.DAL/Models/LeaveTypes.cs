using Alms.DAL.TempModels;
using System;
using System.Collections.Generic;

namespace Alms.DAL.Models;

public partial class LeaveTypes
{
    public int Id { get; set; }

    public string? LeaveType { get; set; }

    public int? Days { get; set; }

    public bool? IsDeleted { get; set; }

    public DateOnly? InsetedDate { get; set; }

    public int? InsertedUid { get; set; }

    public DateOnly? UpdateDate { get; set; }

    public int? UpdatedUid { get; set; }

    public virtual ICollection<Leave> Leaves { get; set; } = new List<Leave>();
}
