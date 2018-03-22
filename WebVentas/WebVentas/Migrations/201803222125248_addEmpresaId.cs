namespace WebVentas.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addEmpresaId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "IdEmpresa", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "IdEmpresa");
        }
    }
}
