using FluentValidation;
using RoomReservation.Application.Features.Reservations.Commands;
using RoomReservation.Application.Services;

namespace RoomReservation.Application.Validators.Reservations
{
    public class UpdateReservationValidator : AbstractValidator<UpdateReservationCommand>
    {
        public UpdateReservationValidator(IHtmlSanitizerService sanitizer)
        {
            RuleFor(x => x.Id)
           .NotEmpty().WithMessage("O campo Id é obrigatório.");

            RuleFor(x => x.RoomId)
               .NotEmpty().WithMessage("O campo RoomId é obrigatório.")
               .NotEqual(Guid.Empty).WithMessage("O campo RoomId não pode ser vazio.");

            RuleFor(x => x.ReservedBy)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("O campo ReservedBy é obrigatório.")
                .MaximumLength(100).WithMessage("O campo ReservedBy deve ter no máximo 100 caracteres.")
                .Must((_, value) =>
                {
                    var sanitized = sanitizer.Sanitize(value ?? string.Empty);
                    return sanitized == value;
                }).WithMessage("O campo ReservedBy contém HTML inválido.");

            RuleFor(x => x.NumberOfAttendees)
               .GreaterThan(0).WithMessage("O número de participantes deve ser maior que zero.");

            RuleFor(x => x.StartTime)
                .NotEmpty().WithMessage("O campo StartTime é obrigatório.");

            RuleFor(x => x.EndTime)
                .NotEmpty().WithMessage("O campo EndTime é obrigatório.")
                .GreaterThan(x => x.StartTime)
                .WithMessage("EndTime deve ser maior que StartTime.");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Status inválido.");
        }
    }
}
