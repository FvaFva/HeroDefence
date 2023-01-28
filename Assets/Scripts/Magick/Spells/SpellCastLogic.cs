using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellCastLogic : ScriptableObject
{
    public abstract void CastAction(CombatUnit target);
}
