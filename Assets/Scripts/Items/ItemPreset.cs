using UnityEngine;

[CreateAssetMenu(fileName = "New item", menuName = "Items/NewItem", order = 51)]
public class ItemPreset: ScriptableObject, ICharacteristicsSource
{
    [SerializeField] private string _name;
    [SerializeField] private ItemType _itemType;
    [SerializeField] private ItemRarity _rarity;
    [SerializeField] private Sprite _icon;

    [SerializeField] private float _minAttackSpeed;
    [SerializeField] private float _maxAttackSpeed;

    [SerializeField] private float _minArmor;
    [SerializeField] private float _maxArmor;

    [SerializeField] private float _minSpeed;
    [SerializeField] private float _maxSpeed;

    [SerializeField] private float _minManaRegen;
    [SerializeField] private float _maxManaRegen;

    [SerializeField] private float _minHitPoints;
    [SerializeField] private float _maxHitPoints;

    [SerializeField] private float _minDamage;
    [SerializeField] private float _maxDamage;

    [SerializeField] private float _minManaPoints;
    [SerializeField] private float _maxManaPoints;

    public string Name => _name;
    public ItemType ItemType => _itemType;
    public ItemRarity Rarity => _rarity;
    public Sprite Icon => _icon;

    public FighterСharacteristics GetCharacteristics()
    {
        FighterСharacteristics characteristics = new FighterСharacteristics();

        characteristics.Armor           = Random.Range(_minArmor, _maxArmor);
        characteristics.HitPoints       = Random.Range(_minHitPoints, _maxHitPoints);
        characteristics.AttackSpeed     = Random.Range(_minAttackSpeed, _maxAttackSpeed);
        characteristics.Damage          = Random.Range(_minDamage, _maxDamage);
        characteristics.Speed           = Random.Range(_minSpeed, _maxSpeed);
        characteristics.ManaPoints      = Random.Range(_minManaPoints, _maxManaPoints);
        characteristics.ManaRegen       = Random.Range(_minManaRegen, _maxManaRegen);

        return characteristics;
    }
}
