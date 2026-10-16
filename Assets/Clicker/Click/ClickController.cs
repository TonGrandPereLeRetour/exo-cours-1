using Clicker.Shared;
using UnityEngine;

namespace Clicker.Click
{
    // Counts through Shared instead of calling the achievements: Click no longer knows Achievements.
    public sealed class ClickController : MonoBehaviour
    {
        [SerializeField] private GameSession session;
        [SerializeField] private long coinsPerClick = 1;

        public void Click()
        {
            session.Wallet.Add(coinsPerClick);
            session.Clicks.Register();
        }
    }
}
