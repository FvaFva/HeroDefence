using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class TempShop : MonoBehaviour
{
    [SerializeField] private List<Perc> _percs;

    private Button _sellPerc;

    public event Action<Perc> SelledPerc;

    private void Awake()
    {
        TryGetComponent<Button>(out _sellPerc);        
    }

    private void OnEnable()
    {
        _sellPerc.onClick.AddListener(SellPerc);
    }

    private void OnDisable()
    {
        _sellPerc.onClick.RemoveListener(SellPerc);
    }

    private void SellPerc()
    {
        if(_percs.Count > 0)
            SelledPerc?.Invoke(_percs[UnityEngine.Random.Range(0,_percs.Count)]);
    }
}
