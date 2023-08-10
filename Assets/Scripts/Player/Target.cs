using UnityEngine;

public struct Target
{
    private Vector3 _point;
    private IFightable _target;

    public Target(Vector3 point, IFightable target = null)
    {
        _point = point;
        _target = target;
    }

    public bool IsFightable => _target != null;

    public bool IstEmpty => _point != Vector3.zero || _target != null;

    public Vector3 CurrentPosition()
    {
        if (_target == null)
            return _point;
        else
            return _target.CurrentPosition;
    }

    public bool TryGetFightable(out IFightable target)
    {
        target = _target;
        return target != null;
    }

    public bool IsIFightableMatches(IFightable checker)
    {
        return _target == checker;
    }
}
