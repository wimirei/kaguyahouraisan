using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using thrucommunity.Data;

namespace thrucommunity.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var replays = await _context.Replays
                .OrderByDescending(r => r.SubmittedAtUtc)
                .Where(r => r.Proven)
                .Take(10)
                .ToListAsync();

            return View(replays);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Resources()
        {
            return View();
        }

        public new IActionResult NotFound()
        {
            Response.StatusCode = 404;

            return View();
        }

        public IActionResult Error()
        {
            Response.StatusCode = 500;
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
    }
}
