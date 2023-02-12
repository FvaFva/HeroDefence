using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CharacterMoveLogic: IReachLogic
{
    private NavMeshAgent _moveAgent;
    private Transform _body;
    private IFightebel _targetEnemy;
    private Quaternion _targetRotation;

    private float _moveSpeed;
    private float _distance;
    private float _rotationSpeed;
    
    private Vector3 _targetPoint;
    private Vector3 _currentTargetPoint;
    private Vector3 _currentPosition;

    public event Action Reached;
    public event Action Failed;

    public bool IsMove { get; private set; }

    public CharacterMoveLogic(NavMeshAgent moveAgent, Transform body)
    {
        _moveAgent = moveAgent;
        _moveAgent.enabled = true;
        _rotationSpeed = moveAgent.angularSpeed;
        _body = body;
        _moveSpeed = 0;
        IsMove = false;
    }

    public void SetTarget(Target target)
    {
        if (target.TryGetFightebel(out _targetEnemy))
            _targetPoint = _body.position;
        else
            _targetPoint = target.CurrentPosition();
    }

    public void SetNewDistanceToTarget(float distance)
    {
        _distance = distance;
    }

    public void SetMoveSpeed(float moveSpeed)
    {
        _moveSpeed = Mathf.Max(0, moveSpeed);
        _moveAgent.speed = _moveSpeed;
    }

    public IEnumerator ReachTarget()
    {
        while(true)
        {
            _currentPosition = _body.position;            

            if (_targetEnemy != null)
            {                
                if (Vector3.Distance(_currentPosition, _targetEnemy.CurrentPosition) > _distance + GameSettings.Character.RangeDelta)
                {
                    Vector3 newTarget = Vector3.MoveTowards(_targetEnemy.CurrentPosition, _currentPosition, _distance);
                    SetNewTargetPointToAgent(newTarget);
                }
                else
                {
                    _targetRotation = Quaternion.LookRotation(_targetEnemy.CurrentPosition - _currentPosition);                    
                    _body.rotation = Quaternion.Slerp(_body.rotation, _targetRotation, _rotationSpeed * Time.deltaTime);
                }
            }            
            else if (_currentTargetPoint != _targetPoint)
            {
                SetNewTargetPointToAgent(_targetPoint);
            }

            IsMove = _currentTargetPoint != _currentPosition;

            yield return GameSettings.Character.OptimizationDelay();
        }
    }

    private void SetNewTargetPointToAgent(Vector3 newTarget)
    {
        _currentTargetPoint = newTarget;
        _moveAgent.SetDestination(newTarget);
    }
}
