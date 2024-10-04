using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAppYte.DAO;
using WebAppYte.Models;
using WebAppYte.Common;
using PagedList;

namespace WebAppYte.Areas.Admin.Controllers
{
    public class NguoiDungsController : Controller
    {
        // GET: Admin/NguoiDungs
        private modelWeb db = new modelWeb();

        // GET: Admin/NguoiDungs
        public ActionResult Index()
        {
            var nguoiDungs = db.BenhNhans.Where(x => x.trangthai == 1) ;
            return View(nguoiDungs.ToList());
        }

        // GET: Admin/NguoiDungs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BenhNhan nguoiDung = db.BenhNhans.Find(id);
            if (nguoiDung == null)
            {
                return HttpNotFound();
            }
            return View(nguoiDung);
        }

        // GET: Admin/NguoiDungs/Create
        public ActionResult Create()
        {
            
            return View();
        }

        // POST: Admin/NguoiDungs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "mabn,tenbn, sdt,email, diachi, ngaysinh, gioitinh,tendn, mk")] BenhNhan nguoiDung)
        {
            if (ModelState.IsValid)
            {
                nguoiDung.trangthai = 1;
                db.BenhNhans.Add(nguoiDung);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

          
            return View(nguoiDung);
        }

        // GET: Admin/NguoiDungs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BenhNhan nguoiDung = db.BenhNhans.Find(id);
            if (nguoiDung == null)
            {
                return HttpNotFound();
            }
            @ViewBag.ngay = (nguoiDung.ngaysinh).Value.ToString("yyyy-MM-dd");
            return View(nguoiDung);
        }

        // POST: Admin/NguoiDungs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "mabn,tenbn, sdt,email, diachi, ngaysinh, gioitinh,tendn, mk")] BenhNhan nguoiDung)
        {
            if (ModelState.IsValid)
            {
                nguoiDung.trangthai = 1;
                db.Entry(nguoiDung).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
         
            return View(nguoiDung);
        }

        // GET: Admin/NguoiDungs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BenhNhan nguoiDung = db.BenhNhans.Find(id);
            if (nguoiDung == null)
            {
                return HttpNotFound();
            }
            @ViewBag.ngay = (nguoiDung.ngaysinh).Value.ToString("yyyy-MM-dd");
            return View(nguoiDung);
        }

        // POST: Admin/NguoiDungs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int mabn, string tenbn,string sdt,string email, string diachi, DateTime ngaysinh, string gioitinh, string tendn, string mk)
        {
            BenhNhan nguoiDung = db.BenhNhans.Find(mabn);
            nguoiDung.mabn = mabn;
            nguoiDung.tenbn = tenbn;
            nguoiDung.sdt = sdt;
            nguoiDung.email = email;
            nguoiDung.diachi = diachi;
            nguoiDung.ngaysinh = ngaysinh;
            nguoiDung.gioitinh = gioitinh;
            nguoiDung.tendn = tendn;
            nguoiDung.mk = mk;
            nguoiDung.trangthai = 0;
            db.Entry(nguoiDung).State = EntityState.Modified;
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