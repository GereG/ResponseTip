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
//        private ArbiterTaskAnswerContext db = new ArbiterTaskAnswerContext();
        private ApplicationDbContext db = new ApplicationDbContext();
//        private responseTipTaskContext responseTipTaskDb = new responseTipTaskContext();

        // GET: TextAnswerValidationTasks
        public ActionResult Index()
        {
//            responseTipTaskDb.ResponseTipTasks.Find(0);
            ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
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

            ResponseTipTask responseTipTask = db.ResponseTipTasks.Find(textAnswerValidationTask.ResponseTipTaskID);
            if(responseTipTask == null)
            {
                return HttpNotFound();
            }

            TextAnswerValidationPresented newTask = new TextAnswerValidationPresented(id, responseTipTask.question, responseTipTask.answer,(decimal)0.02);

            return View(newTask);
        }

        // POST: TextAnswerValidationTasks/Task/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Task([Bind(Include ="textAnswerValidationTaskId,arbiterAnswer")] TextAnswerValidationPresented textAnswerValidationPresented)
        {
            if (textAnswerValidationPresented == null)
            {
                return HttpNotFound();
            }
            TextAnswerValidationTask textAnswerValidationTaskFromDb = db.TextAnswerValidationTasks.Find(textAnswerValidationPresented.textAnswerValidationTaskId);
            if (textAnswerValidationTaskFromDb == null)
            {
                return HttpNotFound();
            }
            if (textAnswerValidationTaskFromDb.taskStatus != ArbiterTaskStatusesEnum.created) //if the task was already answered then go back to index
            {
                return RedirectToAction("Index");
            }

            textAnswerValidationTaskFromDb.taskStatus = ArbiterTown.Models.ArbiterTaskStatusesEnum.answered;
            textAnswerValidationTaskFromDb.arbiterAnswer = textAnswerValidationPresented.arbiterAnswer;
            ModelState.Clear();
            TryValidateModel(textAnswerValidationTaskFromDb);
            if (ModelState.IsValid)
            {
                db.Entry(textAnswerValidationTaskFromDb).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(textAnswerValidationPresented);
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
