# Hướng Dẫn Phát Triển & Làm Việc Với Dự Án

## 1. Yêu Cầu Môi Trường

| Công cụ                 | Phiên bản | Ghi chú                            |
| ----------------------- | --------- | ---------------------------------- |
| .NET SDK                | 10.0+     | `dotnet --version`                 |
| SQL Server              | 2019+     | Hoặc MariaDB 10.4+                 |
| Git                     | 2.x+      |                                    |
| Visual Studio / VS Code | Mới nhất  | Khuyến nghị VS Code + C# Extension |

---

## 2. Cài Đặt Lần Đầu

```bash
# Clone repository
git clone <repo-url>
cd ASPNetProject_TTNN

# Cài EF Core tools (nếu chưa có)
dotnet tool install --global dotnet-ef

# Cấu hình connection string (sửa appsettings.json)
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=TrungTamNN;Trusted_Connection=True;"
  }
}

# Chạy tất cả migrations
dotnet ef database update

# Chạy ứng dụng
dotnet run
```

---

## 3. Làm Việc Với Migration

### Tạo migration mới

```bash
# Tạo migration (TÊN phải mô tả rõ chức năng)
dotnet ef migrations add TenMigration_MoTaRoRang

# VÍ DỤ đúng:
dotnet ef migrations add ThemCotMoTaVaoKhoaHoc
dotnet ef migrations add TaoBangDiemDanh_V2
dotnet ef migrations add NangCapCRMLienHe

# VÍ DỤ sai (quá chung chung):
dotnet ef migrations add Update
dotnet ef migrations add Fix
```

### Áp dụng migration

```bash
# Áp dụng tất cả
dotnet ef database update

# Áp dụng đến migration cụ thể
dotnet ef database update TenMigration

# Rollback về migration trước
dotnet ef database update TenMigrationTruoc
```

### Quy tắc đặt tên migration

```
Format: YYYYMMDD_XX_MoTaRoRang
Ví dụ: 20260310_01_TaoBangChat
        20260310_02_BaoMatDangNhap
        20260315_01_ThemTrinhDoVaoHocVien

Nhóm theo chức năng, KHÔNG gộp nhiều thay đổi vào 1 migration
```

---

## 4. Git Workflow

```bash
# Tạo branch từ main cho feature mới
git checkout -b feature/ten-tinh-nang main

# Commit thường xuyên với message rõ ràng
git commit -m "feat: Thêm chức năng điểm danh online"
git commit -m "fix: Sửa lỗi tính lương giáo viên tháng 2"
git commit -m "docs: Cập nhật tài liệu API endpoint"
git commit -m "migration: Thêm cột hinhThuc vào bảng diemDanh"

# Push và tạo Pull Request
git push origin feature/ten-tinh-nang
```

### Quy tắc commit message

| Prefix       | Ý nghĩa            |
| ------------ | ------------------ |
| `feat:`      | Tính năng mới      |
| `fix:`       | Sửa lỗi            |
| `docs:`      | Cập nhật tài liệu  |
| `migration:` | Thêm/sửa migration |
| `refactor:`  | Tái cấu trúc code  |
| `test:`      | Thêm/sửa test      |

---

## 5. Khi Merge Feature Mới

Sau khi merge PR, cập nhật các file sau:

1. **`docs/03-database/tai-lieu-co-so-du-lieu.md`** – nếu có thêm/sửa bảng CSDL
2. **`docs/04-api/tai-lieu-endpoints-mvc.md`** – nếu có thêm/sửa route/controller
3. **`docs/07-algorithms/quy-trinh-nghiep-vu.md`** – nếu thay đổi quy tắc nghiệp vụ
4. **`docs/progress.md`** – cập nhật tiến độ module

---

## 6. Kiểm Tra Code Trước Khi Push

```bash
# Build để kiểm tra lỗi biên dịch
dotnet build

# Chạy ứng dụng và test thủ công
dotnet run

# Kiểm tra migration hợp lệ
dotnet ef migrations list
```

---

## 7. Cấu Trúc Controller Chuẩn

```csharp
// Controllers/Admin/KhoaHocController.cs
[Area("Admin")]
[Authorize(Roles = "Admin,NhanVien")]
public class KhoaHocController : Controller
{
    private readonly IKhoaHocService _service;

    // Index - danh sách
    public async Task<IActionResult> Index() { ... }

    // Create GET + POST
    public IActionResult Create() { ... }
    [HttpPost] public async Task<IActionResult> Create(KhoaHocDto dto) { ... }

    // Edit GET + POST
    public async Task<IActionResult> Edit(int id) { ... }
    [HttpPost] public async Task<IActionResult> Edit(int id, KhoaHocDto dto) { ... }

    // Delete POST (xóa mềm)
    [HttpPost] public async Task<IActionResult> Delete(int id) { ... }
}
```

---

## 8. Service Pattern

```csharp
// Services/IKhoaHocService.cs
public interface IKhoaHocService
{
    Task<List<KhoaHoc>> GetAllAsync(bool includeDeleted = false);
    Task<KhoaHoc?> GetByIdAsync(int id);
    Task<KhoaHoc> CreateAsync(KhoaHocDto dto);
    Task UpdateAsync(int id, KhoaHocDto dto);
    Task SoftDeleteAsync(int id);  // Xóa mềm (set deleted_at)
    Task RestoreAsync(int id);      // Khôi phục
}
```
