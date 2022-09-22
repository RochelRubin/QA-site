using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace QASite.Data
{
    public class QASiteContext: DbContext
    {
        private string _connectionString;

        public QASiteContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbSet<User> User { get; set; }
        public DbSet<Question> Question { get; set; }
        public DbSet<Answer> Answer { get; set; }
        public DbSet<Likes> Likes { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<QuestionTags> QuestionTags { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBulider)
        {
            optionsBulider.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            modelBuilder.Entity<QuestionTags>()
                .HasKey(qt => new { qt.QuestionId, qt.TagId });
            modelBuilder.Entity<Likes>()
                .HasKey(lk => new { lk.UserId, lk.QuestionId });
            base.OnModelCreating(modelBuilder);
        }

    }
}

