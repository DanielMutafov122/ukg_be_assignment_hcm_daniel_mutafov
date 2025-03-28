using Application.DTOs;
using Application.Validators;
using FluentAssertions;
using Xunit;

namespace Application.UnitTests.Validators;

public class UpdateEmployeeRequestDtoValidatorTests
{
    private readonly UpdateEmployeeRequestDtoValidator _validator = new();

    [Fact]
    public void Validator_Should_Pass_With_Valid_Data()
    {
        var dto = new UpdateEmployeeRequestDto
        {
            Id = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            Role = "Manager",
            Department = "Sales"
        };

        var result = _validator.Validate(dto);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validator_Should_Fail_When_Id_Is_Empty()
    {
        var dto = new UpdateEmployeeRequestDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            Role = "Manager",
            Department = "Sales"
        };

        var result = _validator.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(x => x.PropertyName == "Id");
    }

    [Fact]
    public void Validator_Should_Fail_When_FirstName_Is_Empty()
    {
        var dto = new UpdateEmployeeRequestDto
        {
            Id = Guid.NewGuid(),
            FirstName = "",
            LastName = "Doe",
            Email = "john@example.com",
            Role = "Manager",
            Department = "Sales"
        };

        var result = _validator.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(x => x.PropertyName == "FirstName");
    }

    [Fact]
    public void Validator_Should_Fail_When_Email_Is_Invalid()
    {
        var dto = new UpdateEmployeeRequestDto
        {
            Id = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            Email = "bademail",
            Role = "Manager",
            Department = "Sales"
        };

        var result = _validator.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == "Email");
    }

    [Fact]
    public void Validator_Should_Fail_When_Role_Is_Empty()
    {
        var dto = new UpdateEmployeeRequestDto
        {
            Id = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            Role = "",
            Department = "Sales"
        };

        var result = _validator.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == "Role");
    }

    [Fact]
    public void Validator_Should_Fail_When_Department_Is_Empty()
    {
        var dto = new UpdateEmployeeRequestDto
        {
            Id = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            Role = "Manager",
            Department = ""
        };

        var result = _validator.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == "Department");
    }
}

