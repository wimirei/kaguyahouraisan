using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class AdminOnlyAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(
        ActionExecutingContext context)
    {
        var session =
            context.HttpContext.Session;

        if (session.GetString("Admin") != "true")
        {
            context.Result =
                new RedirectToActionResult(
                    "Login",
                    "AdminMorkovka",
                    null);
        }
    }
}