using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ArbiterTown.Models;
using Microsoft.AspNet.Identity;

namespace ArbiterTown.DAL
{
    public class ArbiterInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            PasswordHasher passHasher = new PasswordHasher();
            var seedArbiterUsers = new List<ApplicationUser>
            {
                new ApplicationUser { UserName = "tom@seznam.cz", Email = "tom@seznam.cz",PasswordHash=passHasher.HashPassword("Blabla0@"),SecurityStamp=Guid.NewGuid().ToString(),PuzzleLimit=10 },
                new ApplicationUser { UserName = "tom2@seznam.cz", Email = "tom2@seznam.cz",PasswordHash=passHasher.HashPassword("Blabla0@"),SecurityStamp=Guid.NewGuid().ToString(),PuzzleLimit=10 },
                new ApplicationUser { UserName = "tom3@seznam.cz", Email = "tom3@seznam.cz",PasswordHash=passHasher.HashPassword("Blabla0@"),SecurityStamp=Guid.NewGuid().ToString(),PuzzleLimit=10 },
                new ApplicationUser { UserName = "tom4@seznam.cz", Email = "tom4@seznam.cz",PasswordHash=passHasher.HashPassword("Blabla0@"),SecurityStamp=Guid.NewGuid().ToString(),PuzzleLimit=10 },
                new ApplicationUser { UserName = "tom5@seznam.cz", Email = "tom5@seznam.cz",PasswordHash=passHasher.HashPassword("Blabla0@"),SecurityStamp=Guid.NewGuid().ToString(),PuzzleLimit=10 },
                new ApplicationUser { UserName = "tom6@seznam.cz", Email = "tom6@seznam.cz",PasswordHash=passHasher.HashPassword("Blabla0@"),SecurityStamp=Guid.NewGuid().ToString(),PuzzleLimit=10 },
                new ApplicationUser { UserName = "tom7@seznam.cz", Email = "tom7@seznam.cz",PasswordHash=passHasher.HashPassword("Blabla0@"),SecurityStamp=Guid.NewGuid().ToString(),PuzzleLimit=10 },
                new ApplicationUser { UserName = "tom8@seznam.cz", Email = "tom8@seznam.cz",PasswordHash=passHasher.HashPassword("Blabla0@"),SecurityStamp=Guid.NewGuid().ToString(),PuzzleLimit=10 },
                new ApplicationUser { UserName = "tom9@seznam.cz", Email = "tom9@seznam.cz",PasswordHash=passHasher.HashPassword("Blabla0@"),SecurityStamp=Guid.NewGuid().ToString(),PuzzleLimit=10 },
                new ApplicationUser { UserName = "tom10@seznam.cz", Email = "tom10@seznam.cz",PasswordHash=passHasher.HashPassword("Blabla0@"),SecurityStamp=Guid.NewGuid().ToString(),PuzzleLimit=10 }
        };
            seedArbiterUsers.ForEach(s => context.Users.Add(s));
            context.SaveChanges();

            var seedResponseTipTasks = new List<ResponseTipTask>
            {
                new ResponseTipTask { userName="460763d9-14e3-4967-9031-e2d546ad2699",question="seededquestion1",twitterUserNameWritten="RichardVelky", taskStatus=TaskStatusesEnum.responseTip_paid, twitterUserNameSelected="RichardVelky",timeCreated=DateTime.Now,BitcoinPublicAdress="mmA6rxdgZsCFAgxb3jgMjbb6tyXUSesmYL",BitcoinReturnPublicAddress="17xm46Mm8ZFGWKdqknF5QF3HLtFb2zd6fb",DollarPrice=1,BitcoinPrice=(decimal)0.0005,ArbiterCount=5}
            };

            seedResponseTipTasks.ForEach(s => context.ResponseTipTasks.Add(s));
            context.SaveChanges();

            base.Seed(context);
        }
    }
}