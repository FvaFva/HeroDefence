using System.Collections.Generic;
using System;

public class CharacterPercBag
{
    private List<Perc> _percs;

    public event Action<Perc> ShowedPerc;

    public CharacterPercBag()
    {
        _percs = new List<Perc>();
    }

    public void AddPerc(Perc perc)
    {
        _percs.Add(perc);
    }

    public bool TryRemovePerc(Perc perc)
    {
        if(_percs.Contains(perc))
        {
            _percs.Remove(perc);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ExecuteActionDepenceAction(IFightable root, IFightable target, float damage, PercActionType type)
    {
        foreach (Perc perc in _percs)
        {
            perc.ExecuteDepenceAction(root, target, damage, type);
        }
    }

    public void ShowPercs()
    {
        foreach (Perc perc in _percs)
        {
            ShowedPerc?.Invoke(perc);
        }
    }
}
