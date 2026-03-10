using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrungTamNgoaiNgu.Migrations
{
    /// <summary>
    /// Migration tạo các bảng học tập bổ sung:
    /// 1. baithi: Bài kiểm tra/thi cuối kỳ
    /// 2. diembaithi: Điểm thi của học viên
    /// 3. danhgiagiaovien: Học viên đánh giá giáo viên
    /// 4. phanhoi: Phản hồi học viên về buổi học
    /// 5. tailieu: Tài liệu học tập
    /// 6. noidungbaihoc: Nội dung chi tiết từng buổi học
    /// </summary>
    public partial class TaoBangHocTapBoSung : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // =============================================
            // Bảng baithi: Bài kiểm tra gắn với khóa học
            // =============================================
            migrationBuilder.CreateTable(
                name: "baithi",
                columns: table => new
                {
                    baiThiId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    khoaHocId = table.Column<int>(type: "int", nullable: true,
                        comment: "FK -> khoahoc.khoaHocId - Bài thi thuộc khóa học nào"),
                    tenBaiThi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    moTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ngayThi = table.Column<DateOnly>(type: "date", nullable: true,
                        comment: "Ngày tổ chức thi"),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_baithi", x => x.baiThiId);
                    table.ForeignKey(
                        name: "FK_baithi_khoahoc_khoaHocId",
                        column: x => x.khoaHocId,
                        principalTable: "khoahoc",
                        principalColumn: "khoaHocId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex("IX_baithi_khoaHocId", "baithi", "khoaHocId");

            // =============================================
            // Bảng diembaithi: Điểm số bài thi của từng học viên
            // =============================================
            migrationBuilder.CreateTable(
                name: "diembaithi",
                columns: table => new
                {
                    diemThiId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    taiKhoanId = table.Column<int>(type: "int", nullable: true,
                        comment: "FK -> taikhoan.taiKhoanId - Học viên"),
                    baiThiId = table.Column<int>(type: "int", nullable: true,
                        comment: "FK -> baithi.baiThiId"),
                    diemSo = table.Column<decimal>(type: "decimal(4,2)", nullable: true,
                        comment: "Điểm số (ví dụ: 8.50, thang điểm 10)"),
                    ghiChu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_diembaithi", x => x.diemThiId);
                    table.ForeignKey(
                        name: "FK_diembaithi_taikhoan_taiKhoanId",
                        column: x => x.taiKhoanId,
                        principalTable: "taikhoan",
                        principalColumn: "taiKhoanId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_diembaithi_baithi_baiThiId",
                        column: x => x.baiThiId,
                        principalTable: "baithi",
                        principalColumn: "baiThiId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex("IX_diembaithi_taiKhoanId", "diembaithi", "taiKhoanId");
            migrationBuilder.CreateIndex("IX_diembaithi_baiThiId", "diembaithi", "baiThiId");

            // =============================================
            // Bảng danhgiagiaovien: Học viên đánh giá giáo viên sau khóa học
            // =============================================
            migrationBuilder.CreateTable(
                name: "danhgiagiaovien",
                columns: table => new
                {
                    danhGiaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    giaoVienId = table.Column<int>(type: "int", nullable: true,
                        comment: "FK -> taikhoan.taiKhoanId (role=1 giáo viên)"),
                    hocVienId = table.Column<int>(type: "int", nullable: true,
                        comment: "FK -> taikhoan.taiKhoanId (role=0 học viên)"),
                    lopHocId = table.Column<int>(type: "int", nullable: true,
                        comment: "FK -> lophoc.lopHocId"),
                    soSao = table.Column<byte>(type: "tinyint", nullable: true,
                        comment: "Số sao đánh giá (1-5)"),
                    noiDung = table.Column<string>(type: "nvarchar(max)", nullable: true,
                        comment: "Nhận xét chi tiết"),
                    ngayDanhGia = table.Column<DateOnly>(type: "date", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_danhgiagiaovien", x => x.danhGiaId);
                    table.ForeignKey(
                        name: "FK_danhgiagiaovien_taikhoan_giaoVienId",
                        column: x => x.giaoVienId,
                        principalTable: "taikhoan",
                        principalColumn: "taiKhoanId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_danhgiagiaovien_taikhoan_hocVienId",
                        column: x => x.hocVienId,
                        principalTable: "taikhoan",
                        principalColumn: "taiKhoanId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_danhgiagiaovien_lophoc_lopHocId",
                        column: x => x.lopHocId,
                        principalTable: "lophoc",
                        principalColumn: "lopHocId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex("IX_danhgiagiaovien_giaoVienId", "danhgiagiaovien", "giaoVienId");
            migrationBuilder.CreateIndex("IX_danhgiagiaovien_hocVienId", "danhgiagiaovien", "hocVienId");
            migrationBuilder.CreateIndex("IX_danhgiagiaovien_lopHocId", "danhgiagiaovien", "lopHocId");

            // =============================================
            // Bảng phanhoi: Phản hồi học viên về buổi học
            // =============================================
            migrationBuilder.CreateTable(
                name: "phanhoi",
                columns: table => new
                {
                    phanHoiId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tieuDe = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    noiDung = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    taiKhoanId = table.Column<int>(type: "int", nullable: true,
                        comment: "FK -> taikhoan.taiKhoanId - Học viên gửi phản hồi"),
                    danhGia = table.Column<byte>(type: "tinyint", nullable: true,
                        comment: "Điểm đánh giá 1-5 sao"),
                    buoiHocId = table.Column<int>(type: "int", nullable: true,
                        comment: "FK -> buoihoc.buoiHocId - Buổi học được phản hồi"),
                    trangThai = table.Column<byte>(type: "tinyint", nullable: true,
                        comment: "0=Chờ duyệt, 1=Đã duyệt, 2=Từ chối"),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_phanhoi", x => x.phanHoiId);
                    table.ForeignKey(
                        name: "FK_phanhoi_taikhoan_taiKhoanId",
                        column: x => x.taiKhoanId,
                        principalTable: "taikhoan",
                        principalColumn: "taiKhoanId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_phanhoi_buoihoc_buoiHocId",
                        column: x => x.buoiHocId,
                        principalTable: "buoihoc",
                        principalColumn: "buoiHocId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex("IX_phanhoi_taiKhoanId", "phanhoi", "taiKhoanId");
            migrationBuilder.CreateIndex("IX_phanhoi_buoiHocId", "phanhoi", "buoiHocId");

            // =============================================
            // Bảng tailieu: Quản lý tài liệu học tập
            // =============================================
            migrationBuilder.CreateTable(
                name: "tailieu",
                columns: table => new
                {
                    taiLieuId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false,
                        comment: "UUID hoặc mã tài liệu"),
                    tenTaiLieu = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    moTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    loaiTaiLieu = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true,
                        comment: "pdf | docx | pptx | mp4 | zip..."),
                    taiKhoanId = table.Column<int>(type: "int", nullable: true,
                        comment: "FK -> taikhoan.taiKhoanId - Người upload"),
                    duongDan = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true,
                        comment: "Đường dẫn lưu trữ file"),
                    khoaHocId = table.Column<int>(type: "int", nullable: true,
                        comment: "FK -> khoahoc.khoaHocId - Thuộc khóa học"),
                    buoiHocId = table.Column<int>(type: "int", nullable: true,
                        comment: "FK -> buoihoc.buoiHocId - Thuộc buổi học cụ thể"),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tailieu", x => x.taiLieuId);
                    table.ForeignKey(
                        name: "FK_tailieu_taikhoan_taiKhoanId",
                        column: x => x.taiKhoanId,
                        principalTable: "taikhoan",
                        principalColumn: "taiKhoanId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_tailieu_khoahoc_khoaHocId",
                        column: x => x.khoaHocId,
                        principalTable: "khoahoc",
                        principalColumn: "khoaHocId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex("IX_tailieu_taiKhoanId", "tailieu", "taiKhoanId");
            migrationBuilder.CreateIndex("IX_tailieu_khoaHocId", "tailieu", "khoaHocId");

            // =============================================
            // Bảng noidungbaihoc: Nội dung chi tiết từng buổi học
            // =============================================
            migrationBuilder.CreateTable(
                name: "noidungbaihoc",
                columns: table => new
                {
                    noiDungId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    buoiHocId = table.Column<int>(type: "int", nullable: true,
                        comment: "FK -> buoihoc.buoiHocId"),
                    tieuDe = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true,
                        comment: "Tiêu đề nội dung bài học"),
                    noiDung = table.Column<string>(type: "nvarchar(max)", nullable: true,
                        comment: "Nội dung chi tiết (HTML hoặc text)"),
                    taiLieuId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true,
                        comment: "FK -> tailieu.taiLieuId (tài liệu đính kèm)"),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_noidungbaihoc", x => x.noiDungId);
                    table.ForeignKey(
                        name: "FK_noidungbaihoc_buoihoc_buoiHocId",
                        column: x => x.buoiHocId,
                        principalTable: "buoihoc",
                        principalColumn: "buoiHocId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_noidungbaihoc_tailieu_taiLieuId",
                        column: x => x.taiLieuId,
                        principalTable: "tailieu",
                        principalColumn: "taiLieuId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex("IX_noidungbaihoc_buoiHocId", "noidungbaihoc", "buoiHocId");
            migrationBuilder.CreateIndex("IX_noidungbaihoc_taiLieuId", "noidungbaihoc", "taiLieuId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("noidungbaihoc");
            migrationBuilder.DropTable("tailieu");
            migrationBuilder.DropTable("phanhoi");
            migrationBuilder.DropTable("danhgiagiaovien");
            migrationBuilder.DropTable("diembaithi");
            migrationBuilder.DropTable("baithi");
        }
    }
}
