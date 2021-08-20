using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MollyjoggerBackend.Models
{
    public class Category
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        [Required,MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string Description { get; set; }
        public ICollection<ShopOfProducts> ShopOfProducts { get; set; }
    }
}
