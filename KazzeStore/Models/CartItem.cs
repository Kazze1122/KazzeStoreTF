namespace KazzeStore.Models
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public string ImagenUrl { get; set; } = string.Empty;

        public string Talla { get; set; } = string.Empty;   
        public int Cantidad { get; set; } = 1;
    }
}