# Progress

Cap nhat: 2026-03-06

## 1. Trang thai tong quan

- Hoan thanh mot phan he thong quan tri theo huong MVC + Service + EF Core.
- Da co nen tang database lon va migration co ban.
- Da co nhat ky he thong de truy vet thao tac quan tri.

## 2. Da hoan thanh (MVP level)

- Dashboard thong ke tong quan.
- Quan ly khoa hoc:
  - CRUD
  - soft delete + trash + restore
  - bulk update
  - trang chi tiet
- Quan ly danh muc khoa hoc:
  - CRUD
  - soft delete + trash + restore
  - chan xoa khi con khoa hoc active
- Audit logs:
  - danh sach + filter + chi tiet
- Service tai chinh:
  - list hoa don
  - thu tien nhieu dot
  - cap nhat trang thai hoa don

## 3. Dang trien khai / Chua hoan thanh

- Auth/Authorization day du cho admin.
- Quan ly lop hoc, buoi hoc, diem danh man hinh day du.
- Quan ly hoc vien/nhan su giao dien day du.
- API JSON cho frontend/mobile.
- Tu dong test (unit/integration).

## 4. Backlog uu tien cao

1. Hoan thien module LopHoc tach nghiep vu khoi KhoaHoc.
2. Hoan thien module DangKyLopHoc + HoaDon + PhieuThu tren UI.
3. Bo sung role-based authorization theo `NhomQuyen/PhanQuyen`.
4. Viet changelog theo tung PR va lien ket docs.
5. Chuan hoa migration strategy khi dong bo voi SQL nguon.

## 5. Dinh huong mo rong

- Chatbot ho tro hoc vien.
- AI cham diem noi.
- AI ho tro diem danh tu dong.
- Bao cao thong minh cho van hanh va tai chinh.
