using System;
using System.Collections.Generic;
using Alms.DAL.Models;
using Alms.DAL.TempModels;

namespace Alms.DAL.Models;

public partial class Leave
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public int NoOfDays { get; set; }

    public int? LeaveTypeId { get; set; }

    public string? Reason { get; set; }

    public int? StatusId { get; set; }

    public bool? IsDeleted { get; set; }

    public DateOnly? InsertedDate { get; set; }

    public int? InsertedUid { get; set; }

    public DateOnly? UpdateDate { get; set; }

    public int? UpdatedUid { get; set; }

  //  public virtual AspNetUser? Employee { get; set; }

    public virtual ICollection<LeaveAttachment> LeaveAttachments { get; set; } = new List<LeaveAttachment>();

    public virtual LeaveTypes? LeaveType { get; set; }

    public virtual Status? Status { get; set; }
}
