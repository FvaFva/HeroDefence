using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;

public class MainUi : MonoBehaviour
{
    [SerializeField] private CharacterViewer _currentCharacterViewer;
    [SerializeField] private SelectedCharactersPanel _selectedCharacters;

    public UnityEvent<Character> OnCharacterSelect;
    
    private void Awake()
    {
        _currentCharacterViewer.Render(null);
        _currentCharacterViewer.SetMainTarget(true);
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
        _currentCharacterViewer.Render(character);

        if (isNewInPool)
        {
            _selectedCharacters.RenderCharacter(character);
        }
    }

    public void ClearAllCharacters()
    {
        _currentCharacterViewer.Render(null);
        _selectedCharacters.ClearSelectedChaViewers();
    }

    private void SetMainCharacter(Character character)
    {
        OnCharacterSelect.Invoke(character);
    }
}
