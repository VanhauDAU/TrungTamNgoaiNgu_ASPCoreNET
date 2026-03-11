using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrungTamNgoaiNgu.Migrations
{
    /// <inheritdoc />
    public partial class AddChatTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "chat_rooms",
                columns: table => new
                {
                    chatRoomId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    loai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    tenPhong = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    lopHocId = table.Column<int>(type: "int", nullable: true),
                    matKhauHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    taoBoiId = table.Column<int>(type: "int", nullable: true),
                    lastMessageId = table.Column<long>(type: "bigint", nullable: true),
                    trangThai = table.Column<byte>(type: "tinyint", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "chat_audit_logs",
                columns: table => new
                {
                    chatAuditLogId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    chatRoomId = table.Column<long>(type: "bigint", nullable: true),
                    chatMessageId = table.Column<long>(type: "bigint", nullable: true),
                    taiKhoanId = table.Column<int>(type: "int", nullable: true),
                    hanhDong = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    duLieuCu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    duLieuMoi = table.Column<string>(type: "nvarchar(max)", nullable: true),
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

            migrationBuilder.CreateTable(
                name: "chat_messages",
                columns: table => new
                {
                    chatMessageId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    chatRoomId = table.Column<long>(type: "bigint", nullable: false),
                    nguoiGuiId = table.Column<int>(type: "int", nullable: false),
                    replyToMessageId = table.Column<long>(type: "bigint", nullable: true),
                    loai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    noiDung = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    metaJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    guiLuc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    deadlineThuHoi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    thuHoiLuc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    xoaLuc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chat_messages", x => x.chatMessageId);
                    table.ForeignKey(
                        name: "FK_chat_messages_chat_messages_replyToMessageId",
                        column: x => x.replyToMessageId,
                        principalTable: "chat_messages",
                        principalColumn: "chatMessageId");
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
                });

            migrationBuilder.CreateTable(
                name: "chat_room_members",
                columns: table => new
                {
                    chatRoomMemberId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    chatRoomId = table.Column<long>(type: "bigint", nullable: false),
                    taiKhoanId = table.Column<int>(type: "int", nullable: false),
                    vaiTro = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    joinedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    joinedByPasswordAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    lastReadMessageId = table.Column<long>(type: "bigint", nullable: true),
                    lastSeenAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    isMuted = table.Column<bool>(type: "bit", nullable: false),
                    roiAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chat_room_members", x => x.chatRoomMemberId);
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

            migrationBuilder.CreateTable(
                name: "chat_message_attachments",
                columns: table => new
                {
                    chatAttachmentId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    chatMessageId = table.Column<long>(type: "bigint", nullable: false),
                    disk = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    path = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    thumbnailPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    tenGoc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    mime = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    width = table.Column<int>(type: "int", nullable: true),
                    height = table.Column<int>(type: "int", nullable: true),
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

            migrationBuilder.CreateTable(
                name: "chat_message_reactions",
                columns: table => new
                {
                    chatReactionId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    chatMessageId = table.Column<long>(type: "bigint", nullable: false),
                    taiKhoanId = table.Column<int>(type: "int", nullable: false),
                    emoji = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chat_message_reactions", x => x.chatReactionId);
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

            migrationBuilder.CreateIndex(
                name: "IX_chat_audit_logs_chatRoomId",
                table: "chat_audit_logs",
                column: "chatRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_chat_audit_logs_taiKhoanId",
                table: "chat_audit_logs",
                column: "taiKhoanId");

            migrationBuilder.CreateIndex(
                name: "IX_chat_message_attachments_chatMessageId",
                table: "chat_message_attachments",
                column: "chatMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_chat_message_deletes_taiKhoanId",
                table: "chat_message_deletes",
                column: "taiKhoanId");

            migrationBuilder.CreateIndex(
                name: "UQ_chat_message_deletes_msg_user",
                table: "chat_message_deletes",
                columns: new[] { "chatMessageId", "taiKhoanId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_chat_message_reactions_taiKhoanId",
                table: "chat_message_reactions",
                column: "taiKhoanId");

            migrationBuilder.CreateIndex(
                name: "UQ_chat_message_reactions_msg_user_emoji",
                table: "chat_message_reactions",
                columns: new[] { "chatMessageId", "taiKhoanId", "emoji" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_chat_messages_chatRoomId",
                table: "chat_messages",
                column: "chatRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_chat_messages_nguoiGuiId",
                table: "chat_messages",
                column: "nguoiGuiId");

            migrationBuilder.CreateIndex(
                name: "IX_chat_messages_replyToMessageId",
                table: "chat_messages",
                column: "replyToMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_chat_room_members_taiKhoanId",
                table: "chat_room_members",
                column: "taiKhoanId");

            migrationBuilder.CreateIndex(
                name: "UQ_chat_room_members_room_user",
                table: "chat_room_members",
                columns: new[] { "chatRoomId", "taiKhoanId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_chat_rooms_lopHocId",
                table: "chat_rooms",
                column: "lopHocId");

            migrationBuilder.CreateIndex(
                name: "IX_chat_rooms_taoBoiId",
                table: "chat_rooms",
                column: "taoBoiId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "chat_audit_logs");

            migrationBuilder.DropTable(
                name: "chat_message_attachments");

            migrationBuilder.DropTable(
                name: "chat_message_deletes");

            migrationBuilder.DropTable(
                name: "chat_message_reactions");

            migrationBuilder.DropTable(
                name: "chat_room_members");

            migrationBuilder.DropTable(
                name: "chat_messages");

            migrationBuilder.DropTable(
                name: "chat_rooms");
        }
    }
}
