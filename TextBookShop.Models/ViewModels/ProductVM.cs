using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextBookShop.Models.ViewModels
{
    public class ProductVM
    {
        public Product Product { get; set; }
        public IEnumerable<SelectListItem> CategoryList { get; set; } // insatalled "Microsoft....ViewFeatures" package for SelectedListItem
        public IEnumerable<SelectListItem> CourseTypeList { get; set; } 
    }
}
