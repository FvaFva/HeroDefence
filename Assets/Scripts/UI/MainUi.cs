using UnityEngine;
using System;
using System.Collections.Generic;

public class MainUi : MonoBehaviour
{
    [SerializeField] private CurrentCharacterInfoPanel _currentCharacter; 
    [SerializeField] private SelectedCharactersContent _selectedCharacters;
    [SerializeField] private InventoryViewer _inventory;
    [SerializeField] private ItemDetailedViewer _itemDetails;

    public event Action<Character> OnCharacterSelect;
    public event Action<Item, bool> ItemWearChanged;

    public void DrawInventory(IReadOnlyList<Item> bug)
    {
        _inventory.DrawInventory(bug);
    }   
    
    public void DrawCurrentCharacter(Character character, bool isNewInPool)
    {
        _currentCharacter.SetNewCurrentCharacter(character);
        _itemDetails.SetItem(null);

        if (isNewInPool)
        {
            _selectedCharacters.RenderCharacter(character);
        }
    }

    public void ClearAllCharacters()
    {
        _currentCharacter.SetNewCurrentCharacter(null);
        _selectedCharacters.ClearSelectedChaViewers();
    }

    private void OnEnable()
    {
        _selectedCharacters.OnCharacterSelect += SetMainCharacter;
        _inventory.ChoseItem += ShowPutOnItemDetails;
        _currentCharacter.ChoseAmmunitionsItem += ShowPutOffItemDetails;
        _itemDetails.ItemWearChanged += OnItemWearChanging;
    }

    private void OnDisable()
    {
        _selectedCharacters.OnCharacterSelect -= SetMainCharacter;
        _inventory.ChoseItem -= ShowPutOnItemDetails;
        _currentCharacter.ChoseAmmunitionsItem -= ShowPutOffItemDetails;
        _itemDetails.ItemWearChanged -= OnItemWearChanging;
    }

    private void SetMainCharacter(Character character)
    {
        OnCharacterSelect?.Invoke(character);
    }

    private void ShowPutOnItemDetails(Item item)
    {
        _itemDetails.SetItem(item, true);
    }

    private void ShowPutOffItemDetails(Item item)
    {
        _itemDetails.SetItem(item, false);
    }

    private void OnItemWearChanging(Item item, bool isPutOn)
    {
        ItemWearChanged?.Invoke(item, isPutOn);
    }
}
