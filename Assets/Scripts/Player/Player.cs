using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInputSystem))]
public class Player : MonoBehaviour
{
    [SerializeField] private MainUi _mainUI;
    [SerializeField] private Team _team;
    [SerializeField] private Shop _shop;

    private Inventory _inventory = new Inventory();
    private Character _currentCharacter;
    private List<Character> _selectedPull = new List<Character>();
    private bool _isControllabilityCharacter;
    private PlayerInputSystem _playerInputSystem;

    private void OnEnable()
    {
        _mainUI.OnCharacterSelect += SetCurrentCharacter;
        _mainUI.ItemWearChanged += OnItemWearChange;
        _playerInputSystem.ChoseCharacter += SetCurrentCharacter;
        _shop.SoldItem += BuyItem;
    }

    private void OnDisable()
    {
        _mainUI.OnCharacterSelect -= SetCurrentCharacter;
        _mainUI.ItemWearChanged -= OnItemWearChange;
        _playerInputSystem.ChoseCharacter -= SetCurrentCharacter;
        _shop.SoldItem -= BuyItem;
    }

    private void GetCurrentCharacterItem(Item item)
    {
        if (item != null && _currentCharacter != null)
        {
            if (_inventory.TryDropItem(item))
            {
                if (_currentCharacter.TryPutOnItem(item) == false)
                    _inventory.TryTakeItem(item);
                else
                    _mainUI.DrawInventory(_inventory.Bug);
            }
        }
    }

    private void StripCurrentCharacterSlot(ItemType slot)
    {
        if (_currentCharacter != null && _currentCharacter.TryDropItem(slot, out Item dropItem))
        {
            if (_inventory.TryTakeItem(dropItem) == false)
                _currentCharacter.TryPutOnItem(dropItem);
            else
                _mainUI.DrawInventory(_inventory.Bug);
        }
    }

    private bool TakeItem(Item item)
    {
        if (_inventory.TryTakeItem(item))
        {
            _mainUI.DrawInventory(_inventory.Bug);
            return true;
        }

        return false;
    }

    private void Awake()
    {
        TryGetComponent(out _playerInputSystem);
    }

    private void BuyItem(Item item)
    {
        TakeItem(item);
    }

    private void ClearSelectedCharacters()
    {
        if (_selectedPull.Count == 0 && _currentCharacter == null)
            return;

        foreach (Character character in _selectedPull)
            character.SetNewCommander(null);

        _selectedPull.Clear();
        _currentCharacter = null;
        _mainUI.ClearAllCharacters();
    }

    private void SetCurrentCharacter(Character character)
    {
        if (character != null && character != _currentCharacter)
        {
            UpdateCharacterControllability(character);

            _currentCharacter = character;

            if (_isControllabilityCharacter)
                _currentCharacter.SetNewCommander(_playerInputSystem);

            bool isNewInPool = _selectedPull.Contains(_currentCharacter) == false;
            _mainUI.DrawCurrentCharacter(character, isNewInPool);

            if (isNewInPool)
                _selectedPull.Add(_currentCharacter);
        }
        else if (character == null)
        {
            ClearSelectedCharacters();
        }
    }

    private void OnItemWearChange(Item item, bool isPutOn)
    {
        if (isPutOn)
            GetCurrentCharacterItem(item);
        else
            StripCurrentCharacterSlot(item.ItemType);
    }

    private void UpdateCharacterControllability(Character character)
    {
        bool newCharacterControllability = character.IsFriendly(_team);

        if (_isControllabilityCharacter == false || newCharacterControllability == false)
            ClearSelectedCharacters();

        _isControllabilityCharacter = newCharacterControllability;
    }
}
