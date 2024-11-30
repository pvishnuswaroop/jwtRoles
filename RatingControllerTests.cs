using NUnit.Framework;
using Moq;
using QuickServeAPP.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using QuickServeAPP.Controllers;
using QuickServeAPP.DTOs;
using QuickServeAPP.Services;
using System.Collections.Generic;

namespace Controllers.Tests
{
    [TestFixture]
    public class RatingControllerTests
    {
        private Mock<IRatingService> _mockRatingService;
        private RatingController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockRatingService = new Mock<IRatingService>();
            _controller = new RatingController(_mockRatingService.Object);
        }

        [Test]
        public async Task SubmitRating_ShouldReturnOk_WhenServiceSucceeds()
        {
            var ratingDto = new SubmitRatingDto
            {
                OrderID = 1,
                UserID = 2,
                RestaurantID = 1,
                RatingScore = 4,
                ReviewText = "Great food!"
            };

            _mockRatingService.Setup(r => r.SubmitRatingAsync(ratingDto)).Returns(Task.CompletedTask);

            var result = await _controller.SubmitRating(ratingDto);

            Assert.IsInstanceOf<OkObjectResult>(result);

            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual("Rating submitted successfully.", okResult.Value.GetType().GetProperty("Message")?.GetValue(okResult.Value));
        }

        [Test]
        public async Task GetRatingsByRestaurantId_ShouldReturnOk_WithRatings()
        {
            int restaurantId = 1;
            var mockRatings = new List<RatingDto>
            {
                new RatingDto { RatingID = 1, RatingScore = 5, ReviewText = "Great!" },
                new RatingDto { RatingID = 2, RatingScore = 4, ReviewText = "Good!" }
            };
            _mockRatingService.Setup(r => r.GetRatingsByRestaurantIdAsync(restaurantId)).ReturnsAsync(mockRatings);

            var result = await _controller.GetRatingsByRestaurantId(restaurantId);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual(mockRatings, okResult.Value);
        }

        [Test]
        public async Task GetRatingsByMenuId_ShouldReturnOk_WithRatings()
        {
            int menuId = 1;
            var mockRatings = new List<RatingDto>
            {
                new RatingDto { RatingID = 1, RatingScore = 5, ReviewText = "Tasty!" },
                new RatingDto { RatingID = 2, RatingScore = 4, ReviewText = "Good!" }
            };
            _mockRatingService.Setup(r => r.GetRatingsByMenuIdAsync(menuId)).ReturnsAsync(mockRatings);

            var result = await _controller.GetRatingsByMenuId(menuId);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual(mockRatings, okResult.Value);
        }
    }
}
