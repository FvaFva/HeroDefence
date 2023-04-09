using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Cast on himself", menuName = "Ability/Spells/Targeting/OnHimself", order = 51)]
public abstract class BaseSpellTargetLogic : ScriptableObject
{
    public abstract event Action<bool, List<IFightable>, Spell, IFightable> TargetsSelected;
    public abstract void SelectTargets(Spell spell, int range, int radius, bool isForEnemy, IFightable caster);
}
