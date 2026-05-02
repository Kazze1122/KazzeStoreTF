using KazzeStore.Models;
using KazzeStore.Repositories;
using Microsoft.AspNetCore.Mvc;
using KazzeStore.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json; 

namespace KazzeStore.Controllers
{
    public class CartController : Controller
    {
        
            private readonly IProductRepository _productRepository;
            private readonly ApplicationDbContext _context; 

            public CartController(IProductRepository productRepository, ApplicationDbContext context)
            {
                _productRepository = productRepository;
                _context = context;
            }
            public IActionResult Index()
        {
            var cart = GetCart();
            return View(cart);
        }

        // Agregar al carrito
        public async Task<IActionResult> AddToCart(int id, string talla = "")
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null || product.Stock <= 0)
            {
                return BadRequest("Sin stock disponible");
            }

            var cart = GetCart();

            cart.AddItem(new CartItem
            {
                ProductId = product.Id,
                Nombre = product.Nombre,
                Precio = product.Precio,
                ImagenUrl = product.ImagenUrl,
                Talla = talla,
                Cantidad = 1
            });
            product.Stock -= 1;
            await _productRepository.UpdateAsync(product);

            SaveCart(cart);

            return Json(new { remainingStock = product.Stock });
        }

        // Eliminar del carrito
        public async Task<IActionResult> Remove(int id)
        {
            var cart = GetCart();
            var itemToRemove = cart.Items.FirstOrDefault(i => i.ProductId == id);

            if (itemToRemove != null)
            {
                var product = await _productRepository.GetByIdAsync(id);
                if (product != null)
                {
                    product.Stock += itemToRemove.Cantidad;
                    await _productRepository.UpdateAsync(product);
                }

                cart.RemoveItem(id);
                SaveCart(cart);
            }

            return RedirectToAction("Index");
        }
        public IActionResult Clear()
        {
            SaveCart(new ShoppingCart());
            return RedirectToAction("Index");
        }

        private ShoppingCart GetCart()
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            return string.IsNullOrEmpty(cartJson)
                ? new ShoppingCart()
                : JsonConvert.DeserializeObject<ShoppingCart>(cartJson) ?? new ShoppingCart();
        }

        private void SaveCart(ShoppingCart cart)
        {
            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
        }
        public IActionResult GetCount()
        {
            var cart = GetCart();
            return Json(cart.Items.Count);
        }

        public IActionResult Checkout()
        {
            var cart = GetCart();
            if (cart.Items.Count == 0)
            {
                return RedirectToAction("Index");
            }

            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmPurchase()
        {
            var cart = GetCart();
            if (cart.Items.Count == 0)
            {
                return BadRequest();
            }

            var userEmail = User.Identity.Name ?? "unknown";
            var userFullName = User.FindFirst("NombreCompleto")?.Value ?? userEmail;
            var detalles = string.Join(" | ", cart.Items.Select(i => $"{i.Nombre} ({i.Talla}) x{i.Cantidad}"));

            var order = new Order
            {
                UserId = userEmail,     
                UserName = userFullName, 
                Total = cart.Total,
                Detalles = detalles
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            SaveCart(new ShoppingCart());

            return RedirectToAction("ThankYou", "Cart");
        }

        public IActionResult ThankYou()
        {
            return View();
        }

    }
 }