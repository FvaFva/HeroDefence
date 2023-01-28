using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
public class Character : MonoBehaviour
{    
    [SerializeField] private CharacterInformation _informationUI;
    [SerializeField] private float _speed;
    
    private float _speedCoefficient;
    private float _hitPointsCoeffecient = 1;
    private CombatUnit _unit;
    private Anima _anima;
    private float _attacDistance = 3f;
    private List<EffectImpact> _effects = new List<EffectImpact>();
    private MoveSistem _moveSistem;
    private Coroutine _staminaRegeneration;
    private Coroutine _updateMovePath;
    private Coroutine _attakEnemy;
    private WaitForSeconds _attackDelay = new WaitForSeconds(0.1f);

    [SerializeField] public string Name;
    [SerializeField] public Sprite Portrait;
    [SerializeField] public Team Team;

    public UnityEvent<float, float> Changed—haracteristics = new UnityEvent<float, float>();
    public UnityEvent SetMainTarget = new UnityEvent();
    public bool IsLive { get; private set; }

    public void SetNewTarget(Vector3 targtPoint)
    {        
        if(_attakEnemy != null)
            StopCoroutine(_attakEnemy);

        _moveSistem.SetTarget(targtPoint);
    }

    public void SetNewTarget(Character targetEnemy)
    {        
        if(targetEnemy != null && targetEnemy != this)
            _attakEnemy = StartCoroutine(AttackEnemy(targetEnemy));

        _moveSistem.SetTarget(targetEnemy);
    }

    public void ApplyDamage(float damage)
    {
        _unit.ApplyDamage(damage);
    }

    public void ApplyHeal(float heal)
    {
        _unit.ApplyHeal(heal);
    }

    public void NotifyChanged—haracteristics()
    {        
        float manaPointsCoeffecient = _anima.ManaPointsCurrent / _anima.ManaPointsMax;
        _informationUI.SetCurrentCharacteristics(_hitPointsCoeffecient, manaPointsCoeffecient);
        Changed—haracteristics.Invoke(_hitPointsCoeffecient, manaPointsCoeffecient);
    }

    private void Start()
    {
        NotifyChanged—haracteristics();
    }

    private void Awake()
    {
        TryGetComponent<NavMeshAgent>(out NavMeshAgent _navigator);
        _moveSistem = new MoveSistem(_navigator, transform, _attacDistance);
        _unit = new CombatUnit(100, 20, 30, 400);   
        _anima = new Anima(100, 1);
        IsLive = true;
        _speedCoefficient = 1;
        UpdateSpeed();
    }

    private void OnEnable()
    {        
        _unit.ImpactingEffect.AddListener(AddEffectImpact);
        _unit.HitPointsChanged.AddListener(UpdateHitPointsCoefficient);
        _unit.Died.AddListener(OnDeth);
        _staminaRegeneration = StartCoroutine(_unit.Resting());
        _updateMovePath = StartCoroutine(_moveSistem.UpdatePathToTarget());
    }

    private void OnDisable()
    {
        _unit.ImpactingEffect.RemoveListener(AddEffectImpact);
        _unit.HitPointsChanged.RemoveListener(UpdateHitPointsCoefficient);
        _unit.Died.RemoveListener(OnDeth);
        
        if (_staminaRegeneration != null)
            StopCoroutine(_staminaRegeneration);
        
        if (_updateMovePath != null)
            StopCoroutine(_updateMovePath);

        if (_attakEnemy != null)
            StopCoroutine(_attakEnemy);
    }

    private void UpdateHitPointsCoefficient(float newCoefficient)
    {
        _hitPointsCoeffecient = newCoefficient;
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
            if (_moveSistem.IsMove == false)
                _unit.Attack(enemy);

            yield return _attackDelay;
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
        _moveSistem.SetMoveSpeed(currentSpeed);
    } 
}
