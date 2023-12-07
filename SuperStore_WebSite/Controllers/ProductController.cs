using SuperStore_WebSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SuperStore_WebSite.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        QL_BANHANGDIENTUEntities1 db = new QL_BANHANGDIENTUEntities1();
        public ActionResult ShowAll()
        {
            List<SANPHAM> sanPhams = db.SANPHAMs.ToList();

            return View(sanPhams);
        }
        public ActionResult Category(string Maloai)
        {
            var listproduct = db.SANPHAMs.Where(s => s.MALOAI == Maloai).ToList();

            if (listproduct.Count == 0)
            {
                ViewBag.TB = "Không tìm thấy sản phẩm";
            }
            else
            {
                string ten = db.LOAIs.Where(s => s.MALOAI == Maloai).ToString();
                ViewBag.LoaiSP = ten;
            }
            return View(listproduct);
        }
        public ActionResult Details(string MaSP)
        {
            var sanPham = db.SANPHAMs.SingleOrDefault(t => t.MASP == MaSP);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }
        public ActionResult Search(String search)
        {
            var model = db.SANPHAMs.Where(p => p.TENSP.Contains(search));
            return View(model.OrderBy(n => n.TENSP).ToList());
        }
    }
}