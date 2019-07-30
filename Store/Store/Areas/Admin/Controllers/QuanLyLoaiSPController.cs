using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Entities;
using PagedList;

namespace Store.Areas.Admin.Controllers
{
    public class QuanLyLoaiSPController : Controller
    {
        private BanHangDbContext db = new BanHangDbContext();

        // GET: Admin/QuanLyLoaiSP
        public ActionResult Index(int? page, string searchString)
        {
            ViewBag.CurrentFilter = searchString;
            var loaiSPs = db.LoaiSanPhams.ToList();
            if (!string.IsNullOrEmpty(searchString))
            {
               
                    loaiSPs = db.LoaiSanPhams.Where(x => x.TenLoai.Contains(searchString)).ToList();
            }
            ViewBag.Page = page;
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(loaiSPs.OrderBy(x => x.MaLoaiSP).ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/QuanLyLoaiSP/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/QuanLyLoaiSP/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaLoaiSP,TenLoai,Icon,BiDanh")] LoaiSanPham loaiSanPham)
        {
            if (ModelState.IsValid)
            {
                db.LoaiSanPhams.Add(loaiSanPham);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(loaiSanPham);
        }

        // GET: Admin/QuanLyLoaiSP/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoaiSanPham loaiSanPham = db.LoaiSanPhams.Find(id);
            if (loaiSanPham == null)
            {
                return HttpNotFound();
            }
            return View(loaiSanPham);
        }

        // POST: Admin/QuanLyLoaiSP/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaLoaiSP,TenLoai,Icon,BiDanh")] LoaiSanPham loaiSanPham)
        {
            if (ModelState.IsValid)
            {
                db.Entry(loaiSanPham).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(loaiSanPham);
        }

        // GET: Admin/QuanLyLoaiSP/Delete/5
        public ActionResult Delete(int? id,int? page, string searchString)
        {
            var loaisp = db.LoaiSanPhams.FirstOrDefault(x => x.MaLoaiSP == id);
            if (loaisp == null)
            {
                return Content("<alert>Không tồn tại loại sản phẩm!</alert>");
            }
            else
            {
                db.LoaiSanPhams.Remove(loaisp);
                db.SaveChanges();
                return RedirectToAction("Index", new { page, searchString  });
            }
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
