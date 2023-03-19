using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInputSystem))]
public class Player : MonoBehaviour
{
    [SerializeField] private MainUi _mainUI;
    [SerializeField] private Team _team;

    private Inventory _inventory = new Inventory();
    private Character _currentCharacter;
    private List<Character> _selectedPull = new List<Character>();
    private bool _isControllabilityCharacter;
    private PlayerInputSystem _playerInputSystem;

    public void GetCurrentCharacterItem(Item item)
    {
        if(item != null && _currentCharacter != null)
            if(_inventory.TryDropItem(item))
                if(_currentCharacter.TryPutOnItem(item) == false)
                    _inventory.TryTakeItem(item);
    }

    public void StreapCurrentCharacterSlot(ItemType slot)
    {
        if (_currentCharacter != null && _currentCharacter.TryDropItem(slot, out Item dropItem))
            if (_inventory.TryTakeItem(dropItem) == false)
                _currentCharacter.TryPutOnItem(dropItem);
    }

    private void Awake()
    {
        TryGetComponent(out _playerInputSystem);
    }

    private void OnEnable()
    {
        _mainUI.OnCharacterSelect += SetCurrentCharacter;
        _playerInputSystem.ChoosedCharacter += SetCurrentCharacter;
    }

    private void OnDisable()
    {
        _mainUI.OnCharacterSelect -= SetCurrentCharacter;
        _playerInputSystem.ChoosedCharacter -= SetCurrentCharacter;
    }   

    private void ClearSelectedCharacters()
    {
        if(_selectedPull.Count == 0 && _currentCharacter == null)
            return;

        foreach (Character character in _selectedPull)
            character.SetNewComander(null);

        _selectedPull.Clear();
        _currentCharacter = null;
        _mainUI.ClearAllCharacters();
    }

    private void SetCurrentCharacter(Character character)
    {
        if(character != null && character != _currentCharacter)
        {
            UpdateCharacterControllability(character);

            _currentCharacter = character;

            if (_isControllabilityCharacter)
                _currentCharacter.SetNewComander(_playerInputSystem);

            bool isNewInPool = _selectedPull.Contains(_currentCharacter) == false;
            _mainUI.DrawCurrentCharacter(character, isNewInPool);

            if (isNewInPool)
                _selectedPull.Add(_currentCharacter);            
        }
        else if(character == null)
        {
            ClearSelectedCharacters();
        }
    }

    private void UpdateCharacterControllability(Character character)
    {
        bool newCharacterControllability = character.IsFriendly(_team);

        if (_isControllabilityCharacter == false || newCharacterControllability == false)
            ClearSelectedCharacters();

        _isControllabilityCharacter = newCharacterControllability;
    }
}
