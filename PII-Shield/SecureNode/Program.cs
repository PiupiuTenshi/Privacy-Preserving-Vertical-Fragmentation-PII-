using SecureNode.Data;
using SecureNode.Models;
using Bogus;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
var app = builder.Build();

// --- BẮT ĐẦU ĐOẠN CODE TẠO DỮ LIỆU (SEEDING) ---
using (var scope = app.Services.CreateScope())
{
    var context = new SecureDbContext();
    context.Database.EnsureCreated();

    if (!context.Customers.Any())
    {
        Console.WriteLine("Đang tao 10,000 dong PII cho Secure Node, vui long doi...");
        
        var customerId = 1;
        // Cấu hình Bogus để tự động sinh tên, SSN và thẻ tín dụng giả
        var faker = new Faker<CustomerPii>()
            .RuleFor(c => c.OID, f => customerId++)
            .RuleFor(c => c.Name, f => f.Name.FullName())
            .RuleFor(c => c.SSN, f => f.Random.Replace("###-##-####"))
            .RuleFor(c => c.CreditCard, f => f.Finance.CreditCardNumber());

        var customers = faker.Generate(10000); // Sinh ra 10.000 object
        
        context.Customers.AddRange(customers);
        context.SaveChanges(); // Lưu vào SQLite (sẽ mất khoảng 2-4 giây)
        
        Console.WriteLine("Tao xong 10,000 khach hang!");
    }
}
// --- KẾT THÚC ĐOẠN CODE TẠO DỮ LIỆU ---

app.MapControllers();
app.Run();