using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Character : MonoBehaviour, IPercBag
{    
    [SerializeField] private CharacterInformation _informationUI;
    [SerializeField] private CharacterPreset _preset;
    [SerializeField] private Team _team;

    private float _speed;
    private float _speedCoefficient;
    private float _hitPointsCurrentCoefficient = 1;

    private Character _targetEnemy;
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
    public bool IsLive { get; private set; }
    public Sprite Portrait{ get; private set; }
    public string TeamName => _team.Name;
    public Color TeamFlag => _team.Flag;

    public event Action<float, float> ChangedIndicators;
    public event Action<float> ChangedSpeed;
    public event Action<float, float, float> ChangedCharacteristics;
    public event Action SetMainTarget;
    public event Action<Perc> ShowedPerc;
    public event Action<Perc> RemovedPerc;
    public event Action<Character ,float, bool> TakenDamage;

    public void SetNewTarget(Vector3 targtPoint)
    {
        CleaOldTargetEnemy();

        _moveLogic.SetTarget(targtPoint);
    }

    public void SetNewTarget(Character target)
    {        
        float socialDistance = GameSettings.Character.SocialDistance;
        CleaOldTargetEnemy();

        if (target != null && target != this && target.CheckFrendly(_team) == false)
        {            
            socialDistance = _attackLogic.AttackDistance;
            SetNewEnemyTarget(target);
        }

        _moveLogic.SetTarget(target, socialDistance);
    }

    public void TakeDamage(Character attacker, float damage, bool isPercTrigered = true)
    {
        bool isDamageTaken = _fightLogic.TryApplyDamage(ref damage, attacker);

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

    public bool CheckFrendly(Team team)
    {
        return _team == team;
    }

    public void SetThisCurrentCharacter()
    {        
        float manaPointsCoeffecient = _anima.ManaPointsCurrent / _anima.ManaPointsMax;
        _informationUI.SetCurrentCharacteristics(_hitPointsCurrentCoefficient, manaPointsCoeffecient);
        ChangedIndicators?.Invoke(_hitPointsCurrentCoefficient, manaPointsCoeffecient);
        ChangedCharacteristics?.Invoke(_preset.Damage, _preset.Armor, _preset.AttacSpeed);
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

    private void OnDamageDealing(Character enemy, float damage, bool triggeredDamage)
    {
        if (triggeredDamage)
            _percBag.ExecuteActionDepenceAction(this, enemy, damage, PercActionType.OnDamageDelay);
    }

    private void CleaOldTargetEnemy()
    {
        if (_attakEnemy != null)
            StopCoroutine(_attakEnemy);

        if (_targetEnemy != null)
            _targetEnemy.TakenDamage -= OnDamageDealing;

        _targetEnemy = null;
    }

    private void SetNewEnemyTarget(Character target)
    {
        _targetEnemy = target;
        _targetEnemy.TakenDamage += OnDamageDealing;
        _attakEnemy = StartCoroutine(AttackEnemy());
    }

    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {        
        _fightLogic.HitPointsChanged += UpdateHitPointsCoefficient;
        _fightLogic.Died += OnDeth;

        _percBag.ShowedPerc += OnPercShowing;

        _staminaRegeneration = StartCoroutine(_fightLogic.Resting());
        _updateMovePath = StartCoroutine(_moveLogic.UpdatePathToTarget());
        SetThisCurrentCharacter();
    }

    private void OnDisable()
    {
        _fightLogic.HitPointsChanged -= UpdateHitPointsCoefficient;
        _fightLogic.Died -= OnDeth;

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
        while (_targetEnemy.IsLive)
        {
            if (_moveLogic.IsMove == false)
                _fightLogic.Attack(this, _targetEnemy);

            yield return GameSettings.Character.OptimizationDelay();
        }

        SetNewTarget(transform.position);
    }

    private void OnDeth()
    {
        IsLive = false;
        gameObject.SetActive(false);        
    }

    private void UpdateHitPointsCoefficient(float newCoefficient)
    {
        _hitPointsCurrentCoefficient = newCoefficient;
        SetThisCurrentCharacter();
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
        IsLive              = true;
        _anima              = new Anima(100, 1);

        _informationUI.SetFlagGolod(TeamFlag);
        UpdateSpeed();
    }

    private void LoadPreset(CharacterPreset preset)
    {        
        _speed              = preset.MoveSpeed;        
        _fightLogic     = new CharacterFightLogic(preset.HitPoints, preset.Armor, preset.Damage, preset.AttacSpeed);     
        UpdateAttackLogic(preset.AttackLogic);
        Name        = preset.Name;
        Profission  = preset.Profission;
        Portrait    = preset.Portrait;
    }    
}
