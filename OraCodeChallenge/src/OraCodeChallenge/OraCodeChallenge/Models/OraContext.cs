using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using OraCodeChallenge.Models.Entities;

namespace OraCodeChallenge.Models
{
    public class OraContext : DbContext
    {
        //public OraContext() : base("DefaultConnection")
        //{
        //}

        public DbSet<User> Users { get; set; }

        public DbSet<Chat> Chats { get; set; }

        public DbSet<ChatMessage> ChatMessages { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingEntitySetNameConvention>();
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Chat>().HasMany(c => c.ChatMessages);

            base.OnModelCreating(modelBuilder);
        }
    }
}