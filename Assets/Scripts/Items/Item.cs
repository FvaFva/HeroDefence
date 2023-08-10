using UnityEngine;

public class Item : ICharacteristicsSource, IPercSource
{
    private FighterCharacteristics _characteristics;

    public Item(ItemPreset preset, Perc perc)
    {
        _characteristics = preset.GetCharacteristics();
        Name = preset.Name;
        Perk = perc;
        Icon = preset.Icon;
        ItemType = preset.ItemType;
    }

    public bool IsTaken { get; private set; }

    public Perc Perk { get; private set; }

    public string Name { get; private set; }

    public ItemType ItemType { get; private set; }

    public Sprite Icon { get; private set; }

    public FighterCharacteristics GetCharacteristics()
    {
        return _characteristics;
    }
}
