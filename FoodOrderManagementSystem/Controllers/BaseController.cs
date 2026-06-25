using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;

namespace FoodOrderManagementSystem.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userEmail = context.HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
            }
            
            base.OnActionExecuting(context);
        }
    }
}
