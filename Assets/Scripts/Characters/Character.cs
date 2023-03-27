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

    private Ammunition _ammunition = new Ammunition();
    private Characteristics _characteristics;
    private CharacterFightLogic _fightLogic;
    private CharacterMoveLogic _moveLogic;
    private CharacterAnimaLogic _animaLogic;
    private CharacterPercBag _percBag;

    private Weapon _baseWeapon;
    private Weapon _currentWeapon;

    private CharacterStateMachine _stateMachine;
    private Coroutine _resting;

    public string Name { get; private set; }
    public string Profession { get; private set; }
    public Sprite Portrait{ get; private set; }
    public Team Team => _team;

    public Vector3 CurrentPosition => transform.position;

    public event Action<float, float> ChangedIndicators;
    public event Action<Fighter—haracteristics> ChangedCharacteristics;
    public event Action<IReadOnlyList<Ability>> ChangedAbilitiesKit;
    public event Action<IReadOnlyDictionary<ItemType, Item>> ChangedAmmunition;
    public event Action Died;

    public void SetNewComander (ITargetChooser comander)
    {
        _stateMachine.SetNewComander(comander);
    }

    public void ApplyStamina(int count)
    {

    }

    public bool TryApplyDamage(IFightable attacker,ref float damage, bool isPercTrigered = true)
    {
        bool isDamageTaken = _fightLogic.TryApplyDamage(ref damage);

        if (isDamageTaken)
        {
            _percBag.ExecuteActionDepenceAction(this, attacker, damage, PercActionType.OnDefence);
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

    public bool IsFriendly(IFightable verifiableIFightebel)
    {
        return verifiableIFightebel.IsFriendly(_team);
    }

    public void ShowAllInformations()
    {                
        ChangedCharacteristics?.Invoke(_characteristics.Current);
        ChangedAbilitiesKit?.Invoke(_percBag.Percs);
        ChangedAmmunition?.Invoke(_ammunition.ThingsWorn);
        ShowIndicators();
    }

    public void RemovePerc(IPercSource source, Perc perc)
    {
        if (_percBag.TryRemovePerc(source, perc))
            ChangedAbilitiesKit?.Invoke(_percBag.Percs);
    }

    public bool TryDropItem(ItemType itemType, out Item dropItem)
    {
        if (_ammunition.TryDropType(itemType, out dropItem))
        {
            if (_percBag.TryRemovePerc(dropItem, dropItem.Perc))
                ChangedAbilitiesKit?.Invoke(_percBag.Percs);

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
        if(_ammunition.TryPutOnItem(item))
        {
            if (_percBag.TryAddPerc(item, item.Perc))
                ChangedAbilitiesKit?.Invoke(_percBag.Percs);

            _characteristics?.ApplyBuff(item);

            if (item.ItemType == ItemType.Weapon)
                SetNewWeapon((Weapon)item);

            ChangedAmmunition?.Invoke(_ammunition.ThingsWorn);

            return true;
        }

        return false;
    }

    private void OnDamageDealing(IFightable enemy, float damage, bool triggeredDamage)
    {
        if (triggeredDamage)
            _percBag.ExecuteActionDepenceAction(this, enemy, damage, PercActionType.OnDamageDelay);
    }

    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {        
        _fightLogic.HitPointsChanged += ShowIndicators;
        _fightLogic.Died += OnDied;
        _resting = StartCoroutine(Resting());
        _characteristics.CharacteristicsChanged += UpdateLogicsCharacteristics;
        ShowAllInformations();
    }

    private void OnDisable()
    {
        _fightLogic.HitPointsChanged -= ShowIndicators;
        _fightLogic.Died -= OnDied;
        _characteristics.CharacteristicsChanged -= UpdateLogicsCharacteristics;

        if (_resting != null)
            StopCoroutine(_resting);                
    }

    private void OnDied()
    {
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
        {
            _currentWeapon.AttackLogic.DamageDealed -= OnDamageDealing;
        }
        
        if(weapon == null)
            _currentWeapon = _baseWeapon;
        else
            _currentWeapon = weapon;

        _currentWeapon.AttackLogic.DamageDealed += OnDamageDealing;
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
        _moveLogic = new CharacterMoveLogic(navigator, transform, _team, _preset.Height);
        
        _informationUI.SetFlagGolod(_team.Flag);
        targetObserver.Init(_moveLogic);

        UpdateLogicsCharacteristics();
        SetNewWeapon(_baseWeapon);
        AllLogics logics = new AllLogics(_fightLogic, new CharacterDieingLogic(transform, _characteristics.Current.Speed), targetObserver, _moveLogic);
        StateMachineLogicBuilder builder = new StateMachineLogicBuilder();

        builder.Build(_stateMachine, logics, this);
    }

    private IEnumerator Resting()
    {
        float delay = GameSettings.Character.SecondsDelay;
        yield return GameSettings.Character.OptimizationDelay;

        while (true)
        {
            _fightLogic.StaminaRegeneration(delay);
            _animaLogic.ManaRegeneration(delay);
            yield return GameSettings.Character.OptimizationDelay;
        }    
    }

    private void LoadPreset(CharacterPreset preset)
    {        
        _characteristics = new Characteristics(preset.GetCharacteristics());
        _characteristics.CharacteristicsChanged += UpdateLogicsCharacteristics;
        _fightLogic = new CharacterFightLogic(_characteristics.Current, this);
        _animaLogic = new CharacterAnimaLogic(_characteristics.Current);

        _baseWeapon = preset.Weapon;
        Name        = preset.Name;
        Profession  = preset.Profession;
        Portrait    = preset.Portrait;
    }    
}
