﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NoteTakingApp.Infrastructure.Data;

namespace NoteTakingApp.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20180626080848_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("NoteTakingApp.Core.Models.AccessToken", b =>
                {
                    b.Property<int>("AccessTokenId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsValid");

                    b.Property<string>("Username");

                    b.Property<string>("Value");

                    b.HasKey("AccessTokenId");

                    b.ToTable("AccessTokens");
                });

            modelBuilder.Entity("NoteTakingApp.Core.Models.EntityVersion", b =>
                {
                    b.Property<int>("EntityVersionId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("EntityId");

                    b.Property<string>("EntityName");

                    b.Property<int>("Version");

                    b.HasKey("EntityVersionId");

                    b.ToTable("EntityVersions");
                });

            modelBuilder.Entity("NoteTakingApp.Core.Models.Note", b =>
                {
                    b.Property<int>("NoteId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Body");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime>("LastModifiedOn");

                    b.Property<string>("Slug");

                    b.Property<string>("Title");

                    b.Property<int>("Version");

                    b.HasKey("NoteId");

                    b.ToTable("Notes");
                });

            modelBuilder.Entity("NoteTakingApp.Core.Models.NoteTag", b =>
                {
                    b.Property<int>("TagId");

                    b.Property<int>("NoteId");

                    b.HasKey("TagId", "NoteId");

                    b.HasIndex("NoteId");

                    b.ToTable("NoteTag");
                });

            modelBuilder.Entity("NoteTakingApp.Core.Models.Tag", b =>
                {
                    b.Property<int>("TagId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedOn");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime>("LastModifiedOn");

                    b.Property<string>("Name");

                    b.Property<string>("Slug");

                    b.Property<int>("Version");

                    b.HasKey("TagId");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("NoteTakingApp.Core.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedOn");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime>("LastModifiedOn");

                    b.Property<string>("Password");

                    b.Property<byte[]>("Salt");

                    b.Property<string>("Username");

                    b.Property<int>("Version");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("NoteTakingApp.Core.Models.NoteTag", b =>
                {
                    b.HasOne("NoteTakingApp.Core.Models.Note", "Note")
                        .WithMany("NoteTags")
                        .HasForeignKey("NoteId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("NoteTakingApp.Core.Models.Tag", "Tag")
                        .WithMany("NoteTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}