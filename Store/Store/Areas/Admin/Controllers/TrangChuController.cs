using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Store.Areas.Admin.Controllers
{
    public class TrangChuController : Controller
    {
        BanHangDbContext db = new BanHangDbContext();

        // GET: Admin/TrangChu
        public ActionResult Index()
        {
            ViewBag.SoNguoiOnline = HttpContext.Application["SoNguoiOnline"].ToString();
            ViewBag.SoNguoiGheTham = HttpContext.Application["SoNguoiGheTham"].ToString();
            ViewBag.TongDoanhThu = TongDoanhThu();
            ViewBag.TongDonHang = TongDonHang();


            return View();
        }

        private decimal TongDoanhThu()
        {
            decimal doanhThu = db.ChiTietDonDatHangs.Sum(x => x.SoLuong * x.DonGia).Value;
            return doanhThu;
        }

        private int TongDonHang()
        {
            int donHang = db.DonDatHangs.Where(x => x.DaThanhToan == false && x.TinhTrangGiaoHang == false).Count();
            return donHang;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (db != null)
                {
                    db.Dispose();

                }
            }
            base.Dispose(disposing);
        }

    }
}