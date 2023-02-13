using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CharacterMoveLogic: IReachLogic
{
    private NavMeshAgent _moveAgent;
    private Transform _body;
    private IFightebel _targetEnemy;
  
    private float _distance;
        
    private Vector3 _currentTargetPoint;
    private bool _isTargetMovebel;

    public event Action Reached;
    public event Action Failed;

    public CharacterMoveLogic(NavMeshAgent moveAgent, Transform body)
    {
        _moveAgent = moveAgent;
        _moveAgent.enabled = true;        
        _body = body;       
    }

    public void SetTarget(Target target)
    {
        _isTargetMovebel = target.TryGetFightebel(out _targetEnemy);

        if (_isTargetMovebel)
            SetNewTargetPointToAgent();
        else
            SetNewTargetPointToAgent(target.CurrentPosition());
    }

    public void SetNewDistanceToTarget(float distance)
    {
        _distance = distance;
    }

    public void SetMoveSpeed(float moveSpeed)
    {
        _moveAgent.speed = Mathf.Max(0, moveSpeed);
    }

    public IEnumerator ReachTarget()
    {
        while(_currentTargetPoint != _body.position)
        {           
            if (_isTargetMovebel)
            {               
                SetNewTargetPointToAgent();
            }

            yield return GameSettings.Character.OptimizationDelay();
        }

        Reached!.Invoke();
    }

    private void SetNewTargetPointToAgent(Vector3 newTarget)
    {
        _currentTargetPoint = newTarget;
        _moveAgent.SetDestination(newTarget);
    }

    private void SetNewTargetPointToAgent()
    {
        Vector3 newTarget = Vector3.MoveTowards(_targetEnemy.CurrentPosition, _body.position, _distance);
        _currentTargetPoint = newTarget;
        _moveAgent.SetDestination(newTarget);
    }
}
