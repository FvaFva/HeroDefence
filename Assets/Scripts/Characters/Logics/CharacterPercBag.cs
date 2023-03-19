using System.Collections.Generic;
using System.Linq;

public class CharacterPercBag
{
    private List<PercSlot> _slots = new List<PercSlot>();

    public IReadOnlyList<Ability> Percs => _slots.Select(ps => ps.Perc).ToList().AsReadOnly();

    public bool TryAddPerc(IPercSource source, Perc perc)
    {
        PercSlot newSlot = new PercSlot(source, perc);
        bool isCanAdd = _slots.Contains(newSlot) == false;

        if (isCanAdd)
            _slots.Add(newSlot);

        return isCanAdd;
    }

    public bool TryRemovePerc(IPercSource source, Perc perc)
    {
        PercSlot newSlot = new PercSlot(source, perc);
        bool isCanRemove = _slots.Contains(newSlot) == false;

        if (isCanRemove)
            _slots.Remove(newSlot);

        return isCanRemove;
    }

    public void ExecuteActionDepenceAction(IFightable root, IFightable target, float damage, PercActionType type)
    {
        foreach (Perc perc in _slots.Select(ps => ps.Perc))
        {
            perc.ExecuteDepenceAction(root, target, damage, type);
        }
    }
}
