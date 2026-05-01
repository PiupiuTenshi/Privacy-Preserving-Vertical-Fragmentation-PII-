namespace PublicNode.Models
{
    public class PurchaseRecord
    {
        public int Id { get; set; } // PK 
        public string Encrypted_OID { get; set; } = string.Empty;
        public string PurchaseHistory { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }
}