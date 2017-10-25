using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace noter.Migrations
{
    public partial class ManyToMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Note_Tag_TagId",
                table: "Note");

            migrationBuilder.DropIndex(
                name: "IX_Note_TagId",
                table: "Note");

            migrationBuilder.AlterColumn<long>(
                name: "TagId",
                table: "Note",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.CreateTable(
                name: "NoteTag",
                columns: table => new
                {
                    NoteId = table.Column<long>(type: "bigint", nullable: false),
                    TagId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteTag", x => new { x.NoteId, x.TagId });
                    table.ForeignKey(
                        name: "FK_NoteTag_Note_NoteId",
                        column: x => x.NoteId,
                        principalTable: "Note",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NoteTag_Tag_TagId",
                        column: x => x.TagId,
                        principalTable: "Tag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NoteTag_TagId",
                table: "NoteTag",
                column: "TagId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NoteTag");

            migrationBuilder.AlterColumn<long>(
                name: "TagId",
                table: "Note",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Note_TagId",
                table: "Note",
                column: "TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_Note_Tag_TagId",
                table: "Note",
                column: "TagId",
                principalTable: "Tag",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
