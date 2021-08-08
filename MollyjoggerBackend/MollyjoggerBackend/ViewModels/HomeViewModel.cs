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
    }
}
