using System;
using System.Collections.Generic;
using UnityEngine;

public class Characteristics
{
    private FighterСharacteristics _base;
    private Dictionary<ICharacteristicsSource, FighterСharacteristics> _baffs = new Dictionary<ICharacteristicsSource, FighterСharacteristics>();

    public FighterСharacteristics BaseСharacteristics => _base;
    public event Action CharacteristicsChanged;

    public FighterСharacteristics Current;

    public Characteristics(FighterСharacteristics baseСharacteristics)
    {
        _base = baseСharacteristics;
        CurrentСharacteristicsUpdate();
    }

    public void ApplyBuff(ICharacteristicsSource source)
    {
        if(_baffs.ContainsKey(source) == false)
        {
            _baffs.Add(source, source.GetCharacteristics());
            CurrentСharacteristicsUpdate();
            CharacteristicsChanged?.Invoke();
        }
    }

    public void RemoveBuff(ICharacteristicsSource source)
    {
        if (_baffs.ContainsKey(source))
        {
            _baffs.Remove(source);
            CurrentСharacteristicsUpdate();
            CharacteristicsChanged?.Invoke();
        }
    }

    private void CurrentСharacteristicsUpdate()
    {
        FighterСharacteristics current = _base;

        foreach (FighterСharacteristics сharacteristics in _baffs.Values)
        {
            current.ApplyCharacteristics(сharacteristics);
        }

        Mathf.Clamp(current.HitPoints, 1, float.MaxValue);
        Current = current;
    }
}