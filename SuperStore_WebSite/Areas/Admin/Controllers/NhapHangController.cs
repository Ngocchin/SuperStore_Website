using SuperStore_WebSite.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace SuperStore_WebSite.Areas.Admin.Controllers
{
    public class NhapHangController : Controller
    {
        QL_BANHANGDIENTUEntities1 db = new QL_BANHANGDIENTUEntities1();
       
        public ActionResult Index()
        {
            if (Session["AlertMessage"] != null)
            {
                ViewBag.AlertMessage = Session["AlertMessage"].ToString();                
                Session.Remove("AlertMessage");
            }
            return View(db.PHIEUNHAPs.ToList());
        } 
        public ActionResult Create()
        {
            PHIEUNHAP pn = new PHIEUNHAP();          
            ViewBag.MANCC = new SelectList(db.NHACUNGCAPs, "MANCC", "TENNCC");            
            return View(pn);
        }        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MAPHNHAP,MANV,MANCC,NGAYLAPPHIEU")] PHIEUNHAP PHIEUNHAP)
        {
            if (ModelState.IsValid) 
            {
                NHANVIEN nv = (NHANVIEN)Session["Account"];
                int maph = db.PHIEUNHAPs.Count() + 1;
                PHIEUNHAP.MAPHNHAP = "PN" + maph.ToString("000");
                PHIEUNHAP.MANV = nv.MANV;
                PHIEUNHAP.NGAYLAPPHIEU = DateTime.Now.Date;
                db.PHIEUNHAPs.Add(PHIEUNHAP);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        Console.WriteLine(error.ErrorMessage);
                    }
                }
                return View(PHIEUNHAP);
            }          
        }
        public ActionResult CreateCTPN(string id, string masp)
        {
            ViewBag.id = id;
            CTPHIEUNHAP pn = new CTPHIEUNHAP();
            ViewBag.CTPN = new SelectList(db.PHIEUNHAPs, "MAPHNHAP", "MAPHNHAP");
            ViewBag.SP = new SelectList(db.SANPHAMs, "MASP", "TENSP");
            return View(pn);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCTPN([Bind(Include = "MAPHNHAP,MASP,SOLUONG,GIANHAP,TONGTIEN")] CTPHIEUNHAP CTPHIEUNHAP, string id )
        {
            if (ModelState.IsValid)
            {
                PHIEUNHAP pn = db.PHIEUNHAPs.Find(id);
                SANPHAM sp = db.SANPHAMs.Find(CTPHIEUNHAP.MASP);
                CTPHIEUNHAP.TONGTIEN = CTPHIEUNHAP.GIANHAP * CTPHIEUNHAP.SOLUONG;
                if (CTPHIEUNHAP.GIANHAP < sp.GIABAN)
                {
                    pn.CTPHIEUNHAPs.Add(CTPHIEUNHAP);
                    if (pn != null)
                    {
                        foreach (var chiTietPN in pn.CTPHIEUNHAPs)
                        {
                            sp = db.SANPHAMs.Find(chiTietPN.MASP);

                            if (sp != null)
                            {
                                if(sp.SOLUONGTON == null)
                                {
                                    sp.SOLUONGTON = chiTietPN.SOLUONG;
                                } 
                                else
                                {
                                    sp.SOLUONGTON += chiTietPN.SOLUONG;
                                }                                    
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
                        return HttpNotFound();
                    }
                }
                else
                {
                    Session["AlertMessage"] = " Giá nhập phải nhỏ hơn giá bán.";
                    return RedirectToAction("Index");
                }    
            }
            else
            {
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        Console.WriteLine(error.ErrorMessage);
                    }
                }
                return View(CTPHIEUNHAP);
            }
        }
        public ActionResult Details(string id)
        {
            var ctpn = db.CTPHIEUNHAPs.Where(t => t.MAPHNHAP == id).ToList();
            if (ctpn == null || ctpn.Count == 0)
            {
                return HttpNotFound();
            }
            return View(ctpn);
        }
    }
}