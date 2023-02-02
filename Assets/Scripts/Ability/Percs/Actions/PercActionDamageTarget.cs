using UnityEngine;

[CreateAssetMenu(fileName = "New perc action", menuName = "Ability/Percs/Action/TargetDamage", order = 51)]
public class PercActionDamageTarget : PercAction
{
    [SerializeField] float _coefficientOfDamage;

    public override void DoAction(Character root, Character target, float damage)
    {
        target.TakeDamage(root, damage * _coefficientOfDamage, false);
    }
}
