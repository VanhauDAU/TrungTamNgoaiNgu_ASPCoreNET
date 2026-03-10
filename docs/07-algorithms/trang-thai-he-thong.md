# Tài Liệu Trạng Thái Hệ Thống (Chuẩn)

> **Nguồn chân lý**: Xem file `Models/Shared/TrangThaiEnums.cs`  
> **Dùng sai = bug** — Luôn dùng enum, KHÔNG dùng số trực tiếp trong code.

---

## Quy Ước Bắt Buộc

```csharp
// ✅ ĐÚNG — dùng enum
lopHoc.TrangThai == LopHocTrangThai.DangHoc
dangKy.TrangThai = DangKyTrangThai.TamDungNoHocPhi

// ❌ SAI — dùng magic number
lopHoc.TrangThai == 4
dangKy.TrangThai = 3
```

---

## 1. Lớp Học — `LopHocTrangThai`

| Giá trị | Enum            | Nhãn hiển thị   | Badge        | Ý nghĩa                        |
| ------- | --------------- | --------------- | ------------ | ------------------------------ |
| `0`     | `SapMo`         | Sắp mở          | ⚫ secondary | Lớp đã tạo, chưa mở đăng ký    |
| `1`     | `DangTuyenSinh` | Đang tuyển sinh | 🔵 info      | Đang nhận học viên đăng ký     |
| `2`     | `ChotDanhSach`  | Chốt danh sách  | 🟡 warning   | Đã chốt, không nhận thêm HV    |
| `3`     | `DaHuy`         | Đã hủy          | 🔴 danger    | Lớp bị hủy                     |
| `4`     | `DangHoc`       | Đang học        | 🟢 success   | Đã khai giảng, đang diễn ra    |
| `5`     | `DaKetThuc`     | Đã kết thúc     | ⚫ dark      | Toàn bộ buổi học đã hoàn thành |

```
SapMo → DangTuyenSinh → ChotDanhSach → DangHoc → DaKetThuc
   └──────────────────────────────────────────────→ DaHuy (bất kỳ lúc nào)
```

---

## 2. Đăng Ký Lớp Học — `DangKyTrangThai`

| Giá trị | Enum              | Nhãn hiển thị    | Badge        | Ý nghĩa                           |
| ------- | ----------------- | ---------------- | ------------ | --------------------------------- |
| `0`     | `ChoThanhToan`    | Chờ thanh toán   | 🟡 warning   | Mới đăng ký, chưa đóng tiền       |
| `1`     | `DaXacNhan`       | Đã xác nhận      | 🔵 info      | Đã thanh toán, chờ lớp khai giảng |
| `2`     | `DangHoc`         | Đang học         | 🟢 success   | Đang tham gia buổi học            |
| `3`     | `TamDungNoHocPhi` | Tạm dừng – nợ HP | 🔴 danger    | Bị khóa điểm danh do nợ học phí   |
| `4`     | `BaoLuu`          | Bảo lưu          | ⚫ secondary | HV xin giữ quyền học sau          |
| `5`     | `HoanThanh`       | Hoàn thành       | 🔷 primary   | Đã học xong khóa                  |
| `6`     | `Huy`             | Đã hủy           | ⚫ dark      | Hủy đăng ký                       |

```
ChoThanhToan → DaXacNhan → DangHoc → HoanThanh
                               ↓ ↑
                          TamDungNoHocPhi (nợ HP)
                               ↓
                          BaoLuu | Huy
```

> 🔒 **Khi điểm danh**: chỉ học viên có `TrangThai = DangHoc` mới được điểm danh.  
> Học viên `TamDungNoHocPhi` → tự động set `DiemDanhTrangThai.KhoaNoHocPhi`.

---

## 3. Buổi Học — `BuoiHocTrangThai`

| Giá trị | Enum          | Nhãn hiển thị | Badge        | Ý nghĩa                |
| ------- | ------------- | ------------- | ------------ | ---------------------- |
| `0`     | `SapDienRa`   | Sắp diễn ra   | ⚫ secondary | Chưa đến giờ học       |
| `1`     | `DangDienRa`  | Đang diễn ra  | 🟢 success   | Đang trong giờ học     |
| `2`     | `DaHoanThanh` | Đã hoàn thành | 🔷 primary   | Xong, điểm danh đã lưu |
| `3`     | `DaHuy`       | Đã hủy        | 🔴 danger    | Buổi bị hủy            |
| `4`     | `DoiLich`     | Đổi lịch      | 🟡 warning   | Dời sang ngày khác     |

```
SapDienRa → DangDienRa → DaHoanThanh
   ↓              ↓
 DoiLich        DaHuy
```

> ⚠️ **Lưu ý phân biệt**:
>
> - `TrangThai = DaHoanThanh` ≠ `daDiemDanh = true`
> - `daDiemDanh`: GV đã bấm hoàn tất điểm danh (cờ riêng, không liên quan TrangThai)
> - `daHoanThanh`: Đồng bộ với `TrangThai = DaHoanThanh`

---

## 4. Điểm Danh — `DiemDanhTrangThai`

| Giá trị | Enum           | Nhãn hiển thị | Badge      | Ý nghĩa                        |
| ------- | -------------- | ------------- | ---------- | ------------------------------ |
| `0`     | `Vang`         | Vắng          | 🔴 danger  | Vắng không lý do               |
| `1`     | `CoMat`        | Có mặt        | 🟢 success | Có mặt đúng giờ **(mặc định)** |
| `2`     | `DiTre`        | Đi trễ        | 🟡 warning | Đến trễ — cần điền `phutDiTre` |
| `3`     | `CoPhep`       | Có phép       | 🔵 info    | Xin phép trước                 |
| `4`     | `KhoaNoHocPhi` | Khóa – nợ HP  | ⚫ dark    | **Hệ thống tự set** khi nợ HP  |

> Unique constraint: mỗi học viên chỉ có **1 bản ghi điểm danh/buổi học**.

---

## 5. Phòng Học — `PhongHocTrangThai`

| Giá trị | Enum         | Nhãn         | Ý nghĩa             |
| ------- | ------------ | ------------ | ------------------- |
| `0`     | `DangBaoTri` | Đang bảo trì | Không xếp lịch      |
| `1`     | `HoatDong`   | Hoạt động    | Sử dụng bình thường |
| `2`     | `TamNgung`   | Tạm ngưng    | Tạm thời không dùng |
| `3`     | `NgungHan`   | Ngưng hẳn    | Không còn sử dụng   |

---

## Dùng Trong Code

### Razor View

```razor
<span class="@Model.TrangThai.GetBadgeClass()">
    <i class="bi @Model.TrangThai.GetIcon()"></i>
    @Model.TrangThaiText
</span>
```

### SelectList trong Controller

```csharp
// Tạo dropdown trạng thái lớp học
var options = Enum.GetValues<LopHocTrangThai>()
    .Select(e => new SelectListItem(e.GetLabel(), ((int)e).ToString()))
    .ToList();
ViewBag.TrangThaiOptions = options;
```

### Query theo trạng thái

```csharp
// Tất cả lớp đang học
var lopDangHoc = await _db.LopHocs
    .Where(l => l.TrangThai == LopHocTrangThai.DangHoc)
    .ToListAsync();

// Học viên bị khóa điểm danh
var biKhoa = await _db.DangKyLopHocs
    .Where(dk => dk.TrangThai == DangKyTrangThai.TamDungNoHocPhi)
    .ToListAsync();
```
