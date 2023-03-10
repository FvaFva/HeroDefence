using UnityEngine;

[CreateAssetMenu(fileName = "New character", menuName = "Characters/NewCharacter", order = 51)]
public class CharacterPreset : ScriptableObject, ICharacteristicsSource
{
    [SerializeField] private float _hitPoints;
    [SerializeField] private float _attacSpeed;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _armor;
    [SerializeField] private float _damage;
    [SerializeField] private float _manaPoints;
    [SerializeField] private float _manaRegen;
    [SerializeField] private float _height;

    [SerializeField] private string _class;
    [SerializeField] private Sprite _portrait;
    [SerializeField] private ItemPreset _baseWeaponCharacteristics;
    [SerializeField] private WeaponType _baseWeaponType;
    [SerializeField] private float _baseWeaponAttackDistance;

    public string Profission => _class;
    public float Height => _height;
    public Sprite Portrait => _portrait;
    public string Name => GameSettings.Character.GetRandomName();
    public Weapon Weapon => new Weapon(_baseWeaponCharacteristics, _baseWeaponAttackDistance, _baseWeaponType);

    public Fighter—haracteristics GetCharacteristics()
    {
        return new Fighter—haracteristics(_attacSpeed, _damage, _armor, _hitPoints, _moveSpeed, _manaRegen, _manaPoints);
    }
}
