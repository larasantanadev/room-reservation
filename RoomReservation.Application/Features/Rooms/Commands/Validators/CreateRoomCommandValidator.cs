using FluentValidation;
using RoomReservation.Application.Features.Rooms.Commands;
using RoomReservation.Application.Services;

namespace RoomReservation.Application.Features.Rooms.Commands.Validators
{
    public class CreateRoomCommandValidator : AbstractValidator<CreateRoomCommand>
    {
        public CreateRoomCommandValidator(IHtmlSanitizerService sanitizer)
        {
            RuleFor(x => x.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("O campo Name é obrigatório.")
                .MaximumLength(100).WithMessage("O campo Name deve ter no máximo 100 caracteres.")
                .Must((_, value) =>
                {
                    var sanitized = sanitizer.Sanitize(value ?? string.Empty);
                    return sanitized == value;
                }).WithMessage("O campo Name contém HTML inválido.");

            RuleFor(x => x.Capacity)
                .GreaterThan(0).WithMessage("O campo Capacity deve ser maior que zero.");
        }
    }
}
