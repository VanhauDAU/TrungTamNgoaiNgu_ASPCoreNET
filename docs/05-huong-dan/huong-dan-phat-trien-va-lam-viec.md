# 05 - Huong dan

## 1. Yeu cau moi truong

- .NET SDK 10
- SQL Server (local/container)
- Cong cu EF Core (`dotnet-ef`)

## 2. Chay project local

```bash
# restore + build
dotnet restore
dotnet build

# update db theo migration
dotnet ef database update

# run app
dotnet run
```

Mac dinh app doc connection string `DefaultConnection` trong `appsettings.json`.

## 3. Workflow git goi y

```bash
# tu main moi nhat
git checkout main
git pull

# tao nhanh feature
git checkout -b codex/<ten-feature>

# sau khi code + test
git add .
git commit -m "feat(courses): mo ta thay doi"
git push -u origin codex/<ten-feature>
```

Quy tac commit nen dung:

- `feat(...)`: them tinh nang
- `fix(...)`: sua loi
- `refactor(...)`: doi cau truc, khong doi hanh vi
- `docs(...)`: cap nhat tai lieu
- `chore(...)`: cong viec ky thuat khac

## 4. Workflow migration an toan

```bash
# tao migration
dotnet ef migrations add <TenMigration>

# apply DB
dotnet ef database update
```

Neu migration co alter PK/FK:

- Mo file migration, sap thu tu Drop FK/Drop PK/Alter/Add PK/Add FK.
- Khong merge migration neu chua test update tren DB local.

## 5. Huong dan sua CSS

Theo cau truc hien tai:

- `wwwroot/css/admin.css`: style chung toan Admin.
- `wwwroot/css/admin-courses.css`: style rieng trang khoa hoc.
- `wwwroot/css/admin-auditlogs.css`: style rieng trang nhat ky.
- `wwwroot/css/site.css`: frontend public.

Nguyen tac:

- Component dung chung -> de trong `admin.css`.
- CSS dac thu man hinh -> tach file `admin-<module>.css`.
- Moi view co CSS rieng thi import trong `@section Styles`.

## 6. Checklist truoc khi tao PR

1. Build pass: `dotnet build`.
2. Migration update duoc tren DB local.
3. Chuc nang thao tac chinh da test tay.
4. Cap nhat `docs/progress.md` + muc docs lien quan.
