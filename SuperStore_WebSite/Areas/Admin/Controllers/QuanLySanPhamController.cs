using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using SuperStore_WebSite.Models;

namespace SuperStore_WebSite.Areas.Admin.Controllers
{
    public class QuanLySanPhamController : Controller
    {
        private QL_BANHANGDIENTUEntities1 db = new QL_BANHANGDIENTUEntities1();

        // GET: QuanLySanPham
        public ActionResult Index()
        {
            var tbl_SanPham = db.SANPHAMs.Include(t => t.LOAI).Include(t => t.NHACUNGCAP);
            return View(tbl_SanPham.ToList());
        }

        public ActionResult Sort(string sortOrder)
        {
            var tbl_SanPham = db.SANPHAMs.Include(t => t.LOAI).Include(t => t.NHACUNGCAP);

            switch (sortOrder)
            {
                case "AdenZ":
                    return View("Index", tbl_SanPham.OrderBy(t => t.TENSP).ToList());
                case "ZdenA":
                    return View("Index", tbl_SanPham.OrderByDescending(t => t.TENSP).ToList());
                case "GiaTang":
                    return View("Index", tbl_SanPham.OrderBy(t => t.GIABAN).ToList());
                case "GiaGiam":
                    return View("Index", tbl_SanPham.OrderByDescending(t => t.GIABAN).ToList());
                case "KM":
                    return View("Index", tbl_SanPham.Where(t => t.MAKM != null).ToList());
                case "NonKM":
                    return View("Index", tbl_SanPham.Where(t => t.MAKM == null).ToList());
                default:
                    return View("Index", tbl_SanPham.ToList());
            }
        }

        public ActionResult KM()
        {
            var tbl_SanPham = db.SANPHAMs.Include(t => t.LOAI).Include(t => t.NHACUNGCAP);
            return View(tbl_SanPham.Where(t=>t.MAKM != null).ToList());
        }
        public ActionResult NonKM()
        {
            var tbl_SanPham = db.SANPHAMs.Include(t => t.LOAI).Include(t => t.NHACUNGCAP);
            return View(tbl_SanPham.Where(t => t.MAKM == null).ToList());
        }

        public ActionResult TimKiem(string tukhoa)
        {
            var tbl_SanPham = db.SANPHAMs.Where(t=>t.TENSP.Contains(tukhoa));
            return View(tbl_SanPham.OrderBy(n=>n.TENSP).ToList());
        }

        public ActionResult AdenZ()
        {
            var tbl_SanPham = db.SANPHAMs.Include(t => t.LOAI).Include(t => t.NHACUNGCAP);
            return View(tbl_SanPham.OrderBy(t=>t.TENSP).ToList());
        }

        public ActionResult ZdenA()
        {
            var tbl_SanPham = db.SANPHAMs.Include(t => t.LOAI).Include(t => t.NHACUNGCAP);
            return View(tbl_SanPham.OrderByDescending(t => t.TENSP).ToList());
        }


        public ActionResult GiaTang()
        {
            var tbl_SanPham = db.SANPHAMs.Include(t => t.LOAI).Include(t => t.NHACUNGCAP);
            return View(tbl_SanPham.OrderBy(t => t.GIABAN).ToList());
        }


        public ActionResult GiaGiam()
        {
            var tbl_SanPham = db.SANPHAMs.Include(t => t.LOAI).Include(t => t.NHACUNGCAP);
            return View(tbl_SanPham.OrderByDescending(t => t.GIABAN).ToList());
        }
        // GET: QuanLySanPham/Details/5


        // GET: QuanLySanPham/Create
        public ActionResult Create()
        {
            ViewBag.MaLoai = new SelectList(db.LOAIs, "MaLoai", "TenLoai");
            ViewBag.MaNCC = new SelectList(db.NHACUNGCAPs, "MaNCC", "TenNCC");
            ViewBag.MaKM = new SelectList(db.KHUYENMAIs, "MaKM", "MaKM");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MASP,TENSP,MALOAI,MANCC,MAKM,GIABAN,SOLUONGTON,GHICHU,HINHANH,HINHLIENQUAN")] SANPHAM tbl_SanPham, HttpPostedFileBase fileupload, HttpPostedFileBase fileupload1)
        {
            if (ModelState.IsValid)
            {
                db.SANPHAMs.Add(tbl_SanPham);
                db.SaveChanges();

                ViewBag.ShowSuccessToast = true;
                return RedirectToAction("Index");
            }

            ViewBag.MaLoai = new SelectList(db.LOAIs, "MaLoai", "TenLoai", tbl_SanPham.MALOAI);
            ViewBag.MaNCC = new SelectList(db.NHACUNGCAPs, "MaNCC", "TenNCC", tbl_SanPham.MANCC);
            ViewBag.MaKM = new SelectList(db.KHUYENMAIs, "MaKM", "MaKM", tbl_SanPham.MAKM);
            return View(tbl_SanPham);
        }

        // GET: QuanLySanPham/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SANPHAM  tbl_SanPham = db.SANPHAMs.Find(id);
            if (tbl_SanPham == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaLoai = new SelectList(db.LOAIs, "MaLoai", "TenLoai", tbl_SanPham.MALOAI);
            ViewBag.MaNCC = new SelectList(db.NHACUNGCAPs, "MaNCC", "TenNCC", tbl_SanPham.MANCC);
            ViewBag.MaKM = new SelectList(db.KHUYENMAIs, "MaKM", "MaKM", tbl_SanPham.MAKM);
            return View(tbl_SanPham);
        }

        // POST: QuanLySanPham/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MASP,TENSP,MALOAI,MANCC,MAKM,GIABAN,SOLUONGTON,GHICHU,HINHANH,HINHLIENQUAN")] SANPHAM tbl_SanPham)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_SanPham).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaLoai = new SelectList(db.LOAIs, "MaLoai", "TenLoai", tbl_SanPham.MALOAI);
            ViewBag.MaNCC = new SelectList(db.NHACUNGCAPs, "MaNCC", "TenNCC", tbl_SanPham.MANCC);
            ViewBag.MaKM = new SelectList(db.KHUYENMAIs, "MaKM", "MaKM", tbl_SanPham.MAKM);
            return View(tbl_SanPham);
        }

        // GET: QuanLySanPham/Delete/5
        // GET: Product/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SANPHAM product = db.SANPHAMs.Find(id);

            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            SANPHAM product = db.SANPHAMs.Find(id);

            if (product == null)
            {
                return HttpNotFound();
            }

            // Gửi thông báo trước khi xóa
            TempData["Message"] = $"Sản phẩm '{product.TENSP}' đã được xóa thành công.";

            db.SANPHAMs.Remove(product);
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
