# Naming Convention (TrungTamNgoaiNgu)

## 1) Mục tiêu
- Tên phải phản ánh đúng domain nghiệp vụ.
- Tránh tên chung chung/mơ hồ.
- Dễ mở rộng khi hệ thống có nhiều module tương tự.

## 2) Nguyên tắc chung
- Dùng tiếng Anh nhất quán cho code: class, file, namespace, route.
- Dùng `PascalCase` cho class/file; `camelCase` cho biến local/param.
- Tên controller là tài nguyên rõ nghĩa, ưu tiên số nhiều.
- Tránh tên kỹ thuật/bao quát như `Common`, `Manage`, `Handler`, `DanhMuc` (không rõ loại danh mục).

## 3) Quy tắc đặt tên theo tầng

### 3.1 Controllers
- Mẫu: `<ResourcePlural>Controller`
- Ví dụ:
  - `CoursesController`
  - `CourseCategoriesController`
  - `AuditLogsController`

### 3.2 Services
- Mẫu interface: `I<ResourcePlural>Service`
- Mẫu implementation: `<ResourcePlural>Service`
- Ví dụ:
  - `ICoursesService` / `CoursesService`
  - `ICourseCategoriesService` / `CourseCategoriesService`
  - `IAuditLogsService` / `AuditLogsService`

### 3.3 View folders
- Mẫu: `Views/Admin/<ResourcePlural>/...`
- Ví dụ:
  - `Views/Admin/Courses/Index.cshtml`
  - `Views/Admin/CourseCategories/Edit.cshtml`

### 3.4 Routes
- Mẫu: `/Admin/<ResourcePlural>`
- Ví dụ:
  - `/Admin/Courses`
  - `/Admin/CourseCategories`
  - `/Admin/AuditLogs`

### 3.5 Models / Entities
- Dùng danh từ số ít.
- Ví dụ:
  - `Course`, `CourseCategory`, `AuditLog`

## 4) Bảng đề xuất đổi tên trong code hiện tại
| Tên hiện tại | Tên đề xuất | Lý do |
| --- | --- | --- |
| `KhoaHocController` | `CoursesController` | Rõ domain, dễ mở rộng |
| `DanhMucController` | `CourseCategoriesController` | Tránh mơ hồ “DanhMuc” |
| `NhatKyHeThongController` | `AuditLogsController` | Tên chuẩn nghiệp vụ audit |
| `IKhoaHocService` | `ICoursesService` (hoặc tách `ICoursesService` + `ICourseCategoriesService`) | Tránh service ôm nhiều domain |
| `KhoaHocService` | `CoursesService` | Đồng nhất với controller |
| `INhatKyHeThongService` | `IAuditLogsService` | Đồng nhất naming tiếng Anh |

## 5) Quy định tránh mơ hồ
- Không dùng `DanhMuc*` nếu chưa chỉ rõ là danh mục gì.
- Không dùng `Index2`, `NewController`, `TempService`.
- Không viết tắt khó hiểu: `QLKH`, `DM`, `HT`.

## 6) Mẫu checklist trước khi tạo file mới
- Tên đã thể hiện đúng domain chưa?
- Nếu hệ thống có thêm 2-3 loại tương tự, tên này còn rõ không?
- Tên controller/service/view folder/route đã đồng nhất chưa?
- Người mới vào project có đoán được chức năng từ tên không?

## 7) Kế hoạch áp dụng
1. Áp dụng cho module mới ngay lập tức.
2. Refactor dần module cũ theo từng vertical slice.
3. Mỗi PR phải tuân thủ checklist mục 6.
