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

        // GET: QuanLyHoaDon
        public ActionResult Index()
        {
            return View(db.HOADONs.ToList());
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
            // Find the invoice with the specified 'ma'
            HOADON hd = db.HOADONs.Find(id);
            NHANVIEN nv = (NHANVIEN)Session["Account"];

            if (hd != null)
            {
                // Update the status of the invoice             
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

                // Save the changes to the database
                db.SaveChanges();

                // Redirect to the 'Index' action
                return RedirectToAction("Index");
            }
            else
            {
                // Handle the case when the invoice with the specified 'ma' is not found
                // You can return a view or perform other actions as needed
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

        // GET: QuanLyHoaDon/Create
        public ActionResult Create()
        {
            ViewBag.MaKH = new SelectList(db.KHACHHANGs, "MaKH", "TenKH");
            ViewBag.MaNV = new SelectList(db.NHANVIENs, "MaNV", "TenNV");
            return View();
        }

        // POST: QuanLyHoaDon/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaHoaDon,NgayLap,MaKH,MaNV,Tinhtrang,TongTen")] HOADON HOADONs)
        {
            if (ModelState.IsValid)
            {
                db.HOADONs.Add(HOADONs);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaKH = new SelectList(db.KHACHHANGs, "MaKH", "TenKH", HOADONs.MANV);
            ViewBag.MaNV = new SelectList(db.NHANVIENs, "MaNV", "TenNV", HOADONs.MANV);
            return View(HOADONs);
        }

        // GET: QuanLyHoaDon/Edit/5
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

        // POST: QuanLyHoaDon/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: QuanLyHoaDon/Delete/5
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

        // POST: QuanLyHoaDon/Delete/5
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
