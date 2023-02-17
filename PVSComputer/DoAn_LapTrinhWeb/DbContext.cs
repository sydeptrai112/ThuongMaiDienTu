﻿using System.Data.Entity;
using DoAn_LapTrinhWeb.Migrations;
using DoAn_LapTrinhWeb.Model;
using DoAn_LapTrinhWeb.Models;

namespace DoAn_LapTrinhWeb
{
    public class DbContext : System.Data.Entity.DbContext
    {
        public DbContext()
            : base("Model11")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DbContext, Configuration>());
        }
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Banner> Banners { get; set; }
        public virtual DbSet<Banner_Detail> Banner_Detail { get; set; }
        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<Delivery> Deliveries { get; set; }
        public virtual DbSet<Discount> Discounts { get; set; }
        public virtual DbSet<Feedback> Feedbacks { get; set; }
        public virtual DbSet<Feedback_Image> Feedback_Image { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<Oder_Detail> Oder_Detail { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Product_Image> Product_Images { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<Tags> Tags { get; set; }
        public virtual DbSet<ChildCategory> ChildCategory { get; set; }
        public virtual DbSet<ParentCategory> ParentCategory { get; set; }
        public virtual DbSet<NewsComments> NewsComments { get; set; }
        public virtual DbSet<StickyPost> StickyPosts { get; set; }
        public virtual DbSet<NewsProducts> NewsProducts { get; set; }
        public virtual DbSet<NewsTags> NewsTags { get; set; }
        public virtual DbSet<ParentGenres> ParentGenres { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .Property(e => e.password)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.Role)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.status)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .HasMany(e => e.Feedbacks)
                .WithRequired(e => e.Account)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Account>()
                .HasMany(e => e.Orders)
                .WithRequired(e => e.Account)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Banner>()
                .Property(e => e.status)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Banner>()
                .Property(e => e.create_by)
                .IsUnicode(false);

            modelBuilder.Entity<Banner>()
                .HasMany(e => e.Banner_Detail)
                .WithRequired(e => e.Banner)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Banner_Detail>()
                .Property(e => e.status)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Banner_Detail>()
                .Property(e => e.create_by)
                .IsUnicode(false);

            modelBuilder.Entity<Brand>()
                .Property(e => e.status)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Brand>()
                .Property(e => e.create_by)
                .IsUnicode(false);

            modelBuilder.Entity<Brand>()
                .HasMany(e => e.Products)
                .WithRequired(e => e.Brand)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Delivery>()
                .Property(e => e.price)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Delivery>()
                .Property(e => e.create_by)
                .IsUnicode(false);

            modelBuilder.Entity<Delivery>()
                .Property(e => e.status)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Delivery>()
                .HasMany(e => e.Orders)
                .WithRequired(e => e.Delivery)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Discount>()
                .Property(e => e.discounts_code)
                .IsUnicode(false);

            modelBuilder.Entity<Discount>()
                .Property(e => e.create_by)
                .IsUnicode(false);

            modelBuilder.Entity<Discount>()
                .Property(e => e.status)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Discount>()
                .HasMany(e => e.Products)
                .WithRequired(e => e.Discount)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<News>()
                .HasMany(e => e.NewsTags)
                .WithRequired(e => e.News)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Tags>()
                .HasMany(e => e.NewsTags)
                .WithRequired(e => e.Tags)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<News>()
                .HasMany(e => e.NewsProducts)
                .WithRequired(e => e.News)
                .WillCascadeOnDelete(false);

           modelBuilder.Entity<Product>()
                .HasMany(e => e.NewsProducts)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Feedback>()
                .Property(e => e.rate_star);          

            modelBuilder.Entity<Feedback>()
                .Property(e => e.create_by)
                .IsUnicode(false);

            modelBuilder.Entity<Feedback>()
                .Property(e => e.status)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Feedback>()
                .HasMany(e => e.Feedback_Image)
                .WithRequired(e => e.Feedback)
                .HasForeignKey(e => new {e.feedback_id, e.account_id})
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Feedback_Image>()
                .Property(e => e.image)
                .IsUnicode(false);

            modelBuilder.Entity<Feedback_Image>()
                .Property(e => e.create_by)
                .IsUnicode(false);

            modelBuilder.Entity<Feedback_Image>()
                .Property(e => e.status)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Genre>()
                .Property(e => e.status)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Genre>()
                .Property(e => e.create_by)
                .IsUnicode(false);

            modelBuilder.Entity<Genre>()
                .HasMany(e => e.Products)
                .WithRequired(e => e.Genre)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<News>()
                .HasMany(e => e.NewsComments)
                .WithRequired(e => e.News)
                .WillCascadeOnDelete(false);

           modelBuilder.Entity<News>()
                .HasMany(e => e.StickyPosts)
                .WithRequired(e => e.News)
                .WillCascadeOnDelete(false);

          modelBuilder.Entity<ParentGenres>()
                .HasMany(e => e.Genres)
                .WithRequired(e => e.ParentGenres)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Oder_Detail>()
                .Property(e => e.status)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Oder_Detail>()
                .Property(e => e.create_by)
                .IsUnicode(false);

            modelBuilder.Entity<Order>()
                .Property(e => e.status)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Order>()
                .Property(e => e.create_by)
                .IsUnicode(false);

            modelBuilder.Entity<Order>()
                .HasMany(e => e.Oder_Detail)
                .WithRequired(e => e.Order)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Payment>()
                .Property(e => e.create_by)
                .IsUnicode(false);

            modelBuilder.Entity<Payment>()
                .Property(e => e.status)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Payment>()
                .HasMany(e => e.Orders)
                .WithRequired(e => e.Payment)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.price);

            modelBuilder.Entity<Product>()
                .Property(e => e.quantity)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.status)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.create_by)
                .IsUnicode(false);

           modelBuilder.Entity<ParentCategory>()
                .HasMany(e => e.ChildCategory)
                .WithRequired(e => e.ParentCategory)
                .HasForeignKey(e => new {e.parentcategory_id })
                .WillCascadeOnDelete(false);

           modelBuilder.Entity<ChildCategory>()
                .HasMany(e => e.News)
                .WithRequired(e => e.ChildCategory)
                .HasForeignKey(e => new {e.childcategory_id })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.Banner_Detail)
                .WithRequired(e => e.Product)
                .HasForeignKey(e => new {e.product_id, e.genre_id, e.disscount_id})
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.Feedbacks)
                .WithRequired(e => e.Product)
                .HasForeignKey(e => new {e.product_id, e.genre_id, e.disscount_id})
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.Oder_Detail)
                .WithRequired(e => e.Product)
                .HasForeignKey(e => new {e.product_id, e.genre_id, e.disscount_id})
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.Product_Images)
                .WithRequired(e => e.Product)
                .HasForeignKey(e => new {e.product_id, e.genre_id, e.discount_id})
                .WillCascadeOnDelete(false);
        }
    }
}