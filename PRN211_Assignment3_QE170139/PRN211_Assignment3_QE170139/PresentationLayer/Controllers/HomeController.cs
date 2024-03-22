using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;
using System.Diagnostics;

namespace PresentationLayer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;           
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            // Truy cập HttpContext từ IHttpContextAccessor
            var httpContext = _httpContextAccessor.HttpContext;
            // Kiểm tra xem phiên đăng nhập có tồn tại hay không
            if (HttpContext.Session.GetString("UserRole") == null)
            {
                // Nếu không, chuyển hướng người dùng đến trang đăng nhập
                return RedirectToAction("Index", "Login");
            }
            else
            {
                // Nếu có, hiển thị trang chính
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
}
