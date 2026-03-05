# Cấu trúc thư mục Models

Mục tiêu: tách model theo domain để dễ tìm bảng, dễ mở rộng và giảm nhầm lẫn khi dự án lớn dần.

## Cấu trúc hiện tại

- `Models/TaiKhoanVaPhanQuyen/`
  - `TaiKhoan.cs`, `HoSoNguoiDung.cs`, `NhanSu.cs`, `NhomQuyen.cs`, `PhanQuyen.cs`
- `Models/KhoaHocVaLopHoc/`
  - `DanhMucKhoaHoc.cs`, `KhoaHoc.cs`, `HocPhi.cs`, `CaHoc.cs`, `PhongHoc.cs`, `LopHoc.cs`, `DangKyLopHoc.cs`, `BuoiHoc.cs`, `DiemDanh.cs`
- `Models/TaiChinh/`
  - `HoaDon.cs`, `PhieuThu.cs`, `Luong.cs`, `LuongChiTiet.cs`
- `Models/CoSoDaoTao/`
  - `CoSoDaoTao.cs`, `TinhThanh.cs`
- `Models/NoiDungVaTuongTac/`
  - `DanhMucBaiViet.cs`, `Tag.cs`, `BaiViet.cs`, `LienHe.cs`, `ThongBao.cs`, `ThongBaoNguoiDung.cs`, `Setting.cs`
- `Models/NhatKyHeThong/`
  - `NhatKyHeThong.cs`
- `Models/Shared/ErrorViewModel.cs`
  - View model dùng cho trang lỗi.

## Quy ước đặt tên

- Thư mục: theo domain nghiệp vụ.
- Tên file: mô tả domain rõ nghĩa, bỏ số thứ tự kiểu `01_`, `02_`.
- Namespace giữ nguyên `TrungTamNgoaiNgu.Models` để không ảnh hưởng code đang dùng.

## Trạng thái

Đã tách theo chuẩn: mỗi bảng (mỗi class model) một file.
