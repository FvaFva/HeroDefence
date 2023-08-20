using UnityEngine;

[CreateAssetMenu(fileName = "New perc action", menuName = "Ability/Percs/Action/RootApplyStamina", order = 51)]
public class PercActionApplyStaminaRoot : PercAction
{
    [SerializeField]private int _countStamina;

    protected override void MainAction(IFightable root, IFightable target, float damage)
    {
        root.ApplyStamina(_countStamina);
    }
}
