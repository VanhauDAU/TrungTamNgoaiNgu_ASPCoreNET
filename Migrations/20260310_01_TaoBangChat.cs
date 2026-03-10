using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrungTamNgoaiNgu.Migrations
{
    /// <summary>
    /// Migration tạo nhóm bảng Chat (phòng chat, tin nhắn, thành viên, reactions, tệp đính kèm, xóa tin, audit log)
    /// Dựa trên schema SQL: chat_rooms, chat_messages, chat_room_members,
    /// chat_message_reactions, chat_message_attachments, chat_message_deletes, chat_audit_logs
    /// </summary>
    public partial class TaoBangChat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // =============================================
            // Bảng chat_rooms: Phòng chat (lớp học hoặc nhắn tin trực tiếp)
            // Loại: class_group (nhóm lớp) | direct (1-1)
            // =============================================
            migrationBuilder.CreateTable(
                name: "chat_rooms",
                columns: table => new
                {
                    chatRoomId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    loai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false,
                        defaultValue: "direct",
                        comment: "class_group = nhóm lớp học | direct = nhắn tin 1-1"),
                    tenPhong = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    lopHocId = table.Column<int>(type: "int", nullable: true,
                        comment: "FK -> lophoc.lopHocId (chỉ có khi loai = class_group)"),
                    matKhauHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true,
                        comment: "Hash mật khẩu phòng (nếu phòng có yêu cầu mật khẩu)"),
                    taoBoiId = table.Column<int>(type: "int", nullable: true,
                        comment: "FK -> taikhoan.taiKhoanId - Người tạo phòng"),
                    lastMessageId = table.Column<long>(type: "bigint", nullable: true,
                        comment: "FK -> chat_messages.chatMessageId - Tin nhắn mới nhất"),
                    trangThai = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)1,
                        comment: "0=Inactive, 1=Active, 2=Archived"),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chat_rooms", x => x.chatRoomId);
                    table.ForeignKey(
                        name: "FK_chat_rooms_lophoc_lopHocId",
                        column: x => x.lopHocId,
                        principalTable: "lophoc",
                        principalColumn: "lopHocId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_chat_rooms_taikhoan_taoBoiId",
                        column: x => x.taoBoiId,
                        principalTable: "taikhoan",
                        principalColumn: "taiKhoanId",
                        onDelete: ReferentialAction.SetNull);
                });

            // =============================================
            // Bảng chat_messages: Tin nhắn trong phòng chat
            // =============================================
            migrationBuilder.CreateTable(
                name: "chat_messages",
                columns: table => new
                {
                    chatMessageId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    chatRoomId = table.Column<long>(type: "bigint", nullable: false,
                        comment: "FK -> chat_rooms.chatRoomId"),
                    nguoiGuiId = table.Column<int>(type: "int", nullable: false,
                        comment: "FK -> taikhoan.taiKhoanId"),
                    replyToMessageId = table.Column<long>(type: "bigint", nullable: true,
                        comment: "FK -> chat_messages.chatMessageId - Trả lời tin nhắn nào"),
                    loai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false,
                        defaultValue: "text",
                        comment: "text | image | file | location | system"),
                    noiDung = table.Column<string>(type: "nvarchar(max)", nullable: true,
                        comment: "Nội dung văn bản hoặc null nếu là file/ảnh"),
                    metaJson = table.Column<string>(type: "nvarchar(max)", nullable: true,
                        comment: "Metadata dạng JSON (tên file, kích thước, tọa độ...)"),
                    guiLuc = table.Column<DateTime>(type: "datetime2", nullable: false,
                        comment: "Thời điểm gửi tin"),
                    deadlineThuHoi = table.Column<DateTime>(type: "datetime2", nullable: true,
                        comment: "Thời hạn có thể thu hồi (thường = guiLuc + 24h)"),
                    thuHoiLuc = table.Column<DateTime>(type: "datetime2", nullable: true,
                        comment: "Thời điểm thu hồi (null = chưa thu hồi)"),
                    xoaLuc = table.Column<DateTime>(type: "datetime2", nullable: true,
                        comment: "Thời điểm xóa (soft delete toàn phòng)"),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chat_messages", x => x.chatMessageId);
                    table.ForeignKey(
                        name: "FK_chat_messages_chat_rooms_chatRoomId",
                        column: x => x.chatRoomId,
                        principalTable: "chat_rooms",
                        principalColumn: "chatRoomId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_chat_messages_taikhoan_nguoiGuiId",
                        column: x => x.nguoiGuiId,
                        principalTable: "taikhoan",
                        principalColumn: "taiKhoanId");
                    table.ForeignKey(
                        name: "FK_chat_messages_chat_messages_replyToMessageId",
                        column: x => x.replyToMessageId,
                        principalTable: "chat_messages",
                        principalColumn: "chatMessageId",
                        onDelete: ReferentialAction.SetNull);
                });

            // =============================================
            // Bảng chat_room_members: Thành viên trong phòng chat
            // =============================================
            migrationBuilder.CreateTable(
                name: "chat_room_members",
                columns: table => new
                {
                    chatRoomMemberId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    chatRoomId = table.Column<long>(type: "bigint", nullable: false),
                    taiKhoanId = table.Column<int>(type: "int", nullable: false),
                    vaiTro = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false,
                        defaultValue: "member",
                        comment: "member | teacher | owner"),
                    joinedAt = table.Column<DateTime>(type: "datetime2", nullable: true,
                        comment: "Thời điểm tham gia phòng"),
                    joinedByPasswordAt = table.Column<DateTime>(type: "datetime2", nullable: true,
                        comment: "Thời điểm nhập mật khẩu để vào phòng"),
                    lastReadMessageId = table.Column<long>(type: "bigint", nullable: true,
                        comment: "ID tin nhắn cuối đã đọc -> tính số tin chưa đọc"),
                    lastSeenAt = table.Column<DateTime>(type: "datetime2", nullable: true,
                        comment: "Lần cuối online trong phòng"),
                    isMuted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false,
                        comment: "Tắt thông báo phòng này"),
                    roiAt = table.Column<DateTime>(type: "datetime2", nullable: true,
                        comment: "Thời điểm rời phòng (null = còn trong phòng)"),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chat_room_members", x => x.chatRoomMemberId);
                    table.UniqueConstraint("UQ_chat_room_members_room_user", x => new { x.chatRoomId, x.taiKhoanId });
                    table.ForeignKey(
                        name: "FK_chat_room_members_chat_rooms_chatRoomId",
                        column: x => x.chatRoomId,
                        principalTable: "chat_rooms",
                        principalColumn: "chatRoomId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_chat_room_members_taikhoan_taiKhoanId",
                        column: x => x.taiKhoanId,
                        principalTable: "taikhoan",
                        principalColumn: "taiKhoanId",
                        onDelete: ReferentialAction.Cascade);
                });

            // =============================================
            // Bảng chat_message_reactions: Cảm xúc (emoji) trên tin nhắn
            // =============================================
            migrationBuilder.CreateTable(
                name: "chat_message_reactions",
                columns: table => new
                {
                    chatReactionId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    chatMessageId = table.Column<long>(type: "bigint", nullable: false),
                    taiKhoanId = table.Column<int>(type: "int", nullable: false),
                    emoji = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false,
                        comment: "Ký tự emoji (❤️, 😂, 👍...)"),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chat_message_reactions", x => x.chatReactionId);
                    table.UniqueConstraint("UQ_chat_message_reactions_msg_user_emoji",
                        x => new { x.chatMessageId, x.taiKhoanId, x.emoji });
                    table.ForeignKey(
                        name: "FK_chat_message_reactions_chat_messages_chatMessageId",
                        column: x => x.chatMessageId,
                        principalTable: "chat_messages",
                        principalColumn: "chatMessageId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_chat_message_reactions_taikhoan_taiKhoanId",
                        column: x => x.taiKhoanId,
                        principalTable: "taikhoan",
                        principalColumn: "taiKhoanId",
                        onDelete: ReferentialAction.Cascade);
                });

            // =============================================
            // Bảng chat_message_attachments: Tệp đính kèm trong tin nhắn
            // =============================================
            migrationBuilder.CreateTable(
                name: "chat_message_attachments",
                columns: table => new
                {
                    chatAttachmentId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    chatMessageId = table.Column<long>(type: "bigint", nullable: false),
                    disk = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false,
                        defaultValue: "public"),
                    path = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false,
                        comment: "Đường dẫn tương đối trong storage"),
                    thumbnailPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true,
                        comment: "Đường dẫn thumbnail (nếu là ảnh/video)"),
                    tenGoc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false,
                        comment: "Tên file gốc của người dùng"),
                    mime = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true,
                        comment: "MIME type (image/jpeg, application/pdf...)"),
                    size = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L,
                        comment: "Kích thước byte"),
                    width = table.Column<int>(type: "int", nullable: true,
                        comment: "Chiều rộng ảnh/video (pixel)"),
                    height = table.Column<int>(type: "int", nullable: true,
                        comment: "Chiều cao ảnh/video (pixel)"),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chat_message_attachments", x => x.chatAttachmentId);
                    table.ForeignKey(
                        name: "FK_chat_message_attachments_chat_messages_chatMessageId",
                        column: x => x.chatMessageId,
                        principalTable: "chat_messages",
                        principalColumn: "chatMessageId",
                        onDelete: ReferentialAction.Cascade);
                });

            // =============================================
            // Bảng chat_message_deletes: Xóa tin nhắn phía cá nhân (chỉ ẩn với người xóa)
            // =============================================
            migrationBuilder.CreateTable(
                name: "chat_message_deletes",
                columns: table => new
                {
                    chatMessageDeleteId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    chatMessageId = table.Column<long>(type: "bigint", nullable: false),
                    taiKhoanId = table.Column<int>(type: "int", nullable: false),
                    deletedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chat_message_deletes", x => x.chatMessageDeleteId);
                    table.UniqueConstraint("UQ_chat_message_deletes_msg_user",
                        x => new { x.chatMessageId, x.taiKhoanId });
                    table.ForeignKey(
                        name: "FK_chat_message_deletes_chat_messages_chatMessageId",
                        column: x => x.chatMessageId,
                        principalTable: "chat_messages",
                        principalColumn: "chatMessageId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_chat_message_deletes_taikhoan_taiKhoanId",
                        column: x => x.taiKhoanId,
                        principalTable: "taikhoan",
                        principalColumn: "taiKhoanId",
                        onDelete: ReferentialAction.Cascade);
                });

            // =============================================
            // Bảng chat_audit_logs: Nhật ký thao tác trong chat (gửi, thu hồi, cảm xúc...)
            // =============================================
            migrationBuilder.CreateTable(
                name: "chat_audit_logs",
                columns: table => new
                {
                    chatAuditLogId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    chatRoomId = table.Column<long>(type: "bigint", nullable: true),
                    chatMessageId = table.Column<long>(type: "bigint", nullable: true),
                    taiKhoanId = table.Column<int>(type: "int", nullable: true),
                    hanhDong = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false,
                        comment: "message.sent | message.recalled | message.reaction_added | message.reaction_removed"),
                    duLieuCu = table.Column<string>(type: "nvarchar(max)", nullable: true,
                        comment: "Dữ liệu JSON trước khi thay đổi"),
                    duLieuMoi = table.Column<string>(type: "nvarchar(max)", nullable: true,
                        comment: "Dữ liệu JSON sau khi thay đổi"),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chat_audit_logs", x => x.chatAuditLogId);
                    table.ForeignKey(
                        name: "FK_chat_audit_logs_chat_rooms_chatRoomId",
                        column: x => x.chatRoomId,
                        principalTable: "chat_rooms",
                        principalColumn: "chatRoomId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_chat_audit_logs_taikhoan_taiKhoanId",
                        column: x => x.taiKhoanId,
                        principalTable: "taikhoan",
                        principalColumn: "taiKhoanId",
                        onDelete: ReferentialAction.SetNull);
                });

            // === Indexes ===
            migrationBuilder.CreateIndex("IX_chat_rooms_lopHocId", "chat_rooms", "lopHocId", unique: true);
            migrationBuilder.CreateIndex("IX_chat_rooms_loai_trangThai", "chat_rooms", new[] { "loai", "trangThai" });
            migrationBuilder.CreateIndex("IX_chat_messages_chatRoomId", "chat_messages", "chatRoomId");
            migrationBuilder.CreateIndex("IX_chat_messages_nguoiGuiId", "chat_messages", "nguoiGuiId");
            migrationBuilder.CreateIndex("IX_chat_messages_guiLuc", "chat_messages", "guiLuc");
            migrationBuilder.CreateIndex("IX_chat_room_members_taiKhoanId", "chat_room_members", "taiKhoanId");
            migrationBuilder.CreateIndex("IX_chat_message_reactions_taiKhoanId", "chat_message_reactions", "taiKhoanId");
            migrationBuilder.CreateIndex("IX_chat_message_attachments_chatMessageId", "chat_message_attachments", "chatMessageId");
            migrationBuilder.CreateIndex("IX_chat_audit_logs_chatRoomId", "chat_audit_logs", "chatRoomId");
            migrationBuilder.CreateIndex("IX_chat_audit_logs_taiKhoanId", "chat_audit_logs", "taiKhoanId");
            migrationBuilder.CreateIndex("IX_chat_audit_logs_hanhDong", "chat_audit_logs", "hanhDong");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("chat_audit_logs");
            migrationBuilder.DropTable("chat_message_deletes");
            migrationBuilder.DropTable("chat_message_attachments");
            migrationBuilder.DropTable("chat_message_reactions");
            migrationBuilder.DropTable("chat_room_members");
            migrationBuilder.DropTable("chat_messages");
            migrationBuilder.DropTable("chat_rooms");
        }
    }
}
