using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

[RequireComponent(typeof(ContentViewerSezer))]
[RequireComponent(typeof(ContentConcealer))]
public class SelectedCharactersContent : MonoBehaviour
{
    [SerializeField] private CharacterViewer _tempViewer;

    private List<CharacterViewer> _characterViewersPool = new List<CharacterViewer>();
    private ContentViewerSezer _sizer;
    private ContentConcealer _concealer;

    public event Action<Character> OnCharacterSelect;

    private void Awake()
    {
        TryGetComponent<ContentViewerSezer>(out _sizer);
        TryGetComponent<ContentConcealer>(out _concealer);
    }

    public void ClearSelectedChaViewers()
    {
        foreach (CharacterViewer viewer in _characterViewersPool)
        {
            viewer.SelectSharacter -= UpdateSelectedViewer;
            viewer.Render(null);
        }

        _concealer.StartMovePanel(false);
        _sizer.UpdateViewersSize(0);
    }

    public void RenderCharacter(Character character)
    {
        var unusedViewer = _characterViewersPool.Where(character => character.IsUsed == false).ToList();
        CharacterViewer newViewer = null;
        int countUsedViewers = _characterViewersPool.Count - unusedViewer.Count;
        
        if (unusedViewer.Count == 0)
        {
            newViewer = Instantiate(_tempViewer, transform);
            _characterViewersPool.Add(newViewer);
        }
        else
        {
            newViewer = unusedViewer.First();
        }

        newViewer.Render(character);
        newViewer.SelectSharacter += UpdateSelectedViewer;
        SetMainViewer(newViewer);
        _concealer.StartMovePanel(++countUsedViewers > 1);
        _sizer.UpdateViewersSize(++countUsedViewers);
    }

    private void UpdateSelectedViewer(Character character, CharacterViewer viewer)
    {
        SetMainViewer(viewer);
        OnCharacterSelect?.Invoke(character);
    }

    private void SetMainViewer(CharacterViewer mainViewer)
    {
        List<CharacterViewer> usedViewer = _characterViewersPool.Where(character => character.IsUsed == true).ToList();

        foreach (CharacterViewer viewer in usedViewer)
            viewer.SetMainTarget(viewer == mainViewer);
    }

}
