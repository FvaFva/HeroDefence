using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New character", menuName = "Characters/NewCharacter", order = 51)]
public class CharacterPreset : ScriptableObject
{
    [SerializeField] private float _hitPoints;
    [SerializeField] private float _attacSpeed;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private string _class;
    [SerializeField] private float _armor;
    [SerializeField] private float _damage;
    [SerializeField] private Sprite _portrait;
    [SerializeField] private AttackLogic _attackLogic;

    public float HitPoints => _hitPoints;
    public float AttacSpeed => _attacSpeed;
    public string Class => _class;
    public float MoveSpeed => _moveSpeed;
    public float Damage => _damage;
    public float Armor => _armor;
    public Sprite Portrait => _portrait;
    public string Name => GameSettings.Character.GetRandomName();
    public AttackLogic AttackLogic => _attackLogic;
}
