using System.ComponentModel.DataAnnotations;

namespace KazzeStore.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Display(Name = "Nombre del Polo")]
        public string Nombre { get; set; } = string.Empty;

        [Display(Name = "Descripción")]
        public string Descripcion { get; set; } = string.Empty;

        [Display(Name = "Precio")]
        [DataType(DataType.Currency)]
        public decimal Precio { get; set; }

        [Display(Name = "Imagen")]
        public string ImagenUrl { get; set; } = string.Empty;

        [Display(Name = "Talla")]
        public string Talla { get; set; } = string.Empty;     

        [Display(Name = "Color")]
        public string Color { get; set; } = string.Empty;

        [Display(Name = "Kanji")]
        public string Kanji { get; set; } = string.Empty;

        [Display(Name = "Stock")]
        public int Stock { get; set; } = 0;

        [Display(Name = "Categoría")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}