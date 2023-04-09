using System;
using System.Collections.Generic;

public class SpellTargetCaster : BaseSpellTargetLogic
{
    public override event Action<bool, List<IFightable>, Spell> TargetsSelected;

    public override void SelectTargets(Spell spell, int range, int radius, bool isForEnemy)
    {
        List<IFightable> taget = new List<IFightable>();
        taget.Add(spell.Caster);
        TargetsSelected?.Invoke(true, taget, spell);
    }
}