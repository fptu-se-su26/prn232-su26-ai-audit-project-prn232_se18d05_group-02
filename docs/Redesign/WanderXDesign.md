# WanderX Design System (TravelHub Design System)

Bản đặc tả hệ thống thiết kế giao diện cho dự án du lịch cao cấp WanderX. Hệ thống được phát triển nhằm mang lại cảm giác sang trọng, thanh lịch như một tạp chí du lịch cao cấp, đồng thời tối ưu hóa tỷ lệ chuyển đổi và trải nghiệm tìm kiếm của khách hàng.

---

## 1. Hệ thống Màu sắc (Color Tokens)

Hệ thống sử dụng tone màu chủ đạo là xanh đại dương sâu (Deep Ocean Blue) mang lại cảm giác tin cậy, kết hợp với màu cam hoàng hôn (Sunset Orange) được lấy cảm hứng từ năng lượng của những chuyến đi.

### Màu sắc cốt lõi:
- **Primary (Xanh Đại Dương Sâu):** `#0f172a` (Slate 900) - Thanh lịch, cao cấp, dùng cho Header, Footer, và các tiêu đề chính.
- **On-Primary:** `#ffffff`
- **Canvas (Nền trang chính):** `#ffffff` - Tạo không gian trắng thoáng đãng, sạch sẽ.
- **Surface Soft (Nền bọc thẻ nhẹ):** `#f8fafc` (Slate 50) - Dùng cho nền các ô tìm kiếm, sidebar, khung nội dung phụ.
- **Surface Mid (Nền hover/phân tách):** `#f1f5f9` (Slate 100) - Dùng cho hiệu ứng hover hoặc phân tách nhẹ.
- **Hairline (Đường viền mỏng):** `#e2e8f0` (Slate 200) - Đường kẻ mỏng phân tách nội dung mà không gây nhiễu thị giác.
- **Muted Foreground (Chữ phụ/Metadata):** `#64748b` (Slate 500) - Dùng cho icon phụ, thông tin ngày tháng, địa điểm.

### Màu sắc điểm nhấn (Accent):
- **Accent Orange (Cam Hồng Hoàng Hôn):** `#ff5a5f` - Màu cam hoàng hôn rực rỡ dùng riêng cho nút Đặt Ngay (Book Now), các nút kêu gọi hành động (CTA) chính và thông tin quan trọng cần làm nổi bật.
- **On-Accent Orange:** `#ffffff`
- **Accent Orange Soft:** `#fff1f2` - Màu nền cho các thẻ giảm giá hoặc trạng thái chờ thanh toán.

### Màu sắc loại hình dịch vụ:
- **Tour Du Lịch:** `#0284c7` (Xanh da trời)
- **Khách Sạn/Resort:** `#10b981` (Xanh lá cây)
- **Vé Máy Bay:** `#6366f1` (Tím xanh)
- **Combo Trọn Gói:** `#f59e0b` (Vàng hổ phách)

### Trạng thái giao dịch (Semantic Colors):
- **Thành công (Paid/Confirmed):** `#22c55e` (Xanh lá)
- **Chờ thanh toán (Pending):** `#eab308` (Vàng)
- **Đã hủy/Hết chỗ (Cancelled):** `#ef4444` (Đỏ)

---

## 2. Hệ thống Bo góc (Border Radius Tokens)

Hệ thống thiết kế sử dụng các góc bo tròn mềm mại tạo cảm giác thân thiện, hiện đại:
- **sm (Bo góc nhỏ):** `6px` (`rounded-sm`) - Dùng cho tag ưu đãi, nhãn nhỏ.
- **md (Bo góc trung bình):** `12px` (`rounded-md`) - Dùng cho các khung nhập liệu, ô chọn ngày.
- **lg (Bo góc lớn):** `16px` (`rounded-lg`) - Chuẩn bo góc cho các thẻ sản phẩm (Card Tour/Card Khách sạn).
- **xl (Bo góc cực lớn):** `24px` (`rounded-xl`) - Dành cho Search Bar nổi bật ở Hero Banner.
- **full (Hình tròn/Viên thuốc):** `9999px` (`rounded-full`) - Dành cho các nút bấm hành động chính (CTA Pill) và avatar.

---

## 3. Hệ thống Typography (Kiểu chữ)

- **Font chữ tiêu đề (Headers):** `Plus Jakarta Sans`, bo tròn nhẹ, hiện đại và trẻ trung.
- **Font chữ nội dung (Body):** `Inter`, dễ đọc, tối ưu hiển thị trên mọi kích thước màn hình.

### Các cấp độ chữ chính:
- **Display XL (Tiêu đề Hero):** `48px`, font-weight `800`, line-height `1.15`, letter-spacing `-1.00px`.
- **Display LG (Tiêu đề trang):** `36px`, font-weight `700`, line-height `1.20`.
- **Headline (Tiêu đề mục lớn):** `24px`, font-weight `700`, line-height `1.35`.
- **Card Title (Tiêu đề card):** `16px`, font-weight `600`, line-height `1.40`.
- **Price Value (Giá trị tiền tệ):** `22px`, font-weight `700`, màu chữ `colors.accent-orange`.
- **Body LG (Nội dung lớn):** `16px`, font-weight `400`, line-height `1.60`.
- **Body (Nội dung thường):** `14px`, font-weight `400`, line-height `1.55`.

---

## 4. Các Thành phần Giao diện Chuẩn (Standard UI Components)

1. **Bộ lọc tìm kiếm (`hero-search-bar`):**
   - Đặt tại trung tâm Hero Banner, màu nền `colors.canvas`, bo góc `xl`, padding `24px`, kèm bóng đổ nhẹ nhàng.
2. **Thẻ sản phẩm (`product-card`):**
   - Nền màu trắng, bo góc `lg`, viền ngoài mỏng `hairline`, ảnh luôn cố định tỉ lệ vàng `4:3` để không bị bóp méo hình ảnh phong cảnh.
3. **Nút kêu gọi hành động (`button-book-now`):**
   - Màu nền `accent-orange`, chữ màu trắng, bo góc `full` (viên thuốc), padding `12px 24px` để thu hút sự chú ý tối đa của khách hàng.
4. **Lịch trình thời gian (`itinerary-timeline`):**
   - Thiết kế đứng dọc với marker số ngày nổi bật, kết nối bằng nét đứt mỏng nhẹ, có khoảng cách thoáng để tạp chí du lịch trông sang trọng.
5. **Thanh đặt tour di động (`bottom-sticky-bar`):**
   - Nằm sát cạnh dưới màn hình di động, hiển thị tổng tiền ở bên trái và nút đặt ở bên phải nhằm tăng tính tiện dụng tối đa trên mobile.
