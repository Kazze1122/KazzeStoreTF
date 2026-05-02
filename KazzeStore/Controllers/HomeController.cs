using Microsoft.AspNetCore.Mvc;
using KazzeStore.Repositories;

namespace KazzeStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRepository _productRepository;

        public HomeController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IActionResult> Index()
        {
            return RedirectToAction("Index", "Products");
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}