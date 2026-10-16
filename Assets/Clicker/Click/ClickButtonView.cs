using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Clicker.Shared;

namespace Clicker.Click
{
    public sealed class ClickButtonView : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private TMP_Text coinsLabel;
        [SerializeField] private ClickController controller;
        [SerializeField] private GameSession session;

        private void OnEnable()
        {
            button.onClick.AddListener(controller.Click);
            session.Wallet.CoinsChanged += Refresh;
            Refresh(session.Wallet.Coins);
        }

        private void OnDisable()
        {
            button.onClick.RemoveListener(controller.Click);
            session.Wallet.CoinsChanged -= Refresh;
        }

        private void Refresh(long coins) => coinsLabel.text = coins.ToString();
    }
}
