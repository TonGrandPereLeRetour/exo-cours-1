using System;
using UnityEngine;
using UnityEngine.AI;

namespace Clicker.Workers
{
    // A body driven by a NavMeshAgent. The states stay as they are: only the way the worker moves
    // changes.
    public sealed class NavMeshBody : IWorkerBody
    {
        private readonly NavMeshAgent agent;

        public NavMeshBody(NavMeshAgent agent)
        {
            this.agent = agent ? agent : throw new ArgumentNullException(nameof(agent));
        }

        // Exercise step 1: true once the path is computed (pathPending) and the agent stands within
        // its stopping distance of the destination (remainingDistance), with a small tolerance.
        public bool HasArrived => throw new NotImplementedException("Exercise step 1: complete NavMeshBody.HasArrived.");

        // Exercise step 1: ask the agent for a path to the destination.
        public void MoveTo(Vector3 destination)
        {
            throw new NotImplementedException("Exercise step 1: complete NavMeshBody.MoveTo.");
        }

        // Exercise step 1: drop the current path, so that the agent stops where it stands.
        public void Stop()
        {
            throw new NotImplementedException("Exercise step 1: complete NavMeshBody.Stop.");
        }
    }
}
