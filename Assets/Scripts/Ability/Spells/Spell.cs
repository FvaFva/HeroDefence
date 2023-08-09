using System;
using UnityEngine;

public class Spell : ISpellInfo
{
    private float _currentCoolDown;
    private IFightable _caster;

    public Spell(IFightable caster, ISpellSource source)
    {
        _caster = caster;
        Source = source;
    }

    public event Action<float> CoolDownChanged;

    public event Action Casted;

    public ISpellSource Source { get; private set; }

    public Sprite Icon => _preset.Icon;

    public int ManaCost => _preset.ManaCost;

    private SpellPreset _preset => Source.SpellPreset;

    public void ReduceCoolDown(float count)
    {
        if (_currentCoolDown > 0)
        {
            _currentCoolDown -= count;
            CoolDownChanged?.Invoke(_currentCoolDown / _preset.CoolDownSeconds);
        }
    }

    public void Cast()
    {
        if (_currentCoolDown > 0)
            return;

        _preset.Casted += OnPresetCasting;
        _preset.Create(this, _caster);
    }

    private void OnPresetCasting(Spell spell, bool isCasted)
    {
        if (spell == this)
        {
            _preset.Casted -= OnPresetCasting;

            if (isCasted)
                OnCastFinish();
        }
    }

    private void OnCastFinish()
    {
        Casted?.Invoke();
        _currentCoolDown = _preset.CoolDownSeconds;
        CoolDownChanged?.Invoke(_currentCoolDown / _preset.CoolDownSeconds);
    }
}