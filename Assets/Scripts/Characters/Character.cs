using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CharacterStateMachine))]
[RequireComponent(typeof(CharacterTargetObserveLogic))]
public class Character : MonoBehaviour, IFightable
{
    [SerializeField] private CharacterIndicatorsPanel _informationUI;
    [SerializeField] private CharacterPreset _preset;
    [SerializeField] private Team _team;
    [SerializeField] private PercImpactPlayer _impactEffectPlayer;

    private Ammunition _ammunition = new Ammunition();
    private Characteristics _characteristics;
    private CharacterFightLogic _fightLogic;
    private CharacterMoveLogic _moveLogic;
    private CharacterAnimaLogic _animaLogic;
    private CharacterPercBag _percBag;
    private CharacterEffectBug _effectBug;

    private Weapon _baseWeapon;
    private Weapon _currentWeapon;

    private CharacterStateMachine _stateMachine;
    private Coroutine _resting;

    public event Action<float, float> ChangedIndicators;

    public event Action<float> StaminaChanged;

    public event Action<FighterCharacteristics> ChangedCharacteristics;

    public event Action<IReadOnlyList<Ability>> ChangedAbilitiesKit;

    public event Action<IReadOnlyList<EffectLogic>> ChangedEffectsKit;

    public event Action<IReadOnlyDictionary<ItemType, Item>> ChangedAmmunition;

    public event Action Died;

    public string Name { get; private set; }

    public string Profession { get; private set; }

    public Sprite Portrait { get; private set; }

    public Team Team => _team;

    public Vector3 CurrentPosition => transform.position;

    public void ShowColoredEffectImpact(Color color)
    {
        _impactEffectPlayer.ShowColoredEffect(color);
    }

    public void SetNewCommander(ITargetChooser commander)
    {
        _stateMachine.SetNewCommander(commander);
    }

    public void ApplyStamina(int count)
    {
        _fightLogic.ApplyStamina(count);
    }

    public void ApplyEffect(EffectLogic effect)
    {
        effect.Calculate(_characteristics.Current);
        _effectBug.ApplyEffect(effect);
    }

    public bool TryApplyDamage(IFightable attacker, ref float damage, bool isPercTriggered = true)
    {
        bool isDamageTaken = _fightLogic.TryApplyDamage(ref damage);

        if (isDamageTaken)
        {
            _percBag.ExecuteDependentAction(this, attacker, damage, PercActionType.OnDefense);
        }

        return isDamageTaken;
    }

    public void ApplyHeal(float heal)
    {
        _fightLogic.ApplyHeal(heal);
    }

    public bool IsFriendly(Team verifiableTeam)
    {
        return _team == verifiableTeam;
    }

    public bool IsFriendly(IFightable verifiableIFightable)
    {
        return verifiableIFightable.IsFriendly(_team);
    }

    public void ShowAllInformation()
    {
        ChangedCharacteristics?.Invoke(_characteristics.Current);
        ChangedAbilitiesKit?.Invoke(_percBag.Perks);
        ChangedAmmunition?.Invoke(_ammunition.ThingsWorn);
        ChangedEffectsKit?.Invoke(_effectBug.Effects);
        ShowIndicators();
    }

    public bool TryDropItem(ItemType itemType, out Item dropItem)
    {
        if (_ammunition.TryDropType(itemType, out dropItem))
        {
            RemovePerc(dropItem);

            _characteristics?.RemoveBuff(dropItem);

            if (itemType == ItemType.Weapon)
                SetNewWeapon(_baseWeapon);

            ChangedAmmunition?.Invoke(_ammunition.ThingsWorn);

            return true;
        }

        return false;
    }

    public bool TryPutOnItem(Item item)
    {
        if (_ammunition.TryPutOnItem(item))
        {
            if (_percBag.TryAddPerc(item))
                ChangedAbilitiesKit?.Invoke(_percBag.Perks);

            _characteristics.ApplyBuff(item);

            if (item.ItemType == ItemType.Weapon)
                SetNewWeapon((Weapon)item);

            ChangedAmmunition?.Invoke(_ammunition.ThingsWorn);

            return true;
        }

        return false;
    }

    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        _fightLogic.HitPointsChanged += ShowIndicators;
        _fightLogic.Died += OnDied;
        _fightLogic.StaminaChanged += OnStaminaChanged;
        _characteristics.CharacteristicsChanged += UpdateLogicsCharacteristics;
        _effectBug.CharacteristicsChanged += OnEffectsChange;
        _effectBug.HealthTic += ApplyHealthChangeFromEffect;

        _resting = StartCoroutine(Resting());
        ShowAllInformation();
    }

    private void OnDisable()
    {
        _fightLogic.HitPointsChanged -= ShowIndicators;
        _fightLogic.Died -= OnDied;
        _fightLogic.StaminaChanged -= OnStaminaChanged;
        _characteristics.CharacteristicsChanged -= UpdateLogicsCharacteristics;
        _effectBug.CharacteristicsChanged -= OnEffectsChange;
        _effectBug.HealthTic -= ApplyHealthChangeFromEffect;

        if (_resting != null)
            StopCoroutine(_resting);
    }

    private void ApplyHealthChangeFromEffect(float healthChange, IFightable source)
    {
        if (healthChange > 0)
            TryApplyDamage(source, ref healthChange, false);
        else
            ApplyHeal(healthChange);
    }

    private void RemovePerc(IPercSource source)
    {
        if (_percBag.TryRemovePerc(source))
            ChangedAbilitiesKit?.Invoke(_percBag.Perks);
    }

    private void OnEffectsChange()
    {
        _characteristics.RemoveBuff(_effectBug);
        _characteristics.ApplyBuff(_effectBug);
        ChangedEffectsKit?.Invoke(_effectBug.Effects);
    }

    private void OnDamageDealing(IFightable enemy, float damage, bool triggeredDamage)
    {
        if (triggeredDamage)
            _percBag.ExecuteDependentAction(this, enemy, damage, PercActionType.OnDamageDelay);
    }

    private void OnDied()
    {
        _moveLogic.OffNavMashAgent();
        Died?.Invoke();
    }

    private void ShowIndicators()
    {
        _informationUI.SetCurrentIndicators(_fightLogic.HitPointsCoefficient, _animaLogic.ManaPointsCoefficient);
        ChangedIndicators?.Invoke(_fightLogic.HitPointsCoefficient, _animaLogic.ManaPointsCoefficient);
    }

    private void UpdateLogicsCharacteristics()
    {
        _moveLogic.SetMoveSpeed(_characteristics.Current.Speed);
        _fightLogic.ApplyNewCharacteristics(_characteristics.Current);
        _animaLogic.ApplyNewCharacteristics(_characteristics.Current);
        ChangedCharacteristics?.Invoke(_characteristics.Current);
        ShowIndicators();
    }

    private void SetNewWeapon(Weapon weapon)
    {
        if (_currentWeapon != null)
            _currentWeapon.AttackLogic.DamageDealt -= OnDamageDealing;

        if (weapon == null)
            _currentWeapon = _baseWeapon;
        else
            _currentWeapon = weapon;

        _currentWeapon.AttackLogic.DamageDealt += OnDamageDealing;
        _moveLogic.SetNewDistanceToEnemy(_currentWeapon.AttackDistance);
        _fightLogic.SetNewAttackLogic(_currentWeapon.AttackLogic);
    }

    private void Init()
    {
        LoadPreset(_preset);
        TryGetComponent(out NavMeshAgent navigator);
        TryGetComponent(out CharacterTargetObserveLogic targetObserver);
        TryGetComponent(out _stateMachine);

        _percBag = new CharacterPercBag();
        _effectBug = new CharacterEffectBug();
        _moveLogic = new CharacterMoveLogic(navigator, transform, _team, _preset.Height);

        _informationUI.SetFlagColor(_team.Flag);
        targetObserver.Init(_moveLogic);

        UpdateLogicsCharacteristics();
        SetNewWeapon(_baseWeapon);
        AllLogics logics = new AllLogics(_fightLogic, new CharacterDyeingLogic(transform, _characteristics.Current.Speed), targetObserver, _moveLogic);
        StateMachineLogicBuilder builder = new StateMachineLogicBuilder();

        builder.Build(_stateMachine, logics, this);
    }

    private IEnumerator Resting()
    {
        float delay = GameSettings.Character.SecondsDelay;
        yield return GameSettings.Character.OptimizationDelay;

        while (true)
        {
            _effectBug.UpdateDuration(delay);
            _fightLogic.StaminaRegeneration(delay);
            _animaLogic.RestingAnima(delay);
            yield return GameSettings.Character.OptimizationDelay;
        }
    }

    private void LoadPreset(CharacterPreset preset)
    {
        _characteristics = new Characteristics(preset.GetCharacteristics());
        _characteristics.CharacteristicsChanged += UpdateLogicsCharacteristics;
        _fightLogic = new CharacterFightLogic(_characteristics.Current, this);
        _animaLogic = new CharacterAnimaLogic(_characteristics.Current, this);

        _baseWeapon = preset.Weapon;
        Name = preset.Name;
        Profession = preset.Profession;
        Portrait = preset.Portrait;
    }

    private void OnStaminaChanged(float currentStamina)
    {
        StaminaChanged?.Invoke(currentStamina);
    }
}
