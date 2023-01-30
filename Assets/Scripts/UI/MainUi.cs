using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;

public class MainUi : MonoBehaviour
{
    [SerializeField] private CurrentCharacterInfoPanel _currentCharacter; 
    [SerializeField] private SelectedCharactersContent _selectedCharacters;

    public UnityEvent<Character> OnCharacterSelect;
    
    private void Awake()
    {
        _currentCharacter.SetNewCurrentCharacter(null);
    }

    private void OnEnable()
    {
        _selectedCharacters.OnCharacterSelect.AddListener(SetMainCharacter);
    }

    private void OnDisable()
    {
        _selectedCharacters.OnCharacterSelect.RemoveListener(SetMainCharacter);
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
        OnCharacterSelect.Invoke(character);
    }
}
