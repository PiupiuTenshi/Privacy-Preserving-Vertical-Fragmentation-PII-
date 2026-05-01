using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json.Nodes;

namespace ClientNode.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public ClientController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet("full-report/{purchaseId}")]
        public async Task<IActionResult> GetDistributedJoin(int purchaseId)
        {
            var watch = Stopwatch.StartNew();

            var publicRes = await _httpClient.GetAsync($"http://public-node:8080/api/public/purchase/{purchaseId}");
            if (!publicRes.IsSuccessStatusCode) return NotFound("Purchase not found in Site A");
            
            var publicData = JsonNode.Parse(await publicRes.Content.ReadAsStringAsync());
            string encryptedOid = publicData["encrypted_OID"].ToString();

            string customerName = "PII Shielded (Node Offline)";
            try
            {
                var secureRes = await _httpClient.GetAsync($"http://public-node:8081/api/secure/decrypt?encryptedOid={Uri.EscapeDataString(encryptedOid)}");
                if (secureRes.IsSuccessStatusCode)
                {
                    var secureData = JsonNode.Parse(await secureRes.Content.ReadAsStringAsync());
                    customerName = secureData["name"].ToString();
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Don't find port 8081");
            }

            watch.Stop();

            return Ok(new {
                ExecutionTime_ms = watch.ElapsedMilliseconds,
                CustomerName = customerName, 
                PurchaseHistory = publicData["purchaseHistory"].ToString(), 
                Amount = publicData["amount"].ToString() 
            });
        }

        [HttpGet("all-reports")]
        public async Task<IActionResult> GetAllDistributedReports([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var watch = Stopwatch.StartNew();

            var publicRes = await _httpClient.GetAsync($"http://public-node:8080/api/public/purchases?page={page}&pageSize={pageSize}");
            if (!publicRes.IsSuccessStatusCode) return BadRequest("Lỗi khi gọi Site A");

            var publicDataList = JsonNode.Parse(await publicRes.Content.ReadAsStringAsync())?.AsArray();
            if (publicDataList == null || publicDataList.Count == 0) return Ok(new { Message = "Hết dữ liệu" });

            var tasks = publicDataList.Select(async item =>
            {
                string encryptedOid = item["encrypted_OID"].ToString();
                string customerName = "PII Shielded (Node Offline)";

                try
                {
                    var secureRes = await _httpClient.GetAsync($"http://public-node:8081/api/secure/decrypt?encryptedOid={Uri.EscapeDataString(encryptedOid)}");
                    if (secureRes.IsSuccessStatusCode)
                    {
                        var secureData = JsonNode.Parse(await secureRes.Content.ReadAsStringAsync());
                        customerName = secureData["name"].ToString();
                    }
                }
                catch { }

                return new
                {
                    PurchaseId = item["id"].ToString(),
                    CustomerName = customerName,
                    PurchaseHistory = item["purchaseHistory"].ToString(),
                    Amount = item["amount"].ToString()
                };
            }).ToList();

            var finalResult = await Task.WhenAll(tasks);

            watch.Stop();

            return Ok(new
            {
                Page = page,
                PageSize = pageSize,
                ExecutionTime_ms = watch.ElapsedMilliseconds,
                TotalItemsInPage = finalResult.Length,
                Data = finalResult
            });
        }
    }
}