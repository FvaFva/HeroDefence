using UnityEngine;

public class Item: ICharacteristicsSource, IPercSource
{
    private FighterCharacteristics _characteristics;

    public bool IsTaken { get; private set; }
    public Perc Perc { get; private set; }
    public string Name { get; private set; }
    public ItemType ItemType { get; private set; }
    public Sprite Icon { get; private set; }

    public Item(ItemPreset preset, Perc perc)
    {
        _characteristics = preset.GetCharacteristics();
        Name = preset.Name;
        Perc = perc;
        Icon = preset.Icon;
        ItemType = preset.ItemType;
    }

    public FighterCharacteristics GetCharacteristics()
    {
        return _characteristics;
    }
}
