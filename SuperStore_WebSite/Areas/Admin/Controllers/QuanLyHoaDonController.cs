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
    public class QuanLyHoaDonController : Controller
    {
        private QL_BANHANGDIENTUEntities1 db = new QL_BANHANGDIENTUEntities1();

     
        public ActionResult Index()
        {
            return View(db.HOADONs.ToList());
        }
        public ActionResult Sort(string sortOrder)
        {
            var tbl_hd = db.HOADONs.ToList();

            switch (sortOrder)
            {
                case "DonCXN":
                    return View("Index", tbl_hd.Where(t => t.TINHTRANG == "Chưa xác nhận").ToList());
                case "DonDXN":
                    return View("Index", tbl_hd.Where(t => t.TINHTRANG == "Đã giao").ToList());
                case "SXMN":
                    return View("Index", tbl_hd.OrderByDescending(t => t.NGAYLAP).ToList());
                case "SXCN":
                    return View("Index", tbl_hd.OrderBy(t => t.NGAYLAP).ToList());               
                default:
                    return View("Index", tbl_hd.ToList());
            }
        }
        public ActionResult DonCXN()
        {
            return View(db.HOADONs.Where(t => t.TINHTRANG == "Chưa xác nhận").ToList());
        }
        public ActionResult DonDXN()
        {
            return View(db.HOADONs.Where(t => t.TINHTRANG == "Đã giao").ToList());
        }
        public ActionResult SXMN()
        {
            var HOADONs = db.HOADONs.Include(t => t.KHACHHANG).Include(t => t.NHANVIEN);
            return View(HOADONs.OrderByDescending(t => t.NGAYLAP).ToList());
        }
        public ActionResult SXCN()
        {
            var HOADONs = db.HOADONs.Include(t => t.KHACHHANG).Include(t => t.NHANVIEN);
            return View(HOADONs.OrderBy(t => t.NGAYLAP).ToList());
        }        

        public ActionResult CapnhatHD(string id, string manv)
        {
           
            HOADON hd = db.HOADONs.Find(id);
            NHANVIEN nv = (NHANVIEN)Session["Account"];

            if (hd != null)
            {
                  
                hd.TINHTRANG = "Đã giao";
                hd.MANV = nv.MANV;

                foreach (var chiTietHD in hd.CTHDs)
                {                   
                    SANPHAM sp = db.SANPHAMs.Find(chiTietHD.MASP);

                    if (sp != null)
                    {                       
                        sp.SOLUONGTON -= chiTietHD.SOLUONG;
                    }
                    else
                    {                        
                        return View();
                    }
                }

                db.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return View("InvoiceNotFound");
            }
        }
        public ActionResult HuyHD(string id, string manv)
        {
            
            HOADON hd = db.HOADONs.Find(id);
            NHANVIEN nv = (NHANVIEN)Session["Account"];
            hd.MANV = nv.MANV;
            if (hd != null)
            {
                
                hd.TINHTRANG = "Đã hủy";
                hd.NGAYLAP = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View("InvoiceNotFound");
            }
        }

        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HOADON HOADONs = db.HOADONs.Find(id);
            if (HOADONs == null)
            {
                return HttpNotFound();
            }
            return View(HOADONs);
        }

        
        public ActionResult Create()
        {
            ViewBag.MaKH = new SelectList(db.KHACHHANGs, "MaKH", "TenKH");
            ViewBag.MaNV = new SelectList(db.NHANVIENs, "MaNV", "TenNV");
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaHoaDon,NgayLap,MaKH,MaNV,Tinhtrang,TongTen")] HOADON HOADONs)
        {
            if (ModelState.IsValid)
            {
                var mahd = db.HOADONs.Count() + 1;
                HOADONs.MAHD = "HD" + mahd.ToString("000");
                db.HOADONs.Add(HOADONs);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaKH = new SelectList(db.KHACHHANGs, "MaKH", "TenKH", HOADONs.MANV);
            ViewBag.MaNV = new SelectList(db.NHANVIENs, "MaNV", "TenNV", HOADONs.MANV);
            return View(HOADONs);
        }

        
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HOADON HOADONs = db.HOADONs.Find(id);
            if (HOADONs == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaKH = new SelectList(db.KHACHHANGs, "MaKH", "TenKH", HOADONs.MANV);
            ViewBag.MaNV = new SelectList(db.NHANVIENs, "MaNV", "TenNV", HOADONs.MANV);
            return View(HOADONs);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaHoaDon,NgayLap,MaKH,MaNV,Tinhtrang,TongTen")] HOADON HOADONs)
        {
            if (ModelState.IsValid)
            {
                db.Entry(HOADONs).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaKH = new SelectList(db.KHACHHANGs, "MaKH", "TenKH", HOADONs.MANV);
            ViewBag.MaNV = new SelectList(db.NHANVIENs, "MaNV", "TenNV", HOADONs.MANV);
            return View(HOADONs);
        }

       
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HOADON HOADONs = db.HOADONs.Find(id);
            if (HOADONs == null)
            {
                return HttpNotFound();
            }
            return View(HOADONs);
        }

       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            HOADON HOADONs = db.HOADONs.Find(id);
            db.HOADONs.Remove(HOADONs);
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
