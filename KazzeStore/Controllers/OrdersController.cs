using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using KazzeStore.Models;
using KazzeStore.Data;
using Microsoft.EntityFrameworkCore;

namespace KazzeStore.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> MyOrders()
        {
            var userEmail = User.Identity.Name;
            var orders = await _context.GetUserOrdersWithDetailsAsync(userEmail);
            return View(orders);
        }

        // Todas las compras (Admin)
            public async Task<IActionResult> AllOrders()
            {
                var orders = await _context.Orders
                    .OrderByDescending(o => o.Fecha)
                    .ToListAsync();

                return View(orders);
            }

        public async Task<IActionResult> SearchSales(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return RedirectToAction("AllOrders");
            }

            var orders = await _context.GetSalesByUserNameAsync(userName);
            ViewBag.SearchTerm = userName;
            return View("AllOrders", orders);
        }
    }
}