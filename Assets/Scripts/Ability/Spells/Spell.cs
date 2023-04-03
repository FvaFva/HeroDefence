using System.Collections.Generic;
using UnityEngine;

public class Spell:ScriptableObject
{
    [SerializeField] private int _manaCost;
    [SerializeField] private int _coolDownSeconds;
    [SerializeField] private ISpellTargetLogic _targetLogic;
    [SerializeField] private List<ISpellCastLogic> _spellCastLogicList;

    private float _currentCoolDown;
    public int ManaCost => _manaCost;
    public int CoolDownSeconds => _coolDownSeconds;

    private void OnEnable()
    {
        _targetLogic.TargetsSelected += Cast;
    }

    private void OnDisable()
    {
        _targetLogic.TargetsSelected -= Cast;
    }

    public void CoolDownReduct(float timeReduct)
    {
        if (_currentCoolDown > 0)
            _currentCoolDown -= timeReduct;
    }

    private void Cast(List<IFightable> targets)
    {
        if (_currentCoolDown > 0)
            return;

        _currentCoolDown = CoolDownSeconds;

        foreach (IFightable target in targets)
        {
            foreach(ISpellCastLogic castLogic in _spellCastLogicList)
            {
                castLogic.CastAction(target);
            }
        }
    }
}
