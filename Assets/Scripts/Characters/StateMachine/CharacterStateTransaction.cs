using System;
using UnityEngine;

public abstract class CharacterStateTransaction : MonoBehaviour
{
    [SerializeField] private CharacterState _targetState;

    private ICharacterComander _comander;

    public event Action<CharacterState, Target> Activited;

    public void Init(ICharacterComander comander, Target target)
    {
        _comander = comander;
        comander!.ChoosedTarget += SetNewTarget;
    }

    public void Off()
    {                
        _comander!.ChoosedTarget -= SetNewTarget;
    }
    
    protected void SetNewTarget(Target target)
    {
        if (IsSuitableTarget(target))
        {            
            _comander!.ChoosedTarget -= SetNewTarget;
            Activited?.Invoke(_targetState, target);
        }
    }

    protected abstract bool IsSuitableTarget(Target target);
}
