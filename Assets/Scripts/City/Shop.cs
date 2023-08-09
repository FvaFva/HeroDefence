using System;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] private Button _shopBut;
    [SerializeField] private ItemFactory _factory;

    public event Action<Item> SoldItem;

    private void OnDisable()
    {
        _shopBut.onClick.RemoveListener(SellItem);
    }

    private void OnEnable()
    {
        _shopBut.onClick.AddListener(SellItem);
    }

    private void SellItem()
    {
        SoldItem?.Invoke(_factory.GetRandomItem());
    }
}
