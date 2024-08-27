using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Price")]
        [Range(1, 1000)]
        public double Price { get; set; }
        [ValidateNever]
        public string ImageUrl { get; set; }
    }
}
