using Dapper;
using FoodOrderManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

using FoodOrderManagementSystem.Controllers;

namespace dapperProject.Controllers
{
    public class UserController : BaseController
    {
        // Listeleme
        public IActionResult List()
        {
            return View(Context.Listeleme<User>("sp_UserGetAll"));
        }

        // Ekle / Düzenle
        public IActionResult Edit(int id = 0)
        {
            if (id == 0) return View(new User());

            DynamicParameters param = new DynamicParameters();
            param.Add("@UserId", id);

            var user = Context.Listeleme<User>("sp_UserGetById", param).FirstOrDefault();
            return View(user);
        }

        // Kaydet
        [HttpPost]
        public IActionResult Save(User user)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@UserId", user.UserId);
            param.Add("@FullName", user.FullName);
            param.Add("@Email", user.Email);
            param.Add("@Password", user.Password);

            Context.Execute("sp_UserKaydet", param);
            return RedirectToAction("List");
        }

        // Sil
        public IActionResult Delete(int id)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@UserId", id);

            Context.Execute("sp_UserDelete", param);
            return RedirectToAction("List");
        }
    }
}