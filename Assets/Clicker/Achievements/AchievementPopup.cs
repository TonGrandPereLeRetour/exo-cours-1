using TMPro;
using UnityEngine;

namespace Clicker.Achievements
{
    public sealed class AchievementPopup : MonoBehaviour
    {
        [SerializeField] private TMP_Text label;
        [SerializeField] private float visibleSeconds = 2f;

        public void Show(string text)
        {
            label.text = text;
            gameObject.SetActive(true);
            CancelInvoke(nameof(Hide));
            Invoke(nameof(Hide), visibleSeconds);
        }

        private void Hide() => gameObject.SetActive(false);
    }
}
