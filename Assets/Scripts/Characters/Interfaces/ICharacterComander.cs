using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterComander
{
    public event Action<Target> ChoosedTarget;
}
