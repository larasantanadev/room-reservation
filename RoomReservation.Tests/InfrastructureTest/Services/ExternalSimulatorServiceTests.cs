using System.Net;
using FluentAssertions;
using Moq;
using Moq.Protected;
using RoomReservation.Infrastructure.Services;

namespace RoomReservation.Tests.InfrastructureTest.Services
{
    public class ExternalSimulatorServiceTests
    {
        [Fact]
        public async Task GetSimulatedResponseAsync_ShouldReturnExpectedString()
        {
            // Arrange
            var expectedContent = "Simulated response from external API";

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(expectedContent),
                });

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("https://localhost:9999")
            };

            var factoryMock = new Mock<IHttpClientFactory>();
            factoryMock.Setup(f => f.CreateClient("ExternalApi")).Returns(httpClient);

            var service = new ExternalSimulatorService(factoryMock.Object);

            // Act
            var result = await service.GetSimulatedResponseAsync();

            // Assert
            result.Should().Be(expectedContent);
        }

        [Fact]
        public async Task GetSimulatedResponseAsync_ShouldThrow_WhenResponseIsUnsuccessful()
        {
            // Arrange
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                });

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("https://localhost:9999")
            };

            var factoryMock = new Mock<IHttpClientFactory>();
            factoryMock.Setup(f => f.CreateClient("ExternalApi")).Returns(httpClient);

            var service = new ExternalSimulatorService(factoryMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => service.GetSimulatedResponseAsync());
        }
    }
}
