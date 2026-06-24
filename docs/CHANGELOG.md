# Changelog

## 1. Quy định ghi Changelog

File này dùng để ghi lại các thay đổi quan trọng trong quá trình thực hiện bài tập, lab, assignment hoặc project.

Nguyên tắc ghi changelog:

- Chỉ ghi những gì đã hoàn thành thật sự.
- Không ghi kế hoạch nếu chưa thực hiện.
- Mỗi thay đổi nên có ngày, nội dung, người thực hiện và minh chứng.
- Nếu có AI hỗ trợ, cần ghi rõ AI đã hỗ trợ phần nào.
- Nếu có commit GitHub, cần ghi link commit.
- Nếu có lỗi đã sửa, cần ghi rõ lỗi, nguyên nhân và cách xử lý.

---

## 2. Thông tin project

| Thông tin | Nội dung |
|---|---|
| Môn học | Building Cross-Platform Back-End Application With .NET |
| Mã môn học | PRN232 |
| Lớp | SE18D05 |
| Học kỳ | SU26 |
| Tên bài tập / Project | Group Project - WanderX Tour Management System |
| Tên sinh viên / Nhóm | Group 2 |
| MSSV / Danh sách MSSV | DE180158 |
| Giảng viên hướng dẫn | Lê Thiện Nhật Quang |
| Repository URL | https://github.com/group-02/wanderx-tour-management |
| Ngày bắt đầu | 17/05/2026 |
| Ngày hoàn thành |  |

---

## 3. Tổng quan các phiên bản/giai đoạn

| Phiên bản/Giai đoạn | Thời gian | Nội dung chính | Trạng thái |
|---|---|---|---|
| Phase 01 | 17/05/2026 | Khởi tạo project | Completed |
| Phase 02 | 17/05 - 20/05/2026 | Phân tích yêu cầu & Design | Completed |
| Phase 03 | 20/05 - ... | Thiết kế hệ thống | In Progress |
| Phase 04 | 25/05 - ... | Implementation - Guide Management | In Progress |
| Phase 05 |  | Testing & Debug | Not Started |
| Phase 06 |  | Hoàn thiện báo cáo và demo | Not Started |

---

# [Phase 04] Implementation - Guide Management & Guide Portal

## Ngày thực hiện

```text
02/06/2026
```

## Đã hoàn thành

- [X] Thiết kế Guide database model
- [X] Tạo Guide entity model class
- [X] Tạo GuideLanguage relationship table
- [X] Tạo GuideServiceArea relationship table
- [X] Implement IGuideRepository interface
- [X] Implement GuideRepository class (data access layer)
- [X] Implement GuideService class (business logic layer)
- [X] Tạo GuideController với endpoints
- [X] Implement authorization checks (Guide chỉ update own profile)
- [X] Thêm validation logic cho inputs
- [X] Implement Guide Portal endpoints
- [X] Test API endpoints với Postman/Swagger

## Thay đổi chi tiết

| STT | Nội dung thay đổi | Người thực hiện | File/Module liên quan | Minh chứng |
|---:|---|---|---|---|
| 1 | Tạo Guide domain model | Group 2 | WanderXServer/Models/Guide.cs | Commit: feat/guide-model |
| 2 | Tạo GuideLanguage & GuideServiceArea entities | Group 2 | WanderXServer/Models/GuideLanguage.cs, GuideServiceArea.cs | Commit: feat/guide-relationships |
| 3 | Implement GuideRepository | Group 2 | WanderXServer/Repositories/GuideRepository.cs | Commit: feat/guide-repository |
| 4 | Implement GuideService | Group 2 | WanderXServer/Services/GuideService.cs | Commit: feat/guide-service |
| 5 | Implement GuideController | Group 2 | WanderXServer/Controllers/GuideController.cs | Commit: feat/guide-controller |
| 6 | Implement Guide Portal endpoints | Group 2 | WanderXServer/Controllers/GuidePortalController.cs | Commit: feat/guide-portal |
| 7 | Add authorization & validation | Group 2 | WanderXServer/Controllers/GuideController.cs | Commit: feat/guide-auth-validation |
| 8 | Implement filtering, search, pagination | Group 2 | WanderXServer/Services/GuideService.cs | Commit: feat/guide-advanced-queries |

## AI có hỗ trợ không?

- [X] Có
- [ ] Không

Nếu có, mô tả AI đã hỗ trợ phần nào:

```text
GitHub Copilot & ChatGPT hỗ trợ:
1. Database schema design - suggest proper normalization with separate tables for Languages & Areas
2. API endpoint design - RESTful convention suggestions
3. DTO models structure - request/response models organization
4. Authorization strategy - implement role-based access control
5. Code generation - scaffold repository, service, controller classes
6. Validation logic - input validation patterns và error handling
7. Best practices - async/await patterns, error handling, logging

Chi tiết trong PROMPTS.md - Prompt số 2
```

## Commit/Screenshot minh chứng

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

## Ghi chú

```text
### Key Features Implemented:

1. Guide Management Module:
   - CRUD operations cho guide information
   - Manage guide languages with proficiency levels
   - Manage guide service areas
   - Track guide experience (số tour đã hướng dẫn)
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
   - Separate GuideLanguage table thay vì JSON array
   - Separate GuideServiceArea table thay vì JSON array
   - Lý do: Better normalization, easier querying, scalable

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

# [Phase 02] Phân tích yêu cầu

## Ngày thực hiện

```text
DD/MM/YYYY
```

## Đã hoàn thành

- [ ] Xác định problem statement
- [ ] Xác định user roles
- [ ] Viết user stories
- [ ] Viết use cases
- [ ] Xác định functional requirements
- [ ] Xác định non-functional requirements
- [ ] Xác định business rules
- [ ] Xác định acceptance criteria
- [ ] Review yêu cầu với giảng viên/nhóm
- [ ] Chỉnh sửa yêu cầu sau feedback

## Thay đổi chi tiết

| STT | Nội dung thay đổi | Người thực hiện | File/Module liên quan | Minh chứng |
|---:|---|---|---|---|
| 1 |  |  |  |  |
| 2 |  |  |  |  |
| 3 |  |  |  |  |

## AI có hỗ trợ không?

- [ ] Có
- [ ] Không

Nếu có, mô tả AI đã hỗ trợ phần nào:

```text
Viết tại đây...
```

## Commit/Screenshot minh chứng

```text
Dán link commit, screenshot hoặc mô tả minh chứng tại đây...
```

## Ghi chú

```text
Viết tại đây...
```

---

# [Phase 03] Thiết kế hệ thống

## Ngày thực hiện

```text
DD/MM/YYYY
```

## Đã hoàn thành

- [ ] Thiết kế kiến trúc tổng quan
- [ ] Thiết kế database/ERD
- [ ] Thiết kế API
- [ ] Thiết kế giao diện/wireframe
- [ ] Thiết kế flow xử lý
- [ ] Thiết kế class diagram
- [ ] Thiết kế sequence diagram
- [ ] Thiết kế security/authorization flow
- [ ] Review thiết kế
- [ ] Chỉnh sửa thiết kế sau feedback

## Thay đổi chi tiết

| STT | Nội dung thay đổi | Người thực hiện | File/Module liên quan | Minh chứng |
|---:|---|---|---|---|
| 1 |  |  |  |  |
| 2 |  |  |  |  |
| 3 |  |  |  |  |

## AI có hỗ trợ không?

- [ ] Có
- [ ] Không

Nếu có, mô tả AI đã hỗ trợ phần nào:

```text
Viết tại đây...
```

## Commit/Screenshot minh chứng

```text
Dán link commit, screenshot hoặc mô tả minh chứng tại đây...
```

## Ghi chú

```text
Viết tại đây...
```

---

# [Phase 04] Implementation

## Ngày thực hiện

```text
DD/MM/YYYY
```

## Đã hoàn thành

- [ ] Tạo project structure
- [ ] Cài đặt database connection
- [ ] Xây dựng backend
- [ ] Xây dựng frontend
- [ ] Xây dựng authentication/authorization
- [ ] Xử lý CRUD
- [ ] Xử lý validation
- [ ] Tích hợp API
- [ ] Xử lý upload/download file
- [ ] Xử lý lỗi
- [ ] Tối ưu giao diện
- [ ] Cập nhật README hướng dẫn chạy

## Thay đổi chi tiết

| STT | Nội dung thay đổi | Người thực hiện | File/Module liên quan | Minh chứng |
|---:|---|---|---|---|
| 1 |  |  |  |  |
| 2 |  |  |  |  |
| 3 |  |  |  |  |
| 4 |  |  |  |  |
| 5 |  |  |  |  |

## AI có hỗ trợ không?

- [ ] Có
- [ ] Không

Nếu có, mô tả AI đã hỗ trợ phần nào:

```text
Viết tại đây...
```

## Commit/Screenshot minh chứng

```text
Dán link commit, screenshot hoặc mô tả minh chứng tại đây...
```

## Ghi chú

```text
Viết tại đây...
```

---

# [Phase 05] Testing & Debug

## Ngày thực hiện

```text
DD/MM/YYYY
```

## Đã hoàn thành

- [ ] Viết test case
- [ ] Chạy test chức năng chính
- [ ] Kiểm tra output
- [ ] Kiểm tra validation
- [ ] Kiểm tra lỗi giao diện
- [ ] Kiểm tra lỗi database
- [ ] Kiểm tra phân quyền
- [ ] Kiểm tra bảo mật cơ bản
- [ ] Fix bug
- [ ] Chạy lại sau khi fix bug
- [ ] Ghi nhận kết quả test

## Danh sách lỗi đã xử lý

| STT | Lỗi phát hiện | Nguyên nhân | Cách xử lý | Trạng thái |
|---:|---|---|---|---|
| 1 |  |  |  | Open / Fixed / Pending |
| 2 |  |  |  | Open / Fixed / Pending |
| 3 |  |  |  | Open / Fixed / Pending |
| 4 |  |  |  | Open / Fixed / Pending |
| 5 |  |  |  | Open / Fixed / Pending |

## Thay đổi chi tiết

| STT | Nội dung thay đổi | Người thực hiện | File/Module liên quan | Minh chứng |
|---:|---|---|---|---|
| 1 |  |  |  |  |
| 2 |  |  |  |  |
| 3 |  |  |  |  |

## AI có hỗ trợ không?

- [ ] Có
- [ ] Không

Nếu có, mô tả AI đã hỗ trợ phần nào:

```text
Viết tại đây...
```

## Commit/Screenshot minh chứng

```text
Dán link commit, screenshot hoặc mô tả minh chứng tại đây...
```

## Ghi chú

```text
Viết tại đây...
```

---

# [Phase 06] Hoàn thiện báo cáo và demo

## Ngày thực hiện

```text
DD/MM/YYYY
```

## Đã hoàn thành

- [ ] Hoàn thiện source code
- [ ] Hoàn thiện README.md
- [ ] Hoàn thiện report
- [ ] Hoàn thiện slide
- [ ] Hoàn thiện video demo
- [ ] Kiểm tra lại `AI_AUDIT_LOG.md`
- [ ] Kiểm tra lại `PROMPTS.md`
- [ ] Hoàn thiện `REFLECTION.md`
- [ ] Kiểm tra lại `CHANGELOG.md`
- [ ] Đóng gói bài nộp

## Thay đổi chi tiết

| STT | Nội dung thay đổi | Người thực hiện | File/Module liên quan | Minh chứng |
|---:|---|---|---|---|
| 1 |  |  |  |  |
| 2 |  |  |  |  |
| 3 |  |  |  |  |

## AI có hỗ trợ không?

- [ ] Có
- [ ] Không

Nếu có, mô tả AI đã hỗ trợ phần nào:

```text
Viết tại đây...
```

## Commit/Screenshot minh chứng

```text
Dán link commit, screenshot hoặc mô tả minh chứng tại đây...
```

## Ghi chú

```text
Viết tại đây...
```

---

# 4. Tổng kết thay đổi cuối project

## 4.1. Các chức năng đã hoàn thành

| STT | Chức năng | Trạng thái | Minh chứng | Ghi chú |
|---:|---|---|---|---|
| 1 |  | Completed / Partial / Not Completed |  |  |
| 2 |  | Completed / Partial / Not Completed |  |  |
| 3 |  | Completed / Partial / Not Completed |  |  |
| 4 |  | Completed / Partial / Not Completed |  |  |
| 5 |  | Completed / Partial / Not Completed |  |  |

---

## 4.2. Các chức năng chưa hoàn thành

| STT | Chức năng | Lý do chưa hoàn thành | Hướng cải thiện |
|---:|---|---|---|
| 1 |  |  |  |
| 2 |  |  |  |
| 3 |  |  |  |

---

## 4.3. Tổng hợp AI hỗ trợ trong project

| Hạng mục | AI có hỗ trợ không? | Mức độ hỗ trợ | Ghi chú |
|---|---|---|---|
| Requirement | Có / Không | Ít / Trung bình / Nhiều |  |
| Design | Có / Không | Ít / Trung bình / Nhiều |  |
| Database | Có / Không | Ít / Trung bình / Nhiều |  |
| Coding | Có / Không | Ít / Trung bình / Nhiều |  |
| Debug | Có / Không | Ít / Trung bình / Nhiều |  |
| Testing | Có / Không | Ít / Trung bình / Nhiều |  |
| Report | Có / Không | Ít / Trung bình / Nhiều |  |
| Presentation | Có / Không | Ít / Trung bình / Nhiều |  |

---

## 4.4. Bài học rút ra

```text
Viết tại đây...
```

---

## 4.5. Hướng cải thiện tiếp theo

```text
Viết tại đây...
```

---

# 5. Cam kết cập nhật Changelog

Sinh viên/nhóm cam kết rằng nội dung changelog phản ánh đúng các thay đổi đã thực hiện trong quá trình làm bài tập/project.

| Đại diện sinh viên/nhóm | Ngày xác nhận |
|---|---|
|  |  |
