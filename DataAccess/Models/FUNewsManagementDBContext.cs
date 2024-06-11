﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace DataAccess.Models
{
    public partial class FUNewsManagementDBContext : DbContext
    {
        public FUNewsManagementDBContext()
        {
        }

        public FUNewsManagementDBContext(DbContextOptions<FUNewsManagementDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<NewsArticle> NewsArticles { get; set; } = null!;
        public virtual DbSet<SystemAccount> SystemAccounts { get; set; } = null!;
        public virtual DbSet<Tag> Tags { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                string ConnectionStr = config.GetConnectionString("DB");

                optionsBuilder.UseSqlServer(ConnectionStr);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.CategoryDesciption).HasMaxLength(250);

                entity.Property(e => e.CategoryName).HasMaxLength(100);
            });

            modelBuilder.Entity<NewsArticle>(entity =>
            {
                entity.ToTable("NewsArticle");

                entity.Property(e => e.NewsArticleId)
                    .HasMaxLength(20)
                    .HasColumnName("NewsArticleID");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.NewsContent).HasMaxLength(4000);

                entity.Property(e => e.NewsTitle).HasMaxLength(150);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.NewsArticles)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_NewsArticle_Category");

                entity.HasOne(d => d.CreatedBy)
                    .WithMany(p => p.NewsArticles)
                    .HasForeignKey(d => d.CreatedById)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_NewsArticle_SystemAccount");

                entity.HasMany(d => d.Tags)
                    .WithMany(p => p.NewsArticles)
                    .UsingEntity<Dictionary<string, object>>(
                        "NewsTag",
                        l => l.HasOne<Tag>().WithMany().HasForeignKey("TagId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_NewsTag_Tag"),
                        r => r.HasOne<NewsArticle>().WithMany().HasForeignKey("NewsArticleId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_NewsTag_NewsArticle"),
                        j =>
                        {
                            j.HasKey("NewsArticleId", "TagId");

                            j.ToTable("NewsTag");

                            j.IndexerProperty<string>("NewsArticleId").HasMaxLength(20).HasColumnName("NewsArticleID");

                            j.IndexerProperty<int>("TagId").HasColumnName("TagID");
                        });
            });

            modelBuilder.Entity<SystemAccount>(entity =>
            {
                entity.HasKey(e => e.AccountId);

                entity.ToTable("SystemAccount");

                entity.Property(e => e.AccountId)
                    .ValueGeneratedNever()
                    .HasColumnName("AccountID");

                entity.Property(e => e.AccountEmail).HasMaxLength(70);

                entity.Property(e => e.AccountName).HasMaxLength(100);

                entity.Property(e => e.AccountPassword).HasMaxLength(70);
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.ToTable("Tag");

                entity.Property(e => e.TagId)
                    .ValueGeneratedNever()
                    .HasColumnName("TagID");

                entity.Property(e => e.Note).HasMaxLength(400);

                entity.Property(e => e.TagName).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
