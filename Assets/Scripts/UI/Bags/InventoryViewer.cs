using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(ContentViewerSezer))]
[RequireComponent(typeof(Content—oncealer))]
public class InventoryViewer : MonoBehaviour
{
    [SerializeField] private ItemViewer _tempViewer;

    private List<ItemViewer> _viewersPool = new List<ItemViewer>();
    private ContentViewerSezer _sizer;
    private bool _isVisibale = false;
    private Content—oncealer _Òoncealer;

    public void ChangeVision()
    {
        _isVisibale = !_isVisibale;
        _Òoncealer.StartMovePanel(_isVisibale);
    }

    public void DrowInventory(IReadOnlyList<Item> bug)
    {
        foreach(Item item in bug)
            GetFreeViewer().DrowItem(item);
    }

    public void Clear()
    {
        foreach(ItemViewer cell in _viewersPool)
        {
            cell.DrowItem(null);
            cell.enabled = false;
        }
    }

    private ItemViewer GetFreeViewer()
    {
        ItemViewer freeCell = _viewersPool.First(cell=>cell.IsUsed = false);

        if (freeCell == null)
        {
            freeCell = Instantiate(_tempViewer, transform);
            _viewersPool.Add(freeCell);
        }

        return freeCell;
    }

    private void Awake()
    {
        TryGetComponent<ContentViewerSezer>(out _sizer);
        TryGetComponent<Content—oncealer>(out _Òoncealer);
    }
}
