using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using ArbiterTown.Models;


namespace ArbiterTown.Controllers
{
    [Authorize]
    public class TextAnswerValidationTasksController : Controller
    {
        private ArbiterTaskAnswersContext db = new ArbiterTaskAnswersContext();
        private ApplicationDbContext userDb = new ApplicationDbContext();

        // GET: TextAnswerValidationTasks
        public ActionResult Index()
        {
            ApplicationUser user = userDb.Users.Find(User.Identity.GetUserId());
            return View(user.TextAnswerValidationTasks.ToList());
            //            return View(db.TextAnswerValidationTasks.ToList());
        }

        // GET: TextAnswerValidationTasks/Task/5
        public ActionResult Task(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TextAnswerValidationTask textAnswerValidationTask = db.TextAnswerValidationTasks.Find(id);

            if (textAnswerValidationTask == null)
            {
                return HttpNotFound();
            }



            if (User.Identity.GetUserId() != textAnswerValidationTask.ApplicationUserId) //check if right user is requesting to answer
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            return View(textAnswerValidationTask);
        }

        // POST: TextAnswerValidationTasks/Task/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Task([Bind(Include = "id,arbiterAnswer")] TextAnswerValidationTask textAnswerValidationTask)
        {
            if (textAnswerValidationTask == null)
            {
                return HttpNotFound();
            }
            TextAnswerValidationTask textAnswerValidationTaskFromDb = db.TextAnswerValidationTasks.Find(textAnswerValidationTask.id);
            if (textAnswerValidationTaskFromDb == null)
            {
                return HttpNotFound();
            }
            if (textAnswerValidationTaskFromDb.taskStatus != ArbiterTaskStatusesEnum.created) //if the task was already answered then go back to index
            {
                return RedirectToAction("Index");
            }

            textAnswerValidationTaskFromDb.taskStatus = ArbiterTown.Models.ArbiterTaskStatusesEnum.answered;
            textAnswerValidationTaskFromDb.arbiterAnswer = textAnswerValidationTask.arbiterAnswer;
            ModelState.Clear();
            TryValidateModel(textAnswerValidationTaskFromDb);
            if (ModelState.IsValid)
            {
                db.Entry(textAnswerValidationTaskFromDb).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(textAnswerValidationTask);
        }




        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
