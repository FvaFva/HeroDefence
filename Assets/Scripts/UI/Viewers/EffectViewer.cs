using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EffectViewer : MonoBehaviour
{
    [SerializeField] private TMP_Text _duration;
    [SerializeField] private Image _icon;

    private Sprite _emptyIcon;
    private EffectLogic _effect;

    public void DrawEffect(EffectLogic effect)
    {
        _effect = effect;

        if (_effect == null)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        _icon.sprite = _effect.Icon;
        DrawDuration(_effect.Duration);
        _effect.ChangeDuration += DrawDuration;
    }

    private void Awake()
    {
        _emptyIcon = _icon.sprite;
    }

    private void OnDisable()
    {
        Clear();
    }

    private void Clear()
    {
        _icon.sprite = _emptyIcon;
        _duration.text = "0";

        if (_effect != null)
            _effect.ChangeDuration -= DrawDuration;
    }

    private void DrawDuration(int duration)
    {
        _duration.text = duration.ToString();
    }
}
