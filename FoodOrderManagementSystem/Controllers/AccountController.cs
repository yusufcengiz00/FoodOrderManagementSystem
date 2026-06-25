using Dapper;
using FoodOrderManagementSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace FoodOrderManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        // GET: /Account/Login
        public IActionResult Login()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("UserEmail")))
            {
                return RedirectToAction("Index", "Admin");
            }
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                TempData["Error"] = "Lütfen tüm alanları doldurun.";
                return View();
            }

            // Dapper stored procedure ile kullanıcıları listeleyip eşleştiriyoruz
            var users = Context.Listeleme<User>("sp_UserGetAll");
            var user = users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase) && u.Password == password);

            if (user != null)
            {
                HttpContext.Session.SetInt32("UserId", user.UserId);
                HttpContext.Session.SetString("UserEmail", user.Email);
                HttpContext.Session.SetString("UserFullName", user.FullName);
                return RedirectToAction("Index", "Admin");
            }

            TempData["Error"] = "E-posta veya şifre hatalı.";
            return View();
        }

        // GET: /Account/Register
        public IActionResult Register()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("UserEmail")))
            {
                return RedirectToAction("Index", "Admin");
            }
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(string fullName, string email, string password)
        {
            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                TempData["Error"] = "Lütfen tüm alanları doldurun.";
                return View();
            }

            // E-posta benzersizlik kontrolü
            var users = Context.Listeleme<User>("sp_UserGetAll");
            if (users.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
            {
                TempData["Error"] = "Bu e-posta adresi zaten kayıtlı.";
                return View();
            }

            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@UserId", 0); // Yeni kullanıcı
                param.Add("@FullName", fullName);
                param.Add("@Email", email);
                param.Add("@Password", password);

                Context.Execute("sp_UserKaydet", param);

                TempData["Success"] = "Kayıt işleminiz başarılı! Şimdi giriş yapabilirsiniz.";
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Kayıt sırasında bir hata oluştu: " + ex.Message;
                return View();
            }
        }

        // GET: /Account/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
