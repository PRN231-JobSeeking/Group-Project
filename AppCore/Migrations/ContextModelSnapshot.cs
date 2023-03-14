﻿// <auto-generated />
using System;
using AppCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AppCore.Migrations
{
    [DbContext(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("AppCore.Models.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsLockout")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Accounts");

                    b.HasData(
                        new
                        {
                            Id = 6,
                            Address = "abc",
                            Email = "admin1@email",
                            FirstName = "abc",
                            IsDeleted = false,
                            IsLockout = false,
                            LastName = "abc",
                            Password = "123",
                            Phone = "0908123456",
                            RoleId = 1
                        },
                        new
                        {
                            Id = 1,
                            Address = "abc",
                            Email = "hr1@email",
                            FirstName = "abc",
                            IsDeleted = false,
                            IsLockout = false,
                            LastName = "abc",
                            Password = "123",
                            Phone = "0908123456",
                            RoleId = 2
                        },
                        new
                        {
                            Id = 2,
                            Address = "abc",
                            Email = "interviewer1@email",
                            FirstName = "abc",
                            IsDeleted = false,
                            IsLockout = false,
                            LastName = "abc",
                            Password = "123",
                            Phone = "0908123456",
                            RoleId = 3
                        },
                        new
                        {
                            Id = 3,
                            Address = "abc",
                            Email = "interviewer2@email",
                            FirstName = "abc",
                            IsDeleted = false,
                            IsLockout = false,
                            LastName = "abc",
                            Password = "123",
                            Phone = "0908123456",
                            RoleId = 3
                        },
                        new
                        {
                            Id = 4,
                            Address = "abc",
                            Email = "interviewer3@email",
                            FirstName = "abc",
                            IsDeleted = false,
                            IsLockout = false,
                            LastName = "abc",
                            Password = "123",
                            Phone = "0908123456",
                            RoleId = 3
                        },
                        new
                        {
                            Id = 5,
                            Address = "abc",
                            Email = "applicant01@email",
                            FirstName = "abc",
                            IsDeleted = false,
                            IsLockout = false,
                            LastName = "abc",
                            Password = "123",
                            Phone = "0908123456",
                            RoleId = 4
                        });
                });

            modelBuilder.Entity("AppCore.Models.Application", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("ApplicantId")
                        .HasColumnType("int");

                    b.Property<string>("CV")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("PostId")
                        .HasColumnType("int");

                    b.Property<bool?>("Status")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("ApplicantId");

                    b.HasIndex("PostId");

                    b.ToTable("Applications");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ApplicantId = 5,
                            CV = "asd",
                            IsDeleted = false,
                            PostId = 1
                        });
                });

            modelBuilder.Entity("AppCore.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            IsDeleted = false,
                            Name = "Backend API"
                        },
                        new
                        {
                            Id = 2,
                            IsDeleted = false,
                            Name = "Frontend Web"
                        });
                });

            modelBuilder.Entity("AppCore.Models.Interview", b =>
                {
                    b.Property<int>("ApplicationId")
                        .HasColumnType("int");

                    b.Property<int>("Round")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Round"), 1L, 1);

                    b.Property<DateTime>("Date")
                        .HasColumnType("date");

                    b.Property<string>("Feedback")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("InterviewerId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<double>("Point")
                        .HasColumnType("float");

                    b.Property<int>("SlotId")
                        .HasColumnType("int");

                    b.HasKey("ApplicationId", "Round");

                    b.HasIndex("InterviewerId");

                    b.HasIndex("SlotId");

                    b.ToTable("Interviews");
                });

            modelBuilder.Entity("AppCore.Models.Level", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Levels");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            IsDeleted = false,
                            Name = "Intern"
                        },
                        new
                        {
                            Id = 2,
                            IsDeleted = false,
                            Name = "Fresher"
                        },
                        new
                        {
                            Id = 3,
                            IsDeleted = false,
                            Name = "Junior"
                        },
                        new
                        {
                            Id = 4,
                            IsDeleted = false,
                            Name = "Senior"
                        });
                });

            modelBuilder.Entity("AppCore.Models.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Locations");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            IsDeleted = false,
                            Name = "location1"
                        });
                });

            modelBuilder.Entity("AppCore.Models.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("LevelId")
                        .HasColumnType("int");

                    b.Property<int>("LocationId")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("LevelId");

                    b.HasIndex("LocationId");

                    b.ToTable("Posts");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Amount = 1,
                            CategoryId = 1,
                            CreateDate = new DateTime(2023, 3, 14, 0, 0, 0, 0, DateTimeKind.Local),
                            Description = "abcdef",
                            EndDate = new DateTime(2023, 3, 24, 0, 0, 0, 0, DateTimeKind.Local),
                            IsDeleted = false,
                            LevelId = 1,
                            LocationId = 1,
                            StartDate = new DateTime(2023, 3, 14, 0, 0, 0, 0, DateTimeKind.Local),
                            Status = true,
                            Title = "Backend API hiring"
                        });
                });

            modelBuilder.Entity("AppCore.Models.PostSkillRequired", b =>
                {
                    b.Property<int>("SkillId")
                        .HasColumnType("int");

                    b.Property<int>("PostId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.HasKey("SkillId", "PostId");

                    b.HasIndex("PostId");

                    b.ToTable("PostSkills");

                    b.HasData(
                        new
                        {
                            SkillId = 1,
                            PostId = 1,
                            IsDeleted = false
                        },
                        new
                        {
                            SkillId = 2,
                            PostId = 1,
                            IsDeleted = false
                        });
                });

            modelBuilder.Entity("AppCore.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            IsDeleted = false,
                            Name = "Administrator"
                        },
                        new
                        {
                            Id = 2,
                            IsDeleted = false,
                            Name = "HR"
                        },
                        new
                        {
                            Id = 3,
                            IsDeleted = false,
                            Name = "Interviewer"
                        },
                        new
                        {
                            Id = 4,
                            IsDeleted = false,
                            Name = "Applicant"
                        });
                });

            modelBuilder.Entity("AppCore.Models.Skill", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Skills");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            IsDeleted = false,
                            Name = "C#"
                        },
                        new
                        {
                            Id = 2,
                            IsDeleted = false,
                            Name = "C++"
                        },
                        new
                        {
                            Id = 3,
                            IsDeleted = false,
                            Name = "Java"
                        },
                        new
                        {
                            Id = 4,
                            IsDeleted = false,
                            Name = "Ruby"
                        });
                });

            modelBuilder.Entity("AppCore.Models.Slot", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<TimeSpan>("EndTime")
                        .HasColumnType("time");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<TimeSpan>("StartTime")
                        .HasColumnType("time");

                    b.HasKey("Id");

                    b.ToTable("Slots");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            EndTime = new TimeSpan(0, 8, 0, 0, 0),
                            IsDeleted = false,
                            StartTime = new TimeSpan(0, 7, 0, 0, 0)
                        },
                        new
                        {
                            Id = 2,
                            EndTime = new TimeSpan(0, 9, 0, 0, 0),
                            IsDeleted = false,
                            StartTime = new TimeSpan(0, 8, 0, 0, 0)
                        },
                        new
                        {
                            Id = 3,
                            EndTime = new TimeSpan(0, 10, 0, 0, 0),
                            IsDeleted = false,
                            StartTime = new TimeSpan(0, 9, 0, 0, 0)
                        });
                });

            modelBuilder.Entity("AppCore.Models.UserSkill", b =>
                {
                    b.Property<int>("SkillId")
                        .HasColumnType("int");

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.HasKey("SkillId", "AccountId");

                    b.HasIndex("AccountId");

                    b.ToTable("UserSkills");

                    b.HasData(
                        new
                        {
                            SkillId = 1,
                            AccountId = 2,
                            IsDeleted = false
                        },
                        new
                        {
                            SkillId = 1,
                            AccountId = 5,
                            IsDeleted = false
                        },
                        new
                        {
                            SkillId = 2,
                            AccountId = 5,
                            IsDeleted = false
                        },
                        new
                        {
                            SkillId = 2,
                            AccountId = 2,
                            IsDeleted = false
                        },
                        new
                        {
                            SkillId = 3,
                            AccountId = 2,
                            IsDeleted = false
                        },
                        new
                        {
                            SkillId = 1,
                            AccountId = 3,
                            IsDeleted = false
                        },
                        new
                        {
                            SkillId = 2,
                            AccountId = 3,
                            IsDeleted = false
                        },
                        new
                        {
                            SkillId = 1,
                            AccountId = 4,
                            IsDeleted = false
                        });
                });

            modelBuilder.Entity("AppCore.Models.Account", b =>
                {
                    b.HasOne("AppCore.Models.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("AppCore.Models.Application", b =>
                {
                    b.HasOne("AppCore.Models.Account", "Applicant")
                        .WithMany()
                        .HasForeignKey("ApplicantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AppCore.Models.Post", "Post")
                        .WithMany()
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Applicant");

                    b.Navigation("Post");
                });

            modelBuilder.Entity("AppCore.Models.Interview", b =>
                {
                    b.HasOne("AppCore.Models.Application", "Application")
                        .WithMany()
                        .HasForeignKey("ApplicationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AppCore.Models.Account", "Interviewer")
                        .WithMany()
                        .HasForeignKey("InterviewerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AppCore.Models.Slot", "Slot")
                        .WithMany("Interviews")
                        .HasForeignKey("SlotId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Application");

                    b.Navigation("Interviewer");

                    b.Navigation("Slot");
                });

            modelBuilder.Entity("AppCore.Models.Post", b =>
                {
                    b.HasOne("AppCore.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AppCore.Models.Level", "Level")
                        .WithMany()
                        .HasForeignKey("LevelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AppCore.Models.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Level");

                    b.Navigation("Location");
                });

            modelBuilder.Entity("AppCore.Models.PostSkillRequired", b =>
                {
                    b.HasOne("AppCore.Models.Post", "Post")
                        .WithMany("SkillRequired")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AppCore.Models.Skill", "Skill")
                        .WithMany()
                        .HasForeignKey("SkillId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");

                    b.Navigation("Skill");
                });

            modelBuilder.Entity("AppCore.Models.UserSkill", b =>
                {
                    b.HasOne("AppCore.Models.Account", "Account")
                        .WithMany("UserSkill")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AppCore.Models.Skill", "Skill")
                        .WithMany()
                        .HasForeignKey("SkillId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Skill");
                });

            modelBuilder.Entity("AppCore.Models.Account", b =>
                {
                    b.Navigation("UserSkill");
                });

            modelBuilder.Entity("AppCore.Models.Post", b =>
                {
                    b.Navigation("SkillRequired");
                });

            modelBuilder.Entity("AppCore.Models.Slot", b =>
                {
                    b.Navigation("Interviews");
                });
#pragma warning restore 612, 618
        }
    }
}
