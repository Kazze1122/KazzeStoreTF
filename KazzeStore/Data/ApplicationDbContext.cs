using KazzeStore.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;


namespace KazzeStore.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .Property(p => p.Precio)
                .HasColumnType("decimal(18,2)");
        }

        public async Task<List<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            return await Products
                .FromSqlRaw("EXEC sp_ObtenerProductosPorCategoria @CategoryId",
                    new SqlParameter("@CategoryId", categoryId))
                .ToListAsync();
        }

        public async Task<List<Product>> SearchProductsAsync(string searchTerm)
        {
            return await Products
                .FromSqlRaw("EXEC sp_BuscarProductos @SearchTerm",
                    new SqlParameter("@SearchTerm", searchTerm))
                .ToListAsync();
        }

        public async Task<List<Order>> GetUserOrdersWithDetailsAsync(string userId)
        {
            return await Orders
                .FromSqlRaw("EXEC sp_ObtenerPedidosUsuarioConDetalles @UserId",
                    new SqlParameter("@UserId", userId))
                .ToListAsync();
        }

        public async Task<List<Order>> GetSalesByUserNameAsync(string userName)
        {
            return await Orders
                .FromSqlRaw("EXEC sp_ObtenerVentasPorUsuario @UserName",
                    new SqlParameter("@UserName", userName))
                .ToListAsync();
        }


    }
}
