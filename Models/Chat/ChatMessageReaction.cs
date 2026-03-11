using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrungTamNgoaiNgu.Models;

/// <summary>
/// Cảm xúc (emoji) trên tin nhắn
/// </summary>
[Table("chat_message_reactions")]
public class ChatMessageReaction
{
    [Key]
    [Column("chatReactionId")]
    public long ChatReactionId { get; set; }

    [Column("chatMessageId")]
    public long ChatMessageId { get; set; }

    [Column("taiKhoanId")]
    public int TaiKhoanId { get; set; }

    /// <summary>Ký tự emoji (❤️, 😂, 👍...)</summary>
    [Required]
    [MaxLength(50)]
    [Column("emoji")]
    public string Emoji { get; set; } = null!;

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    // --- Navigation ---
    public ChatMessage ChatMessage { get; set; } = null!;
    public TaiKhoan TaiKhoan { get; set; } = null!;
}
