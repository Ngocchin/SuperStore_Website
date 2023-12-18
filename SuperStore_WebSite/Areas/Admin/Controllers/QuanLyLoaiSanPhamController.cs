using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SuperStore_WebSite.Models;

namespace SuperStore_WebSite.Areas.Admin.Controllers
{
    public class QuanLyLoaiSanPhamController : Controller
    {
        private QL_BANHANGDIENTUEntities1 db = new QL_BANHANGDIENTUEntities1();

        // GET: QuanLyLoaiSanPham
        public ActionResult Index()
        {
            return View(db.LOAIs.ToList());
        }

        // GET: QuanLyLoaiSanPham/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LOAI lOAI = db.LOAIs.Find(id);
            if (lOAI == null)
            {
                return HttpNotFound();
            }
            return View(lOAI);
        }

        // GET: QuanLyLoaiSanPham/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: QuanLyLoaiSanPham/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MALOAI,TENLOAI")] LOAI lOAI)
        {
            if (ModelState.IsValid)
            {
                //var maloai = db.LOAIs.Count() + 1;
                //lOAI.MALOAI = "L" + maloai.ToString("000");
                db.LOAIs.Add(lOAI);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(lOAI);
        }

        // GET: QuanLyLoaiSanPham/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LOAI lOAI = db.LOAIs.Find(id);
            if (lOAI == null)
            {
                return HttpNotFound();
            }
            return View(lOAI);
        }

        // POST: QuanLyLoaiSanPham/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MALOAI,TENLOAI")] LOAI lOAI)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lOAI).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(lOAI);
        }

        // GET: QuanLyLoaiSanPham/Delete/5
        public ActionResult Delete(string id)
            {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LOAI lOAI = db.LOAIs.Find(id);
            if (lOAI == null)
            {
                return HttpNotFound();
            }
            return View(lOAI);
        }

        // POST: QuanLyLoaiSanPham/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            try
            {
                LOAI loai = db.LOAIs.Find(id);

                if (loai == null)
                {
                    return HttpNotFound();
                }
                db.LOAIs.Remove(loai);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (DbUpdateException ex)

            {
                var innerException = ex.InnerException;
                while (innerException != null)
                {
                    // In ra thông tin chi tiết của lỗi
                    Console.WriteLine(innerException.Message);
                    innerException = innerException.InnerException;
                }
            }
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
