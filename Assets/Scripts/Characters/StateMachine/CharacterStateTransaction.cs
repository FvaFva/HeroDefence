using System;
using UnityEngine;

public abstract class CharacterStateTransaction : MonoBehaviour
{
    [SerializeField] private CharacterState _targetState;

    protected IFightebel _target;

    public event Action<CharacterState> Activited;

    public void Init(IFightebel target)
    {
        _target = target;
        SubscribeToTarget();
    }

    public void Off()
    {
        DescribeToTarget();
        _target = null;
    }
    
    public void SetNewTarget(IFightebel target)
    {
        DescribeToTarget();
        _target = target;
        SubscribeToTarget();
    }

    protected abstract void SubscribeToTarget();

    protected abstract void DescribeToTarget();
}
