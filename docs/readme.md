# Tai lieu du an TrungTamNgoaiNgu

Bo tai lieu nay duoc chia theo tung "vertical slice" de de doc, de cap nhat va de demo voi giang vien.

## Cau truc

- `docs/01-phan-tich/phan-tich-nghiep-vu-va-pham-vi.md`: Phan tich bai toan, pham vi, yeu cau nghiep vu.
- `docs/02-tong-quan/kien-truc-va-tong-quan-he-thong.md`: Tong quan kien truc, cau truc source code, luong xu ly.
- `docs/03-database/tai-lieu-co-so-du-lieu.md`: Mo hinh CSDL, quan he, migration, quy tac schema.
- `docs/04-api/tai-lieu-endpoints-mvc.md`: Danh sach endpoint hien co (MVC route) va input/output chinh.
- `docs/05-huong-dan/huong-dan-phat-trien-va-lam-viec.md`: Huong dan setup dev, chay app, workflow git/migration.
- `docs/06-deployment/huong-dan-trien-khai-he-thong.md`: Huong dan release va trien khai production.
- `docs/07-algorithms/thuat-toan-va-quy-tac-nghiep-vu.md`: Cac thuat toan/quy tac nghiep vu quan trong.
- `docs/data.sql`: Script SQL tham khao de kiem tra du lieu/bao cao nhanh.
- `docs/progress.md`: Tien do module, da xong/chua xong, ke hoach tiep theo.

## Pham vi tai lieu hien tai

Tai lieu phan tich dua tren code dang co trong nhanh hien tai:

- ASP.NET Core MVC (`net10.0`)
- Entity Framework Core SQL Server
- Modules da co man hinh + service: Dashboard, Khoa hoc, Danh muc khoa hoc, Audit logs, Tai chinh (service)

## Nguyen tac cap nhat

- Moi khi merge feature, cap nhat toi thieu 3 muc: `04-api`, `03-database`, `progress`.
- Neu doi quy tac nghiep vu (validation, status machine), cap nhat them `07-algorithms`.
- Neu doi route/controller naming, cap nhat `02-tong-quan` va `04-api`.
