using UnityEngine;

public abstract class PercAction : ScriptableObject
{
    [SerializeField] private string _description;

    public string Description => _description;

    public abstract void DoAction(IFightebel root, IFightebel target, float damage);
}
