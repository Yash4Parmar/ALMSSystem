using System;
using System.Collections.Generic;

namespace Alms.DAL.TempModels;

public partial class Leaf
{
    public int Id { get; set; }

    public int? EmployeeId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public int NoOfDays { get; set; }

    public int? LeaveTypeId { get; set; }

    public string? Reason { get; set; }

    public int? StatusId { get; set; }

    public bool? IsDeleted { get; set; }

    public DateOnly? InsetedDate { get; set; }

    public int? InsertedUid { get; set; }

    public DateOnly? UpdateDate { get; set; }

    public int? UpdatedUid { get; set; }

    public virtual AspNetUser? Employee { get; set; }

    public virtual ICollection<LeaveAttachment> LeaveAttachments { get; set; } = new List<LeaveAttachment>();

    public virtual LeaveType? LeaveType { get; set; }

    public virtual Status? Status { get; set; }
}
