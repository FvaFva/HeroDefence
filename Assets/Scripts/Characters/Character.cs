using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CharacterStateMachine))]
[RequireComponent(typeof(CharacterTargetObserveLogic))]
public class Character : MonoBehaviour, IPercBag, IFightable
{        
    [SerializeField] private CharacterInformation _informationUI;
    [SerializeField] private CharacterPreset _preset;
    [SerializeField] private Team _team;

    private Characteristics _characteristics;
    private CharacterFightLogic _fightLogic;
    private CharacterMoveLogic _moveLogic;
    private CharacterPercBag _percBag;
    private CharacterAnimaLogic _animaLogic;
    private Weapon _baseWeapon;
    private Weapon _currentWeapon;
    private CharacterStateMachine _stateMachine;

    private Coroutine _staminaRegeneration;

    public string Name { get; private set; }
    public string Profession { get; private set; }
    public Sprite Portrait{ get; private set; }
    public Team Team => _team;
    public Vector3 CurrentPosition => transform.position;
    public FighterÑharacteristics Ñharacteristics => _characteristics.Current;

    public event Action<float, float> ChangedIndicators;
    public event Action<FighterÑharacteristics> ChangedCharacteristics;
    public event Action<Perc> ShowedPerc;
    public event Action<Perc> RemovedPerc;
    public event Action Died;

    public void SetNewComander (ITargetChooser comander)
    {
        _stateMachine.SetNewComander(comander);
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
        _percBag.ShowPercs();
        UpdateLogicsCharacteristics();
    }

    public void AddPerc(Perc perc)
    {
        ShowedPerc?.Invoke(perc);
        _percBag.AddPerc(perc);
    }

    public void RemovePerc(Perc perc)
    {
        if(_percBag.TryRemovePerc(perc))
        {
            RemovedPerc?.Invoke(perc);
        }
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
        _percBag.ShowedPerc += OnPercShowing;
        _staminaRegeneration = StartCoroutine(_fightLogic.Resting());
        _characteristics.CharacteristicsChanged += UpdateLogicsCharacteristics;
        ShowAllInformations();
    }

    private void OnDisable()
    {
        _fightLogic.HitPointsChanged -= ShowIndicators;
        _fightLogic.Died -= OnDied;
        _percBag.ShowedPerc -= OnPercShowing;
        _characteristics.CharacteristicsChanged -= UpdateLogicsCharacteristics;

        if (_staminaRegeneration != null)
            StopCoroutine(_staminaRegeneration);                
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

    private void OnPercShowing(Perc showePerc)
    {
        ShowedPerc?.Invoke(showePerc);
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
            _characteristics.RemoveBuff(weapon);
        }
        
        if(weapon == null)
            _currentWeapon = _baseWeapon;
        else
            _currentWeapon = weapon;

        _currentWeapon.AttackLogic.DamageDealed += OnDamageDealing;
        _moveLogic.SetNewDistanceToEnemy(_currentWeapon.AttackDistance);
        _fightLogic.SetNewAttackLogic(_currentWeapon.AttackLogic);
        _characteristics.ApplyBuff(weapon);
        UpdateLogicsCharacteristics();
    }

    private void Init()
    {
        LoadPreset(_preset);
        TryGetComponent(out NavMeshAgent navigator);
        TryGetComponent(out _stateMachine);
        TryGetComponent(out CharacterTargetObserveLogic targetObserver);
        
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

    private void LoadPreset(CharacterPreset preset)
    {        
        _characteristics = new Characteristics(preset.GetCharacteristics());
        _characteristics.CharacteristicsChanged += UpdateLogicsCharacteristics;
        _fightLogic = new CharacterFightLogic(_characteristics.Current, this);
        _animaLogic = new CharacterAnimaLogic(_characteristics.Current);

        _baseWeapon = preset.Weapon;
        Name        = preset.Name;
        Profession  = preset.Profission;
        Portrait    = preset.Portrait;
    }    
}
