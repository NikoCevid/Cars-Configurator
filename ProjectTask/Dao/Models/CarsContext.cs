using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Dao.Models
{
    public partial class CarsContext : DbContext
    {
        public CarsContext() { }

        public CarsContext(DbContextOptions<CarsContext> options)
            : base(options) { }

        public virtual DbSet<CarComponent> CarComponents { get; set; }
        public virtual DbSet<CarComponentCompatibility> CarComponentCompatibilities { get; set; }
        public virtual DbSet<ComponentType> ComponentTypes { get; set; }
        public virtual DbSet<Configuration> Configurations { get; set; }
        public virtual DbSet<ConfigurationCarComponent> ConfigurationCarComponents { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<UserConfiguration> UserConfigurations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CarComponent>(entity =>
            {
                entity.HasOne(d => d.ComponentType)
                      .WithMany(p => p.CarComponents)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK_CarComponent_ComponentType");
            });

            modelBuilder.Entity<CarComponentCompatibility>(entity =>
            {
                entity.HasOne(d => d.CarComponentId1Navigation)
                      .WithMany(p => p.CarComponentCompatibilityCarComponentId1Navigations)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_CarComponentCompatibility_CarComponent1");

                entity.HasOne(d => d.CarComponentId2Navigation)
                      .WithMany(p => p.CarComponentCompatibilityCarComponentId2Navigations)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_CarComponentCompatibility_CarComponent2");
            });

            modelBuilder.Entity<Configuration>(entity =>
            {
                entity.Property(e => e.CreationDate).HasDefaultValueSql("(getutcdate())");

                entity.HasOne(d => d.User)
                      .WithMany(p => p.Configurations)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK_Configuration_User");
            });

            modelBuilder.Entity<ConfigurationCarComponent>(entity =>
            {
                entity.HasOne(d => d.CarComponent)
                      .WithMany(p => p.ConfigurationCarComponents)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_ConfigurationCarComponent_CarComponent");

                entity.HasOne(d => d.Configuration)
                      .WithMany(p => p.ConfigurationCarComponents)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK_ConfigurationCarComponent_Configuration");
            });

            modelBuilder.Entity<UserConfiguration>(entity =>
            {
                entity.HasOne(d => d.CarComponent)
                      .WithMany(p => p.UserConfigurations)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_UserConfiguration_CarComponent");

                entity.HasOne(d => d.User)
                      .WithMany(p => p.UserConfigurations)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK_UserConfiguration_User");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Role).HasDefaultValue("User");
                entity.Property(e => e.Username).HasDefaultValue("");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
