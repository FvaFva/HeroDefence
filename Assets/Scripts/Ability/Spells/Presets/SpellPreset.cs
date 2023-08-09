using System;
using System.Collections.Generic;
using UnityEngine;

public class SpellPreset : ScriptableObject
{
    [SerializeField] private int _manaCost;
    [SerializeField] private int _coolDownSeconds;
    [SerializeField] private int _castRange;
    [SerializeField] private int _castRadius;
    [SerializeField] private Sprite _icon;
    [SerializeField] private bool _isForEnemy;
    [SerializeField] private BaseSpellTargetLogic _targetLogic;
    [SerializeField] private List<BaseSpellCastLogic> _spellCastLogicList;

    public event Action<Spell, bool> Casted;

    public int CoolDownSeconds => _coolDownSeconds;

    public int ManaCost => _manaCost;

    public Sprite Icon => _icon;

    public void Create(Spell spell, IFightable caster)
    {
        _targetLogic.SelectTargets(spell, _castRange, _castRadius, _isForEnemy, caster);
    }

    private void OnEnable()
    {
        _targetLogic.TargetsSelected += Cast;
    }

    private void OnDisable()
    {
        _targetLogic.TargetsSelected -= Cast;
    }

    private void Cast(bool isCasted, List<IFightable> targets, Spell spell, IFightable caster)
    {
        Casted?.Invoke(spell, isCasted);

        if (isCasted)
        {
            foreach (IFightable target in targets)
            {
                foreach (BaseSpellCastLogic castLogic in _spellCastLogicList)
                    castLogic.CastAction(target, caster);
            }
        }
    }
}
