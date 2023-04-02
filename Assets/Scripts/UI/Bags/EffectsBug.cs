using System.Collections.Generic;
using UnityEngine;

public class EffectsBug : MonoBehaviour
{
    [SerializeField] private EffectViewer _tempViewer;
    [SerializeField] int _countMaxEffects;

    private List<EffectViewer> _effectViewers = new List<EffectViewer>();

    private void Awake()
    {
        for(int i = 0; i < _countMaxEffects; i++)
        {
            EffectViewer effectViewer = Instantiate(_tempViewer, transform);
            effectViewer.DrowEffect(null);
            _effectViewers.Add(effectViewer);
        }
    }

    public void DrowEffects(IReadOnlyList<EffectLogic> effects = null)
    {
        if (effects == null)
        {
            for (int i = 0; i < _countMaxEffects; i++)
                _effectViewers[i].DrowEffect(null);
        }
        else if(effects.Count >= _countMaxEffects)
        { 
            for(int i =0; i < _countMaxEffects; i++)
                _effectViewers[i].DrowEffect(effects[i]);
        }
        else
        {
            for (int i = 0; i < effects.Count; i++)
                _effectViewers[i].DrowEffect(effects[i]);
            for (int i = effects.Count; i < _countMaxEffects; i++)
                _effectViewers[i].DrowEffect(null);
        }
    }
}
