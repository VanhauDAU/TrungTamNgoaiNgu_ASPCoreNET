using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrungTamNgoaiNgu.Migrations
{
    /// <inheritdoc />
    public partial class TaoDatabase_LanDau : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cahoc",
                columns: table => new
                {
                    caHocId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tenCa = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    gioBatDau = table.Column<TimeOnly>(type: "time", nullable: true),
                    gioKetThuc = table.Column<TimeOnly>(type: "time", nullable: true),
                    trangThai = table.Column<byte>(type: "tinyint", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cahoc", x => x.caHocId);
                });

            migrationBuilder.CreateTable(
                name: "danhmucbaiviet",
                columns: table => new
                {
                    danhMucId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tenDanhMuc = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    slug = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    moTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    trangThai = table.Column<byte>(type: "tinyint", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_danhmucbaiviet", x => x.danhMucId);
                });

            migrationBuilder.CreateTable(
                name: "danhmuckhoahoc",
                columns: table => new
                {
                    danhMucId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tenDanhMuc = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    slug = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    moTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    trangThai = table.Column<byte>(type: "tinyint", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_danhmuckhoahoc", x => x.danhMucId);
                });

            migrationBuilder.CreateTable(
                name: "lienhe",
                columns: table => new
                {
                    lienHeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    hoTen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    soDienThoai = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    tieuDe = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    noiDung = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    trangThai = table.Column<byte>(type: "tinyint", nullable: true),
                    taiKhoanId = table.Column<int>(type: "int", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lienhe", x => x.lienHeId);
                });

            migrationBuilder.CreateTable(
                name: "nhomquyen",
                columns: table => new
                {
                    nhomQuyenId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tenNhom = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    moTa = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nhomquyen", x => x.nhomQuyenId);
                });

            migrationBuilder.CreateTable(
                name: "settings",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    key = table.Column<string>(type: "nvarchar(191)", maxLength: 191, nullable: false),
                    value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    display_name = table.Column<string>(type: "nvarchar(191)", maxLength: 191, nullable: false),
                    type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    group = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_settings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tags",
                columns: table => new
                {
                    tagId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tenTag = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    slug = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tags", x => x.tagId);
                });

            migrationBuilder.CreateTable(
                name: "tinhthanh",
                columns: table => new
                {
                    tinhThanhId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    maAPI = table.Column<int>(type: "int", nullable: true),
                    tenTinhThanh = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    slug = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    division_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    codename = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tinhthanh", x => x.tinhThanhId);
                });

            migrationBuilder.CreateTable(
                name: "khoahoc",
                columns: table => new
                {
                    khoaHocId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    danhMucId = table.Column<int>(type: "int", nullable: true),
                    tenKhoaHoc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    slug = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    anhKhoaHoc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    moTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    doiTuong = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ketQuaDatDuoc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    yeuCauDauVao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    trangThai = table.Column<byte>(type: "tinyint", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_khoahoc", x => x.khoaHocId);
                    table.ForeignKey(
                        name: "FK_khoahoc_danhmuckhoahoc_danhMucId",
                        column: x => x.danhMucId,
                        principalTable: "danhmuckhoahoc",
                        principalColumn: "danhMucId");
                });

            migrationBuilder.CreateTable(
                name: "phanquyen",
                columns: table => new
                {
                    phanQuyenId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nhomQuyenId = table.Column<int>(type: "int", nullable: false),
                    tinhNang = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    coXem = table.Column<bool>(type: "bit", nullable: false),
                    coThem = table.Column<bool>(type: "bit", nullable: false),
                    coSua = table.Column<bool>(type: "bit", nullable: false),
                    coXoa = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_phanquyen", x => x.phanQuyenId);
                    table.ForeignKey(
                        name: "FK_phanquyen_nhomquyen_nhomQuyenId",
                        column: x => x.nhomQuyenId,
                        principalTable: "nhomquyen",
                        principalColumn: "nhomQuyenId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "taikhoan",
                columns: table => new
                {
                    taiKhoanId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    taiKhoan = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    matKhau = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    role = table.Column<byte>(type: "tinyint", nullable: false),
                    nhomQuyenId = table.Column<int>(type: "int", nullable: true),
                    trangThai = table.Column<byte>(type: "tinyint", nullable: false),
                    lastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_taikhoan", x => x.taiKhoanId);
                    table.ForeignKey(
                        name: "FK_taikhoan_nhomquyen_nhomQuyenId",
                        column: x => x.nhomQuyenId,
                        principalTable: "nhomquyen",
                        principalColumn: "nhomQuyenId");
                });

            migrationBuilder.CreateTable(
                name: "cosodaotao",
                columns: table => new
                {
                    coSoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    maCoSo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    slug = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    tenCoSo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    diaChi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    soDienThoai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    banDoGoogle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    tinhThanhId = table.Column<int>(type: "int", nullable: true),
                    tenPhuongXa = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    viDo = table.Column<decimal>(type: "decimal(10,7)", nullable: true),
                    kinhDo = table.Column<decimal>(type: "decimal(10,7)", nullable: true),
                    ngayKhaiTruong = table.Column<DateOnly>(type: "date", nullable: true),
                    trangThai = table.Column<byte>(type: "tinyint", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cosodaotao", x => x.coSoId);
                    table.ForeignKey(
                        name: "FK_cosodaotao_tinhthanh_tinhThanhId",
                        column: x => x.tinhThanhId,
                        principalTable: "tinhthanh",
                        principalColumn: "tinhThanhId");
                });

            migrationBuilder.CreateTable(
                name: "hocphi",
                columns: table => new
                {
                    hocPhiId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    khoaHocId = table.Column<int>(type: "int", nullable: true),
                    soBuoi = table.Column<int>(type: "int", nullable: true),
                    donGia = table.Column<decimal>(type: "decimal(15,2)", nullable: true),
                    trangThai = table.Column<byte>(type: "tinyint", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hocphi", x => x.hocPhiId);
                    table.ForeignKey(
                        name: "FK_hocphi_khoahoc_khoaHocId",
                        column: x => x.khoaHocId,
                        principalTable: "khoahoc",
                        principalColumn: "khoaHocId");
                });

            migrationBuilder.CreateTable(
                name: "baiviet",
                columns: table => new
                {
                    baiVietId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tieuDe = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    slug = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    tomTat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    noiDung = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    anhDaiDien = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    taiKhoanId = table.Column<int>(type: "int", nullable: false),
                    luotXem = table.Column<int>(type: "int", nullable: false),
                    trangThai = table.Column<byte>(type: "tinyint", nullable: false),
                    published_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_baiviet", x => x.baiVietId);
                    table.ForeignKey(
                        name: "FK_baiviet_taikhoan_taiKhoanId",
                        column: x => x.taiKhoanId,
                        principalTable: "taikhoan",
                        principalColumn: "taiKhoanId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "hosonguoidung",
                columns: table => new
                {
                    taiKhoanId = table.Column<int>(type: "int", nullable: false),
                    hoTen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    soDienThoai = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    zalo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ngaySinh = table.Column<DateOnly>(type: "date", nullable: true),
                    gioiTinh = table.Column<byte>(type: "tinyint", nullable: true),
                    diaChi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    nguoiGiamHo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    sdtGuardian = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    moiQuanHe = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    trinhDoHienTai = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    ngonNguMucTieu = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    nguonBietDen = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ghiChu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cccd = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    anhDaiDien = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hosonguoidung", x => x.taiKhoanId);
                    table.ForeignKey(
                        name: "FK_hosonguoidung_taikhoan_taiKhoanId",
                        column: x => x.taiKhoanId,
                        principalTable: "taikhoan",
                        principalColumn: "taiKhoanId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "luong",
                columns: table => new
                {
                    luongId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    taiKhoanId = table.Column<int>(type: "int", nullable: true),
                    thangLuong = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    tongLuongDay = table.Column<decimal>(type: "decimal(15,2)", nullable: true),
                    thuong = table.Column<decimal>(type: "decimal(15,2)", nullable: false),
                    phat = table.Column<decimal>(type: "decimal(15,2)", nullable: false),
                    phuCap = table.Column<decimal>(type: "decimal(15,2)", nullable: false),
                    tongTienThucLanh = table.Column<decimal>(type: "decimal(15,2)", nullable: true),
                    tongBuoiDay = table.Column<int>(type: "int", nullable: true),
                    ngayThanhToan = table.Column<DateOnly>(type: "date", nullable: true),
                    trangThai = table.Column<byte>(type: "tinyint", nullable: true),
                    ghiChu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_luong", x => x.luongId);
                    table.ForeignKey(
                        name: "FK_luong_taikhoan_taiKhoanId",
                        column: x => x.taiKhoanId,
                        principalTable: "taikhoan",
                        principalColumn: "taiKhoanId");
                });

            migrationBuilder.CreateTable(
                name: "thongbao",
                columns: table => new
                {
                    thongBaoId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tieuDe = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    noiDung = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    nguoiGuiId = table.Column<int>(type: "int", nullable: true),
                    doiTuongGui = table.Column<byte>(type: "tinyint", nullable: true),
                    doiTuongId = table.Column<long>(type: "bigint", nullable: true),
                    loaiGui = table.Column<byte>(type: "tinyint", nullable: false),
                    uuTien = table.Column<byte>(type: "tinyint", nullable: false),
                    ghim = table.Column<bool>(type: "bit", nullable: false),
                    hinhAnh = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ngayGui = table.Column<DateTime>(type: "datetime2", nullable: false),
                    trangThai = table.Column<byte>(type: "tinyint", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_thongbao", x => x.thongBaoId);
                    table.ForeignKey(
                        name: "FK_thongbao_taikhoan_nguoiGuiId",
                        column: x => x.nguoiGuiId,
                        principalTable: "taikhoan",
                        principalColumn: "taiKhoanId");
                });

            migrationBuilder.CreateTable(
                name: "nhansu",
                columns: table => new
                {
                    taiKhoanId = table.Column<int>(type: "int", nullable: false),
                    maNhanVien = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    chucVu = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    luongCoBan = table.Column<decimal>(type: "decimal(15,2)", nullable: true),
                    ngayVaoLam = table.Column<DateOnly>(type: "date", nullable: true),
                    chuyenMon = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    bangCap = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    hocVi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    coSoId = table.Column<int>(type: "int", nullable: true),
                    loaiHopDong = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    trangThai = table.Column<byte>(type: "tinyint", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nhansu", x => x.taiKhoanId);
                    table.ForeignKey(
                        name: "FK_nhansu_cosodaotao_coSoId",
                        column: x => x.coSoId,
                        principalTable: "cosodaotao",
                        principalColumn: "coSoId");
                    table.ForeignKey(
                        name: "FK_nhansu_taikhoan_taiKhoanId",
                        column: x => x.taiKhoanId,
                        principalTable: "taikhoan",
                        principalColumn: "taiKhoanId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "phonghoc",
                columns: table => new
                {
                    phongHocId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tenPhong = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    sucChua = table.Column<int>(type: "int", nullable: true),
                    trangThietBi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    coSoId = table.Column<int>(type: "int", nullable: true),
                    trangThai = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_phonghoc", x => x.phongHocId);
                    table.ForeignKey(
                        name: "FK_phonghoc_cosodaotao_coSoId",
                        column: x => x.coSoId,
                        principalTable: "cosodaotao",
                        principalColumn: "coSoId");
                });

            migrationBuilder.CreateTable(
                name: "baiviet_danhmuc",
                columns: table => new
                {
                    BaiVietsBaiVietId = table.Column<int>(type: "int", nullable: false),
                    DanhMucsDanhMucId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_baiviet_danhmuc", x => new { x.BaiVietsBaiVietId, x.DanhMucsDanhMucId });
                    table.ForeignKey(
                        name: "FK_baiviet_danhmuc_baiviet_BaiVietsBaiVietId",
                        column: x => x.BaiVietsBaiVietId,
                        principalTable: "baiviet",
                        principalColumn: "baiVietId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_baiviet_danhmuc_danhmucbaiviet_DanhMucsDanhMucId",
                        column: x => x.DanhMucsDanhMucId,
                        principalTable: "danhmucbaiviet",
                        principalColumn: "danhMucId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "baiviet_tag",
                columns: table => new
                {
                    BaiVietsBaiVietId = table.Column<int>(type: "int", nullable: false),
                    TagsTagId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_baiviet_tag", x => new { x.BaiVietsBaiVietId, x.TagsTagId });
                    table.ForeignKey(
                        name: "FK_baiviet_tag_baiviet_BaiVietsBaiVietId",
                        column: x => x.BaiVietsBaiVietId,
                        principalTable: "baiviet",
                        principalColumn: "baiVietId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_baiviet_tag_tags_TagsTagId",
                        column: x => x.TagsTagId,
                        principalTable: "tags",
                        principalColumn: "tagId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "thongbaonguoidung",
                columns: table => new
                {
                    thongBaoNguoiDungId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    thongBaoId = table.Column<long>(type: "bigint", nullable: true),
                    taiKhoanId = table.Column<int>(type: "int", nullable: true),
                    daDoc = table.Column<bool>(type: "bit", nullable: false),
                    ngayDoc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_thongbaonguoidung", x => x.thongBaoNguoiDungId);
                    table.ForeignKey(
                        name: "FK_thongbaonguoidung_taikhoan_taiKhoanId",
                        column: x => x.taiKhoanId,
                        principalTable: "taikhoan",
                        principalColumn: "taiKhoanId");
                    table.ForeignKey(
                        name: "FK_thongbaonguoidung_thongbao_thongBaoId",
                        column: x => x.thongBaoId,
                        principalTable: "thongbao",
                        principalColumn: "thongBaoId");
                });

            migrationBuilder.CreateTable(
                name: "lophoc",
                columns: table => new
                {
                    lopHocId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    slug = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    tenLopHoc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    khoaHocId = table.Column<int>(type: "int", nullable: true),
                    phongHocId = table.Column<int>(type: "int", nullable: true),
                    taiKhoanId = table.Column<int>(type: "int", nullable: true),
                    hocPhiId = table.Column<long>(type: "bigint", nullable: true),
                    ngayBatDau = table.Column<DateOnly>(type: "date", nullable: true),
                    ngayKetThuc = table.Column<DateOnly>(type: "date", nullable: true),
                    soBuoiDuKien = table.Column<int>(type: "int", nullable: true),
                    soHocVienToiDa = table.Column<int>(type: "int", nullable: true),
                    donGiaDay = table.Column<decimal>(type: "decimal(15,2)", nullable: true),
                    lichHoc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    coSoId = table.Column<int>(type: "int", nullable: true),
                    caHocId = table.Column<int>(type: "int", nullable: false),
                    trangThai = table.Column<byte>(type: "tinyint", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lophoc", x => x.lopHocId);
                    table.ForeignKey(
                        name: "FK_lophoc_cahoc_caHocId",
                        column: x => x.caHocId,
                        principalTable: "cahoc",
                        principalColumn: "caHocId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_lophoc_cosodaotao_coSoId",
                        column: x => x.coSoId,
                        principalTable: "cosodaotao",
                        principalColumn: "coSoId");
                    table.ForeignKey(
                        name: "FK_lophoc_khoahoc_khoaHocId",
                        column: x => x.khoaHocId,
                        principalTable: "khoahoc",
                        principalColumn: "khoaHocId");
                    table.ForeignKey(
                        name: "FK_lophoc_phonghoc_phongHocId",
                        column: x => x.phongHocId,
                        principalTable: "phonghoc",
                        principalColumn: "phongHocId");
                });

            migrationBuilder.CreateTable(
                name: "buoihoc",
                columns: table => new
                {
                    buoiHocId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    lopHocId = table.Column<int>(type: "int", nullable: true),
                    tenBuoiHoc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ngayHoc = table.Column<DateOnly>(type: "date", nullable: true),
                    caHocId = table.Column<int>(type: "int", nullable: true),
                    phongHocId = table.Column<int>(type: "int", nullable: true),
                    taiKhoanId = table.Column<int>(type: "int", nullable: true),
                    ghiChu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    daDiemDanh = table.Column<bool>(type: "bit", nullable: false),
                    daHoanThanh = table.Column<bool>(type: "bit", nullable: false),
                    trangThai = table.Column<byte>(type: "tinyint", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_buoihoc", x => x.buoiHocId);
                    table.ForeignKey(
                        name: "FK_buoihoc_cahoc_caHocId",
                        column: x => x.caHocId,
                        principalTable: "cahoc",
                        principalColumn: "caHocId");
                    table.ForeignKey(
                        name: "FK_buoihoc_lophoc_lopHocId",
                        column: x => x.lopHocId,
                        principalTable: "lophoc",
                        principalColumn: "lopHocId");
                    table.ForeignKey(
                        name: "FK_buoihoc_phonghoc_phongHocId",
                        column: x => x.phongHocId,
                        principalTable: "phonghoc",
                        principalColumn: "phongHocId");
                });

            migrationBuilder.CreateTable(
                name: "dangkylophoc",
                columns: table => new
                {
                    dangKyLopHocId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    taiKhoanId = table.Column<int>(type: "int", nullable: true),
                    lopHocId = table.Column<int>(type: "int", nullable: true),
                    ngayDangKy = table.Column<DateOnly>(type: "date", nullable: true),
                    trangThai = table.Column<int>(type: "int", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dangkylophoc", x => x.dangKyLopHocId);
                    table.ForeignKey(
                        name: "FK_dangkylophoc_lophoc_lopHocId",
                        column: x => x.lopHocId,
                        principalTable: "lophoc",
                        principalColumn: "lopHocId");
                    table.ForeignKey(
                        name: "FK_dangkylophoc_taikhoan_taiKhoanId",
                        column: x => x.taiKhoanId,
                        principalTable: "taikhoan",
                        principalColumn: "taiKhoanId");
                });

            migrationBuilder.CreateTable(
                name: "luongchitiet",
                columns: table => new
                {
                    luongChiTietId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    luongId = table.Column<int>(type: "int", nullable: true),
                    lopHocId = table.Column<int>(type: "int", nullable: true),
                    soBuoiDay = table.Column<int>(type: "int", nullable: true),
                    donGiaMotBuoi = table.Column<decimal>(type: "decimal(15,2)", nullable: true),
                    tongTien = table.Column<decimal>(type: "decimal(15,2)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_luongchitiet", x => x.luongChiTietId);
                    table.ForeignKey(
                        name: "FK_luongchitiet_lophoc_lopHocId",
                        column: x => x.lopHocId,
                        principalTable: "lophoc",
                        principalColumn: "lopHocId");
                    table.ForeignKey(
                        name: "FK_luongchitiet_luong_luongId",
                        column: x => x.luongId,
                        principalTable: "luong",
                        principalColumn: "luongId");
                });

            migrationBuilder.CreateTable(
                name: "diemdanh",
                columns: table => new
                {
                    diemDanhId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    taiKhoanId = table.Column<int>(type: "int", nullable: true),
                    buoiHocId = table.Column<int>(type: "int", nullable: true),
                    trangThai = table.Column<byte>(type: "tinyint", nullable: true),
                    ghiChu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_diemdanh", x => x.diemDanhId);
                    table.ForeignKey(
                        name: "FK_diemdanh_buoihoc_buoiHocId",
                        column: x => x.buoiHocId,
                        principalTable: "buoihoc",
                        principalColumn: "buoiHocId");
                    table.ForeignKey(
                        name: "FK_diemdanh_taikhoan_taiKhoanId",
                        column: x => x.taiKhoanId,
                        principalTable: "taikhoan",
                        principalColumn: "taiKhoanId");
                });

            migrationBuilder.CreateTable(
                name: "hoadon",
                columns: table => new
                {
                    hoaDonId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    maHoaDon = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ngayLap = table.Column<DateOnly>(type: "date", nullable: true),
                    ngayHetHan = table.Column<DateOnly>(type: "date", nullable: true),
                    tongTien = table.Column<decimal>(type: "decimal(15,2)", nullable: true),
                    giamGia = table.Column<decimal>(type: "decimal(15,2)", nullable: false),
                    thue = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    tongTienSauThue = table.Column<decimal>(type: "decimal(15,2)", nullable: false),
                    daTra = table.Column<decimal>(type: "decimal(15,2)", nullable: true),
                    taiKhoanId = table.Column<int>(type: "int", nullable: true),
                    dangKyLopHocId = table.Column<int>(type: "int", nullable: true),
                    phuongThucThanhToan = table.Column<int>(type: "int", nullable: true),
                    loaiHoaDon = table.Column<byte>(type: "tinyint", nullable: false),
                    coSoId = table.Column<int>(type: "int", nullable: true),
                    trangThai = table.Column<byte>(type: "tinyint", nullable: true),
                    ghiChu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hoadon", x => x.hoaDonId);
                    table.ForeignKey(
                        name: "FK_hoadon_cosodaotao_coSoId",
                        column: x => x.coSoId,
                        principalTable: "cosodaotao",
                        principalColumn: "coSoId");
                    table.ForeignKey(
                        name: "FK_hoadon_dangkylophoc_dangKyLopHocId",
                        column: x => x.dangKyLopHocId,
                        principalTable: "dangkylophoc",
                        principalColumn: "dangKyLopHocId");
                    table.ForeignKey(
                        name: "FK_hoadon_taikhoan_taiKhoanId",
                        column: x => x.taiKhoanId,
                        principalTable: "taikhoan",
                        principalColumn: "taiKhoanId");
                });

            migrationBuilder.CreateTable(
                name: "phieuthu",
                columns: table => new
                {
                    phieuThuId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    maPhieuThu = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    hoaDonId = table.Column<int>(type: "int", nullable: true),
                    soTien = table.Column<decimal>(type: "decimal(15,2)", nullable: true),
                    phuongThucThanhToan = table.Column<byte>(type: "tinyint", nullable: false),
                    ngayThu = table.Column<DateOnly>(type: "date", nullable: true),
                    taiKhoanId = table.Column<int>(type: "int", nullable: true),
                    ghiChu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    trangThai = table.Column<byte>(type: "tinyint", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_phieuthu", x => x.phieuThuId);
                    table.ForeignKey(
                        name: "FK_phieuthu_hoadon_hoaDonId",
                        column: x => x.hoaDonId,
                        principalTable: "hoadon",
                        principalColumn: "hoaDonId");
                    table.ForeignKey(
                        name: "FK_phieuthu_taikhoan_taiKhoanId",
                        column: x => x.taiKhoanId,
                        principalTable: "taikhoan",
                        principalColumn: "taiKhoanId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_baiviet_taiKhoanId",
                table: "baiviet",
                column: "taiKhoanId");

            migrationBuilder.CreateIndex(
                name: "IX_baiviet_danhmuc_DanhMucsDanhMucId",
                table: "baiviet_danhmuc",
                column: "DanhMucsDanhMucId");

            migrationBuilder.CreateIndex(
                name: "IX_baiviet_tag_TagsTagId",
                table: "baiviet_tag",
                column: "TagsTagId");

            migrationBuilder.CreateIndex(
                name: "IX_buoihoc_caHocId",
                table: "buoihoc",
                column: "caHocId");

            migrationBuilder.CreateIndex(
                name: "IX_buoihoc_lopHocId",
                table: "buoihoc",
                column: "lopHocId");

            migrationBuilder.CreateIndex(
                name: "IX_buoihoc_phongHocId",
                table: "buoihoc",
                column: "phongHocId");

            migrationBuilder.CreateIndex(
                name: "IX_cosodaotao_tinhThanhId",
                table: "cosodaotao",
                column: "tinhThanhId");

            migrationBuilder.CreateIndex(
                name: "IX_dangkylophoc_lopHocId",
                table: "dangkylophoc",
                column: "lopHocId");

            migrationBuilder.CreateIndex(
                name: "IX_dangkylophoc_taiKhoanId",
                table: "dangkylophoc",
                column: "taiKhoanId");

            migrationBuilder.CreateIndex(
                name: "IX_diemdanh_buoiHocId",
                table: "diemdanh",
                column: "buoiHocId");

            migrationBuilder.CreateIndex(
                name: "IX_diemdanh_taiKhoanId",
                table: "diemdanh",
                column: "taiKhoanId");

            migrationBuilder.CreateIndex(
                name: "IX_hoadon_coSoId",
                table: "hoadon",
                column: "coSoId");

            migrationBuilder.CreateIndex(
                name: "IX_hoadon_dangKyLopHocId",
                table: "hoadon",
                column: "dangKyLopHocId",
                unique: true,
                filter: "[dangKyLopHocId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_hoadon_taiKhoanId",
                table: "hoadon",
                column: "taiKhoanId");

            migrationBuilder.CreateIndex(
                name: "IX_hocphi_khoaHocId",
                table: "hocphi",
                column: "khoaHocId");

            migrationBuilder.CreateIndex(
                name: "IX_khoahoc_danhMucId",
                table: "khoahoc",
                column: "danhMucId");

            migrationBuilder.CreateIndex(
                name: "IX_lophoc_caHocId",
                table: "lophoc",
                column: "caHocId");

            migrationBuilder.CreateIndex(
                name: "IX_lophoc_coSoId",
                table: "lophoc",
                column: "coSoId");

            migrationBuilder.CreateIndex(
                name: "IX_lophoc_khoaHocId",
                table: "lophoc",
                column: "khoaHocId");

            migrationBuilder.CreateIndex(
                name: "IX_lophoc_phongHocId",
                table: "lophoc",
                column: "phongHocId");

            migrationBuilder.CreateIndex(
                name: "IX_luong_taiKhoanId",
                table: "luong",
                column: "taiKhoanId");

            migrationBuilder.CreateIndex(
                name: "IX_luongchitiet_lopHocId",
                table: "luongchitiet",
                column: "lopHocId");

            migrationBuilder.CreateIndex(
                name: "IX_luongchitiet_luongId",
                table: "luongchitiet",
                column: "luongId");

            migrationBuilder.CreateIndex(
                name: "IX_nhansu_coSoId",
                table: "nhansu",
                column: "coSoId");

            migrationBuilder.CreateIndex(
                name: "IX_phanquyen_nhomQuyenId",
                table: "phanquyen",
                column: "nhomQuyenId");

            migrationBuilder.CreateIndex(
                name: "IX_phieuthu_hoaDonId",
                table: "phieuthu",
                column: "hoaDonId");

            migrationBuilder.CreateIndex(
                name: "IX_phieuthu_taiKhoanId",
                table: "phieuthu",
                column: "taiKhoanId");

            migrationBuilder.CreateIndex(
                name: "IX_phonghoc_coSoId",
                table: "phonghoc",
                column: "coSoId");

            migrationBuilder.CreateIndex(
                name: "IX_taikhoan_nhomQuyenId",
                table: "taikhoan",
                column: "nhomQuyenId");

            migrationBuilder.CreateIndex(
                name: "IX_thongbao_nguoiGuiId",
                table: "thongbao",
                column: "nguoiGuiId");

            migrationBuilder.CreateIndex(
                name: "IX_thongbaonguoidung_taiKhoanId",
                table: "thongbaonguoidung",
                column: "taiKhoanId");

            migrationBuilder.CreateIndex(
                name: "IX_thongbaonguoidung_thongBaoId",
                table: "thongbaonguoidung",
                column: "thongBaoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "baiviet_danhmuc");

            migrationBuilder.DropTable(
                name: "baiviet_tag");

            migrationBuilder.DropTable(
                name: "diemdanh");

            migrationBuilder.DropTable(
                name: "hocphi");

            migrationBuilder.DropTable(
                name: "hosonguoidung");

            migrationBuilder.DropTable(
                name: "lienhe");

            migrationBuilder.DropTable(
                name: "luongchitiet");

            migrationBuilder.DropTable(
                name: "nhansu");

            migrationBuilder.DropTable(
                name: "phanquyen");

            migrationBuilder.DropTable(
                name: "phieuthu");

            migrationBuilder.DropTable(
                name: "settings");

            migrationBuilder.DropTable(
                name: "thongbaonguoidung");

            migrationBuilder.DropTable(
                name: "danhmucbaiviet");

            migrationBuilder.DropTable(
                name: "baiviet");

            migrationBuilder.DropTable(
                name: "tags");

            migrationBuilder.DropTable(
                name: "buoihoc");

            migrationBuilder.DropTable(
                name: "luong");

            migrationBuilder.DropTable(
                name: "hoadon");

            migrationBuilder.DropTable(
                name: "thongbao");

            migrationBuilder.DropTable(
                name: "dangkylophoc");

            migrationBuilder.DropTable(
                name: "lophoc");

            migrationBuilder.DropTable(
                name: "taikhoan");

            migrationBuilder.DropTable(
                name: "cahoc");

            migrationBuilder.DropTable(
                name: "khoahoc");

            migrationBuilder.DropTable(
                name: "phonghoc");

            migrationBuilder.DropTable(
                name: "nhomquyen");

            migrationBuilder.DropTable(
                name: "danhmuckhoahoc");

            migrationBuilder.DropTable(
                name: "cosodaotao");

            migrationBuilder.DropTable(
                name: "tinhthanh");
        }
    }
}
