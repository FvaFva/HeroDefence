using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSistem
{
    private Transform _body;
    private Vector3 _target;
    private float _speed;
    private float _speedCoefficient = 1;

    public MoveSistem(Transform body, float speed)
    {
        _body = body;
        _target = body.position;
        _speed = speed;
    }

    public IEnumerator Move()
    {
        while(_target != _body.position)
        {
            _body.position = Vector3.MoveTowards(_target, _body.position, _speed * Time.deltaTime * _speedCoefficient);
            yield return null;
        }
    }

    public void SetNewTarget(Vector3 newTarget)
    {
        _target = newTarget;
    }
}
