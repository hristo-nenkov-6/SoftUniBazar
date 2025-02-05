using Microsoft.Data.SqlClient.DataClassification;

namespace SoftUniBazar.Models
{
    public class AddToMyCartViewModel
    {
        public string UserId { get; set; } = null!;
        public int AdId { get; set; }
    }
}
