using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using System;

[RequireComponent(typeof(Image))]
public class CharacterViewer : MonoBehaviour
{
    [SerializeField] private Slider _hitPointsBar;
    [SerializeField] private Slider _manaPointsBar;
    [SerializeField] private Image _portrait;
    [SerializeField] private ScrollRect _effectPlace;
    [SerializeField] private Button _selectSharacter;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private Color _targetebleColor;
    [SerializeField] private Image _flagImage;
    [SerializeField] private TMP_Text _flagText;
    [SerializeField] private Image _currentMask;

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
            _character.ChangedIndicators += SetCurrentCharacteristics;

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
            _name.text = character.Name;
            _portrait.sprite = _character.Portrait;
            _character.ChangedIndicators += SetCurrentCharacteristics;
            character.SetThisCurrentCharacter();
            gameObject.SetActive(true);
            _flagImage.color = character.TeamFlag;
            _flagText.text = character.TeamName;
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
        SetCurrentCharacteristics(0, 0);
        IsUsed = false;
        gameObject.SetActive(false);
    }

    private void RemoveListenersFromCharacter()
    {
        if (_character != null)
            _character.ChangedIndicators -= SetCurrentCharacteristics;
    }

    private void SetCurrentCharacteristics(float hitPointsCoeffecient, float manaPointsCoeffecient)
    {
        _hitPointsBar.value = hitPointsCoeffecient;
        _manaPointsBar.value = manaPointsCoeffecient;
    }
}
