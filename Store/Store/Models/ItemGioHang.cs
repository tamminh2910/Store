using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Store.Models
{
    public class ItemGioHang
    {
        public int MaSP { get; set; }
        public string TenSP { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien { get; set; }
        public string HinhAnh { get; set; }

        public ItemGioHang()
        {

        }
        public ItemGioHang(int maSP)
        {
            using (var db = new BanHangDbContext())
            {
                this.MaSP = maSP;
                var sp = db.SanPhams.Single(x => x.MaSP == maSP);
                this.TenSP = sp.TenSP;
                this.HinhAnh = sp.HinhAnh;
                this.SoLuong = 1;
                this.DonGia = sp.DonGia.Value;
                this.ThanhTien = DonGia * SoLuong;
            }
        }
        public ItemGioHang(int maSP, int sl)
        {
            using (var db = new BanHangDbContext())
            {
                this.MaSP = maSP;
                this.SoLuong = sl;
                var sp = db.SanPhams.Single(x => x.MaSP == maSP);
                this.TenSP = sp.TenSP;
                this.HinhAnh = sp.HinhAnh;
                this.DonGia = sp.DonGia.Value;
                this.ThanhTien = DonGia * SoLuong;
            }
        }
    }
}