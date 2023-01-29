using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellPlaceLogic : ScriptableObject
{
    public abstract List<CharacterFightLogic> GetTargets(Vector2 targetPoint);
}
