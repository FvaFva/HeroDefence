using TMPro;
using UnityEngine;

public class CharacteristicViewer : MonoBehaviour
{
    [SerializeField] private TMP_Text _info;

    public void ShowCharacteristic(int characteristic)
    {
        _info.text = characteristic.ToString();
    }
}
