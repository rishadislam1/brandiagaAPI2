using brandiagaAPI2.Data.Models;
using brandiagaAPI2.Dtos;
using brandiagaAPI2.Interfaces.RepositoryInterfaces;
using brandiagaAPI2.Interfaces.ServiceInterfaces;

namespace brandiagaAPI2.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IEmailService _emailService;

        public OrderService(
            IOrderRepository orderRepository,
            IUserRepository userRepository,
            IProductRepository productRepository,
            IInventoryRepository inventoryRepository,
            IEmailService emailService)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _productRepository = productRepository;
            _inventoryRepository = inventoryRepository;
            _emailService = emailService;
        }



        public async Task<OrderResponseDto> CreateOrderAsync(OrderCreateDto orderCreateDto)
        {
            // Validate user
            var user = await _userRepository.GetUserByIdAsync(orderCreateDto.UserId);
            if (user == null || !user.IsActive)
            {
                throw new Exception("User not found or inactive");
            }

            // Validate order items and calculate total amount
            decimal totalAmount = 0;
            var orderItems = new List<OrderItem>();
            foreach (var itemDto in orderCreateDto.OrderItems)
            {
                var product = await _productRepository.GetProductByIdAsync(itemDto.ProductId);
                if (product == null)
                {
                    throw new Exception($"Product with ID {itemDto.ProductId} not found");
                }

                // Check inventory
                var inventory = await _inventoryRepository.GetInventoryByProductIdAsync(itemDto.ProductId);
                if (inventory == null || inventory.Quantity < itemDto.Quantity)
                {
                    throw new Exception($"Insufficient stock for product: {product.Name}");
                }

                // Use discounted price if available, otherwise regular price
                var unitPrice = product.DiscountPrice ?? product.Price;
                totalAmount += unitPrice * itemDto.Quantity;

                orderItems.Add(new OrderItem
                {
                    OrderItemId = Guid.NewGuid(),
                    ProductId = itemDto.ProductId,
                    Quantity = itemDto.Quantity,
                    UnitPrice = unitPrice,
                   
                });

                // Update inventory
                inventory.Quantity -= itemDto.Quantity;
                await _inventoryRepository.UpdateInventoryAsync(inventory);
            }

            // Create order
            var order = new Order
            {
                OrderId = Guid.NewGuid(),
                UserId = orderCreateDto.UserId,
                TotalAmount = totalAmount,
                Status = "Pending",
                OrderDate = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                OrderItems = orderItems
            };

            await _orderRepository.AddOrderAsync(order);

            var orderResponse = new OrderResponseDto
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                UserEmail = user.Email,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                OrderDate = order.OrderDate,
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt,
                OrderItems = orderItems.Select(oi => new OrderItemResponseDto
                {
                    OrderItemId = oi.OrderItemId,
                    ProductId = oi.ProductId,
                    ProductName = oi.Product?.Name ?? "Product",
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }).ToList()
            };

            // Send email
            string subject = "Your Order Confirmation - Brandiaga";
            string emailBody = $@"
<html>
  <head>
    <style>
      body {{
        font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
        background-color: #f9f9f9;
        color: #333;
        margin: 0;
        padding: 20px;
      }}
      .container {{
        background-color: #fff;
        padding: 30px;
        max-width: 600px;
        margin: auto;
        border-radius: 8px;
        box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05);
      }}
      h2 {{
        color: #2e6c80;
        margin-bottom: 10px;
      }}
      p {{
        margin: 5px 0;
        font-size: 15px;
      }}
      table {{
        width: 100%;
        border-collapse: collapse;
        margin-top: 20px;
      }}
      th, td {{
        border: 1px solid #e0e0e0;
        padding: 12px;
        text-align: left;
        font-size: 14px;
      }}
      th {{
        background-color: #f4f6f8;
        color: #555;
      }}
      .total {{
        font-size: 18px;
        margin-top: 20px;
        text-align: right;
        color: #2e6c80;
      }}
      .footer {{
        margin-top: 40px;
        font-size: 13px;
        color: #888;
        text-align: center;
      }}
    </style>
  </head>
  <body>
    <div class='container'>
      <h2>Thank you for your order, {user.FirstName}!</h2>
      <p>We appreciate your purchase. Below is your order summary:</p>

      <table>
        <thead>
          <tr>
            <th>Product</th>
            <th>Quantity</th>
            <th>Unit Price</th>
            <th>Subtotal</th>
          </tr>
        </thead>
        <tbody>
          {string.Join("", orderItems.Select(item =>
            {
                var product = item.Product ?? new Product();
                decimal itemTotal = item.Quantity * item.UnitPrice;
                return $@"
              <tr>
                <td>{product.Name}</td>
                <td>{item.Quantity}</td>
                <td>{item.UnitPrice:C}</td>
                <td>{itemTotal:C}</td>
              </tr>";
            }))}
        </tbody>
      </table>

      <div class='total'>
        Total Amount: <strong>{totalAmount:C}</strong><br/>
        Status: <strong>{order.Status}</strong><br/>
        Order Date: <strong>{order.OrderDate:dddd, dd MMMM yyyy}</strong>
      </div>

      <p style='margin-top: 30px;'>We'll notify you as soon as your items are processed and shipped.</p>

      <p>Thanks again,<br/><strong>The Brandiaga Team</strong></p>

      <div class='footer'>
        &copy; {DateTime.UtcNow.Year} Brandiaga. All rights reserved.
      </div>
    </div>
  </body>
</html>";
            

            await _emailService.SendEmailAsync(user.Email, subject, emailBody);
            return orderResponse;
        }

        public async Task<OrderResponseDto> GetOrderByIdAsync(Guid orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                throw new Exception("Order not found");
            }

            return new OrderResponseDto
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                UserEmail = order.User?.Email,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                OrderDate = order.OrderDate,
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt,
                OrderItems = order.OrderItems.Select(oi => new OrderItemResponseDto
                {
                    OrderItemId = oi.OrderItemId,
                    ProductId = oi.ProductId,
                    ProductName = oi.Product?.Name,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }).ToList()
            };
        }

        public async Task<IEnumerable<OrderResponseDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllOrdersAsync();

            return orders.Select(order => new OrderResponseDto
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                UserEmail = order.User?.Email,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                OrderDate = order.OrderDate,
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt,
                OrderItems = order.OrderItems.Select(oi => new OrderItemResponseDto
                {
                    OrderItemId = oi.OrderItemId,
                    ProductId = oi.ProductId,
                    ProductName = oi.Product?.Name,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }).ToList()
            }).ToList();
        }

        public async Task<IEnumerable<OrderResponseDto>> GetOrdersByUserIdAsync(Guid userId)
        {
            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);

            return orders.Select(order => new OrderResponseDto
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                UserEmail = order.User?.Email,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                OrderDate = order.OrderDate,
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt,
                OrderItems = order.OrderItems.Select(oi => new OrderItemResponseDto
                {
                    OrderItemId = oi.OrderItemId,
                    ProductId = oi.ProductId,
                    ProductName = oi.Product?.Name,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }).ToList()
            }).ToList();
        }

        public async Task<OrderResponseDto> UpdateOrderAsync(Guid orderId, OrderUpdateDto orderUpdateDto)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                throw new Exception("Order not found");
            }

            // Validate and update status
            if (!string.IsNullOrEmpty(orderUpdateDto.Status))
            {
                var validStatuses = new[] { "Pending", "Processing", "Shipped", "Delivered", "Cancelled" };
                if (!validStatuses.Contains(orderUpdateDto.Status))
                {
                    throw new Exception("Invalid status. Allowed values: Pending, Processing, Shipped, Delivered, Cancelled");
                }

                // If cancelling, restore inventory
                if (orderUpdateDto.Status == "Cancelled" && order.Status != "Cancelled")
                {
                    foreach (var item in order.OrderItems)
                    {
                        var inventory = await _inventoryRepository.GetInventoryByProductIdAsync(item.ProductId);
                        if (inventory != null)
                        {
                            inventory.Quantity += item.Quantity;
                       
                            await _inventoryRepository.UpdateInventoryAsync(inventory);
                        }
                    }
                }

                order.Status = orderUpdateDto.Status;
            }

            order.UpdatedAt = DateTime.UtcNow;

            await _orderRepository.UpdateOrderAsync(order);

            return new OrderResponseDto
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                UserEmail = order.User?.Email,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                OrderDate = order.OrderDate,
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt,
                OrderItems = order.OrderItems.Select(oi => new OrderItemResponseDto
                {
                    OrderItemId = oi.OrderItemId,
                    ProductId = oi.ProductId,
                    ProductName = oi.Product?.Name,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }).ToList()
            };
        }

        public async Task DeleteOrderAsync(Guid orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                throw new Exception("Order not found");
            }

            // Restore inventory if order is not already cancelled
            if (order.Status != "Cancelled")
            {
                foreach (var item in order.OrderItems)
                {
                    var inventory = await _inventoryRepository.GetInventoryByProductIdAsync(item.ProductId);
                    if (inventory != null)
                    {
                        inventory.Quantity += item.Quantity;
                        
                        await _inventoryRepository.UpdateInventoryAsync(inventory);
                    }
                }
            }

            await _orderRepository.DeleteOrderAsync(orderId);
        }

        public async Task AddToWishlistAsync(Guid userId, Guid productId)
        {
            var wishlist = new Wishlist
            {
                WishlistId = Guid.NewGuid(),
                UserId = userId,
                ProductId = productId,
                CreatedAt = DateTime.UtcNow
            };
            await _orderRepository.AddToWishlistAsync(wishlist);
        }

        public async Task DeleteFromWishlistAsync(Guid wishlistId)
        {
            await _orderRepository.DeleteFromWishlistAsync(wishlistId);
        }

        public async Task<List<WishlistDto>> GetAllWishlistAsync()
        {
            var wishlist = await _orderRepository.GetAllWishListAsync();

            var wishlistResponse = wishlist.Select(w => new WishlistDto
            {
                WishlistId = w.WishlistId,
                UserId = w.UserId,
                ProductId = w.ProductId,
                CreatedAt = w.CreatedAt
            }
            ).ToList();
            return wishlistResponse;
        }


        public async Task<List<WishlistDto>> GetWishlistAsync(Guid userId)
        {
            var wishlist = await _orderRepository.GetWishlistAsync(userId);

            var wishlistResponse = wishlist.Select(w => new WishlistDto
            {
                WishlistId = w.WishlistId,
                UserId = w.UserId,
                ProductId = w.ProductId,
                CreatedAt = w.CreatedAt
            }
            ).ToList();
            return wishlistResponse;
        }
    }
}
