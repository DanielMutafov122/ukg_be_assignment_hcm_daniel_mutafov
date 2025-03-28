using API.Controllers;
using Application.DTOs;
using Application.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace API.Tests.Controllers;

public class AuthControllerTests
{
    private readonly Mock<IAuthService> _authServiceMock;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _authServiceMock = new Mock<IAuthService>();
        _controller = new AuthController(_authServiceMock.Object);
    }

    [Fact]
    public async Task Login_ReturnsOk_WithValidCredentials()
    {
        var request = new LoginRequestDto { Email = "user@example.com", Password = "password123" };
        var response = new LoginResponseDto { Token = "mocked.jwt.token" };

        _authServiceMock.Setup(s => s.Authenticate(request)).ReturnsAsync(response);

        var result = await _controller.Login(request);

        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeEquivalentTo(response);
    }

    [Fact]
    public async Task Login_ReturnsUnauthorized_WithInvalidCredentials()
    {
        var request = new LoginRequestDto { Email = "wrong@example.com", Password = "wrongpass" };
        _authServiceMock.Setup(s => s.Authenticate(request)).ReturnsAsync((LoginResponseDto?)null);

        var result = await _controller.Login(request);

        result.Should().BeOfType<UnauthorizedResult>();
    }
}
