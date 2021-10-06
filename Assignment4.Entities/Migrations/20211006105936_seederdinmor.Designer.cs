﻿// <auto-generated />
using Assignment4.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Assignment4.Entities.Migrations
{
    [DbContext(typeof(KanbanContext))]
    [Migration("20211006105936_seederdinmor")]
    partial class seederdinmor
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.10")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Assignment4.Entities.Tag", b =>
                {
                    b.Property<string>("name")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.HasKey("name");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("Assignment4.Entities.Task", b =>
                {
                    b.Property<string>("Title")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("AssignedToEmail")
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Title");

                    b.HasIndex("AssignedToEmail");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("Assignment4.Entities.User", b =>
                {
                    b.Property<string>("Email")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Email");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TagTask", b =>
                {
                    b.Property<string>("Tagsname")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("TasksTitle")
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Tagsname", "TasksTitle");

                    b.HasIndex("TasksTitle");

                    b.ToTable("TagTask");
                });

            modelBuilder.Entity("Assignment4.Entities.Task", b =>
                {
                    b.HasOne("Assignment4.Entities.User", "AssignedTo")
                        .WithMany("Tasks")
                        .HasForeignKey("AssignedToEmail");

                    b.Navigation("AssignedTo");
                });

            modelBuilder.Entity("TagTask", b =>
                {
                    b.HasOne("Assignment4.Entities.Tag", null)
                        .WithMany()
                        .HasForeignKey("Tagsname")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Assignment4.Entities.Task", null)
                        .WithMany()
                        .HasForeignKey("TasksTitle")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Assignment4.Entities.User", b =>
                {
                    b.Navigation("Tasks");
                });
#pragma warning restore 612, 618
        }
    }
}
