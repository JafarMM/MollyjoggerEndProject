using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MollyjoggerBackend.ViewModels
{
    public class BasketViewModel
    {
        public int Id { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
        public int Count { get; set; } = 1;
    }
}
