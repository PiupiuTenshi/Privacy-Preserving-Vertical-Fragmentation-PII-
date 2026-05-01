using PublicNode.Data;
using PublicNode.Models;
using Bogus;
using System.Security.Cryptography;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
var app = builder.Build();

// --- BẮT ĐẦU ĐOẠN CODE TẠO DỮ LIỆU (SEEDING) ---
using (var scope = app.Services.CreateScope())
{
    var context = new PublicDbContext();
    context.Database.EnsureCreated();

    if (!context.Purchases.Any())
    {
        Console.WriteLine("Đang tao 10,000 dong Mua Hang cho Public Node, vui long doi...");
        
        // Key này phải GIỐNG HỆT key trong SecureController
        string aesKey = "12345678901234567890123456789012"; 
        
        // Hàm hỗ trợ mã hóa dùng để tạo Mock Data
        string EncryptOID(int oid)
        {
            byte[] iv = new byte[16];
            byte[] array;
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(aesKey);
                aes.IV = iv;
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (StreamWriter sw = new StreamWriter(cs))
                    {
                        sw.Write(oid.ToString());
                    }
                    array = ms.ToArray();
                }
            }
            return Convert.ToBase64String(array);
        }

        var purchaseId = 1;
        var faker = new Faker<PurchaseRecord>()
            .RuleFor(p => p.Id, f => purchaseId++)
            // Random hóa đơn này thuộc về 1 trong 10.000 khách hàng (OID từ 1 đến 10000)
            .RuleFor(p => p.Encrypted_OID, f => EncryptOID(f.Random.Number(1, 10000)))
            .RuleFor(p => p.PurchaseHistory, f => f.Commerce.ProductName()) // Tên món hàng ngẫu nhiên
            .RuleFor(p => p.Amount, f => decimal.Parse(f.Commerce.Price(10, 2000))); // Giá tiền ngẫu nhiên

        var purchases = faker.Generate(10000);
        
        context.Purchases.AddRange(purchases);
        context.SaveChanges();
        
        Console.WriteLine("Tao xong 10,000 lich su mua hang!");
    }
}
// --- KẾT THÚC ĐOẠN CODE TẠO DỮ LIỆU ---

app.MapControllers();
app.Run();