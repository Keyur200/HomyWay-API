﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HomyWayAPI.Models;

public partial class HomyWayContext : DbContext
{
    public HomyWayContext()
    {
    }

    public HomyWayContext(DbContextOptions<HomyWayContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Amenity> Amenities { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<PaymentInfo> PaymentInfos { get; set; }

    public virtual DbSet<PropertyCategoryTbl> PropertyCategoryTbls { get; set; }

    public virtual DbSet<PropertyTbl> PropertyTbls { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Wishlist> Wishlists { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=keyur\\sqlexpress01;Initial Catalog=HomyWay;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Amenity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_amenities");

            entity.ToTable("amenity");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.ToTable("bookings");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.Checkkin).HasColumnName("checkkin");
            entity.Property(e => e.Checkout).HasColumnName("checkout");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.Guests).HasColumnName("guests");
            entity.Property(e => e.HomywayCharges).HasColumnName("homywayCharges");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Nights).HasColumnName("nights");
            entity.Property(e => e.Phone).HasColumnName("phone");
            entity.Property(e => e.PropertyId).HasColumnName("propertyId");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Property).WithMany(p => p.Bookings)
               .HasForeignKey(d => d.PropertyId)
               .HasConstraintName("FK_bookings_propertyTBL");

            entity.HasOne(d => d.User).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_bookings_users");
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.ToTable("groups");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.ToTable("images");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ImageUrl).HasColumnName("imageUrl");
            entity.Property(e => e.PropertId).HasColumnName("propertId");

            entity.HasOne(d => d.Propert).WithMany(p => p.ImagesNavigation)
                .HasForeignKey(d => d.PropertId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_images_images");
        });


        modelBuilder.Entity<PaymentInfo>(entity =>
        {
            entity.ToTable("paymentInfo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BookingId).HasColumnName("bookingId");
            entity.Property(e => e.CreatedDate).HasColumnName("createdDate");
            entity.Property(e => e.PaymentId).HasColumnName("paymentId");
            entity.Property(e => e.PaymentMethod).HasColumnName("paymentMethod");

            entity.HasOne(d => d.Booking).WithMany(p => p.PaymentInfos)
                .HasForeignKey(d => d.BookingId)
                .HasConstraintName("FK_paymentInfo_bookings");
        });

        modelBuilder.Entity<PropertyCategoryTbl>(entity =>
        {
            entity.HasKey(e => e.CategoryId);

            entity.ToTable("property_categoryTBL");

            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("category_name");
        });

        modelBuilder.Entity<PropertyTbl>(entity =>
        {
            entity.HasKey(e => e.PropertyId);

            entity.ToTable("propertyTBL");

            entity.Property(e => e.PropertyId).HasColumnName("property_id");
            entity.Property(e => e.Amenities).HasColumnName("amenities");
            entity.Property(e => e.Bathroom).HasColumnName("bathroom");
            entity.Property(e => e.Bed).HasColumnName("bed");
            entity.Property(e => e.BedRoom).HasColumnName("bed_room");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.HostId).HasColumnName("host_id");
            entity.Property(e => e.Images).HasColumnName("images");
            entity.Property(e => e.Latitude).HasColumnName("latitude");
            entity.Property(e => e.Longitude).HasColumnName("longitude");
            entity.Property(e => e.MaxGuests).HasColumnName("max_guests");
            entity.Property(e => e.PropertyAdderss)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("property_adderss");
            entity.Property(e => e.PropertyCity)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("property_city");
            entity.Property(e => e.PropertyCountry)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("property_country");
            entity.Property(e => e.PropertyDescription)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("property_description");
            entity.Property(e => e.PropertyName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("property_name");
            entity.Property(e => e.PropertyPrice)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("property_price");
            entity.Property(e => e.PropertyState)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("property_state");
            entity.Property(e => e.SlugName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("slugName");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("status");

            entity.HasOne(d => d.Category).WithMany(p => p.PropertyTbls)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_propertyTBL_property_categoryTBL");

            entity.HasOne(d => d.Host).WithMany(p => p.PropertyTbls)
                .HasForeignKey(d => d.HostId)
                .HasConstraintName("FK_propertyTBL_users");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.ToTable("review");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PropertyId).HasColumnName("propertyId");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.Review1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("review1");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Property).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.PropertyId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_review_propertyTBL");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_review_users");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasColumnName("status");

            entity.HasOne(d => d.GidNavigation).WithMany(p => p.Users)
               .HasForeignKey(d => d.Gid)
               .HasConstraintName("FK_users_users");
        });

        modelBuilder.Entity<Wishlist>(entity =>
        {
            entity.ToTable("wishlist");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PropertyId).HasColumnName("propertyId");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Property).WithMany(p => p.Wishlists)
                .HasForeignKey(d => d.PropertyId)
                .HasConstraintName("FK_wishlist_propertyTBL");

            entity.HasOne(d => d.User).WithMany(p => p.Wishlists)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_wishlist_users");
        });
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
