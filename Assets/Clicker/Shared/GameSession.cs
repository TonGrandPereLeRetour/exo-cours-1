using UnityEngine;

namespace Clicker.Shared
{
    // State shared by every slice; one instance in the scene.
    public sealed class GameSession : MonoBehaviour
    {
        public Wallet Wallet { get; } = new Wallet();

        public ClickCounter Clicks { get; } = new ClickCounter();
    }
}
