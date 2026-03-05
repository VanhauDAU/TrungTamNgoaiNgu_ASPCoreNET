# Cấu trúc thư mục Models

Mục tiêu: tách model theo domain để dễ tìm bảng, dễ mở rộng và giảm nhầm lẫn khi dự án lớn dần.

## Cấu trúc hiện tại

- `Models/TaiKhoanVaPhanQuyen/TaiKhoanVaPhanQuyenModels.cs`
  - Bảng: `taikhoan`, `hosonguoidung`, `nhansu`, `nhomquyen`, `phanquyen`
- `Models/KhoaHocVaLopHoc/KhoaHocVaLopHocModels.cs`
  - Bảng: `danhmuckhoahoc`, `khoahoc`, `hocphi`, `cahoc`, `phonghoc`, `lophoc`, `dangkylophoc`, `buoihoc`, `diemdanh`
- `Models/TaiChinh/TaiChinhModels.cs`
  - Bảng: `hoadon`, `phieuthu`, `luong`, `luongchitiet`
- `Models/CoSoDaoTao/CoSoDaoTaoModels.cs`
  - Bảng: `cosodaotao`, `tinhthanh`
- `Models/NoiDungVaTuongTac/NoiDungVaTuongTacModels.cs`
  - Bảng: `danhmucbaiviet`, `tags`, `baiviet`, `lienhe`, `thongbao`, `thongbaonguoidung`, `settings`
- `Models/NhatKyHeThong/NhatKyHeThongModel.cs`
  - Bảng: `nhatkyhethong`
- `Models/Shared/ErrorViewModel.cs`
  - View model dùng cho trang lỗi.

## Quy ước đặt tên

- Thư mục: theo domain nghiệp vụ.
- Tên file: mô tả domain rõ nghĩa, bỏ số thứ tự kiểu `01_`, `02_`.
- Namespace giữ nguyên `TrungTamNgoaiNgu.Models` để không ảnh hưởng code đang dùng.

## Hướng nâng cấp tiếp theo (khuyến nghị)

Nếu muốn rõ hơn nữa, có thể tách mỗi bảng thành 1 file/class trong từng domain.
