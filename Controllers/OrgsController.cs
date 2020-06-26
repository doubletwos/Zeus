﻿using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Zeus.Models;

namespace Zeus.Controllers
{
    public class OrgsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Orgs
        public ActionResult Index(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Org org = db.Orgs.Find(id);
            if (org == null)
            {
                return HttpNotFound();
            }


            //var org1 = db.RegisteredUserOrganisations.Where(o => o.RegisteredUserOrganisationId == id).Select(x => x.RegisteredUserId).FirstOrDefault();

            //var users = db.RegisteredUsers

            //    .Where(i => i.RegisteredUserId == org1)
            //    .ToList();
            //return View(users);


            var orgs = db.Orgs.Include(o => o.Domain).Include(o => o.OrgBrand).Include(o => o.OrgType);
            return View(orgs.ToList());
        }



        // GET: Orgs/SystemAdminIndex
        public ActionResult SystemAdminIndex(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Org org = db.Orgs.Find(id);
            if (org == null)
            {
                return HttpNotFound();
            }

            var orgs = db.Orgs.Include(o => o.Domain).Include(o => o.OrgBrand).Include(o => o.OrgType);
            return View(orgs.ToList());

        }



        // GET: Orgs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Org org = db.Orgs.Find(id);
            if (org == null)
            {
                return HttpNotFound();
            }
            return View(org);
        }

        // GET: Orgs/Create
        public ActionResult Create()
        {
            ViewBag.DomainId = new SelectList(db.Domains, "DomainId", "DomainName");
            ViewBag.OrgBrandId = new SelectList(db.OrgBrands, "OrgBrandId", "OrgBrandName");
            ViewBag.OrgTypeId = new SelectList(db.OrgTypes, "OrgTypeId", "OrgTypeName");
            return View();
        }

        // POST: Orgs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Org org)
        {
            var rud = Session["Email"].ToString();
            var loggedinuser = db.RegisteredUsers.Where(x => x.Email ==  rud).Select(x => x.Email).SingleOrDefault();
            var orgredirect = db.RegisteredUserOrganisations.Where(x => x.Email == rud).Select(x => x.OrgId).FirstOrDefault();


            if ( rud != loggedinuser)
            
            {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            

            if (ModelState.IsValid)
            {

                db.Orgs.Add(org);
                db.SaveChanges();
              
                return RedirectToAction("Index", "Orgs", new { id = orgredirect });
            }

            ViewBag.DomainId = new SelectList(db.Domains, "DomainId", "DomainName", org.DomainId);
            ViewBag.OrgBrandId = new SelectList(db.OrgBrands, "OrgBrandId", "OrgBrandName", org.OrgBrandId);
            ViewBag.OrgTypeId = new SelectList(db.OrgTypes, "OrgTypeId", "OrgTypeName", org.OrgTypeId);
            return View(org);
        }

        // GET: Orgs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Org org = db.Orgs.Find(id);
            if (org == null)
            {
                return HttpNotFound();
            }
            ViewBag.DomainId = new SelectList(db.Domains, "DomainId", "DomainName", org.DomainId);
            ViewBag.OrgBrandId = new SelectList(db.OrgBrands, "OrgBrandId", "OrgBrandName", org.OrgBrandId);
            ViewBag.OrgTypeId = new SelectList(db.OrgTypes, "OrgTypeId", "OrgTypeName", org.OrgTypeId);
            return View(org);
        }

        // POST: Orgs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrgId,OrgName,OrgAddress,CreationDate,DomainId,OrgTypeId,OrgBrandId")] Org org)
        {
            if (ModelState.IsValid)
            {
                db.Entry(org).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DomainId = new SelectList(db.Domains, "DomainId", "DomainName", org.DomainId);
            ViewBag.OrgBrandId = new SelectList(db.OrgBrands, "OrgBrandId", "OrgBrandName", org.OrgBrandId);
            ViewBag.OrgTypeId = new SelectList(db.OrgTypes, "OrgTypeId", "OrgTypeName", org.OrgTypeId);
            return View(org);
        }

        // GET: Orgs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Org org = db.Orgs.Find(id);
            if (org == null)
            {
                return HttpNotFound();
            }
            return View(org);
        }

        // POST: Orgs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Org org = db.Orgs.Find(id);
            db.Orgs.Remove(org);
            db.SaveChanges();
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
