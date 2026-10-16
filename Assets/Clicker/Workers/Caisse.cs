using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Clicker.Workers
{
    // A deposit of ore at a position. Its stock only goes down: Extract never gives more than what is left.
    public sealed class Caisse
    {
        public Vector3 Position { get; }
        [SerializeField] GameObject CaissePrefab;


        public void SpawnCaisse(KeyCode input)
        {
            if (Input.GetKeyDown(KeyCode.E) == true)
            {
                Vector3 newPos = new(30f,  Random.Range(-14f, 14f), 1);
                GameObject.Instantiate(CaissePrefab, newPos, Quaternion.identity);
                Debug.Log("ça marche");
            }
        }
    }
}