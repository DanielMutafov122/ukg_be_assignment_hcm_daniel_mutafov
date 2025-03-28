using Application.DTOs;
using Application.Services;
using Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace Application.UnitTests.Services;

public class AuthServiceTests
{
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    private readonly Mock<IConfiguration> _configMock;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        var store = new Mock<IUserStore<ApplicationUser>>();
        _userManagerMock = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
        _configMock = new Mock<IConfiguration>();

        _configMock.Setup(c => c["Jwt:SecretKey"]).Returns("supersecretkey1234567890SUPERSTRONGKEY");
        _configMock.Setup(c => c["Jwt:Issuer"]).Returns("TestIssuer");
        _configMock.Setup(c => c["Jwt:Audience"]).Returns("TestAudience");

        _authService = new AuthService(_userManagerMock.Object, _configMock.Object);
    }

    [Fact]
    public async Task Authenticate_ReturnsToken_WhenValidUser()
    {
        var request = new LoginRequestDto { Email = "test@example.com", Password = "password" };
        var user = new ApplicationUser { Email = request.Email };

        _userManagerMock.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync(user);
        _userManagerMock.Setup(x => x.CheckPasswordAsync(user, request.Password)).ReturnsAsync(true);
        _userManagerMock.Setup(x => x.GetRolesAsync(user)).ReturnsAsync(new List<string> { "Admin" });

        var result = await _authService.Authenticate(request);

        Assert.NotNull(result);
        Assert.False(string.IsNullOrWhiteSpace(result.Token));
    }

    [Fact]
    public async Task Authenticate_ReturnsNull_WhenUserNotFound()
    {
        var request = new LoginRequestDto { Email = "notfound@example.com", Password = "password" };
        _userManagerMock.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync((ApplicationUser?)null);

        var result = await _authService.Authenticate(request);

        Assert.Null(result);
    }

    [Fact]
    public async Task Authenticate_ReturnsNull_WhenPasswordInvalid()
    {
        var request = new LoginRequestDto { Email = "test@example.com", Password = "wrongpass" };
        var user = new ApplicationUser { Email = request.Email };

        _userManagerMock.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync(user);
        _userManagerMock.Setup(x => x.CheckPasswordAsync(user, request.Password)).ReturnsAsync(false);

        var result = await _authService.Authenticate(request);

        Assert.Null(result);
    }
}
