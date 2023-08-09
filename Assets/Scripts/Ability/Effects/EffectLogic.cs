using System;
using UnityEngine;

public class EffectLogic : ICharacteristicsSource
{
    private EffectImpact _effect;
    private float _durationDetailed;
    private FighterCharacteristics _characteristicsChanges;
    private int _oldDuration;

    public EffectLogic(EffectImpact effect, IFightable caster, float secondDuration)
    {
        Caster = caster;
        _effect = effect;
        _durationDetailed = secondDuration;
    }

    public event Action<int> ChangeDuration;

    public IFightable Caster { get; private set; }

    public float HealthPerSecond { get; private set; }

    public bool IsEndDuration => _durationDetailed <= 0;

    public int Duration => (int)_durationDetailed;

    public Sprite Icon => _effect.Icon;

    public void CutDuration(float count)
    {
        _durationDetailed -= count;

        if (Duration != _oldDuration)
        {
            _oldDuration = Duration;
            ChangeDuration?.Invoke(Duration);
        }
    }

    public FighterCharacteristics GetCharacteristics()
    {
        return _characteristicsChanges;
    }

    public void Calculate(FighterCharacteristics characteristicsTarget)
    {
        _characteristicsChanges = _effect.ApplyEffect(characteristicsTarget);

        switch (_effect.EffectHealthMultiplierType)
        {
            case CharacteristicType.HitPoint:
                HealthPerSecond = _effect.GetHealthPerSec(characteristicsTarget.HitPoints);
                break;
            case CharacteristicType.ManaPoints:
                HealthPerSecond = _effect.GetHealthPerSec(characteristicsTarget.ManaPoints);
                break;
            case CharacteristicType.Damage:
                HealthPerSecond = _effect.GetHealthPerSec(characteristicsTarget.Damage);
                break;
            case CharacteristicType.Armor:
                HealthPerSecond = _effect.GetHealthPerSec(characteristicsTarget.Armor);
                break;
            case CharacteristicType.AttackSpeed:
                HealthPerSecond = _effect.GetHealthPerSec(characteristicsTarget.AttackSpeed);
                break;
            case CharacteristicType.Speed:
                HealthPerSecond = _effect.GetHealthPerSec(characteristicsTarget.Speed);
                break;
            case CharacteristicType.ManaRegen:
                HealthPerSecond = _effect.GetHealthPerSec(characteristicsTarget.ManaRegen);
                break;
        }
    }
}
