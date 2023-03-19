using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class CharacterIndicatorsPanel : MonoBehaviour
{
    [SerializeField] private Slider _hitPointsBar;
    [SerializeField] private Slider _manaPointsBar;
    [SerializeField] private Image _flag;

    private Transform _mainCameraTransform;
    private Canvas _mainCanvas;

    private void Awake()
    {
        TryGetComponent<Canvas>(out _mainCanvas);
        _mainCanvas.worldCamera = Camera.main;
        _mainCameraTransform = Camera.main.transform;
    }

    public void SetFlagGolod(Color color)
    {
        _flag.color = color;
    }

    public void SetCurrentIndicators(float hitPointsCoeffecient, float manaPointsCoeffecient)
    {
        _hitPointsBar.value = hitPointsCoeffecient;
        _manaPointsBar.value = manaPointsCoeffecient;
    }

    private void Update()
    {
        transform.rotation = _mainCameraTransform.rotation;
    }
}
