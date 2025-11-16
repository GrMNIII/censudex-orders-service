using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Models;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly OrderServiceDbContext _context;

    // El framework inyecta automáticamente el DbContext gracias a Program.cs
    public OrdersController(OrderServiceDbContext context)
    {
        _context = context;
    }

    // --- 1. CONSULTAR ESTADO/HISTORIAL DE PEDIDOS (GET) ---
    // GET /api/orders?userId=5&startDate=...
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> GetOrders(
        [FromQuery] int? orderId, 
        [FromQuery] int? userId,
        [FromQuery] DateTime? startDate, 
        [FromQuery] DateTime? endDate)
    {
        IQueryable<Order> query = _context.Orders.Include(o => o.OrderItems); // Incluir items

        // Aplicar filtros dinámicamente
        if (orderId.HasValue) query = query.Where(o => o.OrderId == orderId.Value);
        if (userId.HasValue) query = query.Where(o => o.UserId == userId.Value);
        if (startDate.HasValue) query = query.Where(o => o.OrderDate >= startDate.Value);
        if (endDate.HasValue) query = query.Where(o => o.OrderDate <= endDate.Value);

        return await query.ToListAsync();
    }
    
    // --- 2. CONSULTAR ESTADO DE PEDIDO POR ID (GET) ---
    // GET /api/orders/123
    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetOrder(int id)
    {
        var order = await _context.Orders
            .Include(o => o.OrderItems) // Cargar los ítems relacionados
            .FirstOrDefaultAsync(o => o.OrderId == id);

        if (order == null)
        {
            return NotFound($"Pedido con ID {id} no encontrado.");
        }

        return order;
    }

    // --- 3. CREAR PEDIDO (POST) ---
    // POST /api/orders
    [HttpPost]
    public async Task<ActionResult<Order>> CreateOrder(Order order)
    {
        // NOTA DE LÓGICA: Aquí deberías validar stock y precio, y obtener el ID del usuario
        
        order.OrderDate = DateTime.Now; 
        order.CurrentStatus = "PENDIENTE";
        
        _context.Orders.Add(order);
        // La inserción de OrderItems se hace automáticamente si OrderItems tiene datos
        await _context.SaveChangesAsync();
        

        return CreatedAtAction(nameof(GetOrder), new { id = order.OrderId }, order);
    }
    
    // --- 4. ACTUALIZAR ESTADO DE PEDIDO (PUT) ---
    // PUT /api/orders/123/status
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] string newStatus)
    {
        var order = await _context.Orders.FindAsync(id);

        if (order == null) return NotFound();

        // Lógica para asegurar que el nuevo estado es válido y que el trigger se dispara (o se registra el historial)
        order.CurrentStatus = newStatus.ToUpper(); 
        
        await _context.SaveChangesAsync();
        

        return NoContent(); // 204 No Content, indicando éxito sin cuerpo de respuesta
    }
    
    // --- 5. CANCELAR PEDIDO (DELETE, aunque se suele usar PUT/status) ---
    // DELETE /api/orders/123
    [HttpDelete("{id}")]
    public async Task<IActionResult> CancelOrder(int id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null) return NotFound();

        // Lógica de cancelación: verifica plazo, actualiza el estado, registra el motivo
        order.CurrentStatus = "CANCELADO";
        order.CancellationReason = "Cancelado por el cliente"; 

        await _context.SaveChangesAsync();
        
        
        return NoContent();
    }
}