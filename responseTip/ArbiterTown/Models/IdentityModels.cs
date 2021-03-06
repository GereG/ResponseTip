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
        public float percentageOfPuzzlesSuccesfull { get; set; }
        public int numOfPuzzlesSkipped { get; set; }
        public int numOfPuzzlesExpired { get; set; }
        public int PuzzleLimit { get; set; }
        public int numOfPuzzlesWaiting { get; set; }
        private static Random rndg = new Random();

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

        public void IncrementNumOfPuzzlesNotSuccesfull()
        {
            numOfPuzzlesAttemted++;
            if (numOfPuzzlesAttemted > 10)
                percentageOfPuzzlesSuccesfull = numOfPuzzlesSuccesfull / numOfPuzzlesAttemted;
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

        public void IncrementNumOfPuzzlesExpired()
        {
            numOfPuzzlesExpired++;
        }

        public void UpdateNumOfPuzzlesWaiting()
        {
            int tasksWaiting = 0;
            foreach(TextAnswerValidationTask task in TextAnswerValidationTasks)
            {
                switch(task.taskStatus)
                {
                    case ArbiterTaskStatusesEnum.textAnswerValidation_created:
                    case ArbiterTaskStatusesEnum.textAnswerValidation_waitingForAnswer:
                    case ArbiterTaskStatusesEnum.textAnswerValidation_expired:
                        tasksWaiting++;
                        break;
                }
            }
            numOfPuzzlesWaiting = tasksWaiting;
        }

        public bool IncrementNumOfPuzzlesWaiting()
        {
            numOfPuzzlesWaiting++;

            return false; //always return something to be able to call this function from database query 
        }

        public void DecrementNumOfPuzzlesWaiting()
        {
            numOfPuzzlesWaiting--;
        }

        public int GetNumOfPuzzlesWaiting()
        {
            return numOfPuzzlesWaiting;
        }

        public int GetNumOfPuzzlesLimit()
        {
            return PuzzleLimit;
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
        /// <summary>
        /// returns probability of being picked as arbiter
        ///
        /// </summary>
        /// <returns></returns>
        public float GetProbabilityOfBeingPicked()
        { 
//            float availability = (PuzzleLimit - numOfPuzzlesWaiting) / PuzzleLimit;
            float trustworthiness = 0.3f + 0.7f*(numOfPuzzlesAttemted / (10 + numOfPuzzlesAttemted));
            float randomness = (float)rndg.NextDouble();
            
            return percentageOfPuzzlesSuccesfull * trustworthiness * randomness;
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
            modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2")); // map all datetime na datetime2
            modelBuilder.Entity<ApplicationUser>().Property(model => model.bitcoinEarned).HasPrecision(11, 8);
            modelBuilder.Entity<ResponseTipTask>().Property(model => model.BitcoinPrice).HasPrecision(11, 8);
//            modelBuilder.Entity<ResponseTipTask>().Property(model => model.timeCreated).
            modelBuilder.Entity<TextAnswerValidationTask>().Property(model => model.taskPriceInBitcoin).HasPrecision(11, 8);
            base.OnModelCreating(modelBuilder);
        }

        public System.Data.Entity.DbSet<ArbiterTown.Models.ResponseTipTask> ResponseTipTasks { get; set; }
        public System.Data.Entity.DbSet<ArbiterTown.Models.TextAnswerValidationTask> TextAnswerValidationTasks { get; set; }
//        public System.Data.Entity.DbSet<ArbiterTown.Models.ApplicationUser> ApplicationUsers { get; set; }
    }
}