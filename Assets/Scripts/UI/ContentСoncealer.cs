using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public abstract class Content—oncealer : MonoBehaviour
{
    
    [SerializeField] protected ScrollRect _panel;
    [SerializeField] protected float _showingPanelSpeed = 1000;
    [SerializeField] protected Vector2 _UnshowDirection;

    
    protected Vector3 _unshowingPosition;
    protected Vector3 _showingPosition;
    protected Coroutine _panelMoving;

    protected void InitContentAdaptor()
    {        
        _unshowingPosition = _panel.transform.position;
        _unshowingPosition.x += _UnshowDirection.x;
        _unshowingPosition.y += _UnshowDirection.y;
        _showingPosition = _panel.transform.position;
        StartMovePanel(false);
    }

    protected void StartMovePanel(bool show)
    {
        if (_panelMoving != null)
            StopCoroutine(_panelMoving);

        Vector3 targetPosition = show ? _showingPosition : _unshowingPosition;
        StartCoroutine(MovePanel(targetPosition));
    }

    protected IEnumerator MovePanel(Vector3 position)
    {
        while (_panel.transform.position != position)
        {
            _panel.transform.position = Vector3.MoveTowards(_panel.transform.position, position, Time.deltaTime * _showingPanelSpeed);
            yield return null;
        }
    }
}
