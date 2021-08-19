using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MollyjoggerBackend.DataAccesLayer;
using MollyjoggerBackend.Models;
using MollyjoggerBackend.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MollyjoggerBackend.Controllers
{
    public class HomeController : Controller
    {

        private readonly AppDbContext _dbContext;

        public HomeController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var sliderimages = _dbContext.SliderImages.ToList();
            var products = _dbContext.Products.ToList();
            var shoppartreklam = _dbContext.Shoppartreklams.ToList();
            var aboutus = _dbContext.AboutUs.FirstOrDefault();
            var homeViewModel = new HomeViewModel
            {
                sliderImages = sliderimages,
                Products=products,
                Shoppartreklams=shoppartreklam,
                AboutUs=aboutus
            };
            return View(homeViewModel);
        }
        public IActionResult Search(string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                return Content("Error");
            }

            var products = _dbContext.ShopOfProducts.OrderByDescending(x => x.Id).Where(x => x.ProductName.Contains(search.ToLower())).Take(3).ToList();
            var searchViewModel = new SearchViewModel
            {
                ShopOfProducts = products
            };
            return PartialView("_SearchGlobalPartial", searchViewModel);
        }

        [HttpGet]
        public IActionResult ContactUs()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ContactUs(SendMailDto sendMailDto)
        {


            if (!ModelState.IsValid) return View();

            try
            {
                MailMessage mail = new MailMessage();
                
                mail.From = new MailAddress("mollyjogger77@gmail.com");

                 
                mail.To.Add("cefer.mammadzade@bk.ru");

                mail.Subject = sendMailDto.Email;

                 

                mail.IsBodyHtml = true;

                string content = "Name : " + sendMailDto.Name;
                
                content += "<br/> Email : " + sendMailDto.Email;
                content += "<br/> Phone : " + sendMailDto.PhoneNumber;
                content += "<br/> Message : " + sendMailDto.Message;
                mail.Body = content;


                

                
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");

                
                NetworkCredential networkCredential = new NetworkCredential("mollyjogger77@gmail.com", "adminadmin123");
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = networkCredential;
                smtpClient.Port = 587; 
                smtpClient.EnableSsl = true;  
                smtpClient.Send(mail);

                ViewBag.Message = "Mail Send";

                 
                ModelState.Clear();

            }
            catch (Exception ex)
            {
                
                ViewBag.Message = ex.Message.ToString();
            }
            return View();
        }
        public IActionResult AddToBasket(int? id)
        {
            if (id == null)
                return NotFound();

            var product = _dbContext.ShopOfProducts.First(x=>x.Id==id);

            if (product == null)
                return NotFound();
            List<BasketViewModel> productList;

            var basketCookie = Request.Cookies["basket"];
            if (string.IsNullOrEmpty(basketCookie))
            {
                productList = new List<BasketViewModel>();
            }
            else
            {
                productList = JsonConvert.DeserializeObject<List<BasketViewModel>>(basketCookie);
            }

            var existProduct = productList.FirstOrDefault(x => x.Id == id);
           
            if (existProduct == null)
            {              
                productList.Add(new BasketViewModel { Id = product.Id });
            }
            else
            {
                existProduct.Count++;
            }

            var productJson = JsonConvert.SerializeObject(productList);
            Response.Cookies.Append("basket", productJson);

            return RedirectToAction("Index");
        }
        public IActionResult Basket()
        {
            var cookieBasket = Request.Cookies["basket"];
            if (string.IsNullOrEmpty(cookieBasket))
                return Content("No data in Basket");

            var basketViewModels = JsonConvert.DeserializeObject<List<BasketViewModel>>(cookieBasket);
            var result = new List<BasketViewModel>();
            foreach (var basketViewModel in basketViewModels)
            {
                var dbProduct = _dbContext.ShopOfProducts.Find(basketViewModel.Id);
                if (dbProduct == null)
                    continue; 
                basketViewModel.Price = dbProduct.Price;
                basketViewModel.Image1 = dbProduct.Image1;
                basketViewModel.Image2 = dbProduct.Image2;
                basketViewModel.ProductName = dbProduct.ProductName;
                result.Add(basketViewModel);
            }
            var basket = JsonConvert.SerializeObject(result);
            Response.Cookies.Append("basket", basket);
            var basketView = JsonConvert.DeserializeObject<List<BasketViewModel>>(basket);
            return View(basketView);
        }
    }
}
