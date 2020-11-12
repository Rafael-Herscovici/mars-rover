using AutoFixture;
using FluentAssertions;
using MarsRover.Enums;
using MarsRover.Models;
using MarsRover.Services;
using Xunit;

namespace MarsRover.UnitTests
{
    public class When_Moving_Rover
    {
        private readonly Fixture _fixture = new Fixture();

        [Fact]
        public void With_Invalid_Command_Should_Not_Move()
        {
            // Arrange
            var sut = _fixture.Build<Controller>()
                .With(x => x.Rover, new Rover
                {
                    PosX = 10,
                    PosY = 10,
                    Bearing = Bearing.N
                }).Create();
            var expected = new Rover { PosX = 10, PosY = 10, Bearing = Bearing.N };

            // Act
            var result = sut.Move("X0L2");

            // Assert
            result.Rover.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void With_Command_Within_Bounds_Should_Move_Rover()
        {
            // Arrange
            var sut = _fixture.Build<Controller>()
                .With(x => x.Rover, new Rover
                {
                    PosX = 10,
                    PosY = 10,
                    Bearing = Bearing.N
                }).Create();
            var expected = new Rover { PosX = 60, PosY = 70, Bearing = Bearing.N };

            // Act
            var result = sut.Move("R50L60");

            // Assert
            result.Rover.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void With_Command_Out_of_Bounds_Mode_A_Should_Move_Out_of_Bounds()
        {
            // Arrange
            var sut = new Fixture().Build<Controller>()
                .With(x => x.Options, new ControllerOptions { Mode = Mode.A })
                .With(x => x.Rover, new Rover
                {
                    PosX = 0,
                    PosY = 0,
                    Bearing = Bearing.N
                }).Create();
            var expected = new Rover { PosX = -10, PosY = -10, Bearing = Bearing.S };

            // Act
            var result = sut.Move("L10L10");

            // Assert
            result.Rover.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void With_Command_Out_of_Bounds_Mode_C_Should_Stay_Within_Bounds()
        {
            // Arrange
            var sut = new Fixture().Build<Controller>()
                .With(x => x.Options, new ControllerOptions { Mode = Mode.C })
                .With(x => x.Rover, new Rover
                {
                    PosX = 0,
                    PosY = 0,
                    Bearing = Bearing.N
                }).Create();
            var expected = new Rover { PosX = 11, PosY = 10, Bearing = Bearing.N };

            // Act
            var result = sut.Move("L11L11L11L10");

            // Assert
            result.Rover.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void With_Command_Out_of_Bounds_Mode_S_Should_Stay_Within_Bounds_and_Stop_Processing_Commands()
        {
            // Arrange
            var sut = new Fixture().Build<Controller>()
                .With(x => x.Options, new ControllerOptions { Mode = Mode.S })
                .With(x => x.Rover, new Rover
                {
                    PosX = 10,
                    PosY = 10,
                    Bearing = Bearing.N
                }).Create();
            var expected = new Rover { PosX = 0, PosY = 10, Bearing = Bearing.W };

            // Act
            var result = sut.Move("L11L11L11L11R50");

            // Assert
            result.Rover.Should().BeEquivalentTo(expected);
        }
    }
}
