using System.Collections.Generic;

public class Ammunition
{ 
    private Dictionary<ItemType, Item> _thingsWorn = new Dictionary<ItemType, Item>();

    public IReadOnlyDictionary<ItemType, Item> ThingsWorn => _thingsWorn;

    public bool TryPutOnItem(Item item)
    {
        if (_thingsWorn.ContainsKey(item.ItemType))
        {
            return false;
        }
        else
        {
            _thingsWorn.Add(item.ItemType, item);
            return true;
        }
    }

    public bool TryDropType(ItemType droppedType, out Item dropItem)
    {
        if (_thingsWorn.ContainsKey(droppedType))
        {
            dropItem = _thingsWorn[droppedType];
            _thingsWorn.Remove(droppedType);
            return true;
        }
        else
        {
            dropItem = null;
            return false;
        }
    }
}
