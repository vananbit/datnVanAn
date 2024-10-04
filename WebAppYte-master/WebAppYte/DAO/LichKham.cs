﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WebAppYte.Models;

namespace WebAppYte.DAO
{
    public class LichKham
    {
        public int madat { get; set; }
        public DateTime ngaydat { get; set; }
        public string mota { get; set; }
        public int trangthai { get; set; }
        public int maca { get; set; }
        public int mabn { get; set; }
        public string tenbn { get; set; }
        public DateTime ngaykham { get; set; }
        public string ca { get; set; }
        public string hinhthuc { get; set; }
        public int mand { get; set; }
        public string hoten { get; set; }
        public string sdt { get; set; }
        public DateTime ngaysinh { get; set; }
        public LichKham() { }
        public LichKham(int madat, DateTime ngaydat, DateTime ngaysinh, string mota, int trangthai, int maca, int mabn, string tenbn, string sdt, DateTime ngaykham, string ca, string hinhthuc, int mand, string hoten)
        {
            this.madat = madat;
            this.ngaydat = ngaydat;
            this.mota = mota;
            this.trangthai = trangthai;
            this.maca = maca;
            this.mabn = mabn;
            this.tenbn = tenbn;
            this.ngaykham = ngaykham;
            this.ca = ca;
            this.hinhthuc = hinhthuc;
            this.mand = mand;
            this.hoten = hoten;
            this.sdt = sdt;
            this.ngaysinh = ngaysinh;

        }
        public List<LichKham> DSLichKham()
        {

            modelWeb db = new modelWeb();
            var lst = (
                           from p in db.DatLiches
                           from x in db.CaKhams
                           from z in db.NguoiDungs

                           where p.maca == x.maca && x.mand == z.mand && x.trangthai == 1

                           select new LichKham()
                           {
                               madat = p.madat,
                               ngaydat = (DateTime)p.ngaydat,
                               mota = p.mota,
                               trangthai = (int)p.trangthai,
                               maca = (int)x.maca,

                               mabn = (int)p.mabn,

                               tenbn = p.hoten,
                               ngaysinh = (DateTime)p.ngaysinh,
                               sdt = p.sdt,

                               mand = (int)(from q in db.CaKhams
                                            where q.maca == x.maca
                                            select new
                                            {
                                                q.mand
                                            }).FirstOrDefault().mand,

                               hoten = (from y in db.NguoiDungs

                                        where y.mand == x.mand
                                        select new
                                        {
                                            y.hoten
                                        }).FirstOrDefault().hoten,
                               ngaykham = (DateTime)(from y in db.CaKhams

                                                     where y.maca == p.maca
                                                     select new
                                                     {
                                                         y.ngaykham
                                                     }).FirstOrDefault().ngaykham,
                               ca = (from y in db.CaKhams

                                     where y.maca == p.maca
                                     select new
                                     {
                                         y.ca
                                     }).FirstOrDefault().ca,
                               hinhthuc = (from y in db.CaKhams

                                           where y.maca == p.maca
                                           select new
                                           {
                                               y.hinhthuc
                                           }).FirstOrDefault().hinhthuc,


                           });


            return lst.ToList();
        }
        public int FindMaCa(string ngaykham = "2022-09-09,Sáng", string hinhthuc = "Khám trong giờ", int mand = 1)
        {
            string[] name = ngaykham.Split(',');
            string ngay = name[0], ca = name[1];
            modelWeb db = new modelWeb();
            int ma = (int)(from y in db.CaKhams

                           where (y.ngaykham).ToString() == ngay && y.ca == ca && y.hinhthuc == hinhthuc && y.mand == mand
                           select new
                           {
                               y.maca
                           }).FirstOrDefault().maca;

            return ma;
        }

        public int FindMaBS(string khoa, string chinhanh)
        {
            modelWeb db = new modelWeb();

            var res = (from y in db.NguoiDungs
                       from x in db.Khoas
                       from z in db.ChiNhanhs

                       where y.makhoa == x.makhoa && y.machinhanh == z.machinhanh && x.tenkhoa == khoa && z.diachi == chinhanh
                       select new
                       {
                           y.mand
                       }).FirstOrDefault();
            int ma = 0;
            if (res != null) ma = res.mand;
            return ma;
        }


    }
}