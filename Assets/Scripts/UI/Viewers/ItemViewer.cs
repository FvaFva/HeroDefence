using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

[RequireComponent(typeof(Image))]
public class ItemViewer : MonoBehaviour
{
    private Image _icon;
    private Sprite _baseIcaon;
    private Item _item;

    [SerializeField] private TMP_Text _rarity;
    [SerializeField] private TMP_Text _level;
    [SerializeField] private UIPanel _panelLevel;
    [SerializeField] private UIPanel _panelRarity;
    [SerializeField] private Button _button;

    public event Action<Item> ChoseItem;
    public bool IsUsed { get; private set; }

    public void DrowItem(Item item)
    {
        _item = item;

        if (_item == null)
        { 
            _icon.sprite = _baseIcaon;           
            IsUsed = false;
        }
        else
        { 
            _icon.sprite = item.Icon;
            IsUsed = true;
        }

        ChangeElementsVision();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnButtonClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        ChoseItem?.Invoke(_item);
    }

    private void Awake()
    {
        TryGetComponent<Image>(out _icon);
        _baseIcaon = _icon.sprite;
        ChangeElementsVision();
    }

    private void ChangeElementsVision()
    {
        _rarity.enabled = IsUsed;
        _level.enabled = IsUsed;
        _panelLevel.ChangeVision(IsUsed);
        _panelRarity.ChangeVision(IsUsed);
    }
}
