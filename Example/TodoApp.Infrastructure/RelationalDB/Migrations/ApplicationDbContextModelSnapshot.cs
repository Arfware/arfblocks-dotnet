﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TodoApp.Infrastructure.RelationalDB;

namespace TodoApp.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.6")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TodoApp.Domain.Entities.Department", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Departments");

                    b.HasData(
                        new
                        {
                            Id = new Guid("927dbce3-f162-4e80-8991-4c71d7aa7153"),
                            CreatedAt = new DateTime(2021, 12, 30, 9, 1, 14, 890, DateTimeKind.Utc).AddTicks(7213),
                            IsDeleted = false,
                            Name = "Human Resources"
                        },
                        new
                        {
                            Id = new Guid("423e95a1-44ce-4b4c-bffe-37d4548e51bd"),
                            CreatedAt = new DateTime(2021, 12, 30, 9, 1, 14, 890, DateTimeKind.Utc).AddTicks(7468),
                            IsDeleted = false,
                            Name = "Sales"
                        },
                        new
                        {
                            Id = new Guid("f20c58c7-52d6-4975-aef1-fd5f9fafc841"),
                            CreatedAt = new DateTime(2021, 12, 30, 9, 1, 14, 890, DateTimeKind.Utc).AddTicks(7471),
                            IsDeleted = false,
                            Name = "Marketing"
                        },
                        new
                        {
                            Id = new Guid("138ff80c-4139-4428-a1e0-2a475aa969c4"),
                            CreatedAt = new DateTime(2021, 12, 30, 9, 1, 14, 890, DateTimeKind.Utc).AddTicks(7473),
                            IsDeleted = false,
                            Name = "Information Technologies"
                        });
                });

            modelBuilder.Entity("TodoApp.Domain.Entities.TodoTask", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AssignedDepartmentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime>("StatusChangedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AssignedDepartmentId");

                    b.HasIndex("CreatedById");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("TodoApp.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("DepartmentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("f973d74b-b7df-40a6-a530-017dcdd870e7"),
                            CreatedAt = new DateTime(2021, 12, 30, 9, 1, 14, 891, DateTimeKind.Utc).AddTicks(5849),
                            DepartmentId = new Guid("927dbce3-f162-4e80-8991-4c71d7aa7153"),
                            Email = "mary@company.com",
                            FirstName = "Mary",
                            IsDeleted = false,
                            LastName = "Gleen"
                        },
                        new
                        {
                            Id = new Guid("3f05215c-b48e-479f-985d-001f2bdf2b7b"),
                            CreatedAt = new DateTime(2021, 12, 30, 9, 1, 14, 891, DateTimeKind.Utc).AddTicks(5860),
                            DepartmentId = new Guid("423e95a1-44ce-4b4c-bffe-37d4548e51bd"),
                            Email = "john@company.com",
                            FirstName = "John",
                            IsDeleted = false,
                            LastName = "Doe"
                        });
                });

            modelBuilder.Entity("TodoApp.Domain.Entities.TodoTask", b =>
                {
                    b.HasOne("TodoApp.Domain.Entities.Department", "AssignedDepartment")
                        .WithMany()
                        .HasForeignKey("AssignedDepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TodoApp.Domain.Entities.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AssignedDepartment");

                    b.Navigation("CreatedBy");
                });

            modelBuilder.Entity("TodoApp.Domain.Entities.User", b =>
                {
                    b.HasOne("TodoApp.Domain.Entities.Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Department");
                });
#pragma warning restore 612, 618
        }
    }
}
