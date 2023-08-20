using UnityEngine;

[CreateAssetMenu(fileName = "New perc action", menuName = "Ability/Percs/Action/RootRegen", order = 51)]
public class PercActionHealRoot : PercAction
{
    [SerializeField] private float _coefficientOfHealByDamage;

    protected override void MainAction(IFightable root, IFightable target, float damage)
    {
        root.ApplyHeal(damage * _coefficientOfHealByDamage);
    }
}
