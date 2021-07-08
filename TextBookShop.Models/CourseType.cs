using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextBookShop.Models
{
   public  class CourseType
    {
        public int Id { get; set; }

        [Display(Name = "Sınav Kategorisi")]
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
