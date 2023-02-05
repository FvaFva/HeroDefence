using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class ContentViewerSezer : MonoBehaviour
{
    [SerializeField] protected float _plaseWidth;
    [SerializeField] protected float _plaseHeight;

    protected float _maxViewerSize;
    protected float _maxLeghtSide;
    protected float _squarePlase;
    protected GridLayoutGroup _contentGroup;

    private void Start()
    {
        TryGetComponent<GridLayoutGroup>(out _contentGroup);
        _maxViewerSize = Mathf.Min(_plaseWidth, _plaseHeight);
        _maxLeghtSide = Mathf.Max(_plaseWidth, _plaseHeight);
        _squarePlase = _plaseWidth * _plaseHeight;
    }

    public void UpdateViewersSize(int countsUsedViewers)
    {
        if (countsUsedViewers == 0)
            return;

        float meanViewerSquare = _squarePlase / countsUsedViewers;
        float meanViewerSide = Mathf.Sqrt(meanViewerSquare);
        int realCiewerSideCoefficient = Mathf.CeilToInt(_maxViewerSize / meanViewerSide);
        float newSize = _maxViewerSize / realCiewerSideCoefficient;
        
        _contentGroup.cellSize = new Vector2(newSize, newSize);
    }
}
