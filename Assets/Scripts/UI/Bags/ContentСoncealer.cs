using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class Content—oncealer : MonoBehaviour
{
    [SerializeField] private ScrollRect _panel;
    [SerializeField] private Vector2 _UnshowDirection;

    private float _showingPanelSpeed = 1000;
    private Vector3 _unshowingPosition;
    private Vector3 _showingPosition;
    private Coroutine _panelMoving;

    public void StartMovePanel(bool show)
    {
        if (_panelMoving != null)
            StopCoroutine(_panelMoving);

        Vector3 targetPosition = show ? _showingPosition : _unshowingPosition;
        StartCoroutine(MovePanel(targetPosition));
    }

    private IEnumerator MovePanel(Vector3 position)
    {
        while (_panel.transform.position != position)
        {
            _panel.transform.position = Vector3.MoveTowards(_panel.transform.position, position, Time.deltaTime * _showingPanelSpeed);
            yield return null;
        }
    }

    private void Awake()
    {
        _unshowingPosition = _panel.transform.position;
        _unshowingPosition.x += _UnshowDirection.x;
        _unshowingPosition.y += _UnshowDirection.y;
        _showingPosition = _panel.transform.position;
        StartMovePanel(false);
    }
}
