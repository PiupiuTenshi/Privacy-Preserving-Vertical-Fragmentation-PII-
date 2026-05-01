using Microsoft.AspNetCore.Mvc;
using SecureNode.Data;
using System.Security.Cryptography;
using System.Text;

namespace SecureNode.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SecureController : ControllerBase
    {
        private readonly SecureDbContext _context;
        private readonly string _aesKey = "12345678901234567890123456789012";

        public SecureController()
        {
            _context = new SecureDbContext();
            _context.Database.EnsureCreated(); 

        }

        [HttpGet("decrypt")]
        public IActionResult GetPiiInfo([FromQuery] string encryptedOid)
        {
            try
            {
                string originalOidStr = DecryptString(encryptedOid);
                int oid = int.Parse(originalOidStr);

                var pii = _context.Customers.Find(oid);
                if (pii == null) return NotFound();

                return Ok(new { pii.Name, pii.SSN, pii.CreditCard });
            }
            catch
            {
                return BadRequest("Invalid Encrypted OID");
            }
        }

        private string DecryptString(string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(_aesKey);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}