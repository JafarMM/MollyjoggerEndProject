using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MollyjoggerBackend.Areas.AdminPanel.Utils;
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
using System.Text.RegularExpressions;
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

        #region HomeController
        public IActionResult Index()
        {
            var sliderimages = _dbContext.SliderImages.ToList();
            var products = _dbContext.Products.ToList();
            var shoppartreklam = _dbContext.Shoppartreklams.ToList();
            var aboutus = _dbContext.AboutUs.FirstOrDefault();
            var shopofproducts = _dbContext.ShopOfProducts.OrderByDescending(x=>x.Id).Take(4).ToList();
            var homeViewModel = new HomeViewModel
            {
                sliderImages = sliderimages,
                Products=products,
                Shoppartreklams=shoppartreklam,
                AboutUs=aboutus,
                ShopOfProducts=shopofproducts
            };
            return View(homeViewModel);
        }
        #endregion

        #region GlobalSearch
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
        #endregion

        #region Subscribe
        public async Task<IActionResult> Subscribe(string email)
        {
            if (email == null)
            {
                return Content("You must write email address");
            }
            

            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);
            if (!match.Success)
            {
                return Content("It is not email address!");
            }
            var isExist = await _dbContext.Subscribes.AnyAsync(x => x.Email == email);
            if (isExist)
            {
                return Content("You are already subscribe Edu Home site");
            }
            Subscribe subscribe = new Subscribe { Email = email };
            await _dbContext.Subscribes.AddAsync(subscribe);
            await _dbContext.SaveChangesAsync();
            return Content("Congratulations!");
             
        }
        #endregion

        #region ContactUst
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
        #endregion

        #region Removefrombasket
        public async Task<IActionResult> RemoveAtBasket(int? id)
        {
            if (id == null)
                return BadRequest();

            var product = await _dbContext.ShopOfProducts.FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
                return NotFound();

            var basketCookie = Request.Cookies["Basket"];

            List<BasketViewModel> productList;

            productList = JsonConvert.DeserializeObject<List<BasketViewModel>>(basketCookie);

            var exsistProduct = productList.FirstOrDefault(x => x.Id == id);

            if (exsistProduct.Count > 1)
            {
                exsistProduct.Count--;
            }
            else
            {
                productList.Remove(exsistProduct);
            }

            foreach (var item in productList)
            {
                item.Price *= item.Count;
            }

            var productJson = JsonConvert.SerializeObject(productList);
            if (productList.Count == 0)
            {
                Response.Cookies.Append("Basket", productJson);
                Response.Cookies.Delete("Basket");
                return RedirectToAction("Basket");
            }

            Response.Cookies.Append("Basket", productJson);

            return Json(id);
        }
        #endregion

        #region Addtobasket
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
        #endregion

        #region Basket
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
                basketViewModel.ProductName = dbProduct.ProductName;
                result.Add(basketViewModel);
            }
            var basket = JsonConvert.SerializeObject(result);
            Response.Cookies.Append("basket", basket);
            var basketView = JsonConvert.DeserializeObject<List<BasketViewModel>>(basket);
            return View(basketView);
        }
        #endregion

        #region BuyBasket
        public IActionResult BuyBasket()
        {
            return View();
        }

        [HttpPost]
        public IActionResult BuyBasket(SendMailBasket sendMailBasket)
        {


            if (!ModelState.IsValid) return View();

            try
            {
                MailMessage mail = new MailMessage();

                mail.From = new MailAddress("mollyjogger77@gmail.com");


                mail.To.Add("cefer.mammadzade@bk.ru");
 

                mail.IsBodyHtml = true;

                string content = "Name & Surname : " + sendMailBasket.Name;
                content += "<br/> Surname : " + sendMailBasket.Surname;
                content += "<br/> Address : " + sendMailBasket.Address;
                content += "<br/> Post Code : " + sendMailBasket.PostCode;
              
                mail.Body = content;





                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");


                NetworkCredential networkCredential = new NetworkCredential("mollyjogger77@gmail.com", "adminadmin123");
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = networkCredential;
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;
                smtpClient.Send(mail);

                ViewBag.Message = "Congratulations" +
                    "You received the product" +
                    "We will be contacted you and the product" +
                    "will be delivered within 3-4 days!";


                ModelState.Clear();

                var basketCookie = Request.Cookies["basket"];

                List<BasketViewModel> productList;

                productList = JsonConvert.DeserializeObject<List<BasketViewModel>>(basketCookie);

                productList.Clear();

                var productJson = JsonConvert.SerializeObject(productList);
                if (productList.Count == 0)
                {
                    Response.Cookies.Append("basket", productJson);
                    Response.Cookies.Delete("basket");
                }

                Response.Cookies.Append("basket", productJson);

            }
            catch (Exception ex)
            {

                ViewBag.Message = ex.Message.ToString();
            }
            return View();
        }
        #endregion
    }
}
