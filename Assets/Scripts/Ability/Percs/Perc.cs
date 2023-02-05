using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "New perc logic", menuName = "Ability/Percs/Logic", order = 51)]
public class Perc : Ability
{
    [SerializeField] private List<PercAction> _actions = new List<PercAction>();
    [SerializeField] private List<float> _chances = new List<float>();
    [SerializeField] private List<PercActionType> _percActionTypes = new List<PercActionType>();

    private List<ActionTrigger> _triggers = new List<ActionTrigger>();
    
    private void OnValidate()
    {
        int numberOfLastRecord = Mathf.Min(_actions.Count, _chances.Count, _percActionTypes.Count);

        _triggers.Clear();

        for(int i = 0; i < numberOfLastRecord; i++)
            _triggers.Add(new ActionTrigger(_actions[i], _chances[i], _percActionTypes[i]));
    }

    public void ExecuteDepenceAction(IFightebel root, IFightebel target, float damage, PercActionType type)
    {
        foreach (ActionTrigger action in _triggers.Where(act => act.PercActionType == type))
            action.ExecuteActionIfRandomize(root, target, damage);
    }

    public override string GetDescription()
    {
        string description = "";

        AddNewBlockToDescription("On attack: ", PercActionType.OnAttack, ref description);
        AddNewBlockToDescription("On defence: ", PercActionType.OnDefence, ref description);
        AddNewBlockToDescription("On deal damage: ", PercActionType.OnDamageDelay, ref description);

        return description;
    }

    private void AddNewBlockToDescription(string blocName, PercActionType blocType, ref string description)
    {
        if (description != "")
            description = description + "\n";

        description = description + blocName;

        foreach (ActionTrigger action in _triggers.Where(act => act.PercActionType == blocType))
            description = $"{description} {action.Description}, ";
    }    
}

public struct ActionTrigger
{
    [SerializeField] private PercAction _action;
    [SerializeField] private float _chance;
    [SerializeField] private PercActionType _percActionType;

    public PercActionType PercActionType => _percActionType;
    public string Description => $"{_chance} to {_action.Description}";

    public ActionTrigger(PercAction action, float chance, PercActionType percActionType)
    {
        _action = action;
        _chance = chance;
        _percActionType = percActionType;
    }

    public void ExecuteActionIfRandomize(IFightebel root, IFightebel target, float damage)
    {
        if (_action != null && _chance > Random.Range(GameSettings.Zero, GameSettings.Hundred))
        {
            _action.DoAction(root, target, damage);
        }
    }
}
