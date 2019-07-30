using Entities;
using PagedList;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Store.Areas.Admin.Controllers
{
    public class QLNhaCungCapController : Controller
    {
        private BanHangDbContext db = new BanHangDbContext();

        // GET: Admin/QLNhaCungCap
        public ActionResult Index(int? page, string searchString)
        {
            ViewBag.CurrentFilter = searchString;
            var lstNCC = db.NhaCungCaps.ToList();
            if (!string.IsNullOrEmpty(searchString))
            {
                lstNCC = db.NhaCungCaps.Where(x => x.TenNCC.Contains(searchString)).ToList();
            }
            ViewBag.Page = page;
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(lstNCC.OrderBy(x => x.MaNCC).ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/QLNhaCungCap/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhaCungCap nhaCungCap = db.NhaCungCaps.Find(id);
            if (nhaCungCap == null)
            {
                return HttpNotFound();
            }
            return View(nhaCungCap);
        }

        // GET: Admin/QLNhaCungCap/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/QLNhaCungCap/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaNCC,TenNCC,DiaChi,Email,SoDienThoai,Fax")] NhaCungCap nhaCungCap)
        {
            if (ModelState.IsValid)
            {
                db.NhaCungCaps.Add(nhaCungCap);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nhaCungCap);
        }

        // GET: Admin/QLNhaCungCap/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhaCungCap nhaCungCap = db.NhaCungCaps.Find(id);
            if (nhaCungCap == null)
            {
                return HttpNotFound();
            }
            return View(nhaCungCap);
        }

        // POST: Admin/QLNhaCungCap/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaNCC,TenNCC,DiaChi,Email,SoDienThoai,Fax")] NhaCungCap nhaCungCap)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nhaCungCap).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nhaCungCap);
        }

        // GET: Admin/QLNhaCungCap/Delete/5
        public ActionResult Delete(int? id, int? page, string searchString)
        {
            var lstNCC = db.NhaCungCaps.FirstOrDefault(x => x.MaNCC == id);
            if (lstNCC == null)
            {
                return Content("<alert>Không tồn tại nhà cung cấp!</alert>");
            }
            else
            {
                db.NhaCungCaps.Remove(lstNCC);
                db.SaveChanges();
                return RedirectToAction("Index", new { page, searchString });
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