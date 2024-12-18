using Microsoft.EntityFrameworkCore;
using QuickServeAPP.Data;
using QuickServeAPP.DTOs;
using QuickServeAPP.Models;
using QuickServeAPP.Repository;
using System.Threading.Tasks;

namespace QuickServeAPP.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly AppDbContext _context;

        public PaymentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddPaymentAsync(Payment payment)
        {
            await _context.Payments.AddAsync(payment);
        }

        public async Task<Payment> GetPaymentByOrderIdAsync(int orderId)
        {
            return await _context.Payments
                .Include(p => p.Order) // Include related Order
                .Include(p => p.User)  // Include related User
                .FirstOrDefaultAsync(p => p.OrderID == orderId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByUserIdAsync(int userId)
        {
            return await _context.Payments
                .Include(p => p.Order) // Include related Order information if needed
                .Where(p => p.UserID == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<PaymentDto>> GetPaymentsAndPendingOrdersByUserIdAsync(int userId)
        {
            var orders = await _context.Orders
                .Where(o => o.UserID == userId)
                .ToListAsync();

            var payments = await _context.Payments
                .Where(p => p.UserID == userId)
                .ToListAsync();

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

            var unpaidOrders = orders
                .Where(o => !payments.Any(p => p.OrderID == o.OrderID))
                .Select(o => new PaymentDto
                {
                    PaymentID = 0,
                    OrderID = o.OrderID,
                    UserID = o.UserID,
                    AmountPaid = o.TotalAmount,
                    PaymentMethod = null,
                    PaymentStatus = "Not Completed",
                    PaymentDate = o.OrderDate
                }).ToList();

            return paymentDtos.Concat(unpaidOrders);  // Combine both
        }
    }
}

