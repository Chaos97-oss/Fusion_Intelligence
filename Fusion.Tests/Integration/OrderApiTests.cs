using System.Net;
using System.Net.Http.Json;
using Fusion.Core.DTOs;
using Fusion.Core.Enums;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Fusion.Tests.Integration;

public class OrderApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public OrderApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
        _client.DefaultRequestHeaders.Add("X-Api-Key", "Secret_Assessment_Key_123");
    }

    [Fact]
    public async Task CreateOrder_ValidDto_ReturnsCreated()
    {
        // Arrange
        var createDto = new CreateOrderDto
        {
            CustomerName = "John Doe",
            PickupLocation = "Location A",
            DropoffLocation = "Location B"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/orders", createDto);

        // Assert
        response.EnsureSuccessStatusCode();
        var order = await response.Content.ReadFromJsonAsync<OrderDto>();
        Assert.NotNull(order);
        Assert.Equal("John Doe", order.CustomerName);
        Assert.Equal(OrderStatus.Created.ToString(), order.Status);
    }

    [Fact]
    public async Task CreateOrder_WithoutApiKey_ReturnsUnauthorized()
    {
        // Arrange
        var unauthorizedClient = new HttpClient { BaseAddress = _client.BaseAddress };
        var createDto = new CreateOrderDto { CustomerName = "Test" };

        // Act
        var response = await unauthorizedClient.PostAsJsonAsync("/api/orders", createDto);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
