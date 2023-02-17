namespace DoAn_LapTrinhWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class model1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Account",
                c => new
                    {
                        account_id = c.Int(nullable: false, identity: true),
                        password = c.String(nullable: false, maxLength: 100, unicode: false),
                        Email = c.String(nullable: false, maxLength: 100),
                        Requestcode = c.String(maxLength: 100),
                        Role = c.String(nullable: false, maxLength: 1, fixedLength: true, unicode: false),
                        Name = c.String(nullable: false, maxLength: 20),
                        Phone = c.String(nullable: false, maxLength: 14),
                        Avatar = c.String(maxLength: 500),
                        Addres_1 = c.String(nullable: false, maxLength: 100),
                        Dateofbirth = c.DateTime(nullable: false),
                        Gender = c.String(nullable: false, maxLength: 1),
                        create_by = c.String(maxLength: 100),
                        create_at = c.DateTime(nullable: false),
                        update_by = c.String(maxLength: 100),
                        update_at = c.DateTime(nullable: false),
                        status = c.String(maxLength: 1, fixedLength: true, unicode: false),
                    })
                .PrimaryKey(t => t.account_id);
            
            CreateTable(
                "dbo.Feedback",
                c => new
                    {
                        feedback_id = c.Int(nullable: false, identity: true),
                        account_id = c.Int(nullable: false),
                        product_id = c.Int(nullable: false),
                        genre_id = c.Int(nullable: false),
                        disscount_id = c.Int(nullable: false),
                        description = c.String(maxLength: 200),
                        rate_star = c.Int(nullable: false),
                        create_at = c.DateTime(nullable: false),
                        create_by = c.String(nullable: false, maxLength: 100, unicode: false),
                        status = c.String(maxLength: 1, fixedLength: true, unicode: false),
                        update_by = c.String(nullable: false, maxLength: 100),
                        update_at = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.feedback_id, t.account_id })
                .ForeignKey("dbo.Product", t => new { t.product_id, t.genre_id, t.disscount_id })
                .ForeignKey("dbo.Account", t => t.account_id)
                .Index(t => t.account_id)
                .Index(t => new { t.product_id, t.genre_id, t.disscount_id });
            
            CreateTable(
                "dbo.Feedback_Image",
                c => new
                    {
                        image_id = c.Int(nullable: false, identity: true),
                        feedback_id = c.Int(nullable: false),
                        account_id = c.Int(nullable: false),
                        image = c.String(unicode: false, storeType: "text"),
                        create_at = c.DateTime(nullable: false),
                        create_by = c.String(nullable: false, maxLength: 20, unicode: false),
                        status = c.String(maxLength: 1, fixedLength: true, unicode: false),
                        update_by = c.String(nullable: false, maxLength: 20),
                        update_at = c.DateTime(),
                    })
                .PrimaryKey(t => t.image_id)
                .ForeignKey("dbo.Feedback", t => new { t.feedback_id, t.account_id })
                .Index(t => new { t.feedback_id, t.account_id });
            
            CreateTable(
                "dbo.Product",
                c => new
                    {
                        product_id = c.Int(nullable: false, identity: true),
                        genre_id = c.Int(nullable: false),
                        disscount_id = c.Int(nullable: false),
                        brand_id = c.Int(nullable: false),
                        product_name = c.String(nullable: false, maxLength: 200),
                        title_seo = c.String(nullable: false, maxLength: 150),
                        slug = c.String(maxLength: 159),
                        price = c.Double(nullable: false),
                        view = c.Long(nullable: false),
                        buyturn = c.Long(nullable: false),
                        quantity = c.String(nullable: false, maxLength: 10, fixedLength: true, unicode: false),
                        status = c.String(maxLength: 1, fixedLength: true, unicode: false),
                        create_by = c.String(nullable: false, maxLength: 100, unicode: false),
                        create_at = c.DateTime(nullable: false),
                        updateby = c.String(maxLength: 100),
                        update_at = c.DateTime(nullable: false),
                        image = c.String(maxLength: 500),
                        description = c.String(),
                        specification = c.String(),
                    })
                .PrimaryKey(t => new { t.product_id, t.genre_id, t.disscount_id })
                .ForeignKey("dbo.Brand", t => t.brand_id)
                .ForeignKey("dbo.Discount", t => t.disscount_id)
                .ForeignKey("dbo.Genre", t => t.genre_id)
                .Index(t => t.genre_id)
                .Index(t => t.disscount_id)
                .Index(t => t.brand_id);
            
            CreateTable(
                "dbo.Banner_Detail",
                c => new
                    {
                        banner_id = c.Int(nullable: false),
                        product_id = c.Int(nullable: false),
                        genre_id = c.Int(nullable: false),
                        disscount_id = c.Int(nullable: false),
                        id = c.Int(nullable: false, identity: true),
                        status = c.String(maxLength: 1, fixedLength: true, unicode: false),
                        create_by = c.String(nullable: false, maxLength: 100, unicode: false),
                        create_at = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.banner_id, t.product_id, t.genre_id, t.disscount_id })
                .ForeignKey("dbo.Banner", t => t.banner_id)
                .ForeignKey("dbo.Product", t => new { t.product_id, t.genre_id, t.disscount_id })
                .Index(t => t.banner_id)
                .Index(t => new { t.product_id, t.genre_id, t.disscount_id });
            
            CreateTable(
                "dbo.Banner",
                c => new
                    {
                        banner_id = c.Int(nullable: false, identity: true),
                        banner_name = c.String(nullable: false, maxLength: 200),
                        slug = c.String(maxLength: 209),
                        banner_start = c.DateTime(nullable: false),
                        banner_end = c.DateTime(nullable: false),
                        description = c.String(maxLength: 100),
                        image_thumbnail = c.String(nullable: false, maxLength: 500),
                        status = c.String(maxLength: 1, fixedLength: true, unicode: false),
                        create_by = c.String(nullable: false, maxLength: 100, unicode: false),
                        create_at = c.DateTime(nullable: false),
                        banner_type = c.Int(nullable: false),
                        update_by = c.String(nullable: false, maxLength: 100),
                        update_at = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.banner_id);
            
            CreateTable(
                "dbo.Brand",
                c => new
                    {
                        brand_id = c.Int(nullable: false, identity: true),
                        brand_name = c.String(nullable: false, maxLength: 100),
                        brand_image = c.String(maxLength: 500),
                        slug = c.String(maxLength: 109),
                        description = c.String(maxLength: 200),
                        Web_directory = c.String(maxLength: 200),
                        status = c.String(maxLength: 1, fixedLength: true, unicode: false),
                        create_by = c.String(nullable: false, maxLength: 100, unicode: false),
                        create_at = c.DateTime(nullable: false),
                        update_by = c.String(nullable: false, maxLength: 100),
                        update_at = c.DateTime(),
                    })
                .PrimaryKey(t => t.brand_id);
            
            CreateTable(
                "dbo.Discount",
                c => new
                    {
                        disscount_id = c.Int(nullable: false, identity: true),
                        discount_name = c.String(nullable: false, maxLength: 200),
                        discount_start = c.DateTime(nullable: false),
                        discount_end = c.DateTime(nullable: false),
                        discount_price = c.Double(nullable: false),
                        discounts_code = c.String(maxLength: 10, unicode: false),
                        discounts_type = c.Int(nullable: false),
                        create_at = c.DateTime(nullable: false),
                        create_by = c.String(nullable: false, maxLength: 100, unicode: false),
                        status = c.String(maxLength: 1, fixedLength: true, unicode: false),
                        quantity = c.String(maxLength: 10),
                        update_by = c.String(nullable: false, maxLength: 100),
                        update_at = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.disscount_id);
            
            CreateTable(
                "dbo.Genre",
                c => new
                    {
                        genre_id = c.Int(nullable: false, identity: true),
                        parent_genre_id = c.Int(nullable: false),
                        genre_name = c.String(nullable: false, maxLength: 100),
                        slug = c.String(maxLength: 109),
                        genre_image = c.String(maxLength: 500),
                        description = c.String(maxLength: 200),
                        status = c.String(maxLength: 1, fixedLength: true, unicode: false),
                        create_at = c.DateTime(nullable: false),
                        create_by = c.String(nullable: false, maxLength: 100, unicode: false),
                        update_by = c.String(nullable: false, maxLength: 100),
                        update_at = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.genre_id)
                .ForeignKey("dbo.ParentGenres", t => t.parent_genre_id)
                .Index(t => t.parent_genre_id);
            
            CreateTable(
                "dbo.ParentGenres",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 150),
                        slug = c.String(maxLength: 159),
                        icon = c.String(maxLength: 20),
                        image = c.String(maxLength: 500),
                        description = c.String(maxLength: 200),
                        status = c.String(maxLength: 1),
                        create_at = c.DateTime(nullable: false),
                        create_by = c.String(nullable: false, maxLength: 100),
                        update_by = c.String(nullable: false, maxLength: 100),
                        update_at = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.NewsProducts",
                c => new
                    {
                        news_id = c.Int(nullable: false),
                        product_id = c.Int(nullable: false),
                        genre_id = c.Int(nullable: false),
                        disscount_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.news_id, t.product_id })
                .ForeignKey("dbo.News", t => t.news_id)
                .ForeignKey("dbo.Product", t => new { t.product_id, t.genre_id, t.disscount_id })
                .Index(t => t.news_id)
                .Index(t => new { t.product_id, t.genre_id, t.disscount_id });
            
            CreateTable(
                "dbo.News",
                c => new
                    {
                        news_id = c.Int(nullable: false, identity: true),
                        account_id = c.Int(nullable: false),
                        childcategory_id = c.Int(nullable: false),
                        news_title = c.String(nullable: false, maxLength: 500),
                        meta_title = c.String(nullable: false, maxLength: 150),
                        slug = c.String(nullable: false, maxLength: 159),
                        news_content = c.String(nullable: false),
                        ViewCount = c.Int(nullable: false),
                        image = c.String(nullable: false, maxLength: 500),
                        image2 = c.String(nullable: false, maxLength: 500),
                        create_at = c.DateTime(nullable: false),
                        update_at = c.DateTime(nullable: false),
                        update_by = c.String(maxLength: 100),
                        status = c.String(maxLength: 1),
                    })
                .PrimaryKey(t => t.news_id)
                .ForeignKey("dbo.Account", t => t.account_id, cascadeDelete: true)
                .ForeignKey("dbo.ChildCategory", t => t.childcategory_id)
                .Index(t => t.account_id)
                .Index(t => t.childcategory_id);
            
            CreateTable(
                "dbo.ChildCategory",
                c => new
                    {
                        childcategory_id = c.Int(nullable: false, identity: true),
                        parentcategory_id = c.Int(nullable: false),
                        name = c.String(nullable: false, maxLength: 100),
                        slug = c.String(nullable: false, maxLength: 150),
                        image = c.String(nullable: false, maxLength: 500),
                        image2 = c.String(nullable: false, maxLength: 500),
                        description = c.String(maxLength: 100),
                        create_at = c.DateTime(nullable: false),
                        update_at = c.DateTime(nullable: false),
                        create_by = c.String(maxLength: 100),
                        update_by = c.String(maxLength: 100),
                        status = c.String(maxLength: 1),
                    })
                .PrimaryKey(t => t.childcategory_id)
                .ForeignKey("dbo.ParentCategory", t => t.parentcategory_id)
                .Index(t => t.parentcategory_id);
            
            CreateTable(
                "dbo.ParentCategory",
                c => new
                    {
                        parentcategory_id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 100),
                        slug = c.String(nullable: false, maxLength: 109),
                        image = c.String(nullable: false, maxLength: 500),
                        image2 = c.String(nullable: false, maxLength: 500),
                        category_description = c.String(maxLength: 100),
                        create_at = c.DateTime(nullable: false),
                        update_at = c.DateTime(nullable: false),
                        create_by = c.String(maxLength: 100),
                        update_by = c.String(maxLength: 100),
                        status = c.String(maxLength: 1),
                    })
                .PrimaryKey(t => t.parentcategory_id);
            
            CreateTable(
                "dbo.NewsComments",
                c => new
                    {
                        comment_id = c.Int(nullable: false, identity: true),
                        account_id = c.Int(nullable: false),
                        news_id = c.Int(nullable: false),
                        comment_content = c.String(maxLength: 200),
                        create_at = c.DateTime(nullable: false),
                        update_at = c.DateTime(nullable: false),
                        status = c.String(maxLength: 1),
                    })
                .PrimaryKey(t => t.comment_id)
                .ForeignKey("dbo.Account", t => t.account_id, cascadeDelete: true)
                .ForeignKey("dbo.News", t => t.news_id)
                .Index(t => t.account_id)
                .Index(t => t.news_id);
            
            CreateTable(
                "dbo.NewsTags",
                c => new
                    {
                        news_id = c.Int(nullable: false),
                        tag_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.news_id, t.tag_id })
                .ForeignKey("dbo.Tags", t => t.tag_id)
                .ForeignKey("dbo.News", t => t.news_id)
                .Index(t => t.news_id)
                .Index(t => t.tag_id);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        tag_id = c.Int(nullable: false, identity: true),
                        tag_name = c.String(nullable: false, maxLength: 100),
                        slug = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.tag_id);
            
            CreateTable(
                "dbo.StickyPost",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        news_id = c.Int(nullable: false),
                        priority = c.Int(nullable: false),
                        create_at = c.DateTime(nullable: false),
                        create_by = c.String(nullable: false, maxLength: 100),
                        update_by = c.String(nullable: false, maxLength: 100),
                        update_at = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.News", t => t.news_id)
                .Index(t => t.news_id);
            
            CreateTable(
                "dbo.Oder_Detail",
                c => new
                    {
                        product_id = c.Int(nullable: false),
                        genre_id = c.Int(nullable: false),
                        disscount_id = c.Int(nullable: false),
                        order_id = c.Int(nullable: false),
                        price = c.Double(nullable: false),
                        status = c.String(maxLength: 1, fixedLength: true, unicode: false),
                        transection = c.String(nullable: false, maxLength: 50),
                        quantity = c.Int(nullable: false),
                        create_by = c.String(nullable: false, maxLength: 100, unicode: false),
                        create_at = c.DateTime(nullable: false),
                        update_by = c.String(nullable: false, maxLength: 100),
                        update_at = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.product_id, t.genre_id, t.disscount_id, t.order_id })
                .ForeignKey("dbo.Order", t => t.order_id)
                .ForeignKey("dbo.Product", t => new { t.product_id, t.genre_id, t.disscount_id })
                .Index(t => new { t.product_id, t.genre_id, t.disscount_id })
                .Index(t => t.order_id);
            
            CreateTable(
                "dbo.Order",
                c => new
                    {
                        order_id = c.Int(nullable: false, identity: true),
                        payment_id = c.Int(nullable: false),
                        delivery_id = c.Int(nullable: false),
                        oder_date = c.DateTime(nullable: false),
                        account_id = c.Int(nullable: false),
                        status = c.String(maxLength: 1, fixedLength: true, unicode: false),
                        order_note = c.String(maxLength: 200),
                        create_at = c.DateTime(nullable: false),
                        total = c.Double(nullable: false),
                        create_by = c.String(nullable: false, maxLength: 100, unicode: false),
                        update_by = c.String(nullable: false, maxLength: 100),
                        update_at = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.order_id)
                .ForeignKey("dbo.Delivery", t => t.delivery_id)
                .ForeignKey("dbo.Payment", t => t.payment_id)
                .ForeignKey("dbo.Account", t => t.account_id)
                .Index(t => t.payment_id)
                .Index(t => t.delivery_id)
                .Index(t => t.account_id);
            
            CreateTable(
                "dbo.Delivery",
                c => new
                    {
                        delivery_id = c.Int(nullable: false, identity: true),
                        delivery_name = c.String(nullable: false, maxLength: 100),
                        price = c.Decimal(nullable: false, storeType: "money"),
                        create_at = c.DateTime(nullable: false),
                        create_by = c.String(nullable: false, maxLength: 100, unicode: false),
                        status = c.String(maxLength: 1, fixedLength: true, unicode: false),
                        update_by = c.String(nullable: false, maxLength: 100),
                        update_at = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.delivery_id);
            
            CreateTable(
                "dbo.Payment",
                c => new
                    {
                        payment_id = c.Int(nullable: false, identity: true),
                        payment_name = c.String(nullable: false, maxLength: 100),
                        create_at = c.DateTime(nullable: false),
                        create_by = c.String(nullable: false, maxLength: 100, unicode: false),
                        status = c.String(maxLength: 1, fixedLength: true, unicode: false),
                        update_by = c.String(nullable: false, maxLength: 100),
                        update_at = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.payment_id);
            
            CreateTable(
                "dbo.Product_Image",
                c => new
                    {
                        product_image_id = c.Int(nullable: false, identity: true),
                        product_id = c.Int(nullable: false),
                        genre_id = c.Int(nullable: false),
                        discount_id = c.Int(nullable: false),
                        image_1 = c.String(maxLength: 500),
                        image_2 = c.String(maxLength: 500),
                        image_3 = c.String(maxLength: 500),
                        image_4 = c.String(maxLength: 500),
                        image_5 = c.String(maxLength: 500),
                        status = c.String(maxLength: 1),
                        create_by = c.String(maxLength: 100),
                        update_by = c.String(maxLength: 100),
                        create_at = c.DateTime(nullable: false),
                        update_at = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.product_image_id)
                .ForeignKey("dbo.Product", t => new { t.product_id, t.genre_id, t.discount_id })
                .Index(t => new { t.product_id, t.genre_id, t.discount_id });
            
            CreateTable(
                "dbo.Contact",
                c => new
                    {
                        contact_id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 50),
                        phone = c.String(nullable: false, maxLength: 14),
                        email = c.String(nullable: false, maxLength: 100),
                        content = c.String(nullable: false, maxLength: 200),
                        image = c.String(maxLength: 500),
                        reply = c.String(maxLength: 500),
                        flag = c.Int(nullable: false),
                        status = c.String(nullable: false, maxLength: 1),
                        create_by = c.String(nullable: false, maxLength: 100),
                        create_at = c.DateTime(nullable: false),
                        update_by = c.String(nullable: false, maxLength: 100),
                        update_at = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.contact_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Order", "account_id", "dbo.Account");
            DropForeignKey("dbo.Feedback", "account_id", "dbo.Account");
            DropForeignKey("dbo.Product_Image", new[] { "product_id", "genre_id", "discount_id" }, "dbo.Product");
            DropForeignKey("dbo.Oder_Detail", new[] { "product_id", "genre_id", "disscount_id" }, "dbo.Product");
            DropForeignKey("dbo.Order", "payment_id", "dbo.Payment");
            DropForeignKey("dbo.Oder_Detail", "order_id", "dbo.Order");
            DropForeignKey("dbo.Order", "delivery_id", "dbo.Delivery");
            DropForeignKey("dbo.NewsProducts", new[] { "product_id", "genre_id", "disscount_id" }, "dbo.Product");
            DropForeignKey("dbo.StickyPost", "news_id", "dbo.News");
            DropForeignKey("dbo.NewsTags", "news_id", "dbo.News");
            DropForeignKey("dbo.NewsTags", "tag_id", "dbo.Tags");
            DropForeignKey("dbo.NewsProducts", "news_id", "dbo.News");
            DropForeignKey("dbo.NewsComments", "news_id", "dbo.News");
            DropForeignKey("dbo.NewsComments", "account_id", "dbo.Account");
            DropForeignKey("dbo.ChildCategory", "parentcategory_id", "dbo.ParentCategory");
            DropForeignKey("dbo.News", "childcategory_id", "dbo.ChildCategory");
            DropForeignKey("dbo.News", "account_id", "dbo.Account");
            DropForeignKey("dbo.Product", "genre_id", "dbo.Genre");
            DropForeignKey("dbo.Genre", "parent_genre_id", "dbo.ParentGenres");
            DropForeignKey("dbo.Feedback", new[] { "product_id", "genre_id", "disscount_id" }, "dbo.Product");
            DropForeignKey("dbo.Product", "disscount_id", "dbo.Discount");
            DropForeignKey("dbo.Product", "brand_id", "dbo.Brand");
            DropForeignKey("dbo.Banner_Detail", new[] { "product_id", "genre_id", "disscount_id" }, "dbo.Product");
            DropForeignKey("dbo.Banner_Detail", "banner_id", "dbo.Banner");
            DropForeignKey("dbo.Feedback_Image", new[] { "feedback_id", "account_id" }, "dbo.Feedback");
            DropIndex("dbo.Product_Image", new[] { "product_id", "genre_id", "discount_id" });
            DropIndex("dbo.Order", new[] { "account_id" });
            DropIndex("dbo.Order", new[] { "delivery_id" });
            DropIndex("dbo.Order", new[] { "payment_id" });
            DropIndex("dbo.Oder_Detail", new[] { "order_id" });
            DropIndex("dbo.Oder_Detail", new[] { "product_id", "genre_id", "disscount_id" });
            DropIndex("dbo.StickyPost", new[] { "news_id" });
            DropIndex("dbo.NewsTags", new[] { "tag_id" });
            DropIndex("dbo.NewsTags", new[] { "news_id" });
            DropIndex("dbo.NewsComments", new[] { "news_id" });
            DropIndex("dbo.NewsComments", new[] { "account_id" });
            DropIndex("dbo.ChildCategory", new[] { "parentcategory_id" });
            DropIndex("dbo.News", new[] { "childcategory_id" });
            DropIndex("dbo.News", new[] { "account_id" });
            DropIndex("dbo.NewsProducts", new[] { "product_id", "genre_id", "disscount_id" });
            DropIndex("dbo.NewsProducts", new[] { "news_id" });
            DropIndex("dbo.Genre", new[] { "parent_genre_id" });
            DropIndex("dbo.Banner_Detail", new[] { "product_id", "genre_id", "disscount_id" });
            DropIndex("dbo.Banner_Detail", new[] { "banner_id" });
            DropIndex("dbo.Product", new[] { "brand_id" });
            DropIndex("dbo.Product", new[] { "disscount_id" });
            DropIndex("dbo.Product", new[] { "genre_id" });
            DropIndex("dbo.Feedback_Image", new[] { "feedback_id", "account_id" });
            DropIndex("dbo.Feedback", new[] { "product_id", "genre_id", "disscount_id" });
            DropIndex("dbo.Feedback", new[] { "account_id" });
            DropTable("dbo.Contact");
            DropTable("dbo.Product_Image");
            DropTable("dbo.Payment");
            DropTable("dbo.Delivery");
            DropTable("dbo.Order");
            DropTable("dbo.Oder_Detail");
            DropTable("dbo.StickyPost");
            DropTable("dbo.Tags");
            DropTable("dbo.NewsTags");
            DropTable("dbo.NewsComments");
            DropTable("dbo.ParentCategory");
            DropTable("dbo.ChildCategory");
            DropTable("dbo.News");
            DropTable("dbo.NewsProducts");
            DropTable("dbo.ParentGenres");
            DropTable("dbo.Genre");
            DropTable("dbo.Discount");
            DropTable("dbo.Brand");
            DropTable("dbo.Banner");
            DropTable("dbo.Banner_Detail");
            DropTable("dbo.Product");
            DropTable("dbo.Feedback_Image");
            DropTable("dbo.Feedback");
            DropTable("dbo.Account");
        }
    }
}
