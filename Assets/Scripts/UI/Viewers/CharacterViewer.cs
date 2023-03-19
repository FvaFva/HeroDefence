using UnityEngine;
using UnityEngine.UI;
using TMPro;
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
    [SerializeField] private Color _targetebleColor;
    [SerializeField] private Button _selectSharacter;
    [SerializeField] private ScrollRect _effectPlace;

    private Character _character;
    private Color _baseColor;

    public event Action<Character, CharacterViewer> SelectSharacter;
    public bool IsUsed;

    private void Awake()
    {
        _baseColor = _currentMask.color;
    }

    private void OnEnable()
    {
        if(_character != null)
            _character.ChangedIndicators += SetCurrentIndicators;

        _selectSharacter.onClick.AddListener(Selected);
    }

    private void OnDisable()
    {
        RemoveListenersFromCharacter();
        _selectSharacter.onClick.RemoveListener(Selected);
    }

    public void SetMainTarget(bool isItMain)
    {
        _currentMask.color = isItMain? _targetebleColor : _baseColor;
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
        SelectSharacter?.Invoke(_character, this);
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

    private void RemoveListenersFromCharacter()
    {
        if (_character != null)
            _character.ChangedIndicators -= SetCurrentIndicators;
    }

    private void SetCurrentIndicators(float hitPointsCoeffecient, float manaPointsCoeffecient)
    {
        _hitPointsBar.value = hitPointsCoeffecient;
        _manaPointsBar.value = manaPointsCoeffecient;
    }
}
