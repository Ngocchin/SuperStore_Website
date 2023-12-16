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
    public class QuanLyKhachHangController : Controller
    {
        private QL_BANHANGDIENTUEntities1 db = new QL_BANHANGDIENTUEntities1();

        // GET: tbl_KhachHang
        public ActionResult Index()
        {
            return View(db.KHACHHANGs.ToList());
        }

        public ActionResult Sort(string sortOrder)
        {
            var tbl_khach = db.KHACHHANGs.ToList();

            switch (sortOrder)
            {
                case "AdenZ":
                    return View("Index", tbl_khach.OrderBy(t => t.TENKH).ToList());
                case "ZdenA":
                    return View("Index", tbl_khach.OrderByDescending(t => t.TENKH).ToList());               
                default:
                    return View("Index", tbl_khach.ToList());
            }
        }
        public ActionResult AdenZ()
        {
            return View(db.KHACHHANGs.OrderBy(t=>t.TENKH).ToList());
        }
        public ActionResult ZdenA()
        {
            return View(db.KHACHHANGs.OrderByDescending(t => t.TENKH).ToList());
        }

        public ActionResult TimKiem(string tukhoa)
        {
            var tbl_KhachHang = db.KHACHHANGs.Where(t => t.TENKH.Contains(tukhoa));
            return View(tbl_KhachHang.OrderBy(n => n.TENKH).ToList());

        }
        // GET: tbl_KhachHang/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHACHHANG tbl_KhachHang = db.KHACHHANGs.Find(id);
            if (tbl_KhachHang == null)
            {
                return HttpNotFound();
            }
            return View(tbl_KhachHang);
        }

        // GET: tbl_KhachHang/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: tbl_KhachHang/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaKH,TenKH,Sodienthoai,Email,Password")] KHACHHANG tbl_KhachHang)
        {
            if (ModelState.IsValid)
            {
                db.KHACHHANGs.Add(tbl_KhachHang);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_KhachHang);
        }

        // GET: tbl_KhachHang/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHACHHANG tbl_KhachHang = db.KHACHHANGs.Find(id);
            if (tbl_KhachHang == null)
            {
                return HttpNotFound();
            }
            return View(tbl_KhachHang);
        }

        // POST: tbl_KhachHang/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaKH,TenKH,SDT,NGAYSINH,GIOITINH,DIACHI,Sodienthoai,Email,Password")] KHACHHANG tbl_KhachHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_KhachHang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_KhachHang);
        }

        // GET: tbl_KhachHang/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHACHHANG   tbl_KhachHang = db.KHACHHANGs.Find(id);
            if (tbl_KhachHang == null)
            {
                return HttpNotFound();
            }
            return View(tbl_KhachHang);
        }

        // POST: tbl_KhachHang/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            KHACHHANG tbl_KhachHang = db.KHACHHANGs.Find(id);
            db.KHACHHANGs.Remove(tbl_KhachHang);
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
