using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Cars.Models;

public partial class CarsContext : DbContext
{
    public CarsContext()
    {
    }

    public CarsContext(DbContextOptions<CarsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CarComponent> CarComponents { get; set; }

    public virtual DbSet<CarComponentCompatibility> CarComponentCompatibilities { get; set; }

    public virtual DbSet<ComponentType> ComponentTypes { get; set; }

    public virtual DbSet<Configuration> Configurations { get; set; }

    public virtual DbSet<ConfigurationCarComponent> ConfigurationCarComponents { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserConfiguration> UserConfigurations { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=LAPTOP-F99RDJL0;Database=Cars;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CarComponent>(entity =>
        {
            entity.ToTable("CarComponent");

            entity.Property(e => e.Description).HasMaxLength(1024);
            entity.Property(e => e.ImagePath).HasMaxLength(512);
            entity.Property(e => e.Name).HasMaxLength(256);

            entity.HasOne(d => d.ComponentType).WithMany(p => p.CarComponents)
                .HasForeignKey(d => d.ComponentTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CarComponent_ComponentType");
        });

        modelBuilder.Entity<CarComponentCompatibility>(entity =>
        {
            entity.ToTable("CarComponentCompatibility");

            entity.HasOne(d => d.CarComponentId1Navigation).WithMany(p => p.CarComponentCompatibilityCarComponentId1Navigations)
                .HasForeignKey(d => d.CarComponentId1)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CarComponentCompatibility_CarComponent1");

            entity.HasOne(d => d.CarComponentId2Navigation).WithMany(p => p.CarComponentCompatibilityCarComponentId2Navigations)
                .HasForeignKey(d => d.CarComponentId2)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CarComponentCompatibility_CarComponent2");
        });

        modelBuilder.Entity<ComponentType>(entity =>
        {
            entity.ToTable("ComponentType");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Configuration>(entity =>
        {
            entity.ToTable("Configuration");

            entity.Property(e => e.CreationDate).HasDefaultValueSql("(getutcdate())");

            entity.HasOne(d => d.User).WithMany(p => p.Configurations)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Configuration_User");
        });

        modelBuilder.Entity<ConfigurationCarComponent>(entity =>
        {
            entity.ToTable("ConfigurationCarComponent");

            entity.HasOne(d => d.CarComponent).WithMany(p => p.ConfigurationCarComponents)
                .HasForeignKey(d => d.CarComponentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ConfigurationCarComponent_CarComponent");

            entity.HasOne(d => d.Configuration).WithMany(p => p.ConfigurationCarComponents)
                .HasForeignKey(d => d.ConfigurationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ConfigurationCarComponent_Configuration");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.PwdHash).HasMaxLength(256);
            entity.Property(e => e.PwdSalt).HasMaxLength(256);
            entity.Property(e => e.Role)
                .HasMaxLength(100)
                .HasDefaultValue("User");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasDefaultValue("");
        });

        modelBuilder.Entity<UserConfiguration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserConf__3214EC07FB1CB182");

            entity.ToTable("UserConfiguration");

            entity.HasOne(d => d.CarComponent).WithMany(p => p.UserConfigurations)
                .HasForeignKey(d => d.CarComponentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserConfiguration_CarComponent");

            entity.HasOne(d => d.User).WithMany(p => p.UserConfigurations)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserConfiguration_User");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("UserRole");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Role).HasMaxLength(100);
            entity.Property(e => e.Username).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
