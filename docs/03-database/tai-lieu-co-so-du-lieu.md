# 03 - Database

## 1. Nen tang CSDL

- SQL Server + EF Core Code-First.
- DbContext: `Data/AppDbContext.cs`.
- Design-time factory: `Data/AppDbContextFactory.cs`.
- Migration hien co:
  - `20260303095750_TaoDatabase_LanDau`
  - `20260303111159_AddSoftDeleteToDanhMuc`
  - `20260303195100_AddNhatKyHeThongTable`

## 2. Nhom bang trong he thong

### Nhom 1 - Tai khoan & phan quyen

- `taikhoan`
- `hosonguoidung`
- `nhansu`
- `nhomquyen`
- `phanquyen`

### Nhom 2 - Khoa hoc & lop hoc

- `danhmuckhoahoc`
- `khoahoc`
- `hocphi`
- `cahoc`
- `phonghoc`
- `lophoc`
- `dangkylophoc`
- `buoihoc`
- `diemdanh`

### Nhom 3 - Tai chinh

- `hoadon`
- `phieuthu`
- `luong`
- `luongchitiet`

### Nhom 4 - Co so dao tao

- `cosodaotao`
- `tinhthanh`

### Nhom 5 - Noi dung & tuong tac

- `danhmucbaiviet`
- `tags`
- `baiviet`
- `lienhe`
- `thongbao`
- `thongbaonguoidung`
- `settings`

### Nhom 6 - Audit

- `nhatkyhethong`

## 3. Quan he nghiep vu quan trong

- 1 `DanhMucKhoaHoc` -> n `KhoaHoc`.
- 1 `KhoaHoc` -> n `LopHoc`.
- 1 `KhoaHoc` -> n `HocPhi`.
- 1 `LopHoc` -> n `BuoiHoc`.
- 1 `LopHoc` -> n `DangKyLopHoc`.
- 1 `DangKyLopHoc` -> 1 `HoaDon` (hien tai qua navigation).
- 1 `HoaDon` -> n `PhieuThu`.
- `BaiViet` many-to-many voi `DanhMucBaiViet` qua `baiviet_danhmuc`.
- `BaiViet` many-to-many voi `Tag` qua `baiviet_tag`.
- `ThongBao` -> n `ThongBaoNguoiDung`.

## 4. Quy uoc trang thai quan trong

- `khoahoc.trangThai`: 0 = Tam ngung, 1 = Dang hoat dong.
- `lophoc.trangThai`: 0 = Sap mo, 1 = Dang mo, 2 = Da dong, 3 = Da huy, 4 = Dang hoc.
- `hoadon.trangThai`: 0 = Chua thanh toan, 1 = Thanh toan mot phan, 2 = Da thanh toan du.
- `phieuthu.trangThai`: 0 = Huy, 1 = Hop le.
- `danhmuckhoahoc.deleted_at` va `khoahoc.deleted_at` dung cho soft delete.

## 5. Cau hinh Fluent API dang dung

- One-to-one:
  - `TaiKhoan` - `HoSoNguoiDung`
  - `TaiKhoan` - `NhanSu`
- Many-to-many:
  - `BaiViet` - `DanhMucBaiViet`
  - `BaiViet` - `Tag`
- Column mapping dac biet:
  - `TaiKhoan.TenTaiKhoan` -> cột `taiKhoan`.
- Decimal precision da duoc khai bao cho bang tai chinh/luong/toa do.

## 6. Luu y migration quan trong

- Khi doi kieu/cau truc cot PK/FK, phai drop constraint truoc roi moi alter column.
- Thu tu an toan thuong la:
  1. Drop FK phu thuoc.
  2. Drop PK neu can alter cot PK.
  3. Alter column.
  4. Re-create PK.
  5. Re-create FK.
- Loi thuong gap: `The object ... is dependent on column ...`.

## 7. Checklist truoc khi chay migration

- Backup DB.
- Chay `dotnet ef migrations add <Name>`.
- Doc file migration vua tao, kiem tra lenh alter PK/FK co dung thu tu.
- Chay `dotnet ef database update` tren local.
- Chay smoke test man hinh co lien quan.
