using System;
using UnityEngine;

namespace Clicker.Workers
{
    // A body without a scene: travelling is a straight line at constant speed. It lets the state
    // machine run, and be tested, before any NavMesh exists.
    public sealed class SimulatedBody : IWorkerBody
    {
        private readonly float speed;
        private Vector3 destination;

        public SimulatedBody(Vector3 start, float speed = 3.5f)
        {
            if (speed <= 0f) throw new ArgumentOutOfRangeException(nameof(speed), "Speed must be positive.");
            this.speed = speed;
            Position = start;
            destination = start;
        }

        public Vector3 Position { get; private set; }

        public bool HasArrived => Position == destination;

        public void MoveTo(Vector3 destination) => this.destination = destination;

        public void Stop() => destination = Position;

        // Called once per frame by whoever owns the body, before the worker ticks.
        public void Advance(float deltaTime)
        {
            Position = Vector3.MoveTowards(Position, destination, speed * deltaTime);
        }
    }
}
