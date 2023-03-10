using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New perc action", menuName = "Ability/Percs/Action/RootRegen", order = 51)]
public class PercActionHealRoot : PercAction
{
    [SerializeField] float _coefficientOfHealByDamage;
    public override void DoAction(IFightable root, IFightable target, float damage)
    {
        root.ApplyHeal(damage * _coefficientOfHealByDamage);
    }
}
