using Application.DTOs;
using FluentValidation;

namespace Application.Validators;

public class CreateEmployeeRequestDtoValidator : AbstractValidator<CreateEmployeeRequestDto>
{
    public CreateEmployeeRequestDtoValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("First Name is required.");
        RuleFor(x => x.LastName).NotEmpty().WithMessage("Last Name is required.");
        RuleFor(x => x.Role).NotEmpty().WithMessage("Role is required.");
        RuleFor(x => x.Email).EmailAddress().WithMessage("Invalid email format.");
        RuleFor(x => x.Department).NotEmpty().WithMessage("Department is required.");
    }
}
