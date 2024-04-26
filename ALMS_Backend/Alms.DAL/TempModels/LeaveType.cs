using System;
using System.Collections.Generic;

namespace Alms.DAL.TempModels;

public partial class LeaveType
{
    public int Id { get; set; }

    public string LeaveType1 { get; set; } = null!;

    public int? Days { get; set; }

    public bool? IsDeleted { get; set; }

    public DateOnly? InsetedDate { get; set; }

    public int? InsertedUid { get; set; }

    public DateOnly? UpdateDate { get; set; }

    public int? UpdatedUid { get; set; }

    public virtual ICollection<Leaf> Leaves { get; set; } = new List<Leaf>();
}
