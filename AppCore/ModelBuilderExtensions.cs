using AppCore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    Id= 1,
                    Name= "Administrator",
                },
                new Role
                {
                    Id = 2,
                    Name = "HR"
                },
                new Role
                {
                    Id = 3,
                    Name = "Interviewer"
                },
                new Role
                {
                    Id = 4,
                    Name = "Applicant"
                }
            );
            modelBuilder.Entity<Slot>().HasData(
                new Slot()
                {
                    Id = 1,
                    StartTime = TimeSpan.FromHours(7),
                    EndTime= TimeSpan.FromHours(8)
                },
                new Slot()
                {
                    Id = 2,
                    StartTime = TimeSpan.FromHours(8),
                    EndTime = TimeSpan.FromHours(9)
                },
                new Slot()
                {
                    Id = 3,
                    StartTime = TimeSpan.FromHours(9),
                    EndTime = TimeSpan.FromHours(10)
                }
                );
            modelBuilder.Entity<Account>().HasData(
                new Account()
                {
                    Id = 1,
                    Address = "abc",
                    Email = "hr1@email",
                    FirstName = "abc",
                    LastName = "abc",
                    IsDeleted = false,
                    IsLockout = false,
                    Password = "123",
                    Phone = "0908123456",
                    RoleId = 2,
                },
                new Account()
                {
                    Id = 2,
                    Address = "abc",
                    Email = "interviewer1@email",
                    FirstName = "abc",
                    LastName = "abc",
                    IsDeleted = false,
                    IsLockout = false,
                    Password = "123",
                    Phone = "0908123456",
                    RoleId = 3,
                },
                new Account()
                {
                    Id = 3,
                    Address = "abc",
                    Email = "interviewer2@email",
                    FirstName = "abc",
                    LastName = "abc",
                    IsDeleted = false,
                    IsLockout = false,
                    Password = "123",
                    Phone = "0908123456",
                    RoleId = 3,
                },
                new Account()
                {
                    Id = 4,
                    Address = "abc",
                    Email = "interviewer3@email",
                    FirstName = "abc",
                    LastName = "abc",
                    IsDeleted = false,
                    IsLockout = false,
                    Password = "123",
                    Phone = "0908123456",
                    RoleId = 3,
                },
                new Account()
                {
                    Id = 5,
                    Address = "abc",
                    Email = "applicant01@email",
                    FirstName = "abc",
                    LastName = "abc",
                    IsDeleted = false,
                    IsLockout = false,
                    Password = "123",
                    Phone = "0908123456",
                    RoleId = 4,
                }
                );
            modelBuilder.Entity<Skill>().HasData(
                new Skill()
                {
                    Id = 1,
                    Name= "C#",
                    IsDeleted = false
                },
                new Skill()
                {
                    Id = 2,
                    Name = "C++",
                    IsDeleted = false
                },
                new Skill()
                {
                    Id = 3,
                    Name = "Java",
                    IsDeleted = false
                },
                new Skill()
                {
                    Id = 4,
                    Name = "Ruby",
                    IsDeleted = false
                }
                );
            modelBuilder.Entity<UserSkill>().HasData(
                new UserSkill()
                {
                    IsDeleted= false,
                    AccountId= 2,
                    SkillId= 1,
                },
                new UserSkill()
                {
                    IsDeleted = false,
                    AccountId = 5,
                    SkillId = 1,
                },
                new UserSkill()
                {
                    IsDeleted = false,
                    AccountId = 5,
                    SkillId = 2,
                },
                new UserSkill()
                {
                    IsDeleted = false,
                    AccountId = 2,
                    SkillId = 2                    ,
                },
                new UserSkill()
                {
                    IsDeleted = false,
                    AccountId = 2,
                    SkillId = 3,
                },
                new UserSkill()
                {
                    IsDeleted = false,
                    AccountId = 3,
                    SkillId = 1,
                },
                new UserSkill()
                {
                    IsDeleted = false,
                    AccountId = 3,
                    SkillId = 2,
                },
                new UserSkill()
                {
                    IsDeleted = false,
                    AccountId = 4,
                    SkillId = 1,
                }
                );
            modelBuilder.Entity<Category>().HasData(
                new Category()
                {
                    Id = 1,
                    IsDeleted= false,
                    Name = "Backend API"
                },
                new Category()
                {
                    Id = 2,
                    IsDeleted = false,
                    Name = "Frontend Web"
                }
                );
            modelBuilder.Entity<Level>().HasData(
                new Level()
                {
                    Id = 1,
                    IsDeleted = false,
                    Name = "Intern"
                },
                new Level()
                {
                    Id = 2,
                    IsDeleted= false,
                    Name = "Fresher"
                },
                new Level()
                {
                    Id = 3,
                    IsDeleted = false,
                    Name = "Junior"
                },
                new Level()
                {
                    Id = 4,
                    IsDeleted = false,
                    Name = "Senior"
                }
                );
            modelBuilder.Entity<Location>().HasData(
                new Location()
                {
                    Name= "location1",
                    IsDeleted= false,
                    Id = 1,
                });
            modelBuilder.Entity<Post>().HasData(
                new Post()
                {
                    Id = 1,
                    Amount = 1,
                    CategoryId = 1,                    
                    CreateDate = DateTime.Today,
                    IsDeleted= false,
                    Description = "abcdef",
                    EndDate= DateTime.Today.AddDays(10),
                    LevelId= 1,
                    LocationId = 1,
                    StartDate= DateTime.Today,
                    Status = true,
                    Title = "Backend API hiring"
                }
                );
            modelBuilder.Entity<PostSkillRequired>().HasData(
                new PostSkillRequired()
                {
                    IsDeleted= false,
                    PostId= 1,
                    SkillId= 1,
                },
                new PostSkillRequired()
                {
                    IsDeleted = false,
                    PostId = 1,
                    SkillId = 2,
                }
                );
            modelBuilder.Entity<Application>().HasData(
                new Application()
                {
                    ApplicantId= 5,
                    CV = "asd",
                    Id= 1,
                    IsDeleted= false,
                    PostId= 1,
                    Status= null,
                }
                );
        }
    }
}
