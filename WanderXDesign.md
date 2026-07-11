version: 1.0-travel
name: TravelHub-Design-System
description: "Khung giao diện mang phong cách tạp chí du lịch cao cấp và hiện đại. Hệ thống sử dụng tone màu chủ đạo là xanh đại dương sâu (Deep Ocean Blue) mang lại cảm giác tin cậy, kết hợp với màu cam hoàng hôn (Sunset Orange) được lấy cảm hứng từ năng lượng của những chuyến đi. Trải nghiệm tập trung vào việc hiển thị hình ảnh điểm đến sắc nét, bộ lọc tìm kiếm thông minh và quy trình đặt dịch vụ (Booking) mượt mà, giảm thiểu tối đa rác thị giác."

colors:
  primary: "#0f172a"          # Xanh đại dương sâu (Slate 900) - Thanh lịch, cao cấp
  on-primary: "#ffffff"
  ink: "#0f172a"             # Màu chữ chính
  canvas: "#ffffff"          # Nền trang web chính
  surface-soft: "#f8fafc"    # Nền bọc thẻ (Card), khu vực tìm kiếm nhẹ nhàng (Slate 50)
  surface-mid: "#f1f5f9"     # Nền khi hover hoặc phân tách nhẹ (Slate 100)
  hairline: "#e2e8f0"        # Đường viền mỏng mảnh cho Card và Input (Slate 200)
  muted-fg: "#64748b"        # Chữ phụ, metadata, icon chưa active (Slate 500)
  
  # Màu điểm nhấn (Accent) - Dành riêng cho Nút Đặt ngay (Book Now), Thao tác chính, Điểm nổi bật
  accent-orange: "#ff5a5f"   # Cam hồng hoàng hôn (Giống tone màu kích thích hành động du lịch)
  on-accent-orange: "#ffffff"
  accent-orange-soft: "#fff1f2"

  # Màu định danh loại hình dịch vụ (Thay thế cho các Channel cũ)
  type-tour: "#0284c7"       # Xanh da trời cho Tour du lịch
  type-hotel: "#10b981"      # Xanh lá cho Khách sạn/Nơi ở
  type-flight: "#6366f1"     # Tím xanh cho Vé máy bay
  type-combo: "#f59e0b"      # Vàng hổ phách cho Combo trọn gói

  semantic-success: "#22c55e" # Đã thanh toán / Đặt chỗ thành công
  semantic-warning: "#eab308" # Chờ thanh toán / Giữ chỗ sắp hết hạn
  semantic-error: "#ef4444"   # Đã hủy / Hết chỗ

typography:
  display-xl:
    fontFamily: "Plus Jakarta Sans", "Inter", sans-serif # Font hiện đại, bo tròn nhẹ hợp du lịch
    fontSize: 48px
    fontWeight: 800
    lineHeight: 1.15
    letterSpacing: -1.00px
  display-lg:
    fontFamily: "Plus Jakarta Sans", "Inter", sans-serif
    fontSize: 36px
    fontWeight: 700
    lineHeight: 1.20
  headline:
    fontFamily: "Plus Jakarta Sans", "Inter", sans-serif
    fontSize: 24px
    fontWeight: 700
    lineHeight: 1.35
  card-title:
    fontFamily: "Plus Jakarta Sans", "Inter", sans-serif
    fontSize: 16px
    fontWeight: 600
    lineHeight: 1.40
  body-lg:
    fontFamily: "Inter", sans-serif
    fontSize: 16px
    fontWeight: 400
    lineHeight: 1.60
  body:
    fontFamily: "Inter", sans-serif
    fontSize: 14px
    fontWeight: 400
    lineHeight: 1.55
  price-value:
    fontFamily: "Plus Jakarta Sans", "Inter", sans-serif
    fontSize: 22px
    fontWeight: 700
    lineHeight: 1.20
    textColor: "{colors.accent-orange}"

rounded:
  sm: 6px
  md: 12px     # Tăng độ bo góc để giao diện du lịch trông thân thiện, mượt mà hơn
  lg: 16px     # Dành cho các Card Tour, Card Khách sạn
  xl: 24px     # Dành cho Search Bar lớn ở Hero Banner
  full: 9999px # Dành cho nút tròn, tag ưu đãi

components:
  # Bộ lọc tìm kiếm thông minh (Trọng tâm UX du lịch)
  hero-search-bar:
    backgroundColor: "{colors.canvas}"
    rounded: "{rounded.xl}"
    padding: 24px
    boxShadow: "0 10px 25px -5px rgba(0, 0, 0, 0.05), 0 8px 10px -6px rgba(0, 0, 0, 0.05)"
    border: "1px solid {colors.hairline}"
    
  # Thẻ sản phẩm (Tour/Khách sạn)
  product-card:
    backgroundColor: "{colors.canvas}"
    rounded: "{rounded.lg}"
    border: "1px solid {colors.hairline}"
    overflow: "hidden"
    imageRatio: "4/3" # Tỷ lệ vàng hiển thị ảnh du lịch lung linh
    
  # Nút đặt hành động chính (Kích thích chuyển đổi)
  button-book-now:
    backgroundColor: "{colors.accent-orange}"
    textColor: "{colors.on-accent-orange}"
    typography: "{typography.button}"
    rounded: "{rounded.full}" # Dạng pill/full để tạo cảm giác mời gọi, mềm mại
    padding: 12px 24px
    fontWeight: 600

  button-secondary:
    backgroundColor: "{colors.surface-soft}"
    textColor: "{colors.primary}"
    border: "1px solid {colors.hairline}"
    rounded: "{rounded.full}"
    padding: 12px 24px

  badge-promo:
    backgroundColor: "{colors.accent-orange-soft}"
    textColor: "{colors.accent-orange}"
    rounded: "{rounded.sm}"
    padding: 4px 8px
    fontSize: 12px
    fontWeight: 600