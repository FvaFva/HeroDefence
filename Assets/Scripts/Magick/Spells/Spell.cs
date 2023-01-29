using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell:ScriptableObject
{
    [SerializeField] private int _manaCostBase;
    [SerializeField] private int _manaCostPerLevel;
    [SerializeField] private SpellPlaceLogic PlaceLogic;
    [SerializeField] private List<SpellCastLogic> SpellCastLogicList;
    
    private int _level = 1;

    public void Cast(Vector2 castPoint)
    {
        foreach(CharacterFightLogic target in PlaceLogic.GetTargets(castPoint))
        {
            foreach(SpellCastLogic castLogic in SpellCastLogicList)
            {
                castLogic.CastAction(target);
            }
        }
    }

    public float GetManacost()
    {
        return _manaCostBase + _manaCostPerLevel * _level;
    }
}
