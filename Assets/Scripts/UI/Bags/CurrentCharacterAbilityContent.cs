using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(ContentViewerSezer))]
public class CurrentCharacterAbilityContent : MonoBehaviour
{
    [SerializeField] private AbilityViewer _tempViewer;

    private ContentViewerSezer _contentSizer;
    private List<AbilityViewer> _viewerPool = new List<AbilityViewer>();

    private void Awake()
    {
        TryGetComponent<ContentViewerSezer>(out _contentSizer);   
    }

    public void Render(Ability ability)
    {
        if (_viewerPool.Where(abi => abi.Ability == ability).Count() > 0)
            return;

        var unusedViewer = _viewerPool.Where(abi => abi.Ability == null).ToList();
        AbilityViewer newViewer = null;

        if (unusedViewer.Count == 0)
        {
            newViewer = Instantiate(_tempViewer, transform);
            _viewerPool.Add(newViewer);
        }
        else
        {
            newViewer = unusedViewer.First();
        }

        newViewer.ShowAbility(ability);
        int countUsedViewers = _viewerPool.Count - unusedViewer.Count;
        _contentSizer.UpdateViewersSize(countUsedViewers);
    }

    public void ClearAllRenderedViewers()
    {
        foreach (AbilityViewer viewer in _viewerPool.Where(abi => abi.Ability != null))
            viewer.ShowAbility(null);

        _contentSizer.UpdateViewersSize(0);
    }
}
