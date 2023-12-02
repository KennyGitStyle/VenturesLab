using System.Text.Json;
using FluentAssertions;
using Infrastructure.Service;
using Microsoft.Extensions.Logging;
using Moq;
using StackExchange.Redis;

namespace UnitTest;

public class CacheServiceTests
{
    private readonly Mock<IDatabase> _mockDatabase;
    private readonly Mock<ILogger<CacheService>> _mockLogger;
    private readonly CacheService _cacheService;

    public CacheServiceTests()
    {
        _mockDatabase = new Mock<IDatabase>();
        _mockLogger = new Mock<ILogger<CacheService>>();
        var mockConnectionMultiplexer = new Mock<IConnectionMultiplexer>();

        mockConnectionMultiplexer.Setup(c => c.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(_mockDatabase.Object);
        _cacheService = new CacheService(mockConnectionMultiplexer.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetCachedResponse_ValidKey_ReturnsData()
    {
        string cacheKey = "testKey";
        string cachedData = JsonSerializer.Serialize(new { Message = "Hello" });
        _mockDatabase.Setup(db => db.StringGetAsync(cacheKey, It.IsAny<CommandFlags>())).ReturnsAsync(cachedData);

        var result = await _cacheService.GetCachedResponse(cacheKey);

        result.Should().BeEquivalentTo(cachedData, "because the cache should return the data stored under the valid key");
    }

    [Fact]
    public async Task GetCachedResponse_InvalidKey_ReturnsEmptyString()
    {
        string cacheKey = "invalidKey";
        _mockDatabase.Setup(db => db.StringGetAsync(cacheKey, It.IsAny<CommandFlags>())).ReturnsAsync(RedisValue.Null);

        var result = await _cacheService.GetCachedResponse(cacheKey);

        result.Should().BeEmpty("because the cache should return an empty string for an invalid key");
    }
}
