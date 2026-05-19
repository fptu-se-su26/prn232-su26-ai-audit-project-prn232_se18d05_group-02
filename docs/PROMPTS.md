# Prompt Log

## 1. Thông tin chung

| Thông tin               | Nội dung                                                            |
|-------------------------|---------------------------------------------------------------------|
| Môn học                 | Building Cross-Platform Back-End Application With .NET              |
| Mã môn học              | PRN232                                                              |
| Lớp                     | SE18D05                                                             |
| Học kỳ                  | SU26                                                                |
| Tên bài tập / Project   | Group Project                                                       |
| Tên sinh viên / Nhóm    | Group 2                                                             |
| MSSV / Danh sách MSSV   | DE180158                                                            |
| Giảng viên hướng dẫn    | Lê Thiện Nhật Quang                                                 |
| Ngày bắt đầu            | 2026-05-18                                                          |
| Ngày hoàn thành         |                                                                     |

---

## 2. Mục đích của file Prompt Log

File này dùng để ghi lại các prompt quan trọng đã sử dụng trong quá trình thực hiện bài tập, lab, assignment hoặc project.

Sinh viên/nhóm cần ghi lại:

- Đã hỏi AI điều gì.
- Mục đích sử dụng prompt.
- Công cụ AI đã sử dụng.
- AI đã trả lời hoặc gợi ý gì.
- Kết quả đó có được áp dụng vào bài hay không.
- Sinh viên/nhóm đã kiểm tra, chỉnh sửa hoặc cải tiến gì sau khi nhận kết quả từ AI.

---

## 3. Công cụ AI đã sử dụng

Đánh dấu các công cụ AI đã sử dụng.

- [X] ChatGPT
- [X] Gemini
- [ ] Claude
- [X] GitHub Copilot
- [ ] Cursor
- [X] Antigravity
- [ ] Microsoft Copilot
- [ ] Perplexity
- [ ] Công cụ khác: ....................................

---

## 4. Bảng tổng hợp prompt đã sử dụng

| STT | Ngày | Công cụ AI | Mục đích | Prompt tóm tắt | Kết quả chính | Có sử dụng vào bài không? | Minh chứng |
|---:|---|---|---|---|---|---|---|
| 1 |  |  |  |  |  | Có / Không |  |
| 2 |  |  |  |  |  | Có / Không |  |
| 3 |  |  |  |  |  | Có / Không |  |
| 4 |  |  |  |  |  | Có / Không |  |
| 5 |  |  |  |  |  | Có / Không |  |
| 6 |  |  |  |  |  | Có / Không |  |
| 7 |  |  |  |  |  | Có / Không |  |
| 8 |  |  |  |  |  | Có / Không |  |
| 9 |  |  |  |  |  | Có / Không |  |
| 10 |  |  |  |  |  | Có / Không |  |

---

## 5. Prompt chi tiết

> Sinh viên/nhóm có thể nhân bản mẫu “Prompt số...” nhiều lần tùy số lượng prompt thực tế đã sử dụng.

---

### Prompt-01

| Nội dung                    | Thông tin                                                         |
|-----------------------------|-------------------------------------------------------------------|
| Ngày sử dụng                | 17/05/2026                                                        |
| Công cụ AI                  | ChatGPT / Gemini / Antigravity                                    |
| Mục đích sử dụng            | Generate project screen design and suggest models                 |
| Phần việc liên quan         | Requirement / Design / Database                                   |
| Mức độ sử dụng              | Hỗ trợ một phần                                                   |
#### 5.1. Prompt nguyên văn

```text
Liệt kê ra tất cả các màn hình cần thiết, đảm bảo bao quát được hết tất cả các chức năng.Lưu ý, tên màn hình bằng tiếng anh và trả lời một cách ngắn gọn.
```

#### 5.2. Bối cảnh khi viết prompt

Mô tả ngắn gọn vì sao sinh viên/nhóm cần dùng prompt này.

```text
Khi bắt đầu thực hiện dự án nhóm cho môn học PRN232 (.NET Back-End), nhóm cần phải xác định rõ phạm vi (scope) của dự án và các màn hình cần xây dựng để phục vụ cho các phân hệ người dùng (Khách du lịch, Admin/Nhân viên, Hướng dẫn viên). Do dự án quản lý tour du lịch có quy mô tương đối lớn và nhiều nghiệp vụ phức tạp, nhóm đã sử dụng prompt này để nhờ AI gợi ý một danh sách tổng thể các màn hình cần thiết nhằm có cái nhìn toàn diện và tránh bỏ sót các tính năng cốt lõi.
```

#### 5.3. Kết quả AI trả về

Tóm tắt nội dung AI đã trả lời hoặc gợi ý.

```text
Reference: AI_AUDIT_LOG.md #### 4.2. Kết quả AI gợi ý

AI đã đề xuất một danh sách rất chi tiết gồm 59 màn hình khác nhau, được phân loại rõ ràng thành 5 nhóm chính:
1. Public / Customer Screens (20 màn hình): Trang chủ, danh sách tour, chi tiết tour, đặt tour, thanh toán, quản lý đặt chỗ, hồ sơ cá nhân, làm quiz gợi ý travel style,...
2. Admin / Staff Screens (18 màn hình): Dashboard, quản lý tour, quản lý giá theo mùa, quản lý khuyến mãi, quản lý booking, phê duyệt hủy đặt tour, quản lý khách hàng, quản lý feedback,...
3. Guide / Operation Screens (9 màn hình): Quản lý danh sách guide, phân công guide, lịch làm việc của guide, cổng thông tin guide (Guide Portal) để xem/từ chối tour được phân công,...
4. System / Account Screens (8 màn hình): Đăng nhập, đăng ký, xác thực SMS, quản lý tài khoản, phân quyền vai trò (Role Permission),...
5. Report / Statistic Screens (4 màn hình): Thống kê doanh thu, thống kê đặt tour hàng tháng, thống kê tour đang mở, xếp hạng hướng dẫn viên,...
```

#### 5.4. Kết quả đã áp dụng vào bài

Mô tả phần nào từ kết quả AI đã được sử dụng vào bài tập/project.

```text
Nhóm đã sử dụng toàn bộ khung phân loại màn hình theo 5 nhóm do AI đề xuất. Cụ thể, nhóm đã giữ lại các tính năng cốt lõi như luồng tìm kiếm và đặt tour của khách hàng, các màn hình quản trị của Admin/Staff (đặc biệt là phần quản lý lịch trình, giá tour, khuyến mãi và phê duyệt hủy đặt tour), phân hệ Guide Portal cho hướng dẫn viên, cùng các chức năng hệ thống như phân quyền vai trò (Customer, Staff, Admin, Guide).
```

#### 5.5. Phần sinh viên/nhóm đã chỉnh sửa hoặc cải tiến

Mô tả sinh viên/nhóm đã thay đổi, kiểm tra, sửa lỗi hoặc cải tiến gì so với kết quả AI trả về.

```text
Nhóm đã tiến hành tối ưu hóa và tinh chỉnh lại danh sách của AI để phù hợp với giới hạn thời gian và thực tế triển khai:
1. Gộp các màn hình đơn lẻ có chức năng tương đồng: Thay vì chia nhỏ thành nhiều màn hình riêng biệt như gợi ý của AI (ví dụ: tách riêng Tour Detail, Tour Reviews, Tour Booking Form thành các màn hình khác nhau), nhóm đã gộp chúng lại thành các tab/component trong một màn hình lớn để tối ưu hóa trải nghiệm người dùng (UX) và giảm số lượng view cần viết.
2. Rút gọn số lượng màn hình: Giảm tổng số màn hình từ 59 xuống còn 38 màn hình tối ưu hơn.
3. Bổ sung chi tiết nghiệp vụ cụ thể cho từng màn hình:
   - Thêm phần quản lý thanh toán đặt cọc (deposit payment) và thanh toán nốt phần còn lại (remaining payment).
   - Thêm tính năng gửi thông báo tự động bằng SignalR/Email khi tour bị hủy.
   - Thêm tính năng cho phép Guide cập nhật trạng thái tour từ "Confirmed" sang "Finished".
   - Tích hợp SignalR vào hệ thống thông báo thời gian thực.
```

#### 5.6. Đánh giá chất lượng prompt

Đánh dấu các nhận xét phù hợp.

- [ ] Prompt rõ ràng
- [ ] Prompt có đủ bối cảnh
- [ ] Prompt còn thiếu thông tin
- [X] Prompt tạo ra kết quả tốt
- [ ] Prompt tạo ra kết quả chưa phù hợp
- [ ] Cần hỏi lại AI nhiều lần
- [ ] Cần tự kiểm tra và chỉnh sửa nhiều
- [ ] Kết quả AI có lỗi hoặc chưa chính xác

#### 5.7. Minh chứng liên quan

| Loại minh chứng | Nội dung |
|---|---|
| Link commit |  |
| File liên quan |  |
| Screenshot |  |
| Kết quả chạy/test |  |
| Link tài liệu/báo cáo |  |
| Ghi chú khác |  |

#### 5.8. Ghi chú thêm

```text
Viết tại đây...
```

---

### Prompt số 2

| Nội dung | Thông tin |
|---|---|
| Ngày sử dụng |  |
| Công cụ AI | ChatGPT / Gemini / Claude / GitHub Copilot / Cursor / Antigravity / Khác |
| Mục đích |  |
| Phần việc liên quan | Requirement / Design / Database / Coding / Testing / Debug / Report / Presentation / Other |
| Mức độ sử dụng | Hỏi ý tưởng / Hỏi giải thích / Hỏi review / Hỏi debug / Hỏi sinh code / Hỏi tối ưu |

#### 5.1. Prompt nguyên văn

```text
Dán nguyên văn prompt đã hỏi AI tại đây.
```

#### 5.2. Bối cảnh khi viết prompt

```text
Viết tại đây...
```

#### 5.3. Kết quả AI trả về

```text
Viết tại đây...
```

#### 5.4. Kết quả đã áp dụng vào bài

```text
Viết tại đây...
```

#### 5.5. Phần sinh viên/nhóm đã chỉnh sửa hoặc cải tiến

```text
Viết tại đây...
```

#### 5.6. Đánh giá chất lượng prompt

- [ ] Prompt rõ ràng
- [ ] Prompt có đủ bối cảnh
- [ ] Prompt còn thiếu thông tin
- [ ] Prompt tạo ra kết quả tốt
- [ ] Prompt tạo ra kết quả chưa phù hợp
- [ ] Cần hỏi lại AI nhiều lần
- [ ] Cần tự kiểm tra và chỉnh sửa nhiều
- [ ] Kết quả AI có lỗi hoặc chưa chính xác

#### 5.7. Minh chứng liên quan

| Loại minh chứng | Nội dung |
|---|---|
| Link commit |  |
| File liên quan |  |
| Screenshot |  |
| Kết quả chạy/test |  |
| Link tài liệu/báo cáo |  |
| Ghi chú khác |  |

#### 5.8. Ghi chú thêm

```text
Viết tại đây...
```

---

### Prompt số 3

| Nội dung | Thông tin |
|---|---|
| Ngày sử dụng |  |
| Công cụ AI | ChatGPT / Gemini / Claude / GitHub Copilot / Cursor / Antigravity / Khác |
| Mục đích |  |
| Phần việc liên quan | Requirement / Design / Database / Coding / Testing / Debug / Report / Presentation / Other |
| Mức độ sử dụng | Hỏi ý tưởng / Hỏi giải thích / Hỏi review / Hỏi debug / Hỏi sinh code / Hỏi tối ưu |

#### 5.1. Prompt nguyên văn

```text
Dán nguyên văn prompt đã hỏi AI tại đây.
```

#### 5.2. Bối cảnh khi viết prompt

```text
Viết tại đây...
```

#### 5.3. Kết quả AI trả về

```text
Viết tại đây...
```

#### 5.4. Kết quả đã áp dụng vào bài

```text
Viết tại đây...
```

#### 5.5. Phần sinh viên/nhóm đã chỉnh sửa hoặc cải tiến

```text
Viết tại đây...
```

#### 5.6. Đánh giá chất lượng prompt

- [ ] Prompt rõ ràng
- [ ] Prompt có đủ bối cảnh
- [ ] Prompt còn thiếu thông tin
- [ ] Prompt tạo ra kết quả tốt
- [ ] Prompt tạo ra kết quả chưa phù hợp
- [ ] Cần hỏi lại AI nhiều lần
- [ ] Cần tự kiểm tra và chỉnh sửa nhiều
- [ ] Kết quả AI có lỗi hoặc chưa chính xác

#### 5.7. Minh chứng liên quan

| Loại minh chứng | Nội dung |
|---|---|
| Link commit |  |
| File liên quan |  |
| Screenshot |  |
| Kết quả chạy/test |  |
| Link tài liệu/báo cáo |  |
| Ghi chú khác |  |

#### 5.8. Ghi chú thêm

```text
Viết tại đây...
```

---

## 6. Prompt quan trọng nhất

Chọn một prompt có ảnh hưởng lớn nhất đến bài tập/project.

### 6.1. Prompt được chọn

```text
Dán prompt quan trọng nhất tại đây.
```

### 6.2. Vì sao prompt này quan trọng?

```text
Viết tại đây...
```

### 6.3. Kết quả prompt này mang lại

```text
Viết tại đây...
```

### 6.4. Sinh viên/nhóm đã kiểm tra kết quả như thế nào?

```text
Viết tại đây...
```

### 6.5. Sinh viên/nhóm đã cải tiến gì từ kết quả AI?

```text
Viết tại đây...
```

---

## 7. Prompt chưa hiệu quả

Ghi lại ít nhất một prompt chưa tạo ra kết quả tốt hoặc chưa phù hợp.

### 7.1. Prompt chưa hiệu quả

```text
Dán prompt chưa hiệu quả tại đây.
```

### 7.2. Vì sao prompt này chưa hiệu quả?

```text
Viết tại đây...
```

Gợi ý nguyên nhân:

- Prompt quá ngắn.
- Thiếu bối cảnh bài toán.
- Không nêu rõ yêu cầu đầu ra.
- Không cung cấp ngôn ngữ lập trình/công nghệ đang dùng.
- Không đưa lỗi cụ thể.
- Không đưa ví dụ input/output.
- Không yêu cầu AI giải thích.
- Hỏi AI làm toàn bộ thay vì hỏi từng phần.

### 7.3. Cách cải thiện prompt

```text
Viết tại đây...
```

### 7.4. Prompt sau khi cải tiến

```text
Dán prompt đã được cải tiến tại đây.
```

### 7.5. Kết quả sau khi cải tiến prompt

```text
Viết tại đây...
```

---

## 8. Bài học về cách viết prompt

### 8.1. Khi viết prompt, em/nhóm cần cung cấp thông tin gì để AI trả lời tốt hơn?

```text
Viết tại đây...
```

Gợi ý:

- Mục tiêu cần đạt.
- Bối cảnh bài toán.
- Công nghệ/ngôn ngữ lập trình đang dùng.
- Input/output mong muốn.
- Ràng buộc của đề bài.
- Lỗi đang gặp.
- Format kết quả mong muốn.
- Yêu cầu AI giải thích từng bước.

### 8.2. Em/nhóm đã học được gì về cách đặt câu hỏi cho AI?

```text
Viết tại đây...
```

### 8.3. Lần sau em/nhóm sẽ cải thiện prompt như thế nào?

```text
Viết tại đây...
```

---

## 9. Phân loại prompt đã sử dụng

Đánh dấu số lượng prompt theo từng nhóm.

| Loại prompt | Số lượng | Ví dụ prompt tiêu biểu |
|---|---:|---|
| Prompt phân tích yêu cầu |  |  |
| Prompt giải thích kiến thức |  |  |
| Prompt thiết kế giải pháp |  |  |
| Prompt thiết kế database |  |  |
| Prompt sinh code mẫu |  |  |
| Prompt debug lỗi |  |  |
| Prompt viết test case |  |  |
| Prompt review code |  |  |
| Prompt tối ưu code |  |  |
| Prompt viết báo cáo |  |  |
| Prompt chuẩn bị thuyết trình |  |  |
| Prompt khác |  |  |

---

## 10. Checklist chất lượng prompt

Sinh viên/nhóm tự kiểm tra chất lượng prompt đã dùng.

| Tiêu chí | Đã đạt? | Ghi chú |
|---|:---:|---|
| Prompt có mục tiêu rõ ràng |  |  |
| Prompt có đủ bối cảnh |  |  |
| Prompt có nêu công nghệ/ngôn ngữ sử dụng |  |  |
| Prompt có nêu yêu cầu đầu ra |  |  |
| Prompt không yêu cầu AI làm toàn bộ bài một cách máy móc |  |  |
| Prompt có yêu cầu AI giải thích hoặc phân tích |  |  |
| Kết quả AI được kiểm tra lại |  |  |
| Kết quả AI được chỉnh sửa trước khi sử dụng |  |  |
| Prompt quan trọng được ghi lại đầy đủ |  |  |
| Prompt sai/chưa hiệu quả được rút kinh nghiệm |  |  |

---

## 11. Cam kết sử dụng prompt minh bạch

Sinh viên/nhóm cam kết rằng:

- Các prompt quan trọng đã được ghi lại trung thực.
- Không che giấu việc sử dụng AI trong các phần quan trọng của bài.
- Không nộp nguyên văn kết quả AI nếu chưa kiểm tra và chỉnh sửa.
- Có khả năng giải thích các phần đã sử dụng từ AI.
- Chịu trách nhiệm với sản phẩm cuối cùng.

| Đại diện sinh viên/nhóm | Ngày xác nhận |
|---|---|
|  |  |
