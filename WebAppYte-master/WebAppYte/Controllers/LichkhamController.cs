﻿using System;
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
namespace WebAppYte.Controllers
{
    public class LichkhamController : Controller
    {
        private modelWeb db = new modelWeb();
       

        // GET: Lichkham
        public class listofSegments
        {
            public string Text { get; set; }
            public string Value { get; set; }
        }
        public ActionResult Index(int? id , int? page)
         {
            LichKham ls = new LichKham();

             var lichKhams = ls.DSLichKham().Where(h => h.mabn == id).OrderByDescending(x=>x.ngaydat).ThenBy(x=>x.madat);
            foreach (var item in lichKhams)
            {
                if (@item.ngaykham < DateTime.Now && @item.trangthai == 0)
                {
                    DatLich dl = db.DatLiches.Find(@item.madat);
                    dl.trangthai = 3;
                    db.Entry(dl).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
             int pageSize = 3;
             int pageNumber = (page ?? 1);
             ViewBag.id = id;
             return View(lichKhams.ToPagedList(pageNumber, pageSize));
         }
        
         public ActionResult Dangxuly(int? id, int? page)
         {

            LichKham ls = new LichKham();
            var lichKhams = ls.DSLichKham().Where(h => h.mabn == id && h.trangthai==0).OrderByDescending(x => x.ngaydat).ThenBy(x => x.madat);
            int pageSize = 3;
             int pageNumber = (page ?? 1);
             ViewBag.id = id;
             return View(lichKhams.ToPagedList(pageNumber, pageSize));
         }
         public ActionResult Daxacnhan(int? id, int? page)
         {
            LichKham ls = new LichKham();
            var lichKhams = ls.DSLichKham().Where(h => h.mabn == id && h.trangthai == 1).OrderByDescending(x => x.ngaydat).ThenBy(x => x.madat);
            int pageSize = 3;
             int pageNumber = (page ?? 1);
             ViewBag.id = id;
             return View(lichKhams.ToPagedList(pageNumber, pageSize));
         }
         public ActionResult Datuvanxong(int? id, int? page)
         {
            LichKham ls = new LichKham();
            var lichKhams = ls.DSLichKham().Where(h => h.mabn == id && (h.trangthai == 2||h.trangthai==4)).OrderByDescending(x => x.ngaydat).ThenBy(x => x.madat);
            int pageSize = 3;
             int pageNumber = (page ?? 1);
             ViewBag.id = id;
             return View(lichKhams.ToPagedList(pageNumber, pageSize));
         }

         // GET: Lichkham/Details/5
         public ActionResult Details(int? id)
         {
             if (id == null)
             {
                 return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
             }
            LichKham ls = new LichKham();
            LichKham lichkham=ls.DSLichKham().Where(x=>x.madat==id).FirstOrDefault();

             if (lichkham == null)
             {
                 return HttpNotFound();
             }
             return View(lichkham);
         }


        public ActionResult Create(string hinhthuc, string chinhanh, string khoa, string bacsi,string ngaykham,string cakham, string mota)
        {
            
            var u = Session["user"] as WebAppYte.Models.BenhNhan;

            hinhthuc = LKSave.hinhthuc;
            chinhanh = LKSave.chinhanh;
            khoa = LKSave.khoa;
            bacsi = LKSave.bacsi;
            ngaykham = LKSave.ngaykham; 
            cakham = LKSave.cakham;
            var segmentList = new List<listofSegments>();
            listofSegments segmentItem;
            var strArr = new string[] { "Khám trong giờ", "Khám ngoài giờ", "Khám online" };
            for (int index = 0; index < strArr.Length; index++)
            {
                segmentItem = new listofSegments();
                segmentItem.Text = strArr[index];
                segmentItem.Value = strArr[index];
                segmentList.Add(segmentItem);
            }

            ViewBag.hinhthuc = segmentList;
            ViewBag.ht = hinhthuc;

            List<string> dd = (from p in db.ChiNhanhs select p.diachi).ToList();
            ViewBag.chinhanh = dd;
            ViewBag.cn = chinhanh;


            List<string> tenkhoa = (from p in db.Khoas select p.tenkhoa).ToList();
            ViewBag.khoa = tenkhoa;
            ViewBag.k = khoa;


            TTNguoiDung ls = new TTNguoiDung();
            var bss = ls.ListNguoiDung().ToList();
            if (chinhanh != null && khoa != null)
            {

                bss = ls.ListNguoiDung().Where(x => x.tenchinhanh == chinhanh && x.tenkhoa == khoa).ToList();

            }
            var bs1 = (from p in bss select p.hoten).ToList();
            ViewBag.bacsi = bs1;
            ViewBag.bs = bacsi;

            DateTime a = DateTime.Now;

            var ngay = db.CaKhams.Where(x => x.ngaykham >= a.Date).ToList();
            LichKham lichkham = new LichKham();


            if (LKSave.chinhanh != null && LKSave.khoa != null && LKSave.hinhthuc != null)
            {
                LKSave.mand = lichkham.FindMaBS(LKSave.khoa, LKSave.chinhanh);
                if (LKSave.hinhthuc != null && LKSave.mand != null)
                {
                    ngay = db.CaKhams.Where(x => x.mand == LKSave.mand && x.hinhthuc == LKSave.hinhthuc && x.ngaykham >= a.Date).ToList();
                }
                
            }


            ViewBag.ngaykham = (from p in ngay select p.ngaykham.Value.ToString("yyyy-MM-dd")).ToList();
            ViewBag.nk =ngaykham;


            var ca = db.CaKhams.Where(x => x.ngaykham >= a.Date).ToList();
            if (LKSave.chinhanh != null && LKSave.khoa != null && LKSave.bacsi != null && LKSave.hinhthuc != null && LKSave.ngaykham != null)
            {
                LKSave.mand = lichkham.FindMaBS(LKSave.khoa, LKSave.chinhanh);
               
                   ca = db.CaKhams.Where(x => x.mand == LKSave.mand && x.hinhthuc == LKSave.hinhthuc && (x.ngaykham).ToString() == LKSave.ngaykham&&x.trangthai==1).ToList();


            }
            ViewBag.cakham = (from p in ca select (p.ca)).ToList().Distinct();
            ViewBag.ca = cakham;
            DatLich f = new DatLich();
            f.ngaydat = DateTime.Now;

            f.trangthai = 0;

            if (LKSave.chinhanh != null && LKSave.khoa != null && LKSave.bacsi != null && LKSave.hinhthuc != null && LKSave.ngaykham != null&& LKSave.cakham != null && LKSave.mand != null&& mota != null)
            {
                string bs = (LKSave.ngaykham + "," + LKSave.cakham).ToString();
                LKSave.maca = lichkham.FindMaCa(bs, LKSave.hinhthuc, LKSave.mand);
                f.maca = LKSave.maca;
                if (u != null)
                {
                    f.mabn = u.mabn;
                    if (mota != null)
                    {
                        f.mota = mota;
                        if (ModelState.IsValid)
                        {
                            int check = db.DatLiches.Where(x => x.maca == LKSave.maca).Count();
                            if (check == 0)
                            {

                                db.DatLiches.Add(f);
                                db.SaveChanges();

                                return RedirectToAction("Index", "LichKham", new { id = u.mabn });
                            }
                            else
                            {
                                ViewBag.Fail = "Ca khám này đã có người đặt!";
                                return PartialView();
                            }
                            //db.DatLiches.Add(f);
                           // db.SaveChanges();

                         //   return RedirectToAction("Index", "LichKham", new { id = u.mabn });
                        }
                    }
                }
                else
                {
                    if (mota != null)
                    {
                        f.mota = mota;

                        return RedirectToAction("NhapTTDK", "LichKham");


                    }
                }
            }
            else
            {
                ViewBag.Fail = "Vui lòng nhập đầy đủ thông tin";
                return PartialView();
            }

            return PartialView();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ActionPostData(string hinhthuc, string chinhanh, string khoa, string bacsi, string ngaykham,string cakham, string mota)
        {      
            var u = Session["user"] as WebAppYte.Models.BenhNhan;
            LKSave.chinhanh = chinhanh;
            LKSave.khoa = khoa;
            LKSave.bacsi = bacsi;
            LKSave.hinhthuc = hinhthuc;
            LKSave.ngaykham = ngaykham;
            LKSave.cakham = cakham;
            LKSave.mota = mota;
            return RedirectToAction("Create", new { hinhthuc = LKSave.hinhthuc, chinhanh = LKSave.chinhanh, khoa = LKSave.khoa, bacsi = LKSave.bacsi, ngaykham = LKSave.ngaykham, cakham = LKSave.cakham, mota =LKSave.mota });
        }
    
        public ActionResult NhapTTDK()
        {
       
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NhapTTDK([Bind(Include = "madat,ngaydat,mota,sdt,hoten,ngaysinh,trangthai,maca,mabn")] DatLich f)
        {
            if (f.sdt != null && f.hoten != null && f.ngaysinh != null)
            {

                f.ngaydat = DateTime.Now;
                f.mota = LKSave.mota;

                f.trangthai = 0;
                f.maca = LKSave.maca;
                if (ModelState.IsValid)
                {
                    int check = db.DatLiches.Where(x => x.maca == LKSave.maca).Count();
                    if (check == 0)
                    {

                        db.DatLiches.Add(f);
                        db.SaveChanges();

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.Fail = "Ca khám này đã có người đặt!";
                        return View();
                    }
                }
            }

            else
            {
                ViewBag.Fail = "Vui lòng nhập đầy đủ thông tin";
                return View(f);
            }

            return View(f);

        }
        public ActionResult DanhGia(int? id, int fi, int madat)
        {
            Bien.mabs = (int)id;
            Bien.mabn = fi;
                    DatLich dl = db.DatLiches.Find(madat);
                    dl.trangthai = 4;
                    db.Entry(dl).State = EntityState.Modified;
                    db.SaveChanges();
            var nd = db.NguoiDungs.Where(h => h.mand == id).FirstOrDefault();
            var bn = db.BenhNhans.Where(h => h.mabn == fi).FirstOrDefault();

            ViewBag.mand = nd.hoten;
            ViewBag.mabn = bn.tenbn;

            return View();
        }

        // POST: Admin/Tintucs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult DanhGia([Bind(Include = "ngay,noidung,mand, mabn")] DanhGia danhgia)
        {
    
            if (ModelState.IsValid)
            {
                danhgia.mabn = Bien.mabn;
                danhgia.mand = Bien.mabs;
                var d = DateTime.Now;
                danhgia.ngay = d;
                db.DanhGias.Add(danhgia);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.mand = new SelectList(db.NguoiDungs, "mand", "hoten",danhgia.mand);
            ViewBag.mabn = new SelectList(db.BenhNhans, "mabn", "tenbn",danhgia.mabn);
            return View(danhgia);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LichKham ls= new LichKham();
            var lichkham=ls.DSLichKham().Where(x=>x.madat==id).FirstOrDefault();
            if (lichkham == null)
            {
                return HttpNotFound();
            }
            return View(lichkham);
        }

        // POST: Lichkham/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
           DatLich lichKham = db.DatLiches.Find(id);
            db.DatLiches.Remove(lichKham);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}
