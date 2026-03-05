# 06 - Deployment

## 1. Muc tieu

Trien khai ung dung ASP.NET Core MVC len moi truong server, su dung SQL Server va migration co kiem soat.

## 2. Chuan bi truoc deploy

- Co server chay .NET runtime tuong thich (`net10.0`).
- Co SQL Server production/staging.
- Co bien moi truong cho connection string (khong hard-code trong file).

## 3. Cau hinh bao mat toi thieu

- Khong commit mat khau SQL that vao git.
- Dat `ConnectionStrings__DefaultConnection` qua environment variable.
- Bat HTTPS o reverse proxy/IIS/Nginx.

## 4. Quy trinh deploy de xuat

1. Pull code tu nhanh da duoc merge.
2. Build release:

```bash
dotnet publish -c Release -o ./publish
```

3. Backup database production.
4. Chay migration:

```bash
dotnet ef database update
```

5. Deploy artifact `publish/` len server.
6. Restart service process.
7. Smoke test:
   - `/Admin`
   - `/Admin/Courses`
   - `/Admin/CourseCategories`
   - `/Admin/AuditLogs`

## 5. Rollback strategy

- Rollback app: quay lai build artifact truoc do.
- Rollback DB: phuc hoi tu backup (vi migration co the khong down an toan 100%).
- Ghi ro migration da chay trong changelog phien ban.

## 6. Monitoring toi thieu

- Bat log warning/error cua ASP.NET Core.
- Theo doi:
  - Loi migration/lien ket DB
  - Loi upload file
  - Loi route admin quan trong
