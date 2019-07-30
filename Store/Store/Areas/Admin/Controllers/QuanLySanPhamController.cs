using Entities;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Store.Areas.Admin.Controllers
{
    public class QuanLySanPhamController : Controller
    {
        private BanHangDbContext db = new BanHangDbContext();

        // GET: Admin/QuanLySanPham
        public ActionResult DanhSachSanPham(int? page, string searchString, string loaiSP)
        {
            int maLoaiSP;
            int.TryParse(loaiSP, out maLoaiSP);
            ViewBag.CurrentFilter = searchString;
            var sanPhams = db.SanPhams.ToList();
            ViewBag.LoaiSP = new SelectList(DSLoaiSanPham(), "Value", "Text");
            if (!string.IsNullOrEmpty(searchString))
            {
                if (maLoaiSP != 0)
                {
                    sanPhams = db.SanPhams.Where(x => x.TenSP.Contains(searchString) && x.MaLoaiSP == maLoaiSP).ToList();
                }
                else
                {
                    sanPhams = db.SanPhams.Where(x => x.TenSP.Contains(searchString)).ToList();
                }
            }
            else
            {
                if (maLoaiSP != 0)
                {
                    sanPhams = db.SanPhams.Where(x => x.MaLoaiSP == maLoaiSP).ToList();
                }
            }

            TempData["page"] = new { page, searchString, loaiSP };
           

            ViewBag.LoaiSP = new SelectList(DSLoaiSanPham(), "Value", "Text");

            ViewBag.MaLoaiSP = maLoaiSP;
            ViewBag.Page = page;
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(sanPhams.OrderBy(x => x.MaSP).ToPagedList(pageNumber, pageSize));
        }

        public List<SelectListItem> DSLoaiSanPham()
        {
            var dsLoaiSP = db.LoaiSanPhams.ToList();
            List<SelectListItem> listLoaiSP = new List<SelectListItem>();
            listLoaiSP.Add(new SelectListItem() { Value = "0", Text = "Tất cả" });
            foreach (var item in dsLoaiSP)
            {
                listLoaiSP.Add(new SelectListItem() { Text = item.TenLoai, Value = item.MaLoaiSP.ToString() });
            }
            return listLoaiSP;
        }

        public ActionResult XoaSanPham(int? id, int? page, string searchString, string loaiSP)
        {
            var sp = db.SanPhams.FirstOrDefault(x => x.MaSP == id);
            if (sp == null)
            {
                return Content("<alert>Không tồn tại sản phẩm!</alert>");
            }
            else
            {
                db.SanPhams.Remove(sp);
                db.SaveChanges();
                return RedirectToAction("DanhSachSanPham", new { page, searchString, loaiSP });
            }
        }

        public ActionResult LocLoaiSanPham(int loaiSP, int? page)
        {
            var dssp = db.SanPhams.Where(x => x.MaLoaiSP == loaiSP).ToList();
            if (dssp.Count == 0)
            {
                dssp = db.SanPhams.ToList();
            }
            ViewBag.LoaiSP = new SelectList(DSLoaiSanPham(), "Value", "Text");
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View("DanhSachSanPham", dssp.OrderBy(x => x.MaSP).ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public string ProcessUpload(HttpPostedFileBase file)
        {
            file.SaveAs(Server.MapPath("~/Content/HinhAnhSP/" + file.FileName));
            return file.FileName;
        }

        // GET: Admin/QuanLySanPham/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // GET: Admin/QuanLySanPham/Create
        public ActionResult Create()
        {
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams, "MaLoaiSP", "TenLoai");
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps, "MaNCC", "TenNCC");
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats, "MaNSX", "TenNSX");
            return View();
        }

        // POST: Admin/QuanLySanPham/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaSP,TenSP,DonGia,NgayCapNhap,CauHinh,MoTa,HinhAnh,SoLuongTon,LuotXem,LuotBinhChon,LuotBinhLuan,SoLanMua,Moi,MaNCC,MaNSX,MaLoaiSP,DaXoa,HinhAnh1,HinhAnh2,HinhAnh3,HinhAnh4")] SanPham sanPham)
        {
            if (sanPham.DonGia == null)
            {
                ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams, "MaLoaiSP", "TenLoai", sanPham.MaLoaiSP);
                ViewBag.MaNCC = new SelectList(db.NhaCungCaps, "MaNCC", "TenNCC", sanPham.MaNCC);
                ViewBag.MaNSX = new SelectList(db.NhaSanXuats, "MaNSX", "TenNSX", sanPham.MaNSX);
                ModelState.AddModelError("DonGia", "Vui lòng nhập giá bán");
                return View(sanPham);
            }
            if (ModelState.IsValid)
            {
                if (sanPham.SoLuongTon == null)
                {
                    sanPham.SoLuongTon = 0;
                }

                sanPham.NgayCapNhap = DateTime.Now;
                sanPham.LuotBinhChon = 0;
                sanPham.LuotBinhLuan = 0;
                sanPham.LuotXem = 0;
                sanPham.SoLanMua = 0;
                sanPham.Moi = 1;
                sanPham.DaXoa = false;
                db.SanPhams.Add(sanPham);
                db.SaveChanges();
                return RedirectToAction("DanhSachSanPham", "QuanLySanPham");
            }
            else
            {
                ModelState["DonGia"].Errors.Clear();
                ModelState.AddModelError("DonGia", "Giá trị không hợp lệ");
            }
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams, "MaLoaiSP", "TenLoai", sanPham.MaLoaiSP);
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps, "MaNCC", "TenNCC", sanPham.MaNCC);
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats, "MaNSX", "TenNSX", sanPham.MaNSX);
            return View(sanPham);
        }

        // GET: Admin/QuanLySanPham/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams, "MaLoaiSP", "TenLoai", sanPham.MaLoaiSP);
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps, "MaNCC", "TenNCC", sanPham.MaNCC);
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats, "MaNSX", "TenNSX", sanPham.MaNSX);
            return View(sanPham);
        }

        // POST: Admin/QuanLySanPham/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaSP,TenSP,DonGia,NgayCapNhap,CauHinh,MoTa,HinhAnh,SoLuongTon,LuotXem,LuotBinhChon,LuotBinhLuan,SoLanMua,Moi,MaNCC,MaNSX,MaLoaiSP,DaXoa,HinhAnh1,HinhAnh2,HinhAnh3,HinhAnh4")] SanPham sanPham)
        {
            if (sanPham.DonGia == null)
            {
                ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams, "MaLoaiSP", "TenLoai", sanPham.MaLoaiSP);
                ViewBag.MaNCC = new SelectList(db.NhaCungCaps, "MaNCC", "TenNCC", sanPham.MaNCC);
                ViewBag.MaNSX = new SelectList(db.NhaSanXuats, "MaNSX", "TenNSX", sanPham.MaNSX);
                ModelState.AddModelError("DonGia", "Vui lòng nhập giá bán");
                return View(sanPham);
            }
            if (ModelState.IsValid)
            {
                db.Entry(sanPham).State = EntityState.Modified;
                db.SaveChanges();
                var obj = TempData["page"];
                return RedirectToAction("DanhSachSanPham", obj);
            }
            else
            {
                ModelState["DonGia"].Errors.Clear();
                ModelState.AddModelError("DonGia", "Giá trị không hợp lệ");
            }
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams, "MaLoaiSP", "TenLoai", sanPham.MaLoaiSP);
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps, "MaNCC", "TenNCC", sanPham.MaNCC);
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats, "MaNSX", "TenNSX", sanPham.MaNSX);
            return View(sanPham);
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