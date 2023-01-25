using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour
{
    private MoveSistem _moveSistem;
    private CombatUnit _unit;
    private Anima _anima;
    private List<EffectImpact> _effects = new List<EffectImpact>();
    private Person _target;

    private Coroutine _combatResting;
    private Coroutine _movieng;

    [SerializeField] private float _speed;
    [SerializeField] private float _speedCoefficient;

    public void Awake()
    {
        _moveSistem = new MoveSistem(GetComponent<Transform>(), _speed);
        _unit = new CombatUnit(100, 20, 30, 100);
        _unit.Died.AddListener(OnDeth);
        _anima = new Anima(100, 1);
    }

    public void OnEnable()
    {
        _unit.ImpactingEffect.AddListener(AddEffectImpact);
        _combatResting = StartCoroutine(_unit.Resting());
    }

    public void OnDisable()
    {
        _unit.ImpactingEffect.AddListener(AddEffectImpact);
        StopCoroutine(_combatResting);
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            Vector3 coursorPosition = Input.mousePosition;
            coursorPosition.y = 0;
            SetNewTargetToMove(coursorPosition);
        }
    }

    private void SetNewTargetToMove(Vector3 targtPoint)
    {
        if(_movieng!=null)
            StopCoroutine(_movieng);

        _moveSistem.SetNewTarget(targtPoint);
        _movieng = StartCoroutine(_moveSistem.Move());
    }

    private void OnDeth()
    {
        _unit.Died.RemoveListener(OnDeth);
        Destroy(gameObject);
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

    private void UpdateEffectsImpact()
    {

    }
}
