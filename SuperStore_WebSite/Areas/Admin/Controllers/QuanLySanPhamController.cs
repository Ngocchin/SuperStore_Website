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

        // POST: QuanLySanPham/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MASP,TENSP,MALOAI,MANCC,MAKM,GIABAN,SOLUONGTON,GHICHU,HINHANH,HINHLIENQUAN")] SANPHAM tbl_SanPham, HttpPostedFileBase fileupload, HttpPostedFileBase fileupload1)
        {
            if (ModelState.IsValid)     
            {
                //var fileName = Path.GetFileName(fileupload.FileName);
                //var path = Path.Combine(Server.MapPath("/img"), fileName);
                //fileupload.SaveAs(path);

                //var fileName1 = Path.GetFileName(fileupload1.FileName);
                //var path1 = Path.Combine(Server.MapPath("/img"), fileName1);
                //fileupload1.SaveAs(path1);


                db.SANPHAMs.Add(tbl_SanPham);
                db.SaveChanges();
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
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SANPHAM tbl_SanPham = db.SANPHAMs.Find(id);
            if (tbl_SanPham == null)
            {
                return HttpNotFound();
            }
            return View(tbl_SanPham);
        }

        // POST: QuanLySanPham/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            SANPHAM tbl_SanPham = db.SANPHAMs.Find(id);
            db.SANPHAMs.Remove(tbl_SanPham);
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
