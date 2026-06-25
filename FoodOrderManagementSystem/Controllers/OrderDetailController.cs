using Dapper;
using FoodOrderManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace FoodOrderManagementSystem.Controllers
{
    public class OrderDetailController : BaseController
    {
        public IActionResult List()
        {
            var users = Context.Listeleme<User>("sp_UserGetAll")
                .ToDictionary(u => u.UserId, u => u.FullName);
            ViewBag.Users = users;
            return View(Context.Listeleme<Order>("sp_OrderGetAll"));
        }

        public IActionResult Receipt(int orderId)
        {
            if (orderId <= 0)
                return RedirectToAction("List");

            var order = Context.Listeleme<Order>("sp_OrderGetAll")
                .FirstOrDefault(o => o.OrderId == orderId);

            if (order == null)
                return RedirectToAction("List");

            var user = Context.Listeleme<User>("sp_UserGetAll")
                .FirstOrDefault(u => u.UserId == order.UserId);

            DynamicParameters param = new DynamicParameters();
            param.Add("@OrderId", orderId);
            var details = Context.Listeleme<OrderDetail>("sp_OrderDetailGetByOrderId", param);

            ViewBag.Order = order;
            ViewBag.User = user;

            return View(details);
        }

        public IActionResult ExportExcel(int orderId)
        {
            if (orderId <= 0)
                return RedirectToAction("List");

            var order = Context.Listeleme<Order>("sp_OrderGetAll")
                .FirstOrDefault(o => o.OrderId == orderId);

            if (order == null)
                return RedirectToAction("List");

            var user = Context.Listeleme<User>("sp_UserGetAll")
                .FirstOrDefault(u => u.UserId == order.UserId);

            DynamicParameters param = new DynamicParameters();
            param.Add("@OrderId", orderId);
            var details = Context.Listeleme<OrderDetail>("sp_OrderDetailGetByOrderId", param).ToList();

            var csv = new System.Text.StringBuilder();
            csv.AppendLine("Siparis No;Musteri;Tarih;Toplam Tutar");
            csv.AppendLine($"{order.OrderId};{user?.FullName ?? "Bilinmeyen"};{order.OrderDate:dd.MM.yyyy HH:mm};{order.TotalAmount:N2}");
            csv.AppendLine();
            csv.AppendLine("Urun Adi;Adet;Birim Fiyat;Toplam Tutar");

            foreach (var d in details)
            {
                csv.AppendLine($"{d.ProductName};{d.Quantity};{d.Price:N2};{(d.Price * d.Quantity):N2}");
            }

            var fileBytes = System.Text.Encoding.UTF8.GetPreamble()
                .Concat(System.Text.Encoding.UTF8.GetBytes(csv.ToString()))
                .ToArray();

            return File(fileBytes, "text/csv; charset=utf-8", $"Siparis_{orderId}_Detay.csv");
        }
    }
}
