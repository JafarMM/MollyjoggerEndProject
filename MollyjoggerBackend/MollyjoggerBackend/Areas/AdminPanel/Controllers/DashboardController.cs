using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MollyjoggerBackend.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MollyjoggerBackend.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    //[Authorize(Roles =RoleConstants.AdminRole)]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
