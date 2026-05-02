using KazzeStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KazzeStore.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Administrador
            const string adminEmail = "admin@kazzestore.com";
            const string adminPassword = "Prueba1@";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    NombreCompleto = "KazzeStore",
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddClaimAsync(adminUser, new Claim("NombreCompleto", "KazzeStore"));
                }
            }

            if (context.Products.Any())
            {
                return;
            }

            // Categorías
            var categories = new List<Category>
            {
                new Category { Nombre = "Anime", Descripcion = "Polos con temática anime" },
                new Category { Nombre = "Kanjis", Descripcion = "Polos con kanjis japoneses" },
                new Category { Nombre = "Minimalista", Descripcion = "Polos minimalistas y elegantes" }
            };
            context.Categories.AddRange(categories);
            await context.SaveChangesAsync();

            // Polos para inicio
            var products = new List<Product>
            {
                new Product { Nombre = "Polo Dragón Rojo", Descripcion = "Polo rojo con kanji dragón poderoso", Precio = 49.90m, ImagenUrl = "/images/polos/Polo1.jpg", Talla = "S,M,L,XL", Color = "Rojo", Kanji = "龍", Stock = 25, CategoryId = 2 },
                new Product { Nombre = "Polo Sakura Minimal", Descripcion = "Diseño minimalista con flor de cerezo", Precio = 42.90m, ImagenUrl = "/images/polos/Polo2.webp", Talla = "M,L,XL", Color = "Blanco", Kanji = "桜", Stock = 18, CategoryId = 3 },
                new Product { Nombre = "Polo Anime Hero", Descripcion = "Polo con héroe anime épico", Precio = 55.90m, ImagenUrl = "/images/polos/Polo3.jpg", Talla = "S,M,L", Color = "Negro", Kanji = "英雄", Stock = 12, CategoryId = 1 },
                new Product { Nombre = "Polo Kanji Paz", Descripcion = "Diseño con kanji de paz interior", Precio = 39.90m, ImagenUrl = "/images/polos/Polo4.jpg", Talla = "L,XL,XXL", Color = "Gris", Kanji = "平", Stock = 30, CategoryId = 2 },
                new Product { Nombre = "Polo Minimal Luna", Descripcion = "Diseño minimalista con luna", Precio = 44.90m, ImagenUrl = "/images/polos/Polo5.jpg", Talla = "S,M", Color = "Azul Oscuro", Kanji = "月", Stock = 22, CategoryId = 3 },
                new Product { Nombre = "Polo Anime Warrior", Descripcion = "Guerrero anime en acción", Precio = 52.90m, ImagenUrl = "/images/polos/Polo6.webp", Talla = "M,L,XL", Color = "Negro", Kanji = "戦", Stock = 15, CategoryId = 1 },
                new Product { Nombre = "Polo Kanji Fuerza", Descripcion = "Kanji de fuerza y determinación", Precio = 47.90m, ImagenUrl = "/images/polos/Polo7.webp", Talla = "S,L,XL", Color = "Rojo", Kanji = "力", Stock = 28, CategoryId = 2 },
                new Product { Nombre = "Polo Minimal Montaña", Descripcion = "Diseño minimalista de montaña", Precio = 41.90m, ImagenUrl = "/images/polos/PoloAnime1.jpg", Talla = "M,XL", Color = "Gris", Kanji = "山", Stock = 35, CategoryId = 3 },
                new Product { Nombre = "Polo Anime Ninja", Descripcion = "Ninja stealth anime", Precio = 58.90m, ImagenUrl = "/images/polos/PoloAnime2.jpg", Talla = "S,M,L", Color = "Negro", Kanji = "忍", Stock = 10, CategoryId = 1 },
                new Product { Nombre = "Polo Kanji Amor", Descripcion = "Kanji amor eterno", Precio = 46.90m, ImagenUrl = "/images/polos/PoloAnime3.jpg", Talla = "L,XXL", Color = "Rojo", Kanji = "愛", Stock = 20, CategoryId = 2 },
                new Product { Nombre = "Polo Minimal Bosque", Descripcion = "Bosque minimalista", Precio = 43.90m, ImagenUrl = "/images/polos/PoloAnime4.jpg", Talla = "S,XL", Color = "Verde Oscuro", Kanji = "森", Stock = 25, CategoryId = 3 },
                new Product { Nombre = "Polo Anime Samurai", Descripcion = "Samurai legendario", Precio = 54.90m, ImagenUrl = "/images/polos/PoloAnime5.jpg", Talla = "M,L", Color = "Negro", Kanji = "侍", Stock = 14, CategoryId = 1 },
                new Product { Nombre = "Polo Kanji Eternidad", Descripcion = "Kanji eternidad", Precio = 48.90m, ImagenUrl = "/images/polos/PoloAnime6.jpg", Talla = "S,M,XL", Color = "Azul Oscuro", Kanji = "永", Stock = 19, CategoryId = 2 },
                new Product { Nombre = "Polo Minimal Cielo", Descripcion = "Cielo minimalista", Precio = 40.90m, ImagenUrl = "/images/polos/PoloAnime7.jpg", Talla = "L,XXL", Color = "Blanco", Kanji = "空", Stock = 32, CategoryId = 3 },
                new Product { Nombre = "Polo Anime Demon", Descripcion = "Demonio anime", Precio = 56.90m, ImagenUrl = "/images/polos/PoloAnime8.jpg", Talla = "S,M", Color = "Rojo", Kanji = "鬼", Stock = 11, CategoryId = 1 },
                new Product { Nombre = "Polo Kanji Victoria", Descripcion = "Kanji victoria", Precio = 49.90m, ImagenUrl = "/images/polos/PoloAnime9.jpg", Talla = "XL", Color = "Gris", Kanji = "勝", Stock = 23, CategoryId = 2 },
                new Product { Nombre = "Polo Minimal Río", Descripcion = "Río minimalista", Precio = 42.90m, ImagenUrl = "/images/polos/PoloAnime10.jpg", Talla = "L", Color = "Azul Oscuro", Kanji = "川", Stock = 27, CategoryId = 3 },
                new Product { Nombre = "Polo Anime Phoenix", Descripcion = "Fénix anime", Precio = 57.90m, ImagenUrl = "/images/polos/PoloMili1.jpg", Talla = "S,XL", Color = "Rojo", Kanji = "鳳", Stock = 16, CategoryId = 1 },
                new Product { Nombre = "Polo Kanji Sabiduría", Descripcion = "Kanji sabiduría", Precio = 45.90m, ImagenUrl = "/images/polos/PoloMili2.webp", Talla = "M,L", Color = "Negro", Kanji = "智", Stock = 21, CategoryId = 2 },
                new Product { Nombre = "Polo Minimal Estrella", Descripcion = "Estrella minimalista", Precio = 41.90m, ImagenUrl = "/images/polos/PoloMili3.jpg", Talla = "S", Color = "Blanco", Kanji = "星", Stock = 33, CategoryId = 3 },
                new Product { Nombre = "Polo Anime Fox", Descripcion = "Zorro anime", Precio = 55.90m, ImagenUrl = "/images/polos/PoloMili4.jpg", Talla = "L,XL", Color = "Rojo", Kanji = "狐", Stock = 17, CategoryId = 1 },
                new Product { Nombre = "Polo Kanji Libertad", Descripcion = "Kanji libertad", Precio = 46.90m, ImagenUrl = "/images/polos/PoloMili5.webp", Talla = "M", Color = "Blanco", Kanji = "自由", Stock = 26, CategoryId = 2 },
                new Product { Nombre = "Polo Minimal Camino", Descripcion = "Camino minimalista", Precio = 43.90m, ImagenUrl = "/images/polos/PoloMili6.webp", Talla = "S,L", Color = "Gris", Kanji = "道", Stock = 31, CategoryId = 3 },
                new Product { Nombre = "Polo Anime Spirit", Descripcion = "Espíritu anime", Precio = 54.90m, ImagenUrl = "/images/polos/PoloMili7.jpg", Talla = "XL", Color = "Negro", Kanji = "霊", Stock = 15, CategoryId = 1 },
                new Product { Nombre = "Polo Kanji Sueño", Descripcion = "Kanji sueño", Precio = 48.90m, ImagenUrl = "/images/polos/PoloMili8.jpg", Talla = "S,M", Color = "Azul Oscuro", Kanji = "夢", Stock = 22, CategoryId = 2 },
                new Product { Nombre = "Polo Minimal Horizonte", Descripcion = "Horizonte minimalista", Precio = 40.90m, ImagenUrl = "/images/polos/PoloMili9.webp", Talla = "L", Color = "Verde Oscuro", Kanji = "地平", Stock = 28, CategoryId = 3 },
                new Product { Nombre = "Polo Anime Knight", Descripcion = "Caballero anime", Precio = 53.90m, ImagenUrl = "/images/polos/PoloMili10.webp", Talla = "M,XL", Color = "Gris", Kanji = "騎", Stock = 13, CategoryId = 1 }
            };

            context.Products.AddRange(products);
            await context.SaveChangesAsync();

            Console.WriteLine("✅ Productos actualizados correctamente");
        }
    }
}