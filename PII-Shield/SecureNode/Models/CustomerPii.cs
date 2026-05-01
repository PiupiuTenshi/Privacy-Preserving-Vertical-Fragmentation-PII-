using System.ComponentModel.DataAnnotations;

namespace SecureNode.Models
{
    public class CustomerPii
    {

        [Key]
        public int OID { get; set; } // PK
        public string Name { get; set; } = string.Empty;
        public string SSN { get; set; } = string.Empty;
        public string CreditCard { get; set; } = string.Empty;
    }
}