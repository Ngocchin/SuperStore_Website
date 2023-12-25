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

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaKH,TenKH,Sodienthoai,Email,Password")] KHACHHANG tbl_KhachHang)
        {
            if (ModelState.IsValid)
            {
                var makh = db.KHACHHANGs.Count() + 1;
                tbl_KhachHang.MAKH = "KH" + makh.ToString("000");
                db.KHACHHANGs.Add(tbl_KhachHang);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_KhachHang);
        }

     
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
