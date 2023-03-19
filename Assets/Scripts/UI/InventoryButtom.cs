using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class InventoryButtom : MonoBehaviour
{
    [SerializeField] InventoryViewer _inventory;

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
        _inventory.ChangeVision();
    }
}
