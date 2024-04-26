using System;
using System.Collections.Generic;

namespace Alms.DAL.TempModels;

public partial class EmployeeProject
{
    public int Id { get; set; }

    public int? ProjectId { get; set; }

    public int? EmployeeId { get; set; }

    public bool? IsDeleted { get; set; }

    public DateOnly? InsetedDate { get; set; }

    public int? InsertedUid { get; set; }

    public DateOnly? UpdatedDate { get; set; }

    public int? UpdatedUid { get; set; }

    public virtual AspNetUser? Employee { get; set; }

    public virtual Project? Project { get; set; }
}
