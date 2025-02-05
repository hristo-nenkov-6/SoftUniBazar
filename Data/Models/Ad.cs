using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using SoftUniBazar.Common;

namespace SoftUniBazar.Data.Models
{
    public class Ad
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(ValidationConstants.AdNameMaxLength, MinimumLength = ValidationConstants.AdNameMinLength)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(ValidationConstants.AdDescriptionMaxLength, MinimumLength = ValidationConstants.AdDescriptionMinLength)]
        public string Description { get; set; } = null!;

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string OwnerId { get; set; } = null!;

        [Required]
        public IdentityUser Owner { get; set; } = null!;

        [Required]
        public string ImageUrl { get; set; } = null!;

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public int CategoryId { get; set;}

        [Required]
        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; } = null!;

        public ICollection<AdBuyer> adsBuyers { get; set; } = null!;
    }
}
