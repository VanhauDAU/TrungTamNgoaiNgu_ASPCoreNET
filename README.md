# 🎓 Hệ Thống Quản Lý Trung Tâm Ngoại Ngữ

[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-10.0-blue?logo=dotnet)](https://dotnet.microsoft.com/)
[![EF Core](https://img.shields.io/badge/Entity%20Framework%20Core-ORM-purple)](https://docs.microsoft.com/ef/)
[![License](https://img.shields.io/badge/License-Academic-green)](LICENSE)

Ứng dụng **quản lý toàn diện** cho trung tâm ngoại ngữ: học viên, lớp học, điểm danh, học phí, chat realtime, thông báo và CRM liên hệ.

> 📚 **Đồ án chuyên ngành Công nghệ phần mềm** – Khoa CNTT, Năm học 2025-2026

---

## ✨ Tính Năng Chính

| Mô-đun           | Chức năng                                                       |
| ---------------- | --------------------------------------------------------------- |
| 👥 **Tài khoản** | Đa vai trò (Admin/GV/NV/HV), phân quyền nhóm, nhật ký đăng nhập |
| 🏫 **Cơ sở**     | Quản lý nhiều chi nhánh, phòng học, ca học                      |
| 📚 **Đào tạo**   | Khóa học, lớp học, lịch học, điểm danh đa trạng thái            |
| 💰 **Tài chính** | Hóa đơn, phiếu thu, học phí, lương giáo viên                    |
| 💬 **Chat**      | Nhóm lớp + 1-1, emoji, thu hồi tin, tệp đính kèm                |
| 🔔 **Thông báo** | Lên lịch, ưu tiên, ghim, gửi theo nhóm/lớp/cơ sở                |
| 📞 **CRM**       | Tiếp nhận tư vấn, giao phụ trách, theo dõi lịch sử              |
| 📝 **Blog**      | Bài viết SEO, danh mục, tag, quản lý nội dung                   |

---

## 🛠 Công Nghệ

```
ASP.NET Core MVC (.NET 10) · Entity Framework Core · SQL Server
Bootstrap 5 · jQuery · SignalR (Chat Realtime) · Cookie Auth
```

---

## 🚀 Cài Đặt Nhanh

```bash
# Clone
git clone <repo-url> && cd ASPNetProject_TTNN

# Cài EF tools (nếu chưa có)
dotnet tool install --global dotnet-ef

# Chạy migrations
dotnet ef database update

# Khởi động
dotnet run
# → http://localhost:5000
```

**Cấu hình** database trong `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=TrungTamNN;Trusted_Connection=True;"
}
```

---

## 📁 Cấu Trúc Dự Án

```
├── Controllers/     # MVC Controllers (Admin/Client/API)
├── Models/         # Entity models (35+ bảng)
├── Migrations/     # EF Core migrations (chia theo chức năng)
├── Services/       # Business logic
├── Views/          # Razor Views
├── Data/           # AppDbContext
├── docs/           # 📖 Tài liệu tiếng Việt
│   ├── 01-phan-tich/   → Phân tích nghiệp vụ, use case
│   ├── 02-tong-quan/   → Kiến trúc, luồng xử lý
│   ├── 03-database/    → Sơ đồ CSDL, danh sách bảng
│   ├── 04-api/         → Endpoint & API docs
│   ├── 05-huong-dan/   → Setup dev, Git workflow
│   ├── 06-deployment/  → Triển khai production
│   └── 07-algorithms/  → Quy trình nghiệp vụ
└── wwwroot/        # CSS, JS, uploads
```

---

## 🗃 Database Migrations

Migrations chia nhỏ theo từng nhóm chức năng:

| Migration                                | Mô tả                              |
| ---------------------------------------- | ---------------------------------- |
| `20260303..._TaoDatabase_LanDau`         | Schema ban đầu                     |
| `20260310_01_TaoBangChat`                | 7 bảng Chat realtime               |
| `20260310_02_BaoMatDangNhap`             | Nhật ký đăng nhập + buộc đổi MK    |
| `20260310_03_PhongHocBaoTriVaSoftDelete` | Quản lý bảo trì phòng              |
| `20260310_04_TaiCauTrucDiemDanh`         | Redesign điểm danh đầy đủ          |
| `20260310_05_NangCapHeThongThongBao`     | Thông báo: lên lịch, ưu tiên, file |
| `20260310_06_NangCapCRMLienHe`           | CRM liên hệ đầy đủ                 |
| `20260310_07_TaoBangHocTapBoSung`        | Bài thi, tài liệu, đánh giá GV     |

---

## 📖 Tài Liệu

- [Phân tích nghiệp vụ](docs/01-phan-tich/phan-tich-nghiep-vu.md)
- [Kiến trúc hệ thống](docs/02-tong-quan/kien-truc-he-thong.md)
- [Tài liệu CSDL](docs/03-database/tai-lieu-co-so-du-lieu.md)
- [API & Endpoints](docs/04-api/tai-lieu-endpoints-mvc.md)
- [Hướng dẫn phát triển](docs/05-huong-dan/huong-dan-phat-trien.md)
- [Quy trình nghiệp vụ](docs/07-algorithms/quy-trinh-nghiep-vu.md)

---

## 📌 Nguyên Tắc Phát Triển

- ✅ Tạo **migration riêng** cho mỗi thay đổi cấu trúc CSDL
- ✅ Cập nhật **docs** sau mỗi feature merge
- ✅ Commit message theo format: `feat:`, `fix:`, `docs:`, `migration:`
- ✅ Code service layer – không viết logic trong controller

---

_Đồ Án Chuyên Ngành CNPM · Khoa Công Nghệ Thông Tin · 2025-2026_
