using UnityEngine;
using System.Collections.Generic;
using System;

public class AmmunitionViewer : MonoBehaviour
{
    private Dictionary<ItemType, ItemViewer> _ammunition = new Dictionary<ItemType, ItemViewer>();

    [SerializeField] private ItemViewer _weaponView;
    [SerializeField] private ItemViewer _ringView;
    [SerializeField] private ItemViewer _necleView;
    [SerializeField] private ItemViewer _helmView;
    [SerializeField] private ItemViewer _chestView;
    [SerializeField] private ItemViewer _handView;
    [SerializeField] private ItemViewer _legView;

    public event Action<Item> ChoseItem;

    private void Awake()
    {
        LoadAmmunitionView();
    }

    private void OnEnable()
    {
        ChangeListeningAmmunition(true);
    }

    private void OnDisable()
    {
        ChangeListeningAmmunition(false);
    }

    public void DrowThingsWorn(IReadOnlyDictionary<ItemType, Item> things)
    {
        foreach (KeyValuePair<ItemType, ItemViewer> cell in _ammunition)
            if(things.ContainsKey(cell.Key))
                cell.Value.DrowItem(things[cell.Key]);
            else
                cell.Value.DrowItem(null);
    }

    public void Clear()
    {
        foreach (var cell in _ammunition)
            cell.Value.DrowItem(null);
    }

    private void LoadAmmunitionView()
    {
        _ammunition.Add(ItemType.Weapon, _weaponView);
        _ammunition.Add(ItemType.Ring, _ringView);
        _ammunition.Add(ItemType.Necle, _necleView);
        _ammunition.Add(ItemType.Helm, _helmView);
        _ammunition.Add(ItemType.Chest, _chestView);
        _ammunition.Add(ItemType.Hand, _handView);
        _ammunition.Add(ItemType.Leg, _legView);
    }

    private void ChangeListeningAmmunition(bool isListen)
    {
        if (isListen)
            foreach (ItemViewer cell in _ammunition.Values)
                cell.ChoseItem += OnItemShoose;
        else
            foreach (ItemViewer cell in _ammunition.Values)
                cell.ChoseItem -= OnItemShoose;
    }

    private void OnItemShoose(Item item)
    {
        ChoseItem?.Invoke(item);
    }
}
