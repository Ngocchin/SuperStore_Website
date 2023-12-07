using SuperStore_WebSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SuperStore_WebSite.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        QL_BANHANGDIENTUEntities1 db = new QL_BANHANGDIENTUEntities1();
        public ActionResult Index()
        {
            var model = db.SANPHAMs.Where(p => p.MAKM != null).Take(6).OrderBy(p => p.MASP);
            ViewBag.Tasks = model;

            //var model2 = db.SANPHAMs.Take(6).OrderByDescending(p => p.MASP);
            //ViewBag.Tasks2 = model2;

            return View(model);
        }
        public ActionResult Category()
        {
            var model = db.LOAIs.OrderByDescending(p => p.TENLOAI);
            return PartialView(model);
        }
    }
}