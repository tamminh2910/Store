using Entities;
using PagedList;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Store.Controllers
{
    public class SanPhamController : Controller
    {
        // GET: SanPham
        private BanHangDbContext db = new BanHangDbContext();

        [ChildActionOnly]
        public ActionResult SanPhamStyle1Partial()
        {
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult SanPhamStyle2Partial()
        {
            return PartialView();
        }

        public ActionResult XemChiTiet(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sp = db.SanPhams.SingleOrDefault(x => x.MaSP == id);
            if (sp == null)
            {
                return HttpNotFound();
            }

            return View(sp);
        }

        public ActionResult SanPham(int? maNSX, int? maLoaiSP, int? page, string tuKhoa)
        {
            int pageSize = 9;
            int pageNumber = (page ?? 1);
            var lstSP = db.SanPhams.ToList();
            if (string.IsNullOrEmpty(tuKhoa))
            {
                if (maNSX == null && maLoaiSP != null)
                {
                    lstSP = db.SanPhams.Where(x => x.MaLoaiSP == maLoaiSP).ToList();
                    ViewBag.MaLoaiSP = maLoaiSP;
                }
                if (maLoaiSP != null && maNSX != null)
                {
                    lstSP = db.SanPhams.Where(x => x.MaLoaiSP == maLoaiSP && x.MaNSX == maNSX).ToList();
                    if (lstSP.Count() == 0)
                    {
                        return HttpNotFound();
                    }
                    ViewBag.MaLoaiSP = maLoaiSP;
                    ViewBag.MaNSX = maNSX;
                }
            }
            else
            {
                ViewBag.TuKhoa = tuKhoa;
                if (maNSX == null && maLoaiSP == null)
                {
                    lstSP = db.SanPhams.Where(x => x.TenSP.Contains(tuKhoa)).ToList();
                }
                if (maNSX == null && maLoaiSP != null)
                {
                    lstSP = db.SanPhams.Where(x => x.MaLoaiSP == maLoaiSP && x.TenSP.Contains(tuKhoa)).ToList();
                    ViewBag.MaLoaiSP = maLoaiSP;
                }
                if (maLoaiSP != null && maNSX != null)
                {
                    lstSP = db.SanPhams.Where(x => x.MaLoaiSP == maLoaiSP && x.MaNSX == maNSX && x.TenSP.Contains(tuKhoa)).ToList();
                    if (lstSP.Count() == 0)
                    {
                        return HttpNotFound();
                    }
                    ViewBag.MaLoaiSP = maLoaiSP;
                    ViewBag.MaNSX = maNSX;
                }
            }

            return View(lstSP.OrderBy(x => x.MaSP).ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult TimKiemSanPham(int? page, string tuKhoa)
        {
            var lstSP = db.SanPhams.ToList();
            if (!string.IsNullOrEmpty(tuKhoa))
            {
                lstSP = db.SanPhams.Where(x => x.TenSP.Contains(tuKhoa)).ToList();
            }
            int pageSize = 9;
            int pageNumber = (page ?? 1);
            ViewBag.TuKhoa = tuKhoa;
            return View("SanPham", lstSP.OrderBy(x => x.MaSP).ToPagedList(pageNumber, pageSize));
        }

        public ActionResult TagSearchPartial()
        {
            var nsx = db.NhaSanXuats.ToList();
            return PartialView(nsx);
        }

        public ActionResult TagSearch(int? maNSX, int? page)
        {
            int pageSize = 9;
            int pageNumber = (page ?? 1);
            var sp = db.SanPhams.Where(x => x.MaNSX == maNSX).OrderBy(x=>x.MaSP).ToPagedList(pageNumber,pageSize);

            return View("SanPham",sp);
        }
    }
}