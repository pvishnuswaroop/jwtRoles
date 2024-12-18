
using QuickServeAPP.DTOs;
using QuickServeAPP.Models;
using System.Threading.Tasks;

namespace QuickServeAPP.Services
{
    public interface IPaymentService
    {
        Task<PaymentDto> ProcessPaymentAsync(ProcessPaymentDto paymentDto);
        Task<PaymentDto> GetPaymentByOrderIdAsync(int orderId);
        Task<IEnumerable<PaymentDto>> GetPaymentsByUserIdAsync(int userId);

        Task<IEnumerable<PaymentDto>> GetPaymentsAndPendingOrdersByUserIdAsync(int userId);
    }
}

