using UnityEngine;

namespace Clicker.Workers
{
    // What moves the worker. The states only ask for a destination and wait for the arrival, so the
    // same states drive a simulated body today and a NavMeshAgent later.
    public interface IWorkerBody
    {
        bool HasArrived { get; }

        void MoveTo(Vector3 destination);

        // Stands still where it is: the destination becomes the current position.
        void Stop();
    }
}
