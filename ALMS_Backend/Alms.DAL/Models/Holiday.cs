using System;
using System.Collections.Generic;

namespace Alms.DAL.Models;

public partial class Holiday
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public bool? IsDeleted { get; set; }

    public DateOnly? InsertedDate { get; set; }

    public int? InsertedUid { get; set; }

    public DateOnly? UpdatedDate { get; set; }

    public int? UpdatedUid { get; set; }
}
