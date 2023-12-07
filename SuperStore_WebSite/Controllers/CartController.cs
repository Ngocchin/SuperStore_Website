using SuperStore_WebSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SuperStore_WebSite.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        QL_BANHANGDIENTUEntities1 db = new QL_BANHANGDIENTUEntities1();
        public ActionResult Index()
        {
            var lst = Session["GioHang"] as List<Giohang>;
            if (lst == null)
            {
                ViewBag.TongCong = 0;
                ViewBag.TongTien = 0;
            }
            else
            {
                ViewBag.TongCong = lst.Sum(x => x.SL);
                ViewBag.TongTien = lst.Sum(x => x.ThanhTien);
            }

            return View(LayGioHang());
        }
        private double tongThanhTien()
        {
            double tt = 0;
            List<Giohang> lstGioHang = Session["GioHang"] as List<Giohang>;
            if (lstGioHang != null)
            {
                tt = lstGioHang.Sum(sp => sp.ThanhTien);
            }
            return tt;
        }
        public List<Giohang> LayGioHang()
        {
            List<Giohang> lstGH = Session["GioHang"] as List<Giohang>;
            if (lstGH == null)
            {
                lstGH = new List<Giohang>();
                Session["GioHang"] = lstGH;
            }
            return lstGH;
        }
        public ActionResult GioHangPartial()
        {
            Session["SoLuong"] = LayGioHang().Sum(x => x.SL);
            return PartialView();
        }
        public ActionResult ThemGioHang(string idSP, string strURL)
        {
            List<Giohang> lstGH = LayGioHang();
            Giohang sp = lstGH.Find(s => s.thongtinSP.MASP == idSP);
            if (sp == null)
            {
                sp = new Giohang(idSP);
                lstGH.Add(sp);
                return Redirect(strURL);
            }
            else
            {
                sp.SL++;
                return Redirect(strURL);
            }
        }
        public ActionResult CapNhatGioHang(string id, int sl)
        {
            Giohang sp = LayGioHang().SingleOrDefault(s => s.thongtinSP.MASP.Equals(id));
            if (sp != null)
            {
                sp.SL = sl;
            }
            return RedirectToAction("Index", "Cart");
        }
        public ActionResult XacNhanThongTin()
        {
            KHACHHANG kh = Session["Account"] as KHACHHANG;
            if (kh == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View(kh);
        }
        [HttpPost]
        public ActionResult Order()
        {
            try
            {
                List<Giohang> gh = Session["GioHang"] as List<Giohang>;
                KHACHHANG kh = (KHACHHANG)Session["Account"];
                List<Giohang> lstGioHang = LayGioHang();
                HOADON hd = new HOADON();
                int mahd = db.HOADONs.Count() + 1;
                int macthd = db.CTHDs.Count() + 1;
                hd.MAHD = "HD" + mahd.ToString("000");
                hd.MAHD = "HD" + macthd.ToString("000");
                hd.MAKH = kh.MAKH;

                hd.MANV = null;
                hd.TINHTRANG = "Chưa xác nhận";
                hd.NGAYLAP = DateTime.Now;
                hd.TONGTIEN = ViewBag.TongTien = tongThanhTien();
                db.HOADONs.Add(hd);
                db.SaveChanges();
                foreach (var sp in gh.ToList())
                {
                    CTHD ct = new CTHD();
                    ct.MAHD = hd.MAHD;
                    ct.MASP = sp.thongtinSP.MASP;
                    ct.SOLUONG = sp.SL;
                    ct.DONGIA = sp.ThanhTien;
                    db.CTHDs.Add(ct);
                }
                db.SaveChanges();
                ViewBag.tb = "Đặt hàng thành công";
                gh.Clear();
                return View();
            }
            catch
            {
                ViewBag.tb = "Đặt hàng không thành công";
                return View();
            }
        }
    }
}