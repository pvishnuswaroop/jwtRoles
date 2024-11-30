
using System.Threading.Tasks;
using QuickServeAPP.DTOs;
using QuickServeAPP.Models;
using QuickServeAPP.Repository;
using QuickServeAPP.Services;

namespace QuickServeAPP.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderRepository _orderRepository;

        public PaymentService(IPaymentRepository paymentRepository, IOrderRepository orderRepository)
        {
            _paymentRepository = paymentRepository;
            _orderRepository = orderRepository;
        }

        public async Task<PaymentDto> ProcessPaymentAsync(ProcessPaymentDto paymentDto)
        {
            // Validate order
            var order = await _orderRepository.GetOrderByIdAsync(paymentDto.OrderID);
            if (order == null)
            {
                throw new Exception("Order not found.");
            }

            if (order.TotalAmount != paymentDto.AmountPaid)
            {
                throw new Exception("Payment amount does not match the order total.");
            }

            // Create payment
            var payment = new Payment
            {
                OrderID = paymentDto.OrderID,
                UserID = paymentDto.UserID,
                AmountPaid = paymentDto.AmountPaid,
                PaymentMethod = paymentDto.PaymentMethod,
                PaymentStatus = PaymentStatusEnum.Completed,  // Assuming successful payment for now
                PaymentDate = DateTime.UtcNow
            };

            await _paymentRepository.AddPaymentAsync(payment);
            await _paymentRepository.SaveChangesAsync();

            // Map to PaymentDto
            return new PaymentDto
            {
                PaymentID = payment.PaymentID,
                OrderID = payment.OrderID,
                UserID = payment.UserID,
                AmountPaid = payment.AmountPaid,
                PaymentMethod = payment.PaymentMethod.ToString(),
                PaymentStatus = payment.PaymentStatus.ToString(),
                PaymentDate = payment.PaymentDate
            };
        }

        public async Task<PaymentDto> GetPaymentByOrderIdAsync(int orderId)
        {
            // First, check if the payment exists
            var payment = await _paymentRepository.GetPaymentByOrderIdAsync(orderId);

            // If no payment is found, return the order with "Not Completed" status
            if (payment == null)
            {
                var order = await _orderRepository.GetOrderByIdAsync(orderId);
                if (order == null)
                {
                    throw new Exception("Order not found.");
                }

                // Return order as "Not Completed"
                return new PaymentDto
                {
                    PaymentID = 0,  // No payment yet
                    OrderID = order.OrderID,
                    UserID = order.UserID,
                    AmountPaid = order.TotalAmount,
                    PaymentMethod = null,  // No payment method
                    PaymentStatus = "Not Completed",  // Custom status for unpaid orders
                    PaymentDate = order.OrderDate  // Use order date as reference
                };
            }

            // Return payment details if found
            return new PaymentDto
            {
                PaymentID = payment.PaymentID,
                OrderID = payment.OrderID,
                UserID = payment.UserID,
                AmountPaid = payment.AmountPaid,
                PaymentMethod = payment.PaymentMethod.ToString(),
                PaymentStatus = payment.PaymentStatus.ToString(),
                PaymentDate = payment.PaymentDate
            };
        }

        public async Task<IEnumerable<PaymentDto>> GetPaymentsByUserIdAsync(int userId)
        {
            var payments = await _paymentRepository.GetPaymentsByUserIdAsync(userId);

            // Map Payment entities to PaymentDto
            return payments.Select(p => new PaymentDto
            {
                PaymentID = p.PaymentID,
                OrderID = p.OrderID,
                UserID = p.UserID,
                AmountPaid = p.AmountPaid,
                PaymentMethod = p.PaymentMethod.ToString(),
                PaymentStatus = p.PaymentStatus.ToString(),
                PaymentDate = p.PaymentDate
            });
        }

        public async Task<IEnumerable<PaymentDto>> GetPaymentsAndPendingOrdersByUserIdAsync(int userId)
        {
            // Get all orders placed by the user
            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);

            // Get payments for the user
            var payments = await _paymentRepository.GetPaymentsByUserIdAsync(userId);

            // Map payments to PaymentDto
            var paymentDtos = payments.Select(p => new PaymentDto
            {
                PaymentID = p.PaymentID,
                OrderID = p.OrderID,
                UserID = p.UserID,
                AmountPaid = p.AmountPaid,
                PaymentMethod = p.PaymentMethod.ToString(),
                PaymentStatus = p.PaymentStatus.ToString(),
                PaymentDate = p.PaymentDate
            }).ToList();

            // Identify unpaid orders (orders without an associated payment)
            var unpaidOrders = orders
                .Where(o => !payments.Any(p => p.OrderID == o.OrderID))
                .Select(o => new PaymentDto
                {
                    PaymentID = 0,  // No payment exists yet
                    OrderID = o.OrderID,
                    UserID = o.UserID,
                    AmountPaid = o.TotalAmount,
                    PaymentMethod = null,  // Payment not processed yet
                    PaymentStatus = "Not Completed",  // Custom status for unpaid orders
                    PaymentDate = o.OrderDate  // Use order date as a reference
                }).ToList();

            // Combine paid and unpaid orders
            return paymentDtos.Concat(unpaidOrders);
        }

    }
}

