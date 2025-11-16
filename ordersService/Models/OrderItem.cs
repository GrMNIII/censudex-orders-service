namespace OrderService.Models
{
    public class OrderItem
    {
        public int ItemId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtPurchase { get; set; }
        
        // Relación: Un ítem pertenece a un solo pedido
        public Order Order { get; set; }
    }
}