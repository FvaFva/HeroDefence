using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(PlayerInputSystem))]
public class Player : MonoBehaviour
{
    [SerializeField] private MainUi _mainUI;
    [SerializeField] private Team _team;

    private Character _currentCharacter;
    private List<Character> _selectedPull = new List<Character>();
    private bool _isControllabilityCharacter;
    private PlayerInputSystem _playerInputSystem;

    public string TeamName => _team.Name;
    public Color TeamFlag => _team.Flag;

    private void Awake()
    {
        TryGetComponent(out _playerInputSystem);
    }

    private void OnEnable()
    {
        _mainUI.OnCharacterSelect += SetCurrentCharacter;
        _playerInputSystem.ChoosedCharacter += SetCurrentCharacter;
        _playerInputSystem.ChoosedTarget += OnChoosNewTarget;
    }

    private void OnDisable()
    {
        _mainUI.OnCharacterSelect -= SetCurrentCharacter;
        _playerInputSystem.ChoosedCharacter -= SetCurrentCharacter;
        _playerInputSystem.ChoosedTarget -= OnChoosNewTarget;
    }   

    private void OnChoosNewTarget(Target target)
    {
        if (target.TryGetFightebel(out IFightebel enemy))
            SetCharactersTargetEnemy(enemy);
        else
            SetCharactersTargetPoint(target.CurrentPosition());
    }

    private void SetCharactersTargetEnemy(IFightebel enemy)
    {
       // foreach(Character character in _selectedPull)
            //character.SetNewTarget(enemy);
    }

    private void SetCharactersTargetPoint(Vector3 target)
    {
        int coutCharacters = _selectedPull.Count;
        int formationInRow = (int)Mathf.Sqrt(coutCharacters);
        int inCurrenRow = 0;
        float startX = target.x;

        foreach (Character character in _selectedPull)
        {
            //character.SetNewTarget(target);
            target.x++;
            inCurrenRow++;

            if (inCurrenRow == formationInRow)
            {
                inCurrenRow = 0;
                target.z++;
                target.x = startX;
            }
        }
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
        bool newCharacterControllability = character.CheckFriendly(_team);

        if (_isControllabilityCharacter == false || newCharacterControllability == false)
            ClearSelectedCharacters();

        _isControllabilityCharacter = newCharacterControllability;
    }
}
