using UnityEngine;

public struct ActionTrigger
{
    private PercAction _action;
    private float _chance;
    private PercActionType _percActionType;

    public PercActionType PercActionType => _percActionType;
    public string Description => $"{_chance} to {_action.Description}";

    public ActionTrigger(PercAction action, float chance, PercActionType percActionType)
    {
        _action = action;
        _chance = chance;
        _percActionType = percActionType;
    }

    public void ExecuteActionIfRandomize(IFightable root, IFightable target, float damage)
    {
        if (_action != null && _chance > Random.Range(GameSettings.Zero, GameSettings.Hundred))
        {
            _action.DoAction(root, target, damage);
        }
    }
}