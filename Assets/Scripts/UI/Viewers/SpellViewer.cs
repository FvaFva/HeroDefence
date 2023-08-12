using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellViewer : MonoBehaviour
{
    [SerializeField] private TMP_Text _manaCost;
    [SerializeField] private Image _icon;
    [SerializeField] private Image _cooldown;
    [SerializeField] private Button _button;

    private ISpellInfo _spell;

    public void DrawSpell(Spell spell)
    {
        if (spell == null)
            Clear();
        else
            Render(spell);
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(ActivatedSpell);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(ActivatedSpell);
        Clear();
    }

    private void Render(Spell spell)
    {
        gameObject.SetActive(true);
        _spell = spell;
        _icon.sprite = _spell.Icon;
        _cooldown.fillAmount = 0;
        _spell.CoolDownChanged += UpdateCooldown;
        _manaCost.text = _spell.ManaCost.ToString();
    }

    private void Clear()
    {
        _cooldown.fillAmount = 0;
        _icon.sprite = null;
        _manaCost.text = "0";

        if (_spell != null)
            _spell.CoolDownChanged -= UpdateCooldown;

        _cooldown = null;
        gameObject.SetActive(false);
    }

    private void UpdateCooldown(float cooldownCoefficient)
    {
        _cooldown.fillAmount = 0;
    }

    private void ActivatedSpell()
    {
    }
}
