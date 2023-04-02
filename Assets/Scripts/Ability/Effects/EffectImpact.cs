using UnityEngine;

[CreateAssetMenu(fileName = "New spell effect", menuName ="Ability/Spells/NewSpellEffect", order = 51)]
public class EffectImpact : ScriptableObject
{
    [SerializeField] private float _coefficientMoveSpeed;
    [SerializeField] private float _coefficientAttackSpeed;
    [SerializeField] private float _coefficientDamage;
    [SerializeField] private float _coefficientArmor;
    [SerializeField] private float _healtPerSec;
    [SerializeField] private float _healtCoefficientPerSec;

    [SerializeField] private CharacteristicType _healtMultiplier;
    [SerializeField] private Sprite _icon;

    public CharacteristicType EffectHealthMultiplaceType => _healtMultiplier;
    public Sprite Icon => _icon;

    public FighterCharacteristics ApplyEffect(FighterCharacteristics characteristics)
    {
        FighterCharacteristics tempCharacteristics = characteristics;
        tempCharacteristics.Armor *= _coefficientArmor;
        tempCharacteristics.AttackSpeed *= _coefficientAttackSpeed;
        tempCharacteristics.Damage *= _coefficientDamage;
        tempCharacteristics.Speed *= _coefficientMoveSpeed;
        return tempCharacteristics;
    }

    public float GetHealthPerSec(float multiplace)
    {
        return _healtCoefficientPerSec * multiplace + _healtPerSec;
    }
}
