using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrungTamNgoaiNgu.Models;

/// <summary>
/// Tệp đính kèm trong tin nhắn chat
/// </summary>
[Table("chat_message_attachments")]
public class ChatMessageAttachment
{
    [Key]
    [Column("chatAttachmentId")]
    public long ChatAttachmentId { get; set; }

    [Column("chatMessageId")]
    public long ChatMessageId { get; set; }

    [MaxLength(50)]
    [Column("disk")]
    public string Disk { get; set; } = "public";

    /// <summary>Đường dẫn tương đối trong storage</summary>
    [Required]
    [MaxLength(500)]
    [Column("path")]
    public string Path { get; set; } = null!;

    [MaxLength(500)]
    [Column("thumbnailPath")]
    public string? ThumbnailPath { get; set; }

    /// <summary>Tên file gốc của người dùng</summary>
    [Required]
    [MaxLength(255)]
    [Column("tenGoc")]
    public string TenGoc { get; set; } = null!;

    /// <summary>MIME type</summary>
    [MaxLength(100)]
    [Column("mime")]
    public string? Mime { get; set; }

    /// <summary>Kích thước byte</summary>
    [Column("size")]
    public long Size { get; set; } = 0;

    [Column("width")]
    public int? Width { get; set; }

    [Column("height")]
    public int? Height { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    // --- Navigation ---
    public ChatMessage ChatMessage { get; set; } = null!;
}
