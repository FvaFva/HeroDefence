using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class ContentViewersSizeCorrector : MonoBehaviour
{
    [SerializeField] private float _placeWidth;
    [SerializeField] private float _placeHeight;

    private float _maxViewerSize;
    private float _squarePlace;
    private GridLayoutGroup _contentGroup;

    public void UpdateViewersSize(int countsUsedViewers)
    {
        if (countsUsedViewers == 0)
            return;

        float meanViewerSquare = _squarePlace / countsUsedViewers;
        float meanViewerSide = Mathf.Sqrt(meanViewerSquare);
        int realViewerSideCoefficient = Mathf.CeilToInt(_maxViewerSize / meanViewerSide);
        float newSize = _maxViewerSize / realViewerSideCoefficient;
        _contentGroup.cellSize = new Vector2(newSize, newSize);
    }

    private void Start()
    {
        TryGetComponent<GridLayoutGroup>(out _contentGroup);
        _maxViewerSize = Mathf.Min(_placeWidth, _placeHeight);
        _squarePlace = _placeWidth * _placeHeight;
    }
}
