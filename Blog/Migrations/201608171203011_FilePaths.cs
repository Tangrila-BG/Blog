namespace Blog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FilePaths : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Files", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Files", "Post_Id", "dbo.Posts");
            DropIndex("dbo.Files", new[] { "User_Id" });
            DropIndex("dbo.Files", new[] { "Post_Id" });
            RenameColumn(table: "dbo.Files", name: "Post_Id", newName: "PostId");
            CreateTable(
                "dbo.FilePaths",
                c => new
                    {
                        FilePathId = c.Int(nullable: false, identity: true),
                        FileName = c.String(maxLength: 255),
                        FileType = c.Int(nullable: false),
                        PostID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FilePathId)
                .ForeignKey("dbo.Posts", t => t.PostID, cascadeDelete: true)
                .Index(t => t.PostID);
            
            AlterColumn("dbo.Files", "PostId", c => c.Int(nullable: false));
            CreateIndex("dbo.Files", "PostId");
            AddForeignKey("dbo.Files", "PostId", "dbo.Posts", "Id", cascadeDelete: true);
            DropColumn("dbo.Files", "UserId");
            DropColumn("dbo.Files", "User_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Files", "User_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Files", "UserId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Files", "PostId", "dbo.Posts");
            DropForeignKey("dbo.FilePaths", "PostID", "dbo.Posts");
            DropIndex("dbo.Files", new[] { "PostId" });
            DropIndex("dbo.FilePaths", new[] { "PostID" });
            AlterColumn("dbo.Files", "PostId", c => c.Int());
            DropTable("dbo.FilePaths");
            RenameColumn(table: "dbo.Files", name: "PostId", newName: "Post_Id");
            CreateIndex("dbo.Files", "Post_Id");
            CreateIndex("dbo.Files", "User_Id");
            AddForeignKey("dbo.Files", "Post_Id", "dbo.Posts", "Id");
            AddForeignKey("dbo.Files", "User_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
