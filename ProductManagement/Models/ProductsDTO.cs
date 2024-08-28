using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Models
{
    public class ProductsDTO
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Price")]
        
        public double Price { get; set; }
        [ValidateNever]
        public IFormFile? ImageUrl { get; set; }
    }
}
