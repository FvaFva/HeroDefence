using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Image))]
public class ItemViewer : MonoBehaviour
{
    private Image _icon;
    private Sprite _baseIcaon;
    [SerializeField] private TMP_Text _rarity;
    [SerializeField] private TMP_Text _level;

    public bool IsUsed;

    private void Awake()
    {
        TryGetComponent<Image>(out _icon);
        _baseIcaon = _icon.sprite;
    }

    public void DrowItem(Item item)
    {
        if (item == null)
        { 
            _icon.sprite = _baseIcaon;
            IsUsed = false;
        }
        else
        { 
            _icon.sprite = item.Icon;
            IsUsed = true;
        }
    }
}
