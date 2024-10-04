﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAppYte.Models;
using PagedList;
using WebAppYte.DAO;
namespace WebAppYte.Controllers
{
    public class HoidapController : Controller
    {
        private modelWeb db = new modelWeb();

        // GET: Hoidap
       public ActionResult Index(int ? id, int? page)
        {
            var hoiDaps = db.HoiDaps.Include(h => h.BenhNhan).Include(h => h.NguoiDung).Where(h => h.mabn == id && h.trangthai == 1)
                .OrderByDescending(x => x.ngayhoi).ThenBy(x => x.ma).ToList();
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            ViewBag.id = id;
            return View(hoiDaps.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult ListAll(int? page)
        {
            if (page == null) page = 1;
        
            var model = db.HoiDaps.Include(h => h.BenhNhan).Include(h => h.NguoiDung).Where(n => n.trangthai == 1).OrderByDescending(x => x.ngayhoi).ThenBy(x => x.ma).ToList();
           
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(model.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Cauhoichoxuly(int? id, int? page)
        {
            var hoiDaps = db.HoiDaps.Include(h => h.BenhNhan).Include(h => h.NguoiDung).Where(h => h.mabn == id && h.trangthai == 0)
                .OrderByDescending(x => x.ngayhoi).ThenBy(x => x.ma).ToList();
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            ViewBag.id = id;
            return View(hoiDaps.ToPagedList(pageNumber, pageSize));
        }


        // GET: Hoidap/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoiDap hoiDap = db.HoiDaps.Find(id);
            if (hoiDap == null)
            {
                return HttpNotFound();
            }
            return View(hoiDap);
        }

        // GET: Hoidap/Create
        public ActionResult Create(int? id)
        {
            ViewBag.mabn = new SelectList(db.BenhNhans.Where(h => h.mabn == id), "mabn", "tenbn");
          

            return View();
        }

        // POST: Hoidap/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ma, hoi, ngayhoi, ngaytl, dap,  mabn, trangthai")] HoiDap hoiDap)
        {
            if (ModelState.IsValid)
            {
                var d = DateTime.Now;
                hoiDap.ngayhoi = d;
                hoiDap.trangthai = 0;
                db.HoiDaps.Add(hoiDap);
                db.SaveChanges();
                return RedirectToAction("Index","Hoidap" ,new { id = hoiDap.mabn });
            }

            ViewBag.mabn = new SelectList(db.BenhNhans, "mabn", "tenbn", hoiDap.mabn);

            return View(hoiDap);
        }

        // GET: Hoidap/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoiDap hoiDap = db.HoiDaps.Find(id);
            if (hoiDap == null)
            {
                return HttpNotFound();
            }
            ViewBag.mabn = new SelectList(db.BenhNhans.Where(x => x.mabn == hoiDap.mabn), "mabn", "tenbn", hoiDap.mabn);
            ViewBag.mand = new SelectList(db.NguoiDungs.Where(x => x.mand == hoiDap.mand), "mand", "hoten", hoiDap.mand);
            @ViewBag.ngay = (hoiDap.ngayhoi).Value.ToString("yyyy-MM-dd");
            return View(hoiDap);
        }

        // POST: Hoidap/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ma, hoi, ngayhoi, ngaytl, dap, mand, mabn, trangthai")] HoiDap hoiDap)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hoiDap).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.mabn = new SelectList(db.BenhNhans, "mabn", "tenbn", hoiDap.mabn);
            ViewBag.mand = new SelectList(db.NguoiDungs, "mand", "hoten", hoiDap.mand);
            return View(hoiDap);
        }

        // GET: Hoidap/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoiDap hoiDap = db.HoiDaps.Find(id);
            if (hoiDap == null)
            {
                return HttpNotFound();
            }
            return View(hoiDap);
        }

        // POST: Hoidap/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HoiDap hoiDap = db.HoiDaps.Find(id);
            db.HoiDaps.Remove(hoiDap);
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
