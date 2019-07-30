namespace Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class firstDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BinhLuan",
                c => new
                    {
                        MaBL = c.Int(nullable: false, identity: true),
                        NoiDungBL = c.String(),
                        MaThanhVien = c.Int(),
                        MaSP = c.Int(),
                    })
                .PrimaryKey(t => t.MaBL)
                .ForeignKey("dbo.SanPham", t => t.MaSP)
                .ForeignKey("dbo.ThanhVien", t => t.MaThanhVien)
                .Index(t => t.MaThanhVien)
                .Index(t => t.MaSP);
            
            CreateTable(
                "dbo.SanPham",
                c => new
                    {
                        MaSP = c.Int(nullable: false, identity: true),
                        TenSP = c.String(maxLength: 255),
                        DonGia = c.Decimal(precision: 18, scale: 0),
                        NgayCapNhap = c.DateTime(),
                        CauHinh = c.String(),
                        MoTa = c.String(),
                        HinhAnh = c.String(),
                        SoLuongTon = c.Int(),
                        LuotXem = c.Int(),
                        LuotBinhChon = c.Int(),
                        LuotBinhLuan = c.Int(),
                        SoLanMua = c.Int(),
                        Moi = c.Int(),
                        MaNCC = c.Int(),
                        MaNSX = c.Int(),
                        MaLoaiSP = c.Int(),
                        DaXoa = c.Boolean(),
                        HinhAnh1 = c.String(),
                        HinhAnh2 = c.String(),
                        HinhAnh3 = c.String(),
                        HinhAnh4 = c.String(),
                    })
                .PrimaryKey(t => t.MaSP)
                .ForeignKey("dbo.NhaCungCap", t => t.MaNCC)
                .ForeignKey("dbo.LoaiSanPham", t => t.MaLoaiSP)
                .ForeignKey("dbo.NhaSanXuat", t => t.MaNSX)
                .Index(t => t.MaNCC)
                .Index(t => t.MaNSX)
                .Index(t => t.MaLoaiSP);
            
            CreateTable(
                "dbo.ChiTietDonDatHang",
                c => new
                    {
                        MaChiTietDDH = c.Int(nullable: false, identity: true),
                        MaDDH = c.Int(),
                        MaSP = c.Int(),
                        TenSP = c.String(maxLength: 255),
                        SoLuong = c.Int(),
                        DonGia = c.Decimal(precision: 18, scale: 0),
                    })
                .PrimaryKey(t => t.MaChiTietDDH)
                .ForeignKey("dbo.DonDatHang", t => t.MaDDH)
                .ForeignKey("dbo.SanPham", t => t.MaSP)
                .Index(t => t.MaDDH)
                .Index(t => t.MaSP);
            
            CreateTable(
                "dbo.DonDatHang",
                c => new
                    {
                        MaDDH = c.Int(nullable: false, identity: true),
                        NgayDat = c.DateTime(),
                        TinhTrangGiaoHang = c.Boolean(),
                        NgayGiao = c.DateTime(),
                        DaThanhToan = c.Boolean(),
                        MaKH = c.Int(),
                        UuDai = c.Int(),
                        DaHuy = c.Boolean(),
                        DaXoa = c.Boolean(),
                    })
                .PrimaryKey(t => t.MaDDH)
                .ForeignKey("dbo.KhachHang", t => t.MaKH, cascadeDelete: true)
                .Index(t => t.MaKH);
            
            CreateTable(
                "dbo.KhachHang",
                c => new
                    {
                        MaKH = c.Int(nullable: false, identity: true),
                        TenKH = c.String(maxLength: 100),
                        DiaChi = c.String(maxLength: 100),
                        Email = c.String(maxLength: 255),
                        SoDienThoai = c.String(maxLength: 255),
                        MaThanhVien = c.Int(),
                    })
                .PrimaryKey(t => t.MaKH)
                .ForeignKey("dbo.ThanhVien", t => t.MaThanhVien, cascadeDelete: true)
                .Index(t => t.MaThanhVien);
            
            CreateTable(
                "dbo.ThanhVien",
                c => new
                    {
                        MaThanhVien = c.Int(nullable: false, identity: true),
                        TaiKhoan = c.String(nullable: false, maxLength: 100),
                        MatKhau = c.String(nullable: false, maxLength: 100),
                        HoTen = c.String(maxLength: 100),
                        DiaChi = c.String(maxLength: 255),
                        Email = c.String(maxLength: 255),
                        SoDienThoai = c.String(maxLength: 12, unicode: false),
                        CauHoi = c.String(),
                        CauTraLoi = c.String(),
                        MaLoaiTV = c.Int(),
                    })
                .PrimaryKey(t => t.MaThanhVien)
                .ForeignKey("dbo.LoaiThanhVien", t => t.MaLoaiTV, cascadeDelete: true)
                .Index(t => t.MaLoaiTV);
            
            CreateTable(
                "dbo.LoaiThanhVien",
                c => new
                    {
                        MaLoaiTV = c.Int(nullable: false, identity: true),
                        TenLoai = c.String(maxLength: 50),
                        UuDai = c.Int(),
                    })
                .PrimaryKey(t => t.MaLoaiTV);
            
            CreateTable(
                "dbo.LoaiThanhVien_Quyen",
                c => new
                    {
                        MaLoaiTV = c.Int(nullable: false),
                        MaQuyen = c.String(nullable: false, maxLength: 50),
                        GhiChu = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => new { t.MaLoaiTV, t.MaQuyen })
                .ForeignKey("dbo.Quyen", t => t.MaQuyen)
                .ForeignKey("dbo.LoaiThanhVien", t => t.MaLoaiTV)
                .Index(t => t.MaLoaiTV)
                .Index(t => t.MaQuyen);
            
            CreateTable(
                "dbo.Quyen",
                c => new
                    {
                        MaQuyen = c.String(nullable: false, maxLength: 50),
                        TenQuyen = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.MaQuyen);
            
            CreateTable(
                "dbo.ChiTietPhieuNhap",
                c => new
                    {
                        MaChiTietPN = c.Int(nullable: false, identity: true),
                        MaPN = c.Int(),
                        MaSP = c.Int(),
                        DonGiaNhap = c.Decimal(precision: 18, scale: 0),
                        SoLuongNhap = c.Int(),
                    })
                .PrimaryKey(t => t.MaChiTietPN)
                .ForeignKey("dbo.PhieuNhap", t => t.MaPN)
                .ForeignKey("dbo.SanPham", t => t.MaSP)
                .Index(t => t.MaPN)
                .Index(t => t.MaSP);
            
            CreateTable(
                "dbo.PhieuNhap",
                c => new
                    {
                        MaPN = c.Int(nullable: false, identity: true),
                        MaNCC = c.Int(),
                        NgayNhap = c.DateTime(),
                        DaXoa = c.Boolean(),
                    })
                .PrimaryKey(t => t.MaPN)
                .ForeignKey("dbo.NhaCungCap", t => t.MaNCC, cascadeDelete: true)
                .Index(t => t.MaNCC);
            
            CreateTable(
                "dbo.NhaCungCap",
                c => new
                    {
                        MaNCC = c.Int(nullable: false, identity: true),
                        TenNCC = c.String(maxLength: 100),
                        DiaChi = c.String(maxLength: 255),
                        Email = c.String(maxLength: 255),
                        SoDienThoai = c.String(maxLength: 12, unicode: false),
                        Fax = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.MaNCC);
            
            CreateTable(
                "dbo.LoaiSanPham",
                c => new
                    {
                        MaLoaiSP = c.Int(nullable: false, identity: true),
                        TenLoai = c.String(maxLength: 100),
                        Icon = c.String(),
                        BiDanh = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.MaLoaiSP);
            
            CreateTable(
                "dbo.NhaSanXuat",
                c => new
                    {
                        MaNSX = c.Int(nullable: false, identity: true),
                        TenNSX = c.String(maxLength: 100),
                        ThongTin = c.String(maxLength: 255),
                        Logo = c.String(),
                    })
                .PrimaryKey(t => t.MaNSX);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SanPham", "MaNSX", "dbo.NhaSanXuat");
            DropForeignKey("dbo.SanPham", "MaLoaiSP", "dbo.LoaiSanPham");
            DropForeignKey("dbo.ChiTietPhieuNhap", "MaSP", "dbo.SanPham");
            DropForeignKey("dbo.SanPham", "MaNCC", "dbo.NhaCungCap");
            DropForeignKey("dbo.PhieuNhap", "MaNCC", "dbo.NhaCungCap");
            DropForeignKey("dbo.ChiTietPhieuNhap", "MaPN", "dbo.PhieuNhap");
            DropForeignKey("dbo.ChiTietDonDatHang", "MaSP", "dbo.SanPham");
            DropForeignKey("dbo.ThanhVien", "MaLoaiTV", "dbo.LoaiThanhVien");
            DropForeignKey("dbo.LoaiThanhVien_Quyen", "MaLoaiTV", "dbo.LoaiThanhVien");
            DropForeignKey("dbo.LoaiThanhVien_Quyen", "MaQuyen", "dbo.Quyen");
            DropForeignKey("dbo.KhachHang", "MaThanhVien", "dbo.ThanhVien");
            DropForeignKey("dbo.BinhLuan", "MaThanhVien", "dbo.ThanhVien");
            DropForeignKey("dbo.DonDatHang", "MaKH", "dbo.KhachHang");
            DropForeignKey("dbo.ChiTietDonDatHang", "MaDDH", "dbo.DonDatHang");
            DropForeignKey("dbo.BinhLuan", "MaSP", "dbo.SanPham");
            DropIndex("dbo.PhieuNhap", new[] { "MaNCC" });
            DropIndex("dbo.ChiTietPhieuNhap", new[] { "MaSP" });
            DropIndex("dbo.ChiTietPhieuNhap", new[] { "MaPN" });
            DropIndex("dbo.LoaiThanhVien_Quyen", new[] { "MaQuyen" });
            DropIndex("dbo.LoaiThanhVien_Quyen", new[] { "MaLoaiTV" });
            DropIndex("dbo.ThanhVien", new[] { "MaLoaiTV" });
            DropIndex("dbo.KhachHang", new[] { "MaThanhVien" });
            DropIndex("dbo.DonDatHang", new[] { "MaKH" });
            DropIndex("dbo.ChiTietDonDatHang", new[] { "MaSP" });
            DropIndex("dbo.ChiTietDonDatHang", new[] { "MaDDH" });
            DropIndex("dbo.SanPham", new[] { "MaLoaiSP" });
            DropIndex("dbo.SanPham", new[] { "MaNSX" });
            DropIndex("dbo.SanPham", new[] { "MaNCC" });
            DropIndex("dbo.BinhLuan", new[] { "MaSP" });
            DropIndex("dbo.BinhLuan", new[] { "MaThanhVien" });
            DropTable("dbo.NhaSanXuat");
            DropTable("dbo.LoaiSanPham");
            DropTable("dbo.NhaCungCap");
            DropTable("dbo.PhieuNhap");
            DropTable("dbo.ChiTietPhieuNhap");
            DropTable("dbo.Quyen");
            DropTable("dbo.LoaiThanhVien_Quyen");
            DropTable("dbo.LoaiThanhVien");
            DropTable("dbo.ThanhVien");
            DropTable("dbo.KhachHang");
            DropTable("dbo.DonDatHang");
            DropTable("dbo.ChiTietDonDatHang");
            DropTable("dbo.SanPham");
            DropTable("dbo.BinhLuan");
        }
    }
}
