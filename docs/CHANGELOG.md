# Changelog

## 1. Quy Ä‘á»‹nh ghi Changelog

File nÃ y dÃ¹ng Ä‘á»ƒ ghi láº¡i cÃ¡c thay Ä‘á»•i quan trá»ng trong quÃ¡ trÃ¬nh thá»±c hiá»‡n bÃ i táº­p, lab, assignment hoáº·c project.

NguyÃªn táº¯c ghi changelog:

- Chá»‰ ghi nhá»¯ng gÃ¬ Ä‘Ã£ hoÃ n thÃ nh tháº­t sá»±.
- KhÃ´ng ghi káº¿ hoáº¡ch náº¿u chÆ°a thá»±c hiá»‡n.
- Má»—i thay Ä‘á»•i nÃªn cÃ³ ngÃ y, ná»™i dung, ngÆ°á»i thá»±c hiá»‡n vÃ  minh chá»©ng.
- Náº¿u cÃ³ AI há»— trá»£, cáº§n ghi rÃµ AI Ä‘Ã£ há»— trá»£ pháº§n nÃ o.
- Náº¿u cÃ³ commit GitHub, cáº§n ghi link commit.
- Náº¿u cÃ³ lá»—i Ä‘Ã£ sá»­a, cáº§n ghi rÃµ lá»—i, nguyÃªn nhÃ¢n vÃ  cÃ¡ch xá»­ lÃ½.

---

## 2. ThÃ´ng tin project

| ThÃ´ng tin | Ná»™i dung |
|---|---|
| MÃ´n há»c | Building Cross-Platform Back-End Application With .NET |
| MÃ£ mÃ´n há»c | PRN232 |
| Lá»›p | SE18D05 |
| Há»c ká»³ | SU26 |
| TÃªn bÃ i táº­p / Project | Group Project - WanderX Tour Management System |
| TÃªn sinh viÃªn / NhÃ³m | Group 2 |
| MSSV / Danh sÃ¡ch MSSV | DE180158, DE180166 |
| MSSV / Danh sÃ¡ch MSSV | DE180158 |
| Giáº£ng viÃªn hÆ°á»›ng dáº«n | LÃª Thiá»‡n Nháº­t Quang |
| Repository URL | https://github.com/group-02/wanderx-tour-management |
| NgÃ y báº¯t Ä‘áº§u | 17/05/2026 |
| NgÃ y hoÃ n thÃ nh |  |

---

## 3. Tá»•ng quan cÃ¡c phiÃªn báº£n/giai Ä‘oáº¡n

| PhiÃªn báº£n/Giai Ä‘oáº¡n | Thá»i gian | Ná»™i dung chÃ­nh | Tráº¡ng thÃ¡i |
|---|---|---|---|
| Phase 01 | 17/05/2026 | Khá»Ÿi táº¡o project | Completed |
| Phase 02 | 17/05 - 20/05/2026 | PhÃ¢n tÃ­ch yÃªu cáº§u & Design | Completed |
| Phase 03 | 20/05 - ... | Thiáº¿t káº¿ há»‡ thá»‘ng | In Progress |
| Phase 04 | 25/05 - ... | Implementation - Guide Management | In Progress |
| Phase 04.2 | 20/07/2026 | Implementation - Member 2 Admin/Staff Booking Operations | Completed |
| Phase 05 |  | Testing & Debug | Not Started |
| Phase 06 |  | HoÃ n thiá»‡n bÃ¡o cÃ¡o vÃ  demo | Not Started |

---

# [Phase 04] Implementation - Guide Management & Guide Portal

## NgÃ y thá»±c hiá»‡n

```text
02/06/2026
```

## ÄÃ£ hoÃ n thÃ nh

- [X] Thiáº¿t káº¿ Guide database model
- [X] Táº¡o Guide entity model class
- [X] Táº¡o GuideLanguage relationship table
- [X] Táº¡o GuideServiceArea relationship table
- [X] Implement IGuideRepository interface
- [X] Implement GuideRepository class (data access layer)
- [X] Implement GuideService class (business logic layer)
- [X] Táº¡o GuideController vá»›i endpoints
- [X] Implement authorization checks (Guide chá»‰ update own profile)
- [X] ThÃªm validation logic cho inputs
- [X] Implement Guide Portal endpoints
- [X] Test API endpoints vá»›i Postman/Swagger

## Thay Ä‘á»•i chi tiáº¿t

| STT | Ná»™i dung thay Ä‘á»•i | NgÆ°á»i thá»±c hiá»‡n | File/Module liÃªn quan | Minh chá»©ng |
|---:|---|---|---|---|
| 1 | Táº¡o Guide domain model | Group 2 | WanderXServer/Models/Guide.cs | Commit: feat/guide-model |
| 2 | Táº¡o GuideLanguage & GuideServiceArea entities | Group 2 | WanderXServer/Models/GuideLanguage.cs, GuideServiceArea.cs | Commit: feat/guide-relationships |
| 3 | Implement GuideRepository | Group 2 | WanderXServer/Repositories/GuideRepository.cs | Commit: feat/guide-repository |
| 4 | Implement GuideService | Group 2 | WanderXServer/Services/GuideService.cs | Commit: feat/guide-service |
| 5 | Implement GuideController | Group 2 | WanderXServer/Controllers/GuideController.cs | Commit: feat/guide-controller |
| 6 | Implement Guide Portal endpoints | Group 2 | WanderXServer/Controllers/GuidePortalController.cs | Commit: feat/guide-portal |
| 7 | Add authorization & validation | Group 2 | WanderXServer/Controllers/GuideController.cs | Commit: feat/guide-auth-validation |
| 8 | Implement filtering, search, pagination | Group 2 | WanderXServer/Services/GuideService.cs | Commit: feat/guide-advanced-queries |

## AI cÃ³ há»— trá»£ khÃ´ng?

- [X] CÃ³
- [ ] KhÃ´ng

Náº¿u cÃ³, mÃ´ táº£ AI Ä‘Ã£ há»— trá»£ pháº§n nÃ o:

```text
GitHub Copilot & ChatGPT há»— trá»£:
1. Database schema design - suggest proper normalization with separate tables for Languages & Areas
2. API endpoint design - RESTful convention suggestions
3. DTO models structure - request/response models organization
4. Authorization strategy - implement role-based access control
5. Code generation - scaffold repository, service, controller classes
6. Validation logic - input validation patterns vÃ  error handling
7. Best practices - async/await patterns, error handling, logging

Chi tiáº¿t trong PROMPTS.md - Prompt sá»‘ 2
```

## Commit/Screenshot minh chá»©ng

```text
Commits:
- feat/guide-model
- feat/guide-relationships
- feat/guide-repository
- feat/guide-service
- feat/guide-controller
- feat/guide-portal
- feat/guide-auth-validation
- feat/guide-advanced-queries

Screenshots:
- Guide API Swagger documentation
- Postman test results for all endpoints
- Database schema diagram

Repository: https://github.com/group-02/wanderx-tour-management
```

## Ghi chÃº

```text
### Key Features Implemented:

1. Guide Management Module:
   - CRUD operations cho guide information
   - Manage guide languages with proficiency levels
   - Manage guide service areas
   - Track guide experience (sá»‘ tour Ä‘Ã£ hÆ°á»›ng dáº«n)
   - Guide status management (Active, Inactive, OnLeave)

2. Authorization & Security:
   - Only Admins/Staff can create/delete guides
   - Guides can only update their own profiles
   - Implement [Authorize] attributes
   - User identity validation

3. Guide Portal Features:
   - Guides xem assigned tours
   - Guides reject tour assignments
   - Guides update tour status (Confirmed -> Finished)
   - View tour details & customer lists

4. Query Optimization:
   - Pagination implemented
   - Filtering by status, language, area
   - Search by name, email
   - Sorting by rating, experience

### Architecture Decisions:

1. Database Normalization:
   - Separate GuideLanguage table thay vÃ¬ JSON array
   - Separate GuideServiceArea table thay vÃ¬ JSON array
   - LÃ½ do: Better normalization, easier querying, scalable

2. Design Patterns:
   - Repository Pattern: Abstraction data access
   - Service Layer Pattern: Business logic isolation
   - DTO Pattern: API data transfer
   - Dependency Injection: Loose coupling

3. Error Handling:
   - Custom exceptions for business logic errors
   - Validation at DTO level
   - Proper HTTP status codes

### Next Steps:
- Implement booking & tour assignment logic
- Implement notification system (SignalR)
- Add unit tests & integration tests
- Performance optimization & caching
```

---

# [Phase 02] PhÃ¢n tÃ­ch yÃªu cáº§u

## NgÃ y thá»±c hiá»‡n

```text
DD/MM/YYYY
```

## ÄÃ£ hoÃ n thÃ nh

- [ ] XÃ¡c Ä‘á»‹nh problem statement
- [ ] XÃ¡c Ä‘á»‹nh user roles
- [ ] Viáº¿t user stories
- [ ] Viáº¿t use cases
- [ ] XÃ¡c Ä‘á»‹nh functional requirements
- [ ] XÃ¡c Ä‘á»‹nh non-functional requirements
- [ ] XÃ¡c Ä‘á»‹nh business rules
- [ ] XÃ¡c Ä‘á»‹nh acceptance criteria
- [ ] Review yÃªu cáº§u vá»›i giáº£ng viÃªn/nhÃ³m
- [ ] Chá»‰nh sá»­a yÃªu cáº§u sau feedback

## Thay Ä‘á»•i chi tiáº¿t

| STT | Ná»™i dung thay Ä‘á»•i | NgÆ°á»i thá»±c hiá»‡n | File/Module liÃªn quan | Minh chá»©ng |
|---:|---|---|---|---|
| 1 |  |  |  |  |
| 2 |  |  |  |  |
| 3 |  |  |  |  |

## AI cÃ³ há»— trá»£ khÃ´ng?

- [ ] CÃ³
- [ ] KhÃ´ng

Náº¿u cÃ³, mÃ´ táº£ AI Ä‘Ã£ há»— trá»£ pháº§n nÃ o:

```text
Viáº¿t táº¡i Ä‘Ã¢y...
```

## Commit/Screenshot minh chá»©ng

```text
DÃ¡n link commit, screenshot hoáº·c mÃ´ táº£ minh chá»©ng táº¡i Ä‘Ã¢y...
```

## Ghi chÃº

```text
Viáº¿t táº¡i Ä‘Ã¢y...
```

---

# [Phase 03] Thiáº¿t káº¿ há»‡ thá»‘ng

## NgÃ y thá»±c hiá»‡n

```text
DD/MM/YYYY
```

## ÄÃ£ hoÃ n thÃ nh

- [ ] Thiáº¿t káº¿ kiáº¿n trÃºc tá»•ng quan
- [ ] Thiáº¿t káº¿ database/ERD
- [ ] Thiáº¿t káº¿ API
- [ ] Thiáº¿t káº¿ giao diá»‡n/wireframe
- [ ] Thiáº¿t káº¿ flow xá»­ lÃ½
- [ ] Thiáº¿t káº¿ class diagram
- [ ] Thiáº¿t káº¿ sequence diagram
- [ ] Thiáº¿t káº¿ security/authorization flow
- [ ] Review thiáº¿t káº¿
- [ ] Chá»‰nh sá»­a thiáº¿t káº¿ sau feedback

## Thay Ä‘á»•i chi tiáº¿t

| STT | Ná»™i dung thay Ä‘á»•i | NgÆ°á»i thá»±c hiá»‡n | File/Module liÃªn quan | Minh chá»©ng |
|---:|---|---|---|---|
| 1 |  |  |  |  |
| 2 |  |  |  |  |
| 3 |  |  |  |  |

## AI cÃ³ há»— trá»£ khÃ´ng?

- [ ] CÃ³
- [ ] KhÃ´ng

Náº¿u cÃ³, mÃ´ táº£ AI Ä‘Ã£ há»— trá»£ pháº§n nÃ o:

```text
Viáº¿t táº¡i Ä‘Ã¢y...
```

## Commit/Screenshot minh chá»©ng

```text
DÃ¡n link commit, screenshot hoáº·c mÃ´ táº£ minh chá»©ng táº¡i Ä‘Ã¢y...
```

## Ghi chÃº

```text
Viáº¿t táº¡i Ä‘Ã¢y...
```

---

# [Phase 04.2] Implementation - Member 2 Admin/Staff Booking Operations

## NgÃ y thá»±c hiá»‡n

```text
20/07/2026
```

## ÄÃ£ hoÃ n thÃ nh

- [X] XÃ¢y dá»±ng backend API cho Booking Management
- [X] XÃ¢y dá»±ng backend API cho Booking Status Management
- [X] XÃ¢y dá»±ng backend API cho Cancellation Request Management
- [X] XÃ¢y dá»±ng backend API cho Payment Management
- [X] Bá»• sung DTO request/response cáº§n thiáº¿t
- [X] Táº­n dá»¥ng database hiá»‡n cÃ³ vÃ  bá»• sung cá»™t vÃ o báº£ng Bookings
- [X] XÃ¢y dá»±ng frontend Blazor cho Admin Bookings
- [X] XÃ¢y dá»±ng frontend Blazor cho Admin Cancellation Requests
- [X] XÃ¢y dá»±ng frontend Blazor cho Admin Payments
- [X] Xá»­ lÃ½ validation vÃ  business rules
- [X] Xá»­ lÃ½ lá»—i tÃ­ch há»£p frontend/backend
- [X] Tá»‘i Æ°u giao diá»‡n, popup/modal vÃ  phÃ¢n trang 5 dÃ²ng/trang

## Thay Ä‘á»•i chi tiáº¿t

| STT | Ná»™i dung thay Ä‘á»•i | NgÆ°á»i thá»±c hiá»‡n | File/Module liÃªn quan | Minh chá»©ng |
|---:|---|---|---|---|
| 1 | Implement FE1 Booking Management: danh sÃ¡ch booking, táº¡o/sá»­a booking, guest list, ticket type Adult/Child, search/filter vÃ  phÃ¢n trang | Tráº§n Há»“ng QuÃ¢n - DE180166 | WanderXClient/WanderXClient/Pages/AdminBookings.razor; WanderXServer/Controllers/BookingsController.cs; WanderXServer/Services/BookingService.cs | Test trá»±c tiáº¿p mÃ n hÃ¬nh Admin Bookings |
| 2 | Implement FE2 Booking Status Management vá»›i business rule khÃ³a Finished/Cancelled vÃ  giá»›i háº¡n dropdown tráº¡ng thÃ¡i há»£p lá»‡ | Tráº§n Há»“ng QuÃ¢n - DE180166 | WanderXClient/WanderXClient/Pages/AdminBookings.razor; WanderXServer/Dtos/Bookings/UpdateBookingStatusRequest.cs; WanderXServer/Services/BookingService.cs | Test Ä‘á»•i status Pending/Confirmed/Finished/Cancelled |
| 3 | Implement FE3 Cancellation Request Management: user gá»­i request, admin approve/reject báº±ng popup, Ä‘á»“ng bá»™ giao diá»‡n vÃ  phÃ¢n trang | Tráº§n Há»“ng QuÃ¢n - DE180166 | WanderXClient/WanderXClient/Pages/AdminCancellationRequests.razor; WanderXClient/WanderXClient/Pages/BookingDetail.razor; WanderXServer/Controllers/UsersController.cs; WanderXServer/Controllers/BookingsController.cs | Test táº¡o vÃ  review cancellation request |
| 4 | Implement FE4 Payment Management: quáº£n lÃ½ unpaid/deposit paid/paid/failed, full/deposit/balance payment, reference, invoice/receipt vÃ  khÃ³a booking Ä‘Ã£ paid | Tráº§n Há»“ng QuÃ¢n - DE180166 | WanderXClient/WanderXClient/Pages/AdminPayments.razor; WanderXServer/Dtos/Bookings/UpdatePaymentRequest.cs; WanderXServer/Services/BookingService.cs | Test record payment vÃ  view invoice |
| 5 | Bá»• sung cÃ¡c cá»™t booking/status/cancellation/payment vÃ o báº£ng Bookings báº±ng logic Ä‘áº£m báº£o schema khi cháº¡y app | Tráº§n Há»“ng QuÃ¢n - DE180166 | WanderXServer/BusinessObject/Booking.cs; WanderXServer/DataAccessLayer/WanderXDbContext.cs | Kiá»ƒm tra app cháº¡y vá»›i database hiá»‡n cÃ³ |
| 6 | Chuáº©n bá»‹ gá»­i email tá»± Ä‘á»™ng cho cancellation request received/approved/rejected vÃ  direct cancellation | Tráº§n Há»“ng QuÃ¢n - DE180166 | WanderXServer/Services/SmtpEmailSender.cs; WanderXServer/Services/UserService.cs; WanderXServer/Services/BookingService.cs | Email hook Ä‘Ã£ sáºµn sÃ ng, chá»‰ cáº§n cáº¥u hÃ¬nh SMTP tháº­t |
| 7 | Fix lá»—i CSRF token, route Ä‘á»•i link nhÆ°ng khÃ´ng render form, concurrency khi Ä‘á»•i ngÃ y tour, Failed to fetch vÃ  lá»—i sá»‘ tiá»n bá»‹ trÃ n UI | Tráº§n Há»“ng QuÃ¢n - DE180166 | WanderXClient/WanderXClient/Services/UserApiClient.cs; WanderXClient/WanderXClient/Pages/AdminBookings.razor; WanderXClient/WanderXClient/Pages/AdminPayments.razor; WanderXServer/Services/BookingService.cs | Build vÃ  test thá»§ cÃ´ng sau khi sá»­a |

## AI cÃ³ há»— trá»£ khÃ´ng?

- [X] CÃ³
- [ ] KhÃ´ng

Náº¿u cÃ³, mÃ´ táº£ AI Ä‘Ã£ há»— trá»£ pháº§n nÃ o:

```text
ChatGPT/Codex há»— trá»£:
1. PhÃ¢n tÃ­ch nghiá»‡p vá»¥ Member 2 theo tá»«ng feature FE1-FE4.
2. Äá» xuáº¥t DTO/API/service cáº§n bá»• sung dá»±a trÃªn cáº¥u trÃºc project cÃ³ sáºµn.
3. Há»— trá»£ viáº¿t vÃ  chá»‰nh Blazor UI cho cÃ¡c mÃ n hÃ¬nh Admin/Staff.
4. Há»— trá»£ debug lá»—i runtime vÃ  build phÃ¡t sinh khi tÃ­ch há»£p frontend/backend.
5. Há»— trá»£ rÃ  soÃ¡t Ä‘á»ƒ táº­n dá»¥ng database hiá»‡n cÃ³, khÃ´ng táº¡o báº£ng má»›i ngoÃ i pháº¡m vi yÃªu cáº§u.

Chi tiáº¿t trong PROMPTS.md - Prompt-06 vÃ  AI_AUDIT_LOG.md - Láº§n sá»­ dá»¥ng AI sá»‘ 6.
```

## Commit/Screenshot minh chá»©ng

```text
Files implemented/tested:
- WanderXClient/WanderXClient/Pages/AdminBookings.razor
- WanderXClient/WanderXClient/Pages/AdminCancellationRequests.razor
- WanderXClient/WanderXClient/Pages/AdminPayments.razor
- WanderXServer/Controllers/BookingsController.cs
- WanderXServer/Controllers/UsersController.cs
- WanderXServer/Services/BookingService.cs
- WanderXServer/Services/UserService.cs
- WanderXServer/DataAccessLayer/WanderXDbContext.cs

Káº¿t quáº£ kiá»ƒm tra:
- Build backend/frontend thÃ nh cÃ´ng sau khi fix lá»—i.
- Test thá»§ cÃ´ng cÃ¡c luá»“ng booking, status, cancellation request vÃ  payment.
```

## Ghi chÃº

```text
CÃ¡c chá»©c nÄƒng FE1-FE4 phá»¥c vá»¥ phÃ­a Admin/Staff. Database khÃ´ng táº¡o thÃªm báº£ng má»›i cho cancellation/payment; há»‡ thá»‘ng bá»• sung cÃ¡c cá»™t cáº§n thiáº¿t vÃ o báº£ng Bookings Ä‘á»ƒ lÆ°u tráº¡ng thÃ¡i, yÃªu cáº§u há»§y vÃ  thÃ´ng tin thanh toÃ¡n. Email Ä‘Ã£ Ä‘Æ°á»£c chuáº©n bá»‹ theo dáº¡ng SMTP configuration, khi Ä‘iá»n thÃ´ng tin mail tháº­t trong appsettings thÃ¬ cÃ¡c luá»“ng tá»± Ä‘á»™ng cÃ³ thá»ƒ gá»­i email.
```

---

# [Phase 05] Testing & Debug

## NgÃ y thá»±c hiá»‡n

```text
DD/MM/YYYY
```

## ÄÃ£ hoÃ n thÃ nh

- [ ] Viáº¿t test case
- [ ] Cháº¡y test chá»©c nÄƒng chÃ­nh
- [ ] Kiá»ƒm tra output
- [ ] Kiá»ƒm tra validation
- [ ] Kiá»ƒm tra lá»—i giao diá»‡n
- [ ] Kiá»ƒm tra lá»—i database
- [ ] Kiá»ƒm tra phÃ¢n quyá»n
- [ ] Kiá»ƒm tra báº£o máº­t cÆ¡ báº£n
- [ ] Fix bug
- [ ] Cháº¡y láº¡i sau khi fix bug
- [ ] Ghi nháº­n káº¿t quáº£ test

## Danh sÃ¡ch lá»—i Ä‘Ã£ xá»­ lÃ½

| STT | Lá»—i phÃ¡t hiá»‡n | NguyÃªn nhÃ¢n | CÃ¡ch xá»­ lÃ½ | Tráº¡ng thÃ¡i |
|---:|---|---|---|---|
| 1 |  |  |  | Open / Fixed / Pending |
| 2 |  |  |  | Open / Fixed / Pending |
| 3 |  |  |  | Open / Fixed / Pending |
| 4 |  |  |  | Open / Fixed / Pending |
| 5 |  |  |  | Open / Fixed / Pending |

## Thay Ä‘á»•i chi tiáº¿t

| STT | Ná»™i dung thay Ä‘á»•i | NgÆ°á»i thá»±c hiá»‡n | File/Module liÃªn quan | Minh chá»©ng |
|---:|---|---|---|---|
| 1 |  |  |  |  |
| 2 |  |  |  |  |
| 3 |  |  |  |  |

## AI cÃ³ há»— trá»£ khÃ´ng?

- [ ] CÃ³
- [ ] KhÃ´ng

Náº¿u cÃ³, mÃ´ táº£ AI Ä‘Ã£ há»— trá»£ pháº§n nÃ o:

```text
Viáº¿t táº¡i Ä‘Ã¢y...
```

## Commit/Screenshot minh chá»©ng

```text
DÃ¡n link commit, screenshot hoáº·c mÃ´ táº£ minh chá»©ng táº¡i Ä‘Ã¢y...
```

## Ghi chÃº

```text
Viáº¿t táº¡i Ä‘Ã¢y...
```

---

# [Phase 06] HoÃ n thiá»‡n bÃ¡o cÃ¡o vÃ  demo

## NgÃ y thá»±c hiá»‡n

```text
DD/MM/YYYY
```

## ÄÃ£ hoÃ n thÃ nh

- [ ] HoÃ n thiá»‡n source code
- [ ] HoÃ n thiá»‡n README.md
- [ ] HoÃ n thiá»‡n report
- [ ] HoÃ n thiá»‡n slide
- [ ] HoÃ n thiá»‡n video demo
- [ ] Kiá»ƒm tra láº¡i `AI_AUDIT_LOG.md`
- [ ] Kiá»ƒm tra láº¡i `PROMPTS.md`
- [ ] HoÃ n thiá»‡n `REFLECTION.md`
- [ ] Kiá»ƒm tra láº¡i `CHANGELOG.md`
- [ ] ÄÃ³ng gÃ³i bÃ i ná»™p

## Thay Ä‘á»•i chi tiáº¿t

| STT | Ná»™i dung thay Ä‘á»•i | NgÆ°á»i thá»±c hiá»‡n | File/Module liÃªn quan | Minh chá»©ng |
|---:|---|---|---|---|
| 1 |  |  |  |  |
| 2 |  |  |  |  |
| 3 |  |  |  |  |

## AI cÃ³ há»— trá»£ khÃ´ng?

- [ ] CÃ³
- [ ] KhÃ´ng

Náº¿u cÃ³, mÃ´ táº£ AI Ä‘Ã£ há»— trá»£ pháº§n nÃ o:

```text
Viáº¿t táº¡i Ä‘Ã¢y...
```

## Commit/Screenshot minh chá»©ng

```text
DÃ¡n link commit, screenshot hoáº·c mÃ´ táº£ minh chá»©ng táº¡i Ä‘Ã¢y...
```

## Ghi chÃº

```text
Viáº¿t táº¡i Ä‘Ã¢y...
```

---

# 4. Tá»•ng káº¿t thay Ä‘á»•i cuá»‘i project

## 4.1. CÃ¡c chá»©c nÄƒng Ä‘Ã£ hoÃ n thÃ nh

| STT | Chá»©c nÄƒng | Tráº¡ng thÃ¡i | Minh chá»©ng | Ghi chÃº |
|---:|---|---|---|---|
| 1 |  | Completed / Partial / Not Completed |  |  |
| 2 |  | Completed / Partial / Not Completed |  |  |
| 3 |  | Completed / Partial / Not Completed |  |  |
| 4 |  | Completed / Partial / Not Completed |  |  |
| 5 |  | Completed / Partial / Not Completed |  |  |

---

## 4.2. CÃ¡c chá»©c nÄƒng chÆ°a hoÃ n thÃ nh

| STT | Chá»©c nÄƒng | LÃ½ do chÆ°a hoÃ n thÃ nh | HÆ°á»›ng cáº£i thiá»‡n |
|---:|---|---|---|
| 1 |  |  |  |
| 2 |  |  |  |
| 3 |  |  |  |

---

## 4.3. Tá»•ng há»£p AI há»— trá»£ trong project

| Háº¡ng má»¥c | AI cÃ³ há»— trá»£ khÃ´ng? | Má»©c Ä‘á»™ há»— trá»£ | Ghi chÃº |
|---|---|---|---|
| Requirement | CÃ³ / KhÃ´ng | Ãt / Trung bÃ¬nh / Nhiá»u |  |
| Design | CÃ³ / KhÃ´ng | Ãt / Trung bÃ¬nh / Nhiá»u |  |
| Database | CÃ³ / KhÃ´ng | Ãt / Trung bÃ¬nh / Nhiá»u |  |
| Coding | CÃ³ / KhÃ´ng | Ãt / Trung bÃ¬nh / Nhiá»u |  |
| Debug | CÃ³ / KhÃ´ng | Ãt / Trung bÃ¬nh / Nhiá»u |  |
| Testing | CÃ³ / KhÃ´ng | Ãt / Trung bÃ¬nh / Nhiá»u |  |
| Report | CÃ³ / KhÃ´ng | Ãt / Trung bÃ¬nh / Nhiá»u |  |
| Presentation | CÃ³ / KhÃ´ng | Ãt / Trung bÃ¬nh / Nhiá»u |  |

---

## 4.4. BÃ i há»c rÃºt ra

```text
Viáº¿t táº¡i Ä‘Ã¢y...
```

---

## 4.5. HÆ°á»›ng cáº£i thiá»‡n tiáº¿p theo

```text
Viáº¿t táº¡i Ä‘Ã¢y...
```

---

# 5. Cam káº¿t cáº­p nháº­t Changelog

Sinh viÃªn/nhÃ³m cam káº¿t ráº±ng ná»™i dung changelog pháº£n Ã¡nh Ä‘Ãºng cÃ¡c thay Ä‘á»•i Ä‘Ã£ thá»±c hiá»‡n trong quÃ¡ trÃ¬nh lÃ m bÃ i táº­p/project.

| Äáº¡i diá»‡n sinh viÃªn/nhÃ³m | NgÃ y xÃ¡c nháº­n |
|---|---|
|  |  |
