using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArbiterTown;
using ArbiterTown.Models;

namespace responseTip_backend
{
    public class ArbiterFinder
    {

        //TODO change to generalized tasks
        //TODO optimize searching the database
        public string[] FindArbiter(ArbiterTown.Models.ResponseTipTask task, ApplicationDbContext dbContext, int arbiterNum)
        {

            //            string arbiterId=dbContext.Users.OrderBy(model => model.GetPercentageOfPuzzlesSuccesfull()).ElementAt(arbiterNum).Id;


            //            dbContext.Users.Select(model => model.)

            string[] arbiterIds = new string[task.ArbiterCount];
            arbiterIds=dbContext.Users.Where(model => model.GetPercentageOfPuzzlesSuccesfull() > 0.5f).OrderByDescending(model => model.GetPercentageOfPuzzlesSuccesfull()).Select(model => model.Id).Take(task.ArbiterCount).ToArray();

            return arbiterIds;
        }
    }
}
