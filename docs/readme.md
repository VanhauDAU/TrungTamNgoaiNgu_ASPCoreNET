# 🎓 Hệ Thống Quản Lý Trung Tâm Ngoại Ngữ

> **Dự án đồ án chuyên ngành CNPM** – Ứng dụng quản lý toàn diện cho trung tâm ngoại ngữ, xây dựng bằng **ASP.NET Core MVC** + **Entity Framework Core** + **SQL Server**.

---

## ✨ Tính Năng Chính

| Mô-đun                        | Mô tả                                                       |
| ----------------------------- | ----------------------------------------------------------- |
| 👥 **Tài khoản & Phân quyền** | Học viên, Giáo viên, Nhân viên, Admin. Nhóm quyền tùy chỉnh |
| 🏫 **Cơ sở đào tạo**          | Quản lý nhiều chi nhánh, phòng học, ca học                  |
| 📚 **Khóa học & Lớp học**     | Danh mục, lịch học, đăng ký, điểm danh, bài thi             |
| 💰 **Tài chính**              | Hóa đơn, phiếu thu, học phí, lương giáo viên                |
| 💬 **Chat thời gian thực**    | Nhóm lớp học, nhắn tin 1-1, emoji, thu hồi tin              |
| 🔔 **Thông báo nâng cao**     | Lên lịch, ưu tiên, gửi theo nhóm/lớp/cơ sở, tệp đính kèm    |
| 📝 **Blog & CMS**             | Bài viết, danh mục, tag, slug SEO-friendly                  |
| 📞 **CRM Liên Hệ**            | Tiếp nhận, phân loại, giao phụ trách, theo dõi lịch sử      |
| 📊 **Báo cáo**                | Thống kê học viên, doanh thu, hiệu suất giảng dạy           |

---

## 🛠 Công Nghệ Sử Dụng

```
Backend:    ASP.NET Core MVC (.NET 10)
ORM:        Entity Framework Core (SQL Server)
Database:   MariaDB/MySQL (development), SQL Server (production)
Frontend:   Razor Views + Bootstrap 5 + jQuery
Realtime:   SignalR (chat)
Auth:       Cookie-based Authentication
```

---

## 🗂 Cấu Trúc Dự Án

```
TrungTamNgoaiNgu/
├── Controllers/          # MVC Controllers (Admin, Client, API)
├── Models/              # Entity models (35+ bảng)
├── Migrations/          # EF Core migrations (chia theo chức năng)
│   ├── 20260303..._TaoDatabase_LanDau.cs       # Schema ban đầu
│   ├── 20260310_01_TaoBangChat.cs              # Hệ thống chat
│   ├── 20260310_02_BaoMatDangNhap.cs           # Nhật ký & bảo mật
│   ├── 20260310_03_PhongHocBaoTriVaSoftDelete.cs
│   ├── 20260310_04_TaiCauTrucDiemDanh.cs       # Điểm danh redesign
│   ├── 20260310_05_NangCapHeThongThongBao.cs   # Thông báo nâng cao
│   ├── 20260310_06_NangCapCRMLienHe.cs         # CRM liên hệ
│   └── 20260310_07_TaoBangHocTapBoSung.cs      # Bài thi, tài liệu
├── Services/            # Business logic services
├── Views/               # Razor views theo module
├── Data/                # AppDbContext
├── docs/                # 📖 Tài liệu dự án (tiếng Việt)
│   ├── 01-phan-tich/   # Phân tích nghiệp vụ, use case
│   ├── 02-tong-quan/   # Kiến trúc, sơ đồ hệ thống
│   ├── 03-database/    # Tài liệu CSDL, quan hệ bảng
│   ├── 04-api/         # Danh sách endpoint & API
│   ├── 05-huong-dan/   # Setup dev, Git workflow
│   ├── 06-deployment/  # Hướng dẫn triển khai
│   └── 07-algorithms/  # Quy trình nghiệp vụ
└── wwwroot/             # Static files (CSS, JS, uploads)
```

---

## 🚀 Hướng Dẫn Cài Đặt Nhanh

### Yêu cầu

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server 2019+ hoặc MariaDB 10.4+
- Node.js (cho bundling assets)

### Các bước

```bash
# 1. Clone dự án
git clone <repository-url>
cd ASPNetProject_TTNN

# 2. Cấu hình kết nối database trong appsettings.json
# "ConnectionStrings": { "DefaultConnection": "..." }

# 3. Chạy migrations
dotnet ef database update

# 4. Khởi động ứng dụng
dotnet run
# Mở http://localhost:5000
```

### Tài khoản mặc định (demo)

| Tài khoản | Mật khẩu    | Vai trò       |
| --------- | ----------- | ------------- |
| `admin`   | `Admin@123` | Quản trị viên |

---

## 📖 Tài Liệu Chi Tiết

Xem thư mục [`docs/`](docs/) để đọc tài liệu đầy đủ bằng tiếng Việt:

- [📊 Phân tích nghiệp vụ](docs/01-phan-tich/)
- [🏗 Kiến trúc hệ thống](docs/02-tong-quan/)
- [🗃 Tài liệu cơ sở dữ liệu](docs/03-database/)
- [🔌 Tài liệu API & Endpoint](docs/04-api/)
- [💻 Hướng dẫn phát triển](docs/05-huong-dan/)
- [🚀 Hướng dẫn triển khai](docs/06-deployment/)
- [⚙️ Quy trình nghiệp vụ](docs/07-algorithms/)

---

## 📌 Nguyên Tắc Phát Triển

- Mỗi khi thêm tính năng, tạo migration riêng (không dồn vào 1 file)
- Cập nhật `docs/03-database/` khi thêm/sửa bảng CSDL
- Cập nhật `docs/04-api/` khi thêm/sửa route/controller
- Cập nhật `docs/07-algorithms/` khi thay đổi quy tắc nghiệp vụ

---

## 👨‍💻 Nhóm Phát Triển

Đồ án chuyên ngành – Khoa Công nghệ thông tin  
Năm học 2025-2026
