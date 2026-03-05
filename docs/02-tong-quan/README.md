# 02 - Tong quan

## 1. Kien truc tong the

Ung dung theo mo hinh ASP.NET Core MVC:

- `Controllers`: nhan request, validate co ban, dieu huong view.
- `Services`: chua logic nghiep vu va thao tac du lieu.
- `Data/AppDbContext`: anh xa model <-> bang SQL Server.
- `Models`: entity theo nhom domain.
- `Views`: Razor views cho Admin va Public.
- `wwwroot/css`: style cho giao dien (`admin.css`, `admin-courses.css`, `admin-auditlogs.css`, `site.css`).

## 2. Startup va DI

Trong `Program.cs`:

- Dang ky MVC va EF Core SQL Server.
- Dang ky DI:
  - `IDashboardService -> DashboardService`
  - `ICoursesService -> CoursesService`
  - `IFinanceService -> FinanceService`
  - `IAuditLogsService -> AuditLogsService`
- Tu dong `Database.Migrate()` khi app khoi dong.

## 3. Routing

- Admin route:
  - `Admin/{controller=Dashboard}/{action=Index}/{id?}`
- Public route:
  - `{controller=Home}/{action=Index}/{id?}`

## 4. Cau truc source code hien tai

- `Controllers/Admin`
  - `DashboardController`
  - `CoursesController`
  - `CourseCategoriesController`
  - `AuditLogsController`
- `Services`
  - `DashboardService`
  - `CoursesService`
  - `FinanceService`
  - `AuditLogsService`
- `Services/Interfaces/IServices.cs`
  - DTO + contracts cac service

## 5. Luong xu ly mau (Courses)

1. User mo trang `/Admin/Courses`.
2. `CoursesController.Index` doc filter + paging.
3. Goi `ICoursesService.LayDanhSachPhanTrangAsync`.
4. Service query EF Core (`KhoaHocs`, include `DanhMuc`, `LopHocs`).
5. Tra ve `PagedResult<KhoaHoc>` cho View.

## 6. Vertical slices da co

- Slice 1: Dashboard thong ke.
- Slice 2: Quan ly khoa hoc.
- Slice 3: Quan ly danh muc khoa hoc.
- Slice 4: Audit logs he thong.
- Slice 5: Luong nghiep vu tai chinh (service-level).

## 7. Nguyen tac naming hien dang ap dung

- Controller dat theo domain ro nghia: `CoursesController`, `CourseCategoriesController`.
- Service dat theo domain ro nghia: `CoursesService`, `AuditLogsService`.
- Interface prefix `I` + ten service.
- Action naming uu tien dong tu ro rang: `Index`, `Create`, `Edit`, `Detail`, `Trash`, `restore`, `bulk`.

## 8. Khoang trong can bo sung

- Chua co API JSON public/REST (hien tai chu yeu server-rendered MVC).
- Chua co auth flow day du va policy-based authorization.
- Chua tach dto/request model rieng cho tung use case.
