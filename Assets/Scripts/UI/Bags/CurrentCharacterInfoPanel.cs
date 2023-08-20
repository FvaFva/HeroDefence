using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class CurrentCharacterInfoPanel : MonoBehaviour
{
    [SerializeField] private Image _portrait;
    [SerializeField] private Image _flag;
    [SerializeField] private IndicatorSlider _hitPoints;
    [SerializeField] private IndicatorSlider _manaPoints;
    [SerializeField] private IndicatorSlider _stamina;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _profession;
    [SerializeField] private TMP_Text _teamName;
    [SerializeField] private CharacteristicViewer _damage;
    [SerializeField] private CharacteristicViewer _attackSpeed;
    [SerializeField] private CharacteristicViewer _armor;
    [SerializeField] private CharacteristicViewer _moveSpeed;
    [SerializeField] private AmmunitionViewer _ammunition;
    [SerializeField] private CurrentCharacterAbilityContent _currentCharacterAbility;
    [SerializeField] private EffectsBug _effectsBug;

    private Character _character;

    public event Action<Item> ChoseAmmunitionsItem;

    public void SetNewCurrentCharacter(Character character)
    {
        if (_character != null)
        {
            _character.ChangedIndicators -= UpdateIndicators;
            _character.ChangedCharacteristics -= UpdateCharacteristicsInfo;
            _character.ChangedAbilitiesKit -= DrawAbilities;
            _character.ChangedAmmunition -= _ammunition.DrawThingsWorn;
            _character.ChangedEffectsKit -= _effectsBug.DrawEffects;
            _character.StaminaChanged -= OnStaminaChanged;
        }

        _character = character;

        if (_character == null)
            Clear();
        else
            DrawCharacter();
    }

    public void Clear()
    {
        _portrait.sprite = null;
        UpdateIndicators(0, 0);
        _currentCharacterAbility.ClearAllRenderedViewers();
        _name.text = string.Empty;
        _profession.text = string.Empty;
        _damage.ShowCharacteristic(0);
        _armor.ShowCharacteristic(0);
        _attackSpeed.ShowCharacteristic(0);
        _moveSpeed.ShowCharacteristic(0);
        _hitPoints.SetMaxValue(1);
        _manaPoints.SetMaxValue(1);
        _stamina.SetMaxValue(GameSettings.Character.StaminaPointsToAttack);
        _effectsBug.DrawEffects();
        _teamName.text = string.Empty;
        _ammunition.Clear();
        _flag.color = Color.white;
    }

    private void OnEnable()
    {
        _ammunition.ChoseItem += OnItemChoose;
    }

    private void OnDisable()
    {
        _ammunition.ChoseItem -= OnItemChoose;
    }

    private void DrawCharacter()
    {
        _character.ChangedIndicators += UpdateIndicators;
        _character.StaminaChanged += OnStaminaChanged;
        _character.ChangedCharacteristics += UpdateCharacteristicsInfo;
        _character.ChangedAbilitiesKit += DrawAbilities;
        _character.ChangedAmmunition += _ammunition.DrawThingsWorn;
        _character.ChangedEffectsKit += _effectsBug.DrawEffects;
        _portrait.sprite = _character.Portrait;
        _name.text = _character.Name;
        _profession.text = _character.Profession;
        _teamName.text = _character.Team.Name;
        _flag.color = _character.Team.Flag;
        _character.ShowAllInformation();
    }

    private void UpdateCharacteristicsInfo(FighterCharacteristics characteristics)
    {
        _damage.ShowCharacteristic((int)characteristics.Damage);
        _armor.ShowCharacteristic((int)characteristics.Armor);
        _attackSpeed.ShowCharacteristic((int)characteristics.AttackSpeed);
        _moveSpeed.ShowCharacteristic((int)characteristics.Speed);
        _hitPoints.SetMaxValue(characteristics.HitPoints);
        _manaPoints.SetMaxValue(characteristics.ManaPoints);
        _stamina.SetMaxValue(GameSettings.Character.StaminaPointsToAttack);
    }

    private void UpdateIndicators(float hitPointsCoefficient, float manaPointsCoefficient)
    {
        _hitPoints.SetCurrentCoefficient(hitPointsCoefficient);
        _manaPoints.SetCurrentCoefficient(manaPointsCoefficient);
    }

    private void DrawAbilities(IReadOnlyList<Ability> abilities)
    {
        _currentCharacterAbility.ClearAllRenderedViewers();

        foreach (Ability ability in abilities)
            _currentCharacterAbility.Render(ability);
    }

    private void OnItemChoose(Item item)
    {
        ChoseAmmunitionsItem?.Invoke(item);
    }

    private void OnStaminaChanged(float currentStamina)
    {
        _stamina.SetCurrentCoefficient(currentStamina / GameSettings.Character.StaminaPointsToAttack);
    }
}
