using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class CharacterIndicatorsPanel : MonoBehaviour
{
    [SerializeField] private Slider _hitPointsBar;
    [SerializeField] private Slider _manaPointsBar;
    [SerializeField] private Image _flag;

    private Transform _mainCameraTransform;
    private Canvas _mainCanvas;

    public void SetFlagColor(Color color)
    {
        _flag.color = color;
    }

    public void SetCurrentIndicators(float hitPointsCoefficient, float manaPointsCoefficient)
    {
        _hitPointsBar.value = hitPointsCoefficient;
        _manaPointsBar.value = manaPointsCoefficient;
    }

    private void Awake()
    {
        TryGetComponent<Canvas>(out _mainCanvas);
        _mainCanvas.worldCamera = Camera.main;
        _mainCameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        transform.rotation = _mainCameraTransform.rotation;
    }
}
