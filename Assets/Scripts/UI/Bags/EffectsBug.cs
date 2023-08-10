using System.Collections.Generic;
using UnityEngine;

public class EffectsBug : MonoBehaviour
{
    [SerializeField] private EffectViewer _tempViewer;
    [SerializeField] private int _countMaxEffects;

    private List<EffectViewer> _effectViewers = new List<EffectViewer>();

    public void DrawEffects(IReadOnlyList<EffectLogic> effects = null)
    {
        if (effects == null)
        {
            for (int i = 0; i < _countMaxEffects; i++)
                _effectViewers[i].DrawEffect(null);
        }
        else if (effects.Count >= _countMaxEffects)
        {
            for (int i = 0; i < _countMaxEffects; i++)
                _effectViewers[i].DrawEffect(effects[i]);
        }
        else
        {
            for (int i = 0; i < effects.Count; i++)
                _effectViewers[i].DrawEffect(effects[i]);
            for (int i = effects.Count; i < _countMaxEffects; i++)
                _effectViewers[i].DrawEffect(null);
        }
    }

    private void Awake()
    {
        for (int i = 0; i < _countMaxEffects; i++)
        {
            EffectViewer effectViewer = Instantiate(_tempViewer, transform);
            effectViewer.DrawEffect(null);
            _effectViewers.Add(effectViewer);
        }
    }
}
