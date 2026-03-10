using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrungTamNgoaiNgu.Migrations
{
    /// <summary>
    /// Migration redesign toàn bộ bảng DiemDanh:
    /// - Đổi PK từ string sang bigint AUTO_INCREMENT
    /// - Thêm các trạng thái chi tiết (vắng, đi trễ, có phép, nợ học phí)
    /// - Thêm hình thức (trực tiếp/online)
    /// - Thêm liên kết đăng ký lớp học
    /// - Thêm thông tin người thực hiện điểm danh
    /// - Thêm unique constraint (buoiHocId, taiKhoanId) để tránh điểm danh trùng
    /// </summary>
    public partial class TaiCauTrucDiemDanh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Xóa bảng diemdanh cũ (PK là string, thiếu nhiều trường)
            migrationBuilder.DropTable("diemdanh");

            // Tạo lại bảng DiemDanh với cấu trúc đầy đủ
            migrationBuilder.CreateTable(
                name: "diemDanh",
                columns: table => new
                {
                    diemDanhId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),

                    buoiHocId = table.Column<int>(type: "int", nullable: false,
                        comment: "FK -> buoihoc.buoiHocId - Buổi học được điểm danh"),

                    taiKhoanId = table.Column<int>(type: "int", nullable: false,
                        comment: "FK -> taikhoan.taiKhoanId - Học viên"),

                    dangKyLopHocId = table.Column<int>(type: "int", nullable: true,
                        comment: "FK -> dangkylophoc.dangKyLopHocId - Liên kết đăng ký lớp"),

                    trangThai = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)1,
                        comment: "0=Vắng, 1=Có mặt, 2=Đi trễ, 3=Có phép, 4=Bị khóa(Nợ HP)"),

                    coMat = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)0,
                        comment: "1 nếu có mặt hoặc đi trễ; dùng để thống kê nhanh"),

                    phutDiTre = table.Column<short>(type: "smallint", nullable: true,
                        comment: "Số phút đi trễ (chỉ điền khi trangThai=2)"),

                    lyDo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true,
                        comment: "Lý do vắng / trễ / có phép / nợ HP"),

                    hinhThuc = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)0,
                        comment: "0=Trực tiếp tại lớp, 1=Online"),

                    nguoiDiemDanhId = table.Column<int>(type: "int", nullable: true,
                        comment: "FK -> taikhoan.taiKhoanId - GV/Admin thực hiện điểm danh"),

                    thoiGianDiemDanh = table.Column<DateTime>(type: "datetime2", nullable: true,
                        comment: "Thời điểm ghi nhận điểm danh"),

                    ghiChu = table.Column<string>(type: "nvarchar(max)", nullable: true),

                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_diemDanh", x => x.diemDanhId);

                    // Mỗi học viên chỉ được điểm danh 1 lần trong 1 buổi học
                    table.UniqueConstraint("UQ_diemdanh_buoi_hocvien",
                        x => new { x.buoiHocId, x.taiKhoanId });

                    table.ForeignKey(
                        name: "FK_diemDanh_buoihoc_buoiHocId",
                        column: x => x.buoiHocId,
                        principalTable: "buoihoc",
                        principalColumn: "buoiHocId",
                        onDelete: ReferentialAction.Cascade);

                    table.ForeignKey(
                        name: "FK_diemDanh_taikhoan_taiKhoanId",
                        column: x => x.taiKhoanId,
                        principalTable: "taikhoan",
                        principalColumn: "taiKhoanId");

                    table.ForeignKey(
                        name: "FK_diemDanh_dangkylophoc_dangKyLopHocId",
                        column: x => x.dangKyLopHocId,
                        principalTable: "dangkylophoc",
                        principalColumn: "dangKyLopHocId",
                        onDelete: ReferentialAction.SetNull);

                    table.ForeignKey(
                        name: "FK_diemDanh_taikhoan_nguoiDiemDanhId",
                        column: x => x.nguoiDiemDanhId,
                        principalTable: "taikhoan",
                        principalColumn: "taiKhoanId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex("IX_diemDanh_taiKhoanId", "diemDanh", "taiKhoanId");
            migrationBuilder.CreateIndex("IX_diemDanh_dangKyLopHocId", "diemDanh", "dangKyLopHocId");
            migrationBuilder.CreateIndex("IX_diemDanh_trangThai", "diemDanh", "trangThai");
            migrationBuilder.CreateIndex("IX_diemDanh_nguoiDiemDanhId", "diemDanh", "nguoiDiemDanhId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("diemDanh");

            // Khôi phục bảng cũ (cấu trúc tối giản)
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
                });
        }
    }
}
