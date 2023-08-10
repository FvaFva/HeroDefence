using MovementSystem;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CharacterMoveLogic: IReachLogic, ITargetChooser, ITargetDistanceObserver
{
    private const float AngleVision = GameSettings.Character.AngleAttack;
    private const float AngleDelta = GameSettings.Character.AngleDelta;

    private Transform _body;
    private Team _team;
    private CharacterRotationLogic _turner;
    private NavMeshAgent _moveAgent;

    private ReachingCheckerToPoint _pointChecker;
    private ReachingCheckerToAlly _allyChecker;
    private ReachingCheckerToEnemy _enemyChecker;
    private BaseReachingChecker _currentChecker;

    private IFightable _target;
    private float _distance;

    public event Action<Target> Reached;
    public event Action<Target> ChoseTarget;
    public event Action<IFightable> LostTarget;
    public event Action<IFightable> FoundTarget;

    public CharacterMoveLogic(NavMeshAgent moveAgent, Transform body, Team team, float _height)
    {
        _moveAgent = moveAgent;
        _moveAgent.enabled = true;
        _body = body;
        _team = team;

        _pointChecker = new ReachingCheckerToPoint(_moveAgent, _height);
        _allyChecker = new ReachingCheckerToAlly(_moveAgent);
        _enemyChecker = new ReachingCheckerToEnemy(_distance, _moveAgent);
        _turner = new CharacterRotationLogic(body, GameSettings.Character.AngularSpeed, _height);

        _currentChecker = _pointChecker;
    }

    public void SetTarget(Target target)
    {
        if (target.TryGetFightable(out _target))
            if (_target.IsFriendly(_team))
                _currentChecker = _allyChecker;
            else
                _currentChecker = _enemyChecker;
        else
            _currentChecker = _pointChecker;
        
        _currentChecker.SetTarget(target, _body.position);
        _turner.SetTarget(target);
    }

    public void OffNavMashAgent()
    {
        _moveAgent.enabled = false;
    }

    public void SetNewDistanceToEnemy(float distance)
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
        yield return GameSettings.Character.OptimizationDelay;

        while (_currentChecker.CheckPathEnd(_body.position))
        {
            yield return GameSettings.Character.OptimizationDelay;
        }

        while (_turner.CheckRotateToTarget(AngleDelta) == false) 
        {
            _turner.RotateToTarget();
            yield return GameSettings.Character.OptimizationDelay;
        }

        Reached?.Invoke(new Target(_body.position, _target));
        FoundTarget?.Invoke(_target);
    }

    public IEnumerator ObserveTarget()
    {
        yield return GameSettings.Character.OptimizationDelay; 

        while (_currentChecker.CheckDistance(_body.position) && _turner.CheckRotateToTarget(AngleVision))
        {
            yield return GameSettings.Character.OptimizationDelay;
        }

        ChoseTarget?.Invoke(new Target(_body.position, _target));
        LostTarget?.Invoke(_target);
    }
}
