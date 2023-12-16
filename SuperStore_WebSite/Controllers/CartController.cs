using SuperStore_WebSite.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
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
            try
            {
                Giohang sp = LayGioHang().SingleOrDefault(s => s.thongtinSP.MASP.Equals(id));
                if (sp != null)
                {
                    sp.SL = sl;
                }
                ViewBag.SuccessMessage = "Cập nhật thành công";
                return RedirectToAction("Index", "Cart");
            }
            catch
            {
                ViewBag.SuccessMessage = null;
                return RedirectToAction("Index", "Cart");
            }
        }
        public ActionResult XoaGioHang(string id)
        {
            try
            {
                var lstGioHang = LayGioHang();
                Giohang sp = lstGioHang.Single(s => s.thongtinSP.MASP.Equals(id));
                if (sp != null)
                {
                    lstGioHang.RemoveAll(x => x.thongtinSP.MASP.Equals(id));
                    return RedirectToAction("Index", "Cart");
                }
                if (lstGioHang.Count == 0)
                {
                    return RedirectToAction("ShowAll", "Product");
                }
                ViewBag.SuccessMessage = "Xóa sản phẩm khỏi giỏ hàng thành công";
                return RedirectToAction("Index", "Cart");
            }
            catch
            {
                ViewBag.SuccessMessage = null;
                return RedirectToAction("Index", "Cart");
            }
        }
        public ActionResult XoaAllGioHang()
        {
            try
            {
                LayGioHang().Clear();
                ViewBag.SuccessMessage = "Xóa sản phẩm khỏi giỏ hàng thành công";
                return RedirectToAction("Index", "Cart");
            }
            catch
            {
                ViewBag.SuccessMessage = null;
                return RedirectToAction("Index", "Cart");
            }
        }
        public ActionResult XacNhanThongTin()
        {
            KHACHHANG kh = Session["Account"] as KHACHHANG;            
            if (kh == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var lst = Session["GioHang"] as List<Giohang>;
            if (lst == null)
            {
                ViewBag.TongTien = 0;
            }
            else
            {
                ViewBag.TongTien = lst.Sum(x => x.ThanhTien);
            }
            return View(kh);
        }
        [HttpPost]
        public ActionResult Order(string paymentMethod)
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
                hd.NGAYLAP = DateTime.Now.Date;
                hd.TONGTIEN = ViewBag.TongTien = tongThanhTien();
                db.HOADONs.Add(hd);                
                foreach (var sp in gh.ToList())
                {
                    CTHD ct = new CTHD();
                    ct.MAHD = hd.MAHD;
                    ct.MASP = sp.thongtinSP.MASP;
                    ct.SOLUONG = sp.SL;
                    ct.DONGIA = sp.ThanhTien;                   
                    db.CTHDs.Add(ct);
                    ct.HINHTHUCTT = paymentMethod;
                }
                db.SaveChanges();
                gh.Clear();
                ViewBag.tb = "Đặt hàng thành công";
                return View();              
            }
            catch
            {
                ViewBag.tb = "Đặt hàng không thành công";
                return View();
            }
        }
        public ActionResult OrderFollow(string MaKH)
        {       
            List<HOADON> hoadons = db.HOADONs.Where(s => s.MAKH == MaKH).ToList();

            if (hoadons == null || hoadons.Count == 0)
            {
                return HttpNotFound();
            }

            return View(hoadons);
        }       

    }
}