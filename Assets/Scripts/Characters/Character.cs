using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Character : MonoBehaviour, IPercBag, IFightebel
{    
    private const float SocialDistance = GameSettings.Character.SocialDistance;

    [SerializeField] private CharacterInformation _informationUI;
    [SerializeField] private CharacterPreset _preset;
    [SerializeField] private Team _team;

    private float _speed;
    private float _speedCoefficient;

    private IFightebel _target;
    private CharacterFightLogic _fightLogic;
    private CharacterMoveLogic _moveLogic;
    private CharacterPercBag _percBag = new CharacterPercBag();
    private CharacterAttackLogic _attackLogic;
    private Anima _anima;

    private Coroutine _staminaRegeneration;
    private Coroutine _updateMovePath;
    private Coroutine _attakEnemy;

    public string Name { get; private set; }
    public string Profission { get; private set; }
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

    public void SetNewTarget(Vector3 targtPoint)
    {
        CleaOldTargetEnemy();

        _moveLogic.SetTarget(targtPoint);
    }

    public void SetNewTarget(IFightebel target)
    {
        float distanceToTarget = SocialDistance;
        CleaOldTargetEnemy();

        if (target != null  && target.CheckFriendly(_team) == false)
        {            
            distanceToTarget = _attackLogic.AttackDistance;
            SetNewEnemyTarget(target);
        }

        _moveLogic.SetTarget(target, distanceToTarget);
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

    public void AhowAllInformations()
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
        if (_attakEnemy != null)
            StopCoroutine(_attakEnemy);

        if (_target != null)
            _target.TakenDamage -= OnDamageDealing;

        _target = null;
    }

    private void SetNewEnemyTarget(IFightebel target)
    {
        _target = target;
        _target.TakenDamage += OnDamageDealing;
        _attakEnemy = StartCoroutine(AttackEnemy());
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
        _updateMovePath = StartCoroutine(_moveLogic.UpdatePathToTarget());
        AhowAllInformations();
    }

    private void OnDisable()
    {
        _fightLogic.HitPointsChanged -= ShowHitManaPointsCoefficients;
        _fightLogic.Died -= OnDied;

        _percBag.ShowedPerc -= (OnPercShowing);

        if (_staminaRegeneration != null)
            StopCoroutine(_staminaRegeneration);
        
        if (_updateMovePath != null)
            StopCoroutine(_updateMovePath);

        if (_attakEnemy != null)
            StopCoroutine(_attakEnemy);
    }

    private IEnumerator AttackEnemy()
    {
        while (true)
        {
            if (_moveLogic.IsMove == false)
                _fightLogic.Attack(this, _target);

            yield return GameSettings.Character.OptimizationDelay();
        }
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
        _fightLogic.SetNewAttackLogic(_attackLogic);
    }

    private void Init()
    {
        LoadPreset(_preset);
        TryGetComponent<NavMeshAgent>(out NavMeshAgent _navigator);
        _moveLogic          = new CharacterMoveLogic(_navigator, transform);
        _speedCoefficient   = 1;
        _anima              = new Anima(100, 1);

        _informationUI.SetFlagGolod(TeamFlag);
        UpdateSpeed();
    }

    private void LoadPreset(CharacterPreset preset)
    {        
        _speed      = preset.MoveSpeed;        
        _fightLogic = new CharacterFightLogic(preset.HitPoints, preset.Armor, preset.Damage, preset.AttacSpeed);     
        UpdateAttackLogic(preset.AttackLogic);
        Name        = preset.Name;
        Profission  = preset.Profission;
        Portrait    = preset.Portrait;
    }    
}
