namespace OrderService.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public string DeliveryAddress { get; set; }
        public decimal TotalAmount { get; set; }
        public string CurrentStatus { get; set; }
        public string TrackingNumber { get; set; }
        public string CancellationReason { get; set; }
        
        // Relación: Un pedido tiene muchos ítems
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}