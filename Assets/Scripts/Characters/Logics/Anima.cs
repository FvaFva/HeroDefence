using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Anima
{
    private List<Spell> _spellBook = new List<Spell>();

    public float ManaPointsMax { get; protected set; }
    public float ManaPointsCurrent { get; protected set; }
    public float ManaPerSecond { get; protected set; }

    public float CurrentMPCoefficient => ManaPointsCurrent / ManaPointsMax;
    public event Action<Spell> CastedSpell;
    public event Action<Spell> AddedSpell;

    public Anima(float manaPoints, float manaRegen)
    {      
        ManaPointsCurrent = manaPoints;
        ManaPointsMax = manaPoints;
        ManaPerSecond = manaRegen;
    }

    public void AddSpell(Spell spell)
    {
        _spellBook.Add(spell);
        AddedSpell?.Invoke(spell);
    }

    public void CastSpell(Spell spell, Vector2 castPoint)
    {
        float manaCost = spell.GetManacost();
        if (_spellBook.Contains(spell) && ManaPointsCurrent >= manaCost)
        {
            CastedSpell?.Invoke(spell);
            ManaPointsCurrent -= manaCost;
            spell.Cast(castPoint);
        }
    }
}
