using SoftUniBazar.Common;
using System.ComponentModel.DataAnnotations;

namespace SoftUniBazar.Data.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(ValidationConstants.CategoryNameMaxLength, MinimumLength = ValidationConstants.CategoryNameMinLength)]
        public string Name { get; set; } = null!;

        [Required]
        public ICollection<Ad> Ads { get; set; } = null!;
    }
}
