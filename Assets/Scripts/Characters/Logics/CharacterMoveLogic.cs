using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CharacterMoveLogic: IReachLogic
{    
    private NavMeshAgent _moveAgent;
    private Transform _body;
    private Team _team;
    private IFightebel _targetEnemy;
    private ToPoint _pointChecker;
    private ToAlly _allyChecker;
    private ToEnemy _enemyChecker;
    private ReachingChecker _currentChecker;
    private float _distance;        

    public event Action<Target> Reached;

    public CharacterMoveLogic(NavMeshAgent moveAgent, Transform body, Team team)
    {
        _moveAgent = moveAgent;
        _moveAgent.enabled = true;
        _body = body;
        _team = team;
        _pointChecker = new ToPoint(moveAgent);
        _allyChecker = new ToAlly(moveAgent);
        _enemyChecker = new ToEnemy(_distance, moveAgent);
        _currentChecker = _pointChecker;
    }

    public void SetTarget(Target target)
    {
        if (target.TryGetFightebel(out _targetEnemy))
            if (_targetEnemy.CheckFriendly(_team))
                _currentChecker = _allyChecker;
            else
                _currentChecker = _enemyChecker;
        else
            _currentChecker = _pointChecker;

        _currentChecker.SetTarget(target, _body.position);
    }

    public void SetNewDistanceToTarget(float distance)
    {
        _distance = distance;
        _enemyChecker.SetDistance(distance);
    }

    public void SetMoveSpeed(float moveSpeed)
    {
        _moveAgent.speed = Mathf.Max(0, moveSpeed);
    }

    public IEnumerator ReachTarget()
    {
        while(_currentChecker.CheckReaching(_body.position))
        {                       
            yield return GameSettings.Character.OptimizationDelay();
        }

        Reached!.Invoke(new Target(_body.position, _targetEnemy));
    }

    private abstract class ReachingChecker
    {
        protected float _distance;
        protected NavMeshAgent _moveAgent;
        protected Vector3 _currentTargetPoint;

        public abstract bool CheckReaching(Vector3 currentPosition);
        
        public abstract void SetTarget(Target target, Vector3 currentPosition);

        public virtual void SetDistance(float distance)
        {
            _distance = Mathf.Max(0, distance);
        }

        protected void SetNewTargetPointToAgent(Vector3 currentPosition, Vector3 targetPosition)
        {
            Vector3 newTarget = Vector3.MoveTowards(targetPosition, currentPosition, _distance);
            _currentTargetPoint = newTarget;
            _moveAgent.SetDestination(newTarget);
        }
    }

    private class ToPoint : ReachingChecker
    {                
        public ToPoint(NavMeshAgent moveAgent)
        {
            _moveAgent = moveAgent;
        }

        public override bool CheckReaching(Vector3 currentPosition)
        {
            return GameSettings.CheckCorrespondencePositions(currentPosition, _currentTargetPoint) == false;
        }

        public override void SetTarget(Target target, Vector3 currentPosition)
        {
            SetNewTargetPointToAgent(currentPosition, target.CurrentPosition());
        }
    }

    private class ToAlly : ReachingChecker
    {
        private IFightebel _ally;

        public ToAlly(NavMeshAgent moveAgent)
        {
            _moveAgent = moveAgent;
            _distance = GameSettings.Character.SocialDistance;
        }

        public override bool CheckReaching(Vector3 currentPosition)
        {
            if (_ally == null)            
                return false;            
            else            
                SetNewTargetPointToAgent(currentPosition, _ally.CurrentPosition);                            

            return true;
        }

        public override void SetTarget(Target target, Vector3 currentPosition)
        {
            target.TryGetFightebel(out _ally);
            SetNewTargetPointToAgent(currentPosition, target.CurrentPosition());
        }
    }

    private class ToEnemy : ReachingChecker
    {
        private IFightebel _enemy;        
        
        public ToEnemy(float distance, NavMeshAgent moveAgent)
        {
            _distance = distance;
            _moveAgent = moveAgent;
        }

        public override bool CheckReaching(Vector3 currentPosition)
        {
            if(GameSettings.CheckCorrespondencePositions(currentPosition, _currentTargetPoint) || _enemy == null)
            {
                return false;
            }
            else
            {
                SetNewTargetPointToAgent(currentPosition, _enemy.CurrentPosition);
                return true;
            }
        }

        public override void SetTarget(Target target, Vector3 currentPosition)
        {
            target.TryGetFightebel(out _enemy);
            SetNewTargetPointToAgent(currentPosition, _enemy.CurrentPosition);
        }
    }
}
