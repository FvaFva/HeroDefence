using System.Collections.Generic;

public class Inventory
{
    private List<Item> _bag = new List<Item>();

    public IReadOnlyList<Item> Bug => _bag;
    public int Count => _bag.Count;

    public bool TryTakeItem(Item item)
    {
        bool isTaken = item != null && _bag.Contains(item) == false && _bag.Count <= GameSettings.PlayerBagSize;

        if(isTaken)
            _bag.Add(item);

        return isTaken;
    }

    public bool TryDropItem(Item item)
    {
        bool isDrop = item != null && _bag.Contains(item);

        if(isDrop)
            _bag.Remove(item);

        return isDrop;
    }
}
