-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Máy chủ: 127.0.0.1:3307
-- Thời gian đã tạo: Th3 01, 2026 lúc 10:33 AM
-- Phiên bản máy phục vụ: 11.5.2-MariaDB
-- Phiên bản PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Cơ sở dữ liệu: `doanchuyennganhcnpm_trungtamnn`
--

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `baithi`
--

CREATE TABLE `baithi` (
  `baiThiId` int(11) NOT NULL,
  `khoaHocId` int(11) DEFAULT NULL,
  `tenBaiThi` varchar(255) DEFAULT NULL,
  `moTa` text DEFAULT NULL,
  `ngayThi` date DEFAULT NULL,
  `created_at` datetime DEFAULT current_timestamp(),
  `updated_at` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `baiviet`
--

CREATE TABLE `baiviet` (
  `baiVietId` int(11) NOT NULL,
  `tieuDe` varchar(255) NOT NULL,
  `slug` varchar(255) NOT NULL,
  `tomTat` text DEFAULT NULL,
  `noiDung` longtext NOT NULL,
  `anhDaiDien` varchar(255) DEFAULT NULL,
  `taiKhoanId` int(11) NOT NULL,
  `luotXem` int(11) DEFAULT 0,
  `trangThai` tinyint(4) DEFAULT 0 COMMENT '0: Ẩn, 1: Công khai',
  `published_at` datetime DEFAULT NULL,
  `deleted_at` timestamp NULL DEFAULT NULL,
  `created_at` timestamp NULL DEFAULT current_timestamp(),
  `updated_at` timestamp NULL DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `baiviet`
--

INSERT INTO `baiviet` (`baiVietId`, `tieuDe`, `slug`, `tomTat`, `noiDung`, `anhDaiDien`, `taiKhoanId`, `luotXem`, `trangThai`, `published_at`, `deleted_at`, `created_at`, `updated_at`) VALUES
(3, 'Trải nghiệm học viên sau khóa IELTS tại trung tâm', 'trai-nghiem-hoc-vien-ielts', 'Chia sẻ cảm nhận của học viên sau khi hoàn thành khóa học.', '<p><strong>Nội dung b&agrave;i viết 01</strong></p>\r\n<p><strong><img src=\"http://127.0.0.1:8000/storage/bai-viet/content/6tWKYu9CQlpoUao9KpCMmpx759mF17sJmSwBNzwZ.jpg\" alt=\"\" width=\"251\" height=\"98\"></strong></p>\r\n<p style=\"text-align: center;\"><strong>Xin ch&agrave;o</strong></p>\r\n<p style=\"text-align: left;\"><span style=\"background-color: rgb(0, 0, 0); color: rgb(236, 240, 241);\"><strong><span style=\"background-color: rgb(186, 55, 42);\">Nội dung 01</span></strong></span></p>\r\n<p style=\"text-align: left;\">&nbsp;</p>\r\n<p style=\"text-align: left;\"><span style=\"background-color: rgb(0, 0, 0); color: rgb(236, 240, 241);\"><strong><span style=\"background-color: rgb(186, 55, 42);\"><a title=\"Bấm v&agrave;o để truy cập\" href=\"http://127.0.0.1:8000/blog/trai-nghiem-hoc-vien-ielts\">http://127.0.0.1:8000/blog/trai-nghiem-hoc-vien-ielts</a></span></strong></span></p>\r\n<p style=\"text-align: left;\"><span style=\"background-color: rgb(0, 0, 0); color: rgb(236, 240, 241);\"><strong><span style=\"background-color: rgb(186, 55, 42);\">Mục lục</span></strong></span></p>\r\n<ul>\r\n<li style=\"text-align: left;\"><span style=\"background-color: rgb(236, 240, 241); color: rgb(52, 73, 94);\"><strong>1.1 Hello</strong></span></li>\r\n<li style=\"text-align: left;\"><span style=\"background-color: rgb(236, 240, 241); color: rgb(52, 73, 94);\"><strong>1.2 xin ch&agrave;o</strong></span></li>\r\n</ul>\r\n<p style=\"text-align: left;\">&nbsp;</p>', 'bai-viet/bz6U6T6vqRYh4cOmFTUjIw23mkoPNFUKXenTEUZc.jpg', 1, 67, 1, '2026-01-27 13:11:43', NULL, '2026-01-27 06:11:43', '2026-02-28 08:41:58'),
(4, 'Bí quyết đạt IELTS 8.0 trong 6 tháng', 'bi-quyet-dat-ielts-8-0-trong-6-thang', 'Hành trình chinh phục IELTS 8.0 không khó như bạn nghĩ nếu có lộ trình và phương pháp đúng đắn.', '<p style=\"text-align: justify;\"><span style=\"font-size: 16px;\">Kỳ thi IELTS không chỉ kiểm tra khả năng sử dụng tiếng Anh mà còn đòi hỏi bạn phải có một chiến lược ôn tập cẩn thận và khoa học.</span></p>\r\n<h2><span style=\"color: rgb(41, 105, 176);\">1. Xác định mục tiêu rõ ràng</span></h2>\r\n<p style=\"text-align: justify;\"><span style=\"font-size: 16px;\">Trước khi bắt đầu, hãy tự hỏi bản thân tại sao bạn cần điểm số này. Động lực thực sự sẽ giúp bạn không bỏ cuộc. Đặc biệt hãy cố gắng phân tích điểm số mục tiêu của từng kỹ năng.</span></p>\r\n<h2><span style=\"color: rgb(41, 105, 176);\">2. Lập kế hoạch học tập chi tiết</span></h2>\r\n<p style=\"text-align: justify;\"><span style=\"font-size: 16px;\">Chia nhỏ mục tiêu lớn thành các bước khả thi theo từng tuần, từng ngày. Ví dụ:</span></p>\r\n<ul>\r\n<li style=\"text-align: justify;\"><span style=\"font-size: 16px;\"><strong>Thứ 2, 4, 6:</strong> Luyện Kỹ năng Nghe và Đọc theo Format đề thi Cambridge.</span></li>\r\n<li style=\"text-align: justify;\"><span style=\"font-size: 16px;\"><strong>Thứ 3, 5, 7:</strong> Luyện Nói (ghi âm lại) và Viết (Task 1 &amp; Task 2).</span></li>\r\n<li style=\"text-align: justify;\"><span style=\"font-size: 16px;\"><strong>Chủ nhật:</strong> Làm 1 đề mock test hoàn chỉnh trong 3 tiếng.</span></li>\r\n</ul>\r\n<blockquote style=\"margin: 0 0 0 40px; border: none; padding: 0px;\">\r\n<p style=\"text-align: center;\"><span style=\"font-size: 18px; color: rgb(184, 49, 47);\"><em>\"Thành công là kết quả của sự chuẩn bị kỹ lưỡng.\"</em></span></p>\r\n</blockquote>\r\n<h2><span style=\"color: rgb(41, 105, 176);\">3. Tìm nguồn tài liệu phù hợp</span></h2>\r\n<p style=\"text-align: justify;\"><span style=\"font-size: 16px;\">Không nên ôm đồm quá nhiều sách. Bạn nên tập trung vào bộ <strong>Cambridge IELTS 10 - 18</strong> để tiếp xúc đề thi thật. Đồng thời sử dụng từ điển <em>Oxford Advanced Learner\'s Dictionary</em> để tra cứu chính xác collocation, context.</span></p>\r\n<p style=\"text-align: justify;\"><span style=\"font-size: 16px;\"><img src=\"https://images.unsplash.com/photo-1456513080510-7bf3a84b82f8?q=80&amp;w=2573&amp;auto=format&amp;fit=crop\" alt=\"\" width=\"100%\"></span></p>\r\n<p style=\"text-align: justify;\"><span style=\"font-size: 16px;\">Cuối cùng, đừng ngại mắc sai lầm. Hãy nhờ thầy cô hoặc những người đi trước (đã đạt band cao) feedback thường xuyên để biết được điểm yếu của mình ở đâu nhé. Chúc các bạn may mắn!</span></p>', 'bai-viet/bz6U6T6vqRYh4cOmFTUjIw23mkoPNFUKXenTEUZc.jpg', 1, 1500, 1, '2026-02-20 08:30:00', NULL, '2026-02-20 01:30:00', '2026-02-20 01:30:00'),
(5, '10 lỗi sai phổ biến khi giao tiếp tiếng Anh', '10-loi-sai-pho-bien-khi-giao-tiep-tieng-anh', 'Những lỗi ngữ pháp và phát âm kinh điển mà người Việt hay mắc phải.', '<h2><span style=\"color: rgb(226, 80, 65);\">1. Bỏ quên âm đuôi (Ending sounds)</span></h2>\r\n<p><span style=\"font-size: 16px;\">Trong tiếng Việt không có khái niệm bật âm đuôi, do đó học viên thường có thói quen bỏ qua các âm như /s/, /t/, /d/, /z/ khi nói tiếng Anh. Điều này dẫn đến sự cố khó hiểu (miscommunication).</span></p>\r\n<p><span style=\"font-size: 16px;\"><strong>Ví dụ:</strong> <em>white</em> (trắng), <em>wife</em> (vợ), và <em>wine</em> (rượu) đều bị người Việt phát âm na ná giống \"oai\".</span></p>\r\n<h2><span style=\"color: rgb(226, 80, 65);\">2. Dịch từ sang từ (Word-by-word translation)</span></h2>\r\n<p><span style=\"font-size: 16px;\">Bạn nghĩ bằng tiếng Việt rồi cố gắng tìm từ tiếng Anh với nghĩa tương đương để lắp ghép lại thành 1 câu. Đây là cách chắc chắn làm câu nói của bạn mất tự nhiên.</span></p>\r\n<ul>\r\n<li><span style=\"font-size: 16px;\"><span style=\"color: rgb(226, 80, 65);\"><strong>Sai:</strong></span> I very like it. (Tôi rất thích nó)</span></li>\r\n<li><span style=\"font-size: 16px;\"><span style=\"color: rgb(41, 105, 176);\"><strong>Đúng:</strong></span> I really like it. / I like it very much.</span></li>\r\n</ul>\r\n<h2><span style=\"color: rgb(226, 80, 65);\">3. Thêm \"s\" vô tội vạ</span></h2>\r\n<p><span style=\"font-size: 16px;\">Trái ngược với lỗi mất âm đuôi, khá nhiều bạn lại thêm /s/ vào cuối bất kỳ từ tiếng Anh nào để cảm thấy giọng mình \"Tây\" hơn và \"trôi chảy\" hơn. Điều này thực sự gây khó chịu cho người bản xứ nghe chuyện.</span></p>\r\n<p><span style=\"font-size: 16px;\"><img src=\"https://plus.unsplash.com/premium_photo-1663045330310-90924ec9d582?q=80&amp;w=2670&amp;auto=format&amp;fit=crop\" alt=\"\" width=\"100%\"></span></p>\r\n<p><span style=\"font-size: 16px;\">Cách khắc phục? Hãy tập trung nghe nhiều hơn là nói trong thời gian đầu để hình thành phản xạ kết nối âm chuẩn.</span></p>', 'bai-viet/demo2.jpg', 1, 851, 1, '2026-02-22 10:15:00', NULL, '2026-02-22 03:15:00', '2026-02-28 09:37:34'),
(6, 'Cách học từ vựng tiếng Anh nhớ lâu', 'cach-hoc-tu-vung-tieng-anh-nho-lau', 'Phương pháp Spaced Repetition (Lặp lại ngắt quãng) giúp bạn nhớ từ vựng vĩnh viễn.', '<h2><span style=\"color: rgb(84, 172, 210);\">Tại sao chúng ta hay quên từ vựng?</span></h2>\r\n<p><span style=\"font-size: 16px;\">Theo đường cong lãng quên của tiến sĩ Hermann Ebbinghaus, não bộ con người có thể quên đi khoảng <strong>70-80%</strong> kiến thức vừa học ở ngày hôm trước nếu như chẳng bao giờ ôn tập lại. Cách truyền thống viết chép từ vựng 10 lần ra nháp hoàn toàn vô dụng vì chỉ dùng đến cơ bắp (Trí nhớ ngắn hạn).</span></p>\r\n<h2><span style=\"color: rgb(84, 172, 210);\">Spaced Repetition là gì?</span></h2>\r\n<p><span style=\"font-size: 16px;\">Nó có nghĩa là \"Kỹ thuật Lặp lại ngắt quãng\". Thay vì đâm đầu ôn một danh sách từ vựng liên tục ngày này qua ngày khác, ta sẽ chủ động tính toán để học lại đúng ngay khoảnh khắc <strong>bộ não sắp sửa quên</strong> từ đó đi.</span></p>\r\n<p><span style=\"font-size: 16px;\"><strong>Các mốc thời gian tối ưu Ebbinghaus khuyên dùng:</strong></span></p>\r\n<ol>\r\n<li><span style=\"font-size: 16px;\">Lần 1: Sau 1 giờ học.</span></li>\r\n<li><span style=\"font-size: 16px;\">Lần 2: Sau 24 giờ.</span></li>\r\n<li><span style=\"font-size: 16px;\">Lần 3: Sau 3 ngày.</span></li>\r\n<li><span style=\"font-size: 16px;\">Lần 4: Sau 1 tuần.</span></li>\r\n<li><span style=\"font-size: 16px;\">Lần 5: Sau 1 tháng.</span></li>\r\n</ol>\r\n<p><span style=\"font-size: 16px;\">Bằng cách này, mọi thông tin về từ vựng (spelling, pronunciation, meaning, context) sẽ được chuyển hóa thành trí nhớ dài hạn.</span></p>\r\n<p><span style=\"font-size: 16px;\">Để tránh phải lập thời gian biểu phức tạp, bạn hãy dùng các app tạo Flashcards chạy thuật toán này sẵn như: <strong>Anki, Quizlet, hay Memrise</strong>.</span></p>', 'bai-viet/demo3.jpg', 1, 1200, 1, '2026-02-25 14:00:00', NULL, '2026-02-25 07:00:00', '2026-02-25 07:00:00'),
(7, 'Phân biệt TOEIC, IELTS và TOEFL', 'phan-biet-toeic-ielts-va-toefl', 'Hướng dẫn chi tiết giúp bạn chọn đúng chứng chỉ tiếng Anh theo mục tiêu nghề nghiệp và học tập.', '<h2><span style=\"background-color: rgb(184, 49, 47); color: rgb(255, 255, 255);\">1. IELTS (International English Language Testing System)</span></h2>\r\n<p><span style=\"font-size: 16px;\"><strong>Mục ti&ecirc;u ch&iacute;nh:</strong> Đi du học đại học/sau đại học, định cư tại c&aacute;c quốc gia sử dụng tiếng Anh (Anh, &Uacute;c, New Zealand, Canada). Mới đ&acirc;y cũng rất chuộng để x&eacute;t tốt nghiệp Đại Học VN.</span></p>\r\n<p><span style=\"font-size: 16px;\"><strong>Đặc điểm:</strong> C&oacute; 2 module chia ra r&otilde; rệt l&agrave; Học thuật (Academic) v&agrave; Tổng qu&aacute;t (General). B&agrave;i thi y&ecirc;u cầu bạn thực sự sử dụng 4 kỹ năng (Nghe-N&oacute;i-Đọc-Viết), với phần Speaking bạn sẽ đối thoại 1-1 với gi&aacute;m khảo thực tế ảo bằng xương bằng thịt.</span></p>\r\n<h2><span style=\"background-color: rgb(41, 105, 176); color: rgb(255, 255, 255);\">2. TOEIC (Test of English for International Communication)</span></h2>\r\n<p><span style=\"font-size: 16px;\"><strong>Mục ti&ecirc;u:</strong> L&agrave;m việc thực tế tại c&aacute;c c&ocirc;ng ty, tập đo&agrave;n quốc tế, tổ chức kinh doanh đa quốc tịch tại Ch&acirc;u &Aacute;.</span></p>\r\n<p><span style=\"font-size: 16px;\"><strong>Đặc điểm:</strong> Dạng b&agrave;i phổ biến nhất chỉ c&oacute; 2 kỹ năng bị động l&agrave; Nghe v&agrave; Đọc (Listening &amp; Reading). Tất cả kiến thức xoay quanh chủ đề kinh tế, thảo luận doanh nghiệp, nh&agrave; h&agrave;ng, hợp đồng, t&agrave;i ch&iacute;nh chứ kh&ocirc;ng đi v&agrave;o khoa học x&atilde; hội.</span></p>\r\n<h2><span style=\"background-color: rgb(250, 197, 28);\">3. TOEFL (Test of English as a Foreign Language)</span></h2>\r\n<p><span style=\"font-size: 16px;\"><strong>Mục ti&ecirc;u:</strong> Gần giống IELTS, chuy&ecirc;n để apply học bổng du học Mỹ v&agrave; c&aacute;c nước Bắc Mỹ.</span></p>\r\n<p><span style=\"font-size: 16px;\"><strong>Đặc điểm:</strong> Mang đậm yếu tố h&agrave;n l&acirc;m (Academic). Rất kh&oacute;! Cấu tr&uacute;c thi tập trung kiểm tra khả năng t&iacute;ch hợp kỹ năng: v&iacute; dụ Listening một b&agrave;i diễn văn Sinh Học d&agrave;i 5 ph&uacute;t sau đ&oacute; phải Speaking t&oacute;m tắt lại nội dung v&agrave; đưa ra quan điểm ph&acirc;n t&iacute;ch phản hồi.</span></p>\r\n<p style=\"text-align: center;\"><span style=\"font-size: 18px;\">👉 <strong>Lời khuy&ecirc;n:</strong> Việc x&aacute;c định r&otilde; chứng chỉ cần lấy ngay từ đầu sẽ tiết kiệm cho bạn tối thiểu 2 năm thời gian v&agrave; chục triệu tiền bạc. Đừng học lan man!</span></p>', 'bai-viet/0X7ifD89Aa9vXyMJzFlPkgvYBHgH6194Bf4emIHJ.png', 1, 3205, 1, '2026-02-26 09:30:00', NULL, '2026-02-26 02:30:00', '2026-02-28 10:07:31'),
(8, 'Lộ trình học tiếng Anh cho người mất gốc', 'lo-trinh-hoc-tieng-anh-cho-nguoi-mat-goc', 'Hướng dẫn chi tiết từ con số 0 đến giao tiếp trôi chảy dành cho người đã từ bỏ tiếng Anh nhiều lần.', '<h2><span style=\"color: rgb(44, 130, 201);\">Giai đoạn 1: Chuẩn h&oacute;a hệ thống ph&aacute;t &acirc;m (1 - 2 th&aacute;ng)</span></h2>\r\n<p><span style=\"font-size: 16px;\">Đa số những người \"mất gốc\" đều học ngữ ph&aacute;p v&agrave; từ vựng th&ocirc;ng qua ghi ch&eacute;p c&acirc;m lặng. Đ&oacute; l&agrave; c&aacute;ch sai! Nếu ph&aacute;t &acirc;m sai, khi m&agrave;ng nhĩ bắt s&oacute;ng được &acirc;m chuẩn của người bản ngữ, n&atilde;o bộ cũng từ chối hiểu &acirc;m đ&oacute;.</span></p>\r\n<p><span style=\"font-size: 16px;\">Giải ph&aacute;p duy nhất l&agrave; phải l&agrave;m quen với <strong>Bảng phi&ecirc;n &acirc;m quốc tế IPA (International Phonetic Alphabet)</strong> gồm 44 &acirc;m. Luyện cấu h&igrave;nh miệng của m&igrave;nh ph&aacute;t ra đ&uacute;ng 44 &acirc;m đ&oacute;.</span></p>\r\n<h2><span style=\"color: rgb(44, 130, 201);\">Giai đoạn 2: X&acirc;y vốn từ v&agrave; cấu tr&uacute;c l&otilde;i (2 - 3 th&aacute;ng)</span></h2>\r\n<p><span style=\"font-size: 16px;\">Tuyệt đối kh&ocirc;ng nhồi nh&eacute;t t&agrave;i liệu ngữ ph&aacute;p n&acirc;ng cao. Bạn chỉ cần:</span></p>\r\n<ul>\r\n<li><span style=\"font-size: 16px;\">Học vững 5 th&igrave; căn bản (Hiện tại đơn, Tiếp diễn, Qu&aacute; khứ đơn, Tương lai đơn, Hiện tại ho&agrave;n th&agrave;nh).</span></li>\r\n<li><span style=\"font-size: 16px;\">Sở hữu tối thiểu <strong>1000 - 1500 từ vựng</strong> xoay quanh bản th&acirc;n gia đ&igrave;nh, giới thiệu du lịch, c&ocirc;ng việc. Thường l&agrave; c&aacute;c động từ, danh từ m&ocirc; tả xung quanh bạn.</span></li>\r\n</ul>\r\n<h2><span style=\"color: rgb(44, 130, 201);\">Giai đoạn 3: Phản xạ (Shadowing &amp; Listening Intensive)</span></h2>\r\n<p><span style=\"font-size: 16px;\">T&igrave;m những b&agrave;i Podcast rất ngắn, rất dễ (vd: VOA Learning English, TED-Ed). Vừa mở audio nghe qua tai nghe, vừa cầm Script để <strong>nhại/nh&aacute;i lại song song</strong> tốc độ người bản ngữ. Việc n&agrave;y gi&uacute;p cải thiện cơ miệng để h&igrave;nh th&agrave;nh độ tr&ocirc;i chảy ngữ điệu (Intonation &amp; Fluency).</span></p>\r\n<p><span style=\"font-size: 16px;\"><img src=\"https://images.unsplash.com/photo-1522202176988-66273c2fd55f?q=80&amp;w=2671&amp;auto=format&amp;fit=crop\" alt=\"\" width=\"100%\"></span></p>\r\n<blockquote style=\"margin: 0 0 0 40px; border: none; padding: 0px;\">\r\n<p style=\"text-align: center;\"><span style=\"font-size: 20px; color: rgb(226, 80, 65);\"><strong><em>\"Chậm m&agrave; chắc - Đều đặn học 30 ph&uacute;t mỗi ng&agrave;y quan trọng hơn l&agrave; cuối tuần th&acirc;u đ&ecirc;m suốt s&aacute;ng 6 tiếng rồi bỏ cuộc.\"</em></strong></span></p>\r\n</blockquote>', 'bai-viet/SPCIoIy75IXRM4sDOb0GD9AYqKHRBZIi7Px4aD6i.jpg', 1, 5401, 1, '2026-02-27 18:45:00', NULL, '2026-02-27 11:45:00', '2026-02-28 09:01:38');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `baiviet_danhmuc`
--

CREATE TABLE `baiviet_danhmuc` (
  `baiVietId` int(11) NOT NULL,
  `danhMucId` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `baiviet_danhmuc`
--

INSERT INTO `baiviet_danhmuc` (`baiVietId`, `danhMucId`) VALUES
(8, 1),
(7, 4),
(8, 4),
(3, 5),
(7, 5);

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `baiviet_tag`
--

CREATE TABLE `baiviet_tag` (
  `baiVietId` int(11) NOT NULL,
  `tagId` bigint(20) UNSIGNED NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `baiviet_tag`
--

INSERT INTO `baiviet_tag` (`baiVietId`, `tagId`) VALUES
(7, 3),
(3, 10),
(3, 11),
(3, 12),
(7, 13),
(8, 13);

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `buoihoc`
--

CREATE TABLE `buoihoc` (
  `buoiHocId` int(11) NOT NULL,
  `lopHocId` int(11) DEFAULT NULL,
  `tenBuoiHoc` varchar(255) DEFAULT NULL,
  `ngayHoc` date DEFAULT NULL,
  `caHocId` int(11) DEFAULT NULL,
  `phongHocId` int(11) DEFAULT NULL,
  `taiKhoanId` int(11) DEFAULT NULL COMMENT 'Giáo viên dạy',
  `ghiChu` text DEFAULT NULL,
  `daDiemDanh` tinyint(4) DEFAULT 0,
  `daHoanThanh` tinyint(4) DEFAULT 0,
  `trangThai` tinyint(4) DEFAULT 1,
  `created_at` datetime DEFAULT current_timestamp(),
  `updated_at` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `buoihoc`
--

INSERT INTO `buoihoc` (`buoiHocId`, `lopHocId`, `tenBuoiHoc`, `ngayHoc`, `caHocId`, `phongHocId`, `taiKhoanId`, `ghiChu`, `daDiemDanh`, `daHoanThanh`, `trangThai`, `created_at`, `updated_at`) VALUES
(53, 9, 'Buổi 1: Lớp tiếng nhật giao tiếp 1', '2026-03-02', 1, 4, 5, NULL, 0, 0, 0, '2026-02-27 14:14:12', '2026-02-27 14:14:12'),
(54, 9, 'Buổi 2: Lớp tiếng nhật giao tiếp 1', '2026-03-05', 1, 4, 5, NULL, 0, 0, 0, '2026-02-27 14:14:12', '2026-02-27 14:14:12'),
(55, 9, 'Buổi 3: Lớp tiếng nhật giao tiếp 1', '2026-03-09', 1, 4, 5, NULL, 0, 0, 0, '2026-02-27 14:14:12', '2026-02-27 14:14:12'),
(56, 9, 'Buổi 4: Lớp tiếng nhật giao tiếp 1', '2026-03-12', 1, 4, 5, NULL, 0, 0, 0, '2026-02-27 14:14:12', '2026-02-27 14:14:12'),
(57, 9, 'Buổi 5: Lớp tiếng nhật giao tiếp 1', '2026-03-16', 1, 4, 5, NULL, 0, 0, 0, '2026-02-27 14:14:12', '2026-02-27 14:14:12'),
(58, 9, 'Buổi 6: Lớp tiếng nhật giao tiếp 1', '2026-03-19', 1, 4, 5, NULL, 0, 0, 0, '2026-02-27 14:14:12', '2026-02-27 14:14:12'),
(59, 9, 'Buổi 7: Lớp tiếng nhật giao tiếp 1', '2026-03-23', 1, 4, 5, NULL, 0, 0, 0, '2026-02-27 14:14:12', '2026-02-27 14:14:12'),
(60, 9, 'Buổi 8: Lớp tiếng nhật giao tiếp 1', '2026-03-26', 1, 4, 5, NULL, 0, 0, 0, '2026-02-27 14:14:12', '2026-02-27 14:14:12'),
(61, 9, 'Buổi 9: Lớp tiếng nhật giao tiếp 1', '2026-03-30', 1, 4, 5, NULL, 0, 0, 0, '2026-02-27 14:14:12', '2026-02-27 14:14:12'),
(62, 9, 'Buổi 10: Lớp tiếng nhật giao tiếp 1', '2026-04-02', 1, 4, 5, NULL, 0, 0, 0, '2026-02-27 14:14:12', '2026-02-27 14:14:12'),
(63, 9, 'Buổi 11: Lớp tiếng nhật giao tiếp 1', '2026-04-06', 1, 4, 5, NULL, 0, 0, 0, '2026-02-27 14:14:12', '2026-02-27 14:14:12'),
(64, 9, 'Buổi 12: Lớp tiếng nhật giao tiếp 1', '2026-04-09', 1, 4, 5, NULL, 0, 0, 0, '2026-02-27 14:14:12', '2026-02-27 14:14:12'),
(65, 9, 'Buổi 13: Lớp tiếng nhật giao tiếp 1', '2026-04-13', 1, 4, 5, NULL, 0, 0, 0, '2026-02-27 14:14:12', '2026-02-27 14:14:12'),
(66, 9, 'Buổi 14: Lớp tiếng nhật giao tiếp 1', '2026-04-16', 1, 4, 5, NULL, 0, 0, 0, '2026-02-27 14:14:12', '2026-02-27 14:14:12'),
(67, 9, 'Buổi 15: Lớp tiếng nhật giao tiếp 1', '2026-04-20', 1, 4, 5, NULL, 0, 0, 0, '2026-02-27 14:14:12', '2026-02-27 14:14:12'),
(68, 9, 'Buổi 16: Lớp tiếng nhật giao tiếp 1', '2026-04-23', 1, 4, 5, NULL, 0, 0, 0, '2026-02-27 14:14:12', '2026-02-27 14:14:12'),
(69, 9, 'Buổi 17: Lớp tiếng nhật giao tiếp 1', '2026-04-27', 1, 4, 5, NULL, 0, 0, 0, '2026-02-27 14:14:12', '2026-02-27 14:14:12'),
(70, 9, 'Buổi 18: Lớp tiếng nhật giao tiếp 1', '2026-04-30', 1, 4, 5, NULL, 0, 0, 0, '2026-02-27 14:14:12', '2026-02-27 14:14:12'),
(71, 9, 'Buổi 19: Lớp tiếng nhật giao tiếp 1', '2026-05-04', 1, 4, 5, NULL, 0, 0, 0, '2026-02-27 14:14:12', '2026-02-27 14:14:12'),
(72, 9, 'Buổi 20: Lớp tiếng nhật giao tiếp 1', '2026-05-07', 1, 4, 5, NULL, 0, 0, 0, '2026-02-27 14:14:12', '2026-02-27 14:14:12'),
(73, 9, 'Buổi 21: Lớp tiếng nhật giao tiếp 1', '2026-05-11', 1, 4, 5, NULL, 0, 0, 0, '2026-02-27 14:14:12', '2026-02-27 14:14:12'),
(74, 9, 'Buổi 22: Lớp tiếng nhật giao tiếp 1', '2026-05-14', 1, 4, 5, NULL, 0, 0, 0, '2026-02-27 14:14:12', '2026-02-27 14:14:12'),
(75, 9, 'Buổi 23: Lớp tiếng nhật giao tiếp 1', '2026-05-18', 1, 4, 5, NULL, 0, 0, 0, '2026-02-27 14:14:12', '2026-02-27 14:14:12'),
(76, 9, 'Buổi 24: Lớp tiếng nhật giao tiếp 1', '2026-05-21', 1, 4, 5, NULL, 0, 0, 0, '2026-02-27 14:14:12', '2026-02-27 14:14:12'),
(77, 9, 'Buổi 25: Lớp tiếng nhật giao tiếp 1', '2026-05-25', 1, 4, 5, NULL, 0, 0, 0, '2026-02-27 14:14:12', '2026-02-27 14:14:12'),
(78, 9, 'Buổi 26: Lớp tiếng nhật giao tiếp 1', '2026-05-28', 1, 4, 5, NULL, 0, 0, 0, '2026-02-27 14:14:12', '2026-02-27 14:14:12'),
(79, 9, 'Buổi 27: Lớp tiếng nhật giao tiếp 1', '2026-06-01', 1, 4, 5, NULL, 0, 0, 0, '2026-02-27 14:14:12', '2026-02-27 14:14:12'),
(80, 9, 'Buổi 28: Lớp tiếng nhật giao tiếp 1', '2026-06-04', 1, 4, 5, NULL, 0, 0, 0, '2026-02-27 14:14:12', '2026-02-27 14:14:12'),
(81, 9, 'Buổi 29: Lớp tiếng nhật giao tiếp 1', '2026-06-08', 1, 4, 5, NULL, 0, 0, 0, '2026-02-27 14:14:12', '2026-02-27 14:14:12'),
(82, 9, 'Buổi 30: Lớp tiếng nhật giao tiếp 1', '2026-06-11', 1, 4, 5, NULL, 0, 0, 0, '2026-02-27 14:14:12', '2026-02-27 14:14:12'),
(83, 9, 'Buổi 31: Lớp tiếng nhật giao tiếp 1', '2026-06-15', 1, 4, 5, NULL, 0, 0, 0, '2026-02-27 14:14:12', '2026-02-27 14:14:12'),
(84, 9, 'Buổi 32: Lớp tiếng nhật giao tiếp 1', '2026-06-18', 1, 4, 5, NULL, 0, 0, 0, '2026-02-27 14:14:12', '2026-02-27 14:14:12'),
(85, 9, 'Buổi 33: Lớp tiếng nhật giao tiếp 1', '2026-06-22', 1, 4, 5, NULL, 0, 0, 0, '2026-02-27 14:14:12', '2026-02-27 14:14:12'),
(86, 9, 'Buổi 34: Lớp tiếng nhật giao tiếp 1', '2026-06-25', 1, 4, 5, NULL, 0, 0, 0, '2026-02-27 14:14:12', '2026-02-27 14:14:12'),
(87, 9, 'Buổi 35: Lớp tiếng nhật giao tiếp 1', '2026-06-29', 1, 4, 5, NULL, 0, 0, 0, '2026-02-27 14:14:12', '2026-02-27 14:14:12');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `cache`
--

CREATE TABLE `cache` (
  `key` varchar(255) NOT NULL,
  `value` mediumtext NOT NULL,
  `expiration` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `cache`
--

INSERT INTO `cache` (`key`, `value`, `expiration`) VALUES
('laravel-cache-admin@admin.com|127.0.0.1', 'i:1;', 1772355678),
('laravel-cache-admin@admin.com|127.0.0.1:timer', 'i:1772355678;', 1772355678),
('laravel-cache-admin@gmail.com|127.0.0.1', 'i:2;', 1772355699),
('laravel-cache-admin@gmail.com|127.0.0.1:timer', 'i:1772355699;', 1772355699),
('laravel-cache-admin|::1', 'i:1;', 1771553959),
('laravel-cache-admin|::1:timer', 'i:1771553959;', 1771553959),
('laravel-cache-levanhau1@gmail.com|127.0.0.1', 'i:1;', 1771552084),
('laravel-cache-levanhau1@gmail.com|127.0.0.1:timer', 'i:1771552084;', 1771552084),
('laravel-cache-levanhauadmin114|::1', 'i:1;', 1771553974),
('laravel-cache-levanhauadmin114|::1:timer', 'i:1771553974;', 1771553974),
('laravel-cache-levanhaum@gmail.com|127.0.0.1', 'i:1;', 1771558245),
('laravel-cache-levanhaum@gmail.com|127.0.0.1:timer', 'i:1771558245;', 1771558245),
('trung-tam-ngoai-ngu-cache-admiadmin@admin.com|127.0.0.1', 'i:1;', 1771641489),
('trung-tam-ngoai-ngu-cache-admiadmin@admin.com|127.0.0.1:timer', 'i:1771641489;', 1771641489),
('trung-tam-ngoai-ngu-cache-levanhau1@gmail.com|127.0.0.1', 'i:3;', 1771639455),
('trung-tam-ngoai-ngu-cache-levanhau1@gmail.com|127.0.0.1:timer', 'i:1771639455;', 1771639455);

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `cache_locks`
--

CREATE TABLE `cache_locks` (
  `key` varchar(255) NOT NULL,
  `owner` varchar(255) NOT NULL,
  `expiration` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `cahoc`
--

CREATE TABLE `cahoc` (
  `caHocId` int(11) NOT NULL,
  `tenCa` varchar(50) DEFAULT NULL,
  `gioBatDau` time DEFAULT NULL,
  `gioKetThuc` time DEFAULT NULL,
  `trangThai` tinyint(4) DEFAULT 1,
  `created_at` datetime DEFAULT current_timestamp(),
  `updated_at` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `cahoc`
--

INSERT INTO `cahoc` (`caHocId`, `tenCa`, `gioBatDau`, `gioKetThuc`, `trangThai`, `created_at`, `updated_at`) VALUES
(1, 'Ca Sáng 1', '08:00:00', '10:00:00', 1, '2026-01-23 20:07:20', '2026-01-23 20:07:20'),
(2, 'Ca Tối 2', '18:00:00', '20:00:00', 1, '2026-01-23 20:07:20', '2026-01-23 20:07:20');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `cosodaotao`
--

CREATE TABLE `cosodaotao` (
  `coSoId` int(11) NOT NULL,
  `maCoSo` varchar(20) NOT NULL,
  `slug` varchar(150) NOT NULL,
  `tenCoSo` varchar(255) NOT NULL,
  `diaChi` varchar(255) DEFAULT NULL,
  `soDienThoai` varchar(20) DEFAULT NULL,
  `email` varchar(100) DEFAULT NULL,
  `banDoGoogle` text DEFAULT NULL,
  `tinhThanhId` int(11) DEFAULT NULL,
  `maPhuongXa` int(10) UNSIGNED DEFAULT NULL,
  `tenPhuongXa` varchar(150) DEFAULT NULL,
  `viDo` decimal(10,7) DEFAULT NULL,
  `kinhDo` decimal(10,7) DEFAULT NULL,
  `ngayKhaiTruong` date DEFAULT NULL,
  `trangThai` tinyint(4) DEFAULT 1,
  `created_at` datetime DEFAULT current_timestamp(),
  `updated_at` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `cosodaotao`
--

INSERT INTO `cosodaotao` (`coSoId`, `maCoSo`, `slug`, `tenCoSo`, `diaChi`, `soDienThoai`, `email`, `banDoGoogle`, `tinhThanhId`, `maPhuongXa`, `tenPhuongXa`, `viDo`, `kinhDo`, `ngayKhaiTruong`, `trangThai`, `created_at`, `updated_at`) VALUES
(1, 'CS01', 'chi-nhanh-quan-1', 'Chi nhánh Quận 1', '123 Lê Lợi, Q1, HCM', '02838123456', 'q1@center.com', 'https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3834.641108987922!2d108.21948517688925!3d16.032187484641973!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x314219ee598df9c5%3A0xaadb53409be7c909!2zVHLGsOG7nW5nIMSQ4bqhaSBo4buNYyBLaeG6v24gdHLDumMgxJDDoCBO4bq1bmc!5e0!3m2!1svi!2s!4v1769507200475!5m2!1svi!2s', 217, 26743, 'Phường Bến Thành', 10.7714230, 106.6984710, '2024-01-01', 1, '2026-01-23 20:07:12', '2026-02-21 23:46:16'),
(13, 'CS001', 'chi-nhanh-nguyen-van-linh', 'Chi nhánh Nguyễn Văn Linh', '370 Nguyễn Văn Linh', '0777464347', 'fgnguyenvanlinh@gmail.com', 'https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3834.1240200285793!2d108.20496117539444!3d16.05905283970972!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x3142192e08f910c5%3A0x676e0cb3f8b52221!2zQ2VsbHBob25lUyAtIFRydW5nIHTDom0gTGFwdG9wLCBTbWFydCBIb21lIGNow61uaCBow6NuZywgZ2nDoSB04buRdA!5e0!3m2!1svi!2s!4v1771730749397!5m2!1svi!2s', 210, 20209, 'Phường Thanh Khê', 16.0590528, 108.2049612, '2026-02-23', 1, '2026-02-22 10:26:29', '2026-02-22 10:26:29'),
(14, 'CS-NVL-01', 'chi-nhanh-nguyen-van-linh-da-nang', 'Chi nhánh Nguyễn Văn Linh Đà Nẵng', '400 Nguyễn Văn Linh', '0888888888', 'csnguyenvanlinh@gmail.com', 'https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3834.1240200285793!2d108.20496117539444!3d16.05905283970972!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x314219a8a321e65d%3A0x30f53a57540b4d86!2s2AM%20Coffee!5e0!3m2!1svi!2s!4v1772177164433!5m2!1svi!2s', 210, 20257, 'Phường Hòa Cường', 16.0590528, 108.2049612, '2026-03-01', 1, '2026-02-27 14:26:37', '2026-02-27 14:26:37');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `dangkylophoc`
--

CREATE TABLE `dangkylophoc` (
  `dangKyLopHocId` int(11) NOT NULL,
  `taiKhoanId` int(11) DEFAULT NULL COMMENT 'Học viên',
  `lopHocId` int(11) DEFAULT NULL,
  `ngayDangKy` date DEFAULT NULL,
  `trangThai` int(11) DEFAULT NULL,
  `created_at` datetime DEFAULT current_timestamp(),
  `updated_at` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `dangkylophoc`
--

INSERT INTO `dangkylophoc` (`dangKyLopHocId`, `taiKhoanId`, `lopHocId`, `ngayDangKy`, `trangThai`, `created_at`, `updated_at`) VALUES
(85, 23, 9, '2026-02-27', 1, '2026-02-27 17:40:08', '2026-02-27 17:40:08'),
(87, 23, 11, '2026-03-01', 1, '2026-03-01 16:25:31', '2026-03-01 16:25:31');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `danhgiagiaovien`
--

CREATE TABLE `danhgiagiaovien` (
  `danhGiaId` int(11) NOT NULL,
  `giaoVienId` int(11) DEFAULT NULL,
  `hocVienId` int(11) DEFAULT NULL,
  `lopHocId` int(11) DEFAULT NULL,
  `soSao` tinyint(4) DEFAULT NULL,
  `noiDung` text DEFAULT NULL,
  `ngayDanhGia` date DEFAULT NULL,
  `created_at` datetime DEFAULT current_timestamp(),
  `updated_at` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `danhmucbaiviet`
--

CREATE TABLE `danhmucbaiviet` (
  `danhMucId` int(11) NOT NULL,
  `tenDanhMuc` varchar(100) NOT NULL,
  `slug` varchar(100) NOT NULL,
  `moTa` text DEFAULT NULL,
  `trangThai` tinyint(4) DEFAULT 1,
  `created_at` timestamp NULL DEFAULT current_timestamp(),
  `updated_at` timestamp NULL DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `danhmucbaiviet`
--

INSERT INTO `danhmucbaiviet` (`danhMucId`, `tenDanhMuc`, `slug`, `moTa`, `trangThai`, `created_at`, `updated_at`) VALUES
(1, 'Kinh nghiệm học IELTS', 'kinh-nghiem-hoc-ielts', 'Chia sẻ kinh nghiệm luyện thi IELTS', 1, '2026-01-27 06:11:29', '2026-01-27 06:11:29'),
(2, 'Mẹo học tiếng Anh', 'meo-hoc-tieng-anh', 'Các mẹo học tiếng Anh hiệu quả', 1, '2026-01-27 06:11:29', '2026-01-27 06:11:29'),
(3, 'Tin tức trung tâm', 'tin-tuc-trung-tam', 'Thông báo và tin tức từ trung tâm', 1, '2026-01-27 06:11:29', '2026-01-27 06:11:29'),
(4, 'Chia sẻ học viên', 'chia-se-hoc-vien', 'Câu chuyện và trải nghiệm học viên', 1, '2026-01-27 06:11:29', '2026-01-27 06:11:29'),
(5, 'Đánh giá trung tâm', 'danh-gia-trung-tam', 'Các đánh giá từ học viên về trung tâm', 1, '2026-02-28 08:10:07', '2026-02-28 08:10:22');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `danhmuckhoahoc`
--

CREATE TABLE `danhmuckhoahoc` (
  `danhMucId` int(11) NOT NULL,
  `tenDanhMuc` varchar(100) NOT NULL,
  `slug` varchar(100) NOT NULL,
  `moTa` text DEFAULT NULL,
  `trangThai` tinyint(4) DEFAULT 1,
  `created_at` datetime DEFAULT current_timestamp(),
  `updated_at` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `danhmuckhoahoc`
--

INSERT INTO `danhmuckhoahoc` (`danhMucId`, `tenDanhMuc`, `slug`, `moTa`, `trangThai`, `created_at`, `updated_at`) VALUES
(1, 'Tiếng Anh', 'tieng-anh', 'Các loại khóa học tiếng anh từ cơ bản đến nâng cao', 1, '2026-01-23 20:07:20', '2026-02-28 14:34:15'),
(2, 'Tiếng Nhật', 'tieng-nhat', 'Các loại khóa học tiếng nhật từ cơ bản đến nâng cao', 1, '2026-01-23 20:07:20', '2026-02-28 14:34:27');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `diembaithi`
--

CREATE TABLE `diembaithi` (
  `diemThiId` int(11) NOT NULL,
  `taiKhoanId` int(11) DEFAULT NULL,
  `baiThiId` int(11) DEFAULT NULL,
  `diemSo` decimal(4,2) DEFAULT NULL,
  `ghiChu` text DEFAULT NULL,
  `created_at` datetime DEFAULT current_timestamp(),
  `updated_at` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `diemdanh`
--

CREATE TABLE `diemdanh` (
  `diemDanhId` varchar(50) NOT NULL,
  `taiKhoanId` int(11) DEFAULT NULL COMMENT 'Học viên',
  `buoiHocId` int(11) DEFAULT NULL,
  `trangThai` tinyint(4) DEFAULT NULL COMMENT '0: vắng, 1: có mặt, 2: trễ',
  `ghiChu` text DEFAULT NULL,
  `created_at` datetime DEFAULT current_timestamp(),
  `updated_at` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `failed_jobs`
--

CREATE TABLE `failed_jobs` (
  `id` bigint(20) UNSIGNED NOT NULL,
  `uuid` varchar(255) NOT NULL,
  `connection` text NOT NULL,
  `queue` text NOT NULL,
  `payload` longtext NOT NULL,
  `exception` longtext NOT NULL,
  `failed_at` timestamp NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `hoadon`
--

CREATE TABLE `hoadon` (
  `hoaDonId` int(11) NOT NULL,
  `maHoaDon` varchar(20) DEFAULT NULL,
  `ngayLap` date DEFAULT NULL,
  `ngayHetHan` date DEFAULT NULL,
  `tongTien` decimal(15,2) DEFAULT NULL,
  `giamGia` decimal(15,2) NOT NULL DEFAULT 0.00,
  `thue` decimal(5,2) NOT NULL DEFAULT 0.00,
  `tongTienSauThue` decimal(15,2) NOT NULL DEFAULT 0.00,
  `daTra` decimal(15,2) DEFAULT NULL,
  `taiKhoanId` int(11) DEFAULT NULL,
  `nguoiLapId` bigint(20) UNSIGNED DEFAULT NULL,
  `dangKyLopHocId` int(11) DEFAULT NULL,
  `phuongThucThanhToan` int(11) DEFAULT NULL,
  `loaiHoaDon` tinyint(4) NOT NULL DEFAULT 0 COMMENT '0=Đăng ký mới, 1=Gia hạn, 2=Khác',
  `coSoId` int(11) DEFAULT NULL,
  `trangThai` tinyint(4) DEFAULT NULL COMMENT '0: Chưa thanh toán\r\n1: Đã thanh toán một phần\r\n2: Đã thanh toán đủ',
  `ghiChu` text DEFAULT NULL,
  `created_at` datetime DEFAULT current_timestamp(),
  `updated_at` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `hoadon`
--

INSERT INTO `hoadon` (`hoaDonId`, `maHoaDon`, `ngayLap`, `ngayHetHan`, `tongTien`, `giamGia`, `thue`, `tongTienSauThue`, `daTra`, `taiKhoanId`, `nguoiLapId`, `dangKyLopHocId`, `phuongThucThanhToan`, `loaiHoaDon`, `coSoId`, `trangThai`, `ghiChu`, `created_at`, `updated_at`) VALUES
(14, NULL, '2026-02-27', NULL, 2625000.00, 50000.00, 0.00, 0.00, 675000.00, 23, NULL, 85, 1, 0, 1, 1, 'Đăng ký lớp Lớp tiếng nhật giao tiếp 1', '2026-02-27 17:40:08', '2026-03-01 15:59:39'),
(16, 'HD-202603-000001', '2026-03-01', '2026-03-31', 4000000.00, 0.00, 0.00, 4000000.00, 0.00, 23, NULL, 87, 1, 0, 1, 0, 'Đăng ký lớp Lớp tiếng Nhật giao tiếp nâng cao 2 - Khóa Tiếng Nhật sơ cấp', '2026-03-01 16:25:31', '2026-03-01 16:25:31');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `hocphi`
--

CREATE TABLE `hocphi` (
  `hocPhiId` bigint(20) NOT NULL,
  `khoaHocId` int(11) DEFAULT NULL,
  `soBuoi` int(11) DEFAULT NULL,
  `donGia` decimal(15,2) DEFAULT NULL,
  `trangThai` tinyint(4) DEFAULT 1,
  `created_at` datetime DEFAULT current_timestamp(),
  `updated_at` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `hocphi`
--

INSERT INTO `hocphi` (`hocPhiId`, `khoaHocId`, `soBuoi`, `donGia`, `trangThai`, `created_at`, `updated_at`) VALUES
(3, 4, 20, 15000.00, 1, '2026-02-26 15:10:48', '2026-02-26 15:10:48'),
(4, 4, 25, 20000.00, 1, '2026-02-26 15:11:30', '2026-02-26 15:11:30'),
(5, 3, 35, 50000.00, 1, '2026-02-26 15:28:05', '2026-02-26 15:28:05'),
(6, 5, 40, 100000.00, 1, '2026-02-27 14:07:33', '2026-02-27 14:07:33'),
(7, 5, 35, 75000.00, 1, '2026-02-27 14:07:44', '2026-02-27 14:07:44');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `hosonguoidung`
--

CREATE TABLE `hosonguoidung` (
  `taiKhoanId` int(11) NOT NULL,
  `hoTen` varchar(100) NOT NULL,
  `soDienThoai` varchar(15) DEFAULT NULL,
  `zalo` varchar(20) DEFAULT NULL,
  `ngaySinh` date DEFAULT NULL,
  `gioiTinh` tinyint(4) DEFAULT NULL COMMENT '0: nữ, 1: nam, 2: khác',
  `diaChi` varchar(255) DEFAULT NULL,
  `nguoiGiamHo` varchar(100) DEFAULT NULL,
  `sdtGuardian` varchar(20) DEFAULT NULL,
  `moiQuanHe` varchar(50) DEFAULT NULL,
  `trinhDoHienTai` varchar(30) DEFAULT NULL,
  `ngonNguMucTieu` varchar(50) DEFAULT NULL,
  `nguonBietDen` varchar(50) DEFAULT NULL,
  `ghiChu` text DEFAULT NULL,
  `cccd` varchar(20) DEFAULT NULL,
  `anhDaiDien` varchar(255) DEFAULT NULL,
  `created_at` datetime DEFAULT current_timestamp(),
  `updated_at` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `hosonguoidung`
--

INSERT INTO `hosonguoidung` (`taiKhoanId`, `hoTen`, `soDienThoai`, `zalo`, `ngaySinh`, `gioiTinh`, `diaChi`, `nguoiGiamHo`, `sdtGuardian`, `moiQuanHe`, `trinhDoHienTai`, `ngonNguMucTieu`, `nguonBietDen`, `ghiChu`, `cccd`, `anhDaiDien`, `created_at`, `updated_at`) VALUES
(1, 'Lê Văn Hậu', '0901234567', NULL, '1985-01-01', 1, 'HCM', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '123456789001', 'anh-dai-dien/QQMXMCofxNCH3D7ECdNCEiAm7jgkraeYfUm0hpQe.jpg', '2026-01-23 20:07:12', '2026-02-27 16:56:05'),
(4, 'John Smith', '0904234567', NULL, '1988-10-10', 1, 'USA', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '123456789004', 'van-hau.png', '2026-01-23 20:07:12', '2026-01-26 19:55:38'),
(5, 'Nguyễn Văn An', '0907234567', NULL, '2005-03-20', 1, 'Q1, HCM', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '123456789007', 'hoang-le.png', '2026-01-23 20:07:12', '2026-01-26 19:57:12'),
(22, 'Lê Văn Hậu HV', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '2026-02-21 11:30:47', '2026-02-21 11:30:47'),
(23, 'Hậu Lê Văn', '0777464347', '0816548150', '2004-10-14', 1, 'Số 55, xã điện phong, thị xã điện bàn, tỉnh quảng nam', NULL, NULL, NULL, 'Elementary (Sơ cấp)', 'Tiếng Anh', 'Bạn bè giới thiệu', NULL, '049204011849', 'anh-dai-dien/VjdFONoc95Fvr2eh7R6Askttm1Yo7kiY1jNdLznK.jpg', '2026-02-21 11:45:30', '2026-02-28 13:47:43'),
(26, 'Lê Văn Giáo Viên', '0816548150', NULL, '2000-10-14', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '049204011848', NULL, '2026-02-25 18:37:41', '2026-02-25 18:37:41'),
(27, 'Lê Ngọc Ánh', NULL, NULL, '2004-04-12', 1, NULL, NULL, NULL, 'Mẹ', 'Beginner (Mới bắt đầu)', 'Tiếng Hàn', 'Google / Website', NULL, '094949494944', NULL, '2026-02-27 14:20:38', '2026-02-27 14:20:38');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `jobs`
--

CREATE TABLE `jobs` (
  `id` bigint(20) UNSIGNED NOT NULL,
  `queue` varchar(255) NOT NULL,
  `payload` longtext NOT NULL,
  `attempts` tinyint(3) UNSIGNED NOT NULL,
  `reserved_at` int(10) UNSIGNED DEFAULT NULL,
  `available_at` int(10) UNSIGNED NOT NULL,
  `created_at` int(10) UNSIGNED NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `job_batches`
--

CREATE TABLE `job_batches` (
  `id` varchar(255) NOT NULL,
  `name` varchar(255) NOT NULL,
  `total_jobs` int(11) NOT NULL,
  `pending_jobs` int(11) NOT NULL,
  `failed_jobs` int(11) NOT NULL,
  `failed_job_ids` longtext NOT NULL,
  `options` mediumtext DEFAULT NULL,
  `cancelled_at` int(11) DEFAULT NULL,
  `created_at` int(11) NOT NULL,
  `finished_at` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `khoahoc`
--

CREATE TABLE `khoahoc` (
  `khoaHocId` int(11) NOT NULL,
  `danhMucId` int(11) DEFAULT NULL,
  `tenKhoaHoc` varchar(255) NOT NULL,
  `slug` varchar(255) NOT NULL,
  `anhKhoaHoc` varchar(255) DEFAULT NULL,
  `moTa` text DEFAULT NULL,
  `trangThai` tinyint(4) DEFAULT 1,
  `created_at` datetime DEFAULT current_timestamp(),
  `updated_at` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `doiTuong` text DEFAULT NULL COMMENT 'Đối tượng học viên',
  `ketQuaDatDuoc` text DEFAULT NULL COMMENT 'Kết quả sau khóa học',
  `yeuCauDauVao` text DEFAULT NULL COMMENT 'Yêu cầu kiến thức đầu vào',
  `deleted_at` timestamp NULL DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `khoahoc`
--

INSERT INTO `khoahoc` (`khoaHocId`, `danhMucId`, `tenKhoaHoc`, `slug`, `anhKhoaHoc`, `moTa`, `trangThai`, `created_at`, `updated_at`, `doiTuong`, `ketQuaDatDuoc`, `yeuCauDauVao`, `deleted_at`) VALUES
(3, 1, 'IELTS Intensive 6.5+ 2', 'ielts-intensive-2', 'khoa-hoc/MSskHWqVdeFmSTZlVmKoJkciJzymNuQlOVTnek1d.jpg', 'Khóa học cấp tốc 3 tháng', 1, '2026-01-23 20:07:20', '2026-02-26 15:23:28', NULL, NULL, NULL, NULL),
(4, 1, 'Ielts cơ bản 1', 'ielts-co-ban-1', 'khoa-hoc/Fz8LGvPkv2JAvtNlsGBSSVWgrRng4FOJDMrXvT43.jpg', 'giới thiệu 01', 1, '2026-02-26 14:59:04', '2026-02-26 15:25:44', 'Người bắt đầu học', 'sẽ đạt được chứng chỉ ielts', 'yêu cầu học viên có kỹ năng thành thạo', '2026-02-26 08:25:44'),
(5, 2, 'Tiếng Nhật sơ cấp', 'tieng-nhat-so-cap', NULL, 'mô tả 01', 1, '2026-02-27 14:05:06', '2026-02-27 14:05:14', 'Người bắt đầu học', 'chứng chỉ', 'yêu cầu 01', NULL);

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `lienhe`
--

CREATE TABLE `lienhe` (
  `lienHeId` int(11) NOT NULL,
  `hoTen` varchar(100) DEFAULT NULL,
  `email` varchar(100) DEFAULT NULL,
  `soDienThoai` varchar(15) DEFAULT NULL,
  `tieuDe` varchar(255) DEFAULT NULL,
  `noiDung` text DEFAULT NULL,
  `trangThai` tinyint(4) DEFAULT NULL,
  `taiKhoanId` int(11) DEFAULT NULL,
  `created_at` datetime DEFAULT current_timestamp(),
  `updated_at` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `lienhe`
--

INSERT INTO `lienhe` (`lienHeId`, `hoTen`, `email`, `soDienThoai`, `tieuDe`, `noiDung`, `trangThai`, `taiKhoanId`, `created_at`, `updated_at`) VALUES
(1, 'Lê Hậu', 'Levanhaum@gmail.com', NULL, 'Đăng ký tư vấn miễn phí', 'Đăng ký tư vấn miễn phí\nKhóa học quan tâm: ielts-intensive\n', 0, 1, '2026-02-06 21:38:30', '2026-02-06 21:38:30'),
(2, 'Lê Văn Hậu', NULL, '0777464347', 'Đăng ký tư vấn miễn phí', 'Đăng ký tư vấn miễn phí\nKhóa học quan tâm: ielts-intensive\n', 0, 1, '2026-02-06 21:43:58', '2026-02-06 21:43:58'),
(3, 'Lê Văn Sỹ', 'Levanhaum@gmail.com', '0777464347', 'Đăng ký tư vấn miễn phí', 'Đăng ký tư vấn miễn phí\nKhóa học quan tâm: English for Beginners\nCơ sở: Chi nhánh Đà Nẵng 1\n', 0, 1, '2026-02-06 21:46:26', '2026-02-06 21:46:26'),
(4, 'Hậu Lê Văn', 'levanhaum@gmail.com', '0777464347', 'Đăng ký tư vấn miễn phí', 'Đăng ký tư vấn miễn phí\nKhóa học quan tâm: IELTS Intensive 6.5+\nCơ sở: Chi nhánh Quận 1\n', 0, 1, '2026-02-21 18:16:24', '2026-02-21 18:16:24');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `lophoc`
--

CREATE TABLE `lophoc` (
  `lopHocId` int(11) NOT NULL,
  `slug` varchar(255) DEFAULT NULL,
  `tenLopHoc` varchar(255) DEFAULT NULL,
  `khoaHocId` int(11) DEFAULT NULL,
  `phongHocId` int(11) DEFAULT NULL,
  `taiKhoanId` int(11) DEFAULT NULL COMMENT 'Giáo viên phụ trách',
  `hocPhiId` bigint(20) DEFAULT NULL,
  `ngayBatDau` date DEFAULT NULL,
  `ngayKetThuc` date DEFAULT NULL,
  `soBuoiDuKien` int(11) DEFAULT NULL,
  `soHocVienToiDa` int(11) DEFAULT NULL,
  `donGiaDay` decimal(15,2) DEFAULT NULL,
  `lichHoc` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin DEFAULT NULL,
  `coSoId` int(11) DEFAULT NULL,
  `caHocId` int(11) NOT NULL,
  `trangThai` tinyint(4) DEFAULT NULL COMMENT '0: sắp mở, 1: đang mở, 2: đã đóng, 3: đã hủy, 4: đang học',
  `created_at` datetime DEFAULT current_timestamp(),
  `updated_at` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `lophoc`
--

INSERT INTO `lophoc` (`lopHocId`, `slug`, `tenLopHoc`, `khoaHocId`, `phongHocId`, `taiKhoanId`, `hocPhiId`, `ngayBatDau`, `ngayKetThuc`, `soBuoiDuKien`, `soHocVienToiDa`, `donGiaDay`, `lichHoc`, `coSoId`, `caHocId`, `trangThai`, `created_at`, `updated_at`) VALUES
(9, 'lop-tieng-nhat-giao-tiep-1', 'Lớp tiếng nhật giao tiếp 1', 5, 4, NULL, 7, '2026-03-01', '2026-06-30', 35, 35, 300000.00, '2,5', 1, 1, 3, '2026-02-27 14:11:38', '2026-02-28 13:50:36'),
(10, 'lop-tieng-nhat-giao-tiep-nang-cao', 'Lớp tiếng Nhật giao tiếp nâng cao', 5, 5, NULL, NULL, '2026-03-02', '2026-04-30', 26, 35, 250000.00, '3,4,7', 14, 1, 1, '2026-03-01 16:10:57', '2026-03-01 16:12:48'),
(11, 'lop-tieng-nhat-giao-tiep-nang-cao-2', 'Lớp tiếng Nhật giao tiếp nâng cao 2', 5, 4, NULL, 6, '2026-03-08', '2026-04-30', 15, 25, 200000.00, '6,CN', 1, 1, 1, '2026-03-01 16:20:53', '2026-03-01 16:25:16');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `luong`
--

CREATE TABLE `luong` (
  `luongId` int(11) NOT NULL,
  `taiKhoanId` int(11) DEFAULT NULL,
  `thangLuong` varchar(10) DEFAULT NULL,
  `tongLuongDay` decimal(15,2) DEFAULT NULL,
  `thuong` decimal(15,2) DEFAULT 0.00,
  `phat` decimal(15,2) DEFAULT 0.00,
  `phuCap` decimal(15,2) DEFAULT 0.00,
  `tongTienThucLanh` decimal(15,2) DEFAULT NULL,
  `tongBuoiDay` int(11) DEFAULT NULL,
  `ngayThanhToan` date DEFAULT NULL,
  `phuongThucThanhToan` tinyint(4) DEFAULT NULL,
  `ghiChu` text DEFAULT NULL,
  `trangThai` tinyint(4) DEFAULT NULL,
  `created_at` datetime DEFAULT current_timestamp(),
  `updated_at` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `luongchitiet`
--

CREATE TABLE `luongchitiet` (
  `luongChiTietId` int(11) NOT NULL,
  `luongId` int(11) DEFAULT NULL,
  `lopHocId` int(11) DEFAULT NULL,
  `soBuoiDay` int(11) DEFAULT NULL,
  `donGiaMotBuoi` decimal(15,2) DEFAULT NULL,
  `tongTien` decimal(15,2) DEFAULT NULL,
  `created_at` datetime DEFAULT current_timestamp(),
  `updated_at` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `migrations`
--

CREATE TABLE `migrations` (
  `id` int(10) UNSIGNED NOT NULL,
  `migration` varchar(255) NOT NULL,
  `batch` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `migrations`
--

INSERT INTO `migrations` (`id`, `migration`, `batch`) VALUES
(1, '0001_01_01_000000_create_users_table', 1),
(2, '0001_01_01_000001_create_cache_table', 1),
(3, '0001_01_01_000002_create_jobs_table', 1),
(4, '2026_02_21_224413_add_ma_api_to_tinhthanh_table', 2),
(5, '2026_02_21_224430_add_address_fields_to_cosodaotao_table', 3),
(6, '2026_02_26_152345_add_soft_delete_to_khoahoc_table', 4),
(7, '2026_02_28_162518_add_new_fields_to_thongbao_table', 5),
(8, '2026_03_01_153435_add_fields_to_hoadon_phieuthu_tables', 6);

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `nhansu`
--

CREATE TABLE `nhansu` (
  `taiKhoanId` int(11) NOT NULL,
  `maNhanVien` varchar(20) DEFAULT NULL,
  `chucVu` varchar(50) DEFAULT NULL,
  `luongCoBan` decimal(15,2) DEFAULT NULL,
  `ngayVaoLam` date DEFAULT NULL,
  `chuyenMon` varchar(255) DEFAULT NULL,
  `bangCap` varchar(255) DEFAULT NULL,
  `hocVi` varchar(100) DEFAULT NULL,
  `coSoId` int(11) DEFAULT NULL,
  `loaiHopDong` varchar(255) DEFAULT NULL,
  `trangThai` tinyint(4) DEFAULT NULL COMMENT '0: đang làm, 1: tạm nghỉ, 2: đã nghỉ việc',
  `created_at` datetime DEFAULT current_timestamp(),
  `updated_at` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `nhansu`
--

INSERT INTO `nhansu` (`taiKhoanId`, `maNhanVien`, `chucVu`, `luongCoBan`, `ngayVaoLam`, `chuyenMon`, `bangCap`, `hocVi`, `coSoId`, `loaiHopDong`, `trangThai`, `created_at`, `updated_at`) VALUES
(4, 'GV001', 'Giáo viên bản ngữ', 30000000.00, '2024-01-10', 'IELTS Speaking', 'CELTA', 'Đại học', 1, 'Chính thức', 0, '2026-01-23 20:07:12', '2026-01-26 19:52:29'),
(5, 'GV002', 'Giáo viên VN', 18000000.00, '2024-02-01', 'Grammar', 'IELTS 8.5', 'Thạc Sĩ', 1, 'Chính thức', 0, '2026-01-23 20:07:12', '2026-02-21 23:45:00'),
(26, NULL, 'Giáo viên', NULL, '2026-02-25', 'Tiếng Anh', 'Cử nhân', NULL, 13, 'Toàn thời gian', 1, '2026-02-25 18:37:41', '2026-02-25 18:37:41');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `nhomquyen`
--

CREATE TABLE `nhomquyen` (
  `nhomQuyenId` int(11) UNSIGNED NOT NULL,
  `tenNhom` varchar(100) NOT NULL COMMENT 'VD: Kế toán, Nhân sự, Tư vấn học vụ',
  `moTa` varchar(255) DEFAULT NULL COMMENT 'Mô tả nhóm quyền',
  `created_at` datetime NOT NULL DEFAULT current_timestamp(),
  `updated_at` datetime NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `nhomquyen`
--

INSERT INTO `nhomquyen` (`nhomQuyenId`, `tenNhom`, `moTa`, `created_at`, `updated_at`) VALUES
(5, 'Kế toán', NULL, '2026-02-20 09:54:28', '2026-02-20 09:54:28'),
(6, 'Giáo viên', NULL, '2026-02-22 11:29:32', '2026-02-22 11:29:32'),
(7, 'Tài chính', NULL, '2026-02-27 14:22:41', '2026-02-27 14:22:41');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `noidungbaihoc`
--

CREATE TABLE `noidungbaihoc` (
  `noiDungId` bigint(20) NOT NULL,
  `buoiHocId` int(11) DEFAULT NULL,
  `tieuDe` varchar(255) DEFAULT NULL,
  `noiDung` text DEFAULT NULL,
  `taiLieuId` varchar(50) DEFAULT NULL,
  `created_at` datetime DEFAULT current_timestamp(),
  `updated_at` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `password_reset_tokens`
--

CREATE TABLE `password_reset_tokens` (
  `email` varchar(255) NOT NULL,
  `token` varchar(255) NOT NULL,
  `created_at` timestamp NULL DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `password_reset_tokens`
--

INSERT INTO `password_reset_tokens` (`email`, `token`, `created_at`) VALUES
('levanhaum@gmail.com', '$2y$12$bL5mTSObEDCXF0GkMa9zT.K/wO3js71B/P/4Wg4UbkT3e5AXedRj6', '2026-02-19 04:49:18');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `phanhoi`
--

CREATE TABLE `phanhoi` (
  `phanHoiId` int(11) NOT NULL,
  `tieuDe` varchar(255) DEFAULT NULL,
  `noiDung` text DEFAULT NULL,
  `taiKhoanId` int(11) DEFAULT NULL,
  `danhGia` tinyint(4) DEFAULT NULL,
  `buoiHocId` int(11) DEFAULT NULL,
  `trangThai` tinyint(4) DEFAULT NULL,
  `created_at` datetime DEFAULT current_timestamp(),
  `updated_at` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `phanquyen`
--

CREATE TABLE `phanquyen` (
  `phanQuyenId` int(11) UNSIGNED NOT NULL,
  `nhomQuyenId` int(11) UNSIGNED NOT NULL,
  `tinhNang` varchar(50) NOT NULL COMMENT 'khoa_hoc | lop_hoc | hoc_vien | giao_vien | nhan_vien | tai_chinh | dang_ky | tai_khoan | cai_dat',
  `coXem` tinyint(1) NOT NULL DEFAULT 0,
  `coThem` tinyint(1) NOT NULL DEFAULT 0,
  `coSua` tinyint(1) NOT NULL DEFAULT 0,
  `coXoa` tinyint(1) NOT NULL DEFAULT 0,
  `created_at` datetime NOT NULL DEFAULT current_timestamp(),
  `updated_at` datetime NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `phanquyen`
--

INSERT INTO `phanquyen` (`phanQuyenId`, `nhomQuyenId`, `tinhNang`, `coXem`, `coThem`, `coSua`, `coXoa`, `created_at`, `updated_at`) VALUES
(40, 6, 'khoa_hoc', 0, 0, 0, 0, '2026-02-22 17:56:23', '2026-02-22 17:56:23'),
(41, 6, 'lop_hoc', 1, 0, 0, 0, '2026-02-22 17:56:23', '2026-02-22 17:56:23'),
(42, 6, 'hoc_vien', 1, 1, 0, 0, '2026-02-22 17:56:23', '2026-02-22 17:56:23'),
(43, 6, 'giao_vien', 0, 0, 0, 0, '2026-02-22 17:56:23', '2026-02-22 17:56:23'),
(44, 6, 'nhan_vien', 0, 0, 0, 0, '2026-02-22 17:56:23', '2026-02-22 17:56:23'),
(45, 6, 'tai_chinh', 0, 0, 0, 0, '2026-02-22 17:56:23', '2026-02-22 17:56:23'),
(46, 6, 'dang_ky', 0, 0, 0, 0, '2026-02-22 17:56:23', '2026-02-22 17:56:23'),
(47, 6, 'tai_khoan', 0, 0, 0, 0, '2026-02-22 17:56:23', '2026-02-22 17:56:23'),
(48, 6, 'cai_dat', 0, 0, 0, 0, '2026-02-22 17:56:23', '2026-02-22 17:56:23'),
(49, 5, 'khoa_hoc', 1, 1, 0, 0, '2026-02-27 14:00:47', '2026-02-27 14:00:47'),
(50, 5, 'lop_hoc', 1, 0, 0, 0, '2026-02-27 14:00:47', '2026-02-27 14:00:47'),
(51, 5, 'hoc_vien', 0, 0, 0, 0, '2026-02-27 14:00:47', '2026-02-27 14:00:47'),
(52, 5, 'giao_vien', 0, 0, 0, 0, '2026-02-27 14:00:47', '2026-02-27 14:00:47'),
(53, 5, 'nhan_vien', 0, 0, 0, 0, '2026-02-27 14:00:47', '2026-02-27 14:00:47'),
(54, 5, 'tai_chinh', 0, 0, 0, 0, '2026-02-27 14:00:47', '2026-02-27 14:00:47'),
(55, 5, 'dang_ky', 1, 1, 1, 1, '2026-02-27 14:00:47', '2026-02-27 14:00:47'),
(56, 5, 'tai_khoan', 0, 0, 0, 0, '2026-02-27 14:00:47', '2026-02-27 14:00:47'),
(57, 5, 'cai_dat', 0, 0, 0, 0, '2026-02-27 14:00:47', '2026-02-27 14:00:47'),
(58, 7, 'khoa_hoc', 1, 0, 0, 0, '2026-02-27 14:22:41', '2026-02-27 14:22:41'),
(59, 7, 'lop_hoc', 1, 0, 0, 0, '2026-02-27 14:22:41', '2026-02-27 14:22:41'),
(60, 7, 'hoc_vien', 1, 0, 0, 0, '2026-02-27 14:22:41', '2026-02-27 14:22:41'),
(61, 7, 'giao_vien', 1, 0, 0, 0, '2026-02-27 14:22:41', '2026-02-27 14:22:41'),
(62, 7, 'nhan_vien', 1, 0, 0, 0, '2026-02-27 14:22:41', '2026-02-27 14:22:41'),
(63, 7, 'tai_chinh', 1, 1, 1, 1, '2026-02-27 14:22:41', '2026-02-27 14:22:41'),
(64, 7, 'dang_ky', 0, 0, 0, 0, '2026-02-27 14:22:41', '2026-02-27 14:22:41'),
(65, 7, 'tai_khoan', 0, 0, 0, 0, '2026-02-27 14:22:41', '2026-02-27 14:22:41'),
(66, 7, 'cai_dat', 0, 0, 0, 0, '2026-02-27 14:22:41', '2026-02-27 14:22:41');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `phieuthu`
--

CREATE TABLE `phieuthu` (
  `phieuThuId` bigint(20) NOT NULL,
  `maPhieuThu` varchar(20) DEFAULT NULL,
  `hoaDonId` int(11) DEFAULT NULL,
  `soTien` decimal(15,2) DEFAULT NULL,
  `phuongThucThanhToan` tinyint(4) NOT NULL DEFAULT 1 COMMENT '1=Tiền mặt, 2=Chuyển khoản, 3=VNPay',
  `ngayThu` date DEFAULT NULL,
  `taiKhoanId` int(11) DEFAULT NULL COMMENT 'người thu',
  `nguoiDuyetId` bigint(20) UNSIGNED DEFAULT NULL,
  `ghiChu` text DEFAULT NULL,
  `trangThai` tinyint(4) NOT NULL DEFAULT 1 COMMENT '0=Hủy, 1=Hợp lệ',
  `created_at` datetime DEFAULT current_timestamp(),
  `updated_at` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `phieuthu`
--

INSERT INTO `phieuthu` (`phieuThuId`, `maPhieuThu`, `hoaDonId`, `soTien`, `phuongThucThanhToan`, `ngayThu`, `taiKhoanId`, `nguoiDuyetId`, `ghiChu`, `trangThai`, `created_at`, `updated_at`) VALUES
(3, 'PT-202603-000001', 14, 575000.00, 2, '2026-03-01', 1, 1, 'Phiếu 1, ngày 1.3', 1, '2026-03-01 15:58:39', '2026-03-01 15:58:39'),
(4, 'PT-202603-000002', 14, 100000.00, 1, '2026-03-01', 1, 1, 'Phiếu 2, lần 2', 1, '2026-03-01 15:59:39', '2026-03-01 15:59:39');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `phonghoc`
--

CREATE TABLE `phonghoc` (
  `phongHocId` int(11) NOT NULL,
  `tenPhong` varchar(100) DEFAULT NULL,
  `sucChua` int(11) DEFAULT NULL,
  `trangThietBi` text DEFAULT NULL,
  `coSoId` int(11) DEFAULT NULL,
  `trangThai` int(11) DEFAULT 1,
  `created_at` datetime DEFAULT current_timestamp(),
  `updated_at` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `phonghoc`
--

INSERT INTO `phonghoc` (`phongHocId`, `tenPhong`, `sucChua`, `trangThietBi`, `coSoId`, `trangThai`, `created_at`, `updated_at`) VALUES
(3, 'P101', 50, 'tivi, điều hòa, loa', 13, 1, '2026-02-26 15:29:05', '2026-02-26 15:29:05'),
(4, 'P201', 40, 'tivi, điều hòa, loa', 1, 1, '2026-02-26 15:44:08', '2026-02-26 15:44:08'),
(5, 'Phòng 101 - Q1', 40, NULL, 14, 1, '2026-02-27 14:27:39', '2026-02-27 14:27:39');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `sessions`
--

CREATE TABLE `sessions` (
  `id` varchar(255) NOT NULL,
  `user_id` bigint(20) UNSIGNED DEFAULT NULL,
  `ip_address` varchar(45) DEFAULT NULL,
  `user_agent` text DEFAULT NULL,
  `payload` longtext NOT NULL,
  `last_activity` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `sessions`
--

INSERT INTO `sessions` (`id`, `user_id`, `ip_address`, `user_agent`, `payload`, `last_activity`) VALUES
('6063HQFb6106prGhMohW6NQRGh0z9ImpUrUd3vXq', NULL, '127.0.0.1', 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/145.0.0.0 Safari/537.36', 'YTo0OntzOjY6Il90b2tlbiI7czo0MDoiVjhUY3ZBbUo4R25iRERUVmlZNmtCZ2RsdFNYZzdSNkJENDk0cFlrdSI7czozOiJ1cmwiO2E6MTp7czo4OiJpbnRlbmRlZCI7czozNToiaHR0cDovL2xvY2FsaG9zdDo4MDAwL2FkbWluL2hvYS1kb24iO31zOjk6Il9wcmV2aW91cyI7YToyOntzOjM6InVybCI7czoyNzoiaHR0cDovL2xvY2FsaG9zdDo4MDAwL2xvZ2luIjtzOjU6InJvdXRlIjtzOjU6ImxvZ2luIjt9czo2OiJfZmxhc2giO2E6Mjp7czozOiJvbGQiO2E6MDp7fXM6MzoibmV3IjthOjA6e319fQ==', 1772355667),
('JyQeSw76RAMrMqXmmb35LCqkXN2xDaOrsoJwIu74', 23, '127.0.0.1', 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/145.0.0.0 Safari/537.36', 'YTo1OntzOjY6Il90b2tlbiI7czo0MDoiSFhMSTFpWTRGU3hxTGVHTVdFS01JQzFidWR6TzlXZjJhRFV3N1poNyI7czo2OiJfZmxhc2giO2E6Mjp7czozOiJvbGQiO2E6MDp7fXM6MzoibmV3IjthOjA6e319czo5OiJfcHJldmlvdXMiO2E6Mjp7czozOiJ1cmwiO3M6NDQ6Imh0dHA6Ly8xMjcuMC4wLjE6ODAwMC9hcGkvdGhvbmctYmFvL2NodWEtZG9jIjtzOjU6InJvdXRlIjtzOjMxOiJob21lLmFwaS50aG9uZy1iYW8udW5yZWFkLWNvdW50Ijt9czo1MDoibG9naW5fd2ViXzU5YmEzNmFkZGMyYjJmOTQwMTU4MGYwMTRjN2Y1OGVhNGUzMDk4OWQiO2k6MjM7czo0OiJhdXRoIjthOjE6e3M6MjE6InBhc3N3b3JkX2NvbmZpcm1lZF9hdCI7aToxNzcyMzU2MjczO319', 1772357424),
('k1SVSQRALsexh6sVkO8sZpDD2n919PbgXe20dkfF', 1, '127.0.0.1', 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/145.0.0.0 Safari/537.36', 'YTo1OntzOjY6Il90b2tlbiI7czo0MDoiZVVyb0lUTncwSGRCME84U2U4TWdsWEhocXJZU090MkFqSFpsTkNMWCI7czo5OiJfcHJldmlvdXMiO2E6Mjp7czozOiJ1cmwiO3M6NTA6Imh0dHA6Ly8xMjcuMC4wLjE6ODAwMC9hZG1pbi9hcGkvdGhvbmctYmFvL2Ryb3Bkb3duIjtzOjU6InJvdXRlIjtzOjI4OiJhZG1pbi5hcGkudGhvbmctYmFvLmRyb3Bkb3duIjt9czo2OiJfZmxhc2giO2E6Mjp7czozOiJvbGQiO2E6MDp7fXM6MzoibmV3IjthOjA6e319czo1MDoibG9naW5fd2ViXzU5YmEzNmFkZGMyYjJmOTQwMTU4MGYwMTRjN2Y1OGVhNGUzMDk4OWQiO2k6MTtzOjQ6ImF1dGgiO2E6MTp7czoyMToicGFzc3dvcmRfY29uZmlybWVkX2F0IjtpOjE3NzIzNTYzMzg7fX0=', 1772357417);

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `settings`
--

CREATE TABLE `settings` (
  `id` bigint(20) UNSIGNED NOT NULL,
  `key` varchar(191) NOT NULL,
  `value` text DEFAULT NULL,
  `display_name` varchar(191) NOT NULL,
  `type` varchar(50) NOT NULL DEFAULT 'text',
  `group` varchar(50) NOT NULL DEFAULT 'general',
  `created_at` timestamp NULL DEFAULT current_timestamp(),
  `updated_at` timestamp NULL DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `settings`
--

INSERT INTO `settings` (`id`, `key`, `value`, `display_name`, `type`, `group`, `created_at`, `updated_at`) VALUES
(1, 'site_name', 'Five Genius Academy', 'Tên trung tâm', 'text', 'general', '2026-01-28 11:30:17', '2026-01-28 11:30:17'),
(2, 'site_phone', '0777464347', 'Số điện thoại', 'text', 'general', '2026-01-28 11:30:17', '2026-01-28 11:38:29'),
(3, 'site_email', 'contact@fivegenius.edu.vn', 'Email liên hệ', 'text', 'general', '2026-01-28 11:30:17', '2026-01-28 11:30:17'),
(4, 'site_address', '27G1 Nguyễn Oanh, Phường 7, TP. Vũng Tàu', 'Địa chỉ', 'textarea', 'general', '2026-01-28 11:30:17', '2026-01-28 11:30:17'),
(5, 'site_logo', 'uploads/settings/logo.png', 'Logo Website', 'image', 'general', '2026-01-28 11:30:17', '2026-01-28 11:30:17'),
(6, 'site_favicon', 'uploads/settings/favicon.ico', 'Favicon', 'image', 'general', '2026-01-28 11:30:17', '2026-01-28 11:30:17'),
(7, 'facebook_url', 'https://facebook.com/fivegenius', 'Link Facebook', 'text', 'social', '2026-01-28 11:30:17', '2026-01-28 11:30:17'),
(8, 'youtube_url', 'https://youtube.com/@fivegenius', 'Link Youtube', 'text', 'social', '2026-01-28 11:30:17', '2026-01-28 11:30:17'),
(9, 'tiktok_url', 'https://tiktok.com/@fivegenius', 'Link Tiktok', 'text', 'social', '2026-01-28 11:30:17', '2026-01-28 11:30:17'),
(10, 'meta_title', 'Five Genius Academy - Trung tâm Anh ngữ hàng đầu', 'Tiêu đề SEO', 'text', 'seo', '2026-01-28 11:30:17', '2026-01-28 11:30:17'),
(11, 'meta_description', 'Đào tạo IELTS, SAT, tiếng Anh giao tiếp chuyên nghiệp với lộ trình cá nhân hóa.', 'Mô tả SEO', 'textarea', 'seo', '2026-01-28 11:30:17', '2026-01-28 11:30:17'),
(12, 'google_analytics_id', 'UA-XXXXX-Y', 'Mã Google Analytics', 'text', 'seo', '2026-01-28 11:30:17', '2026-01-28 11:30:17'),
(13, 'dark_mode', '0', 'Chế độ nền tối (1: Bật, 0: Tắt)', 'text', 'theme', '2026-01-28 11:30:17', '2026-01-28 11:30:17'),
(14, 'primary_color', '#e32c2d', 'Màu chủ đạo (Navy)', 'text', 'theme', '2026-01-28 11:30:17', '2026-01-28 11:42:43'),
(15, 'accent_color', '#E31E24', 'Màu nhấn (Đỏ)', 'text', 'theme', '2026-01-28 11:30:17', '2026-01-28 11:30:17');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `tags`
--

CREATE TABLE `tags` (
  `tagId` bigint(20) UNSIGNED NOT NULL,
  `tenTag` varchar(50) NOT NULL,
  `slug` varchar(50) NOT NULL,
  `created_at` timestamp NULL DEFAULT current_timestamp(),
  `updated_at` timestamp NULL DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `tags`
--

INSERT INTO `tags` (`tagId`, `tenTag`, `slug`, `created_at`, `updated_at`) VALUES
(1, 'IELTS', 'ielts', '2026-01-27 06:11:36', NULL),
(2, 'TOEIC', 'toeic', '2026-01-27 06:11:36', NULL),
(3, 'Giao tiếp', 'giao-tiep', '2026-01-27 06:11:36', NULL),
(4, 'Ngữ pháp', 'ngu-phap', '2026-01-27 06:11:36', NULL),
(5, 'Từ vựng', 'tu-vung', '2026-01-27 06:11:36', NULL),
(6, 'Listening', 'listening', '2026-01-27 06:11:36', NULL),
(7, 'Speaking', 'speaking', '2026-01-27 06:11:36', NULL),
(8, 'Reading', 'reading', '2026-01-27 06:11:36', NULL),
(9, 'Writing', 'writing', '2026-01-27 06:11:36', NULL),
(10, 'danhgia', 'danhgia', '2026-02-28 08:31:41', '2026-02-28 08:31:41'),
(11, 'review', 'review', '2026-02-28 08:31:41', '2026-02-28 08:31:41'),
(12, 'hocvien', 'hocvien', '2026-02-28 08:31:41', '2026-02-28 08:31:41'),
(13, 'xin chào', 'xin-chao', '2026-02-28 08:47:32', '2026-02-28 08:47:32');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `taikhoan`
--

CREATE TABLE `taikhoan` (
  `taiKhoanId` int(11) NOT NULL,
  `taiKhoan` varchar(50) NOT NULL,
  `matKhau` varchar(255) NOT NULL,
  `email` varchar(50) DEFAULT NULL,
  `role` tinyint(4) NOT NULL COMMENT '0: học viên, 1: giáo viên, 2: nhân viên, 3: admin',
  `nhomQuyenId` int(11) UNSIGNED DEFAULT NULL COMMENT 'Chỉ dùng cho role 1 (GV) và 2 (NV)',
  `trangThai` tinyint(4) DEFAULT 1 COMMENT '0: Khóa, 1: Hoạt động, 2: Chờ kích hoạt',
  `remember_token` varchar(100) DEFAULT NULL,
  `lastLogin` datetime DEFAULT NULL,
  `created_at` datetime DEFAULT current_timestamp(),
  `updated_at` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `deleted_at` timestamp NULL DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `taikhoan`
--

INSERT INTO `taikhoan` (`taiKhoanId`, `taiKhoan`, `matKhau`, `email`, `role`, `nhomQuyenId`, `trangThai`, `remember_token`, `lastLogin`, `created_at`, `updated_at`, `deleted_at`) VALUES
(1, 'admin', '$2y$12$R12EhHjbvn9YPWfel4EVXO4Q4IF8u60X//oZvBmy.vSQ3AWjXB3AO', 'levanhaum@gmail.com', 3, NULL, 1, 'ylV0rtSxOKIJGFuF4qzwdeBBZi1JZUa5Ob2yaceEmTrSZdIwACHDQ3busYQA', '2026-03-01 16:12:18', '2026-01-23 20:07:12', '2026-03-01 16:12:18', NULL),
(4, 'gv_john_smith', '$2y$10$abcdef', 'Levanhaum11@gmail.com', 1, 6, 0, NULL, NULL, '2026-01-23 20:07:12', '2026-02-25 18:18:17', NULL),
(5, 'gv_le_hoa', '$2y$12$X76PLtZH03IrMNVYcJMCGuhR7iwOGUI9onnYLmZCICcWDEic8Ux7S', 'Levanhaum10@gmail.com', 1, 6, 1, NULL, '2026-02-28 17:16:44', '2026-01-23 20:07:12', '2026-02-28 17:16:44', NULL),
(22, 'Levanhaum1@gmail.com', '$2y$12$1IY6Um0/pOUrYWknZESQmeP8aPcuEiIo.p3FBmrvlqsp7ck7bRYLy', 'Levanhaum1@gmail.com', 0, NULL, 1, NULL, '2026-02-21 11:37:31', '2026-02-21 11:30:47', '2026-02-21 11:54:20', '2026-02-21 04:54:20'),
(23, 'User_049204011849', '$2y$12$Jl5dI7CO8vRlQIaEI5Rlh.6sTKQXeFSbBZ.9w70Lqjzv9ef0k368u', 'levanhau2@gmail.com', 0, NULL, 1, NULL, '2026-03-01 16:11:13', '2026-02-21 11:45:30', '2026-03-01 16:11:13', NULL),
(26, 'User_049204011848', '$2y$12$LAlX9l/zGiYLnfbl6eQ9SeNjv5zpUWLyojiwHcc3VQSQydH/rWv9e', 'Levanhaugv@gmail.com', 1, 6, 1, NULL, '2026-02-27 17:34:21', '2026-02-25 18:37:41', '2026-02-27 17:34:21', NULL),
(27, 'User_094949494944', '$2y$12$u.Ndwx0ohXL8X6RoRZmh..SH7X.9Zw4z4BwB2/t4QisGKQlm25Qja', 'lengocanhk4@gmail.com', 0, NULL, 1, NULL, NULL, '2026-02-27 14:20:38', '2026-02-27 14:20:38', NULL);

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `tailieu`
--

CREATE TABLE `tailieu` (
  `taiLieuId` varchar(50) NOT NULL,
  `tenTaiLieu` varchar(255) DEFAULT NULL,
  `moTa` text DEFAULT NULL,
  `loaiTaiLieu` varchar(50) DEFAULT NULL,
  `taiKhoanId` int(11) DEFAULT NULL,
  `duongDan` varchar(255) DEFAULT NULL,
  `khoaHocId` int(11) DEFAULT NULL,
  `buoiHocId` int(11) DEFAULT NULL,
  `created_at` datetime DEFAULT current_timestamp(),
  `updated_at` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `thongbao`
--

CREATE TABLE `thongbao` (
  `thongBaoId` bigint(20) NOT NULL,
  `tieuDe` varchar(255) DEFAULT NULL,
  `noiDung` text DEFAULT NULL,
  `nguoiGuiId` int(11) DEFAULT NULL,
  `loaiThongBao` tinyint(4) DEFAULT NULL,
  `doiTuongGui` tinyint(4) DEFAULT NULL COMMENT '0-Tất cả, 1-Theo lớp, 2-Theo khóa học, 3-Cá nhân',
  `doiTuongId` bigint(20) DEFAULT NULL,
  `ngayGui` datetime DEFAULT current_timestamp(),
  `trangThai` tinyint(4) DEFAULT NULL,
  `loaiGui` tinyint(4) NOT NULL DEFAULT 0 COMMENT '0=Hệ thống,1=Học tập,2=Tài chính,3=Sự kiện,4=Khẩn cấp',
  `uuTien` tinyint(4) NOT NULL DEFAULT 0 COMMENT '0=Bình thường,1=Quan trọng,2=Khẩn cấp',
  `ghim` tinyint(1) NOT NULL DEFAULT 0,
  `hinhAnh` varchar(255) DEFAULT NULL,
  `created_at` datetime DEFAULT current_timestamp(),
  `updated_at` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `thongbao`
--

INSERT INTO `thongbao` (`thongBaoId`, `tieuDe`, `noiDung`, `nguoiGuiId`, `loaiThongBao`, `doiTuongGui`, `doiTuongId`, `ngayGui`, `trangThai`, `loaiGui`, `uuTien`, `ghim`, `hinhAnh`, `created_at`, `updated_at`) VALUES
(2, 'Thông báo lịch bắt đầu đi học', '<p><strong>XIn chào cả nhà</strong></p><p><a href=\"http://127.0.0.1:8000/blog\" target=\"_blank\">Tôi là admin, ngày 01/03/2026 bắt đầu đi học trở lại</a></p>', 1, NULL, 0, NULL, '2026-02-28 16:40:28', 1, 1, 1, 1, NULL, '2026-02-28 16:40:28', '2026-02-28 16:45:47'),
(4, 'test thông báo 01', '<p>Nội dung</p>', 1, NULL, 4, 0, '2026-02-28 17:17:57', 1, 1, 0, 0, NULL, '2026-02-28 17:17:57', '2026-02-28 17:17:57');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `thongbaonguoidung`
--

CREATE TABLE `thongbaonguoidung` (
  `thongBaoNguoiDungId` bigint(20) NOT NULL,
  `thongBaoId` bigint(20) DEFAULT NULL,
  `taiKhoanId` int(11) DEFAULT NULL,
  `daDoc` tinyint(4) DEFAULT 0,
  `ngayDoc` datetime DEFAULT NULL,
  `created_at` datetime DEFAULT current_timestamp(),
  `updated_at` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `thongbaonguoidung`
--

INSERT INTO `thongbaonguoidung` (`thongBaoNguoiDungId`, `thongBaoId`, `taiKhoanId`, `daDoc`, `ngayDoc`, `created_at`, `updated_at`) VALUES
(1, 2, 5, 0, NULL, '2026-02-28 16:40:28', '2026-02-28 16:40:28'),
(2, 2, 23, 1, '2026-02-28 17:13:27', '2026-02-28 16:40:28', '2026-02-28 17:13:27'),
(3, 2, 26, 0, NULL, '2026-02-28 16:40:28', '2026-02-28 16:40:28'),
(4, 2, 27, 0, NULL, '2026-02-28 16:40:28', '2026-02-28 16:40:28'),
(6, 4, 23, 1, '2026-02-28 17:18:42', '2026-02-28 17:17:57', '2026-02-28 17:18:42'),
(7, 4, 27, 0, NULL, '2026-02-28 17:17:57', '2026-02-28 17:17:57');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `tinhthanh`
--

CREATE TABLE `tinhthanh` (
  `tinhThanhId` int(11) NOT NULL,
  `maAPI` int(10) UNSIGNED DEFAULT NULL,
  `tenTinhThanh` varchar(100) NOT NULL,
  `slug` varchar(100) NOT NULL,
  `division_type` varchar(50) DEFAULT NULL,
  `codename` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `tinhthanh`
--

INSERT INTO `tinhthanh` (`tinhThanhId`, `maAPI`, `tenTinhThanh`, `slug`, `division_type`, `codename`) VALUES
(190, 1, 'Thành phố Hà Nội', 'ha_noi', 'thành phố trung ương', 'ha_noi'),
(191, 4, 'Tỉnh Cao Bằng', 'cao_bang', 'tỉnh', 'cao_bang'),
(192, 8, 'Tỉnh Tuyên Quang', 'tuyen_quang', 'tỉnh', 'tuyen_quang'),
(193, 11, 'Tỉnh Điện Biên', 'dien_bien', 'tỉnh', 'dien_bien'),
(194, 12, 'Tỉnh Lai Châu', 'lai_chau', 'tỉnh', 'lai_chau'),
(195, 14, 'Tỉnh Sơn La', 'son_la', 'tỉnh', 'son_la'),
(196, 15, 'Tỉnh Lào Cai', 'lao_cai', 'tỉnh', 'lao_cai'),
(197, 19, 'Tỉnh Thái Nguyên', 'thai_nguyen', 'tỉnh', 'thai_nguyen'),
(198, 20, 'Tỉnh Lạng Sơn', 'lang_son', 'tỉnh', 'lang_son'),
(199, 22, 'Tỉnh Quảng Ninh', 'quang_ninh', 'tỉnh', 'quang_ninh'),
(200, 24, 'Tỉnh Bắc Ninh', 'bac_ninh', 'tỉnh', 'bac_ninh'),
(201, 25, 'Tỉnh Phú Thọ', 'phu_tho', 'tỉnh', 'phu_tho'),
(202, 31, 'Thành phố Hải Phòng', 'hai_phong', 'thành phố trung ương', 'hai_phong'),
(203, 33, 'Tỉnh Hưng Yên', 'hung_yen', 'tỉnh', 'hung_yen'),
(204, 37, 'Tỉnh Ninh Bình', 'ninh_binh', 'tỉnh', 'ninh_binh'),
(205, 38, 'Tỉnh Thanh Hóa', 'thanh_hoa', 'tỉnh', 'thanh_hoa'),
(206, 40, 'Tỉnh Nghệ An', 'nghe_an', 'tỉnh', 'nghe_an'),
(207, 42, 'Tỉnh Hà Tĩnh', 'ha_tinh', 'tỉnh', 'ha_tinh'),
(208, 44, 'Tỉnh Quảng Trị', 'quang_tri', 'tỉnh', 'quang_tri'),
(209, 46, 'Thành phố Huế', 'hue', 'thành phố trung ương', 'hue'),
(210, 48, 'Thành phố Đà Nẵng', 'da_nang', 'thành phố trung ương', 'da_nang'),
(211, 51, 'Tỉnh Quảng Ngãi', 'quang_ngai', 'tỉnh', 'quang_ngai'),
(212, 52, 'Tỉnh Gia Lai', 'gia_lai', 'tỉnh', 'gia_lai'),
(213, 56, 'Tỉnh Khánh Hòa', 'khanh_hoa', 'tỉnh', 'khanh_hoa'),
(214, 66, 'Tỉnh Đắk Lắk', 'dak_lak', 'tỉnh', 'dak_lak'),
(215, 68, 'Tỉnh Lâm Đồng', 'lam_dong', 'tỉnh', 'lam_dong'),
(216, 75, 'Tỉnh Đồng Nai', 'dong_nai', 'tỉnh', 'dong_nai'),
(217, 79, 'Thành phố Hồ Chí Minh', 'ho_chi_minh', 'thành phố trung ương', 'ho_chi_minh'),
(218, 80, 'Tỉnh Tây Ninh', 'tay_ninh', 'tỉnh', 'tay_ninh'),
(219, 82, 'Tỉnh Đồng Tháp', 'dong_thap', 'tỉnh', 'dong_thap'),
(220, 86, 'Tỉnh Vĩnh Long', 'vinh_long', 'tỉnh', 'vinh_long'),
(221, 91, 'Tỉnh An Giang', 'an_giang', 'tỉnh', 'an_giang'),
(222, 92, 'Thành phố Cần Thơ', 'can_tho', 'thành phố trung ương', 'can_tho'),
(223, 96, 'Tỉnh Cà Mau', 'ca_mau', 'tỉnh', 'ca_mau');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `users`
--

CREATE TABLE `users` (
  `id` bigint(20) UNSIGNED NOT NULL,
  `name` varchar(255) NOT NULL,
  `email` varchar(255) NOT NULL,
  `email_verified_at` timestamp NULL DEFAULT NULL,
  `password` varchar(255) NOT NULL,
  `remember_token` varchar(100) DEFAULT NULL,
  `created_at` timestamp NULL DEFAULT NULL,
  `updated_at` timestamp NULL DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Chỉ mục cho các bảng đã đổ
--

--
-- Chỉ mục cho bảng `baithi`
--
ALTER TABLE `baithi`
  ADD PRIMARY KEY (`baiThiId`),
  ADD KEY `fk_baithi_khoa` (`khoaHocId`);

--
-- Chỉ mục cho bảng `baiviet`
--
ALTER TABLE `baiviet`
  ADD PRIMARY KEY (`baiVietId`),
  ADD UNIQUE KEY `slug` (`slug`),
  ADD KEY `fk_baiviet_taikhoan` (`taiKhoanId`);

--
-- Chỉ mục cho bảng `baiviet_danhmuc`
--
ALTER TABLE `baiviet_danhmuc`
  ADD PRIMARY KEY (`baiVietId`,`danhMucId`),
  ADD KEY `fk_bvdm_danhmuc` (`danhMucId`);

--
-- Chỉ mục cho bảng `baiviet_tag`
--
ALTER TABLE `baiviet_tag`
  ADD PRIMARY KEY (`baiVietId`,`tagId`),
  ADD KEY `fk_bvt_tag` (`tagId`);

--
-- Chỉ mục cho bảng `buoihoc`
--
ALTER TABLE `buoihoc`
  ADD PRIMARY KEY (`buoiHocId`),
  ADD KEY `fk_buoi_gv` (`taiKhoanId`),
  ADD KEY `fk_buoi_phong` (`phongHocId`),
  ADD KEY `fk_buoi_ca` (`caHocId`),
  ADD KEY `fk_buoi_lich` (`lopHocId`);

--
-- Chỉ mục cho bảng `cache`
--
ALTER TABLE `cache`
  ADD PRIMARY KEY (`key`),
  ADD KEY `cache_expiration_index` (`expiration`);

--
-- Chỉ mục cho bảng `cache_locks`
--
ALTER TABLE `cache_locks`
  ADD PRIMARY KEY (`key`),
  ADD KEY `cache_locks_expiration_index` (`expiration`);

--
-- Chỉ mục cho bảng `cahoc`
--
ALTER TABLE `cahoc`
  ADD PRIMARY KEY (`caHocId`);

--
-- Chỉ mục cho bảng `cosodaotao`
--
ALTER TABLE `cosodaotao`
  ADD PRIMARY KEY (`coSoId`),
  ADD UNIQUE KEY `maCoSo` (`maCoSo`),
  ADD UNIQUE KEY `slug` (`slug`),
  ADD KEY `fk_coso_tinhthanh` (`tinhThanhId`);

--
-- Chỉ mục cho bảng `dangkylophoc`
--
ALTER TABLE `dangkylophoc`
  ADD PRIMARY KEY (`dangKyLopHocId`),
  ADD KEY `fk_dk_hocvien` (`taiKhoanId`),
  ADD KEY `fk_dk_lich` (`lopHocId`);

--
-- Chỉ mục cho bảng `danhgiagiaovien`
--
ALTER TABLE `danhgiagiaovien`
  ADD PRIMARY KEY (`danhGiaId`),
  ADD KEY `fk_dg_gv` (`giaoVienId`),
  ADD KEY `fk_dg_hv` (`hocVienId`),
  ADD KEY `fk_dg_lich` (`lopHocId`);

--
-- Chỉ mục cho bảng `danhmucbaiviet`
--
ALTER TABLE `danhmucbaiviet`
  ADD PRIMARY KEY (`danhMucId`),
  ADD UNIQUE KEY `slug` (`slug`);

--
-- Chỉ mục cho bảng `danhmuckhoahoc`
--
ALTER TABLE `danhmuckhoahoc`
  ADD PRIMARY KEY (`danhMucId`),
  ADD UNIQUE KEY `slug` (`slug`);

--
-- Chỉ mục cho bảng `diembaithi`
--
ALTER TABLE `diembaithi`
  ADD PRIMARY KEY (`diemThiId`),
  ADD KEY `fk_diem_hocvien` (`taiKhoanId`),
  ADD KEY `fk_diem_baithi` (`baiThiId`);

--
-- Chỉ mục cho bảng `diemdanh`
--
ALTER TABLE `diemdanh`
  ADD PRIMARY KEY (`diemDanhId`),
  ADD KEY `fk_dd_hocvien` (`taiKhoanId`),
  ADD KEY `fk_dd_buoi` (`buoiHocId`);

--
-- Chỉ mục cho bảng `failed_jobs`
--
ALTER TABLE `failed_jobs`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `failed_jobs_uuid_unique` (`uuid`);

--
-- Chỉ mục cho bảng `hoadon`
--
ALTER TABLE `hoadon`
  ADD PRIMARY KEY (`hoaDonId`),
  ADD UNIQUE KEY `hoadon_mahoadon_unique` (`maHoaDon`),
  ADD KEY `fk_hd_hocvien` (`taiKhoanId`),
  ADD KEY `fk_hd_dk` (`dangKyLopHocId`),
  ADD KEY `fk_hd_coso` (`coSoId`);

--
-- Chỉ mục cho bảng `hocphi`
--
ALTER TABLE `hocphi`
  ADD PRIMARY KEY (`hocPhiId`),
  ADD KEY `fk_hocphi_khoahoc` (`khoaHocId`);

--
-- Chỉ mục cho bảng `hosonguoidung`
--
ALTER TABLE `hosonguoidung`
  ADD PRIMARY KEY (`taiKhoanId`),
  ADD UNIQUE KEY `cccd` (`cccd`);

--
-- Chỉ mục cho bảng `jobs`
--
ALTER TABLE `jobs`
  ADD PRIMARY KEY (`id`),
  ADD KEY `jobs_queue_index` (`queue`);

--
-- Chỉ mục cho bảng `job_batches`
--
ALTER TABLE `job_batches`
  ADD PRIMARY KEY (`id`);

--
-- Chỉ mục cho bảng `khoahoc`
--
ALTER TABLE `khoahoc`
  ADD PRIMARY KEY (`khoaHocId`),
  ADD UNIQUE KEY `slug` (`slug`),
  ADD KEY `fk_khoahoc_loai` (`danhMucId`);

--
-- Chỉ mục cho bảng `lienhe`
--
ALTER TABLE `lienhe`
  ADD PRIMARY KEY (`lienHeId`),
  ADD KEY `fk_lh_taikhoan` (`taiKhoanId`);

--
-- Chỉ mục cho bảng `lophoc`
--
ALTER TABLE `lophoc`
  ADD PRIMARY KEY (`lopHocId`),
  ADD KEY `fk_lop_giaovien` (`taiKhoanId`),
  ADD KEY `fk_lop_hocphi` (`hocPhiId`),
  ADD KEY `fk_lop_khoahoc` (`khoaHocId`),
  ADD KEY `fk_lop_phong` (`phongHocId`),
  ADD KEY `fk_lop_coso` (`coSoId`),
  ADD KEY `fk_lop_cahoc` (`caHocId`);

--
-- Chỉ mục cho bảng `luong`
--
ALTER TABLE `luong`
  ADD PRIMARY KEY (`luongId`),
  ADD KEY `fk_luong_taikhoan` (`taiKhoanId`);

--
-- Chỉ mục cho bảng `luongchitiet`
--
ALTER TABLE `luongchitiet`
  ADD PRIMARY KEY (`luongChiTietId`),
  ADD KEY `fk_ctluong_luong` (`luongId`),
  ADD KEY `fk_ctluong_lich` (`lopHocId`);

--
-- Chỉ mục cho bảng `migrations`
--
ALTER TABLE `migrations`
  ADD PRIMARY KEY (`id`);

--
-- Chỉ mục cho bảng `nhansu`
--
ALTER TABLE `nhansu`
  ADD PRIMARY KEY (`taiKhoanId`),
  ADD UNIQUE KEY `maNhanVien` (`maNhanVien`),
  ADD KEY `fk_nhansu_coso` (`coSoId`);

--
-- Chỉ mục cho bảng `nhomquyen`
--
ALTER TABLE `nhomquyen`
  ADD PRIMARY KEY (`nhomQuyenId`);

--
-- Chỉ mục cho bảng `noidungbaihoc`
--
ALTER TABLE `noidungbaihoc`
  ADD PRIMARY KEY (`noiDungId`),
  ADD KEY `fk_nd_buoi` (`buoiHocId`),
  ADD KEY `fk_nd_tailieu` (`taiLieuId`);

--
-- Chỉ mục cho bảng `password_reset_tokens`
--
ALTER TABLE `password_reset_tokens`
  ADD PRIMARY KEY (`email`);

--
-- Chỉ mục cho bảng `phanhoi`
--
ALTER TABLE `phanhoi`
  ADD PRIMARY KEY (`phanHoiId`),
  ADD KEY `fk_ph_taikhoan` (`taiKhoanId`),
  ADD KEY `fk_ph_buoi` (`buoiHocId`);

--
-- Chỉ mục cho bảng `phanquyen`
--
ALTER TABLE `phanquyen`
  ADD PRIMARY KEY (`phanQuyenId`),
  ADD UNIQUE KEY `uk_nhom_tinhnang` (`nhomQuyenId`,`tinhNang`);

--
-- Chỉ mục cho bảng `phieuthu`
--
ALTER TABLE `phieuthu`
  ADD PRIMARY KEY (`phieuThuId`),
  ADD UNIQUE KEY `phieuthu_maphieuthu_unique` (`maPhieuThu`),
  ADD KEY `fk_pt_hoadon` (`hoaDonId`),
  ADD KEY `fk_pt_nhanvien` (`taiKhoanId`);

--
-- Chỉ mục cho bảng `phonghoc`
--
ALTER TABLE `phonghoc`
  ADD PRIMARY KEY (`phongHocId`),
  ADD KEY `fk_phong_coso` (`coSoId`);

--
-- Chỉ mục cho bảng `sessions`
--
ALTER TABLE `sessions`
  ADD PRIMARY KEY (`id`),
  ADD KEY `sessions_user_id_index` (`user_id`),
  ADD KEY `sessions_last_activity_index` (`last_activity`);

--
-- Chỉ mục cho bảng `settings`
--
ALTER TABLE `settings`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `settings_key_unique` (`key`);

--
-- Chỉ mục cho bảng `tags`
--
ALTER TABLE `tags`
  ADD PRIMARY KEY (`tagId`),
  ADD UNIQUE KEY `slug` (`slug`);

--
-- Chỉ mục cho bảng `taikhoan`
--
ALTER TABLE `taikhoan`
  ADD PRIMARY KEY (`taiKhoanId`),
  ADD UNIQUE KEY `taiKhoan` (`taiKhoan`),
  ADD UNIQUE KEY `email` (`email`),
  ADD KEY `fk_tk_nhomquyen` (`nhomQuyenId`);

--
-- Chỉ mục cho bảng `tailieu`
--
ALTER TABLE `tailieu`
  ADD PRIMARY KEY (`taiLieuId`),
  ADD KEY `fk_tl_taikhoan` (`taiKhoanId`),
  ADD KEY `fk_tl_khoahoc` (`khoaHocId`);

--
-- Chỉ mục cho bảng `thongbao`
--
ALTER TABLE `thongbao`
  ADD PRIMARY KEY (`thongBaoId`),
  ADD KEY `fk_tb_nguoigui` (`nguoiGuiId`);

--
-- Chỉ mục cho bảng `thongbaonguoidung`
--
ALTER TABLE `thongbaonguoidung`
  ADD PRIMARY KEY (`thongBaoNguoiDungId`),
  ADD KEY `fk_tbnd_thongbao` (`thongBaoId`),
  ADD KEY `fk_tbnd_taikhoan` (`taiKhoanId`);

--
-- Chỉ mục cho bảng `tinhthanh`
--
ALTER TABLE `tinhthanh`
  ADD PRIMARY KEY (`tinhThanhId`),
  ADD UNIQUE KEY `tinhthanh_maapi_unique` (`maAPI`);

--
-- Chỉ mục cho bảng `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `users_email_unique` (`email`);

--
-- AUTO_INCREMENT cho các bảng đã đổ
--

--
-- AUTO_INCREMENT cho bảng `baithi`
--
ALTER TABLE `baithi`
  MODIFY `baiThiId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT cho bảng `baiviet`
--
ALTER TABLE `baiviet`
  MODIFY `baiVietId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=19;

--
-- AUTO_INCREMENT cho bảng `buoihoc`
--
ALTER TABLE `buoihoc`
  MODIFY `buoiHocId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=88;

--
-- AUTO_INCREMENT cho bảng `cahoc`
--
ALTER TABLE `cahoc`
  MODIFY `caHocId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT cho bảng `cosodaotao`
--
ALTER TABLE `cosodaotao`
  MODIFY `coSoId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=15;

--
-- AUTO_INCREMENT cho bảng `dangkylophoc`
--
ALTER TABLE `dangkylophoc`
  MODIFY `dangKyLopHocId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=88;

--
-- AUTO_INCREMENT cho bảng `danhgiagiaovien`
--
ALTER TABLE `danhgiagiaovien`
  MODIFY `danhGiaId` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT cho bảng `danhmucbaiviet`
--
ALTER TABLE `danhmucbaiviet`
  MODIFY `danhMucId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT cho bảng `danhmuckhoahoc`
--
ALTER TABLE `danhmuckhoahoc`
  MODIFY `danhMucId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT cho bảng `diembaithi`
--
ALTER TABLE `diembaithi`
  MODIFY `diemThiId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT cho bảng `failed_jobs`
--
ALTER TABLE `failed_jobs`
  MODIFY `id` bigint(20) UNSIGNED NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT cho bảng `hoadon`
--
ALTER TABLE `hoadon`
  MODIFY `hoaDonId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=17;

--
-- AUTO_INCREMENT cho bảng `hocphi`
--
ALTER TABLE `hocphi`
  MODIFY `hocPhiId` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT cho bảng `jobs`
--
ALTER TABLE `jobs`
  MODIFY `id` bigint(20) UNSIGNED NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT cho bảng `khoahoc`
--
ALTER TABLE `khoahoc`
  MODIFY `khoaHocId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT cho bảng `lienhe`
--
ALTER TABLE `lienhe`
  MODIFY `lienHeId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT cho bảng `lophoc`
--
ALTER TABLE `lophoc`
  MODIFY `lopHocId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;

--
-- AUTO_INCREMENT cho bảng `luong`
--
ALTER TABLE `luong`
  MODIFY `luongId` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT cho bảng `luongchitiet`
--
ALTER TABLE `luongchitiet`
  MODIFY `luongChiTietId` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT cho bảng `migrations`
--
ALTER TABLE `migrations`
  MODIFY `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT cho bảng `nhomquyen`
--
ALTER TABLE `nhomquyen`
  MODIFY `nhomQuyenId` int(11) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT cho bảng `noidungbaihoc`
--
ALTER TABLE `noidungbaihoc`
  MODIFY `noiDungId` bigint(20) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT cho bảng `phanhoi`
--
ALTER TABLE `phanhoi`
  MODIFY `phanHoiId` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT cho bảng `phanquyen`
--
ALTER TABLE `phanquyen`
  MODIFY `phanQuyenId` int(11) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=67;

--
-- AUTO_INCREMENT cho bảng `phieuthu`
--
ALTER TABLE `phieuthu`
  MODIFY `phieuThuId` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT cho bảng `phonghoc`
--
ALTER TABLE `phonghoc`
  MODIFY `phongHocId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT cho bảng `settings`
--
ALTER TABLE `settings`
  MODIFY `id` bigint(20) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

--
-- AUTO_INCREMENT cho bảng `tags`
--
ALTER TABLE `tags`
  MODIFY `tagId` bigint(20) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=14;

--
-- AUTO_INCREMENT cho bảng `taikhoan`
--
ALTER TABLE `taikhoan`
  MODIFY `taiKhoanId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=28;

--
-- AUTO_INCREMENT cho bảng `thongbao`
--
ALTER TABLE `thongbao`
  MODIFY `thongBaoId` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT cho bảng `thongbaonguoidung`
--
ALTER TABLE `thongbaonguoidung`
  MODIFY `thongBaoNguoiDungId` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT cho bảng `tinhthanh`
--
ALTER TABLE `tinhthanh`
  MODIFY `tinhThanhId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=224;

--
-- AUTO_INCREMENT cho bảng `users`
--
ALTER TABLE `users`
  MODIFY `id` bigint(20) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- Các ràng buộc cho các bảng đã đổ
--

--
-- Các ràng buộc cho bảng `baithi`
--
ALTER TABLE `baithi`
  ADD CONSTRAINT `fk_baithi_khoa` FOREIGN KEY (`khoaHocId`) REFERENCES `khoahoc` (`khoaHocId`);

--
-- Các ràng buộc cho bảng `baiviet`
--
ALTER TABLE `baiviet`
  ADD CONSTRAINT `fk_baiviet_taikhoan` FOREIGN KEY (`taiKhoanId`) REFERENCES `taikhoan` (`taiKhoanId`) ON DELETE CASCADE;

--
-- Các ràng buộc cho bảng `baiviet_danhmuc`
--
ALTER TABLE `baiviet_danhmuc`
  ADD CONSTRAINT `fk_bvdm_baiviet` FOREIGN KEY (`baiVietId`) REFERENCES `baiviet` (`baiVietId`) ON DELETE CASCADE,
  ADD CONSTRAINT `fk_bvdm_danhmuc` FOREIGN KEY (`danhMucId`) REFERENCES `danhmucbaiviet` (`danhMucId`) ON DELETE CASCADE;

--
-- Các ràng buộc cho bảng `baiviet_tag`
--
ALTER TABLE `baiviet_tag`
  ADD CONSTRAINT `fk_bvt_baiviet` FOREIGN KEY (`baiVietId`) REFERENCES `baiviet` (`baiVietId`) ON DELETE CASCADE,
  ADD CONSTRAINT `fk_bvt_tag` FOREIGN KEY (`tagId`) REFERENCES `tags` (`tagId`) ON DELETE CASCADE;

--
-- Các ràng buộc cho bảng `buoihoc`
--
ALTER TABLE `buoihoc`
  ADD CONSTRAINT `fk_buoi_ca` FOREIGN KEY (`caHocId`) REFERENCES `cahoc` (`caHocId`),
  ADD CONSTRAINT `fk_buoi_gv` FOREIGN KEY (`taiKhoanId`) REFERENCES `taikhoan` (`taiKhoanId`),
  ADD CONSTRAINT `fk_buoi_lich` FOREIGN KEY (`lopHocId`) REFERENCES `lophoc` (`lopHocId`),
  ADD CONSTRAINT `fk_buoi_phong` FOREIGN KEY (`phongHocId`) REFERENCES `phonghoc` (`phongHocId`);

--
-- Các ràng buộc cho bảng `cosodaotao`
--
ALTER TABLE `cosodaotao`
  ADD CONSTRAINT `fk_coso_tinhthanh` FOREIGN KEY (`tinhThanhId`) REFERENCES `tinhthanh` (`tinhThanhId`);

--
-- Các ràng buộc cho bảng `dangkylophoc`
--
ALTER TABLE `dangkylophoc`
  ADD CONSTRAINT `fk_dk_hocvien` FOREIGN KEY (`taiKhoanId`) REFERENCES `taikhoan` (`taiKhoanId`),
  ADD CONSTRAINT `fk_dk_lich` FOREIGN KEY (`lopHocId`) REFERENCES `lophoc` (`lopHocId`);

--
-- Các ràng buộc cho bảng `danhgiagiaovien`
--
ALTER TABLE `danhgiagiaovien`
  ADD CONSTRAINT `fk_dg_gv` FOREIGN KEY (`giaoVienId`) REFERENCES `taikhoan` (`taiKhoanId`),
  ADD CONSTRAINT `fk_dg_hv` FOREIGN KEY (`hocVienId`) REFERENCES `taikhoan` (`taiKhoanId`),
  ADD CONSTRAINT `fk_dg_lich` FOREIGN KEY (`lopHocId`) REFERENCES `lophoc` (`lopHocId`);

--
-- Các ràng buộc cho bảng `diembaithi`
--
ALTER TABLE `diembaithi`
  ADD CONSTRAINT `fk_diem_baithi` FOREIGN KEY (`baiThiId`) REFERENCES `baithi` (`baiThiId`),
  ADD CONSTRAINT `fk_diem_hocvien` FOREIGN KEY (`taiKhoanId`) REFERENCES `taikhoan` (`taiKhoanId`);

--
-- Các ràng buộc cho bảng `diemdanh`
--
ALTER TABLE `diemdanh`
  ADD CONSTRAINT `fk_dd_buoi` FOREIGN KEY (`buoiHocId`) REFERENCES `buoihoc` (`buoiHocId`),
  ADD CONSTRAINT `fk_dd_hocvien` FOREIGN KEY (`taiKhoanId`) REFERENCES `taikhoan` (`taiKhoanId`);

--
-- Các ràng buộc cho bảng `hoadon`
--
ALTER TABLE `hoadon`
  ADD CONSTRAINT `fk_hd_coso` FOREIGN KEY (`coSoId`) REFERENCES `cosodaotao` (`coSoId`),
  ADD CONSTRAINT `fk_hd_dk` FOREIGN KEY (`dangKyLopHocId`) REFERENCES `dangkylophoc` (`dangKyLopHocId`),
  ADD CONSTRAINT `fk_hd_hocvien` FOREIGN KEY (`taiKhoanId`) REFERENCES `taikhoan` (`taiKhoanId`);

--
-- Các ràng buộc cho bảng `hocphi`
--
ALTER TABLE `hocphi`
  ADD CONSTRAINT `fk_hocphi_khoahoc` FOREIGN KEY (`khoaHocId`) REFERENCES `khoahoc` (`khoaHocId`);

--
-- Các ràng buộc cho bảng `hosonguoidung`
--
ALTER TABLE `hosonguoidung`
  ADD CONSTRAINT `fk_hoso_taikhoan` FOREIGN KEY (`taiKhoanId`) REFERENCES `taikhoan` (`taiKhoanId`) ON DELETE CASCADE;

--
-- Các ràng buộc cho bảng `khoahoc`
--
ALTER TABLE `khoahoc`
  ADD CONSTRAINT `fk_khoahoc_loai` FOREIGN KEY (`danhMucId`) REFERENCES `danhmuckhoahoc` (`danhMucId`);

--
-- Các ràng buộc cho bảng `lienhe`
--
ALTER TABLE `lienhe`
  ADD CONSTRAINT `fk_lh_taikhoan` FOREIGN KEY (`taiKhoanId`) REFERENCES `taikhoan` (`taiKhoanId`);

--
-- Các ràng buộc cho bảng `lophoc`
--
ALTER TABLE `lophoc`
  ADD CONSTRAINT `fk_lop_cahoc` FOREIGN KEY (`caHocId`) REFERENCES `cahoc` (`caHocId`),
  ADD CONSTRAINT `fk_lop_coso` FOREIGN KEY (`coSoId`) REFERENCES `cosodaotao` (`coSoId`),
  ADD CONSTRAINT `fk_lop_giaovien` FOREIGN KEY (`taiKhoanId`) REFERENCES `taikhoan` (`taiKhoanId`),
  ADD CONSTRAINT `fk_lop_hocphi` FOREIGN KEY (`hocPhiId`) REFERENCES `hocphi` (`hocPhiId`),
  ADD CONSTRAINT `fk_lop_khoahoc` FOREIGN KEY (`khoaHocId`) REFERENCES `khoahoc` (`khoaHocId`),
  ADD CONSTRAINT `fk_lop_phong` FOREIGN KEY (`phongHocId`) REFERENCES `phonghoc` (`phongHocId`);

--
-- Các ràng buộc cho bảng `luong`
--
ALTER TABLE `luong`
  ADD CONSTRAINT `fk_luong_taikhoan` FOREIGN KEY (`taiKhoanId`) REFERENCES `taikhoan` (`taiKhoanId`);

--
-- Các ràng buộc cho bảng `luongchitiet`
--
ALTER TABLE `luongchitiet`
  ADD CONSTRAINT `fk_ctluong_lich` FOREIGN KEY (`lopHocId`) REFERENCES `lophoc` (`lopHocId`),
  ADD CONSTRAINT `fk_ctluong_luong` FOREIGN KEY (`luongId`) REFERENCES `luong` (`luongId`);

--
-- Các ràng buộc cho bảng `nhansu`
--
ALTER TABLE `nhansu`
  ADD CONSTRAINT `fk_nhansu_coso` FOREIGN KEY (`coSoId`) REFERENCES `cosodaotao` (`coSoId`),
  ADD CONSTRAINT `fk_nhansu_taikhoan` FOREIGN KEY (`taiKhoanId`) REFERENCES `taikhoan` (`taiKhoanId`) ON DELETE CASCADE;

--
-- Các ràng buộc cho bảng `noidungbaihoc`
--
ALTER TABLE `noidungbaihoc`
  ADD CONSTRAINT `fk_nd_buoi` FOREIGN KEY (`buoiHocId`) REFERENCES `buoihoc` (`buoiHocId`),
  ADD CONSTRAINT `fk_nd_tailieu` FOREIGN KEY (`taiLieuId`) REFERENCES `tailieu` (`taiLieuId`);

--
-- Các ràng buộc cho bảng `phanhoi`
--
ALTER TABLE `phanhoi`
  ADD CONSTRAINT `fk_ph_buoi` FOREIGN KEY (`buoiHocId`) REFERENCES `buoihoc` (`buoiHocId`),
  ADD CONSTRAINT `fk_ph_taikhoan` FOREIGN KEY (`taiKhoanId`) REFERENCES `taikhoan` (`taiKhoanId`);

--
-- Các ràng buộc cho bảng `phanquyen`
--
ALTER TABLE `phanquyen`
  ADD CONSTRAINT `fk_pq_nhomquyen` FOREIGN KEY (`nhomQuyenId`) REFERENCES `nhomquyen` (`nhomQuyenId`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Các ràng buộc cho bảng `phieuthu`
--
ALTER TABLE `phieuthu`
  ADD CONSTRAINT `fk_pt_hoadon` FOREIGN KEY (`hoaDonId`) REFERENCES `hoadon` (`hoaDonId`),
  ADD CONSTRAINT `fk_pt_nhanvien` FOREIGN KEY (`taiKhoanId`) REFERENCES `taikhoan` (`taiKhoanId`);

--
-- Các ràng buộc cho bảng `phonghoc`
--
ALTER TABLE `phonghoc`
  ADD CONSTRAINT `fk_phong_coso` FOREIGN KEY (`coSoId`) REFERENCES `cosodaotao` (`coSoId`);

--
-- Các ràng buộc cho bảng `taikhoan`
--
ALTER TABLE `taikhoan`
  ADD CONSTRAINT `fk_tk_nhomquyen` FOREIGN KEY (`nhomQuyenId`) REFERENCES `nhomquyen` (`nhomQuyenId`) ON DELETE SET NULL ON UPDATE CASCADE;

--
-- Các ràng buộc cho bảng `tailieu`
--
ALTER TABLE `tailieu`
  ADD CONSTRAINT `fk_tl_khoahoc` FOREIGN KEY (`khoaHocId`) REFERENCES `khoahoc` (`khoaHocId`),
  ADD CONSTRAINT `fk_tl_taikhoan` FOREIGN KEY (`taiKhoanId`) REFERENCES `taikhoan` (`taiKhoanId`);

--
-- Các ràng buộc cho bảng `thongbao`
--
ALTER TABLE `thongbao`
  ADD CONSTRAINT `fk_tb_nguoigui` FOREIGN KEY (`nguoiGuiId`) REFERENCES `taikhoan` (`taiKhoanId`);

--
-- Các ràng buộc cho bảng `thongbaonguoidung`
--
ALTER TABLE `thongbaonguoidung`
  ADD CONSTRAINT `fk_tbnd_taikhoan` FOREIGN KEY (`taiKhoanId`) REFERENCES `taikhoan` (`taiKhoanId`),
  ADD CONSTRAINT `fk_tbnd_thongbao` FOREIGN KEY (`thongBaoId`) REFERENCES `thongbao` (`thongBaoId`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
