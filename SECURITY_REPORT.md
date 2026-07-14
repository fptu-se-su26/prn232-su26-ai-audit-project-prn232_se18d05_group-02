# BÁO CÁO CÁC BIỆN PHÁP BẢO MẬT HỆ THỐNG WANDERX

Tài liệu này báo cáo chi tiết về việc triển khai các giải pháp bảo mật nâng cao cho hệ thống WanderX bao gồm cả **ASP.NET Core Web API (WanderXServer)** và **Blazor WebAssembly Client (WanderXClient)**.

---

## 1. Rate Limiting (Kiểm Soát Tần Suất Gửi Request)

### Mục tiêu
Kiểm soát số lượng request tối đa từ một người dùng (hoặc IP) trong khoảng thời gian nhất định để tránh quá tải hệ thống (DoS) hoặc bị tấn công dò tìm mật khẩu (Brute-Force).

### Cách triển khai
- Sử dụng thư viện **Microsoft.AspNetCore.RateLimiting** tích hợp sẵn trong .NET 8.
- Cấu hình **Global Limiter**: Sử dụng thuật toán *Sliding Window* giới hạn tối đa **60 requests mỗi phút** cho mỗi địa chỉ IP của Client.
- Cấu hình **Auth Limiter**: Thiết lập bộ giới hạn riêng biệt và nghiêm ngặt hơn tên là `"AuthLimiter"` sử dụng thuật toán *Fixed Window* giới hạn tối đa **5 requests mỗi phút** nhằm bảo vệ các endpoint nhạy cảm như Đăng nhập, Đăng ký và Đổi mật khẩu.
- Áp dụng bộ lọc này bằng cách gắn thuộc tính `[EnableRateLimiting("AuthLimiter")]` lên `AuthController.cs`.

---

## 2. CORS (Cross-Origin Resource Sharing)

### Mục tiêu
Giới hạn các tên miền (domain) cụ thể được phép gọi API, ngăn chặn việc sử dụng API trái phép từ các nguồn không xác định hoặc ứng dụng độc hại.

### Cách triển khai
- Cấu hình CORS động bằng cách đọc danh sách các domain được cho phép từ tệp `appsettings.json` (phần `"Cors:AllowedOrigins"`).
- Chỉ cho phép các domain này truy cập API qua các phương thức HTTP an toàn và cần thiết như `GET`, `POST`, `PUT`, `DELETE`.
- Hỗ trợ gửi thông tin xác thực (`AllowCredentials`) như cookies qua CORS cho các kết nối liên kết.
- Cấu hình phơi bày header tuỳ chỉnh `X-XSRF-TOKEN` qua CORS (`WithExposedHeaders`) để client có thể đọc mã chống CSRF một cách an toàn.

---

## 3. SQL & NoSQL Injection

### Mục tiêu
Ngăn chặn hacker chèn mã độc vào câu lệnh truy vấn dữ liệu bằng cách sử dụng các thư viện ORM bảo mật và tham số hóa dữ liệu đầu vào.

### Cách triển khai
- Hệ thống sử dụng **Entity Framework Core (EF Core)** làm ORM chính để giao tiếp với SQL Server.
- Toàn bộ truy vấn dữ liệu từ controllers và services đều được xây dựng qua LINQ (ví dụ: `_dbContext.Users.FirstOrDefaultAsync(u => u.NormalizedEmail == email)`), cơ chế này tự động dịch mã và tham số hóa (parameterize) tất cả các dữ liệu đầu vào khi sinh câu lệnh SQL. Điều này ngăn chặn triệt để lỗ hổng SQL Injection.
- Các hàm tạo bảng dữ liệu hoặc seeding bằng SQL thô (`ExecuteSqlRaw`) chỉ được viết cố định trong mã nguồn hệ thống (`WanderXDbContext.cs`) mà không hề nối chuỗi với bất kỳ biến nhập liệu nào của người dùng.

---

## 4. CSRF (Cross-Site Request Forgery)

### Mục tiêu
Ngăn chặn việc hacker lợi dụng phiên làm việc hiện tại của người dùng trên trình duyệt (qua cookie tự động) để thực hiện các hành động trái phép (như thay đổi thông tin hướng dẫn viên, hủy tour, rút tiền, v.v.).

### Cách triển khai
- Áp dụng giải pháp **Double Submit Cookie** (stateless và bảo mật cao) kết hợp giữa Cookie và Request Header:
  - **Tầng Server**: 
    - Với các request an toàn (`GET`), `CsrfProtectionMiddleware` sẽ sinh ngẫu nhiên một token bảo mật và ghi vào cookie `XSRF-TOKEN` (HttpOnly = false để client JS đọc được), đồng thời gửi kèm token này trong Header phản hồi `X-XSRF-TOKEN`.
    - Với các request thay đổi trạng thái (`POST`, `PUT`, `DELETE`), middleware sẽ so khớp token nằm trong cookie `XSRF-TOKEN` và token do client đính kèm tại Header `X-XSRF-TOKEN`. Nếu bị thiếu hoặc không trùng khớp, request lập tức bị từ chối bằng mã lỗi `400 BadRequest`.
  - **Tầng Client (Blazor WebAssembly)**:
    - Triển khai một `CsrfHeaderHandler` (HttpClient DelegatingHandler) chịu trách nhiệm tự động thiết lập truyền tải credentials (`Credentials.Include`).
    - Tự động bắt giữ (capture) mã chống CSRF nhận được từ Response Header của server và đính kèm vào Header `X-XSRF-TOKEN` cho toàn bộ các request thay đổi trạng thái kế tiếp một cách hoàn toàn tự động mà không ảnh hưởng đến logic nghiệp vụ.

---

## 5. XSS (Cross-Site Scripting)

### Mục tiêu
Ngăn chặn hacker chèn các mã JavaScript độc hại vào trang web và lưu trữ trong cơ sở dữ liệu để thực thi mã trên trình duyệt của người dùng khác khi dữ liệu được tải lên giao diện.

### Cách triển khai
- **Tầng Client (Blazor WebAssembly)**:
  - Bản thân framework Blazor WebAssembly đã tự động mã hóa HTML (HTML-Encode) cho toàn bộ các biến số được hiển thị trên giao diện thông qua cú pháp `@variable`.
  - Kiểm tra và đảm bảo không sử dụng thuộc tính `MarkupString` (cho phép render HTML thô) trên các phần nhập liệu từ phía người dùng.
- **Tầng Server (API)**:
  - Viết bộ lọc toàn cục **XssSanitizationFilter** được cấu hình trong `Program.cs`.
  - Bộ lọc này tự động quét qua toàn bộ dữ liệu chuỗi (string) trong các tham số request đầu vào và các thuộc tính của DTO.
  - Thực hiện mã hóa HTML bằng `WebUtility.HtmlEncode` và lọc sạch các từ khóa nguy hiểm (như thẻ `<script>`, liên kết `javascript:`, và các sự kiện HTML như `onload`, `onclick`) thông qua lớp tiện ích `XssSanitizer`.
  - Dữ liệu trước khi được ghi xuống cơ sở dữ liệu luôn ở dạng an toàn, đảm bảo không thể chứa mã script thực thi.

---

## Kết Luận
Hệ thống WanderX hiện tại đã được củng cố bảo mật toàn diện từ cả hai phía Client và Server đối với 5 nguy cơ chính: Rate Limiting, CORS, SQL/NoSQL Injection, CSRF, và XSS. Các biện pháp bảo mật trên đều được tích hợp khép kín, hoạt động ổn định và đã được kiểm tra biên dịch thành công 100%.
