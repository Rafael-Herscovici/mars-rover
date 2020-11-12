using AutoFixture;
using FluentAssertions;
using MarsRover.Models;
using MarsRover.Services;
using Moq;
using System;
using System.Collections.Generic;
using MarsRover.Enums;
using Xunit;

namespace MarsRover.UnitTests
{
    public class When_Deploying_Rover
    {
        private readonly IFixture _fixture;
        private readonly IDispatcher _sut;
        private readonly ControllerOptions _controllerOptions;

        public When_Deploying_Rover()
        {
            var mockServiceProvider = new Mock<IServiceProvider>();
            _controllerOptions = new ControllerOptions
            {
                Mode = Mode.A
            };
            mockServiceProvider
                .Setup(x => x.GetService(typeof(ControllerOptions)))
                .Returns(_controllerOptions);
            mockServiceProvider
                .Setup(x => x.GetService(typeof(IController)))
                .Returns(new Controller(_controllerOptions));

            _sut = new Dispatcher(mockServiceProvider.Object);
            _fixture = new Fixture();

        }

        [Fact]
        public void Should_Return_Controller_With_Deployed_Rover()
        {
            // Arrange
            var expected = _fixture.Build<Controller>()
                .With(x => x.Options, _controllerOptions)
                .With(x => x.Rover, new Rover
                {
                    PosX = 0,
                    PosY = 0,
                    Bearing = Bearing.N
                }).Create();

            // Act
            var result = _sut.SetPosition(expected.Rover.PosX, expected.Rover.PosY, expected.Rover.Bearing);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Should_Add_Created_Controller_To_List()
        {
            // Arrange
            var expectedControllers = new List<IController>();
            for (var i = 0; i < 10; i++)
            {
                var fixture = _fixture
                    .Build<Controller>()
                    .Create();
                var controller = _sut.SetPosition(fixture.Rover.PosX, fixture.Rover.PosY, fixture.Rover.Bearing);
                expectedControllers.Add(controller);
            }

            // Act
            var result = _sut.GetAll();

            // Assert
            result.Should().BeEquivalentTo(expectedControllers);
        }
    }
}
