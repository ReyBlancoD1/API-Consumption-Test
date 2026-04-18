using Microsoft.AspNetCore.Mvc;

namespace ProductManagement.Web.Controllers;

public class HomeController : Controller
{
    [HttpGet("/")]
    public IActionResult Index() => View();
}