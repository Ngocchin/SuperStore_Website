using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuperStore_WebSite.Models
{
    public class Giohang
    {
        QL_BANHANGDIENTUEntities1 db = new QL_BANHANGDIENTUEntities1();
        public SANPHAM thongtinSP { get; set; }    
        public int SL { get; set; }
        public double ThanhTien { get => (double)(SL * thongtinSP.GIABAN); }       
        public Giohang(string idSP)
        {
            thongtinSP = db.SANPHAMs.SingleOrDefault(s => s.MASP == idSP);
            SL = 1;
        }
    }
}