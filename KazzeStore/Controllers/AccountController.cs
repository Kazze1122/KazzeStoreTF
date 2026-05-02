using KazzeStore.Models;
using KazzeStore.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using KazzeStore.Repositories;

namespace KazzeStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IProductRepository _productRepository;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IProductRepository productRepository)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _productRepository = productRepository;
        }

        // GET: Registro
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: Registro
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    NombreCompleto = model.NombreCompleto
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Guardar el Nombre para layout)
                    await _userManager.AddClaimAsync(user, new Claim("NombreCompleto", model.NombreCompleto));


                    TempData["SuccessMessage"] = "¡Registro exitoso! Ahora inicia sesión.";
                    return RedirectToAction("Login", "Account");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        // GET: Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Products");
                }
                ModelState.AddModelError("", "Intento de inicio de sesión inválido.");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            if (!string.IsNullOrEmpty(cartJson))
            {
                var cart = JsonConvert.DeserializeObject<ShoppingCart>(cartJson);
                if (cart != null && cart.Items.Any())
                {
                    foreach (var item in cart.Items)
                    {
                        var product = await _productRepository.GetByIdAsync(item.ProductId);
                        if (product != null)
                        {
                            product.Stock += item.Cantidad;
                            await _productRepository.UpdateAsync(product);
                        }
                    }
                }
            }

            await _signInManager.SignOutAsync();
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }
    }
}