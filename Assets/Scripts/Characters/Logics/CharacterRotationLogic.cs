using System;
using System.Collections;
using UnityEngine;

public class CharacterRotationLogic : IReachLogic, ICharacterComander
{
    private float _rotationSpeed;

    private IFightebel _target;
    private Transform _body;
    private Quaternion _targetRotation;
    private Vector3 _currentPosition;

    public event Action<Target> Reached;
    public event Action<Target> ChoosedTarget;

    public CharacterRotationLogic(Transform body, float rotationSpeed)
    {
        _body = body;
        _rotationSpeed = rotationSpeed;
    }

    public IEnumerator ReachTarget()
    {
        _currentPosition = _body.position;
        _targetRotation = Quaternion.LookRotation(_target.CurrentPosition - _currentPosition);

        while (CheckRotateToTerget(GameSettings.Character.AngleDelta) == false)
        {
            _currentPosition = _body.position;
            _targetRotation = Quaternion.LookRotation(_target.CurrentPosition - _currentPosition);
            _body.rotation = Quaternion.Slerp(_body.rotation, _targetRotation, _rotationSpeed * Time.deltaTime);
            yield return GameSettings.Character.OptimizationDelay();
        }

        Reached?.Invoke(new Target(_body.position, _target));
    }

    public IEnumerator ObserveRotateToTarget()
    {
        while (CheckRotateToTerget(GameSettings.Character.AttackAngle))
        {
            yield return GameSettings.Character.OptimizationDelay();
        }

        ChoosedTarget?.Invoke(new Target(_body.position, _target));
    }

    public void SetTarget(Target target)
    {
        target.TryGetFightebel(out _target);
    }

    private bool CheckRotateToTerget(float checkAgle)
    {
        checkAgle = Mathf.Clamp(checkAgle, -1.0f, 1.0f);
        Vector3 toTargetNormalize = _target.CurrentPosition - _currentPosition;
        toTargetNormalize.Normalize();
        bool isInAngle = Vector3.Dot(_body.forward, toTargetNormalize) > checkAgle;
        return isInAngle;
    }
}
