using Entities;
using PagedList;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Store.Areas.Admin.Controllers
{
    public class QLNhaSanXuatController : Controller
    {
        private BanHangDbContext db = new BanHangDbContext();

        // GET: Admin/QLNhaSanXuat
        public ActionResult Index(int? page, string searchString)
        {
            ViewBag.CurrentFilter = searchString;
            var lstNSX = db.NhaSanXuats.ToList();
            if (!string.IsNullOrEmpty(searchString))
            {
                lstNSX = db.NhaSanXuats.Where(x => x.TenNSX.Contains(searchString)).ToList();
            }
            ViewBag.Page = page;
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(lstNSX.OrderBy(x => x.MaNSX).ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public string ProcessUpload(HttpPostedFileBase file)
        {
            file.SaveAs(Server.MapPath("~/Content/Logo/" + file.FileName));
            return file.FileName;
        }

        // GET: Admin/QLNhaSanXuat/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhaSanXuat nhaSanXuat = db.NhaSanXuats.Find(id);
            if (nhaSanXuat == null)
            {
                return HttpNotFound();
            }
            return View(nhaSanXuat);
        }

        // GET: Admin/QLNhaSanXuat/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/QLNhaSanXuat/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaNSX,TenNSX,ThongTin,Logo")] NhaSanXuat nhaSanXuat)
        {
            if (ModelState.IsValid)
            {
                db.NhaSanXuats.Add(nhaSanXuat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nhaSanXuat);
        }

        // GET: Admin/QLNhaSanXuat/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhaSanXuat nhaSanXuat = db.NhaSanXuats.Find(id);
            if (nhaSanXuat == null)
            {
                return HttpNotFound();
            }
            return View(nhaSanXuat);
        }

        // POST: Admin/QLNhaSanXuat/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaNSX,TenNSX,ThongTin,Logo")] NhaSanXuat nhaSanXuat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nhaSanXuat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nhaSanXuat);
        }

        // GET: Admin/QLNhaSanXuat/Delete/5
        public ActionResult Delete(int? id, int? page, string searchString)
        {
            var nsx = db.NhaSanXuats.FirstOrDefault(x => x.MaNSX == id);
            if (nsx == null)
            {
                return Content("<alert>Không tồn tại nhà sản xuất!</alert>");
            }
            else
            {
                db.NhaSanXuats.Remove(nsx);
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