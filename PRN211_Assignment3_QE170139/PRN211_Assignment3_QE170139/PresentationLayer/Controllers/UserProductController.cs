using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;

namespace PresentationLayer.Controllers
{
    public class UserProductController : Controller
    {
        private readonly SalesManagementContext _context;

        public UserProductController(SalesManagementContext context)
        {
            _context = context;
        }

        // GET: UserProduct
        public async Task<IActionResult> Index()
        {
              return _context.Products != null ? 
                          View(await _context.Products.ToListAsync()) :
                          Problem("Entity set 'SalesManagementContext.Products'  is null.");
        }

        // GET: ShoppingCart/AddToCart/5
        [HttpPost]
        public async Task<IActionResult> AddToCart(int? id, int quantity)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            var existingItem = cart.FirstOrDefault(item => item.ProductId == id);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity; // Thêm số lượng vào sản phẩm đã tồn tại trong giỏ hàng
                existingItem.Price = product.UnitPrice * existingItem.Quantity;
            }
            else
            {
                cart.Add(new CartItem { ProductId = id.Value, ProductName = product.ProductName, Quantity = quantity, Price = product.UnitPrice * quantity });
            }

            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return RedirectToAction(nameof(Index));
        }

        // Helper method to get the current user's ID from session
        private int GetCurrentUserId()
        {
            // Here you need to implement logic to get the current user's ID from session
            // For the sake of simplicity, let's assume you have a session key named "UserId" storing the user's ID
            return HttpContext.Session.GetInt32("UserId") ?? 0;
        }
    }


    public static class SessionExtensions
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, System.Text.Json.JsonSerializer.Serialize(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : System.Text.Json.JsonSerializer.Deserialize<T>(value);
        }
    }

    public class CartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
