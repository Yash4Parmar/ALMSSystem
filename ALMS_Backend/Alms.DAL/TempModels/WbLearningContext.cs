using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Alms.DAL.TempModels;

public partial class WbLearningContext : DbContext
{
    public WbLearningContext()
    {
    }

    public WbLearningContext(DbContextOptions<WbLearningContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<Attendance> Attendances { get; set; }

    public virtual DbSet<AttendanceType> AttendanceTypes { get; set; }

    public virtual DbSet<EmployeeManager> EmployeeManagers { get; set; }

    public virtual DbSet<EmployeeProject> EmployeeProjects { get; set; }

    public virtual DbSet<Holiday> Holidays { get; set; }

    public virtual DbSet<Leaf> Leaves { get; set; }

    public virtual DbSet<LeaveAttachment> LeaveAttachments { get; set; }

    public virtual DbSet<LeaveType> LeaveTypes { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<ProjectManager> ProjectManagers { get; set; }

    public virtual DbSet<PunchTime> PunchTimes { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.EmployeeId, "UC_EmployeeId").IsUnique();

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Dob).HasColumnName("DOB");
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.EmployeeId).ValueGeneratedOnAdd();
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.InsertedUid).HasColumnName("InsertedUID");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UpdatedUid).HasColumnName("UpdatedUID");
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("AspNetUserRoles");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Attendance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Attendan__3214EC07AB78F50C");

            entity.Property(e => e.InsertedUid).HasColumnName("InsertedUID");
            entity.Property(e => e.UpdatedUid).HasColumnName("UpdatedUID");

            entity.HasOne(d => d.AttendanceType).WithMany(p => p.Attendances)
                .HasForeignKey(d => d.AttendanceTypeId)
                .HasConstraintName("FK__Attendanc__Atten__0B91BA14");

            entity.HasOne(d => d.Employee).WithMany(p => p.Attendances)
                .HasPrincipalKey(p => p.EmployeeId)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK_AspNetUsers_EmployeeId");

            entity.HasOne(d => d.Status).WithMany(p => p.Attendances)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("FK_Attendances_Status");
        });

        modelBuilder.Entity<AttendanceType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Attendan__3214EC0751CDCF45");

            entity.Property(e => e.AttendanceType1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("AttendanceType");
            entity.Property(e => e.InsertedUid).HasColumnName("InsertedUID");
            entity.Property(e => e.UpdatedUid).HasColumnName("UpdatedUID");
        });

        modelBuilder.Entity<EmployeeManager>(entity =>
        {
            entity.ToTable("EmployeeManager");

            entity.Property(e => e.InsertedUid).HasColumnName("InsertedUID");
            entity.Property(e => e.UpdatedUid).HasColumnName("UpdatedUID");

            entity.HasOne(d => d.Employee).WithMany(p => p.EmployeeManagerEmployees)
                .HasPrincipalKey(p => p.EmployeeId)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK_EmployeeManager_EmployeeId");

            entity.HasOne(d => d.Manager).WithMany(p => p.EmployeeManagerManagers)
                .HasPrincipalKey(p => p.EmployeeId)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("FK_EmployeeManager1_EmployeeId");
        });

        modelBuilder.Entity<EmployeeProject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC0722CC5884");

            entity.Property(e => e.InsertedUid).HasColumnName("InsertedUID");
            entity.Property(e => e.UpdatedUid).HasColumnName("UpdatedUID");

            entity.HasOne(d => d.Employee).WithMany(p => p.EmployeeProjects)
                .HasPrincipalKey(p => p.EmployeeId)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK_EmployeeProjects_EmployeeId");

            entity.HasOne(d => d.Project).WithMany(p => p.EmployeeProjects)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("FK__EmployeeP__Proje__10566F31");
        });

        modelBuilder.Entity<Holiday>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Holidays__3214EC07D3DF1757");

            entity.Property(e => e.InsertedUid).HasColumnName("InsertedUID");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUid).HasColumnName("UpdatedUID");
        });

        modelBuilder.Entity<Leaf>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Leaves__3214EC076E594447");

            entity.Property(e => e.InsertedUid).HasColumnName("InsertedUID");
            entity.Property(e => e.UpdatedUid).HasColumnName("UpdatedUID");

            entity.HasOne(d => d.Employee).WithMany(p => p.Leaves)
                .HasPrincipalKey(p => p.EmployeeId)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK_Leaves_EmployeeId");

            entity.HasOne(d => d.LeaveType).WithMany(p => p.Leaves)
                .HasForeignKey(d => d.LeaveTypeId)
                .HasConstraintName("FK__Leaves__LeaveTyp__1332DBDC");

            entity.HasOne(d => d.Status).WithMany(p => p.Leaves)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("FK__Leaves__StatusId__14270015");
        });

        modelBuilder.Entity<LeaveAttachment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LeaveAtt__3214EC07FBDB57BD");

            entity.Property(e => e.FileData)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.InsertedUid).HasColumnName("InsertedUID");
            entity.Property(e => e.UpdatedUid).HasColumnName("UpdatedUID");

            entity.HasOne(d => d.Leave).WithMany(p => p.LeaveAttachments)
                .HasForeignKey(d => d.LeaveId)
                .HasConstraintName("FK__LeaveAtta__Leave__123EB7A3");
        });

        modelBuilder.Entity<LeaveType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LeaveTyp__3214EC071E9CD8E4");

            entity.Property(e => e.InsertedUid).HasColumnName("InsertedUID");
            entity.Property(e => e.LeaveType1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("LeaveType");
            entity.Property(e => e.UpdatedUid).HasColumnName("UpdatedUID");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Projects__3214EC076F8D57BE");

            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.InsertedUid).HasColumnName("InsertedUID");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUid).HasColumnName("UpdatedUID");
        });

        modelBuilder.Entity<ProjectManager>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProjectM__3214EC07B7155873");

            entity.ToTable("ProjectManager");

            entity.Property(e => e.InsertedUid).HasColumnName("InsertedUID");
            entity.Property(e => e.UpdatedUid).HasColumnName("UpdatedUID");

            entity.HasOne(d => d.Manager).WithMany(p => p.ProjectManagers)
                .HasPrincipalKey(p => p.EmployeeId)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("FK_ProjectManger_EmployeeId");

            entity.HasOne(d => d.Project).WithMany(p => p.ProjectManagers)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("FK__ProjectMa__Proje__160F4887");
        });

        modelBuilder.Entity<PunchTime>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PunchTim__3214EC078F6A3419");

            entity.Property(e => e.InsertedUid).HasColumnName("InsertedUID");
            entity.Property(e => e.UpdatedUid).HasColumnName("UpdatedUID");

            entity.HasOne(d => d.Attendance).WithMany(p => p.PunchTimes)
                .HasForeignKey(d => d.AttendanceId)
                .HasConstraintName("FK__PunchTime__Atten__17F790F9");

        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.ToTable("Status");

            entity.Property(e => e.InsertedUid).HasColumnName("InsertedUID");
            entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
            entity.Property(e => e.StatusName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUid).HasColumnName("UpdatedUID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
