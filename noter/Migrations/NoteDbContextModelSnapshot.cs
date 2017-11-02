﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using noter.Data;
using System;

namespace noter.Migrations
{
    [DbContext(typeof(NoteDbContext))]
    partial class NoteDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("noter.Entities.Another", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Explanation");

                    b.HasKey("Id");

                    b.ToTable("Another");
                });

            modelBuilder.Entity("noter.Entities.Comment", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("NoteId");

                    b.Property<string>("Payload");

                    b.HasKey("Id");

                    b.HasIndex("NoteId");

                    b.ToTable("CommentSet");
                });

            modelBuilder.Entity("noter.Entities.Note", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Payload");

                    b.Property<long?>("TagId");

                    b.Property<long?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Note");
                });

            modelBuilder.Entity("noter.Entities.NoteTag", b =>
                {
                    b.Property<long>("NoteId");

                    b.Property<long>("TagId");

                    b.HasKey("NoteId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("NoteTag");
                });

            modelBuilder.Entity("noter.Entities.Tag", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Details")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<string>("ShortDescription")
                        .IsRequired()
                        .HasMaxLength(80);

                    b.HasKey("Id");

                    b.ToTable("Tag");
                });

            modelBuilder.Entity("noter.Entities.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("noter.Entities.Comment", b =>
                {
                    b.HasOne("noter.Entities.Note")
                        .WithMany("Comments")
                        .HasForeignKey("NoteId");
                });

            modelBuilder.Entity("noter.Entities.Note", b =>
                {
                    b.HasOne("noter.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("noter.Entities.NoteTag", b =>
                {
                    b.HasOne("noter.Entities.Note", "Note")
                        .WithMany("NoteTags")
                        .HasForeignKey("NoteId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("noter.Entities.Tag", "Tag")
                        .WithMany("NoteTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
