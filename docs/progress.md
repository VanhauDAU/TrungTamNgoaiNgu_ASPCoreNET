# Progress

Cap nhat: 2026-03-14

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
- Quan ly lop hoc (dot 1):
  - man hinh Index/Create/Edit/Detail/Trash
  - state machine trang thai lop
  - ajax dropdown dong (co so, phong hoc, hoc phi, phuong/xa)
  - validate nghiep vu si so < suc chua phong hoc
  - ma lop tu sinh, khong cho sua tay
  - hien thi giao vien cung/khac co so de phan cong
- Quan ly lop hoc (dot 2):
  - ngay ket thuc tu tinh theo ngay bat dau + lich hoc + goi hoc phi
  - khong load goi hoc phi truoc khi chon khoa hoc
  - dropdown tinh/phuong xa chi hien thi noi dang co co so
  - index lop hoc duoc polish lai theo huong dashboard van hanh gon hon
- Quan ly co so dao tao:
  - tach khoi `ClassSetup` thanh module rieng `Campuses`
  - co man danh sach, create/edit va detail theo tabs
  - quan ly phong hoc trong ngu canh co so
  - xem duoc danh sach nhan su va lop hoc theo tung co so
- Quan ly danh muc khoa hoc:
  - ma danh muc tu sinh o backend, preview tren form
- Quan ly khoa hoc:
  - ma khoa hoc tu sinh o backend, preview tren form

## 3. Dang trien khai / Chua hoan thanh

- Auth/Authorization day du cho admin.
- Buoi hoc, diem danh, va dang ky hoc vien theo lop chua day du man hinh.
- Quan ly hoc vien/nhan su giao dien day du.
- API JSON cho frontend/mobile.
- Tu dong test (unit/integration).

## 4. Backlog uu tien cao

1. Hoan thien module BuoiHoc + DiemDanh theo ngay hoc thuc te.
2. Hoan thien module DangKyLopHoc + HoaDon + PhieuThu tren UI.
3. Bo sung role-based authorization theo `NhomQuyen/PhanQuyen`.
4. Tu dong test cho rule nghiep vu LopHoc (si so/phong, state machine, ngay ket thuc tu tinh).
5. Ket noi CRUD/module chi tiet nhan su vao tab `Nhan su` cua `Campuses`.
6. Chuan hoa migration strategy khi dong bo voi SQL nguon.

## 5. Dinh huong mo rong

- Chatbot ho tro hoc vien.
- AI cham diem noi.
- AI ho tro diem danh tu dong.
- Bao cao thong minh cho van hanh va tai chinh.
