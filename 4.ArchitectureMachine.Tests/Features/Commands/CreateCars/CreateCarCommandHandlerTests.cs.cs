using _2.Application.Entities;
using _2.Application.Repositories.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace _4.ArchitectureMachine.Tests.Features.Commands.CreateCars
{
// Verify Mock Calls
// Negative Tests
// GetAll / Query Handler tests
// بعدش AutoMapper + Integration mindset
    public class CreateCarCommandHandlerTests
    {
        private readonly Mock<IGenericRepository<Car>> _CarRepositoryMock;
        private readonly Mock<IGenericRepository<CarFeature>> _FeatureRepositoryMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        public CreateCarCommandHandlerTests()
        {
            _CarRepositoryMock = new Mock<IGenericRepository<Car>>();
            _FeatureRepositoryMock = new Mock<IGenericRepository<CarFeature>>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        }

        [Fact]
        public async Task Handle_Should_Create_Car_And_Return_Id()
        {
            var httpContext = new DefaultHttpContext();

            httpContext.User = new ClaimsPrincipal(
                new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.NameIdentifier, "123")
                }));

            _httpContextAccessorMock
                .Setup(x => x.HttpContext)
                .Returns(httpContext);

            var features = new List<CarFeature>
          {
             new CarFeature
           {
               Id = 1,
               Title = "Test Feature",
               Color = "White",
               Rank = 1,
                HealthyBody = 90
            }
           }.AsQueryable();

            _FeatureRepositoryMock.Setup(x => x.GetQueryable())
                .Returns(features);

            _CarRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Car>()))
                .Callback<Car>(car=>
                {
                    car.Id = 1;
                })

               .Returns(Task.CompletedTask);

            _CarRepositoryMock.Setup(x => x.SaveAsync())
                .Returns(Task.CompletedTask);

            // Arrange
            var Handler = new CreateCarCommandHandler(
                _CarRepositoryMock.Object,
                _FeatureRepositoryMock.Object,
                _httpContextAccessorMock.Object);

            var command = new CreateCarCommand
            {
                Name = "BMW",
                FeatureIds = new List<int> { 1 }
            };

            // Act
            var result = await Handler.Handle(
                command,
                CancellationToken.None);

            // Assert
            result.Should().BeGreaterThan(0);
        }
    }
}
