using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrungTamNgoaiNgu.Models;

/// <summary>
/// Nhật ký thao tác trong chat (gửi, thu hồi, cảm xúc...)
/// </summary>
[Table("chat_audit_logs")]
public class ChatAuditLog
{
    [Key]
    [Column("chatAuditLogId")]
    public long ChatAuditLogId { get; set; }

    [Column("chatRoomId")]
    public long? ChatRoomId { get; set; }

    [Column("chatMessageId")]
    public long? ChatMessageId { get; set; }

    [Column("taiKhoanId")]
    public int? TaiKhoanId { get; set; }

    /// <summary>message.sent | message.recalled | message.reaction_added | message.reaction_removed</summary>
    [Required]
    [MaxLength(80)]
    [Column("hanhDong")]
    public string HanhDong { get; set; } = null!;

    [Column("duLieuCu")]
    public string? DuLieuCu { get; set; }

    [Column("duLieuMoi")]
    public string? DuLieuMoi { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // --- Navigation ---
    public ChatRoom? ChatRoom { get; set; }
    public TaiKhoan? TaiKhoan { get; set; }
}
