using System;
using System.Collections;
using UnityEngine;

public class CharacterFightLogic : IReachLogic
{
    private const float StaminaToAttack = GameSettings.Character.StaminaPointsToAttack;

    private float _attackSpeed;
    private float _currentStamina;
    private float _hitPointsMax;
    private float _hitPointsCurrent;
    private float _armor;
    private float _damage;

    private IFightable _enemy;
    private IFightable _attacker;
    private IAttackLogic _attackLogic;

    public CharacterFightLogic(FighterCharacteristics characteristics, IFightable attacker)
    {
        _hitPointsCurrent = characteristics.HitPoints;
        _hitPointsMax = characteristics.HitPoints;
        _currentStamina = StaminaToAttack;
        ApplyNewCharacteristics(characteristics);
        _attacker = attacker;
    }

    public event Action Died;

    public event Action HitPointsChanged;

    public event Action<float> StaminaChanged;

    public event Action<Target> Reached;

    public float HitPointsCoefficient => _hitPointsCurrent / _hitPointsMax;

    public void ApplyNewCharacteristics(FighterCharacteristics characteristics)
    {
        float tempHitPointsCoefficient = HitPointsCoefficient;
        _hitPointsMax = characteristics.HitPoints;
        _hitPointsCurrent = characteristics.HitPoints * tempHitPointsCoefficient;
        _armor = characteristics.Armor;
        _damage = characteristics.Damage;
        _attackSpeed = characteristics.AttackSpeed;
    }

    public void SetNewAttackLogic(IAttackLogic newLogic)
    {
        if (newLogic == null || newLogic == _attackLogic)
            return;

        _attackLogic = newLogic;
    }

    public bool TryApplyDamage(ref float damage)
    {
        damage = GetRealDamage(damage);

        if (damage <= 0)
            return false;

        _hitPointsCurrent -= damage;

        if (_hitPointsCurrent <= 0)
            Died?.Invoke();

        HitPointsChanged?.Invoke();

        return true;
    }

    public void ApplyHeal(float heal)
    {
        _hitPointsCurrent += heal;
        _hitPointsCurrent = (_hitPointsMax < _hitPointsCurrent) ? _hitPointsMax : _hitPointsCurrent;
        HitPointsChanged.Invoke();
    }

    public void ApplyStamina(int count)
    {
        _currentStamina += count;
        StaminaChanged?.Invoke(_currentStamina);
    }

    public void StaminaRegeneration(float delay)
    {
        if (_currentStamina < StaminaToAttack)
        {
            _currentStamina += _attackSpeed * delay;
            StaminaChanged?.Invoke(_currentStamina);
        }
    }

    public IEnumerator ReachTarget()
    {
        yield return GameSettings.Character.OptimizationDelay;

        while (_enemy != null)
        {
            Attack();
            yield return GameSettings.Character.OptimizationDelay;
        }

        Reached!.Invoke(new Target(Vector3.zero, _enemy));
    }

    public void SetTarget(Target target)
    {
        target.TryGetFightable(out _enemy);
    }

    private float GetRealDamage(float damage)
    {
        float armorImpact = Mathf.Pow(GameSettings.Character.ArmorUnitImpact, _armor);
        return armorImpact * damage;
    }

    private void Attack()
    {
        if (_currentStamina >= StaminaToAttack)
        {
            _attackLogic.AttackEnemy(_attacker, _enemy, _damage);
            _currentStamina -= StaminaToAttack;
            StaminaChanged?.Invoke(_currentStamina);
        }
    }
}
