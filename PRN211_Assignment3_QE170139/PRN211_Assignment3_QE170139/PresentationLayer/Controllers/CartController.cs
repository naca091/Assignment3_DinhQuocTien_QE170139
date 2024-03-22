using BusinessObject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    public class CartController : Controller
    {
        private readonly SalesManagementContext _context;

        public CartController(SalesManagementContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Lấy danh sách sản phẩm từ giỏ hàng trong session
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            return View(cart);
        }
        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            // Find and remove the item from the cart based on productId
            var itemToRemove = cart.FirstOrDefault(item => item.ProductId == productId);
            if (itemToRemove != null)
            {
                cart.Remove(itemToRemove);
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }

            // Redirect back to the cart page
            return RedirectToAction("Index");
        }
    }

}
