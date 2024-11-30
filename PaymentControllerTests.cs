using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using QuickServeAPP.Controllers;
using QuickServeAPP.DTOs;
using QuickServeAPP.Services;
using QuickServeAPP.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Controllers.Tests
{
    [TestFixture]
    public class PaymentControllerTests
    {
        private Mock<IPaymentService> _mockPaymentService;
        private PaymentController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockPaymentService = new Mock<IPaymentService>();
            _controller = new PaymentController(_mockPaymentService.Object);
        }

        [Test]
        public async Task ProcessPayment_ShouldReturnOk_WhenPaymentIsProcessedSuccessfully()
        {
            var paymentDto = new ProcessPaymentDto
            {
                OrderID = 1,
                UserID = 1,
                AmountPaid = 100,
                PaymentMethod = PaymentMethodEnum.CreditCard
            };

            var payment = new PaymentDto
            {
                PaymentID = 1,
                OrderID = paymentDto.OrderID,
                UserID = paymentDto.UserID,
                AmountPaid = paymentDto.AmountPaid,
                PaymentMethod = paymentDto.PaymentMethod.ToString(),
                PaymentStatus = "Completed",
                PaymentDate = System.DateTime.UtcNow
            };

            _mockPaymentService.Setup(p => p.ProcessPaymentAsync(paymentDto)).ReturnsAsync(payment);

            var result = await _controller.ProcessPayment(paymentDto);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual(payment, okResult.Value);
        }

        [Test]
        public async Task GetPaymentByOrderId_ShouldReturnOk_WithPaymentDetails()
        {
            int orderId = 1;
            var payment = new PaymentDto
            {
                PaymentID = 1,
                OrderID = orderId,
                UserID = 1,
                AmountPaid = 100,
                PaymentMethod = "CreditCard",
                PaymentStatus = "Completed",
                PaymentDate = System.DateTime.UtcNow
            };

            _mockPaymentService.Setup(p => p.GetPaymentByOrderIdAsync(orderId)).ReturnsAsync(payment);

            var result = await _controller.GetPaymentByOrderId(orderId);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual(payment, okResult.Value);
        }

        [Test]
        public async Task GetPaymentsByUserId_ShouldReturnOk_WithPaymentsList()
        {
            int userId = 1;
            var payments = new List<PaymentDto>
            {
                new PaymentDto
                {
                    PaymentID = 1,
                    OrderID = 1,
                    UserID = userId,
                    AmountPaid = 100,
                    PaymentMethod = "CreditCard",
                    PaymentStatus = "Completed",
                    PaymentDate = System.DateTime.UtcNow
                },
                new PaymentDto
                {
                    PaymentID = 2,
                    OrderID = 2,
                    UserID = userId,
                    AmountPaid = 50,
                    PaymentMethod = "DebitCard",
                    PaymentStatus = "Completed",
                    PaymentDate = System.DateTime.UtcNow
                }
            };

            _mockPaymentService.Setup(p => p.GetPaymentsByUserIdAsync(userId)).ReturnsAsync(payments);

            var result = await _controller.GetPaymentsByUserId(userId);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual(payments, okResult.Value);
        }

        [Test]
        public async Task GetPaymentsAndPendingOrdersByUserId_ShouldReturnOk_WithPaymentsAndPendingOrders()
        {
            int userId = 1;
            var payments = new List<PaymentDto>
            {
                new PaymentDto
                {
                    PaymentID = 1,
                    OrderID = 1,
                    UserID = userId,
                    AmountPaid = 100,
                    PaymentMethod = "CreditCard",
                    PaymentStatus = "Completed",
                    PaymentDate = System.DateTime.UtcNow
                }
            };

            var pendingOrders = new List<PaymentDto>
            {
                new PaymentDto
                {
                    PaymentID = 0,
                    OrderID = 2,
                    UserID = userId,
                    AmountPaid = 50,
                    PaymentMethod = null,
                    PaymentStatus = "Not Completed",
                    PaymentDate = System.DateTime.UtcNow
                }
            };

            _mockPaymentService.Setup(p => p.GetPaymentsAndPendingOrdersByUserIdAsync(userId)).ReturnsAsync(payments.Concat(pendingOrders));

            var result = await _controller.GetPaymentsAndPendingOrdersByUserId(userId);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual(payments.Concat(pendingOrders), okResult.Value);
        }
    }
}
