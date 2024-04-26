using Alms.DAL.TempModels;
using System;
using System.Collections.Generic;

namespace Alms.DAL.Models;

public partial class LeaveAttachment
{
    public int Id { get; set; }

    public int? LeaveId { get; set; }

    public string? FileData { get; set; }

    public bool? IsDeleted { get; set; }

    public DateOnly? InsertedDate { get; set; }

    public int? InsertedUid { get; set; }

    public DateOnly? UpdateDate { get; set; }

    public int? UpdatedUid { get; set; }

    public virtual Leave? Leave { get; set; }
}
