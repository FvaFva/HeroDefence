using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemFactory", menuName = "Items/Create New Item Factory", order = 51)]
public class ItemFactory : ScriptableObject
{
    [SerializeField] private List<ItemPreset> _presets = new List<ItemPreset>();
    [SerializeField] private List<Perc> _commonPerks = new List<Perc>();
    [SerializeField] private List<Perc> _uncommonPerks = new List<Perc>();
    [SerializeField] private List<Perc> _rarePerks = new List<Perc>();
    [SerializeField] private List<Perc> _epicPerks = new List<Perc>();
    [SerializeField] private List<Perc> _legendaryPerks = new List<Perc>();

    public Item GetRandomItem()
    {
        ItemPreset currentPreset = _presets[UnityEngine.Random.Range(0, _presets.Count)];

        if (currentPreset.ItemType == ItemType.Weapon)
            return CreateWeapon(currentPreset);
        else
            return new Item(currentPreset, GetRandomPercForRarity(currentPreset.Rarity));
    }

    private Perc GetRandomPercForRarity(ItemRarity rarity)
    {
        return rarity switch
        {
            ItemRarity.Common => GetRandomPercFromList(_commonPerks),
            ItemRarity.Uncommon => GetRandomPercFromList(_uncommonPerks),
            ItemRarity.Rare => GetRandomPercFromList(_rarePerks),
            ItemRarity.Epic => GetRandomPercFromList(_epicPerks),
            ItemRarity.Legendary => GetRandomPercFromList(_legendaryPerks),
            _ => null,
        };
    }

    private Perc GetRandomPercFromList(List<Perc> perks)
    {
        return perks[UnityEngine.Random.Range(0, perks.Count)];
    }

    private Weapon CreateWeapon(ItemPreset preset)
    {
        int countWeaponTypes = Enum.GetValues(typeof(WeaponType)).Length;
        WeaponType type = (WeaponType)UnityEngine.Random.Range(0, countWeaponTypes);

        switch (type)
        {
            case WeaponType.Melee:
                return InstantiateMeleeWeapon(preset);
            default:
                break;
        }

        return null;
    }

    private Weapon InstantiateMeleeWeapon(ItemPreset preset)
    {
        MeleeAttackLogic attackLogic = new MeleeAttackLogic();
        float attackDistance = 2.5f;
        return new Weapon(preset, attackLogic, attackDistance, GetRandomPercForRarity(preset.Rarity));
    }
}
