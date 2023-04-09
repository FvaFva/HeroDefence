using UnityEngine;

public abstract class BaseSpellCastLogic : ScriptableObject
{
    public abstract void CastAction(IFightable target, IFightable caster);
}
