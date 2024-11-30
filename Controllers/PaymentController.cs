using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickServeAPP.DTOs;
using QuickServeAPP.Services;

namespace QuickServeAPP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost]
        public async Task<IActionResult> ProcessPayment([FromBody] ProcessPaymentDto paymentDto)
        {
            try
            {
                var payment = await _paymentService.ProcessPaymentAsync(paymentDto);
                return Ok(payment);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetPaymentByOrderId(int orderId)
        {
            try
            {
                var payment = await _paymentService.GetPaymentByOrderIdAsync(orderId);
                return Ok(payment);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetPaymentsByUserId(int userId)
        {
            try
            {
                var payments = await _paymentService.GetPaymentsByUserIdAsync(userId);
                return Ok(payments);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("user/{userId}/all")]
        public async Task<IActionResult> GetPaymentsAndPendingOrdersByUserId(int userId)
        {
            try
            {
                var payments = await _paymentService.GetPaymentsAndPendingOrdersByUserIdAsync(userId);
                return Ok(payments);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }


        //[HttpPost("complete-payment")]
        //public async Task<IActionResult> CompletePayment([FromBody] ProcessPaymentDto paymentDto)
        //{
        //    try
        //    {
        //        var payment = await _paymentService.ProcessPaymentAsync(paymentDto);
        //        return Ok(payment);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { Message = ex.Message });
        //    }
        //}

    }

}
