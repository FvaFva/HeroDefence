using UnityEngine;

[CreateAssetMenu(fileName = "New perc action", menuName = "Ability/Percs/Action/RootApplyStamina", order = 51)]
public class PercActionApplyStaminaRoot : PercAction
{
    [SerializeField] int _countStamina;
    public override void DoAction(IFightable root, IFightable target, float damage)
    {
        root.ApplyStamina(_countStamina);
    }
}
