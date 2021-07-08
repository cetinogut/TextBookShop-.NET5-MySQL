using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TextBookShop.Models
{
    public class Company
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Distribütör Adı")]
        public string Name { get; set; }
        [Display(Name = "Adres")]
        public string StreetAddress { get; set; }
        [Display(Name = "Şehir")]
        public string City { get; set; }
        [Display(Name = "Bölge")]
        public string State { get; set; }
        [Display(Name = "Posta Kodu")]
        public string PostalCode { get; set; }
        [Display(Name = "Telefon No")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Yetki Durumu")]
        public bool IsAuthorizedCompany { get; set; }
    }
}
