using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace noter.Migrations
{
    public partial class NoteDeleteCascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Note_NoteId",
                table: "Comments");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Note_NoteId",
                table: "Comments",
                column: "NoteId",
                principalTable: "Note",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Note_NoteId",
                table: "Comments");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Note_NoteId",
                table: "Comments",
                column: "NoteId",
                principalTable: "Note",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
