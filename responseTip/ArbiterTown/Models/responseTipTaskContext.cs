﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ArbiterTown.Models
{
    public class responseTipTaskContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx

        public responseTipTaskContext() : base("name=DefaultConnection")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ResponseTipTask>().Property(model => model.BitcoinPrice).HasPrecision(11, 8);
            base.OnModelCreating(modelBuilder);
        }

        public System.Data.Entity.DbSet<ArbiterTown.Models.ResponseTipTask> ResponseTipTasks { get; set; }
    }
}