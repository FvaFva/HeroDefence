using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
public class Character : MonoBehaviour
{    
    [SerializeField] private CharacterInformation _informationUI;
    [SerializeField] private CharacterPreset _preset;

    private float _speed;
    private float _speedCoefficient;
    private float _hitPointsCurrentCoefficient = 1;
    private CharacterFightLogic _fightLogic;
    private CharacterMoveLogic _moveLogic;
    private Anima _anima;
    private AttackLogic _attackLogic;
    private List<EffectImpact> _effects = new List<EffectImpact>();

    private Coroutine _staminaRegeneration;
    private Coroutine _updateMovePath;
    private Coroutine _attakEnemy;

    public string Name { get; private set; }
    public bool IsLive { get; private set; }
    public Sprite Portrait{ get; private set; }
    
    [SerializeField] public Team Team;

    public UnityEvent<float, float> Changed—haracteristics = new UnityEvent<float, float>();
    public UnityEvent SetMainTarget = new UnityEvent();

    public void SetNewTarget(Vector3 targtPoint)
    {        
        if(_attakEnemy != null)
            StopCoroutine(_attakEnemy);

        _moveLogic.SetTarget(targtPoint);
    }

    public void SetNewTarget(Character target)
    {        
        float socialDistance = GameSettings.Character.SocialDistance;

        if (target != null && target != this && target.Team != Team)
        {            
            socialDistance = _attackLogic.AttackDistance;
            _attakEnemy = StartCoroutine(AttackEnemy(target));
        }

        _moveLogic.SetTarget(target, socialDistance);
    }

    public bool ApplyDamage(float damage)
    {
        return _fightLogic.ApplyDamage(damage);
    }

    public void ApplyHeal(float heal)
    {
        _fightLogic.ApplyHeal(heal);
    }

    public void NotifyChanged—haracteristics()
    {        
        float manaPointsCoeffecient = _anima.ManaPointsCurrent / _anima.ManaPointsMax;
        _informationUI.SetCurrentCharacteristics(_hitPointsCurrentCoefficient, manaPointsCoeffecient);
        Changed—haracteristics.Invoke(_hitPointsCurrentCoefficient, manaPointsCoeffecient);
    }

    private void Awake()
    {
        LoadPreset(_preset);
    }

    private void OnEnable()
    {        
        _fightLogic.ImpactingEffect.AddListener(AddEffectImpact);
        _fightLogic.HitPointsChanged.AddListener(UpdateHitPointsCoefficient);
        _fightLogic.Died.AddListener(OnDeth);
        _staminaRegeneration = StartCoroutine(_fightLogic.Resting());
        _updateMovePath = StartCoroutine(_moveLogic.UpdatePathToTarget());
        NotifyChanged—haracteristics();
    }

    private void OnDisable()
    {
        _fightLogic.ImpactingEffect.RemoveListener(AddEffectImpact);
        _fightLogic.HitPointsChanged.RemoveListener(UpdateHitPointsCoefficient);
        _fightLogic.Died.RemoveListener(OnDeth);
        
        if (_staminaRegeneration != null)
            StopCoroutine(_staminaRegeneration);
        
        if (_updateMovePath != null)
            StopCoroutine(_updateMovePath);

        if (_attakEnemy != null)
            StopCoroutine(_attakEnemy);
    }

    private void UpdateHitPointsCoefficient(float newCoefficient)
    {
        _hitPointsCurrentCoefficient = newCoefficient;
        NotifyChanged—haracteristics();
    }    

    private void OnDeth()
    {
        IsLive = false;
        gameObject.SetActive(false);        
    }

    private IEnumerator AttackEnemy(Character enemy)
    {
        while (enemy.IsLive)
        {
            if (_moveLogic.IsMove == false)
                _fightLogic.Attack(enemy);

            yield return GameSettings.Character.OptimizationDelay();
        }

        SetNewTarget(this.transform.position);
    }

    private void AddEffectImpact(EffectImpact effect)
    {
        _effects.Add(effect);
        effect.EndingEffctDuration.AddListener(EndEffcetTime);
    }

    private void EndEffcetTime(EffectImpact effect)
    {
        effect.EndingEffctDuration.RemoveListener(EndEffcetTime);
        _effects.Remove(effect);
    }

    private void UpdateSpeed()
    {
        float currentSpeed = _speed * _speedCoefficient;
        _moveLogic.SetMoveSpeed(currentSpeed);
    } 

    private void UpdateAttackLogic(AttackLogic logic)
    {
        _attackLogic = logic;
        _fightLogic.SetNewAttackLogic(_attackLogic);
    }

    private void LoadPreset(CharacterPreset preset)
    {
        TryGetComponent<NavMeshAgent>(out NavMeshAgent _navigator);
        _moveLogic          = new CharacterMoveLogic(_navigator, transform);
        _speed              = preset.MoveSpeed;
        _speedCoefficient   = 1;
        UpdateSpeed();
        
        _fightLogic     = new CharacterFightLogic(preset.HitPoints, preset.Armor, preset.Damage, preset.AttacSpeed);        
        IsLive          = true;
        UpdateAttackLogic(preset.AttackLogic);

        Name        = preset.Name;
        Portrait    = preset.Portrait;

        _anima = new Anima(100, 1);
    }
}
