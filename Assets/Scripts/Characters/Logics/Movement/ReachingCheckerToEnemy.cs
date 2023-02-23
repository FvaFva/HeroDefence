using UnityEngine.AI;
using UnityEngine;

namespace MovementSystem
{
    public class ReachingCheckerToEnemy : BaseReachingChecker
    {
        public ReachingCheckerToEnemy(float distance, NavMeshAgent moveAgent)
        {
            _distance = distance;
            _moveAgent = moveAgent;
        }

        public void SetDistance(float distance)
        {
            _distance = Mathf.Max(0, distance);
        }

        public override void SetTarget(Target target, Vector3 currentPosition)
        {
            target.TryGetFightebel(out _target);
            SetNewTargetPointToAgent(currentPosition, _target.CurrentPosition);
        }
    }
}
