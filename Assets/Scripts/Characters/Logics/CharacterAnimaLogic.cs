using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimaLogic
{
    private List<Spell> _spellBook = new List<Spell>();
    private float _manaPointsMax;
    private float _manaPointsCurrent;
    private float _manaRegen;

    public float ManaPointsCoefficient => _manaPointsCurrent / _manaPointsMax;
    public event Action<Spell> CastedSpell;
    public event Action<Spell> AddedSpell;

    public CharacterAnimaLogic(FighterCharacteristics characteristics)
    {      
        _manaPointsCurrent = characteristics.ManaPoints;
        ApplyNewCharacteristics(characteristics);
    }

    public void ApplyNewCharacteristics(FighterCharacteristics characteristics)
    {
        _manaPointsMax = characteristics.ManaPoints;
        _manaPointsCurrent = characteristics.ManaPoints * ManaPointsCoefficient;
        _manaRegen = characteristics.ManaRegen;
    }

    public void AddSpell(Spell spell)
    {
        _spellBook.Add(spell);
        AddedSpell?.Invoke(spell);
    }

    public void CastSpell(Spell spell, Vector2 castPoint)
    {
        float manaCost = spell.GetManacost();
        if (_spellBook.Contains(spell) && _manaPointsCurrent >= manaCost)
        {
            CastedSpell?.Invoke(spell);
            _manaPointsCurrent -= manaCost;
            spell.Cast(castPoint);
        }
    }

    public void ManaRegeneration(float delay)
    {
        if(_manaPointsCurrent < _manaPointsMax)
        {
            _manaPointsCurrent += _manaRegen * delay;
            Mathf.Clamp(_manaPointsCurrent, 0, _manaPointsMax);
        }
    }
}
