using UnityEngine;

public class SpellCastEffect : BaseSpellCastLogic
{
    [SerializeField] private EffectImpact _effect;
    [SerializeField] private float _duration;

    public override void CastAction(IFightable target, IFightable caster)
    {
        target.ApplyEffect(new EffectLogic(_effect, caster, _duration));
    }
}