using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CharacterStateMachine))]
public class Character : MonoBehaviour, IPercBag, IFightebel
{        
    [SerializeField] private CharacterInformation _informationUI;
    [SerializeField] private CharacterPreset _preset;
    [SerializeField] private Team _team;

    private float _speed;
    private float _speedCoefficient;

    private IFightebel _enemy;
    private CharacterFightLogic _fightLogic;
    private CharacterMoveLogic _moveLogic;
    private CharacterRotationLogic _visionLogic;
    private CharacterPercBag _percBag;
    private CharacterAttackLogic _attackLogic;
    private Anima _anima;
    private CharacterStateMachine _stateMachine;

    private Coroutine _staminaRegeneration;
    private Coroutine _controleDistanceToTarget;
    private Coroutine _controleAGleToTarget;

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
    public event Action<IFightebel, float, bool> TakenDamage;
    public event Action Died;

    public void SetNewTarget(Target newTarget)
    {
        CleaOldTarget();

        if (newTarget.TryGetFightebel(out IFightebel target) && target != null)
        {
            _moveLogic.Reached += StartControlDistanceToTarget;
            _visionLogic.Reached += StartControlAgleToTarget;

            if (target.IsFriendly(_team) == false)
            {                
                SetNewEnemyTarget(target);
            }
        }        
    }

    public void SetNewComander (ICharacterComander comander)
    {
        _stateMachine.SetNewComander(comander);
    }

    public void ApplyDamage(IFightebel attacker, float damage, bool isPercTrigered = true)
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

    public bool IsFriendly(IFightebel verifiableIFightebel)
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

    private void OnDamageDealing(IFightebel enemy, float damage, bool triggeredDamage)
    {
        if (triggeredDamage)
            _percBag.ExecuteActionDepenceAction(this, enemy, damage, PercActionType.OnDamageDelay);
    }

    private void CleaOldTarget()
    {
        if(_controleDistanceToTarget!= null)
            StopCoroutine(_controleDistanceToTarget);

        if (_controleAGleToTarget != null)
            StopCoroutine(_controleAGleToTarget);

        if (_enemy != null)
            _enemy!.TakenDamage -= OnDamageDealing;

        _moveLogic.Reached -= StartControlDistanceToTarget;
        _moveLogic.ChoosedTarget -= StopControlDistanceToTarget;
        _visionLogic.Reached -= StartControlAgleToTarget;
        _visionLogic.ChoosedTarget -= StopControlAgleToTarget;
        _enemy = null;
    }

    private void StartControlDistanceToTarget(Target target)
    {        
        if (_controleDistanceToTarget != null)
            StopCoroutine(_controleDistanceToTarget);

        _moveLogic.ChoosedTarget += StopControlDistanceToTarget;
        _moveLogic.Reached -= StartControlDistanceToTarget;
        _controleDistanceToTarget = StartCoroutine(_moveLogic.ObserveDistanceToTarget());
    }

    private void StartControlAgleToTarget(Target target)
    {
        if (_controleAGleToTarget != null)
            StopCoroutine(_controleAGleToTarget);

        _visionLogic.ChoosedTarget += StopControlAgleToTarget;
        _visionLogic.Reached -= StartControlAgleToTarget;
        _controleAGleToTarget = StartCoroutine(_visionLogic.ObserveRotateToTarget());
    }

    private void StopControlAgleToTarget(Target target)
    {
        StopCoroutine(_controleAGleToTarget);
        _visionLogic.ChoosedTarget -= StopControlAgleToTarget;
        _visionLogic.Reached += StartControlAgleToTarget;
    }

    private void StopControlDistanceToTarget(Target target)
    {        
        StopCoroutine(_controleDistanceToTarget);
        _moveLogic.ChoosedTarget -= StopControlDistanceToTarget;
        _moveLogic.Reached += StartControlDistanceToTarget;
    }

    private void SetNewEnemyTarget(IFightebel target)
    {
        _enemy = target;
        _enemy.TakenDamage += OnDamageDealing;
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
        Died?.Invoke();
        CleaOldTarget();
        gameObject.SetActive(false);        
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
        TryGetComponent<NavMeshAgent>(out NavMeshAgent navigator);
        TryGetComponent<CharacterStateMachine>(out _stateMachine);

        _percBag = new CharacterPercBag();
        _moveLogic = new CharacterMoveLogic(navigator, transform, _team, _preset.Height);
        _speedCoefficient = 1;
        _visionLogic = new CharacterRotationLogic(transform, navigator.angularSpeed);
        _anima = new Anima(100, 1);
        _informationUI.SetFlagGolod(TeamFlag);

        UpdateSpeed();
        UpdateAttackLogic(_attackLogic);
        StateMachineLogicBuilder builder = new StateMachineLogicBuilder();
        builder.Build(_stateMachine, _fightLogic, _moveLogic, _visionLogic, this);
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
