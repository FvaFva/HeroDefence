using System.Collections.Generic;
using System.Linq;

public class CharacterPercBag
{
    private List<IPercSource> _slots = new List<IPercSource>();

    public IReadOnlyList<Ability> Percs => _slots.Select(ps => ps.Perc).ToList().AsReadOnly();

    public bool TryAddPerc(IPercSource source)
    {
        bool isCanAdd = _slots.Contains(source) == false;

        if (isCanAdd)
            _slots.Add(source);

        return isCanAdd;
    }

    public bool TryRemovePerc(IPercSource source)
    {
        bool isCanRemove = _slots.Contains(source);

        if (isCanRemove)
            _slots.Remove(source);

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
