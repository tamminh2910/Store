using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Entities;

namespace Store.Areas.Admin.Controllers
{
    public class QLPhieuNhapController : Controller
    {
        private BanHangDbContext db = new BanHangDbContext();

        // GET: Admin/QLPhieuNhap
        [HttpGet]
        public ActionResult NhapHang()
        {
            ViewBag.MaNCC = db.NhaCungCaps;
            ViewBag.ListSanPham = db.SanPhams;
            return View();
        }
        [HttpPost]
        public ActionResult NhapHang(PhieuNhap model, IEnumerable<ChiTietPhieuNhap> lstModel)
        {
            model.DaXoa = false;
            model.NgayNhap = DateTime.Now;
            db.PhieuNhaps.Add(model);
            if (lstModel != null)
            {
                foreach (var item in lstModel)
                {
                    item.MaPN = model.MaPN;
                    var sp = db.SanPhams.SingleOrDefault(x => x.MaSP == item.MaSP);
                    if (sp != null)
                    {
                        sp.SoLuongTon += item.SoLuongNhap;
                    }
                }
                db.ChiTietPhieuNhaps.AddRange(lstModel);
              
            }
            
            ViewBag.MaNCC = db.NhaCungCaps;
            ViewBag.ListSanPham = db.SanPhams;
            return View();
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
