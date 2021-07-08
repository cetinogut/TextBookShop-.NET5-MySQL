using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextBookShop.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [Display(Name = "Adı Soyadı")]
        public string Name { get; set; }
        [Display(Name = "Adres")]
        public string StreetAddress { get; set; }
        [Display(Name = "Şehir")]
        public string City { get; set; }
        [Display(Name = "Bölge")]
        public string State { get; set; }
        [Display(Name = "Posta Kodu")]
        public string PostalCode { get; set; }
       

        public int? CompanyId { get; set; }

        [ForeignKey("CompanyId")]
        [Display(Name = "Distribütör")]
        public Company Company { get; set; }

        [NotMapped] // not pushed to the DB
        [Display(Name = "Yetki")]
        public string Role { get; set; }

    }
}
