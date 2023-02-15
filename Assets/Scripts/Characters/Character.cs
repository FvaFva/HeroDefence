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

    private IFightebel _target;
    private CharacterFightLogic _fightLogic;
    private CharacterMoveLogic _moveLogic;
    private CharacterVisionLogic _visionLogic;
    private CharacterPercBag _percBag;
    private CharacterAttackLogic _attackLogic;
    private Anima _anima;
    private CharacterStateMachine _stateMachine;

    private Coroutine _staminaRegeneration;
    private Coroutine _visionAction;

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
        if (newTarget.TryGetFightebel(out IFightebel target))
        {
            CleaOldTargetEnemy();
            _moveLogic!.Reached += StartVisingTarget;

            if (target != null && target.CheckFriendly(_team) == false)
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

    public bool CheckFriendly(Team verifiableTeam)
    {
        return _team == verifiableTeam;
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

    private void CleaOldTargetEnemy()
    {
        if (_target != null)
            _target.TakenDamage -= OnDamageDealing;

        if(_visionAction!= null)
            StopCoroutine(_visionAction);

        _moveLogic!.Reached -= StartVisingTarget;
        _target = null;
    }

    private void StartVisingTarget(Target target)
    {
        if(_visionAction != null)
            StopCoroutine(_visionAction);

        _moveLogic.Reached -= StartVisingTarget;
        _visionLogic.SetTarget(target);
        _visionAction = StartCoroutine(_visionLogic.ReachTarget());
    }

    private void SetNewEnemyTarget(IFightebel target)
    {
        _target = target;
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

        if (_staminaRegeneration != null)
            StopCoroutine(_staminaRegeneration);                
    }

    private void OnDied()
    {
        Died?.Invoke();
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
        _visionLogic.SetVisionDistance(logic.AttackDistance);
        _moveLogic.SetNewDistanceToTarget(logic.AttackDistance);
        _fightLogic.SetNewAttackLogic(_attackLogic);
    }

    private void Init()
    {
        LoadPreset(_preset);
        TryGetComponent<NavMeshAgent>(out NavMeshAgent navigator);
        TryGetComponent<CharacterStateMachine>(out _stateMachine);

        _percBag = new CharacterPercBag();
        _moveLogic = new CharacterMoveLogic(navigator, transform, _team);
        _speedCoefficient = 1;
        _visionLogic = new CharacterVisionLogic(transform, navigator.angularSpeed);
        _anima = new Anima(100, 1);
        _informationUI.SetFlagGolod(TeamFlag);

        UpdateSpeed();
        UpdateAttackLogic(_attackLogic);

        StateMachineLogicBuilder.Build(_stateMachine, _fightLogic, _moveLogic, _visionLogic, _team);
    }

    private void LoadPreset(CharacterPreset preset)
    {        
        _speed      = preset.MoveSpeed;        
        _fightLogic = new CharacterFightLogic(preset.HitPoints, preset.Armor, preset.Damage, preset.AttacSpeed);
        _attackLogic = preset.AttackLogic;

        Name        = preset.Name;
        Profession  = preset.Profission;
        Portrait    = preset.Portrait;
    }    
}
