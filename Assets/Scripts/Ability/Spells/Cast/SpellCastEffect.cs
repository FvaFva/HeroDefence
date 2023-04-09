using UnityEngine;

public class SpellCastEffect : BaseSpellCastLogic
{
    [SerializeField] private EffectImpact effect;
    [SerializeField] private float _duration;

    public override void CastAction(IFightable target, IFightable caster)
    {
        target.ApplyEffect(new EffectLogic(effect, caster, _duration));
    }
}