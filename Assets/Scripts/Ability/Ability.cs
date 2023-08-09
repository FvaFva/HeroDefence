using UnityEngine;

public abstract class Ability : ScriptableObject
{
    [SerializeField] private Sprite _icon;
    [SerializeField] private string _label;

    public Sprite Icon => _icon;

    public string Label => _label;

    public abstract string GetDescription();
}
