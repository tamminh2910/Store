using Entities;
using Store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
namespace Store.Controllers
{
    public class GioHangController : Controller
    {
        BanHangDbContext db = new BanHangDbContext();
        //Lấy giỏ hàng
        public List<ItemGioHang> LayGioHang()
        {
            //Giỏ hàng tồn tại
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null)
            {
                lstGioHang = new List<ItemGioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }
        // GET: GioHang
        public ActionResult XemGioHang()
        {
            List<ItemGioHang> lstGH = LayGioHang();
            return View(lstGH);
        }
        public int TongSoLuong()
        {
            var lstGH = LayGioHang(); ;
            int tongSoLuong = 0;
            if (lstGH.Count > 0)
            {
                tongSoLuong = lstGH.Sum(x => x.SoLuong);
            }
            return tongSoLuong;
        }
        public decimal TongSoTien()
        {
            List<ItemGioHang> lstGH = LayGioHang();
            decimal tongSoTien = 0;
            if (lstGH.Count > 0)
            {
                tongSoTien = lstGH.Sum(x => x.ThanhTien);
            }
            return tongSoTien;
        }
        public ActionResult GioHangPartial()
        {
            if (TongSoTien() == 0)
            {
                ViewBag.TongSoTien = 0;
                ViewBag.TongSoLuong = 0;
                return PartialView();
            }
            ViewBag.TongSoTien = TongSoTien();
            ViewBag.TongSoLuong = TongSoLuong();
            return PartialView();

        }
        [HttpPost]
        public ActionResult ThemGioHang(int maSP)
        {
            var sp = db.SanPhams.SingleOrDefault(x => x.MaSP == maSP);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Lay gio hang
            List<ItemGioHang> lstGioHang = LayGioHang();
            //Trường hợp 1 sản phẩm đã tồn tại trong giỏ hàng
            var spCheck = lstGioHang.SingleOrDefault(x => x.MaSP == maSP);
            if (spCheck != null)
            {
                if (sp.SoLuongTon < spCheck.SoLuong)
                {
                    return Content("<script>alert('Sản phẩm hết hàng!')</script>");
                }
                spCheck.SoLuong++;
                spCheck.ThanhTien = spCheck.SoLuong * spCheck.DonGia;
                ViewBag.TongSoTien = TongSoTien();
                ViewBag.TongSoLuong = TongSoLuong();
                return PartialView("GioHangPartial");
            }
            var itemGH = new ItemGioHang(maSP);
            if (sp.SoLuongTon < itemGH.SoLuong)
            {
                return Content("<script>alert('Sản phẩm hết hàng!')</script>");
            }
            lstGioHang.Add(itemGH);
            ViewBag.TongSoTien = TongSoTien();
            ViewBag.TongSoLuong = TongSoLuong();
            return PartialView("GioHangPartial");
        }

        public ActionResult ThemGioHang1(int id)
        {
            var sp = db.SanPhams.SingleOrDefault(x => x.MaSP == id);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Lay gio hang
            List<ItemGioHang> lstGioHang = LayGioHang();
            //Trường hợp 1 sản phẩm đã tồn tại trong giỏ hàng
            var spCheck = lstGioHang.SingleOrDefault(x => x.MaSP == id);
            if (spCheck != null)
            {
                if (sp.SoLuongTon < spCheck.SoLuong)
                {
                    return Content("<script>alert('Sản phẩm hết hàng!')</script>");
                }
                spCheck.SoLuong++;
                spCheck.ThanhTien = spCheck.SoLuong * spCheck.DonGia;
                ViewBag.TongSoTien = TongSoTien();
                ViewBag.TongSoLuong = TongSoLuong();
                return RedirectToAction("XemChiTiet", "SanPham", new { id });
            }
            var itemGH = new ItemGioHang(id);
            if (sp.SoLuongTon < itemGH.SoLuong)
            {
                return Content("<script>alert('Sản phẩm hết hàng!')</script>");
            }
            lstGioHang.Add(itemGH);

            return RedirectToAction("XemChiTiet", "SanPham", new { id });
        }

        [HttpPost]
        public ActionResult CapNhatGioHang(int maSP, int sl)
        {
            var lstGH = Session["GioHang"] as List<ItemGioHang>;
            var sp = lstGH.SingleOrDefault(x => x.MaSP == maSP);
            sp.SoLuong = sl;
            sp.ThanhTien = sp.SoLuong * sp.DonGia;

            return PartialView("XemGioHangPartial", lstGH);
        }

        [HttpPost]
        public ActionResult XoaGioHangAjax(int maSP)
        {
            var lstGH = Session["GioHang"] as List<ItemGioHang>;
            if (lstGH == null)
            {
                return RedirectToAction("Index","Home");
            }
            var sp = lstGH.SingleOrDefault(x => x.MaSP == maSP);

            lstGH.Remove(sp);
            if (lstGH.Count == 0)
            {
                return RedirectToAction("Index","Home");
            }
            return PartialView("XemGioHangPartial", lstGH);
        }

        [HttpGet]
        public ActionResult XoaGioHang(int maSP)
        {
            var lstGH = Session["GioHang"] as List<ItemGioHang>;
            if (lstGH == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var sp = lstGH.SingleOrDefault(x => x.MaSP == maSP);

            lstGH.Remove(sp);
            if (lstGH.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            return View("XemGioHang", lstGH);
        }

        [HttpPost]
        public ActionResult ThanhToan(FormCollection f)
        {
            var tv = Session["TaiKhoan"] as ThanhVien;

            KhachHang kh = new KhachHang();
            kh.MaThanhVien = tv.MaThanhVien;
            kh.SoDienThoai = f["SoDienThoai"].ToString();
            kh.TenKH = f["HoTen"].ToString();
            kh.Email = f["Email"].ToString();
            kh.DiaChi = f["DiaChi"].ToString();
            db.KhachHangs.Add(kh);
            db.SaveChanges();

            List<ItemGioHang> lstGH = LayGioHang();
            DonDatHang donDatHang = new DonDatHang();
            donDatHang.NgayDat = DateTime.Now;
            donDatHang.TinhTrangGiaoHang = false;
            donDatHang.DaThanhToan = false;
            donDatHang.MaKH = kh.MaKH;

            db.DonDatHangs.Add(donDatHang);
            db.SaveChanges();
            List<ChiTietDonDatHang> lstCTDDH = new List<ChiTietDonDatHang>();
            foreach (var item in lstGH)
            {
                ChiTietDonDatHang ctddh = new ChiTietDonDatHang();
                ctddh.MaDDH = donDatHang.MaDDH;
                ctddh.MaSP = item.MaSP;
                ctddh.TenSP = item.TenSP;
                ctddh.SoLuong = item.SoLuong;
                ctddh.DonGia = item.DonGia;
                lstCTDDH.Add(ctddh);
            }
            db.ChiTietDonDatHangs.AddRange(lstCTDDH);
            db.SaveChanges();
            ViewBag.ThongBao = true;
            Session["GioHang"] = null;
            return RedirectToAction("XemGioHang");
        }
    }
}