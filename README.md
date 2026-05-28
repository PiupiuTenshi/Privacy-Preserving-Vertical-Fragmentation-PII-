# ENGLISH 
# 🛡️ PII-Shield: Privacy-Preserving Vertical Fragmentation

**University:** Posts and Telecommunications Institute of Technology - PTIT HCM  
**Major:** Software Engineering  
**Subject:** Distributed Database  
**Group:** N23DCCN051  
**Member:** Phạm Minh Sáng  
**Project ID:** #106 - Privacy-Preserving Vertical Fragmentation

---

## 1. Introduction

**PII-Shield** is a lightweight distributed database system designed to protect **Personally Identifiable Information (PII)** in a distributed environment.

The project simulates an e-commerce system where customer identity data and purchase-history data are separated into different sites using **vertical fragmentation**. The public site stores only non-sensitive transaction data and an encrypted linking identifier named `Encrypted_OID`. The secure site stores the original `OID` and sensitive attributes such as name, SSN/CCCD, and credit card number.

The main idea is simple:

> The Public Node can support transaction queries, but it cannot identify customers by itself. Identity reconstruction must go through the Secure Node.

---

## 2. System Architecture

The system has 3 independently running nodes that communicate via HTTP REST APIs.

| Node | Site | Port | Responsibility |
|---|---:|---:|---|
| **Public Node** | Site A | `8080` | Stores public transaction data: `Encrypted_OID`, `PurchaseHistory`. |
| **Secure Node** | Site B | `8081` | Stores sensitive PII: `OID`, `Name`, `SSN/CCCD`, `CreditCard`, and AES key logic. |
| **Client Node** | Site C | `5000` | Acts as query coordinator and returns final reports to the user. |

### Data Fragmentation

Global relation:

```text
Customer_Data(OID, Name, SSN/CCCD, CreditCard, PurchaseHistory)
```

Vertical fragments:

```text
F1 - Public Fragment:  (Encrypted_OID, PurchaseHistory)
F2 - Secure Fragment:  (OID, Name, SSN/CCCD, CreditCard)
```

Secure reconstruction flow:

```text
ResolvedOID = SecureNode.Resolve(F1.Encrypted_OID)
R = Resolved(F1) JOIN F2 ON ResolvedOID = F2.OID
```

This is intentionally different from a direct plaintext join because `F1` does **not** store the original `OID`.

---

## 3. Key Features

- **Privacy-preserving vertical fragmentation:** separates public analytical data from sensitive identity data.
- **Encrypted OID linking:** the Public Node stores `Encrypted_OID` instead of plaintext `OID`.
- **Distributed join simulation:** the Client Node coordinates cross-node reconstruction through REST APIs.
- **Fault tolerance demo:** if the Secure Node is offline, the system still returns purchase history while shielding PII.
- **Mock e-commerce dataset:** generates around `10,000` sample records for testing.
- **Dockerized deployment:** each node runs in an isolated Docker container.

---

## 4. Technologies Used

| Layer | Technology |
|---|---|
| Backend | C# / .NET 10.0 / ASP.NET Core Web API |
| Database | SQLite + Entity Framework Core |
| Cryptography | AES via `System.Security.Cryptography` |
| Containerization | Docker + Docker Compose |
| Mock Data | Bogus library |

---

## 5. Project Structure

```text
Privacy-Preserving-Vertical-Fragmentation-PII-Shield/
├── README.md
└── PII-Shield/
    ├── ClientNode/
    ├── PublicNode/
    ├── SecureNode/
    ├── PII-Shield.sln
    └── docker-compose.yml
```

---

## 6. Setup and Run Guide

### Prerequisites

Install:

- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [.NET SDK](https://dotnet.microsoft.com/download) if you want to run the services without Docker

### Method 1: Run with Docker Compose recommended

Clone the repository and move into the directory that contains `docker-compose.yml`:

```bash
git clone https://github.com/PiupiuTenshi/Privacy-Preserving-Vertical-Fragmentation-PII-Shield.git
cd Privacy-Preserving-Vertical-Fragmentation-PII-Shield/PII-Shield
docker compose up --build
```

If your Docker Compose uses the older command format, use:

```bash
docker-compose up --build
```

On the first run, the services may take a few seconds to create SQLite database files and seed mock data.

### Method 2: Run locally with .NET CLI

Open 3 terminals from the `PII-Shield` directory.

Terminal 1:

```bash
cd PublicNode
dotnet run --urls="http://localhost:8080"
```

Terminal 2:

```bash
cd SecureNode
dotnet run --urls="http://localhost:8081"
```

Terminal 3:

```bash
cd ClientNode
dotnet run --urls="http://localhost:5000"
```

---

## 7. Demo Scenarios

### Scenario 1: Normal distributed join

Open a browser, Postman, or Thunder Client and call:

```http
GET http://localhost:5000/api/client/all-reports?page=1&pageSize=15
```

Expected result:

- Client Node fetches transaction data from Public Node.
- Client Node requests authorized OID resolution/customer data from Secure Node.
- The final response contains paginated purchase-history reports.

You can change `page` and `pageSize` to test different result sets.

### Scenario 2: Secure Node failure

This scenario simulates the sensitive data server going offline.

While Docker Compose is running, open a new terminal and execute:

```bash
docker stop secure-node
```

Then reload:

```http
GET http://localhost:5000/api/client/all-reports?page=1&pageSize=15
```

Expected result:

- The system should not crash.
- Purchase-history data from the Public Node should still be returned.
- Customer identity should not be reconstructed.
- The customer display field should show a safe value such as:

```text
PII Shielded (Node Offline)
```

To start the Secure Node again:

```bash
docker start secure-node
```

---


## 8. Suggested Repository Description and Topics

Suggested GitHub description:

```text
Privacy-preserving distributed database using vertical fragmentation, AES-encrypted OID linking, Dockerized .NET microservices, and SQLite.
```

Suggested topics:

```text
distributed-database, vertical-fragmentation, privacy-preserving, pii, aes-encryption, dotnet, docker, sqlite, microservices
```

----------------------------------------------------------------------------------------------------------------------------------

# VIETNAM
# 🛡️ PII-Shield: Phân Mảnh Dọc Bảo Vệ Quyền Riêng Tư

**Trường:** Học viện Công nghệ Bưu chính Viễn thông - PTIT HCM  
**Chuyên ngành:** Kỹ thuật Phần mềm  
**Môn học:** Cơ sở Dữ liệu Phân tán  
**Nhóm:** N23DCCN051  
**Thành viên:** Phạm Minh Sáng  
**Mã đề tài:** #106 - Phân Mảnh Dọc Bảo Vệ Thông Tin Cá Nhân

---

## 1. Giới Thiệu

**PII-Shield** là một hệ thống cơ sở dữ liệu phân tán nhẹ được thiết kế để bảo vệ **Thông Tin Nhận Dạng Cá Nhân (PII)** trong môi trường phân tán.

Dự án mô phỏng một hệ thống thương mại điện tử, trong đó dữ liệu danh tính khách hàng và dữ liệu lịch sử mua hàng được tách biệt vào các site khác nhau bằng **phân mảnh dọc**. Site công khai chỉ lưu trữ dữ liệu giao dịch không nhạy cảm và một định danh liên kết được mã hóa tên là `Encrypted_OID`. Site bảo mật lưu trữ `OID` gốc và các thuộc tính nhạy cảm như tên, SSN/CCCD, và số thẻ tín dụng.

Ý tưởng chính rất đơn giản:

> Node Công khai có thể xử lý các truy vấn giao dịch, nhưng không thể tự mình xác định danh tính khách hàng. Việc tái tạo danh tính phải đi qua Node Bảo mật.

---

## 2. Kiến Trúc Hệ Thống

Hệ thống gồm 3 node chạy độc lập, giao tiếp với nhau qua HTTP REST API.

| Node | Site | Cổng | Vai trò |
|---|---:|---:|---|
| **Node Công khai** | Site A | `8080` | Lưu trữ dữ liệu giao dịch công khai: `Encrypted_OID`, `PurchaseHistory`. |
| **Node Bảo mật** | Site B | `8081` | Lưu trữ PII nhạy cảm: `OID`, `Name`, `SSN/CCCD`, `CreditCard`, và logic khóa AES. |
| **Node Khách hàng** | Site C | `5000` | Đóng vai trò điều phối truy vấn và trả về báo cáo cuối cùng cho người dùng. |

### Phân Mảnh Dữ Liệu

Quan hệ toàn cục:

```text
Customer_Data(OID, Name, SSN/CCCD, CreditCard, PurchaseHistory)
```

Các mảnh dọc:

```text
F1 - Mảnh Công khai:  (Encrypted_OID, PurchaseHistory)
F2 - Mảnh Bảo mật:   (OID, Name, SSN/CCCD, CreditCard)
```

Luồng tái tạo bảo mật:

```text
ResolvedOID = SecureNode.Resolve(F1.Encrypted_OID)
R = Resolved(F1) JOIN F2 ON ResolvedOID = F2.OID
```

Cách này khác biệt có chủ đích so với phép nối plaintext trực tiếp, vì `F1` **không** lưu trữ `OID` gốc.

---

## 3. Tính Năng Chính

- **Phân mảnh dọc bảo vệ quyền riêng tư:** tách biệt dữ liệu phân tích công khai khỏi dữ liệu danh tính nhạy cảm.
- **Liên kết OID mã hóa:** Node Công khai lưu `Encrypted_OID` thay vì `OID` dạng plaintext.
- **Mô phỏng phép nối phân tán:** Node Khách hàng điều phối việc tái tạo dữ liệu liên node qua REST API.
- **Demo chịu lỗi:** nếu Node Bảo mật ngoại tuyến, hệ thống vẫn trả về lịch sử mua hàng trong khi che chắn PII.
- **Bộ dữ liệu thương mại điện tử mẫu:** tạo khoảng `10.000` bản ghi mẫu để kiểm thử.
- **Triển khai Docker hóa:** mỗi node chạy trong một container Docker riêng biệt.

---

## 4. Công Nghệ Sử Dụng

| Tầng | Công nghệ |
|---|---|
| Backend | C# / .NET 10.0 / ASP.NET Core Web API |
| Cơ sở dữ liệu | SQLite + Entity Framework Core |
| Mật mã học | AES qua `System.Security.Cryptography` |
| Container hóa | Docker + Docker Compose |
| Dữ liệu mẫu | Thư viện Bogus |

---

## 5. Cấu Trúc Dự Án

```text
Privacy-Preserving-Vertical-Fragmentation-PII-Shield/
├── README.md
└── PII-Shield/
    ├── ClientNode/
    ├── PublicNode/
    ├── SecureNode/
    ├── PII-Shield.sln
    └── docker-compose.yml
```

---

## 6. Hướng Dẫn Cài Đặt và Chạy

### Yêu Cầu

Cài đặt:

- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [.NET SDK](https://dotnet.microsoft.com/download) nếu bạn muốn chạy các dịch vụ mà không dùng Docker

### Phương án 1: Chạy với Docker Compose (khuyến nghị)

Clone repository và di chuyển vào thư mục chứa `docker-compose.yml`:

```bash
git clone https://github.com/PiupiuTenshi/Privacy-Preserving-Vertical-Fragmentation-PII-Shield.git
cd Privacy-Preserving-Vertical-Fragmentation-PII-Shield/PII-Shield
docker compose up --build
```

Nếu Docker Compose của bạn dùng định dạng lệnh cũ hơn, hãy sử dụng:

```bash
docker-compose up --build
```

Ở lần chạy đầu tiên, các dịch vụ có thể mất vài giây để tạo file SQLite và seed dữ liệu mẫu.

### Phương án 2: Chạy cục bộ với .NET CLI

Mở 3 terminal từ thư mục `PII-Shield`.

Terminal 1:

```bash
cd PublicNode
dotnet run --urls="http://localhost:8080"
```

Terminal 2:

```bash
cd SecureNode
dotnet run --urls="http://localhost:8081"
```

Terminal 3:

```bash
cd ClientNode
dotnet run --urls="http://localhost:5000"
```

---

## 7. Các Kịch Bản Demo

### Kịch bản 1: Phép nối phân tán bình thường

Mở trình duyệt, Postman, hoặc Thunder Client và gọi:

```http
GET http://localhost:5000/api/client/all-reports?page=1&pageSize=15
```

Kết quả mong đợi:

- Node Khách hàng lấy dữ liệu giao dịch từ Node Công khai.
- Node Khách hàng yêu cầu phân giải OID được ủy quyền / dữ liệu khách hàng từ Node Bảo mật.
- Phản hồi cuối cùng chứa các báo cáo lịch sử mua hàng có phân trang.

Bạn có thể thay đổi `page` và `pageSize` để kiểm thử các tập kết quả khác nhau.

### Kịch bản 2: Node Bảo mật bị lỗi

Kịch bản này mô phỏng trường hợp máy chủ dữ liệu nhạy cảm ngoại tuyến.

Trong khi Docker Compose đang chạy, mở một terminal mới và thực thi:

```bash
docker stop secure-node
```

Sau đó tải lại:

```http
GET http://localhost:5000/api/client/all-reports?page=1&pageSize=15
```

Kết quả mong đợi:

- Hệ thống không bị crash.
- Dữ liệu lịch sử mua hàng từ Node Công khai vẫn được trả về.
- Danh tính khách hàng không được tái tạo.
- Trường hiển thị khách hàng sẽ hiện một giá trị an toàn như:

```text
PII Shielded (Node Offline)
```

Để khởi động lại Node Bảo mật:

```bash
docker start secure-node
```

---

## 8. Gợi Ý Mô Tả và Chủ Đề Repository

Mô tả GitHub gợi ý:

```text
Privacy-preserving distributed database using vertical fragmentation, AES-encrypted OID linking, Dockerized .NET microservices, and SQLite.
```

Chủ đề gợi ý:

```text
distributed-database, vertical-fragmentation, privacy-preserving, pii, aes-encryption, dotnet, docker, sqlite, microservices
```