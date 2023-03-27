using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System;

public class CurrentCharacterInfoPanel : MonoBehaviour
{
    [SerializeField] private Image _portrait;
    [SerializeField] private Image _flag;

    [SerializeField] private Slider _hitPoints;
    [SerializeField] private Slider _manaPoints;

    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _profession;
    [SerializeField] private TMP_Text _teamName;

    [SerializeField] private CharacteristicViewer _damage;
    [SerializeField] private CharacteristicViewer _attackSpeed;
    [SerializeField] private CharacteristicViewer _armor;
    [SerializeField] private CharacteristicViewer _moveSpeed;

    [SerializeField] private AmmunitionViewer _ammunition;
    [SerializeField] private CurrentCharacterAbilityContent _currentCharacterAbility;

    private Character _character;

    public event Action<Item> ShoseAmmunitionsItem;

    public void SetNewCurrentCharacter(Character character)
    {
        if (_character != null)
        {
            _character.ChangedIndicators -= UpdateIndicators;
            _character.ChangedCharacteristics -= UpdateCharacteristicsInfo;
            _character.ChangedAbilitiesKit -= DrowAbilities;
            _character.ChangedAmmunition -= _ammunition.DrowThingsWorn;
        }

        _character = character;

        if (_character == null)
            Clear();
        else
            DrowCharacter();
    }

    public void Clear()
    {        
        _portrait.sprite = null;
        UpdateIndicators(0, 0);
        _currentCharacterAbility.ClearAllRenderedViewers();
        _name.text = "";
        _profession.text = "";
        _damage.ShowCharacteristic(0);
        _armor.ShowCharacteristic(0);
        _attackSpeed.ShowCharacteristic(0);
        _moveSpeed.ShowCharacteristic(0);
        _teamName.text = "";
        _ammunition.Clear();
        _flag.color = Color.white;
    }

    private void DrowCharacter()
    {
        _character.ChangedIndicators += UpdateIndicators;
        _character.ChangedCharacteristics += UpdateCharacteristicsInfo;
        _character.ChangedAbilitiesKit += DrowAbilities;
        _character.ChangedAmmunition += _ammunition.DrowThingsWorn;
        _portrait.sprite = _character.Portrait;
        _name.text = _character.Name;
        _profession.text = _character.Profession;
        _teamName.text = _character.Team.Name;
        _flag.color = _character.Team.Flag;
        _character.ShowAllInformations();
    }

    private void UpdateCharacteristicsInfo(Fighter—haracteristics Òharacteristics)
    {
        _damage.ShowCharacteristic((int)Òharacteristics.Damage);
        _armor.ShowCharacteristic((int)Òharacteristics.Armor);
        _attackSpeed.ShowCharacteristic((int)Òharacteristics.AttackSpeed);
        _moveSpeed.ShowCharacteristic((int)Òharacteristics.Speed);
    }

    private void UpdateIndicators(float hitPointsCoefficient, float manaPointsCoefficient)
    {
        _hitPoints.value = hitPointsCoefficient;
        _manaPoints.value = manaPointsCoefficient;
    }

    private void DrowAbilities(IReadOnlyList<Ability> abilities)
    {
        foreach (Ability ability in abilities)
            _currentCharacterAbility.Render(ability);
    }

    private void OnItemChoose(Item item)
    {
        ShoseAmmunitionsItem?.Invoke(item);
    }

    private void OnEnable()
    {
        _ammunition.ChoseItem += OnItemChoose;
    }

    private void OnDisable()
    {
        _ammunition.ChoseItem -= OnItemChoose;
    }
}
