# wwwroot/css — Cấu trúc CSS

## Cấu trúc hiện tại

```
wwwroot/
└── css/
    ├── admin.css          ← Layout + components toàn bộ admin (sidebar, topbar, table, cards...)
    ├── site.css           ← CSS cho trang public (client-facing)
    └── STRUCTURE.md       ← File này
```

## Quy tắc sử dụng

| File        | Dùng cho                           | Layout                |
| ----------- | ---------------------------------- | --------------------- |
| `admin.css` | Mọi trang Admin (`/Admin/*`)       | `_AdminLayout.cshtml` |
| `site.css`  | Trang frontend (Home, HoaDon, ...) | `_Layout.cshtml`      |

## admin.css — Các section bên trong

admin.css được chia thành các section rõ ràng bằng comment đầu dòng:

```css
/* ═══ CSS VARIABLES (màu sắc, spacing) ═══ */
/* ═══ RESET / BASE ═══ */
/* ═══ LAYOUT (sidebar, main-wrap) ═══ */
/* ═══ SIDEBAR ═══ */
/* ═══ TOPBAR ═══ */
/* ═══ PAGE CONTENT ═══ */
/* ═══ CARDS ═══ */
/* ═══ TABLES ═══ */
/* ═══ FORMS ═══ */
/* ═══ BUTTONS ═══ */
/* ═══ BADGES ═══ */
/* ═══ UTILITIES ═══ */
/* ═══ EMPTY STATES ═══ */
/* ═══ TOASTS / ALERTS ═══ */
```

## Khi nào cần thêm file CSS mới?

Chỉ thêm file mới nếu một trang có **>200 dòng CSS riêng và không tái sử dụng** ở trang khác.
Trong trường hợp đó, đặt tên theo pattern: `admin-[tên-trang].css` và import vào view tương ứng:

```html
<!-- Trong View cụ thể -->
@section Styles {
<link rel="stylesheet" href="~/css/admin-danh-muc.css" />
}
```
