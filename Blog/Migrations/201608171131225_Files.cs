namespace Blog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Files : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Files",
                c => new
                    {
                        FileId = c.Int(nullable: false, identity: true),
                        FileName = c.String(maxLength: 255),
                        ContentType = c.String(maxLength: 100),
                        Content = c.Binary(),
                        FileType = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        User_Id = c.String(maxLength: 128),
                        Post_Id = c.Int(),
                    })
                .PrimaryKey(t => t.FileId)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .ForeignKey("dbo.Posts", t => t.Post_Id)
                .Index(t => t.User_Id)
                .Index(t => t.Post_Id);
            
            DropColumn("dbo.Posts", "Image");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Posts", "Image", c => c.Binary());
            DropForeignKey("dbo.Files", "Post_Id", "dbo.Posts");
            DropForeignKey("dbo.Files", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Files", new[] { "Post_Id" });
            DropIndex("dbo.Files", new[] { "User_Id" });
            DropTable("dbo.Files");
        }
    }
}
