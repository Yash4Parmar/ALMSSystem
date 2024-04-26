using System;
using System.Collections.Generic;
using Alms.DAL.Helper;
using Alms.DAL.TempModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Alms.DAL.Models;

public partial class WbLearningContext : IdentityDbContext<ApplicationUser>
{
    public WbLearningContext()
    {
    }

    public WbLearningContext(DbContextOptions<WbLearningContext> options)
        : base(options)
    {
    }


    public virtual DbSet<Attendance> Attendances { get; set; }

    public virtual DbSet<AttendanceType> AttendanceTypes { get; set; }

    public virtual DbSet<EmployeeManager> EmployeeManagers { get; set; }

    public virtual DbSet<EmployeeProject> EmployeeProjects { get; set; }

    public virtual DbSet<Holiday> Holidays { get; set; }

    public virtual DbSet<Leave> Leaves { get; set; }

    public virtual DbSet<LeaveAttachment> LeaveAttachments { get; set; }

    public virtual DbSet<LeaveTypes> LeaveTypes { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<ProjectManager> ProjectManagers { get; set; }

    public virtual DbSet<PunchTime> PunchTimes { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ApplicationUser>()
           .Property(u => u.EmployeeId)
           .UseIdentityColumn(); 

        modelBuilder.Entity<ApplicationUser>()
            .Property(f => f.FirstName);
        modelBuilder.Entity<ApplicationUser>()
            .Property(l => l.LastName);
        modelBuilder.Entity<ApplicationUser>()
            .Property(d => d.DateOfJoin);

        modelBuilder.Entity<AspNetUserLogin>()
            .HasKey(l => new { l.LoginProvider, l.ProviderKey });

        modelBuilder.Entity<AspNetUserToken>()
            .HasKey(t => new { t.UserId, t.LoginProvider, t.Name });

    }
}
