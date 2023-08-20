using UnityEngine;

public abstract class PercAction : ScriptableObject
{
    [SerializeField] private string _description;
    [SerializeField] private Color _effectColor;

    public string Description => _description;

    public void DoAction(IFightable root, IFightable target, float damage)
    {
        MainAction(root, target, damage);
        root.ShowColoredEffectImpact(_effectColor);
    }

    protected abstract void MainAction(IFightable root, IFightable target, float damage);
}
