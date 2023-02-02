using UnityEngine;
using UnityEngine.UI;

public class AbilityViewer : MonoBehaviour
{
    [SerializeReference] private Image _icon;
    
    private string _description;

    public bool IsUsed;

    public void Render(Ability ability)
    {
        gameObject.SetActive(true);
        IsUsed = true;
        _icon.sprite = ability.Icon;
        _description = ability.GetDescription();
    }

    public void Clear()
    {
        _icon = null;
        _description = null;
        IsUsed = false;
        gameObject.SetActive(false);
    }
}
