using System.ComponentModel.DataAnnotations;

namespace KazzeStore.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Display(Name = "Nombre de Categoría")]
        public string Nombre { get; set; } = string.Empty;

        [Display(Name = "Descripción")]
        public string Descripcion { get; set; } = string.Empty;

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}