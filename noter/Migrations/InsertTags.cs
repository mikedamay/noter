using Microsoft.EntityFrameworkCore.Migrations;

namespace noter.Migrations
{
    public class InsertTags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO dbo.Tag(Detaails, Name, ShortDescription) VALUES ('my tabdets', 'mytagname', 'mysortdesc')");
            migrationBuilder.Sql("INSERT INTO dbo.Tag(Detaails, Name, ShortDescription) VALUES ('my tabdets2', 'mytagname2', 'mysortdesc2')");
            migrationBuilder.Sql("INSERT INTO dbo.Tag(Detaails, Name, ShortDescription) VALUES ('my tabdets3', 'mytagname3', 'mysortdesc3')");
        
        }
    }
}