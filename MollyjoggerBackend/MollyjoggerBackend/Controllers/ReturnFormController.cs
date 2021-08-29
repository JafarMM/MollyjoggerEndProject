using Microsoft.AspNetCore.Mvc;
using MollyjoggerBackend.DataAccesLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MollyjoggerBackend.Controllers
{
    public class ReturnFormController : Controller
    {
        private readonly AppDbContext _dbContext;

        public ReturnFormController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
