using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SuperStore_WebSite.Models;

namespace SuperStore_WebSite.Areas.Admin.Controllers
{
    public class QuanLyKhuyenMaiController : Controller
    {
        private QL_BANHANGDIENTUEntities1 db = new QL_BANHANGDIENTUEntities1();

        public ActionResult Index()
        {
            return View(db.KHUYENMAIs.ToList());
        }

        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHUYENMAI kHUYENMAI = db.KHUYENMAIs.Find(id);
            if (kHUYENMAI == null)
            {
                return HttpNotFound();
            }
            return View(kHUYENMAI);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MAKM,NGAYBD,NGAYKT,GIAGIAM")] KHUYENMAI kHUYENMAI)
        {
            if (ModelState.IsValid)
            {
                db.KHUYENMAIs.Add(kHUYENMAI);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(kHUYENMAI);
        }

        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHUYENMAI kHUYENMAI = db.KHUYENMAIs.Find(id);
            if (kHUYENMAI == null)
            {
                return HttpNotFound();
            }
            return View(kHUYENMAI);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MAKM,NGAYBD,NGAYKT,GIAGIAM")] KHUYENMAI kHUYENMAI)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kHUYENMAI).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(kHUYENMAI);
        }

        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHUYENMAI kHUYENMAI = db.KHUYENMAIs.Find(id);
            if (kHUYENMAI == null)
            {
                return HttpNotFound();
            }
            return View(kHUYENMAI);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            KHUYENMAI kHUYENMAI = db.KHUYENMAIs.Find(id);
            db.KHUYENMAIs.Remove(kHUYENMAI);
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
