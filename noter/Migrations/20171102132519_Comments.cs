using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace noter.Migrations
{
    public partial class Comments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ShortDescription",
                table: "Tag",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 80,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Tag",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Details",
                table: "Tag",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Another",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Explanation = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Another", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CommentSet",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NoteId = table.Column<long>(type: "bigint", nullable: true),
                    Payload = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentSet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommentSet_Note_NoteId",
                        column: x => x.NoteId,
                        principalTable: "Note",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommentSet_NoteId",
                table: "CommentSet",
                column: "NoteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Another");

            migrationBuilder.DropTable(
                name: "CommentSet");

            migrationBuilder.AlterColumn<string>(
                name: "ShortDescription",
                table: "Tag",
                maxLength: 80,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(80)",
                oldMaxLength: 80);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Tag",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Details",
                table: "Tag",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
