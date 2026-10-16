using Clicker.Shared;
using UnityEngine;

namespace Clicker.Buildings
{
    // Places the planned buildings one after the other. The order is given from the Inspector
    // (right-click on the component, in Play mode): no dependency on an input system.
    public sealed class BuildingPlacer : MonoBehaviour
    {
        [SerializeField] private GameSession session;
        [SerializeField] private GameObject buildingPrefab;
        [SerializeField] private Vector3 origin = new Vector3(-8f, 0f, -8f);
        [SerializeField] private float cellSize = 2f;
        [SerializeField] private Vector2Int gridSize = new Vector2Int(9, 9);
        // The middle column, right across the path between the base and the mine.
        [SerializeField] private Vector2Int[] plannedCells = { new Vector2Int(4, 4), new Vector2Int(4, 3), new Vector2Int(4, 5) };

        private BuildingGrid grid;
        private int next;

        private void Awake()
        {
            grid = new BuildingGrid(gridSize.x, gridSize.y);
        }

        [ContextMenu("Place next building")]
        private void PlaceNext()
        {
            if (next >= plannedCells.Length)
            {
                Debug.Log("No planned cell left.");
                return;
            }
            Vector2Int cell = plannedCells[next];
            if (!grid.TryPlace(cell.x, cell.y, session.Wallet))
            {
                Debug.Log($"Cannot build on {cell}: occupied, or {grid.Price} coins needed and {session.Wallet.Coins} available.");
                return;
            }
            next++;
            // The prefab keeps its own height, so a cube of any size stands on the ground.
            var position = origin + new Vector3(cell.x * cellSize, buildingPrefab.transform.position.y, cell.y * cellSize);
            Instantiate(buildingPrefab, position, buildingPrefab.transform.rotation);
            Debug.Log($"Building placed on {cell}, {session.Wallet.Coins} coins left.");
        }
    }
}
