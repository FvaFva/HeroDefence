using System;
using System.Collections.Generic;
using System.Linq;

public class CharacterEffectBug: ICharacteristicsSource
{
    private float _healthChangeCounter = 0;
    private List<EffectLogic> _effects = new List<EffectLogic>();

    public event Action CharacteristicsChanged;
    public event Action<float, IFightable> HealthTic;
    public IReadOnlyList<EffectLogic> Effects => _effects.OrderByDescending(effect => effect.Duration).ToList();

    public FighterCharacteristics GetCharacteristics()
    {
        FighterCharacteristics tempFighterCharacteristics = new FighterCharacteristics();

        foreach (EffectLogic effectImpact in _effects)
            tempFighterCharacteristics.ApplyCharacteristics(effectImpact.GetCharacteristics());

        return tempFighterCharacteristics;
    }

    public void ApplyEffect(EffectLogic effect)
    {
        _effects.Add(effect);
        CharacteristicsChanged?.Invoke();
    }

    public void UpdateDuration(float time)
    {
        if (_effects.Count == 0)
            return;

        bool isDurationEnd = false;
        _healthChangeCounter += time;
        if (_healthChangeCounter >= 1)
            _healthChangeCounter = 0;

        foreach (EffectLogic effectImpact in _effects)
        {
            effectImpact.CutDuration(time);

            if (_healthChangeCounter == 0 && effectImpact.HealthPerSecond != 0)
                HealthTic?.Invoke(effectImpact.HealthPerSecond, effectImpact.Caster);

            if (effectImpact.IsEndDuration)
                isDurationEnd = true;
        }

        if (isDurationEnd)
        {
            List<EffectLogic> endedEffects = _effects.Where(effect => effect.IsEndDuration).ToList();

            foreach (EffectLogic effectImpact in endedEffects)
                _effects.Remove(effectImpact);

            CharacteristicsChanged?.Invoke();
        }
    }
}
