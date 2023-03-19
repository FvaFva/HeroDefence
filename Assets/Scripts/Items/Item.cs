using UnityEngine;

public class Item: ICharacteristicsSource, IPercSource
{
    private FighterСharacteristics _characteristics;

    public bool IsTaken { get; private set; }
    public Perc Perc { get; private set; }
    public string Name { get; private set; }
    public ItemType ItemType { get; private set; }
    public Sprite Icon { get; private set; }

    public Item(ItemPreset preset)
    {
        _characteristics = preset.GetCharacteristics();
        Name = preset.Name;
        Perc = preset.Perc;
        Icon = preset.Icon;
        ItemType = preset.ItemType;
    }

    public FighterСharacteristics GetCharacteristics()
    {
        return _characteristics;
    }
}
