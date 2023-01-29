using UnityEngine;

[CreateAssetMenu(fileName = "New melee logic", menuName = "Characters/AttackLogic/Melee", order = 51)]
public class MeleeLogic : AttackLogic
{
    public override void AttackEnemy(Character enemy, float damage)
    {
        enemy.ApplyDamage(damage);
    }
}
