# Changelog

Tất cả các thay đổi nổi bật của dự án sẽ được ghi nhận trong file này.

## [Unreleased] - 2026-03-11

### 🏫 Tính năng mới — Quản lý Lớp Học (`feature/admin-classes-management`)

**Branch**: `feature/admin-classes-management`

- **`Services/Interfaces/IServices.cs`** — Mở rộng `IClassesService` với đầy đủ methods:
  phân trang, thống kê (stats cards), CRUD, state-machine `ChuyenTrangThaiAsync`, soft delete, dropdowns động.
  Thêm DTO `LopHocQuanLyThongKe`.
- **`Services/ClassesService.cs`** _(mới)_ — Implementation đầy đủ: EF Core `Include()`, slug chống trùng,
  state-machine validation (`SapMo → DangTuyenSinh → ChotDanhSach → DangHoc → DaKetThuc`), ghi `NhatKyHeThong`.
- **`Controllers/Admin/ClassesController.cs`** _(mới)_ — CRUD + changestatus + softdelete + Trash + restore
  - 2 AJAX endpoints (`/PhongHocByCoso`, `/HocPhiByKhoaHoc`) cho dropdowns động.
- **`Views/Admin/Classes/`** _(5 views mới)_:
  - `Index.cshtml` — Danh sách + 5 stats cards + bộ lọc đa chiều + phân trang
  - `Create.cshtml` — Form tạo lớp; chọn Cơ sở → Phòng học tự load AJAX; chọn Khóa học → Học phí tự load
  - `Edit.cshtml` — Form sửa + panel chuyển trạng thái state-machine tích hợp
  - `Detail.cshtml` — Chi tiết lớp + 2 tabs (Học viên / Buổi học) + nút chuyển trạng thái nhanh
  - `Trash.cshtml` — Thùng rác + khôi phục
- **`Program.cs`** — Đăng ký `IClassesService → ClassesService` vào DI container.
- **`Views/Shared/_AdminLayout.cshtml`** — Cập nhật sidebar: controller `Classes` (thay `LopHoc` cũ).

---

## [Unreleased] - 2026-03-11 (trước đó)

### ⚙️ Database & Migrations (Rebuild từ đầu)

- **Xóa toàn bộ migrations cũ** và tạo lại từ đầu theo chuẩn EF Core để đảm bảo migration history sạch, nhất quán.
- **Migration `InitialCreate`**: Tạo toàn bộ schema (~30 bảng) từ `AppDbContext` trong 1 migration duy nhất.
- **Migration `AddChatTables`**: Bổ sung 7 bảng hệ thống Chat còn thiếu.

### 🚀 Tính năng mới — Hệ thống Chat (Chat Models)

- Tạo **7 C# Model classes** tại `Models/Chat/` mapping với các bảng chat trong SQL Schema:
  - `ChatRoom` — Phòng chat (class_group hoặc direct 1-1)
  - `ChatMessage` — Tin nhắn, hỗ trợ reply, thu hồi, xóa mềm
  - `ChatRoomMember` — Thành viên phòng chat, unique constraint (room, user)
  - `ChatMessageReaction` — Emoji reactions, unique (message, user, emoji)
  - `ChatMessageAttachment` — Tệp đính kèm (ảnh, video, file)
  - `ChatMessageDelete` — Xóa tin nhắn phía cá nhân
  - `ChatAuditLog` — Nhật ký thao tác chat
- Thêm **7 `DbSet<>`** vào `AppDbContext` (Nhóm 7: Chat).
- Cấu hình đầy đủ **Fluent API**: FK cascade, self-referencing (ReplyToMessage → NoAction để tránh multiple cascade paths trên SQL Server), unique indexes.

---

## [Unreleased] - 2026-03-10

### 🚀 Tính năng mới & Nâng cấp (Features & Enhancements)

- **Tạo hệ thống Enums Trạng Thái chuẩn hóa**: Định nghĩa tập trung tại `Models/Shared/TrangThaiEnums.cs` để đồng bộ toàn bộ trạng thái trong hệ thống:
  - `LopHocTrangThai`
  - `DangKyTrangThai`
  - `BuoiHocTrangThai`
  - `DiemDanhTrangThai`
  - `HinhThucHoc`
  - `PhongHocTrangThai`
- **Tiện ích mở rộng (Extension Methods) cho Enums**: Thêm `TrangThaiExtensions.cs` cho phép lấy Text, Badge CSS Class, và Icon ngay trên Views/Controllers dễ dàng.
- **Tài liệu toàn diện (Comprehensive Documentation)**: Bổ sung loạt tài liệu tiếng Việt chi tiết trong thư mục `docs/`:
  - Phân tích nghiệp vụ (`phan-tich-nghiep-vu.md`)
  - Kiến trúc hệ thống (`kien-truc-he-thong.md`)
  - Tài liệu cơ sở dữ liệu (`tai-lieu-co-so-du-lieu.md`)
  - API Endpoints & Routes MVC (`tai-lieu-endpoints-mvc.md`)
  - Hướng dẫn phát triển và Git workflow (`huong-dan-phat-trien.md`)
  - Quy trình nghiệp vụ và thuật toán (`quy-trinh-nghiep-vu.md`)
  - Tổng hợp trạng thái hệ thống (`trang-thai-he-thong.md`)
- **Tạo README.md gốc**: Bổ sung `README.md` với huy hiệu (badges), giới thiệu, tính năng và link liên kết tới tài liệu.

### 🛠 Cập nhật & Sửa lỗi (Refactoring & Bug Fixes)

- **Đồng bộ hóa Models với SQL Schema**:
  - `KhoaHoc.cs`: Thêm cột `maKhoaHoc`.
  - `CaHoc.cs`: Thêm cột `moTa`.
  - `DanhMucKhoaHoc.cs`: Thêm `maDanhMuc` và `sort_order`.
  - `LopHoc.cs`: Áp dụng `LopHocTrangThai`, thêm property phục vụ soft delete (`deleted_at`) và `maLopHoc`.
  - `DangKyLopHoc.cs`: Áp dụng `DangKyTrangThai`.
  - `BuoiHoc.cs`: Áp dụng `BuoiHocTrangThai`.
- **Cập nhật Services và Views**: Sửa các file `DashboardService.cs`, `CoursesService.cs`, và `Detail.cshtml` (Admin/Courses) để sử dụng Type-safe Enums thay cho hardcoded integer (ví dụ đổi `TrangThai == 1` thành `LopHocTrangThai.DangTuyenSinh`).
- **Migrations Database**:
  - Tạo các migrations ban đầu theo chuẩn Entity Framework để mapping với SQL structure (bảng Chat, Audit, DiemDanh, ThongBao, CRM Liên Hệ...).
  - Xử lý xung đột cơ sở dữ liệu và bảo đảm các bảng đều được track bởi EF Core qua lệnh `AddColumn` (Migration `DongBoColumnConThieu_Va_TrangThaiEnums`).
- Cập nhật phiên bản **EF Core Tools** lên `10.0.3` để khắc phục lỗi runtime mismatch.

### ⚙️ Database & Migrations

- Đã chạy thành công `dotnet ef database update` để đồng bộ CSDL MariaDB với Models mới nhất. Khắc phục được lỗi đụng độ bảng đã tồn tại bằng cách custom luồng `Up()` trong mã Migration.
- Tạo sẵn các cấu trúc bảng phức tạp hỗ trợ tương lai như: Chat Realtime, Audit Logs, Thông báo cá nhân, Lịch sử liên hệ CRM.
