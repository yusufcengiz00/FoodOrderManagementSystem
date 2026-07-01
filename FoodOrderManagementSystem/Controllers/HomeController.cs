using FoodOrderManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FoodOrderManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var products = Context.Listeleme<Product>("sp_ProductGetAll");
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
