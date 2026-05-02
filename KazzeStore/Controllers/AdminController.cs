using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using KazzeStore.Models;
using KazzeStore.Repositories;
using KazzeStore.Data;
using Microsoft.EntityFrameworkCore;

namespace KazzeStore.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ApplicationDbContext _context;

        public AdminController(IProductRepository productRepository, ApplicationDbContext context)
        {
            _productRepository = productRepository;
            _context = context;
        }

        // ==================== LISTA DE PRODUCTOS CON FILTROS ====================
        public async Task<IActionResult> Products(string color = "", string stockStatus = "", string talla = "", string category = "")
        {
            var products = await _productRepository.GetAllAsync();

            if (!string.IsNullOrEmpty(category))
                products = products.Where(p => p.CategoryId.ToString() == category).ToList();

            if (!string.IsNullOrEmpty(color))
                products = products.Where(p => p.Color.ToLower() == color.ToLower()).ToList();

            if (!string.IsNullOrEmpty(stockStatus))
            {
                products = stockStatus switch
                {
                    "Agotado" => products.Where(p => p.Stock == 0).ToList(),
                    "Reponer" => products.Where(p => p.Stock >= 1 && p.Stock <= 5).ToList(),
                    "Optimo" => products.Where(p => p.Stock >= 6 && p.Stock <= 30).ToList(),
                    "Excedente" => products.Where(p => p.Stock > 30).ToList(),
                    _ => products
                };
            }

            if (!string.IsNullOrEmpty(talla))
                products = products.Where(p => p.Talla.ToUpper() == talla.ToUpper()).ToList();

            products = products.OrderByDescending(p => p.Id).ToList();

            ViewBag.SelectedColor = color;
            ViewBag.SelectedStock = stockStatus;
            ViewBag.SelectedTalla = talla;
            ViewBag.SelectedCategory = category;

            return View(products);
        }

        // ==================== CREAR ====================
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product, IFormFile? ImagenArchivo)
        {
            if (ModelState.IsValid)
            {
                if (ImagenArchivo != null && ImagenArchivo.Length > 0)
                {
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "polos");
                    Directory.CreateDirectory(uploadsFolder);

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + ImagenArchivo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImagenArchivo.CopyToAsync(fileStream);
                    }

                    product.ImagenUrl = "/images/polos/" + uniqueFileName;
                }

                var tallasSeleccionadas = Request.Form["Tallas"];
                product.Talla = string.Join(",", tallasSeleccionadas);

                await _productRepository.AddAsync(product);
                return RedirectToAction("Products");
            }

            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View(product);
        }

        // ==================== EDITAR ====================
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return NotFound();

            ViewBag.Categories = await _context.Categories.ToListAsync();

            ViewBag.SelectedTallas = product.Talla?.Split(',').Select(t => t.Trim()).ToList() ?? new List<string>();

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product, IFormFile? ImagenArchivo)
        {
            if (ModelState.IsValid)
            {
                var productoOriginal = await _productRepository.GetByIdAsync(product.Id);
                if (productoOriginal == null) return NotFound();

                productoOriginal.Nombre = product.Nombre;
                productoOriginal.Descripcion = product.Descripcion;
                productoOriginal.Precio = product.Precio;
                productoOriginal.Color = product.Color;
                productoOriginal.Stock = product.Stock;
                productoOriginal.CategoryId = product.CategoryId;

                var tallasSeleccionadas = Request.Form["Tallas"];
                productoOriginal.Talla = string.Join(",", tallasSeleccionadas);

                if (ImagenArchivo != null && ImagenArchivo.Length > 0)
                {
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "polos");
                    Directory.CreateDirectory(uploadsFolder);

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + ImagenArchivo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImagenArchivo.CopyToAsync(fileStream);
                    }

                    productoOriginal.ImagenUrl = "/images/polos/" + uniqueFileName;
                }

                await _productRepository.UpdateAsync(productoOriginal);
                return RedirectToAction("Products");
            }

            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View(product);
        }

        // ==================== ELIMINAR ====================
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productRepository.DeleteAsync(id);
            return RedirectToAction("Products");
        }
    }
}