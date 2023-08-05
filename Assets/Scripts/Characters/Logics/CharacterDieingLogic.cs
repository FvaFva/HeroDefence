using System;
using System.Collections;
using UnityEngine;

public class CharacterDieingLogic : IReachLogic
{
    private Transform _body;
    private float _speed;

    public event Action<Target> Reached;

    public CharacterDieingLogic(Transform body, float speed)
    {
        _body = body;
        _speed = speed;
    }

    public IEnumerator ReachTarget()
    {
        yield return GameSettings.Character.OptimizationDelay;
        float currentFlight = GameSettings.Character.FlightDeathHight;

        while(currentFlight > 0)
        {
            float distanceFlight = _speed * GameSettings.Character.SecondsDelay;
            currentFlight -= distanceFlight;
            Vector3 oldPosition = _body.position;
            oldPosition.y += distanceFlight;
            _body.position = oldPosition;
            yield return GameSettings.Character.OptimizationDelay;
        }

        Reached?.Invoke(new Target());
        _body.gameObject.SetActive(false);
    }

    public void SetTarget(Target target)
    {
    }
}
