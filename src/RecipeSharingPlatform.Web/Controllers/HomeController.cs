using Microsoft.AspNetCore.Mvc;
using RecipeSharingPlatform.ViewModels;
using System.Diagnostics;
using RecipeSharingPlatform.Web.Controllers;

public class HomeController : BaseController
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        if (this.User.Identity.IsAuthenticated)
        {
            var userId = this.GetUserId();
            _logger.LogInformation($"User {userId} accessed the home page.");
            return RedirectToAction("Index", "Recipe");
        }
        else
        {
            _logger.LogInformation("Anonymous user accessed the home page.");
            return View();
        }
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