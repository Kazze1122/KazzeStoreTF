namespace KazzeStore.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string Detalles { get; set; } = string.Empty;
    }
}