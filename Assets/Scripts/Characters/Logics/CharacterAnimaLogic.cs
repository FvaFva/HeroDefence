using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CharacterAnimaLogic
{
    private List<Spell> _spellBook = new List<Spell>();
    private float _manaPointsMax;
    private float _manaPointsCurrent;
    private float _manaRegen;
    private Spell[] _currentSpells = new Spell[GameSettings.Character.CountOfCharacterSpells];
    private IFightable _caster;

    public CharacterAnimaLogic(FighterCharacteristics characteristics, IFightable caster)
    {
        _manaPointsCurrent = characteristics.ManaPoints;
        ApplyNewCharacteristics(characteristics);
        _caster = caster;
    }

    public event Action<Spell> AddedSpell;

    public float ManaPointsCoefficient => _manaPointsCurrent / _manaPointsMax;

    public void ApplyNewCharacteristics(FighterCharacteristics characteristics)
    {
        _manaPointsMax = characteristics.ManaPoints;
        _manaPointsCurrent = characteristics.ManaPoints * ManaPointsCoefficient;
        _manaRegen = characteristics.ManaRegen;
    }

    public void DropSpell(ISpellSource source)
    {
        Spell spellPresetInCurrent = _currentSpells.Where(spell => spell.Source == source).FirstOrDefault();
    }

    public void AddSpell(ISpellSource source)
    {
        Spell spellPresetInCurrent = _currentSpells.Where(spell => spell.Source == source).FirstOrDefault();

        if (spellPresetInCurrent != null)
            return;

        int freeSlot = GetIdSpellPresetInCurrent(null);

        if (freeSlot == -1)
            return;

        Spell knownSpell = _spellBook.Where(spell => spell.Source == source).FirstOrDefault();

        if (knownSpell == null)
        {
            knownSpell = new Spell(_caster, source);
            _spellBook.Add(knownSpell);
        }

        _currentSpells[freeSlot] = knownSpell;
        AddedSpell?.Invoke(knownSpell);
    }

    public void CastSpell(int id)
    {
        if (id >= _currentSpells.Length || id < 0)
            return;
    }

    public void RestingAnima(float delay)
    {
        if(_manaPointsCurrent < _manaPointsMax)
        {
            _manaPointsCurrent += _manaRegen * delay;
            Mathf.Clamp(_manaPointsCurrent, 0, _manaPointsMax);
        }

        foreach (Spell spell in _spellBook)
            spell.ReduceCoolDown(delay);
    }

    private int GetIdSpellPresetInCurrent(ISpellSource source)
    {
        for (int i = 0; i < _currentSpells.Length; i++)
        {
            if (_currentSpells[i].Source == source)
                return i;
        }
        return -1;
    }
}
