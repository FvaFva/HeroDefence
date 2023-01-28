using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class CharacterInformation : MonoBehaviour
{
    [SerializeField] private Slider _hitPointsBar;
    [SerializeField] private Slider _manaPointsBar;

    private Canvas _mainCanvas;

    private void Awake()
    {
        TryGetComponent<Canvas>(out _mainCanvas);
        _mainCanvas.worldCamera = Camera.main;
    }

    public void SetCurrentCharacteristics(float hitPointsCoeffecient, float manaPointsCoeffecient)
    {
        _hitPointsBar.value = hitPointsCoeffecient;
        _manaPointsBar.value = manaPointsCoeffecient;
    }
}
