using Application.DTOs;
using Application.Validators;
using FluentAssertions;
using Xunit;

namespace Application.UnitTests.Validators;

public class CreateEmployeeRequestDtoValidatorTests
{
    private readonly CreateEmployeeRequestDtoValidator _validator = new();

    [Fact]
    public void Validator_Should_Pass_With_Valid_Data()
    {
        var dto = new CreateEmployeeRequestDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            Role = "Developer",
            Department = "Engineering"
        };

        var result = _validator.Validate(dto);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validator_Should_Fail_When_FirstName_Is_Empty()
    {
        var dto = new CreateEmployeeRequestDto
        {
            FirstName = "",
            LastName = "Doe",
            Email = "john@example.com",
            Role = "Developer",
            Department = "Engineering"
        };

        var result = _validator.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(x => x.PropertyName == "FirstName");
    }

    [Fact]
    public void Validator_Should_Fail_When_Email_Is_Invalid()
    {
        var dto = new CreateEmployeeRequestDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "not-an-email",
            Role = "Developer",
            Department = "Engineering"
        };

        var result = _validator.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == "Email");
    }

    [Fact]
    public void Validator_Should_Fail_When_Role_Is_Empty()
    {
        var dto = new CreateEmployeeRequestDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            Role = "",
            Department = "Engineering"
        };

        var result = _validator.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == "Role");
    }

    [Fact]
    public void Validator_Should_Fail_When_Department_Is_Empty()
    {
        var dto = new CreateEmployeeRequestDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            Role = "Developer",
            Department = ""
        };

        var result = _validator.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == "Department");
    }
}

