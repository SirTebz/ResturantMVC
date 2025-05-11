using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShisaResturant.Data;
using ShisaResturant.Models;

namespace ShisaResturant.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private Repository<Product> _products;
        private Repository<Order> _orders;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _products = new Repository<Product>(context);
            _orders = new Repository<Order>(context);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            //ViewBag.Products = await _products.GetAllAsync();

            var model = HttpContext.Session.Get<OrderViewModel>("OrderViewModel") ?? new OrderViewModel
            {
                OrderItems = new List<OrderItemViewModel>(),
                Products = await _products.GetAllSync()
            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddItem(int prodId, int prodQty)
        {
            var product = await _context.Products.FindAsync(prodId);
            if (product == null)
            {
                return NotFound();
            }

            var model = HttpContext.Session.Get<OrderViewModel>("OrderViewModel") ?? new OrderViewModel
            {
                OrderItems = new List<OrderItemViewModel>(),
                Products = await _products.GetAllSync()
            };

            var existingItem = model.OrderItems.FirstOrDefault(o => o.ProductId == prodId);

            if (existingItem != null)
            {
                existingItem.Quantity += prodQty;
            }
            else
            {
                model.OrderItems.Add(new OrderItemViewModel
                {
                    ProductId = product.ProductId,
                    Price = product.Price,
                    Quantity = prodQty,
                    ProductName = product.Name

                });
            }

            model.TotalAmount = model.OrderItems.Sum(o => o.Price * o.Quantity);

            HttpContext.Session.Set("OrderViewModel", model);

            return RedirectToAction("Create", model);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Cart()
        {
            var model = HttpContext.Session.Get<OrderViewModel>("OrderViewModel");

            if (model == null || model.OrderItems.Count == 0)
            {
                return RedirectToAction("Create");
            }
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PlaceOrder()
        {
            var model = HttpContext.Session.Get<OrderViewModel>("OrderViewModel");
            if (model == null || model.OrderItems.Count == 0)
            {
                return RedirectToAction("Create");
            }

            Order order = new Order
            {
                OrderDate = DateTime.Now,
                TotalAmount = model.TotalAmount,
                UserId = _userManager.GetUserId(User)
            };

            foreach (var item in model.OrderItems)
            {
                order.OrderItems.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price
                });
            }

            await _orders.AddSync(order);

            HttpContext.Session.Remove("OrderViewModel");

            return RedirectToAction("ViewOrders");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ViewOrders()
        {
            var userId = _userManager.GetUserId(User);

            var userOrders = await _orders.GetAllByIdSync(userId, "UserId", new QueryOptions<Order>
            {
                Includes = "OrderItems.Product"
            });

            return View(userOrders);
        }
    }
}
