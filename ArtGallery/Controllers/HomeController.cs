using GaleryArt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GaleryArt.Controllers
{
    public class HomeController : Controller
    {
        private galeryDbContext db = new galeryDbContext();
        public ActionResult Index()
        {
            var obras = db.Obras.ToList();

            return View(obras);
        }
    }
}