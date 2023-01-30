using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(GridLayoutGroup))]
public class SelectedCharactersContent : MonoBehaviour
{
    [SerializeField] private float _plaseWidth; 
    [SerializeField] private float _plaseHeight;
    [SerializeField] private CharacterViewer _tempViewer;
    [SerializeField] private ScrollRect _panel;
    [SerializeField] private float _showingPanelSpeed = 4;

    private float _maxViewerSize;
    private float _maxSideLeght;
    private Vector3 _unshowingPosition;
    private Vector3 _showingPosition;
    private GridLayoutGroup _contentGroup;
    private Coroutine _panelMoving;
    private List<CharacterViewer> _characterViewersPool = new List<CharacterViewer>();

    public UnityEvent<Character> OnCharacterSelect = new UnityEvent<Character>();

    private void Awake()
    {
        TryGetComponent<GridLayoutGroup>(out _contentGroup);
        _maxViewerSize = Mathf.Min(_plaseWidth, _plaseHeight);
        _maxSideLeght = Mathf.Max(_plaseWidth, _plaseHeight);
        int unshowSpace = 500;
        _unshowingPosition = _panel.transform.position;
        _unshowingPosition.x -= unshowSpace;
        _showingPosition = _panel.transform.position;
        UpdateViewersSize(0);
    }

    public void ClearSelectedChaViewers()
    {
        foreach (CharacterViewer viewer in _characterViewersPool)
        {
            viewer.SelectSharacter.RemoveListener(UpdateSelectedViewer);
            viewer.Render(null);
        }

        UpdateViewersSize(0);
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
        newViewer.SelectSharacter.AddListener(UpdateSelectedViewer);
        SetMainViewer(newViewer);
        UpdateViewersSize(++countUsedViewers);
    }

    private void UpdateSelectedViewer(Character character, CharacterViewer viewer)
    {
        SetMainViewer(viewer);
        OnCharacterSelect.Invoke(character);
    }

    private void SetMainViewer(CharacterViewer mainViewer)
    {
        List<CharacterViewer> usedViewer = _characterViewersPool.Where(character => character.IsUsed == true).ToList();

        foreach (CharacterViewer viewer in usedViewer)
            viewer.SetMainTarget(viewer == mainViewer);
    }

    private void UpdateViewersSize(int countsUsedViewers)
    {
        if(countsUsedViewers <= 1)
        {
            StartMovePanel(false);
        }
        else
        {
            StartMovePanel(true);
            int countRow = Mathf.CeilToInt(_maxViewerSize * countsUsedViewers / _maxSideLeght);
            float newSize = _maxViewerSize / countRow;
            int countColum = Mathf.CeilToInt(countsUsedViewers / countRow);
            float freeSpaseInColum = _maxSideLeght - countColum * newSize;
            int countFilleredSpace = countColum + 1;
            float Spacing = freeSpaseInColum / countFilleredSpace;

            _contentGroup.padding.top = (int)Spacing;
            _contentGroup.padding.bottom = (int)Spacing;
            _contentGroup.spacing = new Vector2(_contentGroup.spacing.x, Spacing);
            _contentGroup.cellSize = new Vector2(newSize, newSize);
        }
    }

    private void StartMovePanel(bool show)
    {
        if(_panelMoving != null)
            StopCoroutine(_panelMoving);

        Vector3 targetPosition = show ? _showingPosition : _unshowingPosition;
        StartCoroutine(MovePanel(targetPosition));
    }

    private IEnumerator MovePanel(Vector3 position)
    {
        while(_panel.transform.position != position)
        {
            _panel.transform.position = Vector3.MoveTowards(_panel.transform.position, position, Time.deltaTime * _showingPanelSpeed);
            yield return null;
        }
    }
}
