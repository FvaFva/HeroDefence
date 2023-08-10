using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(Image))]
public class CharacterViewer : MonoBehaviour
{
    [SerializeField] private Slider _hitPointsBar;
    [SerializeField] private Slider _manaPointsBar;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _flagText;
    [SerializeField] private Image _portrait;
    [SerializeField] private Image _flagImage;
    [SerializeField] private Image _currentMask;
    [SerializeField] private Color _targetableColor;
    [SerializeField] private Button _selectCharacter;
    [SerializeField] private ScrollRect _effectPlace;

    private Character _character;
    private Color _baseColor;

    public event Action<Character, CharacterViewer> SelectCharacter;

    public bool IsUsed { get; private set; }

    public void SetMainTarget(bool isItMain)
    {
        _currentMask.color = isItMain ? _targetableColor : _baseColor;
    }

    public void Render(Character character)
    {
        if (character == null)
        {
            Clear();
        }
        else
        {
            IsUsed = true;
            _character = character;
            _character.ChangedIndicators += SetCurrentIndicators;
            _name.text = _character.Name;
            _portrait.sprite = _character.Portrait;
            _flagImage.color = _character.Team.Flag;
            _flagText.text = _character.Team.Name;
            gameObject.SetActive(true);
        }
    }

    public void Selected()
    {
        SelectCharacter?.Invoke(_character, this);
    }

    public void Clear()
    {
        RemoveListenersFromCharacter();
        _character = null;
        _name.text = "|||||";
        _portrait.sprite = null;
        SetCurrentIndicators(0, 0);
        IsUsed = false;
        gameObject.SetActive(false);
    }

    private void Awake()
    {
        _baseColor = _currentMask.color;
    }

    private void OnEnable()
    {
        if (_character != null)
            _character.ChangedIndicators += SetCurrentIndicators;

        _selectCharacter.onClick.AddListener(Selected);
    }

    private void OnDisable()
    {
        RemoveListenersFromCharacter();
        _selectCharacter.onClick.RemoveListener(Selected);
    }

    private void RemoveListenersFromCharacter()
    {
        if (_character != null)
            _character.ChangedIndicators -= SetCurrentIndicators;
    }

    private void SetCurrentIndicators(float hitPointsCoefficient, float manaPointsCoefficient)
    {
        _hitPointsBar.value = hitPointsCoefficient;
        _manaPointsBar.value = manaPointsCoefficient;
    }
}
