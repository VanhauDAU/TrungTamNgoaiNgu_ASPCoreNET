using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrungTamNgoaiNgu.Models;

/// <summary>
/// Xóa tin nhắn phía cá nhân (chỉ ẩn với người xóa)
/// </summary>
[Table("chat_message_deletes")]
public class ChatMessageDelete
{
    [Key]
    [Column("chatMessageDeleteId")]
    public long ChatMessageDeleteId { get; set; }

    [Column("chatMessageId")]
    public long ChatMessageId { get; set; }

    [Column("taiKhoanId")]
    public int TaiKhoanId { get; set; }

    [Column("deletedAt")]
    public DateTime DeletedAt { get; set; } = DateTime.UtcNow;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // --- Navigation ---
    public ChatMessage ChatMessage { get; set; } = null!;
    public TaiKhoan TaiKhoan { get; set; } = null!;
}
