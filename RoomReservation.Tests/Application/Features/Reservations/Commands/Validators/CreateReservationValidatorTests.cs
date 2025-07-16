using FluentValidation.TestHelper;
using RoomReservation.Application.Features.Reservations.Commands;
using RoomReservation.Application.Validators.Reservations;
using RoomReservation.Application.Services;
using Moq;
using Xunit;
using RoomReservation.Domain.Enums;

namespace RoomReservation.Tests.Application.Features.Reservations.Commands.Validators;

public class CreateReservationValidatorTests
{
    private readonly CreateReservationValidator _validator;
    private readonly Mock<IHtmlSanitizerService> _sanitizerMock;

    public CreateReservationValidatorTests()
    {
        _sanitizerMock = new Mock<IHtmlSanitizerService>();
        _sanitizerMock.Setup(x => x.Sanitize(It.IsAny<string>())).Returns<string>(s => s);
        _validator = new CreateReservationValidator(_sanitizerMock.Object);
    }

    [Fact]
    public void Should_Pass_Validation_When_Command_Is_Valid()
    {
        var command = new CreateReservationCommand
        {
            RoomId = Guid.NewGuid(),
            ReservedBy = "João da Silva",
            StartTime = DateTime.UtcNow.AddHours(1),
            EndTime = DateTime.UtcNow.AddHours(2),
            Status = ReservationStatus.Confirmed
        };

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Fail_When_RoomId_Is_Empty()
    {
        var command = new CreateReservationCommand
        {
            RoomId = Guid.Empty,
            ReservedBy = "João",
            StartTime = DateTime.UtcNow,
            EndTime = DateTime.UtcNow.AddHours(1),
            Status = ReservationStatus.Pending
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.RoomId);
    }

    [Fact]
    public void Should_Fail_When_ReservedBy_Has_Html()
    {
        _sanitizerMock.Setup(x => x.Sanitize("<script>")).Returns("");

        var command = new CreateReservationCommand
        {
            RoomId = Guid.NewGuid(),
            ReservedBy = "<script>",
            StartTime = DateTime.UtcNow,
            EndTime = DateTime.UtcNow.AddHours(1),
            Status = ReservationStatus.Confirmed
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.ReservedBy);
    }

    [Fact]
    public void Should_Fail_When_ReservedBy_Exceeds_Max_Length()
    {
        var longName = new string('x', 101);

        var command = new CreateReservationCommand
        {
            RoomId = Guid.NewGuid(),
            ReservedBy = longName,
            StartTime = DateTime.UtcNow,
            EndTime = DateTime.UtcNow.AddHours(1),
            Status = ReservationStatus.Pending
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.ReservedBy);
    }

    [Fact]
    public void Should_Fail_When_EndTime_Is_Before_StartTime()
    {
        var now = DateTime.UtcNow;

        var command = new CreateReservationCommand
        {
            RoomId = Guid.NewGuid(),
            ReservedBy = "Teste",
            StartTime = now.AddHours(2),
            EndTime = now,
            Status = ReservationStatus.Pending
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.EndTime);
    }

    [Fact]
    public void Should_Fail_When_Status_Is_Invalid()
    {
        var command = new CreateReservationCommand
        {
            RoomId = Guid.NewGuid(),
            ReservedBy = "Teste",
            StartTime = DateTime.UtcNow,
            EndTime = DateTime.UtcNow.AddHours(1),
            Status = (ReservationStatus)999 
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Status);
    }
}
