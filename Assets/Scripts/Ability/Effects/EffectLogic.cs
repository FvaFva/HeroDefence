using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EffectLogic
{
    private EffectImpact _effect;
    private CharacterAnimaLogic _caster;
    private float _durationSecond;

    public event Action<EffectLogic> EndingEffctDuration;

    public EffectLogic(EffectImpact effect, CharacterAnimaLogic caster, float secondDuration)
    {
        _caster = caster;
        _effect = effect;
        _durationSecond = secondDuration;
    }
}
