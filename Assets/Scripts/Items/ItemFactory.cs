using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemFactory", menuName = "Items/Create New Item Factory", order = 51)]
public class ItemFactory : ScriptableObject
{
    [SerializeField] List<ItemPreset> _presets  = new List<ItemPreset>();
    [SerializeField] List<Perc> _commonPercs    = new List<Perc>();
    [SerializeField] List<Perc> _uncommonPercs  = new List<Perc>();
    [SerializeField] List<Perc> _rarePercs      = new List<Perc>();
    [SerializeField] List<Perc> _epickPercs     = new List<Perc>();
    [SerializeField] List<Perc> _legandaryPercs = new List<Perc>();
    
    public Item GetRandomItem()
    {
        ItemPreset currentPreset = _presets[UnityEngine.Random.Range(0, _presets.Count)];

        if (currentPreset.ItemType == ItemType.Weapon)
            return CreateWeapon(currentPreset);
        else
            return new Item(currentPreset, GetRandomPercForRariy(currentPreset.Rarity));
    }

    private Perc GetRandomPercForRariy(ItemRarity rarity)
    {
        switch(rarity)
        {
            case ItemRarity.Common:
                return GetRandomPercFromList(_commonPercs);
            case ItemRarity.Uncommon:
                return GetRandomPercFromList(_uncommonPercs);
            case ItemRarity.Rare:
                return GetRandomPercFromList(_rarePercs);
            case ItemRarity.Epic:
                return GetRandomPercFromList(_epickPercs);
            case ItemRarity.Legendary:
                return GetRandomPercFromList(_legandaryPercs);
            default:
                return null;
        }
    }

    private Perc GetRandomPercFromList(List<Perc> percs)
    {
        return percs[UnityEngine.Random.Range(0, percs.Count)];
    }

    private Weapon CreateWeapon(ItemPreset preset)
    {
        int countWeaponTypes = Enum.GetValues(typeof(WeaponType)).Length;
        WeaponType type = (WeaponType)UnityEngine.Random.Range(0, countWeaponTypes);

        switch (type)
        {
            case WeaponType.Melee:
                return InstantiateMeleeWeapon(preset);
        }

        return null;
    }

    private Weapon InstantiateMeleeWeapon(ItemPreset preset)
    {
        MeleeAttackLogic attackLogic = new MeleeAttackLogic();
        float attackDistance = 2.5f;
        return new Weapon(preset, attackLogic, attackDistance, GetRandomPercForRariy(preset.Rarity));
    }
}
