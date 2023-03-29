using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IndicatorSlider : MonoBehaviour
{
    [SerializeField] private TMP_Text _information;
    [SerializeField] private Slider _bar;

    private void OnEnable()
    {
        _bar.onValueChanged.AddListener(ChangeTextInfo);
    }

    private void OnDisable()
    {
        _bar.onValueChanged.RemoveListener(ChangeTextInfo);
    }

    public void SetMaxValue(float newValue)
    {
        float oldCoefficient = _bar.value / _bar.maxValue;
        _bar.maxValue = newValue;
        _bar.value = newValue * oldCoefficient;
    }

    public void SetCurrentCoefficient(float coeficient)
    {
        _bar.value = coeficient * _bar.maxValue;
    }

    public void Clear()
    { 
        _bar.value = 1;
        _bar.maxValue = 1;
    }

    private void ChangeTextInfo(float temp)
    {
        _information.text = $"{(int)_bar.value} / {(int)_bar.maxValue}";
    }
}
