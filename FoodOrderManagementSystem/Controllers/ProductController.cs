using Dapper;
using FoodOrderManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

using FoodOrderManagementSystem.Controllers;

namespace dapperProject.Controllers
{
    public class ProductController : BaseController
    {
        public IActionResult List()
        {
            return View(Context.Listeleme<Product>("sp_ProductGetAll"));
        }

        public IActionResult Edit(int id = 0)
        {
            if (id == 0) return View(new Product());

            DynamicParameters param = new DynamicParameters();
            param.Add("@ProductId", id);
            var product = Context.Listeleme<Product>("sp_ProductGetById", param).FirstOrDefault();
            return View(product);
        }

        [HttpPost]
        public IActionResult Save(Product product)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@ProductId", product.ProductId);
            param.Add("@ProductName", product.ProductName);
            param.Add("@Price", product.Price);

            Context.Execute("sp_ProductKaydet", param);
            return RedirectToAction("List");
        }

        public IActionResult Delete(int id)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@ProductId", id);
            Context.Execute("sp_ProductDelete", param);
            return RedirectToAction("List");
        }
    }
}