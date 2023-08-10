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
        _distanceObserver ??= observer;
    }

    public void SetTarget(Target target)
    {
        StopWork();

        if (target.TryGetFightable(out _target))
        {
            StartWork();
        }
    }

    private void StopWork()
    {
        _distanceObserver.FoundTarget -= StartObserveDistanceToTarget;
        _distanceObserver.LostTarget -= StopObserveDistanceToTarget;

        if (_target != null)
            _target.Died -= OnTargetDyeing;

        _target = null;

        if (_observingTarget != null)
            StopCoroutine(_observingTarget);
    }

    private void StartWork()
    {
        _distanceObserver.FoundTarget += StartObserveDistanceToTarget;
        _target.Died += OnTargetDyeing;
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

    private void OnTargetDyeing()
    {
        ChoseTarget.Invoke(default(Target));
        StopWork();
    }
}
