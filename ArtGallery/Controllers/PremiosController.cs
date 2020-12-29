using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GaleryArt.Models;

namespace GaleryArt.Controllers
{
    [Authorize]
    public class PremiosController : Controller
    {
        private galeryDbContext db = new galeryDbContext();

        // GET: Premios
        public async Task<ActionResult> Index()
        {
            return View(await db.Premios.ToListAsync());
        }

        // GET: Premios/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Premio premio = await db.Premios.FindAsync(id);
            if (premio == null)
            {
                return HttpNotFound();
            }
            return View(premio);
        }

        // GET: Premios/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Premios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,nombre_premio")] Premio premio)
        {
            if (ModelState.IsValid)
            {
                db.Premios.Add(premio);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(premio);
        }

        // GET: Premios/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Premio premio = await db.Premios.FindAsync(id);
            if (premio == null)
            {
                return HttpNotFound();
            }
            return View(premio);
        }

        // POST: Premios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,nombre_premio")] Premio premio)
        {
            if (ModelState.IsValid)
            {
                db.Entry(premio).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(premio);
        }

        // GET: Premios/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Premio premio = await db.Premios.FindAsync(id);
            if (premio == null)
            {
                return HttpNotFound();
            }
            return View(premio);
        }

        // POST: Premios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Premio premio = await db.Premios.FindAsync(id);
            db.Premios.Remove(premio);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public JsonResult getall(string term) 
        {
            var result = db.Premios.Where(x => x.nombre_premio.Contains(term)).Select(x => x.nombre_premio).Take(5).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
