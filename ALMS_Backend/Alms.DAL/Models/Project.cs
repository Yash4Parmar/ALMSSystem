using System;
using System.Collections.Generic;

namespace Alms.DAL.Models;

public partial class Project
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public bool? IsDeleted { get; set; }

    public DateOnly? InsetedDate { get; set; }

    public int? InsertedUid { get; set; }

    public DateOnly? UpdatedDate { get; set; }

    public int? UpdatedUid { get; set; }

    public virtual ICollection<EmployeeProject> EmployeeProjects { get; set; } = new List<EmployeeProject>();

    public virtual ICollection<ProjectManager> ProjectManagers { get; set; } = new List<ProjectManager>();
}
