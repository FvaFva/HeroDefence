using System;
using System.Collections;
using UnityEngine;

public class CharacterVisionLogic : IReachLogic
{
    private float _distance;
    private float _rotationSpeed;

    private IFightebel _target;
    private Transform _body;
    private Quaternion _targetRotation;
    private Vector3 _currentPosition;

    public event Action<Target> Reached;

    public CharacterVisionLogic(Transform body, float rotationSpeed)
    {
        _body = body;
        _rotationSpeed = rotationSpeed;
    }

    public void SetVisionDistance(float distance)
    {        
        _distance = Mathf.Clamp(distance, 0, float.MaxValue);
    }

    public IEnumerator ReachTarget()
    {
        _currentPosition = _body.position;

        while (Vector3.Distance(_currentPosition, _target.CurrentPosition) < _distance + GameSettings.Character.RangeDelta)
        {
            _currentPosition = _body.position;
            _targetRotation = Quaternion.LookRotation(_target.CurrentPosition - _currentPosition);
            _body.rotation = Quaternion.Slerp(_body.rotation, _targetRotation, _rotationSpeed * Time.deltaTime);
            yield return GameSettings.Character.OptimizationDelay();
        }

        Reached?.Invoke(new Target(_body.position, _target));
    }

    public void SetTarget(Target target)
    {
        target.TryGetFightebel(out _target);
    }
}
