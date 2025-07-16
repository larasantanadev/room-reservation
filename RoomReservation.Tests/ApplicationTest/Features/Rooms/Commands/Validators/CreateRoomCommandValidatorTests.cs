using FluentAssertions;
using Moq;
using RoomReservation.Application.Features.Rooms.Commands;
using RoomReservation.Application.Features.Rooms.Commands.Validators;
using RoomReservation.Application.Services;
using Xunit;

namespace RoomReservation.Tests.Application.Features.Rooms.Commands.Validators;

public class CreateRoomCommandValidatorTests
{
    private readonly Mock<IHtmlSanitizerService> _htmlSanitizerMock;
    private readonly CreateRoomCommandValidator _validator;

    public CreateRoomCommandValidatorTests()
    {
        _htmlSanitizerMock = new Mock<IHtmlSanitizerService>();
        _validator = new CreateRoomCommandValidator(_htmlSanitizerMock.Object);
    }

    [Fact]
    public void Should_Pass_When_Command_Is_Valid()
    {
        // Arrange
        var command = new CreateRoomCommand { Name = "Sala de Reunião", Capacity = 10 };
        _htmlSanitizerMock.Setup(s => s.Sanitize(It.IsAny<string>())).Returns((string input) => input);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Fail_When_Name_Is_Empty()
    {
        // Arrange
        var command = new CreateRoomCommand { Name = "", Capacity = 10 };
        _htmlSanitizerMock.Setup(s => s.Sanitize(It.IsAny<string>())).Returns((string input) => input);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name");
    }

    [Fact]
    public void Should_Fail_When_Name_Exceeds_MaxLength()
    {
        // Arrange
        var command = new CreateRoomCommand { Name = new string('a', 101), Capacity = 10 };
        _htmlSanitizerMock.Setup(s => s.Sanitize(It.IsAny<string>())).Returns((string input) => input);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name");
    }

    [Fact]
    public void Should_Fail_When_Name_Has_Unsafe_Html()
    {
        // Arrange
        var command = new CreateRoomCommand { Name = "<script>alert('XSS')</script>", Capacity = 10 };
        _htmlSanitizerMock.Setup(s => s.Sanitize(It.IsAny<string>())).Returns("scriptalert('XSS')script");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name");
    }

    [Fact]
    public void Should_Fail_When_Capacity_Is_Zero()
    {
        // Arrange
        var command = new CreateRoomCommand { Name = "Sala A", Capacity = 0 };
        _htmlSanitizerMock.Setup(s => s.Sanitize(It.IsAny<string>())).Returns((string input) => input);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Capacity");
    }

    [Fact]
    public void Should_Fail_When_Capacity_Is_Negative()
    {
        // Arrange
        var command = new CreateRoomCommand { Name = "Sala B", Capacity = -5 };
        _htmlSanitizerMock.Setup(s => s.Sanitize(It.IsAny<string>())).Returns((string input) => input);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Capacity");
    }
}
