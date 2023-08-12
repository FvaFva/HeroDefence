using System.Collections;
using UnityEngine;

public class ContentConcealer : MonoBehaviour
{
    [SerializeField] private UIPanel _panel;
    [SerializeField] private Vector2 _hideDirection;

    private float _showingPanelSpeed = 1000;
    private Vector3 _hidePosition;
    private Vector3 _showPosition;
    private Coroutine _panelMoving;

    public void StartMovePanel(bool show)
    {
        if (_panelMoving != null)
            StopCoroutine(_panelMoving);

        Vector3 targetPosition = show ? _showPosition : _hidePosition;
        _panelMoving = StartCoroutine(MovePanel(targetPosition));
    }

    private void Awake()
    {
        _hidePosition = _panel.transform.position;
        _hidePosition.x += _hideDirection.x;
        _hidePosition.y += _hideDirection.y;
        _showPosition = _panel.transform.position;
        StartMovePanel(false);
    }

    private IEnumerator MovePanel(Vector3 position)
    {
        while (_panel.transform.position != position)
        {
            _panel.transform.position = Vector3.MoveTowards(_panel.transform.position, position, Time.deltaTime * _showingPanelSpeed);
            yield return null;
        }
    }
}
