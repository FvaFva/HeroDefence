using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    [SerializeField] protected Sprite _icon;
    [SerializeField] protected string _label;

    public Sprite Icon => _icon;
    public string Label => _label;

    public abstract string GetDescription();
}
