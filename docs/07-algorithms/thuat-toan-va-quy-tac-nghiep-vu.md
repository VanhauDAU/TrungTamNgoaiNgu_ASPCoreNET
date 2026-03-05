# 07 - Algorithms

Tai lieu nay ghi lai cac luat xu ly nghiep vu dang duoc code hoa trong service.

## 1. Tao slug khoa hoc khong trung

Nguon: `CoursesService.TaoSlugKhoaHocAsync` + `ChuanHoaSlug`.

Y tuong:

1. Chuan hoa chuoi ve lower-case, bo dau tieng Viet (`đ -> d`), thay ky tu la bang `-`.
2. Rut gon nhieu dau `-` lien tiep.
3. Neu slug da ton tai trong bang `khoahoc`, them hau to `-1`, `-2`, ... cho den khi duy nhat.

Do phuc tap:

- Trung binh: O(k) lan check, k la so luong slug trung.

## 2. Quy tac doi trang thai khoa hoc

Nguon: `CoursesService.CapNhatCoKiemTraAsync` va `DoiTrangThaiHangLoatAsync`.

Luat:

- Khoa hoc chi co 2 trang thai nghiep vu: `0` (tam ngung), `1` (dang hoat dong).
- Neu doi sang `0`, phai kiem tra trong `lophoc` co ban ghi `TrangThai in (1,4)` khong.
- Neu co lop dang mo/dang hoc -> chan thao tac, tra thong bao nghiep vu.

Muc dich:

- Tranh ngat dong nghiep vu giua khoa hoc va lop hoc dang van hanh.

## 3. Xoa mem danh muc co rang buoc

Nguon: `CoursesService.XoaMemDanhMucAsync`.

Luat:

- Khong cho xoa mem danh muc neu con khoa hoc active (`TrangThai != 0`) va chua bi xoa mem.
- Giai phap truoc khi xoa:
  - chuyen khoa hoc sang danh muc khac, hoac
  - tam ngung khoa hoc.

## 4. Thu tien hoa don nhieu dot

Nguon: `FinanceService.ThuTienAsync`.

Luat:

1. Tao `PhieuThu` moi cho moi lan thu tien.
2. Cong don vao `HoaDon.DaTra`.
3. Tu dong cap nhat `HoaDon.TrangThai`:
   - `2` neu da tra >= tong phai tra
   - `1` neu moi tra mot phan

## 5. Ghi nhat ky he thong

Nguon: `CoursesService.GhiNhatKyAsync`.

Luat:

- Moi thao tac quan trong (tao, cap nhat, xoa mem, khoi phuc, bulk action) se ghi ban ghi vao `nhatkyhethong`.
- Module duoc suy ra tu tieu de hanh dong:
  - Co "danh muc" -> `DanhMuc`
  - Co "khoa hoc" -> `KhoaHoc`
  - Con lai -> `HeThong`

## 6. Luu y chat luong thuat toan

- Cac check nghiep vu dang dua tren query truc tiep EF va du lieu thoi diem hien tai.
- De tranh race condition khi dong thoi cao, can bo sung transaction cho cac luong update phuc tap.
