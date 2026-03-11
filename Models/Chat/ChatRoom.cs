using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrungTamNgoaiNgu.Models;

/// <summary>
/// Phòng chat: class_group (nhóm lớp) hoặc direct (1-1)
/// </summary>
[Table("chat_rooms")]
public class ChatRoom
{
    [Key]
    [Column("chatRoomId")]
    public long ChatRoomId { get; set; }

    /// <summary>class_group | direct</summary>
    [Required]
    [MaxLength(20)]
    [Column("loai")]
    public string Loai { get; set; } = "direct";

    [MaxLength(150)]
    [Column("tenPhong")]
    public string? TenPhong { get; set; }

    /// <summary>FK -> LopHoc (chỉ khi loai = class_group)</summary>
    [Column("lopHocId")]
    public int? LopHocId { get; set; }

    [MaxLength(255)]
    [Column("matKhauHash")]
    public string? MatKhauHash { get; set; }

    /// <summary>FK -> TaiKhoan - người tạo phòng</summary>
    [Column("taoBoiId")]
    public int? TaoBoiId { get; set; }

    /// <summary>FK -> ChatMessage - tin nhắn mới nhất</summary>
    [Column("lastMessageId")]
    public long? LastMessageId { get; set; }

    /// <summary>0=Inactive, 1=Active, 2=Archived</summary>
    [Column("trangThai")]
    public byte TrangThai { get; set; } = 1;

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    // --- Navigation ---
    public LopHoc? LopHoc { get; set; }
    public TaiKhoan? TaoBoiTaiKhoan { get; set; }
    public ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
    public ICollection<ChatRoomMember> Members { get; set; } = new List<ChatRoomMember>();
    public ICollection<ChatAuditLog> AuditLogs { get; set; } = new List<ChatAuditLog>();
}
