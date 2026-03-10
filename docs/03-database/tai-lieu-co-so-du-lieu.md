# Tài Liệu Cơ Sở Dữ Liệu

## 1. Sơ Đồ Quan Hệ (Tóm Tắt)

```
tinhthanh
    └── cosodaotao ──┬── phonghoc
                     └── nhansu ──── taikhoan
                                         │
taikhoan ────────────────────────────────┤
    │                                    │
    ├── hosonguoidung                    │
    ├── nhomquyen ──── phanquyen         │
    │                                    │
    └─── [học tập] ───────────────────────┘
            │
    danhmuckhoahoc ── khoahoc ── hocphi
                          │
                      lophoc ──────────── buoihoc ─── diemDanh
                          │                                │
                      dangkylophoc ─── hoadon ─── phieuthu
                                           │
                                      noidungbaihoc
                                      danhgiagiaovien
                                      baithi ── diembaithi

taikhoan ── [chat] ─── chat_rooms ─── chat_messages ─── chat_message_reactions
                           │               │              chat_message_attachments
                      chat_room_members   chat_message_deletes
                      chat_audit_logs

taikhoan ── [thông báo] ── thongbao ─── thongbaonguoidung
                                    └── thongbao_lichsu
                                    └── thongbao_tepdinh

taikhoan ── [blog] ── baiviet ─── baiviet_danhmuc ─── danhmucbaiviet
                              └── baiviet_tag ──────── tags

lienhe ─── lienhe_lichsu
       └── lienhe_phanhoi
```

---

## 2. Mô Tả Chi Tiết Các Bảng

### Nhóm: Tài Khoản & Phân Quyền

| Bảng              | Mô tả                                      | Ghi chú                         |
| ----------------- | ------------------------------------------ | ------------------------------- |
| `taikhoan`        | Tài khoản đăng nhập                        | role: 0=HV, 1=GV, 2=NV, 3=Admin |
| `hosonguoidung`   | Hồ sơ cá nhân                              | 1-1 với taikhoan                |
| `nhomquyen`       | Nhóm quyền tùy chỉnh                       | VD: Kế toán, Giáo viên          |
| `phanquyen`       | Chi tiết quyền CRUD theo tính năng         | coXem, coThem, coSua, coXoa     |
| `nhatky_dangnhap` | Lịch sử đăng nhập (IP, thời gian, kết quả) | Bảo mật brute-force             |

### Nhóm: Cơ Sở & Vật Chất

| Bảng         | Mô tả                          | Trạng thái                                   |
| ------------ | ------------------------------ | -------------------------------------------- |
| `cosodaotao` | Chi nhánh/cơ sở đào tạo        | trangThai: 0=Ngưng, 1=Hoạt động              |
| `phonghoc`   | Phòng học                      | 0=Bảo trì, 1=Hoạt động, 2=Tạm ngưng, 3=Ngưng |
| `cahoc`      | Ca học (giờ bắt đầu, kết thúc) | trangThai: 0/1                               |
| `tinhthanh`  | Danh sách tỉnh thành VN        | 63 tỉnh thành                                |

### Nhóm: Khóa Học & Lớp Học

| Bảng             | Mô tả                                             | Quan trọng                                 |
| ---------------- | ------------------------------------------------- | ------------------------------------------ |
| `danhmuckhoahoc` | Danh mục khóa học (hỗ trợ cây phân cấp parent_id) | sort_order                                 |
| `khoahoc`        | Khóa học                                          | soft delete, doiTuong, ketQuaDatDuoc       |
| `hocphi`         | Bảng giá học phí theo số buổi                     | Nhiều mức giá/khóa                         |
| `lophoc`         | Lớp học cụ thể                                    | maLopHoc, lichHoc JSON, soft delete        |
| `buoihoc`        | Từng buổi học                                     | daDiemDanh, daHoanThanh                    |
| `dangkylophoc`   | Đăng ký lớp của học viên                          | trangThai: 0=Chờ, 1=Đang học, 2=Hoàn thành |

### Nhóm: Điểm Danh & Học Tập

| Bảng              | Mô tả                                              |
| ----------------- | -------------------------------------------------- |
| `diemDanh`        | Điểm danh từng buổi (unique: buoiHocId+taiKhoanId) |
| `danhgiagiaovien` | Học viên đánh giá giáo viên (sao + nhận xét)       |
| `baithi`          | Bài kiểm tra/thi                                   |
| `diembaithi`      | Điểm thi của từng học viên                         |
| `tailieu`         | Tài liệu học tập (PDF, video...)                   |
| `noidungbaihoc`   | Nội dung chi tiết từng buổi                        |
| `phanhoi`         | Phản hồi học viên về buổi học                      |

### Nhóm: Tài Chính

| Bảng           | Mô tả                        | Công thức                                                |
| -------------- | ---------------------------- | -------------------------------------------------------- |
| `hoadon`       | Hóa đơn học phí              | tongTienSauThue = tongTien - giamGia + thue              |
| `phieuthu`     | Phiếu thu tiền               | Một hóa đơn có nhiều phiếu thu                           |
| `luong`        | Bảng lương giáo viên         | tongTienThucLanh = tongLuongDay + thuong + phuCap - phat |
| `luongchitiet` | Chi tiết lương theo từng lớp | soBuoiDay × donGiaMotBuoi                                |

### Nhóm: Chat

| Bảng                       | Mô tả                                            |
| -------------------------- | ------------------------------------------------ |
| `chat_rooms`               | Phòng chat (class_group hoặc direct)             |
| `chat_messages`            | Tin nhắn (hỗ trợ trả lời, thu hồi)               |
| `chat_room_members`        | Thành viên phòng (vai trò: member/teacher/owner) |
| `chat_message_reactions`   | Cảm xúc emoji trên tin nhắn                      |
| `chat_message_attachments` | Tệp đính kèm                                     |
| `chat_message_deletes`     | Xóa tin nhắn phía cá nhân                        |
| `chat_audit_logs`          | Nhật ký toàn bộ hoạt động chat                   |

### Nhóm: Thông Báo

| Bảng                | Mô tả                                        |
| ------------------- | -------------------------------------------- |
| `thongbao`          | Thông báo hệ thống (lên lịch, ghim, ưu tiên) |
| `thongbaonguoidung` | Mapping thông báo → người nhận (daDoc)       |
| `thongbao_lichsu`   | Nhật ký thao tác (tạo nháp, gửi, xóa...)     |
| `thongbao_tepdinh`  | Tệp đính kèm thông báo                       |

### Nhóm: Blog & Nội Dung

| Bảng              | Mô tả                                      |
| ----------------- | ------------------------------------------ |
| `baiviet`         | Bài viết (slug, soft delete, published_at) |
| `danhmucbaiviet`  | Danh mục bài viết                          |
| `baiviet_danhmuc` | Quan hệ nhiều-nhiều                        |
| `tags`            | Nhãn/tag                                   |
| `baiviet_tag`     | Quan hệ nhiều-nhiều bài viết-tag           |

### Nhóm: CRM Liên Hệ

| Bảng             | Mô tả                                                   |
| ---------------- | ------------------------------------------------------- |
| `lienhe`         | Liên hệ từ khách hàng (loại: tu_van, ho_tro, khieu_nai) |
| `lienhe_lichsu`  | Nhật ký xử lý (ai làm gì, khi nào)                      |
| `lienhe_phanhoi` | Nội dung phản hồi (nội bộ hoặc email)                   |

---

## 3. Danh Sách Migrations

| File                                       | Mô tả                                       |
| ------------------------------------------ | ------------------------------------------- |
| `20260303..._TaoDatabase_LanDau`           | Schema ban đầu (toàn bộ bảng cơ bản)        |
| `20260305..._SyncSchemaFromSql10_20260306` | Đồng bộ từ SQL v10                          |
| `20260310_01_TaoBangChat`                  | **Mới**: 7 bảng chat realtime               |
| `20260310_02_BaoMatDangNhap`               | **Mới**: Nhật ký đăng nhập + phaiDoiMatKhau |
| `20260310_03_PhongHocBaoTriVaSoftDelete`   | **Mới**: Quản lý bảo trì phòng học          |
| `20260310_04_TaiCauTrucDiemDanh`           | **Mới**: Redesign điểm danh đầy đủ          |
| `20260310_05_NangCapHeThongThongBao`       | **Mới**: Lên lịch, ưu tiên, tệp đính kèm    |
| `20260310_06_NangCapCRMLienHe`             | **Mới**: CRM đầy đủ cho liên hệ             |
| `20260310_07_TaoBangHocTapBoSung`          | **Mới**: Bài thi, tài liệu, đánh giá GV     |

---

## 4. Quy Ước Đặt Tên

- **Bảng**: camelCase tiếng Việt không dấu (`taikhoan`, `lophoc`, `buoihoc`)
- **Khóa chính**: `{tenBang}Id` (`taiKhoanId`, `lopHocId`)
- **Khóa ngoại**: giữ nguyên tên cột từ bảng gốc
- **Timestamps**: `created_at`, `updated_at`, `deleted_at` (soft delete)
- **Trạng thái**: `trangThai` (tinyint, ghi chú giá trị trong comment)
