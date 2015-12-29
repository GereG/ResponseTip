﻿using System.Data.Entity;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace ArbiterTown.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public decimal bitcoinEarned { get; set; }
        public int numOfPuzzlesSuccesfull { get; set; }
        public int numOfPuzzlesAttemted { get; set; }
        public int numOfPuzzlesSkipped { get; set; }
        public int hourlyPuzzleLimit { get; set; }
        public virtual ICollection<TextAnswerValidationTask> TextAnswerValidationTasks { get; set; }

    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ResponseTipTask>().Property(model => model.BitcoinPrice).HasPrecision(11, 8);
            modelBuilder.Entity<TextAnswerValidationTask>().Property(model => model.taskPriceInBitcoin).HasPrecision(11, 8);
            base.OnModelCreating(modelBuilder);
        }

        public System.Data.Entity.DbSet<ArbiterTown.Models.ResponseTipTask> ResponseTipTasks { get; set; }
        public System.Data.Entity.DbSet<ArbiterTown.Models.TextAnswerValidationTask> TextAnswerValidationTasks { get; set; }
//        public System.Data.Entity.DbSet<ArbiterTown.Models.ApplicationUser> ApplicationUsers { get; set; }
    }
}