using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using responseTip.Models;

namespace responseTip.Controllers
{
    [Authorize]
    public class ResponseTipTasksController : Controller
    {
        private responseTipContext db = new responseTipContext();

        // GET: ResponseTipTasks
        public ActionResult Index()
        {
            return View(db.ResponseTipTasks.ToList());
        }

        // GET: ResponseTipTasks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ResponseTipTask responseTipTask = db.ResponseTipTasks.Find(id);
            if (responseTipTask == null)
            {
                return HttpNotFound();
            }
            return View(responseTipTask);
        }

        // GET: ResponseTipTasks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ResponseTipTasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ResponseTipTaskID,question,socialSiteUser,BitcoinPublicAdress,BitcoinPrice,isQuestionPublic")] ResponseTipTask responseTipTask)
        {
            if (ModelState.IsValid)
            {
                responseTipTask.taskStatus = TaskStatusesEnum.created;
                responseTipTask.userName = User.Identity.Name;
                db.ResponseTipTasks.Add(responseTipTask);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(responseTipTask);
        }

        // GET: ResponseTipTasks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ResponseTipTask responseTipTask = db.ResponseTipTasks.Find(id);
            if (responseTipTask == null)
            {
                return HttpNotFound();
            }
            return View(responseTipTask);
        }

        // POST: ResponseTipTasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ResponseTipTaskID,userID,question,socialSiteUser,BitcoinPublicAdress,BitcoinPrice,isQuestionPublic,taskStatus")] ResponseTipTask responseTipTask)
        {
            if (ModelState.IsValid)
            {
                db.Entry(responseTipTask).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(responseTipTask);
        }

        // GET: ResponseTipTasks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ResponseTipTask responseTipTask = db.ResponseTipTasks.Find(id);
            if (responseTipTask == null)
            {
                return HttpNotFound();
            }
            return View(responseTipTask);
        }

        // POST: ResponseTipTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ResponseTipTask responseTipTask = db.ResponseTipTasks.Find(id);
            db.ResponseTipTasks.Remove(responseTipTask);
            db.SaveChanges();
            return RedirectToAction("Index");
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
