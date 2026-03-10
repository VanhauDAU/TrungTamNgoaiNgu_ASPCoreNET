# Phân Tích Nghiệp Vụ – Hệ Thống Trung Tâm Ngoại Ngữ

## 1. Tổng Quan Bài Toán

Trung tâm ngoại ngữ cần một hệ thống quản lý thống nhất để:

- Quản lý nhiều **cơ sở** (chi nhánh), nhiều **phòng học**, nhiều **ca học**
- Theo dõi toàn bộ vòng đời học viên: tư vấn → đăng ký → học → thi → hoàn thành
- Xử lý **tài chính** (học phí, hóa đơn, phiếu thu, lương giáo viên)
- Giao tiếp nội bộ qua **chat** và **thông báo**
- Quản lý **nội dung** (blog, bài viết, tài liệu học tập)

---

## 2. Đối Tượng Người Dùng

| Vai trò                  | Mô tả                   | Quyền chính     |
| ------------------------ | ----------------------- | --------------- |
| **Admin** (`role=3`)     | Quản trị toàn hệ thống  | Toàn quyền      |
| **Nhân viên** (`role=2`) | Lễ tân, tư vấn, kế toán | Theo nhóm quyền |
| **Giáo viên** (`role=1`) | Giảng dạy, điểm danh    | Theo nhóm quyền |
| **Học viên** (`role=0`)  | Đăng ký học, xem lịch   | Giới hạn        |

---

## 3. Danh Sách Chức Năng (Use Cases)

### 3.1 Quản Lý Cơ Sở & Tổ Chức

- Tạo/sửa/xóa cơ sở đào tạo (chi nhánh)
- Quản lý phòng học: sức chứa, thiết bị, bảo trì
- Quản lý ca học: giờ bắt đầu, kết thúc

### 3.2 Quản Lý Khóa Học & Lớp Học

- CRUD khóa học với danh mục phân cấp (parent/child)
- Tạo lớp học: gán khóa học, giáo viên, phòng, ca, lịch học
- Xem thời khóa biểu các lớp

### 3.3 Quản Lý Học Viên

- Đăng ký học viên mới (tạo tài khoản + hồ sơ)
- Đăng ký lớp học, theo dõi tiến độ
- Điểm danh từng buổi (có mặt, vắng, trễ, có phép, nợ học phí)

### 3.4 Tài Chính

- Lập hóa đơn khi học viên đăng ký lớp
- Ghi nhận phiếu thu thanh toán
- Tính lương giáo viên dựa trên số buổi dạy thực tế
- Báo cáo doanh thu

### 3.5 Chat & Thông Báo

- Chat nhóm lớp học (giáo viên + học viên)
- Chat 1-1 giữa học viên và giáo viên/nhân viên
- Gửi thông báo hàng loạt (lên lịch, ưu tiên, đính kèm file)

### 3.6 CRM Liên Hệ

- Tiếp nhận yêu cầu tư vấn từ website
- Phân loại, giao nhân viên phụ trách
- Theo dõi lịch sử xử lý, phản hồi khách

### 3.7 Nội Dung & Blog

- Viết bài, phân danh mục, gắn tag
- Quản lý trạng thái (nháp/công khai), lên lịch đăng

---

## 4. Quy Tắc Nghiệp Vụ Quan Trọng

### Đăng Ký Lớp Học

```
1. Kiểm tra lớp còn chỗ (soHocVienToiDa)
2. Kiểm tra học viên chưa đăng ký lớp này
3. Tạo dangkylophoc với trangThai=0 (chờ thanh toán)
4. Lập hóa đơn tự động
5. Khi thanh toán đủ → trangThai=2 (xác nhận)
6. Tự động thêm học viên vào chat_room của lớp
```

### Điểm Danh

```
Mỗi buổi học chỉ điểm danh 1 lần/học viên (unique constraint)
Trạng thái:
  0 = Vắng không phép
  1 = Có mặt (mặc định)
  2 = Đi trễ (kèm phutDiTre)
  3 = Nghỉ có phép
  4 = Bị khóa (nợ học phí)
```

### Tính Lương Giáo Viên

```
luong.tongLuongDay = SUM(buoihoc.donGiaDay WHERE giaovien=X AND thang=T)
luong.tongTienThucLanh = tongLuongDay + thuong + phuCap - phat
```
