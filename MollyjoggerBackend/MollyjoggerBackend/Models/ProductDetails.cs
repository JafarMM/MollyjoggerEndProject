using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MollyjoggerBackend.Models
{
    public class ProductDetails
    {
        public int Id { get; set; }
        public string MainImg { get; set; }
        public string Smallimg1 { get; set; }
        public string Smallimg2 { get; set; }
        public string Smallimg3 { get; set; }
        public string Smallimg4 { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
        public string Properties { get; set; }
        [ForeignKey("ShopOfProducts")]
        public int ShopOfProductsId { get; set; }

        public ShopOfProducts ShopOfProducts{ get; set; }
    }
}
