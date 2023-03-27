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

    public Fighter—haracteristics ApplyEffect(Fighter—haracteristics Òharacteristics)
    {
        Fighter—haracteristics tempCharacteristics = Òharacteristics;
        tempCharacteristics.Armor *= _coefficientArmor;
        tempCharacteristics.AttackSpeed *= _coefficientAttackSpeed;
        tempCharacteristics.Damage *= _coefficientDamage;
        tempCharacteristics.Speed *= _coefficientMoveSpeed;
        return tempCharacteristics;
    }

    public float GetHealthPerSec(float muliplace)
    {
        return _healtCoefficientPerSec * muliplace + _healtPerSec;
    }
}
