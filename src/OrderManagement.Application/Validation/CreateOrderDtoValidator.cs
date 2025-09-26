using FluentValidation;
using OrderManagement.Application.DTOs.Orders;


namespace OrderManagement.Application.Validation
{
    public class CreateOrderDtoValidator : AbstractValidator<CreateOrderDto>
    {
        public CreateOrderDtoValidator()
        {
            RuleFor(x => x.Items)
                .NotNull()
                .Must(i => i.Count > 0).WithMessage("Order must contain at least one item.");

            RuleForEach(x => x.Items).SetValidator(new OrderItemDtoValidator());
        }

        private class OrderItemDtoValidator : AbstractValidator<OrderItemDto>
        {
            public OrderItemDtoValidator()
            {
                RuleFor(x => x.Name)
                    .NotEmpty().WithMessage("Item name is required.")
                    .MaximumLength(200);

                RuleFor(x => x.Price)
                    .GreaterThan(0).WithMessage("Item price must be greater than zero.");

                RuleFor(x => x.Quantity)
                    .GreaterThan(0).WithMessage("Quantity must be at least 1.");
            }
        }
    }
}
