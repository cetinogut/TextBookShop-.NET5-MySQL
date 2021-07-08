using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextBookShop.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Kitap Adı")]
        public string Title { get; set; }

        [Display(Name = "Kitap Detay")]
        public string Description { get; set; }
        [Required]
        public string ISBN { get; set; }
        [Required]

        [Display(Name = "Yayınevi")]
        public string Author { get; set; }
        [Required]
        [Range(1, 10000)]
        [Display(Name = "Liste Fiyatı")]
        public double ListPrice { get; set; }
        [Required]
        [Range(1, 10000)]
        [Display(Name = "Fiyatı")]
        public double Price { get; set; }
        [Required]
        [Range(1, 10000)]
        [Display(Name = "Liste Fiyatı (31-50 adet)")]
        public double Price30 { get; set; }
        [Required]
        [Range(1, 10000)]
        [Display(Name = "Liste Fiyatı (51-99 adet)")]
        public double Price50 { get; set; }
        [Required]
        [Range(1, 10000)]
        [Display(Name = "Liste Fiyatı (101+ adet)")]
        public double Price100 { get; set; }
        public string ImageUrl { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        [Required]
        public int CourseTypeId { get; set; }

        [ForeignKey("CourseTypeId")]
        public CourseType CourseType { get; set; }
    }
}
