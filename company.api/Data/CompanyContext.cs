using System;
using System.Collections.Generic;
using company.api.Models;
using Microsoft.EntityFrameworkCore;

namespace company.api.Data;

public partial class CompanyContext : DbContext
{
    public CompanyContext()
    {
    }

    public CompanyContext(DbContextOptions<CompanyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Asset> Assets { get; set; }

    public virtual DbSet<AssetType> AssetTypes { get; set; }

    public virtual DbSet<AttendanceLog> AttendanceLogs { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeeAsset> EmployeeAssets { get; set; }

    public virtual DbSet<Fingerprint> Fingerprints { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<ReportType> ReportTypes { get; set; }

    public virtual DbSet<User> Users { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Asset>(entity =>
        {
            entity.Property(e => e.Status).HasDefaultValue("InStock");

            entity.HasOne(d => d.AssetType).WithMany(p => p.Assets).HasConstraintName("FK_Assets_AssetTypes");
        });

        modelBuilder.Entity<AttendanceLog>(entity =>
        {
            entity.Property(e => e.EventTime).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.Employee).WithMany(p => p.AttendanceLogs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AttendanceLogs_Employees");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.Property(e => e.HireDate).HasDefaultValueSql("(CONVERT([date],getdate()))");
            entity.Property(e => e.Status).HasDefaultValue("Active");

            entity.HasOne(d => d.Department).WithMany(p => p.Employees).HasConstraintName("FK_Employees_Departments");
        });

        modelBuilder.Entity<EmployeeAsset>(entity =>
        {
            entity.Property(e => e.AssignedDate).HasDefaultValueSql("(CONVERT([date],getdate()))");

            entity.HasOne(d => d.Asset).WithMany(p => p.EmployeeAssets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeeAssets_Assets");

            entity.HasOne(d => d.Employee).WithMany(p => p.EmployeeAssets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeeAssets_Employees");
        });

        modelBuilder.Entity<Fingerprint>(entity =>
        {
            entity.Property(e => e.EnrolledDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Employee).WithMany(p => p.Fingerprints)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Fingerprints_Employees");
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.Property(e => e.GeneratedDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.GeneratedBy).WithMany(p => p.ReportGeneratedBies).HasConstraintName("FK_Reports_GeneratedBy");

            entity.HasOne(d => d.RelatedAsset).WithMany(p => p.Reports).HasConstraintName("FK_Reports_RelatedAsset");

            entity.HasOne(d => d.RelatedEmployee).WithMany(p => p.ReportRelatedEmployees).HasConstraintName("FK_Reports_RelatedEmployee");

            entity.HasOne(d => d.ReportType).WithMany(p => p.Reports).HasConstraintName("FK_Reports_ReportTypes");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.Username, "UQ_Users_Username").IsUnique();

            entity.Property(e => e.Role).HasDefaultValue("User");

            entity.HasOne(d => d.Employee)
                .WithMany()
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK_Users_Employees");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
