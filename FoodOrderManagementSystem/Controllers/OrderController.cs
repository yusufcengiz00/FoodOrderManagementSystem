using Dapper;
using FoodOrderManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace FoodOrderManagementSystem.Controllers
{
    public class OrderController : BaseController
    {
        public IActionResult List()
        {
            ViewBag.Users = Context.Listeleme<User>("sp_UserGetAll")
                .ToDictionary(u => u.UserId, u => u.FullName);
            return View(Context.Listeleme<Order>("sp_OrderGetAll"));
        }

        public IActionResult Edit(int id = 0)
        {
            Order order;
            if (id == 0)
            {
                order = new Order();
            }
            else
            {
                order = Context.Listeleme<Order>("sp_OrderGetAll")
                    .FirstOrDefault(o => o.OrderId == id);

                if (order == null)
                    return RedirectToAction("List");
            }

            ViewBag.Users = Context.Listeleme<User>("sp_UserGetAll");
            ViewBag.Products = Context.Listeleme<Product>("sp_ProductGetAll");

            if (order.OrderId > 0)
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@OrderId", order.OrderId);
                ViewBag.Details = Context.Listeleme<OrderDetail>("sp_OrderDetailGetByOrderId", param);
            }
            else
            {
                ViewBag.Details = Enumerable.Empty<OrderDetail>();
            }

            return View(order);
        }

        [HttpPost]
        public IActionResult Save(Order order)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@OrderId", order.OrderId);
            param.Add("@UserId", order.UserId);

            if (order.OrderId == 0)
            {
                int newOrderId = Context.ExecuteScalar<int>("sp_OrderKaydet", param);
                TempData["Success"] = "Sipariş oluşturuldu. Ürün ekleyebilirsiniz.";
                return RedirectToAction("Edit", new { id = newOrderId });
            }

            Context.Execute("sp_OrderKaydet", param);
            TempData["Success"] = "Sipariş güncellendi.";
            return RedirectToAction("Edit", new { id = order.OrderId });
        }

        [HttpPost]
        public IActionResult AddDetail(int orderId, int productId, int quantity)
        {
            if (orderId <= 0 || productId <= 0 || quantity <= 0)
            {
                TempData["Error"] = "Geçersiz sipariş, ürün veya adet bilgisi.";
                return RedirectToAction("Edit", new { id = orderId });
            }

            DynamicParameters productParam = new DynamicParameters();
            productParam.Add("@ProductId", productId);
            var product = Context.Listeleme<Product>("sp_ProductGetById", productParam).FirstOrDefault();

            if (product == null)
            {
                TempData["Error"] = "Seçilen ürün bulunamadı.";
                return RedirectToAction("Edit", new { id = orderId });
            }

            DynamicParameters param = new DynamicParameters();
            param.Add("@OrderId", orderId);
            param.Add("@ProductId", productId);
            param.Add("@Quantity", quantity);
            param.Add("@Price", product.Price);

            Context.Execute("sp_OrderDetailKaydet", param);
            TempData["Success"] = "Ürün siparişe eklendi.";
            return RedirectToAction("Edit", new { id = orderId });
        }

        public IActionResult RemoveDetail(int id, int orderId)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@OrderDetailId", id);
            Context.Execute("sp_OrderDetailDelete", param);
            TempData["Success"] = "Ürün siparişten çıkarıldı.";
            return RedirectToAction("Edit", new { id = orderId });
        }

        public IActionResult Delete(int id)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@OrderId", id);
            Context.Execute("sp_OrderDelete", param);
            TempData["Success"] = "Sipariş silindi.";
            return RedirectToAction("List");
        }
    }
}
