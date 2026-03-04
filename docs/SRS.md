# SRS - TrungTamNgoaiNgu

## 1. Tai lieu

- Project: TrungTamNgoaiNgu (ASP.NET Core MVC)
- Document: Software Requirements Specification (SRS)
- Version: v1.1 (Roadmap-based)
- Last update: 2026-03-04
- Author: Le Van Hau - Sinh vien Truong Dai Hoc Kien Truc Da Nang

## 2. Cach doc SRS khi du an chua hoan thanh

Tai lieu nay khong gia dinh tat ca chuc nang da xong. Moi yeu cau deu co:

- `Status`: `Implemented` | `Planned` | `Research`
- `Target Release`: `v1.0` | `v1.1` | `v1.2` | `v2.0+`
- `Priority`: `P1` (cao) | `P2` (trung binh) | `P3` (thap)

## 3. Pham vi he thong

He thong quan ly trung tam ngoai ngu gom 19 module nghiep vu chinh:

1. Quan ly khoa hoc va goi hoc phi
2. Quan ly danh muc khoa hoc
3. Quan ly tin tuc/blogs
4. Quan ly tai khoan
5. Quan ly hoc vien/nhan vien/giao vien
6. Quan ly diem danh
7. Quan ly tai lieu
8. Quan ly co so dao tao
9. Quan ly lien he
10. Quan ly dang ky lop hoc
11. Quan ly hoa don va phieu thu
12. Quan ly luong giao vien/nhan vien
13. Quan ly quyen truy cap va nhom quyen
14. Quan ly noi dung bai hoc
15. Quan ly phan hoi buoi hoc
16. Quan ly phong hoc
17. Quan ly cai dat he thong
18. Quan ly thong bao he thong
19. Quan ly buoi hoc

## 4. Roadmap tong quan

| Release | Muc tieu                                                                                |
| ------- | --------------------------------------------------------------------------------------- |
| v1.0    | Core admin: khoa hoc, danh muc, nhat ky he thong, dashboard, nen tang tai chinh service |
| v1.1    | Hoc vu core: lop hoc, dang ky lop, phong hoc, buoi hoc, diem danh co ban                |
| v1.2    | Van hanh mo rong: blog, tai lieu, lien he, co so dao tao, thong bao, cai dat            |
| v2.0+   | AI features: chatbot, diem danh AI, AI cham noi                                         |

## 5. Trang thai module (Module Catalog)

| Module                                     | Status hien tai                                 | Target release |
| ------------------------------------------ | ----------------------------------------------- | -------------- |
| Khoa hoc + hoc phi                         | Implemented (core)                              | v1.0           |
| Danh muc khoa hoc                          | Implemented (core)                              | v1.0           |
| Tin tuc/blogs                              | Planned                                         | v1.2           |
| Tai khoan                                  | Planned (service interface co san)              | v1.1           |
| Hoc vien/nhan vien/giao vien               | Planned                                         | v1.1           |
| Diem danh                                  | Planned                                         | v1.1           |
| Tai lieu                                   | Planned                                         | v1.2           |
| Co so dao tao + tinh/thanh + phuong/xa API | Planned                                         | v1.2           |
| Lien he                                    | Planned                                         | v1.2           |
| Dang ky lop hoc                            | Planned (model co)                              | v1.1           |
| Hoa don + phieu thu                        | Implemented (service layer), UI mo rong Planned | v1.1           |
| Luong giao vien/nhan vien                  | Planned                                         | v1.2           |
| Quyen truy cap + nhom quyen                | Planned (model co)                              | v1.1           |
| Noi dung bai hoc                           | Planned                                         | v1.2           |
| Phan hoi buoi hoc                          | Planned                                         | v1.2           |
| Phong hoc                                  | Planned                                         | v1.1           |
| Cai dat he thong                           | Planned                                         | v1.2           |
| Thong bao he thong                         | Planned                                         | v1.2           |
| Buoi hoc                                   | Planned                                         | v1.1           |
| AI features                                | Research                                        | v2.0+          |

## 6. Business rules cot loi

- BR-001: Trang thai khoa hoc chi gom `Dang hoat dong` va `Tam ngung`.
- BR-002: Trang thai `Dang mo`/`Sap khai giang` thuoc cap `Lop hoc`, khong thuoc `Khoa hoc`.
- BR-003: Khong cho tam ngung khoa hoc neu con lop dang mo/dang hoc.
- BR-004: Khong cho xoa mem danh muc neu con khoa hoc dang hoat dong trong danh muc do.
- BR-005: Mot hoa don co the co nhieu phieu thu; trang thai hoa don cap nhat theo tong tien da thu.
- BR-006: Moi thay doi quan tri quan trong phai ghi vao nhat ky he thong.

## 7. Functional Requirements Catalog

### 7.1 Khoa hoc + goi hoc phi

| ID            | Requirement                                             | Status      | Target | Priority |
| ------------- | ------------------------------------------------------- | ----------- | ------ | -------- |
| FR-COURSE-001 | Liet ke khoa hoc co tim kiem, bo loc, phan trang        | Implemented | v1.0   | P1       |
| FR-COURSE-002 | Tao khoa hoc (slug tu dong, upload anh hop le)          | Implemented | v1.0   | P1       |
| FR-COURSE-003 | Sua khoa hoc va kiem tra rang buoc tam ngung            | Implemented | v1.0   | P1       |
| FR-COURSE-004 | Xoa mem/khoi phuc khoa hoc                              | Implemented | v1.0   | P1       |
| FR-COURSE-005 | Bulk action khoa hoc (doi trang thai/xoa mem/khoi phuc) | Implemented | v1.0   | P1       |
| FR-COURSE-006 | Quan ly goi hoc phi theo khoa hoc                       | Planned     | v1.1   | P1       |
| FR-COURSE-007 | Trang chi tiet khoa hoc (thong tin + lop + hoc phi)     | Implemented | v1.0   | P2       |
| FR-COURSE-008 | Auto-save ban nhap va canh bao chua luu                 | Implemented | v1.0   | P2       |

### 7.2 Danh muc khoa hoc

| ID          | Requirement                          | Status      | Target | Priority |
| ----------- | ------------------------------------ | ----------- | ------ | -------- |
| FR-CATE-001 | CRUD danh muc khoa hoc               | Implemented | v1.0   | P1       |
| FR-CATE-002 | Xoa mem/khoi phuc danh muc           | Implemented | v1.0   | P1       |
| FR-CATE-003 | Chan xoa mem neu con khoa hoc active | Implemented | v1.0   | P1       |

### 7.3 Tin tuc/blogs

| ID          | Requirement                                  | Status  | Target | Priority |
| ----------- | -------------------------------------------- | ------- | ------ | -------- |
| FR-BLOG-001 | CRUD bai viet                                | Planned | v1.2   | P2       |
| FR-BLOG-002 | Quan ly category cho bai viet (many-to-many) | Planned | v1.2   | P2       |
| FR-BLOG-003 | Quan ly tag cho bai viet (many-to-many)      | Planned | v1.2   | P2       |

### 7.4 Tai khoan

| ID         | Requirement                                 | Status  | Target | Priority |
| ---------- | ------------------------------------------- | ------- | ------ | -------- |
| FR-ACC-001 | Dang nhap/dang xuat tai khoan admin/noi bo  | Planned | v1.1   | P1       |
| FR-ACC-002 | Khoa/mo tai khoan                           | Planned | v1.1   | P1       |
| FR-ACC-003 | Doi mat khau va cap nhat lan dang nhap cuoi | Planned | v1.1   | P2       |

### 7.5 Hoc vien/nhan vien/giao vien

| ID          | Requirement                                    | Status  | Target | Priority |
| ----------- | ---------------------------------------------- | ------- | ------ | -------- |
| FR-USER-001 | Quan ly danh sach hoc vien/nhan vien/giao vien | Planned | v1.1   | P1       |
| FR-USER-002 | Cap nhat ho so nguoi dung                      | Planned | v1.1   | P1       |
| FR-USER-003 | Gan nhom quyen theo vai tro                    | Planned | v1.1   | P2       |

### 7.6 Diem danh

| ID         | Requirement                             | Status  | Target | Priority |
| ---------- | --------------------------------------- | ------- | ------ | -------- |
| FR-ATT-001 | Tao phien diem danh theo buoi hoc       | Planned | v1.1   | P1       |
| FR-ATT-002 | Ghi nhan co mat/vang/muon               | Planned | v1.1   | P1       |
| FR-ATT-003 | Xem lich su diem danh theo hoc vien/lop | Planned | v1.1   | P2       |

### 7.7 Tai lieu

| ID         | Requirement                               | Status  | Target | Priority |
| ---------- | ----------------------------------------- | ------- | ------ | -------- |
| FR-DOC-001 | Upload tai lieu cho khoa hoc/lop/buoi hoc | Planned | v1.2   | P2       |
| FR-DOC-002 | Quyen xem/tai tai lieu theo vai tro       | Planned | v1.2   | P2       |

### 7.8 Co so dao tao

| ID            | Requirement                            | Status  | Target | Priority |
| ------------- | -------------------------------------- | ------- | ------ | -------- |
| FR-BRANCH-001 | CRUD co so dao tao                     | Planned | v1.2   | P2       |
| FR-BRANCH-002 | Goi API lay phuong/xa theo dia chi moi | Planned | v1.2   | P2       |

### 7.9 Lien he

| ID             | Requirement                       | Status  | Target | Priority |
| -------------- | --------------------------------- | ------- | ------ | -------- |
| FR-CONTACT-001 | Tiep nhan va luu lien he tu form  | Planned | v1.2   | P2       |
| FR-CONTACT-002 | Quan tri trang thai xu ly lien he | Planned | v1.2   | P2       |

### 7.10 Dang ky lop hoc

| ID            | Requirement                                | Status  | Target | Priority |
| ------------- | ------------------------------------------ | ------- | ------ | -------- |
| FR-ENROLL-001 | Tao dang ky hoc vien vao lop               | Planned | v1.1   | P1       |
| FR-ENROLL-002 | Kiem tra si so toi da va dieu kien dau vao | Planned | v1.1   | P1       |
| FR-ENROLL-003 | Lich su dang ky theo hoc vien              | Planned | v1.1   | P2       |

### 7.11 Hoa don va phieu thu

| ID             | Requirement                          | Status                | Target | Priority |
| -------------- | ------------------------------------ | --------------------- | ------ | -------- |
| FR-INVOICE-001 | Tra cuu danh sach hoa don            | Implemented (service) | v1.0   | P1       |
| FR-INVOICE-002 | Thu tien nhieu lan tren 1 hoa don    | Implemented (service) | v1.0   | P1       |
| FR-INVOICE-003 | UI quan tri hoa don/phieu thu day du | Planned               | v1.1   | P1       |

### 7.12 Luong giao vien va nhan vien

| ID             | Requirement                                       | Status  | Target | Priority |
| -------------- | ------------------------------------------------- | ------- | ------ | -------- |
| FR-PAYROLL-001 | Tinh luong theo so buoi day + phu cap/thuong/phat | Planned | v1.2   | P2       |
| FR-PAYROLL-002 | Chot bang luong theo thang                        | Planned | v1.2   | P2       |

### 7.13 Quyen truy cap va nhom quyen

| ID          | Requirement                                 | Status  | Target | Priority |
| ----------- | ------------------------------------------- | ------- | ------ | -------- |
| FR-RBAC-001 | CRUD nhom quyen                             | Planned | v1.1   | P1       |
| FR-RBAC-002 | Gan quyen theo tinh nang (xem/them/sua/xoa) | Planned | v1.1   | P1       |
| FR-RBAC-003 | Kiem tra quyen truy cap theo action         | Planned | v1.1   | P1       |

### 7.14 Noi dung bai hoc

| ID            | Requirement                                  | Status  | Target | Priority |
| ------------- | -------------------------------------------- | ------- | ------ | -------- |
| FR-LESSON-001 | CRUD noi dung bai hoc theo khoa hoc/lop/buoi | Planned | v1.2   | P2       |
| FR-LESSON-002 | Quan ly phien ban noi dung bai hoc           | Planned | v1.2   | P3       |

### 7.15 Phan hoi buoi hoc

| ID          | Requirement                              | Status  | Target | Priority |
| ----------- | ---------------------------------------- | ------- | ------ | -------- |
| FR-FEED-001 | Hoc vien gui phan hoi sau buoi hoc       | Planned | v1.2   | P2       |
| FR-FEED-002 | Giao vien/quan tri xem tong hop phan hoi | Planned | v1.2   | P2       |

### 7.16 Phong hoc

| ID          | Requirement                                  | Status  | Target | Priority |
| ----------- | -------------------------------------------- | ------- | ------ | -------- |
| FR-ROOM-001 | CRUD phong hoc theo co so                    | Planned | v1.1   | P1       |
| FR-ROOM-002 | Quan ly suc chua va trang thai su dung phong | Planned | v1.1   | P1       |

### 7.17 Cai dat he thong

| ID         | Requirement                                             | Status  | Target | Priority |
| ---------- | ------------------------------------------------------- | ------- | ------ | -------- |
| FR-SET-001 | Quan ly tham so he thong (ten trung tam, timezone, ...) | Planned | v1.2   | P2       |
| FR-SET-002 | Cau hinh template thong bao/email co ban                | Planned | v1.2   | P3       |

### 7.18 Thong bao he thong

| ID          | Requirement                                      | Status  | Target | Priority |
| ----------- | ------------------------------------------------ | ------- | ------ | -------- |
| FR-NOTI-001 | Tao va gui thong bao he thong                    | Planned | v1.2   | P2       |
| FR-NOTI-002 | Luu lich su thong bao va trang thai doc/chua doc | Planned | v1.2   | P2       |

### 7.19 Buoi hoc

| ID             | Requirement                                | Status  | Target | Priority |
| -------------- | ------------------------------------------ | ------- | ------ | -------- |
| FR-SESSION-001 | Tao lich buoi hoc cho lop hoc              | Planned | v1.1   | P1       |
| FR-SESSION-002 | Gan giao vien theo tung buoi hoc           | Planned | v1.1   | P1       |
| FR-SESSION-003 | Doi giao vien buoi hoc va ghi log thay doi | Planned | v1.1   | P2       |

### 7.20 Nhat ky he thong

| ID         | Requirement                     | Status      | Target | Priority |
| ---------- | ------------------------------- | ----------- | ------ | -------- |
| FR-LOG-001 | Ghi log ai sua gi, khi nao      | Implemented | v1.0   | P1       |
| FR-LOG-002 | Tra cuu log theo module/tu khoa | Implemented | v1.0   | P1       |
| FR-LOG-003 | Xem chi tiet log                | Implemented | v1.0   | P2       |

### 7.21 AI (future)

| ID        | Requirement                                      | Status   | Target | Priority |
| --------- | ------------------------------------------------ | -------- | ------ | -------- |
| FR-AI-001 | Chatbot ho tro hoc vien (FAQ + tra cuu lich hoc) | Research | v2.0+  | P2       |
| FR-AI-002 | Diem danh tu dong bang AI                        | Research | v2.0+  | P2       |
| FR-AI-003 | AI cham diem ky nang noi tieng Anh               | Research | v2.0+  | P3       |

## 8. Non-functional Requirements (NFR)

| ID            | Requirement                                              | Priority |
| ------------- | -------------------------------------------------------- | -------- |
| NFR-PERF-001  | Trang danh sach admin phai ho tro phan trang va bo loc   | P1       |
| NFR-DATA-001  | Soft delete cho cac doi tuong nghiep vu chinh            | P1       |
| NFR-AUDIT-001 | Hanh dong quan tri quan trong phai co audit log          | P1       |
| NFR-SEC-001   | Bao ve action POST bang anti-forgery token               | P1       |
| NFR-MAIN-001  | Naming convention ro nghia theo domain                   | P2       |
| NFR-UX-001    | Form lon can co auto-save va canh bao roi trang          | P2       |
| NFR-INT-001   | Tich hop API ngoai phai co timeout + xu ly loi           | P2       |
| NFR-PRIV-001  | AI feature phai dap ung yeu cau bao mat du lieu hoc vien | P1       |

## 9. Out of Scope cho v1.0

- Blogs full workflow.
- Attendance UI.
- Branch address API integration.
- Payroll UI.
- AI features.

## 10. Acceptance theo release

### v1.0

- Quan ly khoa hoc/danh muc hoat dong on dinh.
- Trang thai khoa hoc dung nghiep vu (0/1).
- Rang buoc chan tam ngung/xoa mem dung.
- Co nhat ky he thong cho thao tac quan trong.

### v1.1

- Co flow dang ky lop hoc + phong hoc + buoi hoc + diem danh co ban.
- Co RBAC co ban va account management.
- Hoa don/phieu thu co giao dien van hanh.

### v1.2

- Co blogs/tai lieu/lien he/co so dao tao/cai dat/thong bao.
- Hoan thien nghiep vu payroll.

### v2.0+

- Co prototype AI chatbot.
- Co PoC diem danh AI va AI cham noi.

## 11. Vertical slice de demo voi giang vien

### Slice A (v1.0 - da co)

1. Tao danh muc.
2. Tao khoa hoc, upload anh, sinh slug.
3. Sua khoa hoc, doi trang thai.
4. Thu xoa mem danh muc khi con khoa hoc active (bi chan).
5. Vao Audit Logs xem lich su thao tac.

### Slice B (v1.1 - ke tiep)

1. Tao lop hoc cho khoa hoc.
2. Dang ky hoc vien vao lop.
3. Tao buoi hoc, gan giao vien.
4. Mo phien diem danh va luu ket qua.

### Slice C (v1.2 - mo rong)

1. Tao bai viet + tag + category.
2. Tao co so dao tao tu du lieu dia chi API.
3. Gui thong bao he thong.

## 12. Quy tac cap nhat SRS

Moi khi co PR thay doi nghiep vu, phai cap nhat:

1. FR lien quan (Status + Target Release).
2. Business Rule neu logic thay doi.
3. Acceptance criteria cua release bi anh huong.
4. Changelog tai lieu (neu can, trong PR description).
