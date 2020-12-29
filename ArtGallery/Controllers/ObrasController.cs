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
using System.IO;
using System.Web.Script.Serialization;

namespace GaleryArt.Controllers
{
    [Authorize]
    public class ObrasController : Controller
    {
        private galeryDbContext db = new galeryDbContext();

        // GET: Obras
        public async Task<ActionResult> Index()
        {
            var obras = db.Obras.Include(o => o.Manifestacione).Include(o => o.Tecnica1);
            return View(await obras.ToListAsync());
        }

        // GET: Obras/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Obra obra = await db.Obras.FindAsync(id);
            if (obra == null)
            {
                return HttpNotFound();
            }
            return View(obra);
        }

        // GET: Obras/Create
        public ActionResult Create()
        {
            ViewBag.manifestacion = new SelectList(db.Manifestaciones, "Id", "nombre_manifestacion");
            ViewBag.tecnica = new SelectList(db.Tecnicas, "Id", "nombre_tecnica");

            db.Configuration.ProxyCreationEnabled = false;
            ViewBag.premios = db.Premios.ToList();
            return View();
        }

        // POST: Obras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Obra obra, HttpPostedFileBase foto, List<int> premiosPost)
        {
            if (obra.nombre_obra!= null && foto!=null)
            {
                if (foto != null && foto.ContentLength > 0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".tiff", ".tif", ".png", ".bmp", ".svg" };
                    var extension = Path.GetExtension(foto.FileName).ToLower();

                    if (allowedExtensions.Contains(extension))
                    {
                        string file_name = obra.nombre_obra.Trim() + extension;
                        string path = Path.Combine(Server.MapPath("~/Images"), file_name);
                        foto.SaveAs(path);

                        obra.path_foto = file_name;
                        if(premiosPost != null)
                        {
                            foreach (var award in premiosPost)
                             {
                                Premio nuevo = new Premio
                                {
                                    Id = award,
                                    nombre_premio = db.Premios.Where(x => x.Id == award).Select(x => x.nombre_premio).FirstOrDefault()
                                };
                                obra.Premios.Add(nuevo);
                             };                         
                        }
                        db.Obras.Add(obra);
                        await db.SaveChangesAsync();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Error = "Debe seleccionar una imagen. Archivo no válido";
                        ViewBag.manifestacion = new SelectList(db.Manifestaciones, "Id", "nombre_manifestacion", obra.manifestacion);
                        ViewBag.tecnica = new SelectList(db.Tecnicas, "Id", "nombre_tecnica", obra.tecnica);
                        return View(obra);
                    }
                }
                else
                {
                    ViewBag.Error = "Debe seleccionar una imagen. Archivo no válido";
                    ViewBag.manifestacion = new SelectList(db.Manifestaciones, "Id", "nombre_manifestacion", obra.manifestacion);
                    ViewBag.tecnica = new SelectList(db.Tecnicas, "Id", "nombre_tecnica", obra.tecnica);
                    return View(obra);
                }
                    
            }
            else
            {
                ViewBag.manifestacion = new SelectList(db.Manifestaciones, "Id", "nombre_manifestacion", obra.manifestacion);
                ViewBag.tecnica = new SelectList(db.Tecnicas, "Id", "nombre_tecnica", obra.tecnica);
                return View(obra);
            }
 
        }

        // GET: Obras/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Obra obra = await db.Obras.FindAsync(id);
            if (obra == null)
            {
                return HttpNotFound();
            }
            ViewBag.manifestacion = new SelectList(db.Manifestaciones, "Id", "nombre_manifestacion", obra.manifestacion);
            ViewBag.tecnica = new SelectList(db.Tecnicas, "Id", "nombre_tecnica", obra.tecnica);
            return View(obra);
        }

        // POST: Obras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Obra obra, HttpPostedFileBase foto, List<int> premiosPost)
        {
            if (obra.nombre_obra != null && foto != null)
            {
                if (foto != null && foto.ContentLength > 0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".tiff", ".tif", ".png", ".bmp", ".svg" };
                    var extension = Path.GetExtension(foto.FileName).ToLower();

                    if (allowedExtensions.Contains(extension))
                    {
                        string file_name = obra.nombre_obra.Trim() + extension;
                        string path = Path.Combine(Server.MapPath("~/Images"), file_name);
                        foto.SaveAs(path);

                        obra.path_foto = file_name;
                        if (premiosPost != null)
                        {
                            obra.Premios.Clear();
                            foreach (var award in premiosPost)
                            {
                                Premio nuevo = new Premio
                                {
                                    Id = award,
                                    nombre_premio = db.Premios.Where(x => x.Id == award).Select(x => x.nombre_premio).FirstOrDefault()
                                };
                                obra.Premios.Add(nuevo);
                            };
                        }

                        db.Entry(obra).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                        return RedirectToAction("Index");
                       
                    }
                    else
                    {
                        ViewBag.Error = "Debe seleccionar una imagen. Archivo no válido";
                        ViewBag.manifestacion = new SelectList(db.Manifestaciones, "Id", "nombre_manifestacion", obra.manifestacion);
                        ViewBag.tecnica = new SelectList(db.Tecnicas, "Id", "nombre_tecnica", obra.tecnica);
                        return View(obra);
                    }
                }
                else
                {
                    ViewBag.Error = "Debe seleccionar una imagen. Archivo no válido";
                    ViewBag.manifestacion = new SelectList(db.Manifestaciones, "Id", "nombre_manifestacion", obra.manifestacion);
                    ViewBag.tecnica = new SelectList(db.Tecnicas, "Id", "nombre_tecnica", obra.tecnica);
                    return View(obra);
                }

            }
            else
            {
                ViewBag.manifestacion = new SelectList(db.Manifestaciones, "Id", "nombre_manifestacion", obra.manifestacion);
                ViewBag.tecnica = new SelectList(db.Tecnicas, "Id", "nombre_tecnica", obra.tecnica);
                return View(obra);
            }

        }

        // GET: Obras/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Obra obra = await db.Obras.FindAsync(id);
            if (obra == null)
            {
                return HttpNotFound();
            }
            return View(obra);
        }

        // POST: Obras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Obra obra = await db.Obras.FindAsync(id);
            db.Obras.Remove(obra);
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
