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
    public class HomeAdminController : Controller
    {
        QL_BANHANGDIENTUEntities1 db = new QL_BANHANGDIENTUEntities1();
        // GET: Admin/HomeAdmin
        public ActionResult Index()
        {
            var tbl_HoaDon = db.HOADONs.Include(t => t.KHACHHANG).Include(t => t.NHANVIEN);         
            ViewBag.tt = tong();
            ViewData["TongTienHD"] = tong();
            ViewData["Tongcxn"] = chuaxn();
            ViewData["Tongdxn"] = daxn();
            return View(tbl_HoaDon.ToList());
        }
        public double tong()
        {
            int tthd = 0;
            List<HOADON> lsHD = db.HOADONs.ToList();
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
    }
}