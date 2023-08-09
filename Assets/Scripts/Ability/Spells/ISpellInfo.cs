using System;
using UnityEngine;

public interface ISpellInfo
{
    public Sprite Icon { get; }

    public int ManaCost { get; }

    public event Action<float> CoolDownChanged;
}