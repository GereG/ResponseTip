using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using responseTip.Models;
//using responseTip.Bussines_logic;
using System.Diagnostics;
using BtcHandling;
using responseTip.Helpers;

namespace responseTip.Controllers
{
    [RequireHttps]
    [Authorize]
    public class ResponseTipTasksController : Controller
    {
        private TwitterHandling.TwitterHandlingClass.SearchResults UserSearchResults;
        
        private responseTipContext db = new responseTipContext();


 /*       public ActionResult GetImg()
        {
            
            var srcImage = Image.FromFile(imageFile);
            using (var streak = new MemoryStream())
            {
                srcImage.Save(streak, ImageFormat.Png);
                return File(streak.ToArray(), "image/png");
            }
        }*/

            //GET: ResponseTipTasks
        public ActionResult FindUser(int? id)
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
            //            Debug.WriteLine("taskcontroller: FindUser");
            //            TwitterHandling.TwitterHandlingClass.PublishTweet("hello " + responseTipTask.twitterUserNameWritten);
            //            UserSearchResults= TwitterHandling.TwitterHandlingClass.SearchUsersM(responseTipTask.twitterUserNameWritten);
            UserSearchResults = TwitterHandling.TwitterHandlingClass.SearchUsersM(responseTipTask.twitterUserNameWritten);
            //ViewBag.SearchResultsInBag = TwitterHandling.TwitterHandlingClass.SearchUsersM(responseTipTask.twitterUserNameWritten);
            ViewBag.SearchResultsInBag = UserSearchResults;
            return View(responseTipTask);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FindUser(int? id,[Bind(Include = "ResponseTipTaskID,twitterUserNameSelected")] ResponseTipTask responseTipTaskChanged)
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
            responseTipTask.twitterUserNameSelected = responseTipTaskChanged.twitterUserNameSelected;
            if (responseTipTask.twitterUserNameSelected!=null)
            {
                db.SaveChanges();
                ResponseTipTask responseTipTasktest = db.ResponseTipTasks.Find(id);
                return RedirectToAction("Create");
            }
            //            return RedirectToAction("FindUser", new { id = responseTipTask.ResponseTipTaskID });
            //            ViewBag.SearchResultsInBag = TwitterHandling.TwitterHandlingClass.SearchUsersM(responseTipTask.twitterUserNameWritten); 
            UserSearchResults = TwitterHandling.TwitterHandlingClass.SearchUsersM(responseTipTask.twitterUserNameWritten);
            return View(responseTipTask);
 //           return View();
        }

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
            ResponseTipTask newTask = new ResponseTipTask();
            newTask.question = "AutomaticQuestion";
            newTask.twitterUserNameWritten = "RichardVelky";
            newTask.BitcoinReturnPublicAddress = "n2eMqTT929pb1RDNuqEnxdaLau1rxy3efi";
            newTask.DollarPrice = (decimal)1;
            /*            string address = BtcHandling.BtcHandlingClass.GetNewBtcAdress();
                        Debug.WriteLine("new adress: " + address);*/
            //            UserSearchResults = TwitterHandling.TwitterHandlingClass.SearchUsersM("bb");
            return View(newTask);
        }

        // POST: ResponseTipTasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ResponseTipTaskID,question,twitterUserNameWritten,BitcoinReturnPublicAddress,DollarPrice,isQuestionPublic")] ResponseTipTask responseTipTask)
        {
            responseTipTask.BitcoinPublicAdress = BtcHandlingClass.GetNewBtcAdress();
            responseTipTask.BitcoinPrice = responseTipTask.DollarPrice / externalAPIs.UpdateBitcoinAverageDollarPrice();
            decimal estimatedTxFee = BtcHandlingClass.UpdateEstimatedTxFee();
            responseTipTask.BitcoinPrice += estimatedTxFee;
            ModelState.Clear();
            TryValidateModel(responseTipTask);

            if (ModelState.IsValid)
            {
                responseTipTask.taskStatus = TaskStatusesEnum.created;
                responseTipTask.userName = User.Identity.Name;
                
                responseTipTask.timeCreated = DateTime.Now;
                responseTipTask.timeQuestionAsked = DateTime.MinValue;
                db.ResponseTipTasks.Add(responseTipTask);
                db.SaveChanges();
                return RedirectToAction("FindUser",new { id = responseTipTask.ResponseTipTaskID });
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
