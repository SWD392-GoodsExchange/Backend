﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using ExchangeGood.Data.Models;
using Microsoft.EntityFrameworkCore;
namespace ExchangeGood.DAO;

public partial class GoodsExchangeContext : DbContext
{
    public GoodsExchangeContext(DbContextOptions<GoodsExchangeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Bookmark> Bookmarks { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bookmark>(entity =>
        {
            entity.HasKey(e => new { e.ProductId, e.FeId }).HasName("PK__Bookmark__D767AE40027BB560");

            entity.ToTable("Bookmark");

            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.FeId)
                .HasMaxLength(8)
                .HasColumnName("FeID");
            entity.Property(e => e.CreateTime).HasColumnType("datetime");

            entity.HasOne(d => d.Fe).WithMany(p => p.Bookmarks)
                .HasForeignKey(d => d.FeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Bookmark__FeID__571DF1D5");

            entity.HasOne(d => d.Product).WithMany(p => p.Bookmarks)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Bookmark__Produc__5629CD9C");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CateId).HasName("PK__Category__27638D741E77BA20");

            entity.ToTable("Category");

            entity.Property(e => e.CateId).HasColumnName("CateID");
            entity.Property(e => e.CateName)
                .IsRequired()
                .HasMaxLength(100);
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PK__Image__7516F4ECF9759D07");

            entity.ToTable("Image");

            entity.Property(e => e.ImageId).HasColumnName("ImageID");
            entity.Property(e => e.ImageUrl)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.PublicId)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("PublicID");

            entity.HasOne(d => d.Product).WithMany(p => p.Images)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Image__ProductID__4AB81AF0");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.FeId).HasName("PK__Member__36B68AD74915C162");

            entity.ToTable("Member");

            entity.Property(e => e.FeId)
                .HasMaxLength(8)
                .HasColumnName("FeID");
            entity.Property(e => e.Address)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.CreatedTime).HasColumnType("datetime");
            entity.Property(e => e.Dob).HasColumnType("date");
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Gender)
                .IsRequired()
                .HasMaxLength(10);
            entity.Property(e => e.PasswordHash)
                .IsRequired()
                .HasMaxLength(128);
            entity.Property(e => e.PasswordSalt)
                .IsRequired()
                .HasMaxLength(128);
            entity.Property(e => e.Phone)
                .IsRequired()
                .HasMaxLength(15);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.UpdatedTime).HasColumnType("datetime");
            entity.Property(e => e.UserName)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasOne(d => d.Role).WithMany(p => p.Members)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Member__RoleID__3A81B327");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__20CF2E32608B0FD6");

            entity.ToTable("Notification");

            entity.Property(e => e.NotificationId).HasColumnName("NotificationID");
            entity.Property(e => e.Content)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DateRead).HasColumnType("datetime");
            entity.Property(e => e.ExchangerProductIds).HasMaxLength(255);
            entity.Property(e => e.OnwerProductId).HasMaxLength(255);
            entity.Property(e => e.RecipientId)
                .IsRequired()
                .HasMaxLength(8)
                .HasColumnName("RecipientID");
            entity.Property(e => e.RecipientUsername)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.SenderId)
                .IsRequired()
                .HasMaxLength(8)
                .HasColumnName("SenderID");
            entity.Property(e => e.SenderUsername)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Type)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasOne(d => d.Recipient).WithMany(p => p.NotificationRecipients)
                .HasForeignKey(d => d.RecipientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Notificat__Recip__3F466844");

            entity.HasOne(d => d.Sender).WithMany(p => p.NotificationSenders)
                .HasForeignKey(d => d.SenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Notificat__Sende__3E52440B");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Order__C3905BAF5A6ECE92");

            entity.ToTable("Order");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.BuyerId)
                .IsRequired()
                .HasMaxLength(8)
                .HasColumnName("BuyerID");
            entity.Property(e => e.CreatedTime).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Type)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.UpdatedTime).HasColumnType("datetime");

            entity.HasOne(d => d.Buyer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.BuyerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order__BuyerID__4D94879B");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderDetailId).HasName("PK__OrderDet__D3B9D30C56DBEF06");

            entity.ToTable("OrderDetail");

            entity.Property(e => e.OrderDetailId).HasColumnName("OrderDetailID");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.SellerId)
                .IsRequired()
                .HasMaxLength(8)
                .HasColumnName("SellerID");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__Order__52593CB8");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__Produ__534D60F1");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Product__B40CC6ED55451DD4");

            entity.ToTable("Product");

            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.CateId).HasColumnName("CateID");
            entity.Property(e => e.CreatedTime).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.FeId)
                .IsRequired()
                .HasMaxLength(8)
                .HasColumnName("FeID");
            entity.Property(e => e.Origin)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.Type)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.UpdatedTime).HasColumnType("datetime");

            entity.HasOne(d => d.Cate).WithMany(p => p.Products)
                .HasForeignKey(d => d.CateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Product__CateID__45F365D3");

            entity.HasOne(d => d.Fe).WithMany(p => p.Products)
                .HasForeignKey(d => d.FeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Product__FeID__44FF419A");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.RefreshTokenId).HasName("PK__RefreshT__F5845E59E5047C5C");

            entity.ToTable("RefreshToken");

            entity.Property(e => e.RefreshTokenId).HasColumnName("RefreshTokenID");
            entity.Property(e => e.ExpiryDate).HasColumnType("datetime");
            entity.Property(e => e.FeId)
                .IsRequired()
                .HasMaxLength(8)
                .HasColumnName("FeID");
            entity.Property(e => e.Token)
                .IsRequired()
                .HasMaxLength(255);

            entity.HasOne(d => d.Fe).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.FeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RefreshTok__FeID__5EBF139D");
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(e => e.ReportId).HasName("PK__Report__D5BD48E58C24B1DF");

            entity.ToTable("Report");

            entity.Property(e => e.ReportId).HasColumnName("ReportID");
            entity.Property(e => e.CreatedTime).HasColumnType("datetime");
            entity.Property(e => e.FeId)
                .IsRequired()
                .HasMaxLength(8)
                .HasColumnName("FeID");
            entity.Property(e => e.Message)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasOne(d => d.Fe).WithMany(p => p.Reports)
                .HasForeignKey(d => d.FeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Report__FeID__59FA5E80");

            entity.HasOne(d => d.Product).WithMany(p => p.Reports)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Report__ProductI__5AEE82B9");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Role__8AFACE3A3D34A25D");

            entity.ToTable("Role");

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.RoleName)
                .IsRequired()
                .HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}