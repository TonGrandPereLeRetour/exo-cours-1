using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Clicker.Settings
{
    public sealed class SettingsPanel : MonoBehaviour
    {
        [SerializeField] private SettingsController controller;
        [SerializeField] private Slider volumeSlider;
        [SerializeField] private TMP_Dropdown languageDropdown;
        [SerializeField] private string[] languages = { "fr", "en" };

        private void Start()
        {
            volumeSlider.SetValueWithoutNotify(controller.Volume);
            languageDropdown.SetValueWithoutNotify(Math.Max(0, Array.IndexOf(languages, controller.Language)));
            volumeSlider.onValueChanged.AddListener(controller.SetVolume);
            languageDropdown.onValueChanged.AddListener(index => controller.SetLanguage(languages[index]));
        }
    }
}
