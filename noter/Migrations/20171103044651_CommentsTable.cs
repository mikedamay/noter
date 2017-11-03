using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace noter.Migrations
{
    public partial class CommentsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentSet_Note_NoteId",
                table: "CommentSet");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommentSet",
                table: "CommentSet");

            migrationBuilder.RenameTable(
                name: "CommentSet",
                newName: "Comments");

            migrationBuilder.RenameIndex(
                name: "IX_CommentSet_NoteId",
                table: "Comments",
                newName: "IX_Comments_NoteId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comments",
                table: "Comments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Note_NoteId",
                table: "Comments",
                column: "NoteId",
                principalTable: "Note",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Note_NoteId",
                table: "Comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comments",
                table: "Comments");

            migrationBuilder.RenameTable(
                name: "Comments",
                newName: "CommentSet");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_NoteId",
                table: "CommentSet",
                newName: "IX_CommentSet_NoteId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommentSet",
                table: "CommentSet",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentSet_Note_NoteId",
                table: "CommentSet",
                column: "NoteId",
                principalTable: "Note",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
