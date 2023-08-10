using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetailedViewer : MonoBehaviour
{
    [SerializeField] private CharacteristicViewer _damage;
    [SerializeField] private CharacteristicViewer _attackSpeed;
    [SerializeField] private CharacteristicViewer _armor;
    [SerializeField] private CharacteristicViewer _hitPoints;
    [SerializeField] private CharacteristicViewer _speed;
    [SerializeField] private CharacteristicViewer _manaPoints;
    [SerializeField] private CharacteristicViewer _manaRegen;
    [SerializeField] private ItemViewer _itemViewer;
    [SerializeField] private AbilityViewer _abilityViewer;
    [SerializeField] private Button _left;
    [SerializeField] private TMP_Text _leftName;
    [SerializeField] private Button _right;

    private Item _currentItem;
    private bool _isItemPutOn;

    public event Action<Item, bool> ItemWearChanged;

    public void SetItem(Item item, bool isItemPutOn = false)
    {
        _currentItem = item;
        _isItemPutOn = isItemPutOn;
        _itemViewer.DrowItem(_currentItem);
        ChangeButtonsView();

        if (_currentItem == null)
            ClearView();
        else
            DrawCurrentItem();
    }

    private void OnEnable()
    {
        _left.onClick.AddListener(OnWearChanging);
    }

    private void OnDisable()
    {
        _left.onClick.RemoveListener(OnWearChanging);
    }

    private void ChangeButtonsView()
    {
        string leftText = "Put off";

        if (_isItemPutOn)
            leftText = "Put on";

        _leftName.text = leftText;
    }

    private void DrawCurrentItem()
    {
        DrawCharacteristics(_currentItem.GetCharacteristics());
        _abilityViewer.ShowAbility(_currentItem.Perk);
        _left.gameObject.SetActive(true);
        _right.gameObject.SetActive(true);
    }

    private void ClearView()
    {
        DrawCharacteristics(default(FighterCharacteristics));
        _abilityViewer.ShowAbility(null);
        _left.gameObject.SetActive(false);
        _right.gameObject.SetActive(false);
    }

    private void DrawCharacteristics(FighterCharacteristics characteristics)
    {
        _damage.ShowCharacteristic((int)characteristics.Damage);
        _attackSpeed.ShowCharacteristic((int)characteristics.AttackSpeed);
        _armor.ShowCharacteristic((int)characteristics.Armor);
        _hitPoints.ShowCharacteristic((int)characteristics.HitPoints);
        _speed.ShowCharacteristic((int)characteristics.Speed);
        _manaPoints.ShowCharacteristic((int)characteristics.ManaPoints);
        _manaRegen.ShowCharacteristic((int)characteristics.ManaRegen);
    }

    private void OnWearChanging()
    {
        ItemWearChanged?.Invoke(_currentItem, _isItemPutOn);
        SetItem(null);
    }
}
