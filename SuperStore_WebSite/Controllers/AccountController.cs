using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SuperStore_WebSite.Models;

namespace SuperStore_WebSite.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        QL_BANHANGDIENTUEntities1 db = new QL_BANHANGDIENTUEntities1();
        public ActionResult Inf(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHACHHANG tbl_KhachHang = db.KHACHHANGs.Find(id);
            if (tbl_KhachHang == null)
            {
                return HttpNotFound();
            }
            return View(tbl_KhachHang);
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]        
        public ActionResult Login(FormCollection c)
        {
            string email = c["email"].ToString();
            string Password = c["password"].ToString();
            NHANVIEN nv = db.NHANVIENs.SingleOrDefault(i => i.EMAIL == email && i.PASSWORD == Password);
            KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(i => i.EMAIL == email && i.PASSWORD == Password);
            if (ModelState.IsValid)
            {
                if (nv != null && (nv.CHUCVU == "Nhân viên" || nv.CHUCVU == "Quản lý"))
                {
                    Session["Account"] = nv;
                    return RedirectToAction("Index", "HomeAdmin", new { area = "Admin" });
                }
                else if (kh != null)
                {
                    Session["Account"] = kh;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.error = "Login failed";
                    return RedirectToAction("Login");
                }
            }
            return View();
        }
        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]       
        public ActionResult SignUp(KHACHHANG kh, FormCollection c)
        {
            if (kh.PASSWORD == c["repass"])
            {
                if (ModelState.IsValid)
                {
                    kh.EMAIL = c["EMAIL"].ToString();
                    var existingCustomer = db.KHACHHANGs.FirstOrDefault(x => x.EMAIL == kh.EMAIL);
                    if (existingCustomer != null)
                    {
                        ModelState.AddModelError("EMAIL", "Email đã tồn tại");
                        return View(kh);
                    }
                    else if (kh != null)
                    {
                        int nextId = db.KHACHHANGs.Count() + 1;
                        kh.MAKH = "KH00" + nextId.ToString();
                        db.KHACHHANGs.Add(kh);
                        db.SaveChanges();
                        return RedirectToAction("Login", "Account");
                    }
                    else
                    {
                        ViewBag.error = "Sign Up failed";
                        return RedirectToAction("SignUp");
                    }
                }
            }
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}