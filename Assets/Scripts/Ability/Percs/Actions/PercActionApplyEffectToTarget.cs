using UnityEngine;

[CreateAssetMenu(fileName = "New perc action", menuName = "Ability/Perc/Action/EffectToTarget", order = 51)]
public class PercActionApplyEffectToTarget : PercAction
{
    [SerializeField] private int _durationSeconds;
    [SerializeField] private EffectImpact _effect;

    protected override void MainAction(IFightable root, IFightable target, float damage)
    {
        EffectLogic effect = new EffectLogic(_effect, root, _durationSeconds);
        target.ApplyEffect(effect);
    }
}
