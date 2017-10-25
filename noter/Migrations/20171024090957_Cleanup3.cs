using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace noter.Migrations
{
    public partial class Cleanup3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tag_Note_NoteId",
                table: "Tag");

            migrationBuilder.DropIndex(
                name: "IX_Tag_NoteId",
                table: "Tag");

            migrationBuilder.DropColumn(
                name: "NoteId",
                table: "Tag");

            migrationBuilder.AddColumn<long>(
                name: "TagId",
                table: "Note",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Note_Tag_TagId",
                table: "Note");

            migrationBuilder.DropIndex(
                name: "IX_Note_TagId",
                table: "Note");

            migrationBuilder.DropColumn(
                name: "TagId",
                table: "Note");

            migrationBuilder.AddColumn<long>(
                name: "NoteId",
                table: "Tag",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tag_NoteId",
                table: "Tag",
                column: "NoteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tag_Note_NoteId",
                table: "Tag",
                column: "NoteId",
                principalTable: "Note",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
