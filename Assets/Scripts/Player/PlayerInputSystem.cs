using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInputSystem : MonoBehaviour, ITargetChooser
{
    private const float CameraSpeed = GameSettings.UI.CameraMoveSpeed;

    private Coroutine _cameraMover;
    private Camera _camera;
    private RaycastHit _hitTemp;
    private PlayerInput _input;

    public event Action<Target> ChoseTarget;

    public event Action<Character> ChoseCharacter;

    private void Awake()
    {
        _camera = Camera.main;
        _input = new PlayerInput();
    }

    private void OnEnable()
    {
        _input.Enable();
        _input.Player.PlaceTap.performed += ctx => CommandAction();
        _input.Player.ControlTap.performed += ctx => SelectAction();
        _cameraMover = StartCoroutine(CameraMover());
    }

    private void OnDisable()
    {
        _input.Disable();
        _input.Player.PlaceTap.performed -= ctx => CommandAction();
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

    private void CommandAction()
    {
        if (CheckMouseRay())
        {
            if (_hitTemp.collider.gameObject.TryGetComponent<Walkable>(out Walkable place))
                ChoseTarget?.Invoke(new Target(_hitTemp.point));
            else if (_hitTemp.collider.gameObject.TryGetComponent<Character>(out Character enemy))
                ChoseTarget?.Invoke(new Target(_hitTemp.point, enemy));
        }
    }

    private void SelectAction()
    {
        if (CheckMouseRay())
        {
            _hitTemp.collider.gameObject.TryGetComponent<Character>(out Character character);
            ChoseCharacter?.Invoke(character);
        }
    }

    private bool CheckMouseRay()
    {
        if (EventSystem.current.IsPointerOverGameObject() == false)
        {
            Ray tapRay = _camera.ScreenPointToRay(Input.mousePosition);
            return Physics.Raycast(tapRay, out _hitTemp, GameSettings.HittingRange);
        }
        else
        {
            return false;
        }
    }
}
