using SoftUniBazar.Common;
using SoftUniBazar.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace SoftUniBazar.Models
{
    public class AddAdViewModel
    {
        [Required(ErrorMessage = "Name is required!")]
        [StringLength(ValidationConstants.AdNameMaxLength, MinimumLength = ValidationConstants.AdNameMinLength)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Description is required!")]
        [StringLength(ValidationConstants.AdDescriptionMaxLength, MinimumLength = ValidationConstants.AdDescriptionMinLength)]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Image URL is required!")]
        public string ImageUrl { get; set; } = null!;

        [Required(ErrorMessage = "Price is required!")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Category is required!")]
        public int CategoryId { get; set; }

        public List<CategoryViewModel>? Categories { get; set; } = null!;
    }
}
