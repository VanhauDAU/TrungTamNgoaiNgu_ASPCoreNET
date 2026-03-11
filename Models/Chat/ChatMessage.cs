using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrungTamNgoaiNgu.Models;

/// <summary>
/// Tin nhắn trong phòng chat
/// </summary>
[Table("chat_messages")]
public class ChatMessage
{
    [Key]
    [Column("chatMessageId")]
    public long ChatMessageId { get; set; }

    [Column("chatRoomId")]
    public long ChatRoomId { get; set; }

    [Column("nguoiGuiId")]
    public int NguoiGuiId { get; set; }

    /// <summary>Trả lời tin nhắn nào</summary>
    [Column("replyToMessageId")]
    public long? ReplyToMessageId { get; set; }

    /// <summary>text | image | file | location | system</summary>
    [Required]
    [MaxLength(20)]
    [Column("loai")]
    public string Loai { get; set; } = "text";

    [Column("noiDung")]
    public string? NoiDung { get; set; }

    [Column("metaJson")]
    public string? MetaJson { get; set; }

    [Column("guiLuc")]
    public DateTime GuiLuc { get; set; } = DateTime.UtcNow;

    [Column("deadlineThuHoi")]
    public DateTime? DeadlineThuHoi { get; set; }

    [Column("thuHoiLuc")]
    public DateTime? ThuHoiLuc { get; set; }

    [Column("xoaLuc")]
    public DateTime? XoaLuc { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    // --- Navigation ---
    public ChatRoom ChatRoom { get; set; } = null!;
    public TaiKhoan NguoiGui { get; set; } = null!;
    public ChatMessage? ReplyToMessage { get; set; }
    public ICollection<ChatMessage> Replies { get; set; } = new List<ChatMessage>();
    public ICollection<ChatMessageReaction> Reactions { get; set; } = new List<ChatMessageReaction>();
    public ICollection<ChatMessageAttachment> Attachments { get; set; } = new List<ChatMessageAttachment>();
    public ICollection<ChatMessageDelete> Deletes { get; set; } = new List<ChatMessageDelete>();
}
