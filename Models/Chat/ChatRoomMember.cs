using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrungTamNgoaiNgu.Models;

/// <summary>
/// Thành viên trong phòng chat
/// </summary>
[Table("chat_room_members")]
public class ChatRoomMember
{
    [Key]
    [Column("chatRoomMemberId")]
    public long ChatRoomMemberId { get; set; }

    [Column("chatRoomId")]
    public long ChatRoomId { get; set; }

    [Column("taiKhoanId")]
    public int TaiKhoanId { get; set; }

    /// <summary>member | teacher | owner</summary>
    [Required]
    [MaxLength(20)]
    [Column("vaiTro")]
    public string VaiTro { get; set; } = "member";

    [Column("joinedAt")]
    public DateTime? JoinedAt { get; set; }

    [Column("joinedByPasswordAt")]
    public DateTime? JoinedByPasswordAt { get; set; }

    [Column("lastReadMessageId")]
    public long? LastReadMessageId { get; set; }

    [Column("lastSeenAt")]
    public DateTime? LastSeenAt { get; set; }

    [Column("isMuted")]
    public bool IsMuted { get; set; } = false;

    [Column("roiAt")]
    public DateTime? RoiAt { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    // --- Navigation ---
    public ChatRoom ChatRoom { get; set; } = null!;
    public TaiKhoan TaiKhoan { get; set; } = null!;
}
