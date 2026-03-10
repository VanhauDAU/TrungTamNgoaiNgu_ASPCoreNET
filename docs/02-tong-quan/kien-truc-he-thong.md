# Tổng Quan Kiến Trúc Hệ Thống

## 1. Kiến Trúc Tổng Quan

```
[Trình duyệt]
    │ HTTP/WebSocket
    ↓
[ASP.NET Core MVC]
    ├── Controllers/Admin/   → Quản trị hệ thống
    ├── Controllers/Client/  → Giao diện học viên
    ├── Controllers/Api/     → REST API (chat, thông báo realtime)
    └── Hubs/ChatHub.cs      → SignalR (WebSocket cho chat)
        │
        ↓
[Services Layer]
    ├── KhoaHocService
    ├── LopHocService
    ├── DiemDanhService
    ├── HoaDonService
    ├── ThongBaoService
    └── ChatService
        │
        ↓
[Data Layer - Entity Framework Core]
    └── AppDbContext → SQL Server / MariaDB
```

---

## 2. Phân Tách Module

| Module    | Controllers                  | Services          | Bảng CSDL                       |
| --------- | ---------------------------- | ----------------- | ------------------------------- |
| Tài khoản | `TaiKhoanController`         | `TaiKhoanService` | taikhoan, hosonguoidung, nhansu |
| Khóa học  | `KhoaHocController`          | `KhoaHocService`  | khoahoc, danhmuckhoahoc, hocphi |
| Lớp học   | `LopHocController`           | `LopHocService`   | lophoc, buoihoc, cahoc          |
| Học viên  | `HocVienController`          | `HocVienService`  | dangkylophoc, diemdanh          |
| Tài chính | `TaiChinhController`         | `HoaDonService`   | hoadon, phieuthu, luong         |
| Chat      | `ChatController` + `ChatHub` | `ChatService`     | chat_rooms, chat_messages...    |
| Thông báo | `ThongBaoController`         | `ThongBaoService` | thongbao, thongbaonguoidung     |
| CRM       | `LienHeController`           | `LienHeService`   | lienhe, lienhe_lichsu           |
| Blog      | `BaiVietController`          | `BaiVietService`  | baiviet, danhmucbaiviet         |
| Cơ sở     | `CoSoController`             | `CoSoService`     | cosodaotao, phonghoc            |

---

## 3. Luồng Xử Lý Request Chính

```
HTTP Request → Middleware (Auth, CORS, Logging)
    → Router → Controller.Action()
        → Validate Input (ModelState)
        → Service Layer (Business Logic)
            → Repository (EF Core + DbContext)
                → SQL Server
            ← Entity
        ← ViewModel/DTO
    → View (Razor) hoặc JsonResult
← HTTP Response
```

---

## 4. Cấu Hình Quan Trọng (appsettings.json)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=...;Database=TrungTamNN;..."
  },
  "AppSettings": {
    "TenTrungTam": "Five Genius Academy",
    "ChatMessageRecallDeadlineHours": 24,
    "MaxFileUploadMB": 10,
    "AllowedFileExtensions": [".pdf", ".docx", ".xlsx", ".jpg", ".png"]
  }
}
```

---

## 5. Soft Delete Pattern

Các bảng có `deleted_at`:

- `taikhoan`, `khoahoc`, `lophoc`, `baiviet`, `lienhe`, `thongbao`, `phonghoc`

```csharp
// Query luôn lọc deleted_at = NULL (chưa xóa)
var list = await _db.KhoaHocs
    .Where(k => k.deleted_at == null)
    .ToListAsync();

// Xóa mềm
entity.deleted_at = DateTime.UtcNow;
await _db.SaveChangesAsync();

// Xem thùng rác
var deleted = await _db.KhoaHocs
    .Where(k => k.deleted_at != null)
    .ToListAsync();
```
