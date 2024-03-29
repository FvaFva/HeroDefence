using UnityEngine;
using UnityEngine.UI;

public class AbilityViewer : MonoBehaviour
{
    [SerializeReference] private Image _icon;
    private string _description;

    public Ability Ability { get; private set; }

    public void ShowAbility(Ability ability)
    {
        if (ability == null)
        {
            Clear();
            return;
        }

        gameObject.SetActive(true);
        _icon.sprite = ability.Icon;
        _description = ability.GetDescription();
        Ability = ability;
    }

    private void Clear()
    {
        _icon.sprite = null;
        _description = null;
        Ability = null;
        gameObject.SetActive(false);
    }
}
