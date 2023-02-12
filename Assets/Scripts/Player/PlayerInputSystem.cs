using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInputSystem : MonoBehaviour, ICharacterComander
{
    private const float CameraSpeed = GameSettings.UI.CameraMoveSpeed;

    private Coroutine _cameraMover;
    private Camera _camera;
    private RaycastHit _hitTemp;
    private PlayerInput _input;

    public event Action<Target> ChoosedTarget;
    public event Action<Character> ChoosedCharacter;

    private void Awake()
    {
        _camera = Camera.main;
        _input = new PlayerInput();
    }

    private void OnEnable()
    {
        _input.Enable();
        _input.Player.PlaceTap.performed += ctx => ComandAction();
        _input.Player.ControlTap.performed += ctx => SelectAction();
        _cameraMover = StartCoroutine(CameraMover());
    }

    private void OnDisable()
    {
        _input.Disable();
        _input.Player.PlaceTap.performed -= ctx => ComandAction();
        _input.Player.ControlTap.performed -= ctx => SelectAction();
        StopCoroutine(_cameraMover);
    }

    private IEnumerator CameraMover()
    {
        while (true)
        {
            Vector2 direction = _input.Player.CameraMoove.ReadValue<Vector2>();

            if (direction.sqrMagnitude > 0.1)
            {
                float scaleMoveSpeed = CameraSpeed * Time.deltaTime;
                Vector3 targetPosition = new Vector3(direction.x, 0, direction.y);
                _camera.transform.position += targetPosition * scaleMoveSpeed;
            }

            yield return null;
        }
    }

    private void ComandAction()
    {
        if (CheckMouseRay())
        {
            if (_hitTemp.collider.gameObject.TryGetComponent<Walkable>(out Walkable place))
                ChoosedTarget?.Invoke(new(_hitTemp.point));
            else if (_hitTemp.collider.gameObject.TryGetComponent<Character>(out Character enemy))
                ChoosedTarget?.Invoke(new(_hitTemp.point, enemy));
        }
    }

    private void SelectAction()
    {
        if (CheckMouseRay())
        {
            _hitTemp.collider.gameObject.TryGetComponent<Character>(out Character character);
            ChoosedCharacter?.Invoke(character);           
        }
    }

    private bool CheckMouseRay()
    {
        if (EventSystem.current.IsPointerOverGameObject() == false)
        {
            Ray tapRay = _camera.ScreenPointToRay(Input.mousePosition);
            return Physics.Raycast(tapRay, out _hitTemp, GameSettings.HitingRange);
        }
        else
        {
            return false;
        }
    }
}
