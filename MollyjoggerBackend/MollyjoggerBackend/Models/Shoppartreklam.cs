using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MollyjoggerBackend.Models
{
    public class Shoppartreklam
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string ButtonTitle { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
    }
}
