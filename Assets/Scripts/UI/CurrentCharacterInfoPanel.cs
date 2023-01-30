using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CurrentCharacterInfoPanel : MonoBehaviour
{
    [SerializeField] private Image _portrait;
    [SerializeField] private Slider _hitPoints;
    [SerializeField] private Slider _manaPoints;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _profession;
    [SerializeField] private TMP_Text _damage;
    [SerializeField] private TMP_Text _armor;
    [SerializeField] private TMP_Text _attackSpeed;
    [SerializeField] private TMP_Text _moveSpeed;
    [SerializeField] private Image _flag;
    [SerializeField] private TMP_Text _teamName;

    private Character _character;

    public void SetNewCurrentCharacter(Character character)
    {
        if (_character != null)
        {
            _character.ChangedIndicators.RemoveListener(UpdateBarsInfo);
            _character.ChangedCharacteristics.RemoveListener(UpdateCharacteristicsInfo);
            _character.ChangedSpeed.RemoveListener(UpdateMoveSpeedInfo);
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
        UpdateBarsInfo(0, 0);
        _name.text = "";
        _profession.text = "";
        _damage.text = "";
        _armor.text = "";
        _attackSpeed.text = "";
        _moveSpeed.text = "";
        _teamName.text = "";
        _flag.color = Color.white;
    }

    private void DrowCharacter()
    {
        _character.ChangedIndicators.AddListener(UpdateBarsInfo);
        _character.ChangedCharacteristics.AddListener(UpdateCharacteristicsInfo);
        _character.ChangedSpeed.AddListener(UpdateMoveSpeedInfo);
        _portrait.sprite = _character.Portrait;
        _name.text = _character.Name;
        _profession.text = _character.Profission;
        _teamName.text = _character.TeamName;
        _flag.color = _character.TeamFlag;
    }

    private void UpdateMoveSpeedInfo(float speed)
    {
        _moveSpeed.text = speed.ToString();
    }

    private void UpdateCharacteristicsInfo(float damage, float armor, float attackspeed)
    {
        _damage.text = damage.ToString();
        _armor.text = armor.ToString();
        _attackSpeed.text = attackspeed.ToString();
    }

    private void UpdateBarsInfo(float hitPointsCoefficient, float manaPointsCoefficient)
    {
        _hitPoints.value = hitPointsCoefficient;
        _manaPoints.value = manaPointsCoefficient;
    }
}
