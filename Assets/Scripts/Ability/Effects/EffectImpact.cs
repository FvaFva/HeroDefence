using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EffectImpact
{
    private EffectLogic _effect;
    private Anima _caster;
    private float _durationSecond;

    public event Action<EffectImpact> EndingEffctDuration;

    public EffectImpact(EffectLogic effect, Anima caster, float secondDuration)
    {
        _caster = caster;
        _effect = effect;
        _durationSecond = secondDuration;
    }

    public void UpdateEffect(out float slowMove, out float slowAttack, out float hitPontsChange)
    {
        float delta = Time.deltaTime;
        _durationSecond -= delta;

        if (_effect.IsBlockMove)
            slowMove = 1;
        else
            slowMove = _effect.ProcentMoveSlow;

        if (_effect.IsBlockFight)
            slowAttack = 1;
        else
            slowAttack = _effect.ProcentAttackSlow;

        hitPontsChange = (_effect.HealPersSecond + _effect.DamagePersSecond) * delta;

        if (_durationSecond <= 0)
            EndingEffctDuration?.Invoke(this);
    }
}
