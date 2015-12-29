using System.Data.Entity;
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
        private int numOfPuzzlesSuccesfull { get; set; }
        private int numOfPuzzlesAttemted { get; set; }
        private float percentageOfPuzzlesSuccesfull { get; set; }
        private int numOfPuzzlesSkipped { get; set; }
        public int hourlyPuzzleLimit { get; set; }
        public int numOfPuzzlesWaiting { get; set; }

        public virtual ICollection<TextAnswerValidationTask> TextAnswerValidationTasks { get; set; }

        public ApplicationUser()
        {
            percentageOfPuzzlesSuccesfull = 0.5f;
        }

        /// <summary>
        /// Increments number of sucesfull and if he has more then 10 answers it computes new sucess percentage 
        /// </summary>
        public void IncrementNumOfPuzzlesSuccesfull()
        {
            numOfPuzzlesAttemted++;
            numOfPuzzlesSuccesfull++;
            if(numOfPuzzlesAttemted>10)
                percentageOfPuzzlesSuccesfull = numOfPuzzlesSuccesfull / numOfPuzzlesAttemted;
            //else 0.5f
        }

        /// <summary>
        /// Increments number of skipped and if he has more then 10 answers it computes new sucess percentage
        /// </summary>
        public void IncrementNumOfPuzzlesSkipped() 
        {
            numOfPuzzlesAttemted++;
            numOfPuzzlesSkipped++;
            percentageOfPuzzlesSuccesfull = numOfPuzzlesSuccesfull / numOfPuzzlesAttemted;
        }

        public int GetNumOfPuzzlesSuccesfull()
        {
            return numOfPuzzlesSuccesfull;
        }

        public int GetNumOfPuzzlesAttemted()
        {
            return numOfPuzzlesAttemted;
        }

        public int GetNumOfPuzzlesSkipped()
        {
            return numOfPuzzlesSkipped;
        }

        public float GetPercentageOfPuzzlesSuccesfull()
        {
            return percentageOfPuzzlesSuccesfull;
        }


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
            modelBuilder.Entity<ApplicationUser>().Property(model => model.bitcoinEarned).HasPrecision(11, 8);
            modelBuilder.Entity<ResponseTipTask>().Property(model => model.BitcoinPrice).HasPrecision(11, 8);
            modelBuilder.Entity<TextAnswerValidationTask>().Property(model => model.taskPriceInBitcoin).HasPrecision(11, 8);
            base.OnModelCreating(modelBuilder);
        }

        public System.Data.Entity.DbSet<ArbiterTown.Models.ResponseTipTask> ResponseTipTasks { get; set; }
        public System.Data.Entity.DbSet<ArbiterTown.Models.TextAnswerValidationTask> TextAnswerValidationTasks { get; set; }
//        public System.Data.Entity.DbSet<ArbiterTown.Models.ApplicationUser> ApplicationUsers { get; set; }
    }
}