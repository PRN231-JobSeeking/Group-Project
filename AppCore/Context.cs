﻿using AppCore.Models;
using Microsoft.EntityFrameworkCore;

namespace AppCore
{
    public class Context : DbContext
    {
        public DbSet<Account> Accounts { get; set; } = null!;
        public DbSet<Application> Applications { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Interview> Interviews { get; set; } = null!;
        public DbSet<Level> Levels { get; set; } = null!;
        public DbSet<Location> Locations { get; set; } = null!;
        public DbSet<Post> Posts { get; set; } = null!;
        public DbSet<PostSkillRequired> PostSkills { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<Skill> Skills { get; set; } = null!;
        public DbSet<UserSkill> UserSkills { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("server=WILLIAMTRUNG\\MYSQL;database=JobSeekingDB;uid=sa;pwd=123;trusted_connection=true");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().HasKey(a => a.Id);
            modelBuilder.Entity<Interview>().HasKey(i => new { i.ApplicationId, i.Round });
            modelBuilder.Entity<PostSkillRequired>().HasKey(ps => new { ps.SkillId, ps.PostId});
            modelBuilder.Entity<UserSkill>().HasKey(us => new { us.SkillId, us.AccountId});
            //modelBuilder.Entity<>
            base.OnModelCreating(modelBuilder);
        }
    }
}