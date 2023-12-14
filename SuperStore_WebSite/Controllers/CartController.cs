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
            //var cthd = Session["CTHD"] as List<CTHD>;
            //ViewBag.HTTT = cthd.HINHTHUCTT;
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
        public ActionResult Order()
        {
            try
            {
                List<Giohang> gh = Session["GioHang"] as List<Giohang>;
                KHACHHANG kh = (KHACHHANG)Session["Account"];
                List<Giohang> lstGioHang = LayGioHang();
                HOADON hd = new HOADON();
                //CTHD cthd = new CTHD();
                //List<CTHD> cthd = Session["CTHD"] as List<CTHD>;
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
                db.SaveChanges();
                foreach (var sp in gh.ToList())
                {
                    CTHD ct = new CTHD();
                    ct.MAHD = hd.MAHD;
                    ct.MASP = sp.thongtinSP.MASP;
                    ct.SOLUONG = sp.SL;
                    ct.DONGIA = sp.ThanhTien;
                    //var existingOrder = db.CTHDs.SingleOrDefault(i => i.MAHD == hd.MAHD);
                    //existingOrder.HINHTHUCTT = ct.HINHTHUCTT;
                    //db.SaveChanges();
                    db.CTHDs.Add(ct);

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
            //ViewBag.tb = "Đặt hàng thành công";            
            List<HOADON> hoadons = db.HOADONs.Where(s => s.MAKH == MaKH).ToList();

            if (hoadons == null || hoadons.Count == 0)
            {
                return HttpNotFound();
            }

            return View(hoadons);
        }

        public ActionResult PayMethod()
        {            
            return View();
        }
        [HttpPost]
        public ActionResult Xacnhanthanhtoan(string id)
        {
            CTHD cthd = new CTHD(); 
            HOADON hd = new HOADON();
            if (ModelState.IsValid)
            {
                // Kiểm tra xem có đơn hàng có mã MAHD trùng khớp không
                var existingOrder = db.CTHDs.SingleOrDefault(i => i.MAHD == hd.MAHD);

                if (existingOrder != null)
                {
                    try
                    {
                        // Cập nhật thông tin thanh toán cho đơn hàng tồn tại
                        existingOrder.HINHTHUCTT = cthd.HINHTHUCTT;

                        // Lưu thay đổi xuống cơ sở dữ liệu
                        db.SaveChanges();

                        // Cập nhật thông tin cho HOADON (nếu cần)
                        var hoadon = db.HOADONs.Find();
                        if (hoadon != null)
                        {
                            hoadon.TINHTRANG = "Đã thanh toán";
                            db.SaveChanges();
                        }

                        return RedirectToAction("OrderFollow", "Cart", new { id = cthd.MAHD });
                    }
                    catch
                    {
                        // Xử lý nếu có lỗi khi cập nhật thông tin thanh toán
                        ViewBag.error = "Có lỗi xảy ra khi cập nhật thanh toán";
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    // Xử lý nếu không tìm thấy đơn hàng với mã MAHD tương ứng
                    ViewBag.error = "Không tìm thấy đơn hàng để thanh toán";
                    return RedirectToAction("Index", "Home");
                }
            }

            // Xử lý nếu ModelState.IsValid không hợp lệ (có lỗi kiểm tra dữ liệu)
            return View(cthd);
        }

    }
}