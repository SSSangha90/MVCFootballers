using MVCFootballers.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;



namespace MVCFootballers.Controllers
{
    public class HomeController : Controller
    {
        private ManUtdPlayersEntities db = new ManUtdPlayersEntities();

        public ActionResult Index(string searchString, string playerPosition)
        {
            List<string> positionList = new List<string>();
            var positionQuery = from p in db.Footballers
                                orderby p.Position
                                select p.Position;
            positionList.AddRange(positionQuery.Distinct());
            ViewBag.playerPosition = new SelectList(positionList);

            // getting all the players from the db
            var footballers = from f in db.Footballers
                              select f;

            //filtering by position
            if (!String.IsNullOrEmpty(playerPosition))
            {
                footballers = footballers.Where(x => x.Position == playerPosition);
            }

            //searching by name
            if (!String.IsNullOrEmpty(searchString))
            {
                footballers = footballers.Where(x => x.Name.Contains(searchString));
            }
            return View(footballers);
        } 

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add([Bind(Include = "PlayerID, Name, Age, Position, Value, ImageUrl, VideoUrl")]Footballer footballer)
        {
            if(footballer.ImageUrl == null)
            {
                footballer.ImageUrl = "https://www.manutd.com/";
            }
            if (ModelState.IsValid)
            {
            db.Footballers.Add(footballer);
            db.SaveChanges();
            return RedirectToAction("Index");
            }
            return View(footballer);
        }

        public ActionResult Edit(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Footballer footballer = db.Footballers.Find(id);
            if(footballer == null)
            {
                return HttpNotFound();
            }
            return View(footballer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PlayersID, Name, Age, Position, Value, ImageUrl, VideoUrl")]Footballer footballer)
        {
            if(footballer.ImageUrl == null)
            {
                footballer.ImageUrl = "https://www.manutd.com/";
            }
            if (ModelState.IsValid)
            { 
            db.Entry(footballer).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
            }
            return View(footballer);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Footballer footballer = db.Footballers.Find(id);
            if (footballer == null)
            {
                return HttpNotFound();
            }
            return View(footballer);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Footballer footballer = db.Footballers.Find(id);
            if (footballer == null)
            {
                return HttpNotFound();
            }
            return View(footballer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
            public ActionResult DeleteConfirmed(int id)
        {
            Footballer footballer = db.Footballers.Find(id);
            db.Footballers.Remove(footballer);
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
