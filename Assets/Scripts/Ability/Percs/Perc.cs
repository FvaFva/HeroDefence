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

    public void ExecuteDependentAction(IFightable root, IFightable target, float damage, PercActionType type)
    {
        foreach (ActionTrigger action in _triggers.Where(act => act.PercActionType == type))
            action.ExecuteActionIfRandomize(root, target, damage);
    }

    public override string GetDescription()
    {
        string description = string.Empty;

        AddNewBlockToDescription("On attack: ", PercActionType.OnAttack, ref description);
        AddNewBlockToDescription("On defense: ", PercActionType.OnDefense, ref description);
        AddNewBlockToDescription("On deal damage: ", PercActionType.OnDamageDelay, ref description);

        return description;
    }

    private void OnValidate()
    {
        int numberOfLastRecord = Mathf.Min(_actions.Count, _chances.Count, _percActionTypes.Count);

        _triggers.Clear();

        for (int i = 0; i < numberOfLastRecord; i++)
            _triggers.Add(new ActionTrigger(_actions[i], _chances[i], _percActionTypes[i]));
    }

    private void AddNewBlockToDescription(string blocName, PercActionType blocType, ref string description)
    {
        string actionDescriptions = string.Empty;

        foreach (ActionTrigger action in _triggers.Where(act => act.PercActionType == blocType))
            actionDescriptions = $"{actionDescriptions} {action.Description}, ";

        if (actionDescriptions != string.Empty)
            {
                description += description == string.Empty ? "\n" : "";
                description = $"{description} {blocName} {actionDescriptions}";
            }
    }
}
