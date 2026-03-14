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

### 4.1 Admin – `ClassesController` (route thực tế hiện tại)

> Route runtime theo `Program.cs`: `/Admin/{controller}/{action}/{id?}`.

| Method | URL | Mô tả |
| ------ | --- | ----- |
| GET | `/Admin/Classes/Index` | Danh sách lớp + filter + phân trang |
| GET | `/Admin/Classes/Detail/{id}` | Chi tiết lớp |
| GET | `/Admin/Classes/Create` | Form tạo lớp |
| POST | `/Admin/Classes/Create` | Tạo lớp mới |
| GET | `/Admin/Classes/Edit/{id}` | Form chỉnh sửa lớp |
| POST | `/Admin/Classes/Edit` | Lưu chỉnh sửa lớp |
| POST | `/Admin/Classes/changestatus` | Chuyển trạng thái theo state-machine |
| POST | `/Admin/Classes/softdelete` | Xóa mềm lớp |
| GET | `/Admin/Classes/Trash` | Danh sách lớp đã xóa mềm |
| POST | `/Admin/Classes/restore` | Khôi phục lớp từ thùng rác |

### 4.2 AJAX endpoints cho form Lớp học

| Method | URL | Mô tả |
| ------ | --- | ----- |
| GET | `/Admin/Classes/SinhMaLop?khoaHocId={id}` | Sinh mã lớp tự động |
| GET | `/Admin/Classes/HocPhiByKhoaHoc?khoaHocId={id}` | Danh sách gói học phí theo khóa. Chỉ gọi sau khi người dùng đã chọn khóa học |
| GET | `/Admin/Classes/PhongHocByCoso?coSoId={id}` | Danh sách phòng theo cơ sở (kèm `sucChua`) |
| GET | `/Admin/Classes/CoSoByTinh?tinhThanhId={id}&phuongXa={name}` | Danh sách cơ sở theo tỉnh/phường |
| GET | `/Admin/Classes/PhuongXaByTinh?tinhThanhId={id}&baoGomPhuongXa={name}` | Danh sách phường/xã có cơ sở trong hệ thống |

### 4.3 Ghi chú nghiệp vụ form Lớp học

- `Ngày kết thúc` không nhập tay ở Create/Edit lớp học; backend là nguồn chân lý để tự tính từ `Ngày bắt đầu + Lịch học + Số buổi`.
- Nếu lớp có gắn `HocPhiId`, frontend và backend đều yêu cầu đủ `Ngày bắt đầu` và `Lịch học` để tính lịch kết thúc.
- Dropdown địa bàn chỉ hiển thị `Tỉnh/Thành` và `Phường/Xã` có cơ sở đang hoạt động để giảm nhiễu dữ liệu.
- Link quản lý `Cơ sở` và `Phòng học` trong form lớp học đã chuyển sang module admin `Campuses`.

### 4.4 Admin - `CampusesController`

| Method | URL | Mo ta |
| ------ | --- | ----- |
| GET | `/Admin/Campuses/Index` | Danh sach co so + filter theo tu khoa, tinh/thanh, trang thai |
| GET | `/Admin/Campuses/Create` | Form them co so |
| POST | `/Admin/Campuses/Create` | Luu co so moi |
| GET | `/Admin/Campuses/Edit/{id}` | Form sua co so |
| POST | `/Admin/Campuses/Edit` | Luu thay doi co so |
| GET | `/Admin/Campuses/Detail/{id}?tab=overview|rooms|staff|classes` | Trang chi tiet co so theo tab |
| POST | `/Admin/Campuses/SaveRoom` | Them/sua phong hoc trong ngu canh co so |
| POST | `/Admin/Campuses/DeleteRoom` | Xoa phong hoc cua co so |
| POST | `/Admin/Campuses/Delete/{id}` | Xoa co so neu khong con rang buoc |

### 4.5 Admin - `ClassSetupController` sau refactor

| Method | URL | Mo ta |
| ------ | --- | ----- |
| GET | `/Admin/ClassSetup/Index` | Dashboard du lieu nen mo lop |
| GET/POST | `/Admin/ClassSetup/CaHoc` | Quan ly ca hoc |
| GET/POST | `/Admin/ClassSetup/HocPhi` | Quan ly goi hoc phi |
| GET | `/Admin/ClassSetup/CoSo` | Route cu, redirect sang `Campuses` |
| GET | `/Admin/ClassSetup/PhongHoc` | Route cu, redirect sang tab `rooms` cua `Campuses` |

### 4.6 Ghi chu dieu huong sau khi tach module

- `Co so dao tao` khong con la mot muc con trong `Thiết lập lớp học`.
- `Phong hoc` duoc quan ly ben trong detail cua tung co so, khong con la man CRUD doc lap.
- Giữ route cu o `ClassSetup` theo che do redirect de tranh vo link noi bo trong admin.

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
