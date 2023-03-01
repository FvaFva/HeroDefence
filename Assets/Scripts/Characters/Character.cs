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

    private float _speed;
    private float _speedCoefficient;

    private CharacterFightLogic _fightLogic;
    private CharacterMoveLogic _moveLogic;
    private CharacterPercBag _percBag;
    private CharacterAttackLogic _attackLogic;
    private TempBeta_Anima _anima;
    private CharacterStateMachine _stateMachine;
    private NavMeshAgent _navigator;

    private Coroutine _staminaRegeneration;
    private IFightable _target;

    public string Name { get; private set; }
    public string Profession { get; private set; }
    public Sprite Portrait{ get; private set; }
    public string TeamName => _team.Name;
    public Color TeamFlag => _team.Flag;
    public Vector3 CurrentPosition => transform.position;

    public event Action<float, float> ChangedIndicators;
    public event Action<float> ChangedSpeed;
    public event Action<FighterÑharacteristics> ChangedCharacteristics;
    public event Action SetMainTarget;
    public event Action<Perc> ShowedPerc;
    public event Action<Perc> RemovedPerc;
    public event Action<IFightable, float, bool> TakenDamage;
    public event Action Died;

    public void SetNewTarget(Target newTarget)
    {
        CleaOldTarget();
        
        if (newTarget.TryGetFightebel(out IFightable target) && target != null)
        {
            SetNewTarget(target);
        }
    }

    public void SetNewComander (ITargetChooser comander)
    {
        _stateMachine.SetNewComander(comander);
    }

    public void ApplyDamage(IFightable attacker, float damage, bool isPercTrigered = true)
    {
        bool isDamageTaken = _fightLogic.TryApplyDamage(ref damage);

        TakenDamage?.Invoke(this, damage, isPercTrigered);

        if (isDamageTaken)
        {
            _percBag.ExecuteActionDepenceAction(this, attacker, damage, PercActionType.OnDefence);
        }
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
        _informationUI.SetCurrentCharacteristics(_fightLogic.HiPointsCoefficient, _anima.CurrentMPCoefficient);
        ChangedIndicators?.Invoke(_fightLogic.HiPointsCoefficient, _anima.CurrentMPCoefficient);
        ChangedCharacteristics?.Invoke(_fightLogic.Ñharacteristics);
        _percBag.ShowPercs();
        SetMainTarget?.Invoke();
        UpdateSpeed();
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
            RemovedPerc(perc);
            RemovedPerc?.Invoke(perc);
        }
    }

    private void OnDamageDealing(IFightable enemy, float damage, bool triggeredDamage)
    {
        if (triggeredDamage)
            _percBag.ExecuteActionDepenceAction(this, enemy, damage, PercActionType.OnDamageDelay);
    }

    private void CleaOldTarget()
    {    
        if (_target != null && _target.IsFriendly(_team) == false)
            _target!.TakenDamage -= OnDamageDealing;

        _target = null;
    }

    private void SetNewTarget(IFightable target)
    {
        _target = target;

        if (target.IsFriendly(_team) == false)
            _target.TakenDamage += OnDamageDealing;
    }

    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {        
        _fightLogic.HitPointsChanged += ShowHitManaPointsCoefficients;
        _fightLogic.Died += OnDied;
        _percBag.ShowedPerc += OnPercShowing;
        _staminaRegeneration = StartCoroutine(_fightLogic.Resting());
        ShowAllInformations();
    }

    private void OnDisable()
    {
        _fightLogic.HitPointsChanged -= ShowHitManaPointsCoefficients;
        _fightLogic.Died -= OnDied;
        _percBag.ShowedPerc -= (OnPercShowing);
        CleaOldTarget();

        if (_staminaRegeneration != null)
            StopCoroutine(_staminaRegeneration);                
    }

    private void OnDied()
    {
        _navigator.enabled = false;     
        Died?.Invoke();
    }

    private void ShowHitManaPointsCoefficients()
    {
        _informationUI.SetCurrentCharacteristics(_fightLogic.HiPointsCoefficient, _anima.CurrentMPCoefficient);
        ChangedIndicators?.Invoke(_fightLogic.HiPointsCoefficient, _anima.CurrentMPCoefficient);
    }    

    private void OnPercShowing(Perc showePerc)
    {
        ShowedPerc?.Invoke(showePerc);
    }

    private void UpdateSpeed()
    {
        float currentSpeed = _speed * _speedCoefficient;
        _moveLogic.SetMoveSpeed(currentSpeed);
        ChangedSpeed?.Invoke(currentSpeed);
    } 

    private void UpdateAttackLogic(CharacterAttackLogic logic)
    {
        _attackLogic = logic;
        _moveLogic.SetNewDistanceToTarget(logic.AttackDistance);
        _fightLogic.SetNewAttackLogic(_attackLogic);
    }

    private void Init()
    {
        LoadPreset(_preset);
        TryGetComponent(out _navigator);
        TryGetComponent(out _stateMachine);
        TryGetComponent(out CharacterTargetObserveLogic targetObserver);

        _percBag = new CharacterPercBag();
        _moveLogic = new CharacterMoveLogic(_navigator, transform, _team, _preset.Height);
        targetObserver.Init(_moveLogic);
        _speedCoefficient = 1;
        _anima = new TempBeta_Anima(100, 1);
        _informationUI.SetFlagGolod(TeamFlag);

        UpdateSpeed();
        UpdateAttackLogic(_attackLogic);
        AllLogics logics = new AllLogics(_fightLogic, new CharacterDieingLogic(transform, _speed), targetObserver, _moveLogic);
        StateMachineLogicBuilder builder = new StateMachineLogicBuilder();
        builder.Build(_stateMachine, logics, this);
    }

    private void LoadPreset(CharacterPreset preset)
    {        
        _speed      = preset.MoveSpeed;        
        _fightLogic = new CharacterFightLogic(preset.HitPoints, preset.Armor, preset.Damage, preset.AttacSpeed, this);
        _attackLogic = preset.AttackLogic;

        Name        = preset.Name;
        Profession  = preset.Profission;
        Portrait    = preset.Portrait;
    }    
}
