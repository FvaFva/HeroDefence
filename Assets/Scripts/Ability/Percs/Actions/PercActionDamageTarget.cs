using UnityEngine;

[CreateAssetMenu(fileName = "New perc action", menuName = "Ability/Percs/Action/TargetDamage", order = 51)]
public class PercActionDamageTarget : PercAction
{
    [SerializeField] private float _coefficientOfDamage;

    protected override void MainAction(IFightable root, IFightable target, float damage)
    {
        float newDamage = damage * _coefficientOfDamage;
        target.TryApplyDamage(root, ref newDamage, false);
    }
}
