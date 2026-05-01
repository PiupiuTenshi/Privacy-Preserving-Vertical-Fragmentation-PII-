# 🛡️ PII-Shield: Distributed Database with Privacy Preserving

## ENGLISH
**University:** Posts and Telecommunications Institute of Technology (PTITHCM)  
**Major:** Software Engineering  
**Subject:** Distributed Database  
**Group:** N23DCCN051  
**Member:** Phạm Minh Sáng  

---

## 📖 Introduction
**PII-Shield** is a lightweight distributed database system (Microservices Architecture) designed to address the challenge of securing sensitive data (PII - Personally Identifiable Information) in a distributed environment.

This project simulates an e-commerce system where transaction data (Public) and customer data (Sensitive/PII) are fragmented and stored on completely isolated physical Nodes. The data joining process (Distributed Join) is securely performed over the network, combined with the AES encryption algorithm.

## 🏗️ System Architecture
The system consists of 3 independently operating Nodes that communicate via HTTP RESTful APIs:

1. **🌐 Public Node (Site A - Port 8080):**
   * Manages purchase transaction history.
   * Does not contain personally identifiable information, only stores `Encrypted_OID` (encrypted customer ID).
2. **🔒 Secure Node (Site B - Port 8081):**
   * Manages sensitive PII data (Name, CCCD/SSN, Credit card number).
   * Holds the **Secret Key (AES-256)** to decrypt `Encrypted_OID`.
3. **💻 Client Node (Coordinator - Port 5000):**
   * Acts as the user-facing application.
   * Executes the **Distributed Join** algorithm: Fetches transaction history from Node A → Sends decryption requests for OIDs and retrieves names from Node B → Aggregates results and returns them to the user.

## ✨ Key Features
* **Data Privacy:** Applies AES encryption to foreign keys linking data across Nodes.
* **Big Data (Mock Data):** Automatically generates `10,000` realistic mock records using the Bogus library.
* **Network Performance Optimization (N+1 Query Problem Solved):** Uses `Task.WhenAll` for asynchronous multi-threaded decryption and implements **Pagination**.
* **Fault Tolerance:** Graceful exception handling. If the Secure Node goes down unexpectedly, the system still displays the purchase history with a safe warning message *"PII Shielded (Node Offline)"* instead of crashing the entire application.
* **Dockerized:** Fully packaged using Docker & Docker Compose for platform-independent deployment.

## 🛠️ Technologies Used
* **Framework:** .NET 10.0 (ASP.NET Core Web API)
* **Database:** Entity Framework Core & SQLite (Auto-creates internal DB)
* **Cryptography:** System.Security.Cryptography (AES)
* **Containerization:** Docker & Docker Compose
* **Other:** Bogus library (Mock data)

---

## 🚀 Setup and Run Guide

### Prerequisites
* Install [Docker Desktop](https://www.docker.com/products/docker-desktop).
* Install [.NET 10.0 SDK](https://dotnet.microsoft.com/download) (if you want to run directly without Docker).

### Method 1: Run with Docker Compose (Recommended)
This is the fastest way to launch a perfect distributed environment.

1. Clone the project to your machine and open a Terminal in the root directory (containing the `docker-compose.yml` file).
2. Run the following command:
   ```bash
   docker-compose up --build
3. On the first run, the system will take a few seconds to automatically create the database files and "seed" 10,000 sample records into SQLite. Please wait until the Terminal finishes executing all processes and displays messages like: `Application started. Press Ctrl+C to shut down`. or `Now listening on: http://[::]...` for all 3 nodes.

### Method 2: Run Locally with .NET CLI (Without Docker)
If you encounter difficulties with Docker, you can run directly using the .NET CLI:
Open 3 separate Terminals, navigate into each project directory and run the following commands:

- Terminal 1: `cd PublicNode && dotnet run --urls="http://localhost:8080"`

- Terminal 2: `cd SecureNode && dotnet run --urls="http://localhost:8081"`

- Terminal 3: `cd ClientNode && dotnet run --urls="http://localhost:5000"`

## 🔍 Demo Scenarios & API Endpoints
After the system has started successfully (via Docker or Local), open a web browser or use Postman/Thunder Client to test the following features:

1. Distributed Join with Pagination (Normal Operation)
Query the list of transaction history. The Client Node will automatically call the Public Node to fetch transactions, then call the Secure Node to decrypt OIDs and retrieve customer names.
GET `http://localhost:5000/api/client/all-reports?page=1&pageSize=15`
(You can adjust the `page` and `pageSize` parameters in the URL to view different data sets).

2. System Failure Scenario (Fault Tolerance)
This scenario simulates the sensitive data server (Site B - Secure Node) being powered off or having a network disconnection.

1. While the Docker system is running, open a new Terminal and type the following command to stop Node B:

    ```bash
   docker-compose up --build
2. Go back to your browser and reload (F5) the all-reports link from Scenario 1.

3. Expected result: Instead of crashing the entire system or leaving the page loading indefinitely, the API immediately returns the purchase history list. However, the CustomerName field will automatically be replaced with a safe warning string: "PII Shielded (Node Offline)", ensuring an uninterrupted user experience.

----------------------------------------------------------------------------------------------------------
## Việt Nam
**Trường:** Học viện Công nghệ Bưu Chính Viễn Thông (PTITHCM)
**Chuyên ngành:** Kỹ thuật phần mềm (Software Engineering)
**Môn học:** Cơ sở dữ liệu phân tán (Distributed Database)
**Nhóm:** N23DCCN051
**Thành viên:** Phạm Minh Sáng

---

## 📖 Giới thiệu 
**PII-Shield** là một hệ thống cơ sở dữ liệu phân tán thu nhỏ (Microservices Architecture) được thiết kế nhằm giải quyết bài toán bảo mật dữ liệu nhạy cảm (PII - Personally Identifiable Information) trong môi trường phân tán. 

Dự án mô phỏng một hệ thống thương mại điện tử nơi dữ liệu giao dịch (Public) và dữ liệu khách hàng (Sensitive/PII) được phân mảnh và lưu trữ ở các Node (máy chủ) vật lý hoàn toàn cách ly. Quá trình ghép nối dữ liệu (Distributed Join) được thực hiện an toàn qua mạng, kết hợp cùng thuật toán mã hóa AES.

## 🏗️ Kiến trúc hệ thống 
Hệ thống bao gồm 3 Node hoạt động độc lập và giao tiếp với nhau qua HTTP RESTful API:

1. **🌐 Public Node (Site A - Port 8080):** 
   - Quản lý lịch sử giao dịch mua hàng.
   - Không chứa thông tin định danh khách hàng, chỉ lưu trữ `Encrypted_OID` (ID khách hàng đã bị mã hóa).
2. **🔒 Secure Node (Site B - Port 8081):**
   - Quản lý dữ liệu PII nhạy cảm (Tên, CCCD/SSN, Số thẻ tín dụng).
   - Nắm giữ **Secret Key (AES-256)** để giải mã `Encrypted_OID`.
3. **💻 Client Node (Trạm điều phối - Port 5000):**
   - Đóng vai trò là ứng dụng phía người dùng.
   - Thực hiện thuật toán **Distributed Join**: Lấy lịch sử giao dịch từ Node A -> Gửi yêu cầu giải mã OID và lấy Tên từ Node B -> Tổng hợp kết quả trả về cho người dùng.

## ✨ Tính năng nổi bật 
- **Bảo mật dữ liệu (Data Privacy):** Áp dụng mã hóa AES cho các khóa ngoại (Foreign Keys) liên kết giữa các Node.
- **Dữ liệu lớn (Mock Data):** Tự động sinh `10,000` records giả lập dữ liệu thực tế bằng thư viện `Bogus`.
- **Tối ưu hiệu năng mạng (N+1 Query Problem Solved):** Sử dụng `Task.WhenAll` để xử lý bất đồng bộ (Asynchronous) nhiều luồng giải mã cùng lúc và áp dụng **Phân trang (Pagination)**.
- **Xử lý sự cố (Fault Tolerance):** Khả năng bắt lỗi (Exception Handling) mượt mà. Khi Secure Node bị sập đột ngột, hệ thống vẫn hiển thị lịch sử mua hàng kèm theo cảnh báo *"PII Shielded (Node Offline)"* thay vì làm treo toàn bộ ứng dụng.
- **Dockerized:** Đóng gói hoàn chỉnh bằng Docker & Docker Compose để triển khai độc lập nền tảng.

## 🛠️ Công nghệ sử dụng 
- **Framework:** .NET 10.0 (ASP.NET Core Web API)
- **Database:** Entity Framework Core & SQLite (Tự động tạo DB nội bộ)
- **Cryptography:** System.Security.Cryptography (AES)
- **Containerization:** Docker & Docker Compose
- **Khác:** Thư viện Bogus (Mock data)

---

## 🚀 Hướng dẫn cài đặt và chạy dự án (How to run)

### Yêu cầu tiên quyết 
- Cài đặt [Docker Desktop](https://www.docker.com/products/docker-desktop).
- Cài đặt [.NET 10.0 SDK](https://dotnet.microsoft.com/download) (Nếu muốn chạy trực tiếp không qua Docker).

### Cách 1: Chạy bằng Docker Compose (Khuyên dùng)
Đây là cách nhanh nhất để khởi chạy môi trường phân tán hoàn hảo.
1. Clone dự án về máy và mở Terminal tại thư mục gốc (nơi chứa file `docker-compose.yml`).
2. Chạy lệnh sau:
   ```bash
   docker-compose up --build
3. Lần khởi chạy đầu tiên, hệ thống sẽ mất khoảng vài giây để tự động tạo file cơ sở dữ liệu và "bơm" (seed) 10.000 dòng dữ liệu mẫu vào SQLite. Bạn hãy đợi đến khi Terminal chạy xong các tiến trình và xuất hiện dòng chữ tương tự như: `Application started. Press Ctrl+C to shut down.` hoặc `Now listening on: http://[::]...` cho cả 3 node.

### Cách 2: Chạy Local bằng .NET CLI (Không dùng Docker)
Nếu bạn gặp khó khăn với Docker, có thể chạy trực tiếp bằng công cụ dòng lệnh của .NET:
Mở 3 Terminal riêng biệt, di chuyển vào từng thư mục dự án và chạy các lệnh sau:
- Terminal 1: `cd PublicNode && dotnet run --urls="http://localhost:8080"`
- Terminal 2: `cd SecureNode && dotnet run --urls="http://localhost:8081"`
- Terminal 3: `cd ClientNode && dotnet run --urls="http://localhost:5000"`

---

## 🔍 Kịch bản Demo & API Endpoints

Sau khi hệ thống khởi chạy thành công (bằng Docker hoặc Local), hãy mở trình duyệt web hoặc sử dụng phần mềm Postman/Thunder Client để kiểm tra các tính năng:

### 1. Phép Join phân tán có phân trang (Hoạt động bình thường)
Truy vấn danh sách lịch sử giao dịch. Client Node sẽ tự động gọi Public Node lấy giao dịch, sau đó gọi Secure Node để giải mã OID và lấy tên khách hàng.
👉 **GET** `http://localhost:5000/api/client/all-reports?page=1&pageSize=15`
*(Bạn có thể thay đổi số `page` và `pageSize` trên đường link để xem các dữ liệu khác nhau).*

### 2. Kịch bản lỗi hệ thống 
Kịch bản này mô phỏng việc máy chủ chứa dữ liệu nhạy cảm (Site B - Secure Node) bị sập nguồn hoặc bị ngắt kết nối mạng.
1. Trong khi hệ thống Docker đang chạy, mở một Terminal mới và gõ lệnh sau để tắt Node B:
   ```bash
   docker stop secure-node
2. Quay lại trình duyệt và tải lại (F5) đường link `all-reports` ở kịch bản 1.
3. Kết quả kỳ vọng: Thay vì báo lỗi toàn bộ hệ thống hoặc tải trang vô tận, API vẫn trả về danh sách lịch sử mua hàng ngay lập tức. Tuy nhiên, trường `CustomerName` sẽ tự động chuyển thành chuỗi cảnh báo an toàn: "PII Shielded (Node Offline)", đảm bảo trải nghiệm người dùng không bị gián đoạn.