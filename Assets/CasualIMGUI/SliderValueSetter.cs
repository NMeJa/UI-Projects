using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CasualIMGUI
{
    public class SliderValueSetter : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private TMP_Text text;

        private void Awake()
        {
            slider.onValueChanged.AddListener(OnValueChanged);
            OnValueChanged(35);
        }

        private void OnValueChanged(float value)
        {
            text.text = $"{value.ToString(CultureInfo.InvariantCulture)}/{slider.maxValue}";
        }
    }
}