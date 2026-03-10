# Quy Trình Nghiệp Vụ & Thuật Toán

## 1. Quy Trình Đăng Ký Học Viên – Lớp Học

```
[Học viên] Chọn lớp học
    ↓
Kiểm tra lớp còn chỗ? (dangkylophoc COUNT < lophoc.soHocVienToiDa)
    └── Không → Thông báo "Lớp đã đầy"
    └── Có →
        Kiểm tra học viên đã đăng ký chưa? (dangkylophoc WHERE taiKhoanId + lopHocId)
            └── Rồi → Thông báo "Đã đăng ký"
            └── Chưa →
                Tạo dangkylophoc (trangThai = 0: Chờ thanh toán)
                Lập hóa đơn tự động (hoadon)
                [Nhân viên] Ghi nhận thanh toán (phieuthu)
                Khi daTra >= tongTienSauThue:
                    → hoadon.trangThai = 2 (Đã thanh toán đủ)
                    → dangkylophoc.trangThai = 2 (Xác nhận)
                    → Tự động thêm vào chat_rooms của lớp
```

---

## 2. Quy Trình Điểm Danh

```
[GV mở buổi học]
    ↓
Lấy danh sách học viên của lớp (dangkylophoc WHERE trangThai=2)
    ↓
Với mỗi học viên, tạo bản ghi diemDanh:
    trangThai:
        1 = Có mặt (mặc định)
        0 = Vắng không phép
        2 = Đi trễ → nhập phutDiTre
        3 = Có phép → nhập lyDo
        4 = Bị khóa (nợ HP) → hệ thống tự set khi hoadon quá hạn

    coMat = (trangThai == 1 || trangThai == 2) ? 1 : 0

    hinhThuc: 0=Trực tiếp, 1=Online

Khi điểm danh xong:
    buoihoc.daDiemDanh = 1
    Ghi nguoiDiemDanhId + thoiGianDiemDanh

UNIQUE CONSTRAINT: (buoiHocId, taiKhoanId) → không thể điểm danh trùng
```

---

## 3. Máy Trạng Thái Phòng Học

```
       [admin mở bảo trì]            [bảo trì xong]
           ┌──────────┐               ┌──────────┐
           ↓          │               ↓          │
   ┌─────────────┐    │       ┌─────────────────┐│
   │ 0: Đang     │────┘       │ 1: Hoạt động    ││
   │   bảo trì   │←───────────│   bình thường   ││
   └─────────────┘            └─────────────────┘│
                                      │          │
                              [tạm đóng]  [mở lại]
                                      ↓          │
                              ┌─────────────┐    │
                              │ 2: Tạm ngưng│────┘
                              └─────────────┘
                                      │
                              [đóng hẳn]
                                      ↓
                              ┌─────────────┐
                              │ 3: Ngưng    │
                              │   hẳn       │
                              └─────────────┘
```

---

## 4. Quy Trình Chat – Thu Hồi Tin Nhắn

```
Gửi tin → guiLuc = NOW()
          deadlineThuHoi = guiLuc + 24 giờ

Thu hồi (chỉ trong deadline):
    IF NOW() <= deadlineThuHoi:
        chat_messages.thuHoiLuc = NOW()
        chat_messages.noiDung = "Tin nhắn đã được thu hồi"
        Ghi audit log: hanhDong = "message.recalled"

Xóa phía cá nhân:
    Tạo bản ghi chat_message_deletes (chatMessageId, taiKhoanId)
    → Khi query, lọc bỏ tin nhắn có trong bảng này với user hiện tại

Số tin chưa đọc:
    SELECT COUNT(*) FROM chat_messages
    WHERE chatRoomId=X AND chatMessageId > lastReadMessageId
    AND taiKhoanId NOT IN (SELECT taiKhoanId FROM chat_message_deletes WHERE...)
```

---

## 5. Tính Lương Giáo Viên

```
Đầu vào: taiKhoanId (giáo viên), thangLuong (YYYY-MM)

Bước 1: Đếm số buổi dạy thực tế trong tháng
    SELECT COUNT(*), lopHocId
    FROM buoihoc
    WHERE taiKhoanId = X AND daHoanThanh = 1
    AND MONTH(ngayHoc) = M AND YEAR(ngayHoc) = Y
    GROUP BY lopHocId

Bước 2: Tính tiền từng lớp
    tienTungLop = soBuoiDay × lophoc.donGiaDay

Bước 3: Tổng hợp bảng lương
    luong.tongLuongDay = SUM(tienTungLop)
    luong.tongTienThucLanh = tongLuongDay + thuong + phuCap - phat

Ghi luongchitiet: 1 bản ghi / lớp (để truy vết)
```

---

## 6. Hệ Thống Thông Báo – Lên Lịch Gửi

```
sendTrangThai:
    0 = Nháp     → Chưa gửi, chưa lên lịch
    1 = Đã lên lịch → scheduled_at != NULL, chưa đến giờ
    2 = Đã gửi   → sent_at != NULL
    3 = Gửi lỗi  → failed_at != NULL, failure_reason có nội dung

doiTuongGui:
    0 = Tất cả học viên
    1 = Theo lớp (doiTuongId = lopHocId)
    2 = Theo khóa học (doiTuongId = khoaHocId)
    3 = Cá nhân (doiTuongId = taiKhoanId)
    4 = Theo vai trò (doiTuongId = role value)
    5 = Theo cơ sở (doiTuongId = coSoId)
```

---

## 7. Máy Trạng Thái Liên Hệ CRM

```
0 = Chưa xử lý  ──[gán phụ trách]──→  1 = Đang xử lý
                                              │
                                    [xử lý xong/từ chối]
                                              │
                                    2 = Hoàn thành / 3 = Từ chối

Mỗi thay đổi trạng thái ghi 1 bản ghi lienhe_lichsu
```
