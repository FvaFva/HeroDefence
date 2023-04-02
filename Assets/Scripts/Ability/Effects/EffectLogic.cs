using System;
using UnityEngine;

public class EffectLogic: ICharacteristicsSource
{
    private EffectImpact _effect;
    private float _durationDetailed;
    private FighterCharacteristics _characteristicsChanges;
    private int _oldDuration;

    public IFightable Caster { get; private set; }
    public float HealthPerSecond { get; private set; }
    public bool IsEndDuration => _durationDetailed <= 0;
    public int Duration => (int)_durationDetailed;
    public Sprite Icon => _effect.Icon;
    public event Action<int> ChangeDuration;

    public EffectLogic(EffectImpact effect, IFightable caster, float secondDuration)
    {
        Caster = caster;
        _effect = effect;
        _durationDetailed = secondDuration;
    }

    public void CutDuration(float cuted)
    {
        _durationDetailed -= cuted;

        if(Duration != _oldDuration)
        {
            _oldDuration = Duration;
            ChangeDuration?.Invoke(Duration);
        }
    }

    public FighterCharacteristics GetCharacteristics()
    {
        return _characteristicsChanges;
    }

    public void Calculate(FighterCharacteristics characteristicsTerget)
    {
        _characteristicsChanges = _effect.ApplyEffect(characteristicsTerget);

        switch (_effect.EffectHealthMultiplaceType)
        {
            case CharacteristicType.HitPoint:
                HealthPerSecond = _effect.GetHealthPerSec(characteristicsTerget.HitPoints);
                break;
            case CharacteristicType.ManaPoints:
                HealthPerSecond = _effect.GetHealthPerSec(characteristicsTerget.ManaPoints);
                break;
            case CharacteristicType.Damage:
                HealthPerSecond = _effect.GetHealthPerSec(characteristicsTerget.Damage);
                break;
            case CharacteristicType.Armor:
                HealthPerSecond = _effect.GetHealthPerSec(characteristicsTerget.Armor);
                break;
            case CharacteristicType.AttackSpped:
                HealthPerSecond = _effect.GetHealthPerSec(characteristicsTerget.AttackSpeed);
                break;
            case CharacteristicType.Speed:
                HealthPerSecond = _effect.GetHealthPerSec(characteristicsTerget.Speed);
                break;
            case CharacteristicType.ManaRegen:
                HealthPerSecond = _effect.GetHealthPerSec(characteristicsTerget.ManaRegen);
                break;
        }
    }
}
