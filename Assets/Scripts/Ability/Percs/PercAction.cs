using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PercAction : ScriptableObject
{
    [SerializeField] private string _description;

    public string Description => _description;

    public abstract void DoAction(Character root, Character target, float damage);
}
