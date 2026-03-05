# 04 - API (MVC Endpoints)

Du an hien tai su dung server-rendered MVC, nen "API" chu yeu la endpoint HTML (khong phai REST JSON).

## 1. Public endpoints

| Method | Route | Mo ta |
|---|---|---|
| GET | `/` | Trang chu (`Home/Index`) |
| GET | `/Home/Privacy` | Trang privacy |
| GET | `/Home/Error` | Trang loi |

## 2. Admin Dashboard

| Method | Route | Mo ta |
|---|---|---|
| GET | `/Admin` | Dashboard mac dinh |
| GET | `/Admin/Dashboard/Index` | Dashboard thong ke |

## 3. Admin Courses

| Method | Route | Mo ta |
|---|---|---|
| GET | `/Admin/Courses` | Danh sach khoa hoc (filter + paging) |
| GET | `/Admin/Courses/Detail/{id}` | Chi tiet khoa hoc |
| GET | `/Admin/Courses/Create` | Form tao khoa hoc |
| POST | `/Admin/Courses/Create` | Tao khoa hoc moi |
| GET | `/Admin/Courses/Edit/{id}` | Form sua khoa hoc |
| POST | `/Admin/Courses/Edit` | Cap nhat khoa hoc |
| POST | `/Admin/Courses/softdelete/{id}` | Xoa mem khoa hoc |
| GET | `/Admin/Courses/Trash` | Danh sach khoa hoc da xoa mem |
| POST | `/Admin/Courses/restore/{id}` | Khoi phuc khoa hoc |
| POST | `/Admin/Courses/bulk` | Bulk doi trang thai/xoa mem |
| POST | `/Admin/Courses/bulkrestore` | Bulk khoi phuc |

### Query params cua `GET /Admin/Courses`

- `tuKhoa`: tim theo ten khoa hoc.
- `danhMucId`: loc theo danh muc.
- `trangThai`: 0 hoac 1.
- `page`, `pageSize`: phan trang.

## 4. Admin Course Categories

| Method | Route | Mo ta |
|---|---|---|
| GET | `/Admin/CourseCategories` | Danh sach danh muc |
| GET | `/Admin/CourseCategories/Create` | Form tao danh muc |
| POST | `/Admin/CourseCategories/Create` | Tao danh muc |
| GET | `/Admin/CourseCategories/Edit/{id}` | Form sua danh muc |
| POST | `/Admin/CourseCategories/Edit` | Cap nhat danh muc |
| POST | `/Admin/CourseCategories/softdelete/{id}` | Xoa mem danh muc |
| GET | `/Admin/CourseCategories/Trash` | Danh muc da xoa mem |
| POST | `/Admin/CourseCategories/restore/{id}` | Khoi phuc danh muc |

## 5. Admin Audit Logs

| Method | Route | Mo ta |
|---|---|---|
| GET | `/Admin/AuditLogs` | Danh sach nhat ky (filter + paging) |
| GET | `/Admin/AuditLogs/Details/{id}` | Chi tiet ban ghi nhat ky |

### Query params cua `GET /Admin/AuditLogs`

- `module`: loc module (`KhoaHoc`, `DanhMuc`, `HeThong`, ...).
- `tuKhoa`: tim fulltext co ban tren hanh dong/noi dung/nguoi thuc hien.
- `page`, `pageSize`: phan trang.

## 6. Rang buoc request quan trong

- Cac POST action su dung `[ValidateAntiForgeryToken]`.
- Upload anh khoa hoc:
  - Dinh dang: `.jpg`, `.jpeg`, `.png`, `.webp`
  - MIME: `image/jpeg`, `image/png`, `image/webp`
  - Kich thuoc toi da: 5MB

## 7. De xuat nang cap API ve sau

1. Tao REST API cho module Courses/Categories song song voi MVC view.
2. Tach request/response DTO rieng.
3. Them OpenAPI/Swagger de test nhanh va dong bo tai lieu.
