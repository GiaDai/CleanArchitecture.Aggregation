using CleanArchitecture.Aggregation.Application.TypeInputs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Aggregation.Application.Validators
{
    public class ProductInputValidator : AbstractValidator<ProductTypeInput>
    {
        public ProductInputValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Tên là bắt buộc")
                .NotNull();
        }
    }
}
