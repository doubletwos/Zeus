namespace Zeus.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2027 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StudentSubjects", "test", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.StudentSubjects", "test");
        }
    }
}