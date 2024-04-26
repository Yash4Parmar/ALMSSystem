using System;
using System.Collections.Generic;

namespace Alms.DAL.Models;

public partial class EmployeeManager
{
    public int Id { get; set; }

    public int? EmployeeId { get; set; }

    public int? ManagerId { get; set; }

    public bool? IsDeleted { get; set; }

    public DateOnly? InsertedDate { get; set; }

    public int? InsertedUid { get; set; }

    public DateOnly? UpdatedDate { get; set; }

    public int? UpdatedUid { get; set; }

    public virtual AspNetUser? Employee { get; set; }

    public virtual AspNetUser? Manager { get; set; }
}
