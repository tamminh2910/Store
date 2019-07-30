using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CaptchaMvc.HtmlHelpers;
using CaptchaMvc.Models;

namespace Store.Controllers
{
    public class HomeController : Controller
    {

        BanHangDbContext db = new BanHangDbContext();
        public ActionResult Index()
        {
            var lstDTM = db.SanPhams.Where(x => x.MaLoaiSP == 1 && x.Moi == 1 && x.DaXoa == false).ToList();
            ViewBag.ListDTM = lstDTM;

            var lstMTB = db.SanPhams.Where(x => x.MaLoaiSP == 2 && x.Moi == 1 && x.DaXoa == false).ToList();
            ViewBag.ListMTB = lstMTB;

            var lstLaptop = db.SanPhams.Where(x => x.MaLoaiSP == 3 && x.Moi == 1 && x.DaXoa == false).ToList();
            ViewBag.ListLaptop = lstLaptop;

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult MenuPartial()
        {
            var lstSP = db.SanPhams.ToList();
            return PartialView(lstSP);
        }

        [HttpGet]
        public ActionResult DangKy()
        {
            ViewBag.Success = false;
            ViewBag.CauHoi = new SelectList(LoadCauHoi());
            return View();
        }

        [HttpPost]
        public ActionResult DangKy(ThanhVien tv, FormCollection f)
        {
            ViewBag.Success = false;
            var taikhoan = db.ThanhViens.Where(x => x.TaiKhoan == tv.TaiKhoan);
            if (taikhoan.Count() > 0)
            {
                ModelState.AddModelError("TaiKhoan", "Tài khoản tồn tại. Vui lòng nhập tên tài khoản khác!");
            }
            ViewBag.CauHoi = new SelectList(LoadCauHoi());

            if (this.IsCaptchaValid("Captcha không hợp lệ"))
            {
                if (ModelState.IsValid)
                {
                    ViewBag.ThongBao = "Đăng ký thành công!";
                    ViewBag.Success = true;
                    db.ThanhViens.Add(tv);
                    db.SaveChanges();
                }
                else
                {
                    ViewBag.ThongBao = "Thêm thất bại";
                }
                return View();

            }
            ViewBag.ThongBao = "Sai mã captcha";

            return View();
        }

        public List<string> LoadCauHoi()
        {
            List<string> listCauHoi = new List<string>();
            listCauHoi.Add("Ca sĩ bạn thích nhất là ai?");
            listCauHoi.Add("Món ăn bạn thích ăn nhất là?");
            return listCauHoi;
        }

        public ActionResult DangNhap()
        {

            return View();
        }

        [HttpPost]
        public ActionResult DangNhap(FormCollection f)
        {
            var staikhoan = f["TaiKhoan"].ToString();
            var smatkhau = f["MatKhau"].ToString();

            ThanhVien tk = db.ThanhViens.SingleOrDefault(x => x.TaiKhoan == staikhoan && x.MatKhau == smatkhau);
            if (tk != null)
            {
                Session["TaiKhoan"] = tk;
                var tv = Session["TaiKhoan"] as ThanhVien;
                return RedirectToAction("Index");
            }
            ViewBag.Error = "Tài khoản hoặc mật khẩu không đúng";
            return View();
        }
        public ActionResult DangXuat()
        {
            var tv = Session["TaiKhoan"] as KhachHang;
            Session["TaiKhoan"] = null;
            return RedirectToAction("Index");
        }
    }
}