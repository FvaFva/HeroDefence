using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CharacterMoveLogic: IReachLogic, ICharacterComander
{    
    private Transform _body;
    private Team _team;

    private ToPoint _pointChecker;
    private ToAlly _allyChecker;
    private ToEnemy _enemyChecker;
    private ReachingChecker _currentChecker;

    private IFightebel _target;
    private float _distance;

    public event Action<Target> Reached;
    public event Action<Target> ChoosedTarget;

    public CharacterMoveLogic(NavMeshAgent moveAgent, Transform body, Team team, float _height)
    {
        moveAgent.enabled = true;
        _body = body;
        _team = team;
        _pointChecker = new ToPoint(moveAgent, _height);
        _allyChecker = new ToAlly(moveAgent);
        _enemyChecker = new ToEnemy(_distance, moveAgent);
        _currentChecker = _pointChecker;
    }

    public void SetTarget(Target target)
    {
        if (target.TryGetFightebel(out _target))
            if (_target.IsFriendly(_team))
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
        _currentChecker.SetMoveSpeed(moveSpeed);
    }

    public IEnumerator ReachTarget()
    {        
        while (_currentChecker.CheckPathEnd(_body.position))
        {
            yield return GameSettings.Character.OptimizationDelay();
        }
        Reached?.Invoke(new Target(_body.position, _target));
    }

    public IEnumerator CheckTargetInRadius()
    {
        while (_currentChecker.CheckDistance(_body.position))
        {
            yield return GameSettings.Character.OptimizationDelay();
        }

        ChoosedTarget?.Invoke(new Target(_body.position, _target));
    }

    private abstract class ReachingChecker
    {
        protected float _distance;
        protected NavMeshAgent _moveAgent;
        protected Vector3 _currentTargetPoint;
        protected IFightebel _target;

        public virtual bool CheckPathEnd(Vector3 currentPosition)
        {
            if (_target == null || CheckDistance(currentPosition))
            {
                return false;
            }    
            else
                SetNewTargetPointToAgent(currentPosition, _target.CurrentPosition);

            return true;
        }

        public bool CheckDistance(Vector3 currentPosition)
        {
            if(_target == null)
                return true;

            return GameSettings.CheckCorrespondencePositions(currentPosition, _target.CurrentPosition, _distance);
        }
        
        public abstract void SetTarget(Target target, Vector3 currentPosition);

        public void SetMoveSpeed(float moveSpeed)
        {
            _moveAgent.speed = Mathf.Clamp(moveSpeed, GameSettings.Character.MinMoveSpeed, GameSettings.Character.MaxMoveSpeed);
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
        float _height;

        public ToPoint(NavMeshAgent moveAgent, float height)
        {
            _moveAgent = moveAgent;
            _height = height;
        }

        public override bool CheckPathEnd(Vector3 currentPosition)
        {
            return GameSettings.CheckCorrespondencePositions(currentPosition, _currentTargetPoint) == false;
        }

        public override void SetTarget(Target target, Vector3 currentPosition)
        {
            SetNewTargetPointToAgent(currentPosition, target.CurrentPosition());
            _currentTargetPoint.y += _height;
        }
    }

    private class ToAlly : ReachingChecker
    {        
        public ToAlly(NavMeshAgent moveAgent)
        {
            _moveAgent = moveAgent;
            _distance = GameSettings.Character.SocialDistance;
        }

        public override void SetTarget(Target target, Vector3 currentPosition)
        {
            target.TryGetFightebel(out _target);
            SetNewTargetPointToAgent(currentPosition, target.CurrentPosition());
        }
    }

    private class ToEnemy : ReachingChecker
    {                        
        public ToEnemy(float distance, NavMeshAgent moveAgent)
        {
            _distance = distance;
            _moveAgent = moveAgent;
        }

        public void SetDistance(float distance)
        {
            _distance = Mathf.Max(0, distance);
        }

        public override void SetTarget(Target target, Vector3 currentPosition)
        {
            target.TryGetFightebel(out _target);
            SetNewTargetPointToAgent(currentPosition, _target.CurrentPosition);
        }
    }
}
