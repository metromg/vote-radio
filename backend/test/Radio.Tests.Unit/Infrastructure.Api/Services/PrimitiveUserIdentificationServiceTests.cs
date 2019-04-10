using System;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using NUnit.Framework;
using Radio.Infrastructure.Api.Services;

namespace Radio.Tests.Unit.Infrastructure.Api.Services
{
    [TestFixture]
    public class PrimitiveUserIdentificationServiceTests
    {
        private IPrimitiveUserIdentificationService _primitiveUserIdentificationService;

        [SetUp]
        public void Setup()
        {
            _primitiveUserIdentificationService = new PrimitiveUserIdentificationService();
        }

        [Test]
        public void GetOrCreateUserId_WithExistingUserId_ReturnsExistingUserId()
        {
            // Arrange
            var existingUserId = Guid.NewGuid();

            var httpContext = Substitute.For<HttpContext>();
            httpContext.Request.Cookies["USER_ID"].Returns(existingUserId.ToString());

            // Act
            var userId = _primitiveUserIdentificationService.GetOrCreateUserId(httpContext);

            // Assert
            Assert.That(userId, Is.EqualTo(existingUserId));

            httpContext.Response.Cookies.Received(0).Append("USER_ID", Arg.Any<string>(), Arg.Any<CookieOptions>());
        }

        [Test]
        public void GetOrCreateUserId_WithNoExistingUserId_ReturnsNewUserId()
        {
            // Arrange
            var httpContext = Substitute.For<HttpContext>();
            httpContext.Request.Cookies["USER_ID"].Returns(default(string));

            // Act
            var userId = _primitiveUserIdentificationService.GetOrCreateUserId(httpContext);

            // Assert
            Assert.That(userId, Is.Not.Null);
            Assert.That(userId, Is.Not.EqualTo(Guid.Empty));

            httpContext.Response.Cookies.Received(1).Append("USER_ID", userId.ToString(), Arg.Any<CookieOptions>());
        }
    }
}
