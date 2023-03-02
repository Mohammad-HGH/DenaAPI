﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable


using DenaAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace DenaAPI
{
    public partial class DenaDbContext : DbContext
    {
        public DenaDbContext()
        {
        }

        public DenaDbContext(DbContextOptions<DenaDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

        public virtual DbSet<User> Users { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Intermediate> Intermediate { get; set; }
        public DbSet<Entities.Attribute> Attributes { get; set; }
        public DbSet<Product> yProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RefreshToken>(entity =>
            {

                entity.Property(e => e.ExpiryDate).HasColumnType("smalldatetime");

                entity.Property(e => e.TokenHash)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.TokenSalt)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.Ts)
                    .HasColumnType("smalldatetime")
                    .HasColumnName("TS");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.RefreshTokens)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RefreshToken_User");
                entity.ToTable("RefreshToken");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasOne(d => d.Intermediate)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.IntermediateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_Intermediate");
                entity.ToTable("Product");
            });


            modelBuilder.Entity<Entities.Attribute>(entity =>
            {
                entity.HasOne(d => d.Intermediate)
                    .WithMany(p => p.Attributes)
                    .HasForeignKey(d => d.IntermediateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Attribute_Intermediate");
                entity.ToTable("Attribute");
            });


            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.PasswordSalt)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Ts)
                    .HasColumnType("smalldatetime")
                    .HasColumnName("TS");

                entity.ToTable("User");

            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}