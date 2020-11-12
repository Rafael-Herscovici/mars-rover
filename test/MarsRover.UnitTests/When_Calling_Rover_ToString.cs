using System;
using AutoFixture;
using FluentAssertions;
using MarsRover.Enums;
using MarsRover.Models;
using Xunit;

namespace MarsRover.UnitTests
{
    public class When_Calling_Rover_ToString
    {
        [Fact]
        public void Should_Return_Formatted_String()
        {
            // Arrange
            var sut = new Fixture().Create<Rover>();

            // Act
            var result = sut.ToString();

            // Assert
            result.Should().Be($"{sut.PosX}, {sut.PosY}, {Enum.GetName(typeof(Bearing), sut.Bearing)}");
        }
    }
}
