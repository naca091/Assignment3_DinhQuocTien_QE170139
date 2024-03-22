using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.Models;
using DataAccess.Repository;
using eStore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace PresentationLayer.Controllers
{
    public class UserOrdersController : Controller
    {
        private readonly SalesManagementContext _context;

        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailsRepository _orderDetailsRepository;

        public UserOrdersController(SalesManagementContext context,IOrderRepository orderRepository , IOrderDetailsRepository orderDetailsRepository)
        {
            _context = context;
            _orderRepository = orderRepository;
            _orderDetailsRepository = orderDetailsRepository;
        }

        // GET: UserOrders
        // GET: UserOrders
        public async Task<IActionResult> Index()
        {
            // Lấy ID của người dùng hiện tại từ session
            int userId = GetCurrentUserId();

            // Lấy các đơn hàng của người dùng hiện tại
            var orders = await _context.Orders
                .Include(o => o.Member)
                .Where(o => o.MemberId == userId)
                .ToListAsync();

            // Sử dụng dictionary để lưu tổng giá trị của mỗi đơn hàng
            Dictionary<int, decimal> orderTotalPrices = new Dictionary<int, decimal>();

            // Tính tổng giá trị của mỗi đơn hàng
            foreach (var order in orders)
            {
                decimal totalPrice = _context.OrderDetails
                    .Where(od => od.OrderId == order.OrderId)
                    .Sum(od => od.UnitPrice * od.Quantity);

                orderTotalPrices.Add(order.OrderId, totalPrice);
            }

            // Truyền tổng giá trị vào view thông qua ViewBag
            ViewBag.OrderTotalPrices = orderTotalPrices;

            return View(orders);
        }



        // GET: UserOrders/Create
        public async Task<IActionResult> Create()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            if (cart.Count == 0)
            {
                return RedirectToAction("Index", "UserProduct"); // Redirect to product page if cart is empty
            }

            // Create a new Order entity
            var order = new Order
            {
                OrderDate = DateTime.Now,
                RequiredDate = DateTime.Now.AddDays(7), // For example, require delivery within 7 days
                MemberId = GetCurrentUserId() // Get the member ID from session
            };

            // Add the Order entity to the context but don't save changes yet
            _context.Add(order);
            await _context.SaveChangesAsync(); // Save changes to generate OrderId

            // Now that the OrderId has been generated, create OrderDetail entities
            foreach (var item in cart)
            {
                _context.Add(new OrderDetail
                {
                    OrderId = order.OrderId, // Set the OrderId to the generated OrderId
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Price // Use the price from the cart item
                });
            }

            // Save changes to add OrderDetail entities to the database
            await _context.SaveChangesAsync();

            // Remove the cart from session
            HttpContext.Session.Remove("Cart");

            // Redirect to the cart page
            return RedirectToAction("Index", "Cart");
        }

        // GET: UserOrders/CreateOrder
        public IActionResult CreateOrder()
        {
            var order = HttpContext.Session.GetObjectFromJson<Order>("Order");
            ViewData["MemberId"] = new SelectList(_context.Members, "MemberId", "City", order.MemberId);
            return View(order);
        }

        // POST: UserOrders/CreateOrder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrder(Order order)
        {
            if (ModelState.IsValid)
            {
                // Lưu đơn hàng vào cơ sở dữ liệu và xóa giỏ hàng từ session
                _context.Add(order);
                await _context.SaveChangesAsync();
                HttpContext.Session.Remove("Cart");
                return RedirectToAction(nameof(Index));
            }
            ViewData["MemberId"] = new SelectList(_context.Members, "MemberId", "City", order.MemberId);
            return View(order);
        }

        // Other methods like Edit, Details, Delete are omitted for brevity

        // Helper method to get the current user's ID from session
        private int GetCurrentUserId()
        {
            // Here you need to implement logic to get the current user's ID from session
            // For the sake of simplicity, let's assume you have a session key named "UserId" storing the user's ID
            //return try parse
            string id = HttpContext.Session.GetString("UserId");
            return int.TryParse(id, out int result) ? result : 0;
        }

        //details page
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Member)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            // Lấy thông tin chi tiết đơn hàng từ bảng OrderDetails
            var orderDetails = await _context.OrderDetails
                .Where(od => od.OrderId == id)
                .ToListAsync();


            // Tạo danh sách để lưu thông tin chi tiết sản phẩm
            var productDetails = new List<ProductDetail>();

            // Lấy thông tin chi tiết sản phẩm từ bảng Product dựa trên ProductId
            foreach (var detail in orderDetails)
            {
                var product = await _context.Products
                    .FirstOrDefaultAsync(p => p.ProductId == detail.ProductId);

                if (product != null)
                {
                    var productDetail = new ProductDetail
                    {
                        ProductName = product.ProductName,
                        Quantity = detail.Quantity,
                        UnitPrice = detail.UnitPrice
                    };

                    productDetails.Add(productDetail);
                }
            }
            // Tạo một ViewModel để chứa cả thông tin về đơn hàng và danh sách chi tiết đơn hàng
            var orderViewModel = new OrderViewModel
            {
                Order = order,
                ProductDetails = productDetails
            };



            return View(orderViewModel);
        }
    }
}
