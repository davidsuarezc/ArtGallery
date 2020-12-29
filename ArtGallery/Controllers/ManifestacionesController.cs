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
    public class ManifestacionesController : Controller
    {
        private galeryDbContext db = new galeryDbContext();

        // GET: Manifestaciones
        public async Task<ActionResult> Index()
        {
            return View(await db.Manifestaciones.ToListAsync());
        }

        // GET: Manifestaciones/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manifestacione manifestacione = await db.Manifestaciones.FindAsync(id);
            if (manifestacione == null)
            {
                return HttpNotFound();
            }
            return View(manifestacione);
        }

        // GET: Manifestaciones/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Manifestaciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,nombre_manifestacion")] Manifestacione manifestacione)
        {
            if (ModelState.IsValid)
            {
                db.Manifestaciones.Add(manifestacione);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(manifestacione);
        }

        // GET: Manifestaciones/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manifestacione manifestacione = await db.Manifestaciones.FindAsync(id);
            if (manifestacione == null)
            {
                return HttpNotFound();
            }
            return View(manifestacione);
        }

        // POST: Manifestaciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,nombre_manifestacion")] Manifestacione manifestacione)
        {
            if (ModelState.IsValid)
            {
                db.Entry(manifestacione).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(manifestacione);
        }

        // GET: Manifestaciones/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manifestacione manifestacione = await db.Manifestaciones.FindAsync(id);
            if (manifestacione == null)
            {
                return HttpNotFound();
            }
            return View(manifestacione);
        }

        // POST: Manifestaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Manifestacione manifestacione = await db.Manifestaciones.FindAsync(id);
            db.Manifestaciones.Remove(manifestacione);
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
    }
}
