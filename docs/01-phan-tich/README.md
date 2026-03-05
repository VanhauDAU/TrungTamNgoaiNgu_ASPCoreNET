# 01 - Phan tich

## 1. Bai toan

Xay dung he thong quan tri trung tam ngoai ngu gom nhieu module lien thong:

- Khoa hoc, lop hoc, hoc phi
- Dang ky, hoa don, phieu thu
- Hoc vien, giao vien, nhan vien, phan quyen
- Noi dung/blog, thong bao, lien he
- Nhat ky he thong phuc vu audit

He thong huong den 2 muc tieu:

1. So hoa quy trinh van hanh noi bo cua trung tam.
2. Tao nen tang mo rong de them cac tinh nang AI ve sau.

## 2. Van de can giai quyet

- Du lieu hoc vien/lop hoc/tai chinh de roi rac, kho doi soat.
- Kiem soat trang thai nghiep vu chua chat (xoa, tam ngung, khoi phuc).
- Thieu audit trail ro rang: ai sua gi, khi nao.
- Quy trinh thao tac hang loat con thu cong.

## 3. Stakeholders

- Ban giam doc: can dashboard tong quan, bao cao nhanh.
- Phong dao tao: quan ly khoa hoc/lop hoc/lich hoc.
- Ke toan: theo doi hoa don, phieu thu, cong no.
- Giao vu/CSKH: xu ly lien he, thong bao, dang ky.
- Nhan vien ky thuat: van hanh, deployment, migration.

## 4. Pham vi hien tai (co trong code)

- Dashboard thong ke tong quan (`DashboardController` + `DashboardService`).
- Quan ly khoa hoc (`CoursesController` + `CoursesService`).
- Quan ly danh muc khoa hoc (`CourseCategoriesController`).
- Nhat ky he thong (`AuditLogsController` + `AuditLogsService`).
- Logic tai chinh cap service (`FinanceService`).

## 5. Yeu cau chuc nang quan trong

- Quan ly khoa hoc:
  - CRUD + soft delete + khoi phuc.
  - Bulk action: doi trang thai, xoa mem, khoi phuc.
  - Tao slug khong trung.
  - Chan tam ngung khoa hoc neu con lop dang mo/dang hoc.
- Quan ly danh muc:
  - CRUD + soft delete + khoi phuc.
  - Chan xoa mem danh muc neu con khoa hoc active.
- Audit:
  - Ghi log cac hanh dong quan tri quan trong vao `nhatkyhethong`.
  - Loc theo module/tu khoa, xem chi tiet ban ghi.
- Tai chinh:
  - Thu tien theo nhieu dot cho 1 hoa don.
  - Tu dong cap nhat trang thai hoa don theo so tien da tra.

## 6. Yeu cau phi chuc nang

- Tinh toan nhat quan du lieu: su dung EF Core migration.
- Kha nang bao tri: tach service/interface, controller gon.
- Kha nang truy vet: co bang nhat ky he thong.
- Bao mat co ban:
  - Validate upload file anh.
  - Anti-forgery token cho cac POST action.

## 7. Rui ro ky thuat

- Schema lon, nhieu FK phu thuoc cheo -> de loi khi alter column neu migration sai thu tu.
- Chuoi ket noi DB dang nam trong `appsettings.json` (khong an toan cho production).
- Chua co test tu dong -> nguy co hoi quy khi doi logic trang thai.

## 8. Dinh huong tiep theo (uu tien)

1. Hoan thien module LopHoc tach ro khoi KhoaHoc.
2. Bo sung Auth/Authorization that su cho trang admin.
3. Viet integration test cho luong nghiep vu nhay cam (khoa hoc, thu tien, soft delete).
4. Chuan hoa naming + route convention cho toan bo controller/service.
