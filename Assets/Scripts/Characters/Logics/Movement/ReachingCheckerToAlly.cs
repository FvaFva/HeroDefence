using UnityEngine.AI;
using UnityEngine;

namespace MovementSystem
{
    public class ReachingCheckerToAlly : BaseReachingChecker
    {
        public ReachingCheckerToAlly(NavMeshAgent moveAgent)
        {
            _moveAgent = moveAgent;
            _distance = GameSettings.Character.SocialDistance;
        }

        public override void SetTarget(Target target, Vector3 currentPosition)
        {
            target.TryGetFightebel(out _target);
            SetNewTargetPointToAgent(currentPosition, target.CurrentPosition());
        }
    }
}
