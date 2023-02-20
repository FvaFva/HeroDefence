using UnityEngine;

public struct Target 
{
    private Vector3 _point;
    private IFightebel _target;

    public bool IsFightebel => _target != null;
    public bool IstEmpte => _point != Vector3.zero || _target != null;

    public Target(Vector3 point, IFightebel target = null)
    {
        _point = point;
        _target = target;
    }

    public Vector3 CurrentPosition()
    {
        if (_target == null)
            return _point;
        else
            return _target.CurrentPosition;
    }

    public bool TryGetFightebel(out IFightebel target)
    {
        target = _target;
        return target != null;
    } 
    
    public bool IsIFightebelMatches(IFightebel checker)
    {
        return _target == checker;
    }
}
