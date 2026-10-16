using System;
using UnityEngine;

namespace Clicker.Workers
{
    // Tuning values, editable in the Inspector of WorkerRunner.
    [Serializable]
    public sealed class WorkerSettings
    {
        public int Capacity = 10;
        public float OrePerSecond = 2f;
        public long CoinsPerOre = 1;
        public Vector3 HomePoint = new Vector3(-8.5f, 0f, 0f);
        public Vector3 MinePoint = new Vector3(8.5f, 0f, 0f);
        public int RestEveryTrips = 3;
        public float RestDuration = 2f;
    }
}
