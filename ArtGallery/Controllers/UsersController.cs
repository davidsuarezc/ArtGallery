﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GaleryArt.Models;
using System.Web.Helpers;
using System.Web.Security;

namespace GaleryArt.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private galeryDbContext db = new galeryDbContext();

        public async Task<ActionResult> Index()
        {
            return View(await db.Users.ToListAsync());
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,name,username,password")] User user)
        {
            if (ModelState.IsValid)
            {
                user.name = user.name.Trim();
                user.username = user.username.Trim();
                user.password = Crypto.SHA256(user.password);
                db.Users.Add(user);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            user.password = String.Empty;
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,name,username,password")] User user)
        {
            if (ModelState.IsValid)
            {
                user.name = user.name.Trim();
                user.username = user.username.Trim();
                user.password = Crypto.SHA256(user.password);
                db.Entry(user).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            user.password = String.Empty;
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            User user = await db.Users.FindAsync(id);
            db.Users.Remove(user);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public ActionResult login()
        {
            if(TempData["error"]!=null) { ViewBag.Error = TempData["error"].ToString(); }
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult login(string username, string password)
        {
            var user = db.Users.Where(usr => usr.username == username).FirstOrDefault();
            if(user != null)
            {
                password = Crypto.SHA256(password);
                string hashed = user.password;
                bool match = String.Equals(password, hashed);
                if(match)
                {
                    FormsAuthentication.SetAuthCookie(user.username, false);
                    Session["username"] = user.username;
                    return RedirectToAction("Index", "Home");
                }
            }

            TempData["error"] = "Usuario o contraseña incorrectos";
            return RedirectToAction("login", "Users");
        }

        public ActionResult logout()
        {
            Session["username"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

    }
}
