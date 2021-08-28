using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MollyjoggerBackend.Models
{
    public class SendMailBasket
    {

        [Required]
        public string Name { get; set; }

        [Required]
        public int Surname { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public int PostCode { get; set; }
        [Required]
        public int CardNumber { get; set; }
        [Required]
        public int CvcNumber { get; set; }
       
    }
}
