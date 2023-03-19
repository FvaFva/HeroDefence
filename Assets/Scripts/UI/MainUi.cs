using UnityEngine;
using System;

public class MainUi : MonoBehaviour
{
    [SerializeField] private CurrentCharacterInfoPanel _currentCharacter; 
    [SerializeField] private SelectedCharactersContent _selectedCharacters;

    public event Action<Character> OnCharacterSelect;

    private void OnEnable()
    {
        _selectedCharacters.OnCharacterSelect += SetMainCharacter;
    }

    private void OnDisable()
    {
        _selectedCharacters.OnCharacterSelect -= SetMainCharacter;
    }

    public void DrawCurrentCharacter(Character character, bool isNewInPool)
    {
        _currentCharacter.SetNewCurrentCharacter(character);

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
}
