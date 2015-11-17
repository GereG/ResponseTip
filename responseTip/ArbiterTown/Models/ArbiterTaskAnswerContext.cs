using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ArbiterTown.Models
{
    public class ArbiterTaskAnswerContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public ArbiterTaskAnswerContext() : base("name=DefaultConnection")
        {
        }

        public System.Data.Entity.DbSet<ArbiterTown.Models.TextAnswerValidationTask> TextAnswerValidationTasks { get; set; }
        public System.Data.Entity.DbSet<ArbiterTown.Models.ResponseTipTask> ResponseTipTasks { get; set; }
    }
}
