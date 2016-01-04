using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArbiterTown;
using ArbiterTown.Models;
using responseTip.Helpers;

namespace responseTip_backend
{
    public class ArbiterFinder
    {
        Logger logger=new Logger();
        private ApplicationDbContext dbContext;
        IOrderedEnumerable<ApplicationUser> orderedArbiters;
        IEnumerable<ApplicationUser> orderedArbitersAvailable;
        //TODO change to generalized tasks
        //TODO optimize searching the database
        public ArbiterFinder(ApplicationDbContext initContext)
        {
            dbContext = initContext;
        }

        public string[] FindArbiters(int arbiterCount)
        {
            if (arbiterCount > dbContext.Users.Count())
            {
                logger.LogLine("Not enough arbiters available! " + arbiterCount+" wanted. ", Logger.log_types.ERROR_LOG);
                throw new responseTip.Exceptions.NotEnoughArbitersAvailable();
            }
            string[] arbiterIds = new string[arbiterCount];
            //arbiterIds=dbContext.Users.Where(model => model.GetPercentageOfPuzzlesSuccesfull() > 0.5f).OrderByDescending(model => model.GetPercentageOfPuzzlesSuccesfull()).Select(model => model.Id).Take(task.ArbiterCount).ToArray();
            orderedArbiters = dbContext.Users.ToList().OrderByDescending(model => model.GetProbabilityOfBeingPicked());
            //TODO check if order is not changed by takewhile
            orderedArbitersAvailable = orderedArbiters.TakeWhile(model => model.GetNumOfPuzzlesWaiting() < model.GetNumOfPuzzlesLimit());
            arbiterIds=orderedArbitersAvailable.Select(model => model.Id).Take(arbiterCount).ToArray();

            

            //            arbiterIds = dbContext.Users.OrderByDescending(model => model.GetProbabilityOfBeingPicked()).Select(model => model.Id).Take(arbiterCount).ToArray();

            return arbiterIds;
        }

    }
}
