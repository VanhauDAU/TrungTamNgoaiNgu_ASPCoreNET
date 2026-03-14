# Changelog

Tất cả các thay đổi nổi bật của dự án sẽ được ghi nhận trong file này.

## [Unreleased] - 2026-03-14

### ✨ Nâng cấp — Admin quản lý lớp học, khóa học và danh mục

- **Branch**: `codex/admin-core-polish`
- Nâng cấp mạnh luồng quản trị `Lớp học`, `Danh mục khóa học`, `Khóa học` theo hướng chuẩn nghiệp vụ hơn và đồng bộ UI hơn.

### 🏫 Lớp học — Create/Edit/Index

- **`Services/ClassesService.cs`**
  - Không load gói học phí khi chưa chọn khóa học.
  - Chỉ load `Tỉnh/Thành` và `Phường/Xã` đang thực sự có cơ sở hoạt động.
  - Bổ sung `LayPhuongXaByTinhAsync(...)` lấy địa bàn từ dữ liệu nội bộ thay vì phụ thuộc Open API.
  - Tự tính `NgayKetThuc` từ `NgayBatDau + LichHoc + SoBuoiDuKien`.
  - Nếu đã chọn gói học phí thì bắt buộc phải có `Ngày bắt đầu` và `Lịch học` để hệ thống tính lịch kết thúc.
  - Chặn lệch `SoBuoiDuKien` so với lịch học thực tế sau khi hệ thống tự tính ngày kết thúc.
- **`Controllers/Admin/ClassesController.cs`**
  - Bỏ phụ thuộc `IHttpClientFactory` trong `ClassesController`.
  - Đổi endpoint `GET /Admin/Classes/PhuongXaByTinh` sang dùng `tinhThanhId` và dữ liệu nội bộ.
  - Dropdown tỉnh/thành khi Create/Edit chỉ lấy những nơi đang có cơ sở, nhưng vẫn giữ được tỉnh hiện tại khi sửa dữ liệu cũ.
- **`Views/Admin/Classes/Create.cshtml`**
  - Ô `Ngày kết thúc` chuyển sang readonly và tự cập nhật theo thao tác người dùng.
  - Chọn gói học phí sau khi chọn ngày bắt đầu vẫn tự tính lại `Ngày kết thúc`.
  - Không preload gói học phí trước khi chọn khóa học.
  - UI địa bàn chỉ hiển thị tỉnh/phường/xã đang có cơ sở thật.
- **`Views/Admin/Classes/Edit.cshtml`**
  - Đồng bộ trải nghiệm với trang Create: `Ngày kết thúc` tự tính, `Số buổi` đồng bộ theo gói học phí, địa bàn lấy từ dữ liệu nội bộ.
- **`Views/Admin/Classes/Index.cshtml`**
  - Làm lại hàng hiển thị danh sách lớp: rõ mã lớp, khóa học, cơ sở vận hành, mức lấp đầy, lịch học và trạng thái.
  - Gỡ bỏ block `lh-command` sau khi thử nghiệm UI để giữ giao diện gọn hơn.
  - Làm lại khu vực lọc theo dạng card gọn, có label rõ ràng, không còn kéo dọc thiếu cân đối.

### 🏷 Khóa học & Danh mục — Mã tự sinh

- **`Services/CoursesService.cs`**
  - Thêm cơ chế tự sinh `MaDanhMuc` và `MaKhoaHoc` ở backend, chống trùng và chuẩn hóa format.
  - Khi sửa dữ liệu cũ mà chưa có mã, hệ thống tự bổ sung mã khi lưu.
  - Giữ mã ổn định trên dữ liệu đã có để tránh ảnh hưởng các luồng tra cứu và sinh mã lớp.
- **`Views/Admin/CourseCategories/Create.cshtml`** / **`Edit.cshtml`**
  - Bỏ nhập tay `Mã danh mục`, chuyển sang preview mã dự kiến/mã hiện tại.
- **`Views/Admin/Courses/Create.cshtml`** / **`Edit.cshtml`**
  - Thêm preview `Mã khóa học` trong form thay vì để người dùng tự suy format.

### 🧭 Thiết lập cơ sở

- **`Views/Admin/ClassSetup/CoSo.cshtml`**
  - Đồng bộ màn `Thiết lập cơ sở` sang endpoint địa bàn mới.
  - Datalist phường/xã giờ chỉ gợi ý các địa bàn đang có cơ sở trong hệ thống.

## [Unreleased] - 2026-03-11

### 🧩 Feature — Thiết lập dữ liệu nền để tạo lớp học

- **Branch**: `codex/class-prerequisites-crud`
- Thêm module admin `ClassSetup` để quản lý nhanh `Ca học`, `Gói học phí`, `Cơ sở đào tạo`, `Phòng học` trên web.
- Thêm service `IClassSetupService` / `ClassSetupService` để gom toàn bộ nghiệp vụ CRUD, kiểm tra ràng buộc xóa và ghi nhật ký hệ thống.
- Thêm hub điều hướng “Thiết lập lớp học” trong sidebar admin và nút quay lại `Tạo lớp` từ mọi màn hình thiết lập.
- Bổ sung link/empty-state trong form `Thêm lớp` và `Sửa lớp` để đi thẳng tới dữ liệu nền còn thiếu.

### 🩹 Hotfix — Quản lý lớp học (Create/Edit + địa chỉ + ràng buộc nghiệp vụ)

- **`Controllers/Admin/ClassesController.cs`**
  - Thêm endpoint `GET /Admin/Classes/PhuongXaByTinh?maApi=...` làm proxy server-side tới Open API.
  - Sửa `Create` để hiển thị lỗi nghiệp vụ ngay trên form (không redirect mất context).
  - Mở rộng JSON của `PhongHocByCoso` trả thêm `sucChua` để validate ở UI.
  - Mở rộng `CoSoByTinh` nhận thêm `phuongXa` để lọc cơ sở theo địa bàn.
- **`Services/ClassesService.cs`**
  - Bắt buộc **mã lớp tự sinh ở backend** khi tạo mới (`SinhMaLopHocAsync`), không tin dữ liệu nhập tay.
  - Thêm rule nghiệp vụ: `SoHocVienToiDa` phải **nhỏ hơn** `PhongHoc.SucChua` khi tạo/sửa.
  - Nạp dữ liệu giáo viên kèm `NhanSu.CoSo` để UI hiển thị cùng/khác cơ sở.
  - Hỗ trợ lọc `LayCoSoByTinhAsync(..., phuongXa)` theo tên phường/xã.
- **`Views/Admin/Classes/Create.cshtml`**
  - Khóa ô `MaLopHoc` (readonly), nâng cấp giao diện form.
  - Sửa luồng load phường/xã qua endpoint nội bộ để tránh lệch schema API v2.
  - Hiển thị rõ giáo viên cùng cơ sở/khác cơ sở trong dropdown.
  - Validate client-side sĩ số < sức chứa phòng trước khi submit.
- **`Views/Admin/Classes/Edit.cshtml`**
  - Khóa chỉnh sửa mã lớp.
  - Hiển thị sức chứa phòng học và chặn submit nếu sĩ số không hợp lệ.
  - Hiển thị scope cơ sở của giáo viên tương tự trang Create.
- **`Views/Admin/Classes/Index.cshtml`**
  - Nâng cấp style danh sách/stats cards theo hướng hiện đại, rõ hierarchy thông tin.
- **`Program.cs`**
  - Đăng ký `AddHttpClient()` phục vụ gọi Open API từ server.

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
