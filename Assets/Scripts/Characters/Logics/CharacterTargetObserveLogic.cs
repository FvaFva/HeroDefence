using System;
using UnityEngine;

public class CharacterTargetObserveLogic : MonoBehaviour, ITargetChooser
{
    private Coroutine _observingTarget;
    private ITargetDistanceObserver _distanceObserver;
    private IFightable _target;

    public event Action<Target> ChoseTarget;

    public void Init(ITargetDistanceObserver observer)
    {
        if (_distanceObserver == null)
            _distanceObserver = observer;
    }

    public void SetTarget(Target target)
    {
        StopWork();

        if(target.TryGetFightebel(out _target))
        {
            StartWork();
        }
    }

    private void StopWork()
    {
        _distanceObserver.FoundTarget -= StartObserveDistanceToTarget;
        _distanceObserver.LostTarget -= StopObserveDistanceToTarget;

        if (_target != null)
            _target.Died -= OnTargetDieing;

        _target = null;

        if (_observingTarget != null)
            StopCoroutine(_observingTarget);
    }

    private void StartWork()
    {
        _distanceObserver.FoundTarget += StartObserveDistanceToTarget;
        _target.Died += OnTargetDieing;
    }

    private void OnDisable()
    {
        StopWork();
    }

    private void StartObserveDistanceToTarget(IFightable target)
    {
        if (_observingTarget != null)
            StopCoroutine(_observingTarget);

        _distanceObserver.LostTarget += StopObserveDistanceToTarget;
        _distanceObserver.FoundTarget -= StartObserveDistanceToTarget;
        _observingTarget = StartCoroutine(_distanceObserver.ObserveTarget());
    }

    private void StopObserveDistanceToTarget(IFightable target)
    {
        StopCoroutine(_observingTarget);
        _distanceObserver.LostTarget -= StopObserveDistanceToTarget;
        _distanceObserver.FoundTarget += StartObserveDistanceToTarget;
    }

    private void OnTargetDieing()
    {
        ChoseTarget.Invoke(new Target());
        StopWork();
    }
}
