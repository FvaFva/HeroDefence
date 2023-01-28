using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    [SerializeField] private MainUi _mainUI;
    [SerializeField] private Team _team;

    private Character _currentCharacter;
    private List<Character> _selectedPull = new List<Character>();
    private RaycastHit _hitTemp;
    private PlayerInput _input;
    private Camera _camera;
    private float _cameraSpeed = 10;
    private Coroutine _cameraMover;

    public Team Team => _team;

    private void Awake()
    {
        _input = new PlayerInput();
        _input.Player.PlaceTap.performed += ctx => ComandAction();
        _input.Player.ControlTap.performed += ctx => SelectAction();
        _camera = Camera.main;
    }

    private void OnEnable()
    {
        _input.Enable();
        _mainUI.OnCharacterSelect.AddListener(SetCurrentCharacter);
        _cameraMover = StartCoroutine(CameraMover());
    }

    private void OnDisable()
    {
        _input.Disable();
        _mainUI.OnCharacterSelect.RemoveListener(SetCurrentCharacter);
        StopCoroutine(_cameraMover);
    }

    private IEnumerator CameraMover()
    {
        while (true)
        {            
            Vector2 direction = _input.Player.CameraMoove.ReadValue<Vector2>();

            if (direction.sqrMagnitude > 0.1)
            {
                float scaleMoveSpeed = _cameraSpeed * Time.deltaTime;
                Vector3 targetPosition = new Vector3(direction.x, 0, direction.y);
                _camera.transform.position += targetPosition * scaleMoveSpeed;
            }

            yield return null;
        }
    }

    private void ComandAction()
    {      
        if (_currentCharacter != null && CheckMouseRay())
        {
            if (_hitTemp.collider.gameObject.TryGetComponent<Walkable>(out Walkable place))
                SetCharactersTargetPoint();
            else if (_hitTemp.collider.gameObject.TryGetComponent<Character>(out Character enemy))
                SetCharactersTargetEnemy(enemy);
        }
    }

    private void SetCharactersTargetEnemy(Character enemy)
    {
        foreach(Character character in _selectedPull)
            character.SetNewTarget(enemy);
    }

    private void SetCharactersTargetPoint()
    {
        Vector3 hitPoint = _hitTemp.point;
        int coutCharacters = _selectedPull.Count;
        int formationInRow = (int)Mathf.Sqrt(coutCharacters);
        int inCurrenRow = 0;
        float startX = hitPoint.x;

        foreach (Character character in _selectedPull)
        {
            character.SetNewTarget(hitPoint);
            hitPoint.x++;
            inCurrenRow++;

            if (inCurrenRow == formationInRow)
            {
                inCurrenRow = 0;
                hitPoint.z++;
                hitPoint.x = startX;
            }
        }
    }

    private void SelectAction()
    {
        if(CheckMouseRay())
        {         
            if (_hitTemp.collider.gameObject.TryGetComponent<Character>(out Character character))
            {
                SetCurrentCharacter(character);
            }
            else
            {
                ClearSelectedCharacters();
            }
        }
    }

    private void ClearSelectedCharacters()
    {
        _selectedPull.Clear();
        _currentCharacter = null;
        _mainUI.ClearAllCharacters();
    }

    private void SetCurrentCharacter(Character character)
    {
        if(character != null && character != _currentCharacter)
        { 
            _currentCharacter = character;
            bool isNewInPool = _selectedPull.Contains(_currentCharacter) == false;
            _mainUI.DrawCurrentCharacter(character, isNewInPool);

            if (isNewInPool)
                _selectedPull.Add(_currentCharacter);
        }
    }

    private bool CheckMouseRay()
    {
        if (EventSystem.current.IsPointerOverGameObject() == false)
        {
            Ray tapRay = _camera.ScreenPointToRay(Input.mousePosition);
            int RayDistance = 100;
            return Physics.Raycast(tapRay, out _hitTemp, RayDistance);
        }
        else
        {
            return false;
        }
    }
}
