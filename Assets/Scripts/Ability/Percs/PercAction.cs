using UnityEngine;

public abstract class PercAction : ScriptableObject
{
    [SerializeField] private string _description;

    public string Description => _description;

    public abstract void DoAction(IFightable root, IFightable target, float damage);
}
