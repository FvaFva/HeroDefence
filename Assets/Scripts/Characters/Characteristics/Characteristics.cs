using System;
using System.Collections.Generic;
using UnityEngine;

public class Characteristics
{
    private FighterCharacteristics _base;
    private Dictionary<ICharacteristicsSource, FighterCharacteristics> _baffs = new Dictionary<ICharacteristicsSource, FighterCharacteristics>();

    public Characteristics(FighterCharacteristics baseCharacteristics)
    {
        _base = baseCharacteristics;
        CurrentCharacteristicsUpdate();
    }

    public event Action CharacteristicsChanged;

    public FighterCharacteristics BaseCharacteristics => _base;

    public FighterCharacteristics Current;

    public void ApplyBuff(ICharacteristicsSource source)
    {
        if(_baffs.ContainsKey(source) == false)
        {
            _baffs.Add(source, source.GetCharacteristics());
            CurrentCharacteristicsUpdate();
            CharacteristicsChanged?.Invoke();
        }
    }

    public void RemoveBuff(ICharacteristicsSource source)
    {
        if (_baffs.ContainsKey(source))
        {
            _baffs.Remove(source);
            CurrentCharacteristicsUpdate();
            CharacteristicsChanged?.Invoke();
        }
    }

    private void CurrentCharacteristicsUpdate()
    {
        FighterCharacteristics current = _base;

        foreach (FighterCharacteristics characteristics in _baffs.Values)
        {
            current.ApplyCharacteristics(characteristics);
        }

        Mathf.Clamp(current.HitPoints, 1, float.MaxValue);
        Current = current;
    }
}