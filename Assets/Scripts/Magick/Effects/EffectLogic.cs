using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSpellEffect", menuName ="Spells/NewSpellEffect", order = 51)]
public class EffectLogic : ScriptableObject
{
    [SerializeField] public bool IsBlockMove { get; private set; }
    [SerializeField] public bool IsBlockFight { get; private set; }
    [SerializeField] public bool IsBlockSpellCast { get; private set; }
    [SerializeField] public float ProcentMoveSlow { get; private set; }
    [SerializeField] public float ProcentAttackSlow { get; private set; }
    [SerializeField] public float DamagePersSecond { get; private set; }
    [SerializeField] public float HealPersSecond { get; private set; }
}
