using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class ContentViewerSezer : MonoBehaviour
{
    [SerializeField] protected float _plaseWidth;
    [SerializeField] protected float _plaseHeight;

    protected float _maxViewerSize;
    protected float _maxSideLeght;
    protected GridLayoutGroup _contentGroup;

    private void Start()
    {
        TryGetComponent<GridLayoutGroup>(out _contentGroup);
        _maxViewerSize = Mathf.Min(_plaseWidth, _plaseHeight);
        _maxSideLeght = Mathf.Max(_plaseWidth, _plaseHeight);
    }

    public void UpdateViewersSize(int countsUsedViewers)
    {
        if (countsUsedViewers == 0)
            return;

        int countRow = Mathf.CeilToInt(_maxViewerSize * countsUsedViewers / _maxSideLeght);
        float newSize = _maxViewerSize / countRow;
        int countColum = Mathf.CeilToInt(countsUsedViewers / countRow);
        float freeSpaseInColum = _maxSideLeght - countColum * newSize;
        int countFilleredSpace = countColum + 1;
        float Spacing = freeSpaseInColum / countFilleredSpace;

        _contentGroup.padding.top = (int)Spacing;
        _contentGroup.padding.bottom = (int)Spacing;
        _contentGroup.spacing = new Vector2(_contentGroup.spacing.x, Spacing);
        _contentGroup.cellSize = new Vector2(newSize, newSize);
    }
}
