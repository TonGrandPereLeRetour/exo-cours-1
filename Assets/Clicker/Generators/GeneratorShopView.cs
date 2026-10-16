using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Clicker.Generators
{
    // One shop line per generator of the catalog.
    public sealed class GeneratorShopView : MonoBehaviour
    {
        [SerializeField] private GeneratorController controller;
        [SerializeField] private int generatorIndex;
        [SerializeField] private Button buyButton;
        [SerializeField] private TMP_Text costLabel;
        [SerializeField] private TMP_Text ownedLabel;

        private void OnEnable() => buyButton.onClick.AddListener(Buy);

        private void OnDisable() => buyButton.onClick.RemoveListener(Buy);

        // Start, not OnEnable: the controller builds its generators in Awake.
        private void Start() => Refresh();

        private void Buy()
        {
            if (controller.Buy(generatorIndex)) Refresh();
        }

        private void Refresh()
        {
            Generator generator = controller.Generators[generatorIndex];
            costLabel.text = generator.NextCost.ToString();
            ownedLabel.text = generator.Owned.ToString();
        }
    }
}
