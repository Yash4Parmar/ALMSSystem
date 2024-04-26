using System;
using System.Collections.Generic;

namespace Alms.DAL.TempModels;

public partial class ProjectManager
{
    public int Id { get; set; }

    public int? ProjectId { get; set; }

    public int? ManagerId { get; set; }

    public bool? IsDeleted { get; set; }

    public DateOnly? InsetedDate { get; set; }

    public int? InsertedUid { get; set; }

    public DateOnly? UpdatedDate { get; set; }

    public int? UpdatedUid { get; set; }

    public virtual AspNetUser? Manager { get; set; }

    public virtual Project? Project { get; set; }
}
