// =============================================================================
// TỈNH THÀNH SEEDER — 34 đơn vị hành chính sau sáp nhập 2025
// Dữ liệu từ: https://provinces.open-api.vn/api/p/ (v2)
// MaAPI = code field trong API response
// =============================================================================

using Microsoft.EntityFrameworkCore;
using TrungTamNgoaiNgu.Models;

namespace TrungTamNgoaiNgu.Data;

public static class TinhThanhSeeder
{
    /// <summary>
    /// Seed 34 tỉnh/thành phố (sau sáp nhập 2025).
    /// Bỏ qua nếu bảng đã có dữ liệu.
    /// </summary>
    public static async Task SeedAsync(AppDbContext db)
    {
        if (await db.TinhThanhs.AnyAsync()) return;

        var tinhThanhs = new List<TinhThanh>
        {
            new() { MaAPI = 1,  TenTinhThanh = "Thành phố Hà Nội",      Slug = "ha-noi",      DivisionType = "thành phố trung ương", Codename = "ha_noi"     },
            new() { MaAPI = 4,  TenTinhThanh = "Tỉnh Cao Bằng",         Slug = "cao-bang",    DivisionType = "tỉnh",                 Codename = "cao_bang"   },
            new() { MaAPI = 8,  TenTinhThanh = "Tỉnh Tuyên Quang",      Slug = "tuyen-quang", DivisionType = "tỉnh",                 Codename = "tuyen_quang"},
            new() { MaAPI = 11, TenTinhThanh = "Tỉnh Điện Biên",        Slug = "dien-bien",   DivisionType = "tỉnh",                 Codename = "dien_bien"  },
            new() { MaAPI = 12, TenTinhThanh = "Tỉnh Lai Châu",         Slug = "lai-chau",    DivisionType = "tỉnh",                 Codename = "lai_chau"   },
            new() { MaAPI = 14, TenTinhThanh = "Tỉnh Sơn La",           Slug = "son-la",      DivisionType = "tỉnh",                 Codename = "son_la"     },
            new() { MaAPI = 15, TenTinhThanh = "Tỉnh Lào Cai",          Slug = "lao-cai",     DivisionType = "tỉnh",                 Codename = "lao_cai"    },
            new() { MaAPI = 19, TenTinhThanh = "Tỉnh Thái Nguyên",      Slug = "thai-nguyen", DivisionType = "tỉnh",                 Codename = "thai_nguyen"},
            new() { MaAPI = 20, TenTinhThanh = "Tỉnh Lạng Sơn",         Slug = "lang-son",    DivisionType = "tỉnh",                 Codename = "lang_son"   },
            new() { MaAPI = 22, TenTinhThanh = "Tỉnh Quảng Ninh",       Slug = "quang-ninh",  DivisionType = "tỉnh",                 Codename = "quang_ninh" },
            new() { MaAPI = 24, TenTinhThanh = "Tỉnh Bắc Ninh",         Slug = "bac-ninh",    DivisionType = "tỉnh",                 Codename = "bac_ninh"   },
            new() { MaAPI = 25, TenTinhThanh = "Tỉnh Phú Thọ",          Slug = "phu-tho",     DivisionType = "tỉnh",                 Codename = "phu_tho"    },
            new() { MaAPI = 31, TenTinhThanh = "Thành phố Hải Phòng",   Slug = "hai-phong",   DivisionType = "thành phố trung ương", Codename = "hai_phong"  },
            new() { MaAPI = 33, TenTinhThanh = "Tỉnh Hưng Yên",         Slug = "hung-yen",    DivisionType = "tỉnh",                 Codename = "hung_yen"   },
            new() { MaAPI = 37, TenTinhThanh = "Tỉnh Ninh Bình",        Slug = "ninh-binh",   DivisionType = "tỉnh",                 Codename = "ninh_binh"  },
            new() { MaAPI = 38, TenTinhThanh = "Tỉnh Thanh Hóa",        Slug = "thanh-hoa",   DivisionType = "tỉnh",                 Codename = "thanh_hoa"  },
            new() { MaAPI = 40, TenTinhThanh = "Tỉnh Nghệ An",          Slug = "nghe-an",     DivisionType = "tỉnh",                 Codename = "nghe_an"    },
            new() { MaAPI = 42, TenTinhThanh = "Tỉnh Hà Tĩnh",          Slug = "ha-tinh",     DivisionType = "tỉnh",                 Codename = "ha_tinh"    },
            new() { MaAPI = 44, TenTinhThanh = "Tỉnh Quảng Trị",        Slug = "quang-tri",   DivisionType = "tỉnh",                 Codename = "quang_tri"  },
            new() { MaAPI = 46, TenTinhThanh = "Thành phố Huế",         Slug = "hue",         DivisionType = "thành phố trung ương", Codename = "hue"        },
            new() { MaAPI = 48, TenTinhThanh = "Thành phố Đà Nẵng",     Slug = "da-nang",     DivisionType = "thành phố trung ương", Codename = "da_nang"    },
            new() { MaAPI = 51, TenTinhThanh = "Tỉnh Quảng Ngãi",       Slug = "quang-ngai",  DivisionType = "tỉnh",                 Codename = "quang_ngai" },
            new() { MaAPI = 52, TenTinhThanh = "Tỉnh Gia Lai",          Slug = "gia-lai",     DivisionType = "tỉnh",                 Codename = "gia_lai"    },
            new() { MaAPI = 56, TenTinhThanh = "Tỉnh Khánh Hòa",        Slug = "khanh-hoa",   DivisionType = "tỉnh",                 Codename = "khanh_hoa"  },
            new() { MaAPI = 66, TenTinhThanh = "Tỉnh Đắk Lắk",         Slug = "dak-lak",     DivisionType = "tỉnh",                 Codename = "dak_lak"    },
            new() { MaAPI = 68, TenTinhThanh = "Tỉnh Lâm Đồng",         Slug = "lam-dong",    DivisionType = "tỉnh",                 Codename = "lam_dong"   },
            new() { MaAPI = 75, TenTinhThanh = "Tỉnh Đồng Nai",         Slug = "dong-nai",    DivisionType = "tỉnh",                 Codename = "dong_nai"   },
            new() { MaAPI = 79, TenTinhThanh = "Thành phố Hồ Chí Minh", Slug = "ho-chi-minh", DivisionType = "thành phố trung ương", Codename = "ho_chi_minh"},
            new() { MaAPI = 80, TenTinhThanh = "Tỉnh Tây Ninh",         Slug = "tay-ninh",    DivisionType = "tỉnh",                 Codename = "tay_ninh"   },
            new() { MaAPI = 82, TenTinhThanh = "Tỉnh Đồng Tháp",        Slug = "dong-thap",   DivisionType = "tỉnh",                 Codename = "dong_thap"  },
            new() { MaAPI = 86, TenTinhThanh = "Tỉnh Vĩnh Long",         Slug = "vinh-long",   DivisionType = "tỉnh",                 Codename = "vinh_long"  },
            new() { MaAPI = 91, TenTinhThanh = "Tỉnh An Giang",          Slug = "an-giang",    DivisionType = "tỉnh",                 Codename = "an_giang"   },
            new() { MaAPI = 92, TenTinhThanh = "Thành phố Cần Thơ",     Slug = "can-tho",     DivisionType = "thành phố trung ương", Codename = "can_tho"    },
            new() { MaAPI = 96, TenTinhThanh = "Tỉnh Cà Mau",           Slug = "ca-mau",      DivisionType = "tỉnh",                 Codename = "ca_mau"     },
        };

        await db.TinhThanhs.AddRangeAsync(tinhThanhs);
        await db.SaveChangesAsync();
    }
}
