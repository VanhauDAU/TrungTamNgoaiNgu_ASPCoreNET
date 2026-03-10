# Danh Sách API & Endpoint MVC

## 1. Quy Ước Routing

```
Admin:  /admin/{module}/{action}/{id?}
Client: /{route}
API:    /api/{resource}/{action}
```

---

## 2. Tài Khoản & Xác Thực

| Method | URL             | Mô tả                 |
| ------ | --------------- | --------------------- |
| GET    | `/login`        | Trang đăng nhập       |
| POST   | `/login`        | Xác thực, tạo session |
| POST   | `/logout`       | Đăng xuất             |
| GET    | `/doi-mat-khau` | Đổi mật khẩu          |
| POST   | `/doi-mat-khau` | Lưu mật khẩu mới      |

---

## 3. Admin – Quản Lý Học Viên

| Method | URL                        | Mô tả              |
| ------ | -------------------------- | ------------------ |
| GET    | `/admin/hoc-vien`          | Danh sách học viên |
| GET    | `/admin/hoc-vien/them`     | Form thêm mới      |
| POST   | `/admin/hoc-vien/them`     | Lưu học viên mới   |
| GET    | `/admin/hoc-vien/{id}`     | Chi tiết học viên  |
| POST   | `/admin/hoc-vien/{id}/xoa` | Xóa mềm            |

---

## 4. Admin – Khóa Học & Lớp Học

| Method   | URL                             | Mô tả              |
| -------- | ------------------------------- | ------------------ |
| GET      | `/admin/khoa-hoc`               | Danh sách khóa học |
| GET/POST | `/admin/khoa-hoc/them`          | Thêm khóa học      |
| GET/POST | `/admin/khoa-hoc/{id}/sua`      | Sửa khóa học       |
| POST     | `/admin/khoa-hoc/{id}/xoa`      | Soft delete        |
| GET      | `/admin/lop-hoc`                | Danh sách lớp      |
| GET/POST | `/admin/lop-hoc/them`           | Tạo lớp mới        |
| GET      | `/admin/lop-hoc/{id}/diem-danh` | Trang điểm danh    |
| POST     | `/admin/lop-hoc/{id}/diem-danh` | Lưu điểm danh      |

---

## 5. Admin – Tài Chính

| Method | URL                              | Mô tả                |
| ------ | -------------------------------- | -------------------- |
| GET    | `/admin/hoa-don`                 | Danh sách hóa đơn    |
| GET    | `/admin/hoa-don/{id}`            | Chi tiết hóa đơn     |
| POST   | `/admin/hoa-don/{id}/thanh-toan` | Ghi phiếu thu        |
| GET    | `/admin/luong`                   | Bảng lương giáo viên |
| POST   | `/admin/luong/tinh`              | Tính lương tháng     |

---

## 6. API Realtime (JSON)

| Method | URL                               | Mô tả                 |
| ------ | --------------------------------- | --------------------- |
| GET    | `/api/thong-bao/chua-doc`         | Số thông báo chưa đọc |
| POST   | `/api/thong-bao/{id}/da-doc`      | Đánh dấu đã đọc       |
| GET    | `/api/chat/phong`                 | Danh sách phòng chat  |
| GET    | `/api/chat/phong/{id}/tin-nhan`   | Lịch sử tin nhắn      |
| POST   | `/api/chat/tin-nhan/{id}/thu-hoi` | Thu hồi tin nhắn      |
| POST   | `/api/chat/tin-nhan/{id}/cam-xuc` | Toggle emoji reaction |

---

## 7. Client – Trang Học Viên

| Method | URL                   | Mô tả                   |
| ------ | --------------------- | ----------------------- |
| GET    | `/`                   | Trang chủ               |
| GET    | `/khoa-hoc`           | Danh sách khóa học      |
| GET    | `/khoa-hoc/{slug}`    | Chi tiết khóa học       |
| GET    | `/dang-ky/{lopHocId}` | Đăng ký lớp             |
| GET    | `/lich-hoc`           | Thời khóa biểu học viên |
| GET    | `/blog`               | Danh sách bài viết      |
| GET    | `/blog/{slug}`        | Chi tiết bài viết       |
| GET    | `/lien-he`            | Form liên hệ/tư vấn     |
| POST   | `/lien-he`            | Gửi yêu cầu tư vấn      |
