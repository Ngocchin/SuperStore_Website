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
    public class ThongKeDonHangController : Controller
    {
        private QL_BANHANGDIENTUEntities1 db = new QL_BANHANGDIENTUEntities1();

        // GET: ThongKeDonHang
        public ActionResult Index(int? thang, string date)
        {
            var tbl_HoaDon = db.HOADONs.Include(t => t.KHACHHANG).Include(t => t.NHANVIEN);
            if (thang != null)
            {
                tbl_HoaDon = db.HOADONs.Include(t => t.KHACHHANG).Include(t => t.NHANVIEN).Where(t => t.NGAYLAP.Value.Month == thang);
            }
            else
            {
                if (date != null)
                {
                    tbl_HoaDon = db.HOADONs.Include(t => t.KHACHHANG).Include(t => t.NHANVIEN).Where(t => t.NGAYLAP.Value.ToString() == date);

                }
            }

            ViewBag.tt = tong();
            ViewData["TongTienHD"] = tong();
            ViewData["Tongcxn"] = chuaxn();
            ViewData["Tongdxn"] = daxn();
            ViewData["Tonghuy"] = dahuy();
            return View(tbl_HoaDon.ToList());
        }


        public double tong()
        {
            int tthd = 0;
            List<HOADON> lsHD = db.HOADONs.Where(t => t.TINHTRANG != "Đã Hủy").ToList();
            if (lsHD != null)
            {

                tthd = (int)lsHD.Sum(s => s.TONGTIEN);
            }
            return tthd;

        }

        public int chuaxn()
        {
            int cxn = 0;
            List<HOADON> lsHD = db.HOADONs.ToList();
            if (lsHD != null)
            {
                if (lsHD.Any(s => s.TINHTRANG == "Chưa xác nhận"))
                {
                    cxn = lsHD.Where(s => s.TINHTRANG == "Chưa xác nhận").Count();
                }
            }
            return cxn;

        }

        public int dahuy()
        {
            int dahuy = 0;
            List<HOADON> lsHD = db.HOADONs.ToList();
            if (lsHD != null)
            {
                if (lsHD.Any(s => s.TINHTRANG == "Đã hủy"))
                {
                    dahuy = lsHD.Where(s => s.TINHTRANG == "Đã hủy").Count();
                }
            }
            return dahuy;

        }
        public int daxn()
        {
            int dg = 0;
            List<HOADON> lsHD = db.HOADONs.ToList();
            if (lsHD != null)
            {
                if (lsHD.Any(s => s.TINHTRANG == "Đã giao"))
                {
                    dg = lsHD.Where(s => s.TINHTRANG == "Đã giao").Count();
                }
            }
            return dg;

        }

        // GET: ThongKeDonHang/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HOADON tbl_HoaDon = db.HOADONs.Find(id);
            if (tbl_HoaDon == null)
            {
                return HttpNotFound();
            }
            return View(tbl_HoaDon);
        }

        // GET: ThongKeDonHang/Create
        public ActionResult Create()
        {
            ViewBag.MaKH = new SelectList(db.KHACHHANGs, "MaKH", "TenKH");
            ViewBag.MaNV = new SelectList(db.NHANVIENs, "MaNV", "TenNV");
            return View();
        }

        // POST: ThongKeDonHang/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaHoaDon,NgayLap,MaKH,MaNV,Tinhtrang,TongTien")] HOADON tbl_HoaDon)
        {
            if (ModelState.IsValid)
            {
                db.HOADONs.Add(tbl_HoaDon);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaKH = new SelectList(db.KHACHHANGs, "MaKH", "TenKH", tbl_HoaDon.MAKH);
            ViewBag.MaNV = new SelectList(db.NHANVIENs, "MaNV", "TenNV", tbl_HoaDon.MANV);
            return View(tbl_HoaDon);
        }

        // GET: ThongKeDonHang/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HOADON tbl_HoaDon = db.HOADONs.Find(id);
            if (tbl_HoaDon == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaKH = new SelectList(db.KHACHHANGs, "MaKH", "TenKH", tbl_HoaDon.MAKH);
            ViewBag.MaNV = new SelectList(db.NHANVIENs, "MaNV", "TenNV", tbl_HoaDon.MANV);
            return View(tbl_HoaDon);
        }

        // POST: ThongKeDonHang/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaHoaDon,NgayLap,MaKH,MaNV,Tinhtrang,TongTien")] HOADON tbl_HoaDon)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_HoaDon).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaKH = new SelectList(db.KHACHHANGs, "MaKH", "TenKH", tbl_HoaDon.MAKH);
            ViewBag.MaNV = new SelectList(db.NHANVIENs, "MaNV", "TenNV", tbl_HoaDon.MANV);
            return View(tbl_HoaDon);
        }

        // GET: ThongKeDonHang/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HOADON tbl_HoaDon = db.HOADONs.Find(id);
            if (tbl_HoaDon == null)
            {
                return HttpNotFound();
            }
            return View(tbl_HoaDon);
        }

        // POST: ThongKeDonHang/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            HOADON tbl_HoaDon = db.HOADONs.Find(id);
            db.HOADONs.Remove(tbl_HoaDon);
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
