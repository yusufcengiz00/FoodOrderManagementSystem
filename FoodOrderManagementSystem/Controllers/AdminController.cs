using Dapper;
using FoodOrderManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace FoodOrderManagementSystem.Controllers
{
    public class AdminController : BaseController
    {
        public IActionResult Index()
        {
            // İstatistikleri çekiyoruz
            var orders = Context.Listeleme<Order>("sp_OrderGetAll").ToList();
            var users = Context.Listeleme<User>("sp_UserGetAll").ToList();
            var products = Context.Listeleme<Product>("sp_ProductGetAll").ToList();

            int totalOrders = orders.Count;
            decimal totalRevenue = orders.Sum(o => o.TotalAmount);
            int totalUsers = users.Count;
            int totalProducts = products.Count;

            // Görünüm için ViewBag'e atıyoruz
            ViewBag.TotalOrders = totalOrders;
            ViewBag.TotalRevenue = totalRevenue;
            ViewBag.TotalUsers = totalUsers;
            ViewBag.TotalProducts = totalProducts;

            // Son 5 Sipariş
            var recentOrders = orders.Take(5).ToList();
            ViewBag.RecentOrders = recentOrders;
            ViewBag.UserDict = users.ToDictionary(u => u.UserId, u => u.FullName);

            return View();
        }
    }
}
