using MollyjoggerBackend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MollyjoggerBackend.ViewModels
{
    public class HomeViewModel
    {
       
        public List<SliderImages> sliderImages { get; set; }
        public List<Products> Products { get; set; }
        public List<Shoppartreklam> Shoppartreklams { get; set; }
    
        public AboutUs AboutUs { get; set; }
        public Bio Bio { get; set; }
    }
}
