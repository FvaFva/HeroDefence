using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MoveSistem
{
    private NavMeshAgent _moveAgent;
    private Transform _body;
    private Vector3 _targetPoint;
    private Vector3 _currentTargetPoint;
    private Character _targetEnemy;
    private float _moveSpeed;
    private float _atackDistance;
    private Vector3 _currentPosition;
    private Vector3 _currentEnemyPosition;
    private float _attackDistanceDelta = 0.1f;
    private WaitForSeconds _moveDelay = new WaitForSeconds(0.1f);
    private float _rotationSpeed;
    private Quaternion _targetRotation;

    public bool IsMove { get; private set; }

    public MoveSistem(NavMeshAgent moveAgent, Transform body, float atackDistance)
    {
        _moveAgent = moveAgent;
        _moveAgent.enabled = true;
        _rotationSpeed = moveAgent.angularSpeed;
        _body = body;
        _moveSpeed = 0;
        _atackDistance = atackDistance;
        IsMove = false;
    }

    public void SetTarget(Character enemy)
    {
        _targetEnemy = enemy;
        _targetPoint = _body.position;
    }   

    public void SetTarget(Vector3 point)
    {
        _targetPoint = point;
        _targetEnemy = null;
    }

    public void SetMoveSpeed(float moveSpeed)
    {
        _moveSpeed = Mathf.Max(0, moveSpeed);
        _moveAgent.speed = _moveSpeed;
        _moveAgent.acceleration = _moveSpeed;
    }

    public IEnumerator UpdatePathToTarget()
    {
        while(true)
        {
            _currentPosition = _body.position;            

            if (_targetEnemy != null)
            {
                _currentEnemyPosition = _targetEnemy.transform.position;

                if (Vector3.Distance(_currentPosition, _currentEnemyPosition) > _atackDistance + _attackDistanceDelta)
                {
                    Vector3 newTarget = Vector3.MoveTowards(_currentEnemyPosition, _currentPosition, _atackDistance);
                    SetNewTargetPointToAgent(newTarget);
                }
                else
                {
                    _targetRotation = Quaternion.LookRotation(_currentEnemyPosition - _currentPosition);                    
                    _body.rotation = Quaternion.Slerp(_body.rotation, _targetRotation, _rotationSpeed * Time.deltaTime);
                }
            }            
            else if (_currentTargetPoint != _targetPoint)
            {
                SetNewTargetPointToAgent(_targetPoint);
            }

            IsMove = _currentTargetPoint != _currentPosition;

            yield return _moveDelay;
        }
    }

    private void SetNewTargetPointToAgent(Vector3 newTarget)
    {
        _currentTargetPoint = newTarget;
        _moveAgent.SetDestination(newTarget);
    }
}
