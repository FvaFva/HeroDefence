using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class InventoryButton : MonoBehaviour
{
    [SerializeField] private ContentConcealer _inventory;

    private bool _isVisible = false;
    private Button _openInventory;

    private void Awake()
    {
        TryGetComponent<Button>(out _openInventory);
    }

    private void OnEnable()
    {
        _openInventory.onClick.AddListener(ChangeVisionInventory);
    }

    private void OnDisable()
    {
        _openInventory.onClick.RemoveListener(ChangeVisionInventory);
    }

    private void ChangeVisionInventory()
    {
        _isVisible = !_isVisible;
        _inventory.StartMovePanel(_isVisible);
    }
}
