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
    public class QuanLyNhanVienController : Controller
    {
        private QL_BANHANGDIENTUEntities1 db = new QL_BANHANGDIENTUEntities1();

        // GET: QuanLyNhanVien
        public ActionResult Index()
        {
            return View(db.NHANVIENs.ToList());
        }
        public ActionResult Sort(string sortOrder)
        {
            var tbl_nv = db.NHANVIENs.ToList();

            switch (sortOrder)
            {
                case "AdenZ":
                    return View("Index", tbl_nv.OrderBy(t => t.TENNV).ToList());
                case "ZdenA":
                    return View("Index", tbl_nv.OrderByDescending(t => t.TENNV).ToList());
                default:
                    return View("Index", tbl_nv.ToList());
            }
        }
        public ActionResult AdenZ()
        {
            return View(db.NHANVIENs.OrderBy(t=>t.TENNV).ToList());

        }
        public ActionResult ZdenA()
        {
            return View(db.NHANVIENs.OrderByDescending(t => t.TENNV).ToList());

        }

        public ActionResult TimKiem(string tukhoa)
        {
            var tbl_NhanVien = db.NHANVIENs.Where(t => t.TENNV.Contains(tukhoa));
            return View(tbl_NhanVien.OrderBy(n => n.TENNV).ToList());
        }


        // GET: QuanLyNhanVien/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NHANVIEN tbl_NhanVien = db.NHANVIENs.Find(id);
            if (tbl_NhanVien == null)
            {
                return HttpNotFound();
            }
            return View(tbl_NhanVien);
        }

        // GET: QuanLyNhanVien/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: QuanLyNhanVien/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaNV,TenNV,Sodienthoai,Email,Diachi,Chucvu,Password")] NHANVIEN  tbl_NhanVien)
        {
            if (ModelState.IsValid)
            {
                var manv = db.NHANVIENs.Count() + 1;
                tbl_NhanVien.MANV = "NV" + manv.ToString("000");
                db.NHANVIENs.Add(tbl_NhanVien);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_NhanVien);
        }

        // GET: QuanLyNhanVien/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NHANVIEN tbl_NhanVien = db.NHANVIENs.Find(id);
            if (tbl_NhanVien == null)
            {
                return HttpNotFound();
            }
            return View(tbl_NhanVien);
        }

        // POST: QuanLyNhanVien/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaNV,TenNV,Sodienthoai,Email,Diachi,Chucvu,Password")] NHANVIEN tbl_NhanVien)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_NhanVien).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_NhanVien);
        }

        // GET: QuanLyNhanVien/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NHANVIEN tbl_NhanVien = db.NHANVIENs.Find(id);
            if (tbl_NhanVien == null)
            {
                return HttpNotFound();
            }
            return View(tbl_NhanVien);
        }

        // POST: QuanLyNhanVien/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            NHANVIEN tbl_NhanVien = db.NHANVIENs.Find(id);
            db.NHANVIENs.Remove(tbl_NhanVien);
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
