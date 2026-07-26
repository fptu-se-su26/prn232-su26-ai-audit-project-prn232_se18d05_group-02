# AI Audit Log

## 1. Thông tin chung

| Thông tin               | Nội dung                                                            |
|-------------------------|---------------------------------------------------------------------|
| Môn học                 | Building Cross-Platform Back-End Application With .NET              |
| Mã môn học              | PRN232                                                              |
| Lớp                     | SE18D05                                                             |
| Học kỳ                  | SU26                                                                |
| Tên bài tập / Project   | Group Project                                                       |
| Tên sinh viên / Nhóm    | Group 2                                                             |
| MSSV / Danh sách MSSV   | DE180158, DE180166, DE180127                                        |
| Giảng viên hướng dẫn    | Lê Thiện Nhật Quang                                                 |
| Ngày bắt đầu            | 2026-05-12                                                          |
| Ngày hoàn thành         |                                                                     |

---

## 2. Công cụ AI đã sử dụng

Đánh dấu các công cụ AI đã sử dụng trong quá trình thực hiện bài tập/project.

- [X] ChatGPT
- [X] Gemini
- [X] Claude
- [X] GitHub Copilot
- [ ] Cursor
- [X] Antigravity
- [ ] Perplexity
- [ ] Microsoft Copilot
- [ ] Công cụ khác: ....................................

---

## 3. Mục tiêu sử dụng AI

Mô tả ngắn gọn sinh viên/nhóm đã sử dụng AI để hỗ trợ những công việc nào.

Ví dụ:

- Phân tích yêu cầu bài toán
- Gợi ý ý tưởng giải pháp
- Thiết kế database
- Thiết kế giao diện
- Viết code mẫu
- Debug lỗi
- Tối ưu code
- Viết test case
- Kiểm tra bảo mật
- Viết báo cáo
- Chuẩn bị slide thuyết trình
- Tìm hiểu công nghệ mới

### Mô tả mục tiêu sử dụng AI

### Lần sử dụng AI số 1

| Nội dung                    | Thông tin                                                         |
|-----------------------------|-------------------------------------------------------------------|
| Ngày sử dụng                | 17/05/2026                                                        |
| Công cụ AI                  | ChatGPT / Gemini / Antigravity                                    |
| Mục đích sử dụng            | Generate project screen design and suggest models                 |
| Phần việc liên quan         | Requirement / Design / Database                                   |
| Mức độ sử dụng              | Hỗ trợ một phần                                                   |

#### 4.1. Prompt đã sử dụng

```text
PROMPTS.md #Prompt-01
```

#### 4.2. Kết quả AI gợi ý

```text
1. Public / Customer Screens
    1.	Home Screen 
    2.	Tour List Screen 
    3.	Tour Detail Screen 
    4.	Tour Schedule Detail Screen 
    5.	Tour Booking Screen 
    6.	Booking Confirmation Screen 
    7.	Payment Screen 
    8.	Invoice / Booking Receipt Screen 
    9.	My Bookings Screen 
    10.	Booking Detail Screen 
    11.	Cancel Booking Request Screen 
    12.	Customer Profile Screen 
    13.	Edit Profile Screen 
    14.	Travel Companions Screen 
    15.	Special Request Screen 
    16.	My Special Requests Screen 
    17.	Tour Review Screen 
    18.	Tour Reviews Screen 
    19.	Travel Style Quiz Screen 
    20.	Recommended Tours Screen 
2. Admin / Staff Screens
    21.	Admin Dashboard Screen 
    22.	Tour Management Screen 
    23.	Create Tour Screen 
    24.	Edit Tour Screen 
    25.	Tour Pricing Management Screen 
    26.	Seasonal Pricing Screen 
    27.	Promotion Management Screen 
    28.	Tour Itinerary Management Screen 
    29.	Tour Schedule Management Screen 
    30.	Booking Management Screen 
    31.	Create Booking Screen 
    32.	Edit Booking Screen 
    33.	Booking Status Management Screen 
    34.	Cancel Booking Approval Screen 
    35.	Payment Management Screen 
    36.	Customer Management Screen 
    37.	Special Request Management Screen 
    38.	Review Management Screen 
3. Guide / Operation Screens
    39.	Guide Management Screen 
    40.	Create Guide Screen 
    41.	Edit Guide Profile Screen 
    42.	Guide Assignment Screen 
    43.	Guide Schedule Screen 
    44.	Guide Portal Screen 
    45.	Assigned Tour Detail Screen 
    46.	Reject Assigned Tour Screen 
    47.	Guide Status Management Screen 
4. System / Account Screens
    48.	Login Screen 
    49.	Register Screen 
    50.	SMS Verification Screen 
    51.	Forgot Password Screen 
    52.	Account Management Screen 
    53.	Role Permission Management Screen 
    54.	Account Lock / Unlock Screen 
    55.	Notification Management Screen 
5. Report / Statistic Screens
    56.	Revenue Statistics Screen 
    57.	Monthly Booking Statistics Screen 
    58.	Open Tour Statistics Screen 
    59.	Top Guide Ranking Screen

```

#### 4.3. Phần sinh viên/nhóm đã sử dụng từ AI

```text
Sử dụng danh sách các screen gợi ý bởi AI để có thể sơ lược và tiến hành một danh sách UI sát với yêu cầu của dự án.
```

#### 4.4. Phần sinh viên/nhóm tự chỉnh sửa hoặc cải tiến

```text
1. Public / Customer Screens
    1.	Home Screen 
    2.	Tour List Screen 
    3.	Tour Detail Screen 
        •Tour schedule detail 
        •Tour price / promotion 
        •Tour reviews 
        •Tour booking form 
    4.	Booking Confirmation Screen 
    5.	Payment Screen 
        •Full payment 
        •Deposit payment 
        •Remaining amount 
    6.	Invoice / Booking Receipt Screen 
    7.	My Bookings Screen 
    8.	Booking Detail Screen 
        •Booking status tracking 
        •Cancel booking request 
        •Payment information 
    9.	Customer Profile Screen 
        •View profile 
        •Edit profile 
    10.	Travel Companions Screen 
        •Add companion 
        •Edit companion 
        •Delete companion 
    11.	Special Requests Screen 
        •Create special request 
        •View request list 
        •Track request status 
    12.	Tour Review Screen 
        •Submit rating and feedback 
        •View submitted reviews 
    13.	Travel Style Quiz Screen 
        •Answer quiz 
        •Save quiz result 
    14.	Recommended Tours Screen 
        •Recommended by quiz 
        •Filter by budget, destination, type 
        •Random tour 

2. Admin / Staff Screens
    15.	Admin Dashboard Screen 
        •Open tour statistics 
        •Monthly booking statistics 
        •Expected revenue 
        •Top guide ranking 
    16.	Tour Management Screen 
        •View tour list 
        •Search / filter tours 
        •Lock / unlock tour 
        •Hide / delete tour 
    17.	Tour Form Screen 
        •Create tour 
        •Edit tour 
        •Basic tour information 
    18.	Tour Detail Management Screen 
        •Manage itinerary 
        •Manage tour schedule 
        •Change itinerary order 
    19.	Tour Pricing Management Screen 
        •Basic price 
        •Seasonal price 
        •Promotion / discount 
    20.	Booking Management Screen 
        •View booking list 
        •Search / filter booking 
        •Create booking 
        •Edit booking 
        •Link booking with tour schedule 
    21.	Booking Detail Management Screen 
        •Update booking status 
        •View status history 
        •View payment information 
        •Send notification when tour is cancelled 
    22.	Cancel Booking Approval Screen 
        •View cancel requests 
        •Approve / reject request 
        •Save cancellation reason 
    23.	Payment Management Screen 
        •Check payment method 
        •Track deposit / remaining payment 
        •Export invoice / booking receipt 
    24.	Customer Management Screen 
        •View customer list 
        •View customer profile 
        •View customer bookings 
    25.	Special Request Management Screen 
        •View customer service requests 
        •Update request status 
        •Filter requests by status 
    26.	Review Management Screen 
        •View customer reviews 
        •Manage review list 

3. Guide / Operation Screens
    27.	Guide Management Screen 
        •View guide list 
        •Add guide 
        •Edit guide information 
        •Update language, area, experience 
        •Update guide status 
    28.	Guide Assignment Screen 
        •Assign guide to tour 
        •Change assigned guide 
        •Check guide availability 
    29.	Guide Schedule Screen 
        •View guide work schedule 
        •View busy / available status 
    30.	Guide Portal Screen 
        •View assigned tours 
        •View tour details 
        •Reject assigned tour 
        •Update tour status from confirmed to finished 
    31.	Assigned Tour Detail Screen 
        •Tour information 
        •Customer list 
        •Schedule detail 
        •Special requests 

4. System / Account Screens
    32.	Login Screen 
    33.	Register Screen 
    34.	SMS Verification Screen 
    35.	Forgot Password Screen 
    36.	Account Management Screen 
        •View account list 
        •Lock / unlock account 
        •Manage account status 
    37.	Role Permission Management Screen 
        •Manage Customer role 
        •Manage Staff role 
        •Manage Admin role 
        •Manage Guide role 
    38.	Notification Management Screen 
        •View sent notifications 
        •Manage email / SignalR notifications

```

#### 4.5. Minh chứng

| Loại minh chứng         | Nội dung                                                                     |
|-------------------------|------------------------------------------------------------------------------|
| Link commit             |                                                                              |
| File liên quan          |                                                                              |
| Screenshot              | ../img/AI_AUDIT_LOG/prompt1_proof1.png                                       |
| Kết quả chạy/test       |                                                                              |
| Link video demo         |                                                                              |
| Ghi chú khác            |                                                                              |

#### 4.6. Nhận xét cá nhân/nhóm

```text
Sau lần sử dụng AI đầu tiên này để phác thảo danh sách màn hình của hệ thống, nhóm đã rút ra được một số bài học kinh nghiệm quý giá:

1. AI chỉ là 1 công cụ để trực quan hóa các yêu cầu ban đầu của nhóm, còn việc thiết kế và phát triển vẫn cần nhóm xem lại và chỉnh sửa cho phù hợp với yêu cầu thực tế.
 
```

---

### Lần sử dụng AI số 2

| Nội dung | Thông tin |
|---|---|
| Ngày sử dụng | 25/05/2026 |
| Công cụ AI | GitHub Copilot / ChatGPT |
| Mục đích sử dụng | Implement Guide Management Backend Features |
| Phần việc liên quan | Backend / Database |
| Mức độ sử dụng | Hỗ trợ một phần |

#### 4.1. Prompt đã sử dụng

```text
Tiến hành chức năng Quản lý hướng dẫn viên:
1. Thêm hướng dẫn viên
2. Cập nhật thông tin hướng dẫn viên (hướng dẫn viên có thể tự cập nhật)
3. Cập nhật ngôn ngữ (chuyên ngành ngôn ngữ của hướng dẫn viên), khu vực và kinh nghiệm (đi hướng dẫn được bao nhiêu tour rồi?)

Yêu cầu:
- Tạo API endpoint cho các chức năng trên
- Implement logic xử lý trong backend (.NET 8)
- Thiết kế database schema cho guide management
- Ensure validation và authorization (guides chỉ có thể edit profile của mình)
```

#### 4.2. Kết quả AI gợi ý

```text
1. Database Schema:
   - Table: Guides
     * GuidId (PK)
     * FullName
     * PhoneNumber
     * Email
     * Languages (JSON/String array)
     * ServiceAreas (JSON/String array)
     * ExperienceTours (số tour đã hướng dẫn)
     * TotalRating
     * Status (Active/Inactive/OnLeave)
     * CreatedDate
     * UpdatedDate

2. API Endpoints:
   - POST /api/guides - Create new guide
   - GET /api/guides/{id} - Get guide details
   - PUT /api/guides/{id} - Update guide information
   - PATCH /api/guides/{id}/profile - Guide tự cập nhật profile
   - GET /api/guides - List all guides with filters
   - PATCH /api/guides/{id}/languages - Update guide languages
   - PATCH /api/guides/{id}/areas - Update service areas
   - PATCH /api/guides/{id}/experience - Update experience info

3. Request/Response Models:
   - CreateGuideRequest
   - UpdateGuideRequest
   - GuideProfileUpdateRequest
   - GuideDto
   - GuideLanguageDto
   - GuidExperienceDto

4. Validation Rules:
   - Email validation
   - Phone number validation
   - At least one language required
   - At least one service area required
   - Experience count should be non-negative

5. Authorization:
   - Admin: Full access to all guide operations
   - Staff: Can create and manage guides
   - Guide: Can only update their own profile information
```

#### 4.3. Phần sinh viên/nhóm đã sử dụng từ AI

```text
1. Database schema design và model structure cho Guide management
2. API endpoint structure và routing convention
3. Request/Response DTO models
4. Validation logic và authorization checks
5. Repository pattern implementation cho guide data access
6. Service layer design cho business logic
7. Exception handling strategy
```

#### 4.4. Phần sinh viên/nhóm tự chỉnh sửa hoặc cải tiến

```text
1. Thêm fields: Avatar, Bio, Certification, LanguageProficiency (level)
2. Tối ưu Service Areas: thay vì string array, tạo separate table GuideServiceArea để quản lý Many-to-Many relationship
3. Tối ưu Languages: tạo separate table GuideLanguage với proficiency level
4. Thêm Rating/Review tracking: GuidRating table để track average rating
5. Audit trail: Thêm logging khi cập nhật guide information
6. Thêm status management: OnLeave, Inactive states có effective dates
7. Performance optimization: Implement caching cho guide list
8. Enhance query filters: Search by name, email, language, area, experience range
9. Thêm guide assignment history tracking
```

#### 4.5. Minh chứng

| Loại minh chứng | Nội dung |
|---|---|
| Link commit | WanderXServer/Repositories/ (Guide-related repositories) |
| File liên quan | WanderXServer/Models/Guide.cs, WanderXServer/Services/GuideService.cs, WanderXServer/Controllers/GuideController.cs |
| Screenshot | ../img/AI_AUDIT_LOG/ai-use-2-guide-management.png |
| Kết quả chạy/test | API endpoints tested via Postman/Swagger |
| Link video demo | N/A |
| Ghi chú khác | Xem CHANGELOG.md cho chi tiết implementation |

#### 4.6. Nhận xét cá nhân/nhóm

```text
Lần sử dụng AI thứ 2 này cho phép nhóm nhanh chóng thiết kế database schema và API structure cho Guide Management module. 
AI cung cấp một foundation tốt về:

1. RESTful API design conventions
2. DTO pattern và validation approach
3. Authorization strategy cho role-based access control

Tuy nhiên, nhóm phải thực hiện các cải tiến quan trọng:

1. Tối ưu database design bằng cách tạo separate tables cho Languages và Service Areas thay vì using JSON arrays
2. Thêm nhiều fields thực tế hơn như Avatar, Bio, Certification, LanguageProficiency level
3. Implement audit logging để track thay đổi
4. Optimize queries và implement caching strategies
5. Enhance validation logic phù hợp hơn với yêu cầu business

Điều này chứng minh rằng AI là công cụ hỗ trợ tốt nhưng vẫn cần human expertise để tạo ra solution hoàn chỉnh và production-ready.
```

---

### Lần sử dụng AI số 3

| Nội dung | Thông tin |
|---|---|
| Ngày sử dụng | 05/06/2026 |
| Công cụ AI | ChatGPT / Antigravity |
| Mục đích sử dụng | Chỉnh sửa và hoàn thiện UX cho trang Guide Portal |
| Phần việc liên quan | Frontend |
| Mức độ sử dụng | Hỗ trợ một phần |

#### 4.1. Prompt đã sử dụng

```text
Khi nhấn vào 1 tour bên trong calendar thì sẽ nhảy xuống tour phía dưới Upcoming assigned tour(s). Thêm filter theo tour status và filter bằng cách chọn ngày trên lịch.
```

#### 4.2. Kết quả AI gợi ý

```text
AI gợi ý các hướng giải quyết:

1. Kiến trúc:
   - Sử dụng event listener trên calendar elements
   - Implement smooth scroll tới section Upcoming assigned tours
   - State management: selectedDate, selectedStatus, filteredTours

2. Cơ chế hoạt động:
   - Click trên date → lưu selectedDate vào state
   - Filter tours: array.filter(tour => tour.date === selectedDate && tour.status === selectedStatus)
   - Scroll: element.scrollIntoView({behavior: 'smooth'})

3. UI Components:
   - Calendar (có highlight selected date)
   - Filter chips/buttons cho status (Assigned, Confirmed, In Progress, Finished)
   - Upcoming tours list (scroll-to-target)
   - Reset filter button

4. Code patterns:
   - React hooks (useState, useEffect, useRef)
   - Event handlers cho calendar clicks
   - Array filtering logic
   - Scroll behavior implementation
```

#### 4.3. Phần sinh viên/nhóm đã sử dụng từ AI

```text
1. Event handling pattern: Click listener trên calendar dates
2. Scroll functionality: scrollIntoView() với smooth behavior
3. Filter logic structure: Multi-criteria filtering (date + status)
4. State management approach: selectedDate và selectedStatus state
5. Component architecture: Tách filter logic từ display logic
```

#### 4.4. Phần sinh viên/nhóm tự chỉnh sửa hoặc cải tiến

```text
1. UI/UX enhancements:
   - Tinh chỉnh smooth scroll timing (duration, easing)
   - Thêm visual indicators: highlight selected date, bold active filter
   - Improve responsive design cho mobile
   - Thêm animation transitions cho better UX

2. Feature refinements:
   - Lọc theo date và status đồng thời (multi-filter)
   - Hiển thị tour count cho mỗi status
   - Thêm "Clear All Filters" button
   - Persist filter state khi navigate away và quay lại

3. Performance optimizations:
   - Memoize filtered tours (useMemo) để tránh unnecessary re-renders
   - Lazy load tour details khi cần
   - Debounce filter updates

4. Accessibility improvements:
   - Keyboard navigation cho calendar
   - ARIA labels cho filter buttons
   - Screen reader support cho scroll-to functionality
```

#### 4.5. Minh chứng

| Loại minh chứng | Nội dung |
|---|---|
| Link commit | WanderXClient/Pages/GuidePage.razor |
| File liên quan | WanderXClient/Components/GuidePortal.razor, WanderXClient/js/guide-calendar.js |
| Screenshot |  |
| Kết quả chạy/test | Tested calendar click, date filter, status filter, scroll behavior |
| Link video demo | N/A |
| Ghi chú khác | Feature hoàn thiện trong phase cuối của Guide Portal implementation |

#### 4.6. Nhận xét cá nhân/nhóm

```text
Lần sử dụng AI thứ 3 này cho phép nhóm nhanh chóng thiết kế UX flow cho Guide Portal calendar feature:

1. Ưu điểm:
   - AI cung cấp rõ kiến trúc state management
   - Gợi ý cách implement scroll behavior hiệu quả
   - Gợi ý multi-filter logic

2. Điều chỉnh nhóm:
   - Tinh chỉnh UX interaction để mượt hơn
   - Thêm performance optimization (memoization)
   - Enhance accessibility
   - Cải thiện responsive design cho mobile

3. Bài học:
   - AI là tốt cho brainstorming UI interactions
   - Nhưng cần human judgment cho UX details
   - Performance và accessibility không phải lúc nào AI cũng suggest
```

---

### Lần sử dụng AI số 4

| Nội dung | Thông tin |
|---|---|
| Ngày sử dụng | 16/06/2026 |
| Công cụ AI | Gemini |
| Mục đích sử dụng | Responsive Design Implementation for Mobile & Laptop |
| Phần việc liên quan | Frontend |
| Mức độ sử dụng | Hỗ trợ chính |

#### 4.1. Prompt đã sử dụng

```text
Nhằm đáp ứng nhu cầu cho người dùng sử dụng phone để duyệt web, hãy làm responsive cho trang web cho cả laptop và mobile.
Đảm bảo nâng cao UI/UX cho người dùng.
Đảm bảo các chức năng và giao diện trên phiên bản laptop thì vẫn hoạt động bình thường.
Đảm bảo hạn chế ảnh hưởng đến các thành phần khác nếu không cần thiết.
```

#### 4.2. Kết quả AI gợi ý

```text
Gemini gợi ý một chiến lược responsive design toàn diện:

1. CSS Responsive Framework:
   - Sử dụng CSS Media Queries với breakpoints: Mobile (320-480px), Tablet (481-768px), Desktop (769px+)
   - Implement CSS Grid và Flexbox cho layout adaptable
   - Sử dụng relative units (rem, em, %) thay vì fixed units (px)
   - Implement CSS Custom Properties (variables) cho consistent theming

2. Mobile-First Approach:
   - Bắt đầu từ mobile layout, sau đó enhance cho tablet và desktop
   - Progressive enhancement cho các tính năng advanced
   - Optimize images với srcset và picture elements

3. UI/UX Improvements cho Mobile:
   - Tăng touch targets lên 44px-48px minimum
   - Simplify navigation: hamburger menu, bottom navigation tabs
   - Implement collapsible sections và accordion layouts
   - Optimize form layouts với larger input fields
   - Implement sticky headers và footers cho quick access

4. Performance Optimization:
   - Lazy load images để reduce initial load time
   - Minimize CSS/JS bundles
   - Implement code splitting
   - Cache strategy cho assets static
   - Optimize font loading

5. Testing Strategy:
   - Test trên multiple real devices (iOS, Android)
   - Use Chrome DevTools device emulation
   - Performance testing với Lighthouse
   - Touch event testing
   - Orientation change testing (portrait/landscape)

6. Component-Level Responsive:
   - Navigation (responsive menu)
   - Cards (2-3 columns desktop, 1 column mobile)
   - Forms (single column mobile, multi-column desktop)
   - Images (full width mobile, constrained desktop)
   - Modals (full screen mobile, centered desktop)
   - Tables (scroll horizontally mobile, normal desktop)
```

#### 4.3. Phần sinh viên/nhóm đã sử dụng từ AI

```text
1. CSS Framework Implementation:
   - Tạo responsive breakpoints global trong _variables.scss
   - Implement media query mixins cho reusable responsive styles
   - Refactor CSS để sử dụng Flexbox và Grid
   - Convert fixed widths thành relative widths

2. Layout Responsive:
   - Navigation: Convert thành hamburger menu trên mobile
   - Sidebar: Convert thành collapsible drawer trên mobile
   - Grid layouts: Adjust columns dựa trên breakpoints
   - Forms: Optimize cho touch interaction

3. Mobile-Specific UI:
   - Bottom navigation tabs cho main features
   - Larger buttons và input fields (min 44px)
   - Sticky header với back button
   - Drawer menu thay vì sidebar
   - Simplified forms với fewer fields per view

4. Image Optimization:
   - Implement responsive images với srcset
   - Lazy loading cho images
   - Optimize image sizes cho different devices
   - Use appropriate image formats (WebP with fallback)

5. Component Updates:
   - Tour Card: Stack vertically mobile, horizontal desktop
   - Calendar: Simplified view mobile, full calendar desktop
   - Booking Form: Step-by-step mobile, multi-column desktop
   - Guide Portal: Responsive tables với horizontal scroll
```

#### 4.4. Phần sinh viên/nhóm tự chỉnh sửa hoặc cải tiến

```text
1. Smart Component Architecture:
   - Create responsive wrappers cho existing components
   - Avoid component duplication (use CSS + logic instead)
   - Maintain single source of truth
   - Use conditional rendering only when necessary

2. Performance Fine-tuning:
   - Implement intersection observer cho lazy loading
   - Optimize re-renders với React.memo, useMemo, useCallback
   - Reduce bundle size bằng code splitting
   - Implement virtual scrolling cho long lists

3. Enhanced UX Patterns:
   - Add micro-interactions (swipe, bounce effects)
   - Implement pull-to-refresh cho mobile
   - Add haptic feedback support
   - Smooth scroll-to-top functionality
   - Sticky footer CTA buttons

4. Accessibility Improvements:
   - Ensure touch targets meet 48px minimum
   - Improve color contrast cho readability
   - Add ARIA labels cho responsive components
   - Ensure keyboard navigation works
   - Test screen reader compatibility

5. Testing & Validation:
   - Manual testing trên 5+ real devices
   - Automated responsive testing
   - Performance profiling với Lighthouse
   - Cross-browser testing
   - User feedback collection

6. Documentation:
   - Create responsive design guidelines
   - Document breakpoint strategy
   - Provide component responsive examples
   - Add troubleshooting guide
```

#### 4.5. Minh chứng

| Loại minh chứng | Nội dung |
|---|---|
| Link commit | WanderXClient/Shared/Styles/, WanderXClient/Components/ responsive updates |
| File liên quan | _layout.scss, _variables.scss, responsive component files |
| Screenshot | Responsive demo screenshots (mobile 375px, tablet 768px, desktop 1920px) |
| Kết quả chạy/test | Lighthouse report, responsive test results, device testing log |
| Link video demo | Responsive demo video |
| Ghi chú khác | Tested trên iOS Safari, Android Chrome, responsive down to 320px |

#### 4.6. Nhận xét cá nhân/nhóm

```text
Lần sử dụng AI thứ 4 này cho phép nhóm triển khai responsive design một cách có hệ thống:

1. Ưu điểm:
   - Gemini cung cấp chiến lược responsive toàn diện
   - Gợi ý cụ thể về breakpoints, units, và best practices
   - Provide component-level responsive patterns
   - Bao gồm performance optimization strategies

2. Điều chỉnh nhóm:
   - Smart component architecture để tránh code duplication
   - Performance optimization (intersection observer, memoization)
   - Enhanced UX patterns (micro-interactions, haptic feedback)
   - Comprehensive accessibility improvements
   - Thorough testing trên multiple real devices

3. Bài học:
   - Responsive design cần comprehensive planning, không chỉ media queries
   - Component-level thinking giúp maintain clean architecture
   - Performance và accessibility cần được planned từ đầu
   - Testing trên real devices là critical
```


### Lần sử dụng AI số 5

| Nội dung | Thông tin |
|---|---|
| Ngày sử dụng | 23/06/2026 |
| Công cụ AI | Antigravity / Gemini |
| Mục đích sử dụng | Tái cấu trúc toàn diện UI/UX cho Travel Website (Homepage, Listing, Detail) |
| Phần việc liên quan | Frontend / UI-UX |
| Mức độ sử dụng | Hỗ trợ chính |

#### 4.1. Prompt đã sử dụng

```text
PROMPTS.md #Prompt-05
```

#### 4.2. Kết quả AI gợi ý

```text
AI đã đưa ra phân tích chi tiết về 4 điểm yếu UX kinh điển của web du lịch truyền thống và cung cấp giải pháp thiết kế giao diện hiện đại:

1. Thiết kế Homepage:
   - Hero Section tràn màn hình, ảnh nền chất lượng cao tạo cảm hứng du lịch.
   - Cụm thanh tìm kiếm tích hợp bộ lọc đa năng dạng Tab (Tours, Hotels, Flights) với các trường thông tin gọn gàng.
   - Hiển thị danh mục điểm đến hot dạng Grid hình ảnh 3D hover và danh sách tour giờ chót với tông màu cam nhấn ({colors.accent-orange}).

2. Thiết kế Listing Page:
   - Filter Sidebar đặt bên trái với phân cấp rõ ràng (giá, xếp hạng, loại hình tour), có khoảng trắng thoáng đãng.
   - Product Grid 3 cột sang trọng. Mỗi thẻ sản phẩm (Product Card) bo góc rounded-lg, tỷ lệ ảnh 4:3 sắc nét, hiển thị rõ giá và CTA "Book Now".

3. Thiết kế Detail Page:
   - Phân bổ thông tin rõ ràng: Khối Gallery ảnh (1 lớn + 4 nhỏ) -> Timeline lịch trình tour chi tiết mượt mà -> Sticky Booking Widget cố định bên phải màn hình khi cuộn chuột.

4. Responsive Design:
   - Layout co giãn linh hoạt theo breakpoints của Tailwind.
   - Phiên bản Mobile: chuyển bộ lọc sang dạng Drawer trượt dưới lên, product list về Grid 1 cột, widget đặt tour chuyển thành Bottom Sticky Bar cố định.
```

#### 4.3. Phần sinh viên/nhóm đã sử dụng từ AI

```text
1. Giao diện bộ lọc và danh sách sản phẩm:
   - Áp dụng cấu trúc Flexbox và Grid từ code mẫu để dựng khung Homepage và Listing page.
   - Sử dụng CSS của AI cho Sticky Booking Widget trên desktop.
2. Responsive layout:
   - Sử dụng các class responsive của Tailwind CSS (`md:`, `lg:`) theo gợi ý của AI.
   - Áp dụng cơ chế Drawer trên Mobile để tiết kiệm không gian màn hình.
3. Design tokens:
   - Áp dụng màu cam nhấn `{colors.accent-orange}` (`#FF6B35`) cho giá tiền và các nút CTA nổi bật.
   - Dùng khoảng trắng rộng rãi (`py-12`, `py-16`, `gap-8`) tạo cảm giác cao cấp.
```

#### 4.4. Phần sinh viên/nhóm tự chỉnh sửa hoặc cải tiến

```text
1. Tái cấu trúc mã nguồn (Component Refactoring):
   - Chia nhỏ file code gộp của AI thành các React components riêng biệt trong dự án WanderXClient (`HeroSection.jsx`, `SearchBar.jsx`, `FilterSidebar.jsx`, `ProductCard.jsx`, `BookingWidget.jsx`, `Timeline.jsx`).
2. Tối ưu UX & Hiệu ứng động:
   - Thêm hiệu ứng Skeleton Loading giúp giao diện mượt mà khi tải dữ liệu từ API.
   - Sử dụng transition và transform CSS cho hiệu ứng Hover trên Product Cards phóng to nhẹ ảnh nền mà không làm vỡ layout.
   - Đảm bảo hình ảnh không bị méo bằng thuộc tính `object-cover aspect-[4/3]`.
3. Tích hợp Backend API:
   - Đấu nối dữ liệu thực tế từ cơ sở dữ liệu (Database) thông qua API thay vì dùng mock data tĩnh của AI.
   - Bổ sung logic validation form đặt tour trên Sticky Booking Widget trước khi chuyển hướng sang trang thanh toán.
```

#### 4.5. Minh chứng

| Loại minh chứng | Nội dung |
|---|---|
| Link commit | |
| File liên quan | WanderXClient/Components/SearchBar.jsx, WanderXClient/Components/ProductCard.jsx, WanderXClient/Components/BookingWidget.jsx, WanderXClient/Pages/Home.jsx, WanderXClient/Pages/TourList.jsx, WanderXClient/Pages/TourDetail.jsx |
| Screenshot | Ảnh chụp so sánh UI trước và sau refactor |
| Kết quả chạy/test | Kiểm tra hoạt động mượt mà trên Mobile Safari và Chrome Desktop |
| Link video demo | |
| Ghi chú khác | Hệ thống đạt tiêu chuẩn giao diện tạp chí cao cấp, đáp ứng tốt UX của người dùng |

#### 4.6. Nhận xét cá nhân/nhóm

```text
Lần sử dụng AI thứ 5 giúp định hình phong cách thiết kế UI/UX hiện đại theo chuẩn tạp chí cho dự án:

1. Ưu điểm:
   - AI cung cấp code mẫu Tailwind CSS trực quan, hiện đại, đúng tinh thần của Design System WanderXDesign.md.
   - Phân tích tốt các lỗi UX thường gặp của web cũ giúp nhóm rút kinh nghiệm khi triển khai thực tế.
   - Gợi ý responsive mobile tối ưu (Drawer, Sticky Bottom Bar).

2. Điều chỉnh nhóm:
   - Tách code mẫu thành các React components tái sử dụng được, clean code.
   - Bổ sung hiệu ứng Skeleton loading và mượt mà hóa chuyển động mở Drawer/Hover.
   - Kết nối API dữ liệu thực tế của dự án.

3. Bài học:
   - Tận dụng sức mạnh của CSS Grid/Flexbox giúp layout responsive rất dễ dàng.
   - Khoảng trắng (whitespace) và phân cấp thông tin rõ ràng là chìa khóa tạo nên thiết kế cao cấp (premium feel).
```

### Lần sử dụng AI số 6

| Nội dung                    | Thông tin                                                         |
|-----------------------------|-------------------------------------------------------------------|
| Ngày sử dụng                | 20/07/2026                                                        |
| Công cụ AI                  | ChatGPT / Codex                                                   |
| Mục đích sử dụng            | Implement Member 2 features for Admin/Staff booking operations    |
| Phần việc liên quan         | Backend API / Blazor Frontend / Debug / UI refinement             |
| Mức độ sử dụng              | Hỗ trợ một phần                                                   |

#### 4.1. Prompt đã sử dụng

```text
PROMPTS.md #Prompt-06
```

#### 4.2. Kết quả AI gợi ý

```text
AI đề xuất và hỗ trợ triển khai luồng nghiệp vụ cho các feature của thành viên 2:
1. Booking Management: tạo, xem, sửa booking, quản lý guest list và phân trang.
2. Booking Status Management: ràng buộc chuyển trạng thái Pending -> Confirmed/Cancelled, Confirmed -> Finished/Cancelled, khóa Finished/Cancelled.
3. Cancellation Request Management: user gửi yêu cầu hủy, Admin/Staff duyệt hoặc từ chối bằng popup, gửi email thông báo khi có cấu hình SMTP.
4. Payment Management: ghi nhận full payment, 40% deposit, remaining balance, reference giao dịch, invoice/receipt và khóa chỉnh sửa khi booking đã paid.
5. Debug lỗi thực tế: CSRF token mismatch, lỗi chuyển route Blazor phải refresh, DbUpdateConcurrencyException khi đổi ngày tour, lỗi fetch API và lỗi UI bị tràn số.
```

#### 4.3. Phần sinh viên/nhóm đã sử dụng từ AI

```text
Sử dụng gợi ý của AI để xác định luồng nghiệp vụ, thiết kế DTO/API cần bổ sung, tổ chức lại UI Admin/Staff theo các màn hình Bookings, Cancel Requests và Payments, đồng thời kiểm tra build sau khi hoàn thành.
```

#### 4.4. Phần sinh viên/nhóm tự chỉnh sửa hoặc cải tiến

```text
Trần Hồng Quân (DE180166) đã kiểm tra lại nghiệp vụ theo yêu cầu project và điều chỉnh kết quả AI:
1. Không tạo thêm bảng mới cho cancellation/payment, tận dụng bảng Bookings và bổ sung các cột cần thiết để phù hợp database hiện có.
2. Đồng bộ giao diện các trang Admin/Staff theo layout có sẵn của WanderX, thêm modal/popup thay vì chèn form vào list.
3. Bổ sung phân trang 5 dòng/trang và chỉnh responsive để hạn chế thanh kéo ngang.
4. Khóa các thao tác không hợp lệ theo trạng thái booking/payment.
5. Chuẩn bị phần gửi email qua SMTP để khi cấu hình tài khoản mail thật thì hệ thống có thể gửi thông báo tự động.
```

#### 4.5. Minh chứng

| Loại minh chứng | Nội dung |
|---|---|
| Link commit | |
| File liên quan | WanderXServer/Controllers/BookingsController.cs, WanderXServer/Services/BookingService.cs, WanderXServer/Services/UserService.cs, WanderXClient/WanderXClient/Pages/AdminBookings.razor, WanderXClient/WanderXClient/Pages/AdminCancellationRequests.razor, WanderXClient/WanderXClient/Pages/AdminPayments.razor |
| Screenshot | Test trực tiếp các màn hình Admin Bookings, Cancel Requests, Payments |
| Kết quả chạy/test | `dotnet build` backend và frontend thành công sau khi fix lỗi |
| Link video demo | |
| Ghi chú khác | Các feature FE1-FE4 của Member 2 đã được triển khai theo nghiệp vụ Admin/Staff |

#### 4.6. Nhận xét cá nhân/nhóm

```text
AI hỗ trợ tốt trong việc bóc tách nghiệp vụ và xử lý lỗi phát sinh khi tích hợp frontend Blazor với backend API. Sinh viên vẫn phải kiểm tra lại luồng trạng thái, dữ liệu thực tế trong database, cấu trúc project hiện có và điều chỉnh UI để phù hợp với hệ thống WanderX.
```

---

### Lần sử dụng AI số 7

| Nội dung                    | Thông tin                                                         |
|-----------------------------|-------------------------------------------------------------------|
| Ngày sử dụng                | 12/07/2026 – 22/07/2026                                           |
| Công cụ AI                  | Antigravity / Claude (Sonnet 4.6)                                 |
| Mục đích sử dụng            | Implement Member 3 features: Customer Management & Tour Reviews   |
| Phần việc liên quan         | Backend API / Blazor Frontend / Debug / Database                  |
| Mức độ sử dụng              | Hỗ trợ một phần                                                   |

#### 4.1. Prompt đã sử dụng

```text
PROMPTS.md #Prompt-07
```

#### 4.2. Kết quả AI gợi ý

```text
AI hỗ trợ triển khai 4 feature của thành viên 3:

1. Feature 1 – Quản lý hồ sơ khách hàng:
   - Profile.razor: form xem/sửa thông tin, đổi mật khẩu, hiển thị avatar.
   - API endpoint: GET/PUT api/users/profile?email=...

2. Feature 2 – Theo dõi booking cá nhân:
   - MyBookings.razor: danh sách booking theo email, phân trang 5/trang có số trang.
   - BookingDetail.razor: chi tiết booking + timeline 4 bước (Paid→Confirmed→Completed) + hủy.
   - API endpoint: GET api/users/bookings, GET api/users/bookings/{id}, PUT cancel.

3. Feature 3 – Yêu cầu dịch vụ đặc biệt:
   - ServiceUserRequest.razor: khách tạo/xem yêu cầu, phân trang.
   - AdminServiceRequests.razor: admin drill-down booking → người → dịch vụ → duyệt/từ chối.
   - API: GET/POST api/users/special-requests; admin endpoints trong UsersController.

4. Feature 4 – Đánh giá tour:
   - TourReview.razor: form 1–5 sao + nhận xét, block nếu chưa CompletedAt, sửa/xóa.
   - AdminTourReviews.razor: bảng tất cả đánh giá, moderate Visible/Hidden/Deleted.
   - TourReviewsController: explicit route [Route("api/tourreviews")], 204 khi chưa có review.
   - TourReviewService: validate bằng tiếng Việt, load lại entity sau SaveChanges.

5. Debug lỗi:
   - URL cache cũ api/tour_review → đổi thành api/tourreviews, Hard Reload.
   - GetByBookingId Ok(null) → client JsonException → đổi thành 204 NoContent.
   - catch(Exception){} silent → thêm _errorMessage hiển thị lỗi lên UI.
   - Closure bug @for star rating → var starIndex = i.
   - Email query string thay JWT để tránh lỗi AuthenticationScheme chưa cấu hình.
```

#### 4.3. Phần sinh viên/nhóm đã sử dụng từ AI

```text
1. Cấu trúc API endpoint cho tất cả 4 feature.
2. Code mẫu TourReviewsController, TourReviewService, TourReview.razor, AdminTourReviews.razor, AdminServiceRequests.razor.
3. Chiến lược dùng email query string để định danh user thay JWT.
4. Logic phân trang có số trang bấm được (TotalPages, CurrentPage).
5. Phân tích và fix 4 lỗi runtime quan trọng sau khi build thành công.
```

#### 4.4. Phần sinh viên/nhóm tự chỉnh sửa hoặc cải tiến

```text
Võ Quang Đăng Khoa (DE180127) kiểm tra lại nghiệp vụ và điều chỉnh kết quả AI:

1. Không tạo thêm bảng mới – tận dụng TourReviews và UserSpecialRequests có sẵn trong DB schema.
2. Đồng bộ giao diện Profile, MyBookings, BookingDetail, ServiceUserRequest theo layout WanderX hiện có (sidebar menu bên trái như các trang khác của khách hàng).
3. Thêm error message box màu đỏ trên TourReview.razor thay vì để UI im lặng khi thất bại.
4. Đổi GetByBookingId trả 204 NoContent thay Ok(null) để client không bị crash khi parse JSON null.
5. Kiểm tra và sửa closure bug trong vòng lặp @for tạo 5 sao đánh giá (dùng var starIndex = i).
6. Viết lại TourReviewService với error message bằng tiếng Việt để UI hiển thị thân thiện.
7. Kiểm tra build backend + frontend (dotnet build) sau mỗi lần thay đổi để đảm bảo 0 lỗi.
```

#### 4.5. Minh chứng

| Loại minh chứng         | Nội dung                                                                                       |
|-------------------------|-----------------------------------------------------------------------------------------------|
| Link commit             |                                                                                               |
| File liên quan          | WanderXServer/Controllers/TourReviewsController.cs, WanderXServer/Services/TourReviewService.cs, WanderXClient/WanderXClient/Pages/TourReview.razor, WanderXClient/WanderXClient/Pages/AdminTourReviews.razor, WanderXClient/WanderXClient/Pages/AdminServiceRequests.razor, WanderXClient/WanderXClient/Pages/MyBookings.razor, WanderXClient/WanderXClient/Pages/BookingDetail.razor, WanderXClient/WanderXClient/Pages/ServiceUserRequest.razor, WanderXClient/WanderXClient/Pages/Profile.razor |
| Screenshot              |                                                                                               |
| Kết quả chạy/test       | dotnet build backend + frontend: 0 error. Test thủ công toàn bộ luồng 4 feature.             |
| Link video demo         |                                                                                               |
| Ghi chú khác            | Xem CHANGELOG.md Phase 04.3 và PROMPTS.md Prompt-07 để biết chi tiết                         |

#### 4.6. Nhận xét cá nhân/nhóm

```text
Lần sử dụng AI thứ 7 này hỗ trợ tốt cho thành viên 3 triển khai toàn bộ luồng Customer Management:

1. Ưu điểm:
   - AI phân tích đúng cấu trúc project Blazor + ASP.NET Core và đề xuất code phù hợp.
   - Hỗ trợ phân tích nguyên nhân lỗi nhanh (URL sai, 204 parse, silent catch, closure bug).
   - Tổng hợp nhiều lần debug lỗi thành giải pháp rõ ràng.

2. Điều chỉnh của sinh viên:
   - Kiểm tra lại nghiệp vụ: chỉ cho phép đánh giá khi CompletedAt đã có (tour hoàn thành).
   - Đồng bộ UI theo layout WanderX hiện có, không để lẫn lộn với các trang khác.
   - Sửa logic 204 NoContent để client không crash khi booking chưa có review.
   - Thêm error message rõ ràng thay vì catch im lặng.

3. Bài học:
   - AI là công cụ hỗ trợ mạnh nhưng cần kiểm tra lại từng phần theo nghiệp vụ thực tế.
   - Debug lỗi tích hợp frontend-backend đòi hỏi hiểu cả 2 layer, AI gợi ý hướng nhưng cần developer xác nhận nguyên nhân thực.
   - Hard Reload browser khi test Blazor WASM để tránh nhầm do cache cũ.
```

---

### Lần sử dụng AI số 8

| Nội dung | Thông tin |
|---|---|
| Ngày sử dụng | 24/07/2026 |
| Người thực hiện | Nguyễn Lê Huy Hùng |
| MSSV | DE180118 |
| Công cụ AI | OpenAI Codex / ChatGPT |
| Mục đích sử dụng | Review requirement, hoàn thiện bảo mật, validation và UI cho Travel Style Quiz |
| Phần việc liên quan | M5-F01 Travel Style Quiz / Backend / Frontend / Security / Testing |
| Mức độ sử dụng | Hỗ trợ một phần |

#### 4.1. Prompt đã sử dụng

```text
PROMPTS.md #Prompt-08
```

#### 4.2. Kết quả AI gợi ý

```text
AI đối chiếu mã nguồn Travel Style Quiz với requirement M5-F01 và bộ giao diện redesign, sau đó chỉ ra các vấn đề chính:

1. Endpoint quiz và CRUD cấu hình chưa kiểm tra authorization ở server.
2. API tin cậy email do client truyền lên, có nguy cơ đọc hoặc ghi kết quả của người dùng khác.
3. Frontend dùng index/DisplayOrder thay cho QuestionId thật nên có thể liên kết sai đáp án khi đổi thứ tự câu hỏi.
4. Backend bỏ qua câu hỏi hoặc option không hợp lệ thay vì trả lỗi nghiệp vụ.
5. CompletedAt do client cung cấp thay vì được tạo theo UTC ở backend.
6. Cần giữ giao diện Blazor đồng bộ với quiz1.html, quiz2.html, Quiz3.html và quizresult.html.
7. Nên tách file khi commit để giảm xung đột tại Program.cs, WanderXDbContext.cs và các component dùng chung.
```

#### 4.3. Phần sinh viên/nhóm đã sử dụng từ AI

```text
1. Áp dụng [Authorize(Roles = "Customer")] cho API lưu/xem kết quả quiz.
2. Áp dụng [Authorize(Roles = "Admin")] cho API CRUD câu hỏi và lựa chọn.
3. Lấy UserId từ JWT NameIdentifier thay vì nhận email từ request.
4. Chuyển QuizAnswerDto sang QuestionId và OptionId kiểu Guid.
5. Bổ sung validation cho thiếu câu trả lời, câu hỏi trùng, question không tồn tại và option không thuộc question.
6. Để backend tự tạo CompletedAt bằng DateTime.UtcNow.
7. Cập nhật TravelQuiz.razor và UserApiClient để sử dụng contract mới.
8. Dùng kết quả review để xác định danh sách file cần commit và các file cấu hình cá nhân không nên đưa lên repository.
```

#### 4.4. Phần sinh viên/nhóm tự chỉnh sửa hoặc cải tiến

```text
1. Kiểm tra lại requirement M5-F01 và quyết định giữ GET questions là public để Guest có thể mở và làm thử quiz.
2. Giữ giao diện quiz trong một Blazor page động thay vì tách thành nhiều HTML page độc lập.
3. Kết hợp các layout quiz1, quiz2, Quiz3 và quizresult thành ba phase Splash, Question và Result.
4. Giữ lại cấu trúc authentication/session sẵn có của WanderX thay vì tạo hệ thống đăng nhập mới.
5. Kiểm tra các thay đổi với dữ liệu câu hỏi được seed trong WanderXDbContext.
6. Tách các phần chưa đủ hợp đồng liên module như QuizAttempt/versioning và tour recommendation thật để tiếp tục thống nhất với nhóm.
7. Rà soát danh sách staged files trước khi commit nhằm tránh đưa .vscode và tài liệu tạm vào source code runtime.
```

#### 4.5. Minh chứng

| Loại minh chứng | Nội dung |
|---|---|
| Link commit | Commit message đề xuất: `feat: implement secure travel style quiz flow` |
| File liên quan | WanderXServer/Controllers/TravelStyleQuizController.cs; WanderXServer/Services/TravelStyleQuizService.cs; WanderXServer/Dtos/Users/QuizAnswerDto.cs; WanderXClient/WanderXClient/Pages/TravelQuiz.razor; WanderXClient/WanderXClient/Models/QuizContracts.cs; WanderXClient/WanderXClient/Services/UserApiClient.cs |
| Screenshot | Ảnh Git Changes với 19 file Function 1 được stage |
| Kết quả chạy/test | `dotnet build WanderXServer/WanderXServer.sln --no-restore`: thành công, 0 error; `dotnet build WanderXClient/WanderXClient.sln --no-restore`: thành công, 0 error |
| Link video demo | Chưa có |
| Ghi chú khác | Chưa xác nhận end-to-end với database và browser; QuizAttempt/versioning và recommendation API chưa nằm trong lần triển khai này |

#### 4.6. Nhận xét cá nhân/nhóm

```text
Qua lần sử dụng AI này, em nhận thấy việc sinh code chỉ là một phần nhỏ; bước quan trọng hơn là đối chiếu code với requirement và kiểm tra các ranh giới bảo mật.

AI giúp phát hiện nhanh việc dùng email từ client và DisplayOrder làm định danh, nhưng em vẫn phải kiểm tra lại kiến trúc JWT, DTO, DbContext và UI hiện có trước khi áp dụng. Em đã chủ động giữ luồng Guest xem câu hỏi, Customer lưu kết quả và Admin quản lý cấu hình theo đúng vai trò của hệ thống.

Kết quả cuối cùng được kiểm chứng bằng build server/client. Các phần chưa đủ thông tin liên module được ghi nhận rõ thay vì triển khai bằng dữ liệu giả. Bài học chính là cần review, hiểu và kiểm chứng mọi thay đổi do AI hỗ trợ trước khi commit hoặc merge vào nhánh chung.
```

---
### Lần sử dụng AI số 9

| Nội dung | Thông tin |
|---|---|
| Ngày sử dụng | 24/07/2026 |
| Người thực hiện | Nguyễn Lê Huy Hùng |
| MSSV | DE180118 |
| Công cụ AI | OpenAI Codex / ChatGPT |
| Mục đích sử dụng | Phân tích và triển khai M5-F02 gợi ý tour theo nhu cầu |
| Phần việc liên quan | Recommendation API / Rule scoring / Filter / Random / Blazor UI / Testing |
| Mức độ sử dụng | Hỗ trợ một phần |

#### 4.1. Prompt đã sử dụng

```text
PROMPTS.md #Prompt-09
```

#### 4.2. Kết quả AI gợi ý

```text
AI đề xuất ánh xạ requirement vào schema thật của WanderX, triển khai tập tour đủ điều kiện, giá hiệu lực, số chỗ còn lại, đánh giá, rule-based MatchScore/MatchReasons, filter, sort ổn định, random có exclude list và giao diện recommendations.
```

#### 4.3. Phần sinh viên/nhóm đã sử dụng và điều chỉnh

```text
Áp dụng API/service/UI đề xuất nhưng giữ nguyên schema module Tour. GuideTourAssignment được dùng làm lịch khởi hành; chỉ booking Paid/Confirmed trừ capacity; không dùng dữ liệu mẫu. Destination và style/type được xử lý theo các trường sẵn có. SecondaryStyle được hoãn vì QuizResult chưa có dữ liệu này.
```

#### 4.4. Minh chứng

| Loại minh chứng | Nội dung |
|---|---|
| File liên quan | RecommendationsController.cs; RecommendationService.cs; RecommendationOptions.cs; Recommendations.razor; TourRecommendationCard.razor; UserApiClient.cs |
| Kết quả chạy/test | `dotnet build WanderX.slnx --no-restore`: thành công, 0 warning, 0 error |
| Ghi chú | Chưa tạo migration; không thay đổi file solution hoặc cấu hình .vscode |

#### 4.5. Nhận xét cá nhân/nhóm

```text
AI hỗ trợ đối chiếu requirement và xây dựng khung thuật toán, nhưng em kiểm tra lại toàn bộ model hiện có để tránh giả định có Departure, DestinationId hoặc Tags. Kết quả được điều chỉnh theo dữ liệu thật và build trước khi commit.
```

---
### Lần sử dụng AI số 10

| Nội dung | Thông tin |
|---|---|
| Ngày sử dụng | 24/07/2026 |
| Người thực hiện | Nguyễn Lê Huy Hùng |
| MSSV | DE180118 |
| Công cụ AI | OpenAI Codex / ChatGPT |
| Mục đích sử dụng | Triển khai M5-F03 Dashboard thống kê |
| Phần việc liên quan | KPI / Revenue / Booking series / Guide ranking / Authorization / Blazor UI |
| Mức độ sử dụng | Hỗ trợ một phần |

#### 4.1. Prompt đã sử dụng

```text
PROMPTS.md #Prompt-10
```

#### 4.2. Kết quả đã sử dụng và kiểm chứng

```text
Áp dụng policy DASHBOARD_VIEW, bốn API dashboard và giao diện Admin Dashboard. Công thức được điều chỉnh theo entity thật: booking theo CreatedAt, doanh thu từ Confirmed/Finished, đã thu từ PaidAmount, top guide dựa trên assignment/review. Build toàn solution thành công, 0 error.
```

#### 4.3. Giới hạn được ghi nhận

```text
Chưa có permission table, Payment entity, audit tiến độ, khiếu nại hoặc điểm Admin. Vì vậy không tự tạo dữ liệu thay thế; Staff được policy cho phép theo role hiện tại và top guide hiển thị breakdown trên thang 85.
```

| Loại minh chứng | Nội dung |
|---|---|
| File liên quan | DashboardController.cs; DashboardService.cs; AdminDashboard.razor; UserApiClient.cs; Program.cs |
| Kết quả build | Thành công, 0 error; 2 warning cũ ngoài phạm vi |

---
### Lần sử dụng AI số 11

| Nội dung | Thông tin |
|---|---|
| Ngày sử dụng | 25/07/2026 |
| Người thực hiện | Nguyễn Lê Huy Hùng |
| MSSV | DE180118 |
| Công cụ AI | OpenAI Codex / ChatGPT |
| Mục đích sử dụng | Triển khai M5-F04 Account Management, RBAC và Phone OTP |
| Phần việc liên quan | Authorization / User management / Audit / Token revocation / OTP / UI |
| Mức độ sử dụng | Hỗ trợ một phần |

#### 4.1. Prompt đã sử dụng

```text
PROMPTS.md #Prompt-11
```

#### 4.2. Phần sử dụng và tự điều chỉnh

```text
Áp dụng policy permission trên mô hình role đơn hiện có, quản lý user có phân trang/filter, đổi role và lock/unlock có audit transaction, bảo vệ Admin cuối cùng, token version, OTP hash/cooldown/max attempts và hai UI theo layout WanderX. Không thay thế authentication stack hay tạo multi-role làm vỡ contract hiện tại.
```

#### 4.3. Giới hạn phụ thuộc ngoài

```text
Chưa có SMS provider và credential nên adapter mặc định trả SMS_PROVIDER_ERROR; không log hoặc trả OTP để tránh vi phạm bảo mật. Cần nhóm chọn provider rồi cài adapter ISmsSender. Migration được viết thủ công vì môi trường thiếu dotnet-ef.
```

| Minh chứng | Nội dung |
|---|---|
| File | AccountManagementService.cs; AccountManagementController.cs; AdminUsers.razor; VerifyPhone.razor; migration AddAccountRbacAuditAndPhoneOtp |
| Build | WanderX.slnx build thành công, 0 error |

---
## 5. Bảng tổng hợp mức độ sử dụng AI

Đánh dấu mức độ AI hỗ trợ ở từng hạng mục.

| Hạng mục | Không dùng AI | AI hỗ trợ ít | AI hỗ trợ nhiều | AI sinh chính | Ghi chú |
|---|:---:|:---:|:---:|:---:|---|
| Phân tích yêu cầu |  |  |  |  |  |
| Viết user story/use case |  |  |  |  |  |
| Thiết kế database |  |  |  |  |  |
| Thiết kế kiến trúc hệ thống |  |  |  |  |  |
| Thiết kế giao diện |  |  | X |  | Sử dụng AI cho Guide Portal calendar design (Prompt 3) |
| Code frontend |  |  | X |  | AI hỗ trợ triển khai Admin Booking, Cancellation Request và Payment UI (Prompt 6) |
| Code backend |  |  | X |  | AI hỗ trợ DTO/API/service cho booking, status, cancellation và payment (Prompt 6) |
| Debug lỗi |  |  | X |  | AI hỗ trợ phân tích CSRF, route render, concurrency, fetch và lỗi UI overflow (Prompt 6) |
| Viết test case |  |  |  |  |  |
| Kiểm thử sản phẩm |  |  |  |  |  |
| Tối ưu code |  | X |  |  | AI gợi ý, nhóm implement tối ưu thêm (Prompt 3) |
| Viết báo cáo |  |  |  |  |  |
| Làm slide thuyết trình |  |  |  |  |  |

---

## 6. Các lỗi hoặc hạn chế từ AI

Ghi lại các trường hợp AI trả lời sai, thiếu, chưa phù hợp hoặc sinh code không chạy.

| STT | Lỗi/hạn chế từ AI | Cách phát hiện | Cách xử lý/cải tiến |
|---:|---|---|---|
| 1 |  |  |  |
| 2 |  |  |  |
| 3 |  |  |  |

---

## 7. Kiểm chứng kết quả AI

Mô tả cách sinh viên/nhóm kiểm tra lại kết quả do AI gợi ý.

Có thể bao gồm:

- Chạy thử chương trình
- Viết test case
- So sánh với yêu cầu đề bài
- Kiểm tra output
- Đối chiếu tài liệu môn học
- Hỏi lại giảng viên
- Review cùng thành viên nhóm
- Kiểm tra lỗi bảo mật
- Kiểm tra bằng dữ liệu mẫu
- So sánh trước và sau khi dùng AI

### Nội dung kiểm chứng

```text
Kiểm chứng bằng cách chạy build backend/frontend, chạy trực tiếp các màn hình Admin/Staff, tạo/sửa booking, đổi trạng thái booking theo business rule, gửi/review cancellation request, ghi nhận payment full/deposit/balance và kiểm tra các trường hợp bị khóa thao tác như Finished, Cancelled hoặc Paid.
```

---

## 8. Đóng góp cá nhân hoặc đóng góp nhóm

### 8.1. Đối với bài cá nhân

Mô tả phần sinh viên tự làm, phần AI hỗ trợ và phần đã tự cải tiến.

```text
Viết tại đây...
```

### 8.2. Đối với bài nhóm

| Thành viên | MSSV | Nhiệm vụ chính | Có sử dụng AI không? | Minh chứng đóng góp |
|---|---|---|---|---|
| Trần Hồng Quân | DE180166 | FE1 Booking Management, FE2 Booking Status Management, FE3 Cancellation Request Management, FE4 Payment Management | Có | PROMPTS.md - Prompt-06; CHANGELOG.md - Phase 04.2 Member 2 Implementation |
| Võ Quang Đăng Khoa | DE180127 | Feature 1 Quản lý hồ sơ khách hàng, Feature 2 Tra cứu và theo dõi booking, Feature 3 Quản lý yêu cầu dịch vụ, Feature 4 Đánh giá và nhận xét tour | Có | PROMPTS.md - Prompt-07; CHANGELOG.md - Phase 04.3 Member 3 Implementation |
|  |  |  | Có / Không |  |

---

## 9. Reflection cuối bài

### 9.1. AI đã hỗ trợ em/nhóm ở điểm nào?

```text
Viết tại đây...
```

### 9.2. Phần nào em/nhóm không sử dụng theo gợi ý của AI? Vì sao?

```text
Viết tại đây...
```

### 9.3. Em/nhóm đã kiểm tra tính đúng đắn của kết quả AI như thế nào?

```text
Viết tại đây...
```

### 9.4. Nếu không có AI, phần nào sẽ khó khăn nhất?

```text
Viết tại đây...
```

### 9.5. Sau bài tập/project này, em/nhóm học được gì về môn học?

```text
Viết tại đây...
```

### 9.6. Sau bài tập/project này, em/nhóm học được gì về cách sử dụng AI có trách nhiệm?

```text
Viết tại đây...
```

---

## 10. Cam kết học thuật

Sinh viên/nhóm cam kết rằng:

- Nội dung AI hỗ trợ đã được ghi nhận trung thực.
- Không nộp nguyên văn kết quả AI mà không kiểm tra.
- Có khả năng giải thích các phần đã nộp.
- Chịu trách nhiệm về tính đúng đắn của sản phẩm cuối cùng.
- Hiểu rằng việc sử dụng AI không khai báo có thể ảnh hưởng đến kết quả đánh giá.

| Đại diện sinh viên/nhóm | Ngày xác nhận |
|---|---|
|  |  |

---

### Lần sử dụng AI số 12

| Nội dung | Thông tin |
|---|---|
| Ngày sử dụng | 26/07/2026 |
| Người thực hiện | Huỳnh Phúc Tấn |
| MSSV | DE180161 |
| Công cụ AI | OpenAI Codex / ChatGPT |
| Mục đích sử dụng | Hoàn thiện Member 1 - Quản lý Tour/F00/MVC |
| Phần việc liên quan | Tour CRUD / Tour Schedule / Tour Pricing / Booked Tour List / UI Debug |
| Mức độ sử dụng | Hỗ trợ một phần |

#### Prompt đã sử dụng

```text
PROMPTS.md #Prompt-12
```

#### Kết quả AI gợi ý

```text
AI đề xuất cách chia module theo service/controller/client, bổ sung trạng thái Locked cho tour, tự động khóa tour khi hết chỗ hoặc tới ngày khởi hành, hỗ trợ filter danh sách tour theo trạng thái/điểm đến/ngày khởi hành và fix lỗi UI card lịch trình bị tràn chữ.
```

#### Phần sinh viên đã sử dụng và tự điều chỉnh

```text
Sinh viên kiểm tra lại model Tour, Booking, TourSchedule, TourSeasonPrice và TourPromotion hiện có để triển khai theo schema thật của dự án. Không tạo dữ liệu giả; logic capacity dựa trên booking thực tế. UI được điều chỉnh theo layout WanderX hiện có.
```

#### Minh chứng

| Loại minh chứng | Nội dung |
|---|---|
| File backend | ToursController.cs; TourService.cs; TourSchedulesController.cs; TourScheduleService.cs; TourPricingController.cs; TourPricingService.cs; BookedToursController.cs; BookedTourService.cs |
| File frontend | AdminTours.razor; AdminTourSchedules.razor; AdminTourPricing.razor; AdminBookedTours.razor; app.css |
| Kết quả kiểm tra | Build backend/frontend thành công, test thủ công các luồng chính |
| Commit đề xuất | `feat: implement tour management workflows` |

#### Dòng đóng góp nhóm cần thêm vào bảng đóng góp

```md
| Huỳnh Phúc Tấn | DE180161 | Member 1 - Quản lý Tour: thông tin tour, lịch trình, giá tour, danh sách booked tour | Có | PROMPTS.md - Prompt-12; CHANGELOG.md - Phase 04.4 Member 1 Tour Management |
```
