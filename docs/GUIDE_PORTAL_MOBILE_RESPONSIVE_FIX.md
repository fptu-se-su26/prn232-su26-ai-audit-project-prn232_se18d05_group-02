# Tài liệu Sửa đổi Giao diện Responsive Mobile cho Guide Portal (`/guide-portal`)

Tài liệu này ghi lại các thay đổi liên quan đến việc tối ưu hóa giao diện hiển thị trên thiết bị di động (mobile) và sửa lỗi tràn văn bản (text overflow) trên các thẻ thông tin chi tiết của Tour.

---

## 1. Vấn đề trước khi thay đổi (Issues)

1. **Banner chiếm nhiều diện tích:** Banner đầu trang (`guide-self-hero`) chiếm quá nhiều không gian dọc trên giao diện di động, khiến người dùng phải cuộn trang nhiều để xem được lịch làm việc.
2. **Tràn văn bản ở thẻ chi tiết (Text Overflow):**
   - Trong panel **Tour detail**, bốn thẻ thông tin (`TOUR CODE`, `SCHEDULE`, `REGION`, `MEETING POINT`) được xếp nằm ngang trên 4 cột (`repeat(4, minmax(0, 1fr))`).
   - Trên các màn hình có độ rộng trung bình hoặc khi panel hiển thị ở sidebar hẹp (độ rộng dưới 400px), các cột bị bó lại cực kỳ nhỏ (chỉ khoảng 80px - 90px mỗi thẻ).
   - Do thiếu thuộc tính ngắt từ (`word-break` / `overflow-wrap`) và giá trị `min-width: 0`, các văn bản dài như cụm từ *"Southern Africa"*, khoảng ngày lịch trình, hay *"Arusha Coffee Lodge reception"* bị đẩy tràn ra khỏi đường viền bo góc của thẻ, gây mất thẩm mỹ nghiêm trọng.

---

## 2. Giải pháp và các thay đổi chi tiết (Proposed Changes)

Chúng tôi đã thực hiện các điều chỉnh tối thiểu nhưng hiệu quả cao nhất để giải quyết triệt để các vấn đề trên mà không ảnh hưởng đến logic nghiệp vụ hay các trang khác.

### 2.1. Tối ưu hóa không gian hiển thị đầu trang
* **File sửa đổi:** [GuidePortal.razor](file:///d:/FPT/SU26/PRN232/prn232-su26-ai-audit-project-prn232_se18d05_group-02/WanderXClient/WanderXClient/Pages/GuidePortal.razor) (Dòng 13 - 19)
* **Chi tiết:** Loại bỏ hoàn toàn khối mã HTML hiển thị Hero Banner:
  ```razor
  <section class="guide-self-hero">
      <div class="wx-container guide-self-hero__inner">
          <p class="guide-eyebrow">Guide Portal</p>
          <h1>Your Assigned Tours</h1>
          <p>View your working schedule, inspect assigned tour details, decline with a valid reason, or finish confirmed tours.</p>
      </div>
  </section>
  ```
  Việc loại bỏ giúp trang bắt đầu trực tiếp từ thanh công cụ và lịch trình làm việc, tối ưu hóa giao diện di động.

### 2.2. Khắc phục lỗi tràn khung và cải thiện Responsive cho thẻ thông tin Tour Detail
* **File sửa đổi:** [app.css](file:///d:/FPT/SU26/PRN232/prn232-su26-ai-audit-project-prn232_se18d05_group-02/WanderXClient/WanderXClient/wwwroot/css/app.css) (Dòng 2338 - 2364)
* **Chi tiết thay đổi:**
  1. Thay đổi số lượng cột mặc định của `.guide-tour-detail-grid` trên desktop/tablet từ **4 cột** thành **2 cột** (`grid-template-columns: repeat(2, 1fr)`). Điều này giúp nhân đôi diện tích bề ngang của mỗi thẻ trong panel chi tiết hẹp, giúp các chuỗi văn bản có đủ khoảng trống hiển thị.
  2. Bổ sung các thuộc tính định dạng chữ cho thẻ chứa (`div`), nhãn (`span`) và giá trị (`strong`) để tự động xuống dòng và ngắt từ phù hợp:
     * `min-width: 0;` (quan trọng để các item trong CSS Grid có thể co giãn nhỏ hơn kích thước nội dung mặc định của chúng).
     * `overflow-wrap: break-word;`
     * `word-wrap: break-word;`
     * `word-break: break-word;`

  ```css
  /* Mã CSS mới sau khi cập nhật */
  .guide-tour-detail-grid {
      display: grid;
      grid-template-columns: repeat(2, 1fr);
      gap: 14px;
  }

  .guide-tour-detail-grid div {
      display: grid;
      gap: 4px;
      border: 1px solid rgba(195, 198, 214, 0.55);
      border-radius: var(--wx-radius);
      background: var(--wx-surface-low);
      padding: 14px;
      min-width: 0;
      overflow-wrap: break-word;
      word-wrap: break-word;
      word-break: break-word;
  }

  .guide-tour-detail-grid span {
      color: var(--wx-outline-strong);
      font-size: 11px;
      font-weight: 850;
      text-transform: uppercase;
      overflow-wrap: break-word;
      word-wrap: break-word;
      word-break: break-word;
  }

  .guide-tour-detail-grid strong {
      color: var(--wx-ink);
      font-size: 14px;
      overflow-wrap: break-word;
      word-wrap: break-word;
      word-break: break-word;
  }
  ```

---

## 3. Kết quả đạt được (Results)

* **Hiển thị trên Mobile:** Khi ở chế độ mobile (chiều rộng màn hình $\le$ 900px), các thẻ thông tin tự động chuyển thành **1 cột dọc** nhờ quy tắc `@media` định sẵn. Kết hợp với các quy tắc ngắt từ mới, nội dung chi tiết hiển thị đẹp mắt, không có hiện tượng tràn khung hoặc méo mó giao diện.
* **Hiển thị trên Desktop/Tablet:** Panel chi tiết hiển thị ở cột bên phải theo định dạng **2 cột x 2 hàng**, văn bản tự động xuống hàng một cách tự nhiên khi nội dung quá dài (ví dụ: điểm hẹn hay mã tour).
* **Độ ổn định:** Toàn bộ ứng dụng Blazor WebAssembly phía Client được build lại thành công (`dotnet build`) mà không gặp bất kỳ lỗi biên dịch hay cảnh báo nào.
