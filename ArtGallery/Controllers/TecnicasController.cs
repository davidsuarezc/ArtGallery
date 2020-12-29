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
    public class TecnicasController : Controller
    {
        private galeryDbContext db = new galeryDbContext();

        // GET: Tecnicas
        public async Task<ActionResult> Index()
        {
            var tecnicas = db.Tecnicas.Include(t => t.Manifestacione);
            return View(await tecnicas.ToListAsync());
        }

        // GET: Tecnicas/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tecnica tecnica = await db.Tecnicas.FindAsync(id);
            if (tecnica == null)
            {
                return HttpNotFound();
            }
            return View(tecnica);
        }

        // GET: Tecnicas/Create
        public ActionResult Create()
        {
            ViewBag.manifestacion = new SelectList(db.Manifestaciones, "Id", "nombre_manifestacion");
            return View();
        }

        // POST: Tecnicas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,nombre_tecnica,manifestacion")] Tecnica tecnica)
        {
            if (ModelState.IsValid)
            {
                db.Tecnicas.Add(tecnica);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.manifestacion = new SelectList(db.Manifestaciones, "Id", "nombre_manifestacion", tecnica.manifestacion);
            return View(tecnica);
        }

        // GET: Tecnicas/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tecnica tecnica = await db.Tecnicas.FindAsync(id);
            if (tecnica == null)
            {
                return HttpNotFound();
            }
            ViewBag.manifestacion = new SelectList(db.Manifestaciones, "Id", "nombre_manifestacion", tecnica.manifestacion);
            return View(tecnica);
        }

        // POST: Tecnicas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,nombre_tecnica,manifestacion")] Tecnica tecnica)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tecnica).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.manifestacion = new SelectList(db.Manifestaciones, "Id", "nombre_manifestacion", tecnica.manifestacion);
            return View(tecnica);
        }

        // GET: Tecnicas/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tecnica tecnica = await db.Tecnicas.FindAsync(id);
            if (tecnica == null)
            {
                return HttpNotFound();
            }
            return View(tecnica);
        }

        // POST: Tecnicas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Tecnica tecnica = await db.Tecnicas.FindAsync(id);
            db.Tecnicas.Remove(tecnica);
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
