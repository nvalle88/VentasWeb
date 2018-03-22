namespace WebVentas.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class camposnuevosEstado : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "Estado", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "Estado", c => c.Boolean(nullable: false));
        }
    }
}
