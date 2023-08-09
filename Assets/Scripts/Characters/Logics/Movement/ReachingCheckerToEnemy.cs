using UnityEngine;
using UnityEngine.AI;

namespace MovementSystem
{
    public class ReachingCheckerToEnemy : BaseReachingChecker
    {
        public ReachingCheckerToEnemy(float distance, NavMeshAgent moveAgent)
            : base(distance, moveAgent) { }

        public void SetDistance(float distance)
        {
            ChangeDistance(distance);
        }

        public override void SetTarget(Target target, Vector3 currentPosition)
        {
            TryGetFightable(target);
            SetNewTargetPointToAgent(currentPosition, Target.CurrentPosition);
        }
    }
}
