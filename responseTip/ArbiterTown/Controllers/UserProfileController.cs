using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;
using ArbiterTown.Models;

namespace ArbiterTown.Controllers
{
    [Authorize]
    public class UserProfileController : Controller
    {
//        private ApplicationDbContext userDb = new ApplicationDbContext();

        // GET: Task
        public ActionResult Index()
        {
            //            return View(userDb.Users.Find(User.Identity.GetUserId()));
            return View();
        }
    }
}