/* ============================================================
   DATA.SQL - Script tham khao kiem tra du lieu van hanh
   Duoc viet dua tren schema hien tai cua project.
   ============================================================ */

/* 1) Tong quan khoa hoc theo trang thai */
SELECT
    COUNT(*) AS tong_khoa_hoc,
    SUM(CASE WHEN deleted_at IS NULL AND trangThai = 1 THEN 1 ELSE 0 END) AS dang_hoat_dong,
    SUM(CASE WHEN deleted_at IS NULL AND trangThai = 0 THEN 1 ELSE 0 END) AS tam_ngung,
    SUM(CASE WHEN deleted_at IS NOT NULL THEN 1 ELSE 0 END) AS da_xoa_mem
FROM khoahoc;

/* 2) Khoa hoc dang bi chan tam ngung vi con lop dang mo/dang hoc */
SELECT DISTINCT
    kh.khoaHocId,
    kh.tenKhoaHoc,
    lh.lopHocId,
    lh.tenLopHoc,
    lh.trangThai AS lop_trang_thai
FROM khoahoc kh
JOIN lophoc lh ON lh.khoaHocId = kh.khoaHocId
WHERE kh.deleted_at IS NULL
  AND kh.trangThai = 1
  AND lh.trangThai IN (1, 4)
ORDER BY kh.khoaHocId DESC;

/* 3) Danh muc khong duoc xoa mem vi con khoa hoc active */
SELECT
    dm.danhMucId,
    dm.tenDanhMuc,
    COUNT(kh.khoaHocId) AS so_khoa_hoc_active
FROM danhmuckhoahoc dm
JOIN khoahoc kh ON kh.danhMucId = dm.danhMucId
WHERE dm.deleted_at IS NULL
  AND kh.deleted_at IS NULL
  AND kh.trangThai <> 0
GROUP BY dm.danhMucId, dm.tenDanhMuc
HAVING COUNT(kh.khoaHocId) > 0
ORDER BY so_khoa_hoc_active DESC;

/* 4) Cong no hoa don */
SELECT
    hd.hoaDonId,
    hd.maHoaDon,
    hd.tongTien,
    hd.tongTienSauThue,
    hd.daTra,
    (CASE WHEN hd.tongTienSauThue > 0 THEN hd.tongTienSauThue ELSE ISNULL(hd.tongTien, 0) END - ISNULL(hd.daTra, 0)) AS con_no,
    hd.trangThai
FROM hoadon hd
ORDER BY hd.created_at DESC;

/* 5) Doanh thu theo thang (chi tinh phieu thu hop le) */
SELECT
    YEAR(ngayThu) AS nam,
    MONTH(ngayThu) AS thang,
    SUM(ISNULL(soTien, 0)) AS doanh_thu
FROM phieuthu
WHERE trangThai = 1
  AND ngayThu IS NOT NULL
GROUP BY YEAR(ngayThu), MONTH(ngayThu)
ORDER BY nam DESC, thang DESC;

/* 6) 100 dong audit logs moi nhat */
SELECT TOP 100
    nhatKyId,
    module,
    hanhDong,
    nguoiThucHien,
    created_at
FROM nhatkyhethong
ORDER BY created_at DESC;

/* 7) Kiem tra thong bao chua co nguoi doc */
SELECT
    tb.thongBaoId,
    tb.tieuDe,
    COUNT(tbn.thongBaoNguoiDungId) AS tong_nguoi_nhan,
    SUM(CASE WHEN tbn.daDoc = 1 THEN 1 ELSE 0 END) AS da_doc,
    SUM(CASE WHEN tbn.daDoc = 0 THEN 1 ELSE 0 END) AS chua_doc
FROM thongbao tb
LEFT JOIN thongbaonguoidung tbn ON tbn.thongBaoId = tb.thongBaoId
GROUP BY tb.thongBaoId, tb.tieuDe
ORDER BY tb.thongBaoId DESC;
