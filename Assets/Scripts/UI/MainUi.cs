using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using System;

public class MainUi : MonoBehaviour
{
    [SerializeField] private CurrentCharacterInfoPanel _currentCharacter; 
    [SerializeField] private SelectedCharactersContent _selectedCharacters;
    [SerializeField] private TempShop _tempShop;

    private Character _character;

    public event Action<Character> OnCharacterSelect;

    private void OnEnable()
    {
        _selectedCharacters.OnCharacterSelect += SetMainCharacter;
        _tempShop.SelledPerc += AddAbilityToCurrent;
    }

    private void OnDisable()
    {
        _selectedCharacters.OnCharacterSelect -= SetMainCharacter;
        _tempShop.SelledPerc -= AddAbilityToCurrent;
    }

    public void DrawCurrentCharacter(Character character, bool isNewInPool)
    {
        _currentCharacter.SetNewCurrentCharacter(character);
        _character = character;

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

    private void SetMainCharacter(Character character)
    {
        OnCharacterSelect?.Invoke(character);
    }

    private void AddAbilityToCurrent(Perc perc)
    {
        if(_character != null)
            _character.AddPerc(perc);
    }
}
