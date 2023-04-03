using System;
using System.Collections.Generic;

public interface ISpellTargetLogic 
{
    public event Action<List<IFightable>> TargetsSelected;
    public void SelectTargets();
}
