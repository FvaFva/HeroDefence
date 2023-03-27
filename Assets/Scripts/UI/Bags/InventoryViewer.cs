using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class InventoryViewer : MonoBehaviour
{
    [SerializeField] private ItemViewer _tempViewer;

    private List<ItemViewer> _viewersPool = new List<ItemViewer>();

    public event Action<Item> ChoseItem;

    public void DrowInventory(IReadOnlyList<Item> bug)
    {
        Clear();

        foreach (Item item in bug)
            GetFreeViewer().DrowItem(item);
    }

    public void Clear()
    {
        foreach(ItemViewer cell in _viewersPool)
        {
            cell.DrowItem(null);
            cell.ChoseItem -= OnItemChoose;
        }
    }

    private ItemViewer GetFreeViewer()
    {
        ItemViewer freeCell = _viewersPool.Where(cell => cell.IsUsed == false).FirstOrDefault();

        if (freeCell == null)
        {
            freeCell = Instantiate(_tempViewer, transform);
            _viewersPool.Add(freeCell);
        }

        freeCell.ChoseItem += OnItemChoose;
        return freeCell;
    }

    private void Awake()
    {
        for (int i = 0; i < GameSettings.PlayerBagSize; i++)
            _viewersPool.Add(Instantiate(_tempViewer, transform));
    }

    private void OnItemChoose(Item item)
    {
        ChoseItem?.Invoke(item);
    }
}
