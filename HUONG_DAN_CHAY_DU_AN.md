# HƯỚNG DẪN HỆ THỐNG THI TOEIC READING 100 CÂU (LEXORA)
**Môn học:** Lập trình di động - HUTECH  
**Mã số sinh viên (MSSV):** 2380600189  
**Cơ sở dữ liệu:** Microsoft SQL Server `.\SQLEXPRESS` (Database: `_2380600189Db`)  

---

## 1. PHÂN QUYỀN RÕ RÀNG THEO YÊU CẦU ĐỀ TÀI

### 👨‍🏫 GIẢNG VIÊN (CHẠY TRÊN NỀN TẢNG WEB BROWSER)
Truy cập qua trình duyệt máy tính tại: **[http://localhost:5000](http://localhost:5000)**
Bao gồm 2 nhóm chức năng chính:
1. **Quản lý bài thi (`/Exams`):**
   - **Tải lên đề thi từ file Word (`/Exams/Import`):** Phân tích tự động 100 câu hỏi (Part 5, Part 6, Part 7) theo bộ thẻ chuẩn. Nếu file Word bị sai format, hệ thống dừng lại và báo lỗi chi tiết, không lưu rác vào CSDL.
   - **Kích hoạt ca thi:** Nút bấm bật/tắt (Xuất bản / Thu hồi đề thi) tức thời bằng AJAX. Khi xuất bản, sinh viên trên app di động sẽ thấy và vào thi được.
   - **Xem trước đề thi (`/Exams/Preview/{id}`):** Xem toàn bộ 100 câu, highlight đáp án đúng màu xanh lá và hiển thị lời giải thích chi tiết.
   - **Chỉnh sửa đề thi & câu hỏi (`/Exams/Edit/{id}`, `/Exams/EditQuestion/{id}`):** Điều chỉnh nội dung, các đáp án A/B/C/D và đáp án đúng.
   - **Báo cáo & Thống kê (`/Exams/Statistics/{id}`):** Xem 4 thẻ KPI (Tổng lượt thi, Điểm TB, Điểm cao nhất, Điểm thấp nhất) và **Bảng TOP 10 CÂU HỎI SINH VIÊN LÀM SAI NHẤT** kèm thanh tỉ lệ sai (%), đáp án chuẩn và giải thích.

2. **Quản lý sinh viên (`/Students`):**
   - **Danh sách sinh viên:** Xem toàn bộ sinh viên đã tham gia thi (Họ tên, MSSV, Tổng số lần thi, Điểm cao nhất, Điểm trung bình, Lần thi gần nhất).
   - **Tìm kiếm sinh viên:** Tìm kiếm nhanh theo Họ tên hoặc Mã số sinh viên (MSSV).
   - **Lịch sử làm bài (`/Students/Detail/{mssv}`):** Xem chi tiết từng lần thi của sinh viên đó (Thời gian làm bài, Điểm số, Số câu Đúng/Sai/Bỏ qua).
   - **Xem chi tiết bài làm (`/Students/AttemptDetail/{attemptId}`):** Giảng viên bấm vào để soi chi tiết từng câu trong bài làm của sinh viên (xem sinh viên đã chọn đáp án nào, câu nào đúng, câu nào sai, và giải thích tương ứng).

---

### 📱 SINH VIÊN (CHẠY ỨNG DỤNG DI ĐỘNG FLUTTER - ANDROID / IOS)
Mã nguồn nằm trong thư mục: [`frontend/`](file:///c:/L%E1%BA%ADp%20tr%C3%ACnh%20di%20%C4%91%E1%BB%99ng/KiemTra/frontend/)
Ứng dụng được thiết kế **CHỈ DÙNG ĐỂ LÀM BÀI THI**, tinh gọn và bảo mật:
1. **Màn hình Trang chủ:**
   - Xem các ca thi TOEIC đang được giảng viên mở.
   - Nhập Họ tên và Mã số sinh viên (MSSV: `2380600189`) để bắt đầu vào phòng thi.
   - Nút cài đặt địa chỉ IP Backend (linh hoạt cho cả máy thật, máy ảo Android, Windows và Web).
2. **Màn hình Phòng thi (75 phút):**
   - **Đồng hồ đếm ngược 75 phút thời gian thực** (chuyển sang màu đỏ cảnh báo khi còn dưới 10 phút).
   - **Tự động nộp bài:** Khi hết giờ (00:00), hệ thống tự động khóa bài và nộp lên máy chủ.
   - **Lưới chuyển câu hỏi (100 câu):** Nhảy nhanh đến bất kỳ câu hỏi nào trong 100 câu.
   - **Hiển thị bài đọc:** Part 6 & 7 hiển thị đoạn văn đọc hiểu trong khung riêng biệt, dễ đọc trên màn hình điện thoại.
   - Chọn đáp án radio A, B, C, D rõ ràng, mượt mà.
3. **Màn hình Kết quả & Chấm điểm TOEIC:**
   - Điểm số quy đổi chuẩn ETS quốc tế từ **5 – 495 điểm**.
   - Đánh giá xếp loại năng lực theo khung CEFR.
   - Thống kê số câu Đúng, Sai, Chưa làm và điểm từng Part 5, 6, 7.
4. **Màn hình Xem lại bài thi chi tiết:**
   - Xem lại 100 câu hỏi kèm đáp án sinh viên đã chọn (khoanh đỏ nếu sai, xanh nếu đúng).
   - Lọc nhanh các câu làm sai hoặc câu bỏ qua.
   - Đọc lời giải thích chi tiết tiếng Việt cho từng câu.

---

## 2. CÁCH KHỞI CHẠY HỆ THỐNG

### Bước 1: Chạy Web Giảng viên (Backend ASP.NET Core MVC)
Mở PowerShell tại thư mục:
```powershell
cd "c:\Lập trình di động\KiemTra\backend\src\_2380600189.Web.Mvc"
dotnet run --urls="http://localhost:5000"
```
Mở trình duyệt:
- Quản lý đề thi: [http://localhost:5000/Exams](http://localhost:5000/Exams)
- Quản lý sinh viên: [http://localhost:5000/Students](http://localhost:5000/Students)

### Bước 2: Chạy App Di động Sinh viên (Flutter)
Mở một cửa sổ PowerShell mới:
```powershell
cd "c:\Lập trình di động\KiemTra\frontend"

# Chạy trên Android Emulator hoặc Điện thoại thật:
flutter run

# Hoặc chạy kiểm thử nhanh trên Web Chrome:
flutter run -d chrome

# Hoặc chạy kiểm thử trên Windows Desktop:
flutter run -d windows
```

*(Lưu ý: Nếu cần chạy máy ảo Android, trong app di động bấm vào icon ⚙️ Cài đặt ở góc trên phải và đổi IP thành `http://10.0.2.2:5000/api/exam` để kết nối vào máy chủ backend của máy tính).*

---

## 3. FILE ĐỀ THI MẪU (.DOCX) ĐÃ ĐƯỢC TẠO
- [`TOEIC_RC_Test_02.docx`](file:///c:/L%E1%BA%ADp%20tr%C3%ACnh%20di%20%C4%91%E1%BB%99ng/KiemTra/TOEIC_RC_Test_02.docx): File đề thi chính thức chuẩn 100 câu từ file PDF đề thi mẫu (ETS TOEIC RC Test 02).
- [`TOEIC_Reading_Test_01.docx`](file:///c:/L%E1%BA%ADp%20tr%C3%ACnh%20di%20%C4%91%E1%BB%99ng/KiemTra/TOEIC_Reading_Test_01.docx): File đề thi kiểm tra số 1.
