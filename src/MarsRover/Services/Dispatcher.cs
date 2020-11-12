using System;
using MarsRover.Enums;
using MarsRover.Models;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace MarsRover.Services
{
    /// <summary>
    /// Dispatcher interface
    /// </summary>
    public interface IDispatcher
    {
        /// <summary>
        /// Get a all the deployed rovers
        /// </summary>
        /// <returns>An <see cref="IReadOnlyCollection{T}"/> of <see cref="IRover"/></returns>
        public IReadOnlyCollection<IController> GetAll();
        /// <summary>
        /// Deploy and start tacking a rover
        /// </summary>
        /// <param name="x">Initial PosX Position</param>
        /// <param name="y">Initial PosY Position</param>
        /// <param name="direction">Initial <see cref="Bearing"/> of the rover</param>
        /// <returns>The deployed rover</returns>
        /// <remarks>
        /// While this looks like a functionality of the <see cref="Controller"/>,
        /// Imagine it more like the spaceship the lands the rover in its initial position
        /// </remarks>
        public IController SetPosition(int x, int y, Bearing direction);
    }

    /// <summary>
    /// This would be our so called "Factory"
    /// Responsibilities:
    /// Deploy new rovers, Keep track of deployed rovers
    /// </summary>
    /// <remarks>
    /// Currently bare boned, but later on we could implement GetAll, GetById, etc...
    /// If we were to implement Grid size (e.g MaxX, MaxY), we would have implemented it here,
    /// So any rover dispatched by this dispatcher, would have the same grid size
    /// And pass it to the controller (so its aware of the grid)
    /// </remarks>
    public class Dispatcher : IDispatcher
    {
        private readonly List<IController> _controllers;
        private readonly IServiceProvider _serviceProvider;
        public Dispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _controllers = new List<IController>();
        }

        public IReadOnlyCollection<IController> GetAll()
        {
            return _controllers.AsReadOnly();
        }

        public IController SetPosition(int x, int y, Bearing bearing)
        {
            var rover = new Rover
            {
                PosX = x,
                PosY = y,
                Bearing = bearing
            };

            // Get a new instance of controller
            var controller = _serviceProvider.GetRequiredService<IController>();

            // Assign this rover to be commanded by that controller
            controller.Rover = rover;

            // Add the rover to the tracked rovers
            _controllers.Add(controller);
            return controller;
        }
    }
}
